Imports System.Data.SqlClient
Public Class t_tzd_cx

    Private Sub ToolStripButton_refresh_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripButton_refresh.Click
        Refresh_ExDataGridView_master()
    End Sub


    Private Sub Refresh_ExDataGridView_master(Optional ByVal s As String = "")
        Dim sqlcon As New SqlConnection
        Dim sqlcmd As New SqlCommand
        Dim sqlad As New SqlDataAdapter


        Dim dt As New DataTable
        Dim sql As String
        Dim s1 As String = ""



        sqlcon = getconn()
        sqlcmd.Connection = sqlcon


        If Me.ComboBox1.Text.Trim.Length > 0 Then
            s = " and a.depot_code='" & Me.ComboBox2.Text & "'"
        End If

        sql = "select a.*,b.depot_name,c.item_name,c.item_standard,c.unit from t_tzd a left join	 " & _
"				t_depot b on a.depot_code=b.depot_code left join  " & _
"				t_item c on a.item_code=c.item_code " & _
"              where a.tz_date>='" & Format(Me.DateTimePicker1.Value.Date, "yyyy-MM-dd") & " 00:00:00' and a.tz_date<=  '" & Format(Me.DateTimePicker2.Value.Date, "yyyy-MM-dd") & " 23:59:59' " & s


        sqlcmd.CommandText = sql
        sqlad.SelectCommand = sqlcmd
        dt.Rows.Clear()
        sqlad.Fill(dt)

        Me.ExDataGridView1.SetDataSource(dt)


        Me.ExDataGridView1.AddColumn("depot_name", "�ֿ�")
        Me.ExDataGridView1.AddColumn("item_code", "���ϱ���")
        Me.ExDataGridView1.AddColumn("item_name", "��������")
        Me.ExDataGridView1.AddColumn("item_standard", "���")
        Me.ExDataGridView1.AddColumn("unit", "��λ")


        Me.ExDataGridView1.AddColumn("OLD_amount", "����ǰ���", , , , DataGridViewContentAlignment.MiddleRight, "#0.000")
        Me.ExDataGridView1.AddColumn("amount", "��������", , , , DataGridViewContentAlignment.MiddleRight, "#0.000")

        Me.ExDataGridView1.AddColumn("tz_man", "������")

        Me.ExDataGridView1.AddColumn("tz_date", "��������")




    End Sub

    Private Sub t_tzd_cx_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim sqlcon As New SqlConnection
        Dim sqlcmd As New SqlCommand
        Dim sqlad As New SqlDataAdapter


        Dim dt As New DataTable
        Dim sql As String
        Dim i As Integer
        Dim s As String = ""

        Me.ComboBox1.Items.Clear()

        sqlcon = getconn()
        sqlcmd.Connection = sqlcon



        sql = "select a.* from t_depot a inner join t_user_depot b on a.depot_code=b.depot_code where a.stop=1 and b.login_id='" & login_id & "' order by a.depot_code"
        sqlcmd.CommandText = sql
        sqlad.SelectCommand = sqlcmd

        dt.Clear()
        sqlad.Fill(dt)

        Me.ComboBox1.Items.Add("")
        Me.ComboBox2.Items.Add("")

        For i = 0 To dt.Rows.Count - 1
            Me.ComboBox1.Items.Add(dt.Rows(i)("depot_name"))
            Me.ComboBox2.Items.Add(dt.Rows(i)("depot_code"))
        Next
    End Sub

    Private Sub ComboBox1_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ComboBox1.SelectedIndexChanged
        Me.ComboBox2.SelectedIndex = Me.ComboBox1.SelectedIndex
    End Sub

    Private Sub ToolStripButton_exit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripButton_exit.Click
        Me.Close()
    End Sub

    Private Sub DateTimePicker1_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles DateTimePicker1.ValueChanged

    End Sub
End Class