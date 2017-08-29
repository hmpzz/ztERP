Imports System.Data.SqlClient
Public Class warehouse_question
    Public cha As String = ""


    Private Sub ToolStripButton_refresh_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripButton_refresh.Click
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

        If Me.CheckBox1.Checked Then
            s1 = " and a.amount<>0 "
        Else
            s1 = ""
        End If


        If Me.CheckBox2.Checked Or Me.CheckBox3.Checked Or Me.CheckBox4.Checked Then
            s1 = s1 & " and ( "
            If Me.CheckBox2.Checked Then
                s1 = s1 & " a.amount>b.max_amount or "
            End If

            If Me.CheckBox3.Checked Then
                s1 = s1 & " a.amount<b.min_amount or "
            End If

            If Me.CheckBox4.Checked Then
                s1 = s1 & " a.end_date<getdate()-30 or "
            End If
            s1 = Microsoft.VisualBasic.Left(s1, Len(s1) - 3)
            s1 = s1 & ")"
        End If

      





        sql = "SELECT a.*, b.item_name ,b.item_standard,b.unit ,e.depot_name,b.max_amount,b.min_amount " & _
            "FROM T_part a LEFT  JOIN " & _
            "      t_item b ON a.item_code = b.item_code left join  " & _
            "	t_depot e on a.depot_code=e.depot_code " & _
            " where b.stop=1 " & s1 & s


        sqlcmd.CommandText = sql
        sqlad.SelectCommand = sqlcmd
        dt.Rows.Clear()
        sqlad.Fill(dt)

        Me.ExDataGridView1.SetDataSource(dt)


        Me.ExDataGridView1.AddColumn("depot_name", "仓库")
        Me.ExDataGridView1.AddColumn("item_code", "物料编码")
        Me.ExDataGridView1.AddColumn("item_name", "物料名称")
        Me.ExDataGridView1.AddColumn("item_standard", "规格")
        Me.ExDataGridView1.AddColumn("unit", "单位")

        Me.ExDataGridView1.AddColumn("amount", "当前库存", , , , DataGridViewContentAlignment.MiddleRight, "#0.000")
        Me.ExDataGridView1.AddColumn("qc_amount", "期初库存", , , , DataGridViewContentAlignment.MiddleRight, "#0.000")

        Me.ExDataGridView1.AddColumn("max_amount", "库存上限", , , , DataGridViewContentAlignment.MiddleRight, "#0.000")

        Me.ExDataGridView1.AddColumn("min_amount", "库存下限", , , , DataGridViewContentAlignment.MiddleRight, "#0.000")
        If price_state = 1 Then
            Me.ExDataGridView1.AddColumn("price", "单价", , , , DataGridViewContentAlignment.MiddleRight, "#0.0000")
            Me.ExDataGridView1.AddColumn("money", "金额", , , , DataGridViewContentAlignment.MiddleRight, "#0.00")
        End If
        Me.ExDataGridView1.AddColumn("end_date", "最后一次出入库时间", , , , , "yyyy-MM-dd")


    End Sub

    Private Sub ToolStripButton_exit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripButton_exit.Click
        Me.Close()
    End Sub

    Private Sub ToolStripButton_question_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripButton_question.Click
        Dim warehouse_question_cha As New warehouse_question_cha
        warehouse_question_cha.Owner = Me
        cha = ""
        warehouse_question_cha.ShowDialog()

        If warehouse_question_cha.DialogResult = Windows.Forms.DialogResult.OK Then
            Refresh_ExDataGridView_master(cha)
        Else
            Exit Sub
        End If
    End Sub

    Private Sub ToolStripButton_excel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripButton_excel.Click
        Dim ToExcel As New BaseClass.TOEXCEL

        If ToExcel.ExToExcel(Me.ExDataGridView1, Me.SaveFileDialog1, True, "当前库存", "期初库存") Then
            MsgBox("导出成功！", MsgBoxStyle.OkOnly, "提示")
        End If
    End Sub

    Private Sub ExDataGridView1_CellContentClick(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles ExDataGridView1.CellContentClick

    End Sub

    Private Sub ExDataGridView1_RowPrePaint(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewRowPrePaintEventArgs) Handles ExDataGridView1.RowPrePaint
        'If (e.RowIndex > Me.ExDataGridView1.Rows.Count - 1) Then Return
        Dim dgr As DataGridViewRow = Me.ExDataGridView1.Rows(e.RowIndex)
        Try
            'dgr.DefaultCellStyle.BackColor = Color.Black

            If (dgr.Cells("max_amount").Value > 0) Then
                If dgr.Cells("amount").Value < dgr.Cells("min_amount").Value Then
                    dgr.DefaultCellStyle.ForeColor = Color.Red
                End If

                If dgr.Cells("amount").Value > dgr.Cells("max_amount").Value Then
                    dgr.DefaultCellStyle.ForeColor = Color.DarkGreen
                End If

              
            End If

            If (dgr.Cells("amount").Value > 0) Then
                If dgr.Cells("end_date").Value < Now().AddDays(-180) Then
                    dgr.DefaultCellStyle.ForeColor = Color.DarkOrange
                End If
            End If
        Catch ex As Exception

        End Try
    End Sub

    Private Sub ToolStripButton1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripButton1.Click
        Dim printView As New printView

        Dim warehouse_question_RPT As New warehouse_question_RPT

        Dim sqlcon As New SqlConnection
        Dim sqlcmd As New SqlCommand



        If Me.ExDataGridView1.SelectedCells.Count = 0 Then
            MsgBox("请选择要打印的单据", MsgBoxStyle.OkOnly, "提示")
            Exit Sub
        End If


        sqlcon = getconn()
        sqlcmd.Connection = sqlcon





        Showwarehouse_question_RPT(warehouse_question_RPT)

        printView.CrystalReportViewer1.ReportSource = warehouse_question_RPT
        printView.CrystalReportViewer1.RefreshReport()
        printView.ShowDialog()
    End Sub


    Public Sub Showwarehouse_question_RPT(ByRef warehouse_question_RPT As warehouse_question_RPT)
        Dim sql As String
        Dim sqlcon As New SqlConnection
        Dim sqlcmd As New SqlCommand
        Dim sqlad As New SqlDataAdapter
        Dim ds As New DataSet

        Dim dt As New DataTable
        Dim s1 As String = ""



        sqlcon = getconn()
        sqlcmd.Connection = sqlcon


        ds.Tables.Clear()

        Dim doc As New System.Drawing.Printing.PrintDocument
        Dim rawKind As Int32 = 1

       

        warehouse_question_RPT.PrintOptions.PaperSize = rawKind

      
        warehouse_question_RPT.PrintOptions.PaperOrientation = CrystalDecisions.Shared.PaperOrientation.Portrait








        sqlcon = getconn()
        sqlcmd.Connection = sqlcon

        If Me.CheckBox1.Checked Then
            s1 = " and a.amount<>0 "
        Else
            s1 = ""
        End If


        If Me.CheckBox2.Checked Or Me.CheckBox3.Checked Or Me.CheckBox4.Checked Then
            s1 = s1 & " and ( "
            If Me.CheckBox2.Checked Then
                s1 = s1 & " a.amount>b.max_amount or "
            End If

            If Me.CheckBox3.Checked Then
                s1 = s1 & " a.amount<b.min_amount or "
            End If

            If Me.CheckBox4.Checked Then
                s1 = s1 & " a.end_date<getdate()-30 or "
            End If
            s1 = Microsoft.VisualBasic.Left(s1, Len(s1) - 3)
            s1 = s1 & ")"
        End If







        sql = "SELECT a.*, b.item_name ,b.item_standard,b.unit ,e.depot_name,b.max_amount,b.min_amount,'" & rs_name & "' as print_man,getdate() as print_date " & _
            "FROM T_part a LEFT  JOIN " & _
            "      t_item b ON a.item_code = b.item_code left join  " & _
            "	t_depot e on a.depot_code=e.depot_code " & _
            " where b.stop=1 " & s1 & cha


        sqlcmd.CommandText = sql
        sqlad.SelectCommand = sqlcmd

        sqlad.Fill(ds, "t_part")


        warehouse_question_RPT.SetDataSource(ds)
        warehouse_question_RPT.Refresh()

        'save_pdf(warehouse_question_RPT, billno)
    End Sub

    Private Sub ToolStripButton2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripButton2.Click

        Dim warehouse_question_RPT As New warehouse_question_RPT

        Dim sqlcon As New SqlConnection
        Dim sqlcmd As New SqlCommand



        If Me.ExDataGridView1.SelectedCells.Count = 0 Then
            MsgBox("请选择要打印的单据", MsgBoxStyle.OkOnly, "提示")
            Exit Sub
        End If


        sqlcon = getconn()
        sqlcmd.Connection = sqlcon

 

        Showwarehouse_question_RPT(warehouse_question_RPT)
        warehouse_question_RPT.PrintToPrinter(1, True, 0, 0)

        MsgBox("打印完成！", MsgBoxStyle.OkOnly, "提示")
    End Sub
End Class