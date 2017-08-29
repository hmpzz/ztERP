Imports System.Data.SqlClient
Public Class T_KCMYJZ_question
    Public cha As String = ""


    Private Sub ToolStripButton_refresh_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripButton_refresh.Click
        If Me.ComboBox1.SelectedIndex < 0 Then
            MsgBox("��ѡ��Ҫ���ҵ��·�!", MsgBoxStyle.OkOnly, "��ʾ")
            Exit Sub
        End If
        cha = ""

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

    
        sql = "SELECT a.*, b.item_name ,b.item_standard,b.unit ,e.depot_name,b.max_amount,b.min_amount " & _
            "FROM T_part_history a LEFT  JOIN " & _
            "      t_item b ON a.item_code = b.item_code left join  " & _
            "	t_depot e on a.depot_code=e.depot_code " & _
            " where a.accountterm='" & Me.ComboBox1.Text.Trim & "' " & s1 & s


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
        If price_state = 1 Then
            Me.ExDataGridView1.AddColumn("price", "����", , , , DataGridViewContentAlignment.MiddleRight, "#0.0000")
            Me.ExDataGridView1.AddColumn("money", "���", , , , DataGridViewContentAlignment.MiddleRight, "#0.00")
        End If
        Me.ExDataGridView1.AddColumn("end_date", "���һ�γ����ʱ��", , , , , "yyyy-MM-dd")

        Me.ExDataGridView1.AddColumn("accountterm", "����")
    End Sub

    Private Sub ToolStripButton_exit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripButton_exit.Click
        Me.Close()
    End Sub

    Private Sub T_KCMYJZ_question_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim sqlcon As New SqlConnection
        Dim sqlcmd As New SqlCommand
        Dim sqlad As New SqlDataAdapter

        Dim dt As New DataTable
        Dim sql As String
        Dim i As Int32

        sqlcon = getconn()
        sqlcmd.Connection = sqlcon



        Me.ComboBox1.Items.Clear()

        sql = "select distinct(accountterm) from t_part_history order by accountterm desc "

        sqlcmd.CommandText = sql
        sqlad.SelectCommand = sqlcmd
        dt.Rows.Clear()
        sqlad.Fill(dt)

        For i = 0 To dt.Rows.Count - 1
            Me.ComboBox1.Items.Add(dt.Rows(i)("accountterm"))
        Next
    End Sub

    Private Sub ToolStripButton_excel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripButton_excel.Click
        Dim ToExcel As New BaseClass.TOEXCEL

        If ToExcel.ExToExcel(Me.ExDataGridView1, Me.SaveFileDialog1, True, "��ǰ���", "�ڳ����") Then
            MsgBox("�����ɹ���", MsgBoxStyle.OkOnly, "��ʾ")
        End If
    End Sub
End Class