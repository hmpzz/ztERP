Imports System.Data.SqlClient
Public Class part_out_LL

    Private Sub ToolStripButton_refresh_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripButton_refresh.Click
        Refresh_JM()
        'Refresh_TreeView()
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
        'Me.ExDataGridView1.DataSource = Nothing
        'Me.ExDataGridView1.DataMember = Nothing
        Me.ExDataGridView1.ClearDSDM()

    End Sub

    Private Sub Refresh_TreeView(ByVal s As String)
        Dim sqlcon As New SqlConnection
        Dim sqlcmd As New SqlCommand
        Dim sqlad As New SqlDataAdapter


        Dim dt As New DataTable
        Dim sql As String
        Dim i As Integer

        Dim ny As String = "'"

        sqlcon = getconn()

        sqlcmd.Connection = sqlcon

        sql = "SELECT * FROM T_part_out_master where     state=1 and billno in (SELECT billno FROM t_part_out_detail where ok_amount<amount) and depot_code='" & s & "' order by project_no "

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

    Private Sub TreeView1_AfterSelect(ByVal sender As System.Object, ByVal e As System.Windows.Forms.TreeViewEventArgs) Handles TreeView1.AfterSelect

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



        sql = " SELECT a.*, b.item_name, b.item_standard,b.unit,a.amount-a.ok_amount as wck_amount,a.amount-a.ok_amount as cksl,d.amount as kcsl,c.input_man,c.input_date,c.ll_man,isnull(f.RK_amount,0) as rk_amount,c.memo " & _
