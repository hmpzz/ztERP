Imports System.Data.SqlClient
Public Class JGF_detail_question

    Private Sub JGF_detail_question_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim sqlcon As New SqlConnection
        Dim sqlcmd As New SqlCommand
        Dim sqlad As New SqlDataAdapter

        Dim dt As New DataTable
        Dim sql As String
        Dim i As Int32

        sqlcon = getconn()
        sqlcmd.Connection = sqlcon



        Me.ComboBox1.Items.Clear()
        Me.ComboBox2.Items.Clear()

        Me.ComboBox1.Items.Add("")
        Me.ComboBox2.Items.Add("")

        sql = "select * from t_supplier where stop=1 order by supplier_name"

        sqlcmd.CommandText = sql
        sqlad.SelectCommand = sqlcmd
        dt.Rows.Clear()
        sqlad.Fill(dt)

        For i = 0 To dt.Rows.Count - 1
            Me.ComboBox1.Items.Add(dt.Rows(i)("supplier_name"))
            Me.ComboBox2.Items.Add(dt.Rows(i)("supplier_no"))
        Next
    End Sub

    Private Sub ComboBox1_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ComboBox1.SelectedIndexChanged
        Me.ComboBox2.SelectedIndex = Me.ComboBox1.SelectedIndex
    End Sub

    Private Sub ToolStripButton_exit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripButton_exit.Click
        Me.Close()
    End Sub

    Private Sub ToolStripButton_refresh_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripButton_refresh.Click
        Dim sqlcon As New SqlConnection
        Dim sqlcmd As New SqlCommand
        Dim sqlad As New SqlDataAdapter

        Dim dt As New DataTable
        Dim sql As String


        'If Me.ComboBox1.SelectedIndex < 0 Then
        '    MsgBox("请选择供应商！", MsgBoxStyle.OkOnly, "提示")
        '    Exit Sub
        'End If

        sqlcon = getconn()
        sqlcmd.Connection = sqlcon

        If Me.ComboBox1.SelectedIndex <= 0 Then
            sql = "SELECT a.project_no,case state when 0 then '新建' when 1 then '审核' when 2 then '结案' end as state1, " & _
            "	c.supplier_name,case print_state when 0 then '未打印' when 1 then '已打印' end as print_state1, " & _
            "	b.*,a.check_date,a.print_date,a.kp_money " & _
            "FROM t_outwork_master a left join  " & _
            "	t_outwork_detail b on a.billno=b.billno left join  " & _
            "     t_supplier c on a.supplier_no=c.supplier_no  " & _
            "where 1=1  and a.input_date >='" & Format(Me.DateTimePicker1.Value, "yyyy-MM-dd") & " 00:00:00' and a.input_date<='" & Format(Me.DateTimePicker2.Value, "yyyy-MM-dd") & " 23:59:59' "
        Else
            sql = "SELECT a.project_no,case state when 0 then '新建' when 1 then '审核' when 2 then '结案' end as state1, " & _
            "	c.supplier_name,case print_state when 0 then '未打印' when 1 then '已打印' end as print_state1, " & _
            "	b.*,a.check_date,a.print_date,a.kp_money " & _
            "FROM t_outwork_master a left join  " & _
            "	t_outwork_detail b on a.billno=b.billno left join  " & _
            "     t_supplier c on a.supplier_no=c.supplier_no  " & _
            "where 1=1 and a.supplier_no='" & Me.ComboBox2.Text & "' and a.input_date >='" & Format(Me.DateTimePicker1.Value, "yyyy-MM-dd") & " 00:00:00' and a.input_date<='" & Format(Me.DateTimePicker2.Value, "yyyy-MM-dd") & " 23:59:59' "
        End If
        



        sqlcmd.CommandText = sql
        sqlad.SelectCommand = sqlcmd
        dt.Rows.Clear()
        sqlad.Fill(dt)

        Me.ExDataGridView1.SetDataSource(dt)
        Me.ExDataGridView1.Columns.Clear()

        Me.ExDataGridView1.AddColumn("billno", "加工单号")
        Me.ExDataGridView1.AddColumn("supplier_name", "供应商")

        Me.ExDataGridView1.AddColumn("project_no", "工程号")
        Me.ExDataGridView1.AddColumn("name", "名称")



        Me.ExDataGridView1.AddColumn("amount", "数量", , , , DataGridViewContentAlignment.BottomRight)


        If price_state = 1 Then
            Me.ExDataGridView1.AddColumn("price", "单价", , , , DataGridViewContentAlignment.BottomRight)
            Me.ExDataGridView1.AddColumn("money", "金额", , , , DataGridViewContentAlignment.BottomRight)
            Me.ExDataGridView1.AddColumn("kp_money", "已开票金额", , , , DataGridViewContentAlignment.BottomRight)
        End If
        Me.ExDataGridView1.AddColumn("memo", "说明", , , , DataGridViewContentAlignment.BottomRight)

        Me.ExDataGridView1.AddColumn("check_date", "审核日期", , , , , "yyyy-MM-dd")
        Me.ExDataGridView1.AddColumn("print_date", "打印日期", , , , , "yyyy-MM-dd")


        If price_state = 1 Then
            sql = "select sum(money) as Summoney from (" & sql & ") a"
            sqlcmd.CommandText = sql
            Me.Label4.Text = "金额总计：" & sqlcmd.ExecuteScalar
        Else
            Me.Label4.Text = ""
        End If
    End Sub

    Private Sub ToolStripButton_excel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripButton_excel.Click
        Dim ToExcel As New BaseClass.TOEXCEL

        If ToExcel.ExToExcel(Me.ExDataGridView1, Me.SaveFileDialog1, True, "数量") = True Then
            MsgBox("导出成功！", MsgBoxStyle.OkOnly, "提示")
        End If
    End Sub
End Class