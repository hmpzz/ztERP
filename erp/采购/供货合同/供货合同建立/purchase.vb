Imports System.Data.SqlClient

Public Class purchase



    Private Sub ToolStripButton_add_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripButton_add.Click
        Dim purchase_add As New purchase_add
        purchase_add.state = "new"

        purchase_add.Owner = Me

        If purchase_add.ShowDialog() = Windows.Forms.DialogResult.OK Then
            Refresh_ExDataGridView_master()
        End If
    End Sub

    Private Sub ToolStripButton_mod_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripButton_mod.Click
        Dim purchase_add As New purchase_add

        Dim sqlcon As New SqlConnection
        Dim sqlcmd As New SqlCommand
        Dim sql As String = "'"




        If Me.ExDataGridView1.SelectedCells.Count <= 0 Then
            MsgBox("请选择要修改的行！", MsgBoxStyle.OkOnly, "提示")
            Exit Sub
        End If

        sqlcon = getconn()

        sql = "select count(*) from  purchase_master where state=0 and billno='" & Me.ExDataGridView1.Rows(Me.ExDataGridView1.SelectedCells(0).RowIndex).Cells("billno").Value & "' "
        sqlcmd.Connection = sqlcon
        sqlcmd.CommandText = sql
        If sqlcmd.ExecuteScalar() = 0 Then
            MsgBox("该记录已经被删除或者是审核，请刷新后再试！", MsgBoxStyle.OkOnly, "提示")
            Exit Sub
        End If


       

        ''************************************************************************************************
        'sql = "select count(*) " & _
        '        "from t_order_BH " & _
        '        "where bh_billno='" & Me.ExDataGridView1.Rows(Me.ExDataGridView1.SelectedCells(0).RowIndex).Cells("bh_no").Value & "' "
        'sqlcmd.CommandText = sql

        'If sqlcmd.ExecuteScalar > 0 Then
        '    MsgBox("该单据与销售订单有对应关系，不可以修改！", MsgBoxStyle.OkOnly, "提示")
        '    Exit Sub


        'End If
        ''************************************************************************************************




        purchase_add.Owner = Me
        purchase_add.state = "mod"


        ' purchase_add.TextBox1.Text = Me.ExDataGridView1.Rows(Me.ExDataGridView1.SelectedCells.Item(0).RowIndex).Cells("billno").Value
        purchase_add.ShowDialog()

        cellclick(Me.ExDataGridView1.SelectedCells.Item(0).RowIndex)
    End Sub

    Private Sub ToolStripButton_refresh_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripButton_refresh.Click
        Refresh_ExDataGridView_master()
    End Sub

    Private Sub ToolStripButton_exit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripButton_exit.Click
        Me.Close()
    End Sub

    Private Sub Refresh_ExDataGridView_master(Optional ByVal s As String = "")
        Dim sqlcon As New SqlConnection
        Dim sqlcmd As New SqlCommand
        Dim sqlad As New SqlDataAdapter
        Dim s1 As String = ""

        Dim dt As New DataTable
        Dim sql As String



        sqlcon = getconn()
        sqlcmd.Connection = sqlcon

        If Me.CheckBox1.Checked Then
            s1 = s1 & " a.state=0 or "
        End If

        If Me.CheckBox2.Checked Then
            s1 = s1 & " a.state=1 or "
        End If

        If Me.CheckBox3.Checked Then
            s1 = s1 & " a.state=2 or "
        End If

        If s1.Length <> 0 Then
            s1 = Microsoft.VisualBasic.Left(s1, s1.Length - 3)
            s1 = " and (" & s1 & ")"
        End If



        If BM_id = 49 Or BM_id = 78 Then
            sql = "SELECT a.*,case state when 0 then '新建' when 1 then '审核' when 2 then '结案' end as state1,b.supplier_name,c.depot_name " & _
                    "FROM purchase_master a left join " & _
                    "t_supplier b on a.supplier_no=b.supplier_no left join " & _
                    "t_depot c on a.depot_code=c.depot_code " & _
                    "where 1=1 " & s & s1 & "  order by a.billno "
        Else
            sql = "SELECT a.*,case state when 0 then '新建' when 1 then '审核' when 2 then '结案' end as state1,b.supplier_name,c.depot_name " & _
                    "FROM purchase_master a left join " & _
                    "t_supplier b on a.supplier_no=b.supplier_no left join " & _
                    "t_depot c on a.depot_code=c.depot_code left join  " & _
                       " t_rs on a.input_man=t_rs.rs_name " & _
                    "where t_rs.bm_id= " & BM_id & s & s1 & "  order by a.billno "
        End If



        sqlcmd.CommandText = sql
        sqlad.SelectCommand = sqlcmd
        dt.Rows.Clear()
        sqlad.Fill(dt)

        Me.ExDataGridView1.SetDataSource(dt)
        Me.ExDataGridView1.Columns.Clear()

        Me.ExDataGridView1.AddColumn("billno", "采购申请单号")
        'Me.ExDataGridView1.AddColumn("project_no", "工程号")
        Me.ExDataGridView1.AddColumn("supplier_name", "供应商")
        Me.ExDataGridView1.AddColumn("depot_name", "仓库")
        Me.ExDataGridView1.AddColumn("state1", "状态")

        Me.ExDataGridView1.AddColumn("memo", "说明")


        Me.ExDataGridView1.AddColumn("input_man", "录入人")
        Me.ExDataGridView1.AddColumn("input_date", "录入日期", , , , , "yyyy-MM-dd")
        
        Me.ExDataGridView1.AddColumn("check_man", "审核人")
        Me.ExDataGridView1.AddColumn("check_date", "审核日期", , , , , "yyyy-MM-dd")


       
        Me.ExDataGridView1.AddColumn("id", "id", , , , , , False)

        If Me.ExDataGridView1.Rows.Count > 0 Then
            cellclick(0)
        Else
            cellclick(-1)
        End If
    End Sub



    Private Sub cellclick(ByVal index As int32)
        Dim sqlcon As New SqlConnection
        Dim sqlcmd As New SqlCommand
        Dim sqlad As New SqlDataAdapter

        Dim dt As New DataTable
        Dim sql As String


        sqlcon = getconn()


        'sql = "select * from  t_product_FY_detail where fy_id='" & Me.ExDataGridView1.Rows(index).Cells("id").Value & "' order by id"
        If index <> -1 Then
            sql = "select a.*,b.item_name,b.item_standard,b.unit " & _
                    "from purchase_detail a left join  " & _
                    "	t_item b on a.item_code=b.item_code " & _
                     "where a.billno='" & Me.ExDataGridView1.Rows(index).Cells("billno").Value & "' order by a.billno "
        Else
            sql = "select a.*,b.item_name " & _
                     "from purchase_detail a left join  " & _
                     "	t_item b on a.item_code=b.item_code " & _
                      "where a.id=-1 order by a.billno "
        End If
        sqlcmd.Connection = sqlcon
        sqlcmd.CommandText = sql
        sqlad.SelectCommand = sqlcmd
        dt.Rows.Clear()
        sqlad.Fill(dt)

        Me.ExDataGridView2.SetDataSource(dt)
        Me.ExDataGridView2.Columns.Clear()



        Me.ExDataGridView2.AddColumn("item_code", "物料编码")
        Me.ExDataGridView2.AddColumn("item_name", "物料名称")
        Me.ExDataGridView2.AddColumn("item_standard", "规格")
        Me.ExDataGridView2.AddColumn("unit", "单位")


        Me.ExDataGridView2.AddColumn("amount", "数量", , , , DataGridViewContentAlignment.MiddleRight, "#0.00")
        Me.ExDataGridView2.AddColumn("price", "单价", , , , DataGridViewContentAlignment.MiddleRight, "#0.0000")
        Me.ExDataGridView2.AddColumn("money", "金额", , , , DataGridViewContentAlignment.MiddleRight, "#0.00")
        'Me.ExDataGridView2.AddColumn("OK_amount", "已完成数量", 120, , , DataGridViewContentAlignment.MiddleRight, "#0.000")
        Me.ExDataGridView2.AddColumn("OK_amount", "已入库数量", , , , DataGridViewContentAlignment.MiddleRight, "#0.00")

        Me.ExDataGridView2.AddColumn("id", "id", , , , , , False)
    End Sub

    Private Sub ToolStripButton_del_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripButton_del.Click

        Dim sqlcon As New SqlConnection
        Dim sqlcmd As New SqlCommand
        Dim sqlad As New SqlDataAdapter
        Dim mytrans As SqlTransaction

        Dim dt As New DataTable
        Dim sql As String
        Dim i As int32 = 0


        If Me.ExDataGridView1.SelectedCells.Count < 0 Then
            MsgBox("请选择要修改的行！", MsgBoxStyle.OkOnly, "提示")
            Exit Sub
        End If

        If MsgBox("确认要删除采购单号为：" & Me.ExDataGridView1.Rows(Me.ExDataGridView1.SelectedCells(0).RowIndex).Cells("billno").Value & "的单据吗？", MsgBoxStyle.YesNo, "提示") = MsgBoxResult.No Then
            Exit Sub
        End If



        sqlcon = getconn()
        sqlcmd.Connection = sqlcon

        mytrans = sqlcon.BeginTransaction

        sqlcmd.Transaction = mytrans

        Try




            sql = "SELECT count(*)  " & _
                           "FROM purchase_master  where state<>0 and billno='" & Me.ExDataGridView1.Rows(Me.ExDataGridView1.SelectedCells(0).RowIndex).Cells("billno").Value & "' "


            sqlcmd.CommandText = sql
            If sqlcmd.ExecuteScalar <> 0 Then
                Throw New System.Exception("此单据不是新建状态，不能删除！")
            End If



            sql = "delete from purchase_master where billno='" & Me.ExDataGridView1.Rows(Me.ExDataGridView1.SelectedCells(0).RowIndex).Cells("billno").Value & "' "
            sqlcmd.CommandText = sql
            sqlcmd.ExecuteNonQuery()

            sql = "delete from purchase_detail where billno='" & Me.ExDataGridView1.Rows(Me.ExDataGridView1.SelectedCells(0).RowIndex).Cells("billno").Value & "' "
            sqlcmd.CommandText = sql
            sqlcmd.ExecuteNonQuery()

            mytrans.Commit()


        Catch ex As Exception
            mytrans.Rollback()
            MsgBox(ex.Message, MsgBoxStyle.OkOnly, "错误")
            Exit Sub
        End Try

        MsgBox("删除成功！", MsgBoxStyle.OkOnly, "提示")
        Me.Refresh_ExDataGridView_master()
    End Sub

    Private Sub ExDataGridView1_CellClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles ExDataGridView1.CellClick
        If e.RowIndex < 0 Then
            Exit Sub
        Else

            cellclick(e.RowIndex)
        End If
    End Sub


    Private Sub ToolStripButton_show_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripButton_show.Click
        Dim printView As New printView

        Dim GHHT_RPT As New GHHT_RPT

        If Me.ExDataGridView1.SelectedCells.Count = 0 Then
            MsgBox("请选择要打印的单据", MsgBoxStyle.OkOnly, "提示")
            Exit Sub
        End If

        ShowBHTZ_RPT(GHHT_RPT, Me.ExDataGridView1.Rows(Me.ExDataGridView1.SelectedCells(0).RowIndex).Cells("billno").Value)

        printView.CrystalReportViewer1.ReportSource = GHHT_RPT
        printView.CrystalReportViewer1.RefreshReport()
        printView.ShowDialog()
    End Sub


    Public Sub ShowBHTZ_RPT(ByRef GHHT_RPT As GHHT_RPT, ByVal billno As String)
        Dim sql As String
        Dim sqlcon As New SqlConnection
        Dim sqlcmd As New SqlCommand
        Dim sqlad As New SqlDataAdapter
        Dim ds As New DataSet
        Dim i As Int32

        sqlcon = getconn()
        sqlcmd.Connection = sqlcon


        ds.Tables.Clear()

        Dim doc As New System.Drawing.Printing.PrintDocument
        Dim rawKind As Int32 = 11

        For i = 0 To doc.PrinterSettings.PaperSizes.Count - 1
            If doc.PrinterSettings.PaperSizes(i).PaperName.ToUpper = "ZT" Then
                rawKind = doc.PrinterSettings.PaperSizes(i).RawKind
                Exit For
            End If
        Next


        GHHT_RPT.PrintOptions.PaperSize = rawKind

        If rawKind = 11 Then
            GHHT_RPT.PrintOptions.PaperOrientation = CrystalDecisions.Shared.PaperOrientation.Landscape
        Else
            GHHT_RPT.PrintOptions.PaperOrientation = CrystalDecisions.Shared.PaperOrientation.Portrait
        End If



        ' CType(CrystalDecisions.Shared.PaperSize rawKind)

        sql = "SELECT a.*,b.supplier_name,'" & rs_name & "' as print_man,getdate() as print_date " & _
              "FROM purchase_master a left join " & _
              " t_supplier b on a.supplier_no=b.supplier_no" & _
              " where a.billno='" & billno & "'"

        sqlcmd.CommandText = sql
        sqlad.SelectCommand = sqlcmd
        sqlad.Fill(ds, "purchase_master")


        sql = "select a.* ,b.item_name,b.item_standard,b.unit   " & _
                     "from purchase_detail a left join  " & _
                     "	t_item b on a.item_code=b.item_code " & _
                     " where a.billno='" & billno & "' order by a.id "
        sqlcmd.CommandText = sql
        sqlad.SelectCommand = sqlcmd
        sqlad.Fill(ds, "purchase_detail")

        GHHT_RPT.SetDataSource(ds)
        GHHT_RPT.Refresh()

        save_pdf(GHHT_RPT, billno)
    End Sub

    Private Sub ToolStripButton_print_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripButton_print.Click

        Dim GHHT_RPT As New GHHT_RPT

        If Me.ExDataGridView1.SelectedCells.Count = 0 Then
            MsgBox("请选择要打印的单据", MsgBoxStyle.OkOnly, "提示")
            Exit Sub
        End If

        ShowBHTZ_RPT(GHHT_RPT, Me.ExDataGridView1.Rows(Me.ExDataGridView1.SelectedCells(0).RowIndex).Cells("billno").Value)
        GHHT_RPT.PrintToPrinter(1, True, 0, 0)

        MsgBox("打印完成！", MsgBoxStyle.OkOnly, "提示")
    End Sub

    Private Sub ToolStripButton_question_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripButton_question.Click
        Dim purchase_cha As New purchase_cha
        purchase_cha.Owner = Me

        Me.Tag = ""
        purchase_cha.ShowDialog()

        If purchase_cha.DialogResult = Windows.Forms.DialogResult.OK Then
            Refresh_ExDataGridView_master(Me.Tag)
        Else
            Exit Sub
        End If
    End Sub

    Private Sub ExDataGridView1_CellContentClick(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles ExDataGridView1.CellContentClick

    End Sub
End Class