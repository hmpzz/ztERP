Imports System.Data.SqlClient
Public Class YKD
    Friend dt_P As New DataTable

    Private Sub YKD_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Refresh_Form()
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

    Private Sub 增加ToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles 增加ToolStripMenuItem.Click
        Dim warehouser_amount As New warehouse_amount

        Dim sqlcon As New SqlConnection
        Dim sqlcmd As New SqlCommand
        Dim sqlad As New SqlDataAdapter

        Dim dt As New DataTable
        Dim sql As String
        Dim i As Int32
        Dim s As String = ""

        If Me.ComboBox1.SelectedIndex < 0 Then
            MsgBox("请选择出库仓库！", MsgBoxStyle.OkOnly, "提示")
            Exit Sub
        End If

        If Me.ComboBox3.SelectedIndex < 0 Then
            MsgBox("请选择入库仓库！", MsgBoxStyle.OkOnly, "提示")
            Exit Sub
        End If



        warehouse_amount.depot_code = Me.ComboBox2.Text.Trim

        For i = 0 To Me.ExDataGridView1.Rows.Count - 1
            s = s & Me.ExDataGridView1.Rows(i).Cells("id").Value & ","
        Next
        If s.Length > 0 Then
            s = s.Substring(0, s.Length - 1)
        End If

        warehouse_amount.id = s

        warehouse_amount.Owner = Me


        warehouse_amount.ShowDialog()
        If warehouse_amount.DialogResult = Windows.Forms.DialogResult.Cancel Then
            warehouse_amount.Dispose()
            Exit Sub
        End If


        If Me.Tag = "" Then
            Exit Sub
        End If


        sqlcon = getconn()
        sqlcmd.Connection = sqlcon

        sql = "select a.amount,b.item_code,b.item_name,b.item_standard,b.unit ,0.0 as yk_amount,b.id  " & _
                "from t_part a left join " & _
                "	t_item b on a.item_code=b.item_code " & _
                " where b.id in(" & Me.Tag & ") and a.depot_code='" & Me.ComboBox2.Text.Trim & "'"
        sqlcmd.CommandText = sql

        sqlad.SelectCommand = sqlcmd
        sqlad.Fill(dt)

        If Me.ExDataGridView1.Columns.Count = 0 Then
            dt_P = dt

            Me.ExDataGridView1.SetDataSource(dt_P)
            Me.ExDataGridView1.Columns.Clear()

            Me.ExDataGridView1.AddColumn("item_code", "物料编码")
            Me.ExDataGridView1.AddColumn("item_name", "物料名称")

            Me.ExDataGridView1.AddColumn("item_standard", "规格")
            Me.ExDataGridView1.AddColumn("unit", "单位")

            Me.ExDataGridView1.AddColumn("yk_amount", "移库数量", , False, , , "#0.000")
            Me.ExDataGridView1.AddColumn("amount", "库存数量", , , , , "#0.000")


            Me.ExDataGridView1.AddColumn("id", "id", , , , , , False)
        Else
            For i = 0 To dt.Rows.Count - 1

                Dim DR As DataRow = Me.dt_P.NewRow

                DR("item_code") = dt.Rows(i)("item_code")

                DR("item_name") = dt.Rows(i)("item_name")
                DR("item_standard") = dt.Rows(i)("item_standard")
                DR("unit") = dt.Rows(i)("unit")
                DR("amount") = dt.Rows(i)("amount")
                DR("yk_amount") = dt.Rows(i)("yk_amount")


                DR("id") = dt.Rows(i)("id")

                dt_P.Rows.Add(DR)


            Next
        End If


        warehouse_amount.Dispose()



        If Me.ExDataGridView1.Rows.Count > 0 Then
            Me.ComboBox1.Enabled = False
            Me.ComboBox3.Enabled = False
        Else
            Me.ComboBox1.Enabled = True
            Me.ComboBox3.Enabled = True
        End If
    End Sub

    Private Sub ToolStripButton_refresh_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripButton_refresh.Click
        Refresh_Form()
    End Sub

    Private Sub Refresh_Form()
        Dim sqlcon As New SqlConnection
        Dim sqlcmd As New SqlCommand
        Dim sqlad As New SqlDataAdapter


        Dim dt As New DataTable
        Dim sql As String
        Dim i As Integer



        Me.ComboBox1.Items.Clear()
        Me.ComboBox2.Items.Clear()

        Me.ComboBox3.Items.Clear()
        Me.ComboBox4.Items.Clear()

        Me.TextBox2.Text = ""

        sqlcon = getconn()
        sqlcmd.Connection = sqlcon

        sql = "select a.* from t_depot a  where a.stop=1   order by a.depot_code"
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

        Me.TextBox2.Focus()

        If Me.ExDataGridView1.Rows.Count = 0 Then
            MsgBox("没有需要保存的内容！", MsgBoxStyle.OkOnly, "提示")
            Exit Sub
        End If

        For i = 0 To Me.ExDataGridView1.Rows.Count - 1
            If Me.ExDataGridView1.Rows(i).Cells("yk_amount").Value = 0 Then
                MsgBox("要保存的行中包括数量为0的行，请确认后保存！", MsgBoxStyle.OkOnly, "提示")
                Exit Sub
            End If
        Next

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


            sql = "INSERT INTO B_TransferBill_master ( aim_depot,billno,input_date,input_man,source_depot,state,memo) values " & _
            "( '" & RK_depot_code & "', '" & billno & "', getdate(), '" & rs_name & "', '" & CK_depot_code & "', '0','" & Me.TextBox2.Text.Trim & "')"
            sqlcmd.CommandText = sql
            sqlcmd.ExecuteNonQuery()

            For i = 0 To Me.ExDataGridView1.Rows.Count - 1

                sql = "INSERT INTO B_TransferBill_detail ( Amount,BillNo,CK_amount,item_code,RK_amount) values " & _
                    "( '" & Me.ExDataGridView1.Rows(i).Cells("yk_amount").Value & "', " & _
                    "'" & billno & "', '0', '" & Me.ExDataGridView1.Rows(i).Cells("item_code").Value & "', '0')"

                sqlcmd.CommandText = sql
                sqlcmd.ExecuteNonQuery()



            Next

            mytrans.Commit()
        Catch ex As Exception
            mytrans.Rollback()
            MsgBox(ex.Message, MsgBoxStyle.OkOnly, "提示")
            Exit Sub
        End Try

        Me.dt_P.Rows.Clear()

        MsgBox("保存成功！", MsgBoxStyle.OkOnly, "提示")
        Refresh_Form()
    End Sub

    Private Sub ToolStripButton_exit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripButton_exit.Click
        Me.Close()
    End Sub
End Class