Imports System.Data.SqlClient
Public Class GoodsQuesonGYS
    Friend dt_P As New DataTable

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        Dim warehouser_amount As New warehouse_amount

        Dim sqlcon As New SqlConnection
        Dim sqlcmd As New SqlCommand
        Dim sqlad As New SqlDataAdapter

        Dim dt As New DataTable
        Dim sql As String

        Dim s As String = ""

   



        warehouse_amount.Owner = Me


        warehouse_amount.ShowDialog()
        If warehouse_amount.DialogResult = Windows.Forms.DialogResult.Cancel Then
            warehouse_amount.Dispose()
            Exit Sub
        End If


        If Me.Tag = "" Then
            Exit Sub
        End If


        sqlcon = getconn()
        sqlcmd.Connection = sqlcon

        sql = "select item_name,item_code " & _
                "from t_item  " & _
                " where id in (" & Me.Tag & ")"
        sqlcmd.CommandText = sql

        sqlcmd.CommandText = sql
        sqlad.SelectCommand = sqlcmd
        dt.Rows.Clear()
        sqlad.Fill(dt)

        Me.TextBox1.Text = dt.Rows(0)("item_name")
        Me.TextBox2.Text = dt.Rows(0)("item_code")

        Me.Tag = ""
    End Sub

    Private Sub ToolStripButton_exit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripButton_exit.Click
        Me.Close()
    End Sub

    Private Sub ToolStripButton_excel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripButton_excel.Click
        Dim ToExcel As New BaseClass.TOEXCEL

        If ToExcel.ExToExcel(Me.ExDataGridView1, Me.SaveFileDialog1, True, "数量") = True Then
            MsgBox("导出成功！", MsgBoxStyle.OkOnly, "提示")
        End If
    End Sub

    Private Sub ToolStripButton_refresh_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripButton_refresh.Click
        Dim sqlcon As New SqlConnection
        Dim sqlcmd As New SqlCommand
        Dim sqlad As New SqlDataAdapter

        Dim dt As New DataTable
        Dim sql As String


        If Me.TextBox1.Text.Trim.Length = 0 Then
            MsgBox("请选择物料！", MsgBoxStyle.OkOnly, "提示")
            Exit Sub
        End If

        sqlcon = getconn()
        sqlcmd.Connection = sqlcon

        sql = "select c.*,a.billno as cgbillno,d.supplier_name,b.input_date,e.item_name,b.kp_money from  " & _
              "	purchase_master a inner join  " & _
              "	t_StockBill_master b on a.billno=b.resourceno and b.tempflag=0 and billkindcode='20' inner join  " & _
              "	t_StockBill_detail c on b.billno=c.billno inner join " & _
              "   t_supplier d on a.supplier_no=d.supplier_no inner join " & _
              "   t_item e on c.item_code=e.item_code  " & _
              "where 1=1 and c.item_code='" & Me.TextBox2.Text.Trim & "' and b.input_date >='" & Format(Me.DateTimePicker1.Value, "yyyy-MM-dd") & " 00:00:00' and b.input_date<='" & Format(Me.DateTimePicker2.Value, "yyyy-MM-dd") & " 23:59:59' " & _
              "union all  " & _
              "select c.*,a.billno as cgbillno,d.supplier_name,b.input_date,e.item_name,b.kp_money from  " & _
              "	purchase_master a inner join  " & _
              "	t_StockBill_master_history b on a.billno=b.resourceno and b.tempflag=0 and billkindcode='20' inner join  " & _
              "	t_StockBill_detail_history c on b.billno=c.billno inner join " & _
              "   t_supplier d on a.supplier_no=d.supplier_no inner join  " & _
              "   t_item e on c.item_code=e.item_code  " & _
              "where 1=1 and c.item_code='" & Me.TextBox2.Text.Trim & "' and b.input_date >='" & Format(Me.DateTimePicker1.Value, "yyyy-MM-dd") & " 00:00:00' and b.input_date<='" & Format(Me.DateTimePicker2.Value, "yyyy-MM-dd") & " 23:59:59' "




        sqlcmd.CommandText = sql
        sqlad.SelectCommand = sqlcmd
        dt.Rows.Clear()
        sqlad.Fill(dt)

        Me.ExDataGridView1.SetDataSource(dt)
        Me.ExDataGridView1.Columns.Clear()

        Me.ExDataGridView1.AddColumn("cgbillno", "采购单号")
        Me.ExDataGridView1.AddColumn("supplier_name", "供应商")

        Me.ExDataGridView1.AddColumn("billno", "入库单号")
        Me.ExDataGridView1.AddColumn("item_code", "物料编码")
        Me.ExDataGridView1.AddColumn("item_name", "物料名称")


        Me.ExDataGridView1.AddColumn("amount", "数量", , , , DataGridViewContentAlignment.BottomRight)


        If price_state = 1 Then
            Me.ExDataGridView1.AddColumn("price", "单价", , , , DataGridViewContentAlignment.BottomRight)
            Me.ExDataGridView1.AddColumn("money", "金额", , , , DataGridViewContentAlignment.BottomRight)
            Me.ExDataGridView1.AddColumn("kp_money", "已开票金额", , , , DataGridViewContentAlignment.BottomRight)
        End If
        Me.ExDataGridView1.AddColumn("input_date", "入库日期", , , , , "yyyy-MM-dd")


     
    End Sub
End Class