"                        FROM t_part_out_detail a left join   " & _
"                        	t_item b on a.item_code=b.item_code  left join  " & _
"                           t_part_out_master c on a.billno=c.billno left join " & _
"                           t_part d on c.depot_code=d.depot_code and a.item_code=d.item_code left join  " & _
"                           B_TransferBill_master e on c.depot_code=e.aim_depot and e.ll_billno=c.billno  left join " & _
"                           B_TransferBill_detail f on a.id=f.ll_detail_id and e.billno=f.billno " & _
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
        Me.ExDataGridView1.AddColumn("OK_amount", "已出库数量", , , , DataGridViewContentAlignment.MiddleRight, "#0.0000")
        Me.ExDataGridView1.AddColumn("wck_amount", "未出库数量", , , , DataGridViewContentAlignment.MiddleRight, "#0.0000")
        Me.ExDataGridView1.AddColumn("cksl", "本次出库数量", , False, , DataGridViewContentAlignment.MiddleRight, "#0.0000")
        Me.ExDataGridView1.AddColumn("rk_amount", "移库入库数量", , , , DataGridViewContentAlignment.MiddleRight, "#0.0000")
        Me.ExDataGridView1.AddColumn("ll_man", "领料人")
        Me.ExDataGridView1.AddColumn("kcsl", "库存数量", , , , DataGridViewContentAlignment.MiddleRight, "#0.0000")
        Me.ExDataGridView1.AddColumn("input_man", "制单人")
        Me.ExDataGridView1.AddColumn("input_date", "制单日期")
        Me.ExDataGridView1.AddColumn("memo", "说明")

        Me.ExDataGridView1.AddColumn("item_code", "item_code", , , , , , False)
    
        Me.ExDataGridView1.AddColumn("id", "id", , , , , , False)
    End Sub

    Private Sub ToolStripButton_save_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripButton_save.Click
        Dim i As Int32
        Dim b As Boolean = False

        Dim sqlcon As New SqlConnection
        Dim sqlcmd As New SqlCommand
        Dim sqlad As New SqlDataAdapter


        Dim mytrans As SqlTransaction

        Dim dt As New DataTable
        Dim ds As New DataSet
        Dim sql As String = ""

        Dim accountterm As String = ""
        Dim depot_code As String = ""
        Dim billno As String = ""

        Dim q_amount As Decimal = 0
        Dim h_amount As Decimal = 0

        Dim B_price As Decimal
        'Dim B_money As Decimal

        Me.TextBox1.Focus()

        If Month_end(Format(Me.DateTimePicker1.Value, "yyyy-MM-dd")) = False Then
            MsgBox("您所选择的出库日期不在本帐期内！", MsgBoxStyle.OkOnly, "提示")
            Exit Sub
        End If

        If Me.ExDataGridView1.Rows.Count = 0 Then
            MsgBox("请选择需要出库的数据！", MsgBoxStyle.OkOnly, "提示")
            Exit Sub
        End If

        For i = 0 To Me.ExDataGridView1.Rows.Count - 1
            If Me.ExDataGridView1.Rows(i).Cells("cksl").Value <> 0 Then
                b = True
                Exit For
            End If
        Next

        If b = False Then
            MsgBox("请输入要出库的数量！", MsgBoxStyle.OkOnly, "提示")
            Exit Sub
        End If




        accountterm = GetAccountterm()
        depot_code = Get_depot_code(Me.ComboBox1.Text.Trim)

        If depot_code = "" Then
            MsgBox("仓库选择不正确，刷新后再试！", MsgBoxStyle.OkOnly, "提示")
            Exit Sub
        End If

        sqlcon = getconn()
        sqlcmd.Connection = sqlcon


        sql = "select stop from t_depot where depot_code='" & depot_code & "'"
        sqlcmd.CommandText = sql
        If sqlcmd.ExecuteScalar = 0 Then
            MsgBox("仓库为停用状态，不可以出库！", MsgBoxStyle.OkOnly, "提示")
            Exit Sub
        End If

        sql = "select state from T_part_out_master where billno='" & Me.TreeView1.SelectedNode.Text.Trim & "'  "
        sqlcmd.CommandText = sql
        i = sqlcmd.ExecuteScalar
        If i = 0 Then
            MsgBox("此单据没有审核，请查证后再出库！", MsgBoxStyle.OkOnly, "提示")
            Exit Sub
        ElseIf i = 2 Then
            MsgBox("此单据已经结案，请查证后再出库！", MsgBoxStyle.OkOnly, "提示")
            Exit Sub
        End If

        Me.ToolStripButton_save.Enabled = False

        mytrans = sqlcon.BeginTransaction()
        sqlcmd.Transaction = mytrans

        Try

            Me.TreeView1.Enabled = False


            sql = "update t_dj_num set num=num+1 where  tt='CK-" & depot_code & "'"
            sqlcmd.CommandText = sql
            sqlcmd.ExecuteNonQuery()

            sql = "select * from t_dj_num where tt='CK-" & depot_code & "'"
            sqlcmd.CommandText = sql
            sqlad.SelectCommand = sqlcmd
            dt.Rows.Clear()
            sqlad.Fill(dt)

            billno = dt.Rows(0)("TT") & "-" & GetAccountterm() & "-" & Format(dt.Rows(0)("num"), dt.Rows(0)("format"))


            sql = "INSERT INTO t_stockbill_master ( AccountTerm,BillDate,BillKindCode,BillNo,Depot_Code,input_man,TempFlag,Type,ResourceNo) values " & _
            "( '" & accountterm & "', '" & Format(Me.DateTimePicker1.Value, "yyyy-MM-dd") & "', '12', '" & billno & "', '" & depot_code & "', '" & rs_name & "', '0', '1', '" & Me.TreeView1.SelectedNode.Text.Trim & "')"
            sqlcmd.CommandText = sql
            sqlcmd.ExecuteNonQuery()

            For i = 0 To Me.ExDataGridView1.Rows.Count - 1
                If Me.ExDataGridView1.Rows(i).Cells("cksl").Value <> 0 Then


                    sql = "select count(*) from t_part where item_code='" & Me.ExDataGridView1.Rows(i).Cells("item_code").Value & "'   and depot_code='" & depot_code & "'"

                    sqlcmd.CommandText = sql
                    If sqlcmd.ExecuteScalar = 0 Then
                        Throw New System.Exception(Me.ExDataGridView1.Rows(i).Cells("item_code").Value & "还没有库存，不可以出库！")
                    End If

                    sql = "update t_part set amount=amount - " & Me.ExDataGridView1.Rows(i).Cells("cksl").Value & ",money=case when money - price* " & Me.ExDataGridView1.Rows(i).Cells("cksl").Value & "<0 then 0 else  money - price* " & Me.ExDataGridView1.Rows(i).Cells("cksl").Value & " end,end_date=getdate() where item_code='" & Me.ExDataGridView1.Rows(i).Cells("item_code").Value & "'  and depot_code='" & depot_code & "'"
                    sqlcmd.CommandText = sql
                    sqlcmd.ExecuteNonQuery()

                    sql = "update t_part set money=0 where money<0"
                    sqlcmd.CommandText = sql
                    sqlcmd.ExecuteNonQuery()


                    sql = " select price from  t_part  where item_code='" & Me.ExDataGridView1.Rows(i).Cells("item_code").Value & "'  and depot_code='" & depot_code & "'"
                    sqlcmd.CommandText = sql
                    B_price = sqlcmd.ExecuteScalar



                    sql = "update t_part set price=(case when money<0 then 0 else money end)/(case when amount=0 then 1 else amount end ) where item_code='" & Me.ExDataGridView1.Rows(i).Cells("item_code").Value & _
                              "' and depot_code='" & depot_code & "'"
                    sqlcmd.CommandText = sql
                    sqlcmd.ExecuteNonQuery()




                    sql = "INSERT INTO t_stockbill_detail ( Amount,BillNo,item_code,ResourceID,sourceflag,price,money) values " & _
                                                                         "( '" & Me.ExDataGridView1.Rows(i).Cells("cksl").Value & "'," & _
                                                                         "'" & billno & "', '" & Me.ExDataGridView1.Rows(i).Cells("item_code").Value & "', " & _
                                                                         "'" & Me.ExDataGridView1.Rows(i).Cells("id").Value & "', '1','" & B_price & "','" & B_price * Me.ExDataGridView1.Rows(i).Cells("cksl").Value & "')"
                    sqlcmd.CommandText = sql
                    sqlcmd.ExecuteNonQuery()


                    sql = "update T_part_out_detail set ok_amount=ok_amount + " & Me.ExDataGridView1.Rows(i).Cells("cksl").Value & "  where id=" & Me.ExDataGridView1.Rows(i).Cells("id").Value
                    sqlcmd.CommandText = sql
                    sqlcmd.ExecuteNonQuery()
                End If
            Next







            mytrans.Commit()
            Me.TreeView1.Enabled = True
        Catch ex As Exception
            Me.TreeView1.Enabled = True
            mytrans.Rollback()
            Me.ToolStripButton_save.Enabled = True
            MsgBox(ex.Message & vbCrLf & sql, MsgBoxStyle.OkOnly, "提示")
            Exit Sub
        End Try

        Refresh_ExDataGridView(Me.TreeView1.SelectedNode.Text)




        Dim s As String
        s = Print_Out(billno)
        If s = "" Then
            Me.ToolStripButton_save.Enabled = True
            MsgBox("保存成功！", MsgBoxStyle.OkOnly, "提示")
        Else
            Me.ToolStripButton_save.Enabled = True
            MsgBox(s, MsgBoxStyle.OkOnly, "提示")
        End If
    End Sub

    Private Sub part_out_LL_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Refresh_JM()
        'Refresh_TreeView()
    End Sub

    Private Sub ComboBox1_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ComboBox1.SelectedIndexChanged
        Me.ComboBox2.SelectedIndex = Me.ComboBox1.SelectedIndex
        Refresh_TreeView(Me.ComboBox2.Text.Trim)
    End Sub

    Private Sub ExDataGridView1_CellContentClick(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles ExDataGridView1.CellContentClick

    End Sub

    Private Sub ExDataGridView1_RowPrePaint(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewRowPrePaintEventArgs) Handles ExDataGridView1.RowPrePaint
        'If (e.RowIndex > Me.ExDataGridView1.Rows.Count - 1) Then Return
        Dim dgr As DataGridViewRow = Me.ExDataGridView1.Rows(e.RowIndex)
        Try
            'dgr.DefaultCellStyle.BackColor = Color.Black

            If (dgr.Cells("amount").Value > dgr.Cells("kcsl").Value) Then

                dgr.DefaultCellStyle.ForeColor = Color.Red
            End If
        Catch ex As Exception

        End Try
    End Sub
End Class