Imports System.Data.SqlClient
Public Class purchase_JA

    

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
                                   "where  t_rs.bm_id= " & BM_id & s & s1 & "  order by a.billno "
        End If

      


        sqlcmd.CommandText = sql
        sqlad.SelectCommand = sqlcmd
        dt.Rows.Clear()
        sqlad.Fill(dt)

        Me.ExDataGridView1.SetDataSource(dt)
        Me.ExDataGridView1.Columns.Clear()


        Me.ExDataGridView1.AddColumn("billno", "采购单号")
        'Me.ExDataGridView1.AddColumn("project_no", "工程号")
        Me.ExDataGridView1.AddColumn("supplier_name", "供应商")
        Me.ExDataGridView1.AddColumn("depot_name", "仓库")
        Me.ExDataGridView1.AddColumn("state1", "状态")

        Me.ExDataGridView1.AddColumn("memo", "说明")


        Me.ExDataGridView1.AddColumn("input_man", "录入人")
        Me.ExDataGridView1.AddColumn("input_date", "录入日期", , , , , "yyyy-MM-dd")

        Me.ExDataGridView1.AddColumn("check_man", "审核人")
        Me.ExDataGridView1.AddColumn("check_date", "审核日期", , , , , "yyyy-MM-dd")

        Me.ExDataGridView1.AddColumn("state", "state", , , , , , False)
        Me.ExDataGridView1.AddColumn("id", "id", , , , , , False)

        If Me.ExDataGridView1.Rows.Count > 0 Then
            cellclick(0)
        Else
            cellclick(-1)
        End If
    End Sub


    Private Sub cellclick(ByVal index As Int32)
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

    Private Sub ExDataGridView1_CellClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles ExDataGridView1.CellClick
        If e.RowIndex < 0 Then
            Exit Sub
        Else

            cellclick(e.RowIndex)
        End If
    End Sub


    Private Sub ExDataGridView1_CellMouseDown(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellMouseEventArgs) Handles ExDataGridView1.CellMouseDown
        If Me.ExDataGridView1.SelectedCells.Count = 0 Then

            Me.结案ToolStripMenuItem.Enabled = False

        End If

        If e.RowIndex < 0 Then
            Exit Sub
        End If

        If Me.ExDataGridView1.Rows(e.RowIndex).Cells("state1").Value = "新建" Then

            Me.结案ToolStripMenuItem.Enabled = True

        ElseIf Me.ExDataGridView1.Rows(e.RowIndex).Cells("state1").Value = "审核" Then

            Me.结案ToolStripMenuItem.Enabled = True

        ElseIf Me.ExDataGridView1.Rows(e.RowIndex).Cells("state1").Value = "结案" Then

            Me.结案ToolStripMenuItem.Enabled = False

        End If
    End Sub

    Private Sub 结案ToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles 结案ToolStripMenuItem.Click
        Dim sqlcon As New SqlConnection
        Dim sqlcmd As New SqlCommand

        Dim sql As String
        Dim k As Int32


        If Me.ExDataGridView1.SelectedCells.Count = 0 Then
            MsgBox("请选择要结案的单据！", MsgBoxStyle.OkOnly, "提示")
            Exit Sub
        End If


        sqlcon = getconn()
        sqlcmd.Connection = sqlcon

        sql = "SELECT state " & _
                "FROM purchase_master where billno='" & Me.ExDataGridView1.Rows(Me.ExDataGridView1.SelectedCells(0).RowIndex).Cells("billno").Value & "'"


        sqlcmd.CommandText = sql
        k = sqlcmd.ExecuteScalar

        If k = 2 Then
            MsgBox("该单据已经结案，请检查后操作！", MsgBoxStyle.OkOnly, "提示")
            Exit Sub
        End If

        sql = "update  purchase_master set state=2 where billno='" & Me.ExDataGridView1.Rows(Me.ExDataGridView1.SelectedCells(0).RowIndex).Cells("billno").Value & "'"
        sqlcmd.CommandText = sql
        sqlcmd.ExecuteNonQuery()

        'Refresh_ExDataGridView(Me.TreeView1.SelectedNode.Text)
        Me.ExDataGridView1.Rows(Me.ExDataGridView1.SelectedCells(0).RowIndex).Cells("state1").Value = "结案"
        Me.ExDataGridView1.Rows(Me.ExDataGridView1.SelectedCells(0).RowIndex).Cells("state").Value = 2
        MsgBox("结案成功！", MsgBoxStyle.OkOnly, "提示")
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
End Class