Imports System.Data.SqlClient
Public Class sys_month

    

    Private Sub sys_month_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Me.NumericUpDown1.Value = Year(Now)

    End Sub

    Private Sub ToolStripButton_refresh_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripButton_refresh.Click
        If Me.RadioButton1.Checked Then
            Refresh_ExDataGridView()
        Else
            Refresh_ExDataGridView(Me.NumericUpDown1.Value)
        End If
    End Sub

    Public Sub Refresh_ExDataGridView(Optional ByVal Y As String = "")

        Dim sqlcon As New SqlConnection
        Dim sqlcmd As New SqlCommand
        Dim sqlad As New SqlDataAdapter

        Dim sql As String = ""
        Dim dt As New DataTable


        sqlcon = getconn()


        sqlcmd.Connection = sqlcon

        If Y <> "" Then
            sql = "SELECT accountterm,begin_date,end_date, case when state=0 then '' else '使用' end as stateU from t_monthend_date where left(AccountTerm,4)= '" & Y & "'"
        Else
            sql = "SELECT accountterm,begin_date,end_date, case when state=0 then '' else '使用' end as stateU from t_monthend_date "
        End If

        sqlcmd.CommandText = sql
        sqlad.SelectCommand = sqlcmd
        dt.Rows.Clear()
        sqlad.Fill(dt)

        Me.ExDataGridView1.SetDataSource(dt)
        Me.ExDataGridView1.Columns.Clear()

        Me.ExDataGridView1.AddColumn("AccountTerm", "帐期")
        Me.ExDataGridView1.AddColumn("begin_date", "开始日期")
        Me.ExDataGridView1.AddColumn("end_date", "结束日期")
        Me.ExDataGridView1.AddColumn("stateU", "状态")


    End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        Dim sys_month_SC As New sys_month_SC
        sys_month_SC.Owner = Me
        sys_month_SC.ShowDialog()
    End Sub

    Private Sub RadioButton2_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RadioButton2.CheckedChanged
        Refresh_ExDataGridView(Me.NumericUpDown1.Value)
    End Sub

    Private Sub RadioButton1_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RadioButton1.CheckedChanged
        Refresh_ExDataGridView()
    End Sub

    Private Sub ExDataGridView1_CellContentClick(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles ExDataGridView1.CellContentClick

    End Sub

    Private Sub ExDataGridView1_CellDoubleClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles ExDataGridView1.CellDoubleClick
        Dim sqlcon As New SqlConnection
        Dim sqlcmd As New SqlCommand
        Dim sys_month_tz As New sys_month_TZ



        Dim sql As String

        If Me.ExDataGridView1.SelectedRows.Count = 0 Then Exit Sub

        sqlcon = getconn()
        sqlcmd.Connection = sqlcon

        sql = "select state from t_monthend_date where AccountTerm='" & Me.ExDataGridView1.SelectedRows(0).Cells("accountterm").Value & "'"
        sqlcmd.CommandText = sql
        If sqlcmd.ExecuteScalar() = 1 Then
            MsgBox("该会计日期已经启用，不可以修改！", MsgBoxStyle.OkOnly, "提示")
            Exit Sub
        End If

        sys_month_tz.Owner = Me
        sys_month_tz.DateTimePicker1.Value = Me.ExDataGridView1.SelectedRows(0).Cells("begin_date").Value
        sys_month_tz.DateTimePicker2.Value = Me.ExDataGridView1.SelectedRows(0).Cells("end_date").Value
        sys_month_tz.accountterm = Me.ExDataGridView1.SelectedRows(0).Cells("accountterm").Value
        sys_month_tz.ShowDialog()

        If sys_month_tz.DialogResult = Windows.Forms.DialogResult.OK Then
            If Me.RadioButton1.Checked Then
                Refresh_ExDataGridView()
            Else
                Refresh_ExDataGridView(Me.NumericUpDown1.Value)
            End If
        End If
    End Sub

 
    Private Sub NumericUpDown1_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles NumericUpDown1.ValueChanged
        Me.RadioButton2.Checked = True
        Refresh_ExDataGridView(Me.NumericUpDown1.Value)
    End Sub

    Private Sub ToolStripButton_exit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripButton_exit.Click
        Me.Close()
    End Sub
End Class