Imports System.Data.SqlClient
Public Class FYD_detail_question

    Private Sub JGF_detail_question_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

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




        sqlcon = getconn()
        sqlcmd.Connection = sqlcon

        sql = "SELECT a.project_no,case state when 0 then '新建' when 1 then '审核' when 2 then '结案' end as state1, " & _
        "	b.*,a.check_date,a.print_date,a.input_date,a.input_man " & _
        "FROM t_outlay_master a left join  " & _
        "	t_outlay_detail b on a.billno=b.billno " & _
        "where 1=1  and a.input_date >='" & Format(Me.DateTimePicker1.Value, "yyyy-MM-dd") & " 00:00:00' and a.input_date<='" & Format(Me.DateTimePicker2.Value, "yyyy-MM-dd") & " 23:59:59' "


        '     sql = "SELECT a.*,case a.state when 0 then '新建' when 1 then '审核' when 2 then '结案' end as state1,b.project_name " & _
        '"                FROM t_outlay_master a  left join   " & _
        '"                	  	project_master b on a.project_no=b.project_no  " & _
        '"                where 1=1 " & s1 & s & " order by a.billno"


        sqlcmd.CommandText = sql
        sqlad.SelectCommand = sqlcmd
        dt.Rows.Clear()
        sqlad.Fill(dt)

        Me.ExDataGridView1.SetDataSource(dt)
        Me.ExDataGridView1.Columns.Clear()

        Me.ExDataGridView1.AddColumn("billno", "费用单号")


        Me.ExDataGridView1.AddColumn("project_no", "工程号")

        If price_state = 1 Then
            Me.ExDataGridView1.AddColumn("money", "金额", , , , DataGridViewContentAlignment.BottomRight)
        End If

        Me.ExDataGridView1.AddColumn("check_date", "审核日期", , , , , "yyyy-MM-dd")
        Me.ExDataGridView1.AddColumn("print_date", "打印日期", , , , , "yyyy-MM-dd")
        Me.ExDataGridView1.AddColumn("input_man", "录入人")
        Me.ExDataGridView1.AddColumn("input_date", "录入日期", , , , , "yyyy-MM-dd")
        Me.ExDataGridView1.AddColumn("memo", "说明")

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