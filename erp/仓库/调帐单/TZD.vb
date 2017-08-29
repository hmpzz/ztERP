Imports System.Data.SqlClient
Public Class TZD

    Private Sub TZD_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Dim sqlcon As New SqlConnection
        Dim sqlcmd As New SqlCommand
        Dim sqlad As New SqlDataAdapter


        Dim dt As New DataTable
        Dim sql As String
        Dim i As Integer

        Me.ComboBox1.Items.Clear()

        sqlcon = getconn()
        sqlcmd.Connection = sqlcon

        Sql = "select a.* from t_depot a inner join t_user_depot b on a.depot_code=b.depot_code where a.stop=1 and b.login_id='" & login_id & "' order by a.depot_code"
        sqlcmd.CommandText = Sql
        sqlad.SelectCommand = sqlcmd

        dt.Clear()
        sqlad.Fill(dt)

        For i = 0 To dt.Rows.Count - 1
            Me.ComboBox1.Items.Add(dt.Rows(i)("depot_name"))
            Me.ComboBox2.Items.Add(dt.Rows(i)("depot_code"))
        Next

    End Sub

    Private Sub ToolStripButton_exit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripButton_exit.Click
        Me.Close()
    End Sub

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


        If Me.CheckBox1.Checked Then
            s1 = " and a.amount<>0 "
        End If

        sql = "SELECT a.*, b.item_name ,b.item_standard,b.unit ,e.depot_name,b.max_amount,b.min_amount " & _
            "FROM T_part a LEFT  JOIN " & _
            "      t_item b ON a.item_code = b.item_code left join  " & _
            "	t_depot e on a.depot_code=e.depot_code " & _
            " where  a.depot_code='" & Me.ComboBox2.Text & "'  " & s1 & s


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

        Me.ExDataGridView1.AddColumn("amount", "��ǰ���", , , , DataGridViewContentAlignment.MiddleRight, "#0.000")
        Me.ExDataGridView1.AddColumn("qc_amount", "�ڳ����", , , , DataGridViewContentAlignment.MiddleRight, "#0.000")

        Me.ExDataGridView1.AddColumn("max_amount", "�������", , , , DataGridViewContentAlignment.MiddleRight, "#0.000")

        Me.ExDataGridView1.AddColumn("min_amount", "�������", , , , DataGridViewContentAlignment.MiddleRight, "#0.000")


        Me.ExDataGridView1.AddColumn("price", "����", , , , DataGridViewContentAlignment.MiddleRight, "#0.0000")

        Me.ExDataGridView1.AddColumn("money", "���", , , , DataGridViewContentAlignment.MiddleRight, "#0.0000")






    End Sub

    Private Sub ToolStripButton_excel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripButton_excel.Click
        Dim ToExcel As New BaseClass.TOEXCEL

        If ToExcel.ExToExcel(Me.ExDataGridView1, Me.SaveFileDialog1, True, "��ǰ���", "�ڳ����") Then
            MsgBox("�����ɹ���", MsgBoxStyle.OkOnly, "��ʾ")
        End If
    End Sub

    Private Sub ��������ToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ��������ToolStripMenuItem.Click
        Dim tzd_mod As New tzd_mod

        If Me.ComboBox1.SelectedIndex < 0 Then
            MsgBox("��ѡ��ֿ⣡", MsgBoxStyle.OkOnly, "��ʾ")
            Exit Sub
        End If

        If Me.ExDataGridView1.SelectedCells.Count = 0 Then
            MsgBox("��ѡ��Ҫ�������У�", MsgBoxStyle.OkOnly, "��ʾ")
            Exit Sub
        End If


        tzd_mod.Owner = Me
        tzd_mod.TextBox1.Text = Me.ExDataGridView1.Rows(Me.ExDataGridView1.SelectedCells(0).RowIndex).Cells("item_code").Value
        tzd_mod.TextBox2.Text = Me.ExDataGridView1.Rows(Me.ExDataGridView1.SelectedCells(0).RowIndex).Cells("item_name").Value
        tzd_mod.TextBox3.Text = Me.ExDataGridView1.Rows(Me.ExDataGridView1.SelectedCells(0).RowIndex).Cells("amount").Value
        tzd_mod.TextBox4.Text = Me.ExDataGridView1.Rows(Me.ExDataGridView1.SelectedCells(0).RowIndex).Cells("price").Value
        tzd_mod.TextBox5.Text = Me.ExDataGridView1.Rows(Me.ExDataGridView1.SelectedCells(0).RowIndex).Cells("money").Value
        tzd_mod.depot_code = Me.ComboBox2.Text.Trim
        tzd_mod.ShowDialog()

    End Sub

    Private Sub ComboBox1_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ComboBox1.SelectedIndexChanged
        Me.ExDataGridView1.ClearDSDM()
        Me.ExDataGridView1.Rows.Clear()
        Me.ComboBox2.SelectedIndex = Me.ComboBox1.SelectedIndex
    End Sub


End Class