Imports System.Data.SqlClient

Public Class T_JEYS_query
    Public cha As String = ""


    Private Sub DateTimePicker1_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles DateTimePicker1.LostFocus
        Dim sqlcon As New SqlConnection
        Dim sqlcmd As New SqlCommand
        Dim sqlad As New SqlDataAdapter

        'sql = "select a.*,b.project_no,b.project_name,b.project_money,b.SK_money,b.effective_date,b.memo " & _
        '          "from t_skys_detail a left join  " & _
        '          "	project_master b on a.project_no=b.project_no " & _
        '          "  where a.billno='" & Me.ExDataGridView1.Rows(index).Cells("billno").Value & "' order by a.id "

        Dim dt As New DataTable
        Dim sql As String

        Dim i As Int32

        sql = "select distinct b.project_no  from t_skys_master a left join t_skys_detail b on a.billno=b.billno where  a.input_date>='" & Format(Me.DateTimePicker1.Value, "yyyy-MM-dd") & " 00:00:00' and a.input_date<='" & Format(Me.DateTimePicker2.Value, "yyyy-MM-dd") & " 23:59:59' " & _
        " and b.project_no in (select project_no from project_master a where a.factory_code in (select c.factory_code " & _
                "                       from sys_user a inner join  " & _
                "                       	t_user_factory b on a.login_id=b.login_id inner join  " & _
                "                       	t_factory c on b.factory_code=c.factory_code " & _
                "                       where a.stop_login=1 and c.stop=1 and a.login_id='" & login_id & "' " & _
                "                       ))"

        sqlcon = getconn()
        sqlcmd.CommandText = sql
        sqlcmd.Connection = sqlcon
        sqlad.SelectCommand = sqlcmd

        sqlad.Fill(dt)

        Me.ComboBox1.Items.Clear()

        For i = 0 To dt.Rows.Count - 1
            Me.ComboBox1.Items.Add(dt.Rows(i)("project_no"))
        Next

    End Sub




    Private Sub DateTimePicker2_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles DateTimePicker2.LostFocus
        Dim sqlcon As New SqlConnection
        Dim sqlcmd As New SqlCommand
        Dim sqlad As New SqlDataAdapter

        'sql = "select a.*,b.project_no,b.project_name,b.project_money,b.SK_money,b.effective_date,b.memo " & _
        '          "from t_skys_detail a left join  " & _
        '          "	project_master b on a.project_no=b.project_no " & _
        '          "  where a.billno='" & Me.ExDataGridView1.Rows(index).Cells("billno").Value & "' order by a.id "

        Dim dt As New DataTable
        Dim sql As String

        Dim i As Int32

        sql = "select distinct b.project_no  from t_skys_master a left join t_skys_detail b on a.billno=b.billno where  a.input_date>='" & Format(Me.DateTimePicker1.Value, "yyyy-MM-dd") & " 00:00:00' and a.input_date<='" & Format(Me.DateTimePicker2.Value, "yyyy-MM-dd") & " 23:59:59'" & _
        " and b.project_no in (select project_no from project_master a where a.factory_code in (select c.factory_code " & _
                "                       from sys_user a inner join  " & _
                "                       	t_user_factory b on a.login_id=b.login_id inner join  " & _
                "                       	t_factory c on b.factory_code=c.factory_code " & _
                "                       where a.stop_login=1 and c.stop=1 and a.login_id='" & login_id & "' " & _
                "                       ))"

        sqlcon = getconn()
        sqlcmd.CommandText = sql
        sqlcmd.Connection = sqlcon
        sqlad.SelectCommand = sqlcmd

        sqlad.Fill(dt)

        Me.ComboBox1.Items.Clear()

        For i = 0 To dt.Rows.Count - 1
            Me.ComboBox1.Items.Add(dt.Rows(i)("project_no"))
        Next

    End Sub

    Private Sub ToolStripButton_refresh_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripButton_refresh.Click
        Me.cha = ""
        Refresh_ExDataGridView_master()
    End Sub




    Private Sub Refresh_ExDataGridView_master(Optional ByVal s As String = "")
        Dim sqlcon As New SqlConnection
        Dim sqlcmd As New SqlCommand
        Dim sqlad As New SqlDataAdapter

        Dim dt As New DataTable
        Dim sql As String
        Dim sql1 As String

        sqlcon = getconn()
        sqlcmd.Connection = sqlcon

        'sql = "select * from  t_product_FY_detail where fy_id='" & Me.ExDataGridView1.Rows(index).Cells("id").Value & "' order by id"

        sql = "select a.*,b.project_name,b.project_money,b.SK_money,b.effective_date,b.memo " & _
                "from t_skys_detail a left join  " & _
                "	project_master b on a.project_no=b.project_no left join " & _
                "   t_skys_master c on a.billno=c.billno " & _
                    "  where a.project_no like '%" & Me.ComboBox1.Text.Trim & "%' and c.input_date>='" & Format(Me.DateTimePicker1.Value, "yyyy-MM-dd") & " 00:00:00' and c.input_date<='" & Format(Me.DateTimePicker2.Value, "yyyy-MM-dd") & " 23:59:59' " & _
                    " and b.project_no in (select project_no from project_master a where a.factory_code in (select c.factory_code " & _
                "                       from sys_user a inner join  " & _
                "                       	t_user_factory b on a.login_id=b.login_id inner join  " & _
                "                       	t_factory c on b.factory_code=c.factory_code " & _
                "                       where a.stop_login=1 and c.stop=1 and a.login_id='" & login_id & "' " & _
                "                       ))"



        sql1 = "select isnull(sum(a.money),0) as summoney from (" & sql & ") a"

        sqlcmd.CommandText = sql1
        Me.Label4.Text = "总金额：" & sqlcmd.ExecuteScalar


        sqlcmd.CommandText = sql
        sqlad.SelectCommand = sqlcmd
        dt.Rows.Clear()
        sqlad.Fill(dt)

        Me.ExDataGridView1.SetDataSource(dt)
        Me.ExDataGridView1.Columns.Clear()




        Me.ExDataGridView1.AddColumn("project_no", "工程号", 120)
        Me.ExDataGridView1.AddColumn("project_name", "工程名称")
        Me.ExDataGridView1.AddColumn("project_money", "工程金额", , , , , "#0")
        Me.ExDataGridView1.AddColumn("money", "收款金额", , , , , "#0")


        Me.ExDataGridView1.AddColumn("effective_date", "有效日期", , , , , "yyyy-MM-dd")


        Me.ExDataGridView1.AddColumn("memo", "备注")

        Me.ExDataGridView1.AddColumn("id", "id", , , , , , False)


    End Sub

    Private Sub ToolStripButton_exit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripButton_exit.Click
        Me.Close()
    End Sub

    Private Sub ToolStripButton_excel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripButton_excel.Click
        Dim ToExcel As New BaseClass.TOEXCEL

        If ToExcel.ExToExcel(Me.ExDataGridView1, Me.SaveFileDialog1, True, "产值总计") = True Then
            MsgBox("导出成功！", MsgBoxStyle.OkOnly, "提示")
        End If
    End Sub

    Private Sub DateTimePicker2_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles DateTimePicker2.ValueChanged

    End Sub

    Private Sub DateTimePicker1_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles DateTimePicker1.ValueChanged

    End Sub

    Private Sub ComboBox1_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ComboBox1.SelectedIndexChanged

    End Sub
End Class