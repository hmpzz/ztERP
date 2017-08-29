Imports System.Data.SqlClient
Public Class LL_YKD

    Private Sub ToolStripButton_refresh_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripButton_refresh.Click
        Refresh_JM()
        Refresh_TreeView()
    End Sub

    Private Sub ToolStripButton_exit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripButton_exit.Click
        Me.Close()
    End Sub



    Private Sub Refresh_JM()
        Dim sqlcon As New SqlConnection
        Dim sqlcmd As New SqlCommand
        Dim sqlad As New SqlDataAdapter


        Dim dt As New DataTable
        Dim sql As String
        Dim i As Integer

        Me.TreeView1.Nodes.Clear()
        Me.TreeView1.Nodes.Add("all", "出库单", 0, 1)

        Me.ComboBox1.Items.Clear()

        sqlcon = getconn()
        sqlcmd.Connection = sqlcon

        sql = "select a.* from t_depot a inner join t_user_depot b on a.depot_code=b.depot_code where a.stop=1 and b.login_id='" & login_id & "' order by a.depot_code"
        sqlcmd.CommandText = sql
        sqlad.SelectCommand = sqlcmd

        dt.Clear()
        sqlad.Fill(dt)

        For i = 0 To dt.Rows.Count - 1
            Me.ComboBox1.Items.Add(dt.Rows(i)("depot_name"))
            Me.ComboBox2.Items.Add(dt.Rows(i)("depot_code"))
        Next

        Me.ExDataGridView1.Columns.Clear()
     
        Me.ExDataGridView1.ClearDSDM()

    End Sub


    Private Sub Refresh_TreeView()
        Dim sqlcon As New SqlConnection
        Dim sqlcmd As New SqlCommand
        Dim sqlad As New SqlDataAdapter


        Dim dt As New DataTable
        Dim sql As String
        Dim i As Integer

        Dim ny As String = "'"

        sqlcon = getconn()

        sqlcmd.Connection = sqlcon

        sql = "SELECT * FROM T_part_out_master a left join " & _
        " B_TransferBill_master b on a.billno=b.ll_billno " & _
        " where     a.state=1 and a.billno not in (SELECT billno FROM t_part_out_detail where ok_amount<>0) and b.billno is null  order by a.project_no "


        sqlcmd.CommandText = sql
        sqlad.SelectCommand = sqlcmd

        dt.Clear()
        sqlad.Fill(dt)

        Me.TreeView1.Nodes.Clear()


        Dim Tnode As TreeNode
        Dim Dnode As TreeNode = Nothing
        Dim Fnode As TreeNode = Nothing

        Me.TreeView1.Nodes.Clear()
        Tnode = Me.TreeView1.Nodes.Add("all", "出库单", 0, 1)

        For i = 0 To dt.Rows.Count - 1
            If ny <> dt.Rows(i)("project_no") Then
                Dnode = Tnode.Nodes.Add("P" & dt.Rows(i)("project_no"), dt.Rows(i)("project_no"), 0, 1)
            End If

            Fnode = Dnode.Nodes.Add("F" & dt.Rows(i)("billno"), dt.Rows(i)("billno"), 2, 2)

            ny = dt.Rows(i)("project_no")
        Next
        Me.TreeView1.Nodes(0).Expand()


        Me.ExDataGridView1.Columns.Clear()
        'Me.ExDataGridView1.DataSource = Nothing
        'Me.ExDataGridView1.DataMember = Nothing
        Me.ExDataGridView1.ClearDSDM()
    End Sub


    Private Sub TreeView1_NodeMouseClick(ByVal sender As Object, ByVal e As System.Windows.Forms.TreeNodeMouseClickEventArgs) Handles TreeView1.NodeMouseClick
        If e.Node.Name.Substring(0, 1).ToUpper <> "F" Then
            Me.ExDataGridView1.Columns.Clear()
            'Me.ExDataGridView1.DataSource = Nothing
            'Me.ExDataGridView1.DataMember = Nothing
            Me.ExDataGridView1.ClearDSDM()
            Exit Sub

        End If

        Refresh_ExDataGridView(Microsoft.VisualBasic.Right(e.Node.Name.Trim, e.Node.Name.Trim.Length - 1))
    End Sub





    Private Sub Refresh_ExDataGridView(ByVal s As String)

        Dim sqlcon As New SqlConnection
        Dim sqlcmd As New SqlCommand
        Dim sqlad As New SqlDataAdapter

        Dim dt As New DataTable
        Dim sql As String


        sqlcon = getconn()



        sql = " SELECT a.*, b.item_name, b.item_standard,b.unit,c.input_man,c.input_date,c.ll_man " & _
