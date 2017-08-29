Imports System.Data.SqlClient
Public Class project_CZ_quality
    Public cha As String = ""
    Private Sub ToolStripButton_refresh_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripButton_refresh.Click
        Me.cha = ""

        Refresh_ExDataGridView_master()
    End Sub

    Private Sub ToolStripButton_exit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripButton_exit.Click
        Me.Close()
    End Sub


    Private Sub Refresh_ExDataGridView_master(Optional ByVal s As String = "")
        Dim sqlcon As New SqlConnection
        Dim sqlcmd As New SqlCommand
        Dim sqlad As New SqlDataAdapter
        Dim s1 As String = ""
        Dim s2 As String = ""

        Dim dt As New DataTable
        Dim sql As String
        Dim sql1 As String



        sqlcon = getconn()
        sqlcmd.Connection = sqlcon

       

        sql = "select a.project_no,b.project_name,sum(a.tz_money) as tz_money " & _
                "from project_detail a left join  " & _
                "	project_master b on a.project_no=b.project_no " & _
                "where  b.state<>0 and a.project_no like '%" & Me.ComboBox1.Text.Trim & "%' and a.tz_date>='" & Format(Me.DateTimePicker1.Value, "yyyy-MM-dd") & " 00:00:00' and a.tz_date<='" & Format(Me.DateTimePicker2.Value, "yyyy-MM-dd") & " 23:59:59' " & _
                " and b.factory_code in (select c.factory_code " & _
                "                       from sys_user a inner join  " & _
                "                       	t_user_factory b on a.login_id=b.login_id inner join  " & _
                "                       	t_factory c on b.factory_code=c.factory_code " & _
                "                       where a.stop_login=1 and c.stop=1 and a.login_id='" & login_id & "' " & _
                "                       ) " & _
                "group by a.project_no,b.project_name "


        sql1 = "select isnull(sum(a.tz_money),0) as summoney from (" & sql & ") a"


        sqlcmd.CommandText = sql1
        Me.Label4.Text = "总金额：" & sqlcmd.ExecuteScalar

        sqlcmd.CommandText = sql
        sqlad.SelectCommand = sqlcmd
        dt.Rows.Clear()
        sqlad.Fill(dt)

        Me.ExDataGridView1.SetDataSource(dt)
        Me.ExDataGridView1.Columns.Clear()

        'Me.ExDataGridView1.AddColumn("check", "", 20, False, New DataGridViewCheckBoxColumn)
        Me.ExDataGridView1.AddColumn("project_no", "工程号")
        Me.ExDataGridView1.AddColumn("project_name", "工程名称")

        Me.ExDataGridView1.AddColumn("tz_money", "产值总计", , , , , "#0.00", )


        'Me.ExDataGridView1.AddColumn("effective_date", "有效日期", , , , , "yyyy-MM-dd")


        'Me.ExDataGridView1.AddColumn("memo", "备注")
        'Me.ExDataGridView1.AddColumn("state1", "状态")

        'Me.ExDataGridView1.AddColumn("begin_date", "开工日期", , , , , "yyyy-MM-dd")
        'Me.ExDataGridView1.AddColumn("end_date", "完工日期", , , , , "yyyy-MM-dd")
        'Me.ExDataGridView1.AddColumn("end_man", "确认人")

        'Me.ExDataGridView1.AddColumn("input_man", "录入人")
        'Me.ExDataGridView1.AddColumn("input_date", "录入日期", , , , , "yyyy-MM-dd")
        'Me.ExDataGridView1.AddColumn("check_man", "审核人")
        'Me.ExDataGridView1.AddColumn("check_date", "审核日期", , , , , "yyyy-MM-dd")

        Me.ExDataGridView1.AddColumn("id", "id", , , , , , False)


    End Sub

    Private Sub ToolStripButton_excel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripButton_excel.Click
        Dim ToExcel As New BaseClass.TOEXCEL

        If ToExcel.ExToExcel(Me.ExDataGridView1, Me.SaveFileDialog1, True, "产值总计") = True Then
            MsgBox("导出成功！", MsgBoxStyle.OkOnly, "提示")
        End If
    End Sub

    Private Sub DateTimePicker2_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles DateTimePicker2.LostFocus
        Dim sqlcon As New SqlConnection
        Dim sqlcmd As New SqlCommand
        Dim sqlad As New SqlDataAdapter


        Dim dt As New DataTable
        Dim sql As String

        Dim i As Int32

        sql = "select distinct a.project_no  " & _
              "from project_detail a inner join " & _
              "     project_master b on a.project_no=b.project_no " & _
              "where  a.tz_date>='" & Format(Me.DateTimePicker1.Value, "yyyy-MM-dd") & " 00:00:00' and a.tz_date<='" & Format(Me.DateTimePicker2.Value, "yyyy-MM-dd") & " 23:59:59' " & _
              "and b.factory_code in (select c.factory_code " & _
              "                       from sys_user a inner join  " & _
              "                       	t_user_factory b on a.login_id=b.login_id inner join  " & _
              "                       	t_factory c on b.factory_code=c.factory_code " & _
              "                       where a.stop_login=1 and c.stop=1 and a.login_id='" & login_id & "' " & _
              "                       )"

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

    Private Sub DateTimePicker2_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles DateTimePicker2.ValueChanged


    End Sub

    Private Sub DateTimePicker1_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles DateTimePicker1.LostFocus
        Dim sqlcon As New SqlConnection
        Dim sqlcmd As New SqlCommand
        Dim sqlad As New SqlDataAdapter


        Dim dt As New DataTable
        Dim sql As String

        Dim i As Int32

        sql = "select distinct a.project_no  " & _
                "from project_detail a inner join " & _
                "     project_master b on a.project_no=b.project_no " & _
                "where  a.tz_date>='" & Format(Me.DateTimePicker1.Value, "yyyy-MM-dd") & " 00:00:00' and a.tz_date<='" & Format(Me.DateTimePicker2.Value, "yyyy-MM-dd") & " 23:59:59' " & _
                "and b.factory_code in (select c.factory_code " & _
                "                       from sys_user a inner join  " & _
                "                       	t_user_factory b on a.login_id=b.login_id inner join  " & _
                "                       	t_factory c on b.factory_code=c.factory_code " & _
                "                       where a.stop_login=1 and c.stop=1 and a.login_id='" & login_id & "' " & _
                "                       )"

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

    Private Sub DateTimePicker1_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles DateTimePicker1.ValueChanged
      
    End Sub

    Private Sub ComboBox1_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ComboBox1.SelectedIndexChanged

    End Sub
End Class