Imports System.Data.SqlClient
Public Class T_FPYS_add_detail

    Private Sub ToolStripButton_refresh_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripButton_refresh.Click
        Refresh_ExDataGridView_master(" and a.project_no like '%" & Me.TextBox1.Text.Trim & "%' ")
    End Sub

    Private Sub ToolStripButton_exit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripButton_exit.Click
        Me.DialogResult = Windows.Forms.DialogResult.Cancel
    End Sub



    Private Sub Refresh_ExDataGridView_master(Optional ByVal s As String = "")
        Dim sqlcon As New SqlConnection
        Dim sqlcmd As New SqlCommand
        Dim sqlad As New SqlDataAdapter
        Dim s1 As String = ""
        Dim s2 As String = ""

        Dim dt As New DataTable
        Dim sql As String



        sqlcon = getconn()
        sqlcmd.Connection = sqlcon


        s1 = s1 & " and a.state=1  "

        s2 = s2 & " and a.effective_date>=getdate()"


        sql = "SELECT a.*,case state when 0 then '新建' when 1 then '审核' when 2 then '结案' end as state1,a.project_money-fp_money as wkp_money " & _
                "FROM project_master a " & _
                "where fp_money<project_money and a.factory_code in (select c.factory_code " & _
                "                       from sys_user a inner join  " & _
                "                       	t_user_factory b on a.login_id=b.login_id inner join  " & _
                "                       	t_factory c on b.factory_code=c.factory_code " & _
                "                       where a.stop_login=1 and c.stop=1 and a.login_id='" & login_id & "' " & _
                "                       )" & s & s1 & s2 & "  order by a.id "
        sqlcmd.CommandText = sql
        sqlad.SelectCommand = sqlcmd
        dt.Rows.Clear()
        sqlad.Fill(dt)

        Me.ExDataGridView1.SetDataSource(dt)
        Me.ExDataGridView1.Columns.Clear()

        Me.ExDataGridView1.AddColumn("check", "", 20, False, New DataGridViewCheckBoxColumn)

        Me.ExDataGridView1.AddColumn("project_no", "工程号", 120)
        Me.ExDataGridView1.AddColumn("project_name", "工程名称")
        Me.ExDataGridView1.AddColumn("project_money", "工程金额", , , , , "#0")
        Me.ExDataGridView1.AddColumn("fp_money", "已开票金额", , , , , "#0")

        Me.ExDataGridView1.AddColumn("wkp_money", "未开票金额", , , , , "#0")



        Me.ExDataGridView1.AddColumn("effective_date", "有效日期", , , , , "yyyy-MM-dd")


        Me.ExDataGridView1.AddColumn("memo", "备注")
        Me.ExDataGridView1.AddColumn("state1", "状态")



        Me.ExDataGridView1.AddColumn("id", "id", , , , , , False)


    End Sub

    Private Sub ToolStripButton_save_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripButton_save.Click
        Dim i As Int32
        Dim s As String

        s = ""

        Me.TextBox1.Focus()
        For i = 0 To Me.ExDataGridView1.Rows.Count - 1
            If Me.ExDataGridView1.Rows(i).Cells("check").Value Then
                s = s & "'" & Me.ExDataGridView1.Rows(i).Cells("project_no").Value & "',"
            End If
        Next

        If s.Length > 0 Then
            s = s.Substring(0, Len(s) - 1)
        End If

        Me.Owner.Tag = s
        Me.DialogResult = Windows.Forms.DialogResult.OK
        Me.Close()
    End Sub

    Private Sub ExDataGridView1_CellContentClick(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles ExDataGridView1.CellContentClick

    End Sub
End Class