"                        FROM t_part_out_detail a left join   " & _
"                        	t_item b on a.item_code=b.item_code  left join  " & _
"                           t_part_out_master c on a.billno=c.billno left join " & _
"                           t_part d on c.depot_code=d.depot_code and a.item_code=d.item_code " & _
"                        where a.billno='" & s & "' and a.amount>a.ok_amount order by a.id"


        sqlcmd.Connection = sqlcon
        sqlcmd.CommandText = sql
        sqlad.SelectCommand = sqlcmd
        dt.Rows.Clear()
        sqlad.Fill(dt)

        Me.ExDataGridView1.SetDataSource(dt)
        Me.ExDataGridView1.Columns.Clear()


        Me.ExDataGridView1.AddColumn("item_code", "物料编码")
        Me.ExDataGridView1.AddColumn("item_name", "物料名称")
        Me.ExDataGridView1.AddColumn("item_standard", "规格")
        Me.ExDataGridView1.AddColumn("unit", "单位")


        Me.ExDataGridView1.AddColumn("amount", "数量", , , , DataGridViewContentAlignment.MiddleRight, "#0.0000")
        
        Me.ExDataGridView1.AddColumn("input_man", "制单人")
        Me.ExDataGridView1.AddColumn("input_date", "制单日期")

        Me.ExDataGridView1.AddColumn("item_code", "item_code", , , , , , False)

        Me.ExDataGridView1.AddColumn("id", "id", , , , , , False)
    End Sub

    Private Sub ToolStripButton_save_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripButton_save.Click
        Dim i As Int32


        Dim sqlcon As New SqlConnection
        Dim sqlcmd As New SqlCommand
        Dim sqlad As New SqlDataAdapter


        Dim mytrans As SqlTransaction

        Dim dt As New DataTable
        Dim sql As String = ""

        Dim CK_depot_code As String
        Dim RK_depot_code As String

        Dim billno As String = ""
        Dim accountterm As String = ""

        Me.TextBox1.Focus()

        If Me.ExDataGridView1.Rows.Count = 0 Then
            MsgBox("没有需要保存的内容！", MsgBoxStyle.OkOnly, "提示")
            Exit Sub
        End If

        If Me.ComboBox2.Text.Trim.Length = 0 Then
            MsgBox("出库仓库不能为空,请重新选择！", MsgBoxStyle.OkOnly, "提示")
            Exit Sub
        End If

        If Me.ComboBox4.Text.Trim.Length = 0 Then
            MsgBox("入库仓库不能为空,请重新选择！", MsgBoxStyle.OkOnly, "提示")
            Exit Sub
        End If

        CK_depot_code = Me.ComboBox2.Text
        RK_depot_code = Me.ComboBox4.Text


        If Get_depot_state(CK_depot_code) = False Then
            MsgBox("出库仓库为停用状态，不可以入库！", MsgBoxStyle.OkOnly, "提示")
            Exit Sub
        End If

        If Get_depot_state(RK_depot_code) = False Then
            MsgBox("入库仓库为停用状态，不可以入库！", MsgBoxStyle.OkOnly, "提示")
            Exit Sub
        End If


        sqlcon = getconn()
        sqlcmd.Connection = sqlcon


        mytrans = sqlcon.BeginTransaction()
        sqlcmd.Transaction = mytrans

        accountterm = GetAccountterm()

        Try
            sql = "update t_dj_num set num=num+1 where  tt='YK'"
            sqlcmd.CommandText = sql
            sqlcmd.ExecuteNonQuery()

            sql = "select * from t_dj_num where tt='YK'"
            sqlcmd.CommandText = sql
            sqlad.SelectCommand = sqlcmd
            dt.Rows.Clear()
            sqlad.Fill(dt)

            billno = dt.Rows(0)("TT") & "-" & GetAccountterm() & "-" & Format(dt.Rows(0)("num"), dt.Rows(0)("format"))


            sql = "INSERT INTO B_TransferBill_master ( aim_depot,billno,input_date,input_man,source_depot,state,memo,ll_billno,source) values " & _
            "( '" & RK_depot_code & "', '" & billno & "', getdate(), '" & rs_name & "', '" & CK_depot_code & "', '1','','" & Me.TreeView1.SelectedNode.Name.Replace("F", "") & "',1)"
            sqlcmd.CommandText = sql
            sqlcmd.ExecuteNonQuery()

            For i = 0 To Me.ExDataGridView1.Rows.Count - 1

                sql = "INSERT INTO B_TransferBill_detail ( Amount,BillNo,CK_amount,item_code,RK_amount,ll_detail_id) values " & _
                    "( '" & Me.ExDataGridView1.Rows(i).Cells("amount").Value & "', " & _
                    "'" & billno & "', '0', '" & Me.ExDataGridView1.Rows(i).Cells("item_code").Value & "', '0'," & Me.ExDataGridView1.Rows(i).Cells("id").Value & ")"

                sqlcmd.CommandText = sql
                sqlcmd.ExecuteNonQuery()



            Next

            mytrans.Commit()
        Catch ex As Exception
            mytrans.Rollback()
            MsgBox(ex.Message, MsgBoxStyle.OkOnly, "提示")
            Exit Sub
        End Try



        MsgBox("保存成功！", MsgBoxStyle.OkOnly, "提示")

        Refresh_JM()
        Refresh_TreeView()
   
    End Sub

    Private Sub LL_YKD_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Refresh_JM()
        Refresh_TreeView()
    End Sub

    Private Sub ComboBox1_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ComboBox1.SelectedIndexChanged
        Dim sqlcon As New SqlConnection
        Dim sqlcmd As New SqlCommand
        Dim sqlad As New SqlDataAdapter


        Dim dt As New DataTable
        Dim sql As String
        Dim i As Integer


        Me.ComboBox2.SelectedIndex = Me.ComboBox1.SelectedIndex


        Me.ComboBox3.Items.Clear()
        Me.ComboBox4.Items.Clear()

        sqlcon = getconn()
        sqlcmd.Connection = sqlcon

        sql = "select a.* from t_depot a  where a.depot_code<> '" & Me.ComboBox2.Text.Trim & "' and  a.stop=1  order by a.depot_code"
        sqlcmd.CommandText = sql
        sqlad.SelectCommand = sqlcmd

        dt.Clear()
        sqlad.Fill(dt)

        For i = 0 To dt.Rows.Count - 1
            Me.ComboBox3.Items.Add(dt.Rows(i)("depot_name"))
            Me.ComboBox4.Items.Add(dt.Rows(i)("depot_code"))
        Next


    End Sub

    Private Sub ComboBox3_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ComboBox3.SelectedIndexChanged
        Me.ComboBox4.SelectedIndex = Me.ComboBox3.SelectedIndex

    End Sub
End Class