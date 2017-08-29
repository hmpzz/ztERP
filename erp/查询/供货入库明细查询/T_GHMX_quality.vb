Imports System.Data.SqlClient
Public Class T_GHMX_quality


    Private Sub T_GHMX_quality_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
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

        If Me.ComboBox1.SelectedIndex > 0 Then


            sql = "select c.*,a.billno as cgbillno,d.supplier_name,b.input_date,e.item_name,b.kp_money from  " & _
            "	purchase_master a inner join  " & _
            "	t_StockBill_master b on a.billno=b.resourceno and b.tempflag=0 and billkindcode='20' inner join  " & _
            "	t_StockBill_detail c on b.billno=c.billno inner join " & _
            "   t_supplier d on a.supplier_no=d.supplier_no inner join " & _
            "   t_item e on c.item_code=e.item_code  " & _
            "where 1=1 and a.supplier_no='" & Me.ComboBox2.Text & "' and b.input_date >='" & Format(Me.DateTimePicker1.Value, "yyyy-MM-dd") & " 00:00:00' and b.input_date<='" & Format(Me.DateTimePicker2.Value, "yyyy-MM-dd") & " 23:59:59' " & _
            "union all  " & _
            "select c.*,a.billno as cgbillno,d.supplier_name,b.input_date,e.item_name,b.kp_money from  " & _
            "	purchase_master a inner join  " & _
            "	t_StockBill_master_history b on a.billno=b.resourceno and b.tempflag=0 and billkindcode='20' inner join  " & _
            "	t_StockBill_detail_history c on b.billno=c.billno inner join " & _
            "   t_supplier d on a.supplier_no=d.supplier_no inner join  " & _
            "   t_item e on c.item_code=e.item_code  " & _
            "where 1=1 and a.supplier_no='" & Me.ComboBox2.Text & "' and b.input_date >='" & Format(Me.DateTimePicker1.Value, "yyyy-MM-dd") & " 00:00:00' and b.input_date<='" & Format(Me.DateTimePicker2.Value, "yyyy-MM-dd") & " 23:59:59' "

        Else
            sql = "select c.*,a.billno as cgbillno,d.supplier_name,b.input_date,e.item_name,b.kp_money from  " & _
                 "	purchase_master a inner join  " & _
                 "	t_StockBill_master b on a.billno=b.resourceno and b.tempflag=0 and billkindcode='20' inner join  " & _
                 "	t_StockBill_detail c on b.billno=c.billno inner join " & _
                 "   t_supplier d on a.supplier_no=d.supplier_no inner join " & _
                 "   t_item e on c.item_code=e.item_code  " & _
                 "where 1=1 and b.input_date >='" & Format(Me.DateTimePicker1.Value, "yyyy-MM-dd") & " 00:00:00' and b.input_date<='" & Format(Me.DateTimePicker2.Value, "yyyy-MM-dd") & " 23:59:59' " & _
                 "union all  " & _
                 "select c.*,a.billno as cgbillno,d.supplier_name,b.input_date,e.item_name,b.kp_money from  " & _
                 "	purchase_master a inner join  " & _
                 "	t_StockBill_master_history b on a.billno=b.resourceno and b.tempflag=0 and billkindcode='20' inner join  " & _
                 "	t_StockBill_detail_history c on b.billno=c.billno inner join " & _
                 "   t_supplier d on a.supplier_no=d.supplier_no inner join  " & _
                 "   t_item e on c.item_code=e.item_code  " & _
                 "where 1=1  and b.input_date >='" & Format(Me.DateTimePicker1.Value, "yyyy-MM-dd") & " 00:00:00' and b.input_date<='" & Format(Me.DateTimePicker2.Value, "yyyy-MM-dd") & " 23:59:59' "

        End If

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

    Private Sub DateTimePicker1_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles DateTimePicker1.ValueChanged

    End Sub

    Private Sub DateTimePicker2_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles DateTimePicker2.ValueChanged

    End Sub
End Class