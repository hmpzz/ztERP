Imports System.Data.SqlClient
Public Class project_check


    Private Sub ToolStripButton_exit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripButton_exit.Click
        Me.Close()
    End Sub

    Private Sub ToolStripButton_refresh_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripButton_refresh.Click
        Refresh_ExDataGridView_master(" and a.project_no like '%" & Me.TextBox1.Text.Trim & "%' ")
    End Sub





    Private Sub Refresh_ExDataGridView_master(Optional ByVal s As String = "")
        Dim sqlcon As New SqlConnection
        Dim sqlcmd As New SqlCommand
        Dim sqlad As New SqlDataAdapter
        Dim s1 As String = ""
        Dim s2 As String = ""

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


        If Me.CheckBox4.Checked Then
            s2 = s2 & " and a.effective_date>=getdate()"
        End If


        If BM_id = 49 Or BM_id = 78 Then
            sql = "SELECT a.*,case state when 0 then '新建' when 1 then '审核' when 2 then '结案' end as state1,b.customer_name " & _
                           "FROM project_master a left join " & _
                           " t_customer b on a.customer_no=b.customer_no " & _
                           "where factory_code in (select c.factory_code " & _
                           "                       from sys_user a inner join  " & _
                           "                       	t_user_factory b on a.login_id=b.login_id inner join  " & _
                           "                       	t_factory c on b.factory_code=c.factory_code " & _
                           "                       where a.stop_login=1 and c.stop=1 and a.login_id='" & login_id & "' " & _
                           "                       ) " & s & s1 & s2 & "  order by a.id "
        Else
            sql = "SELECT a.*,case state when 0 then '新建' when 1 then '审核' when 2 then '结案' end as state1,b.customer_name " & _
                           "FROM project_master a left join " & _
                           " t_customer b on a.customer_no=b.customer_no left join  " & _
                       " t_rs on a.input_man=t_rs.rs_name " & _
                           "where factory_code in (select c.factory_code " & _
                           "                       from sys_user a inner join  " & _
                           "                       	t_user_factory b on a.login_id=b.login_id inner join  " & _
                           "                       	t_factory c on b.factory_code=c.factory_code " & _
                           "                       where a.stop_login=1 and c.stop=1 and a.login_id='" & login_id & "' " & _
                           "                       ) and t_rs.bm_id= " & BM_id & s & s1 & s2 & "  order by a.id "
        End If

       

        sqlcmd.CommandText = sql
        sqlad.SelectCommand = sqlcmd
        dt.Rows.Clear()
        sqlad.Fill(dt)

        Me.ExDataGridView1.SetDataSource(dt)
        Me.ExDataGridView1.Columns.Clear()

        'Me.ExDataGridView1.AddColumn("check", "", 20, False, New DataGridViewCheckBoxColumn)
        Me.ExDataGridView1.AddColumn("project_no", "工程号")
        Me.ExDataGridView1.AddColumn("project_name", "工程名称")
        Me.ExDataGridView1.AddColumn("project_money", "工程金额", , , , , "#0")
        Me.ExDataGridView1.AddColumn("customer_name", "客户名称")

        Me.ExDataGridView1.AddColumn("FP_money", "开票金额", , , , , "#0")
        Me.ExDataGridView1.AddColumn("SK_money", "收款金额", , , , , "#0")

        Me.ExDataGridView1.AddColumn("effective_date", "有效日期", , , , , "yyyy-MM-dd")


        Me.ExDataGridView1.AddColumn("memo", "备注")
        Me.ExDataGridView1.AddColumn("state1", "状态")

        Me.ExDataGridView1.AddColumn("begin_date", "开工日期", , , , , "yyyy-MM-dd")
        Me.ExDataGridView1.AddColumn("end_date", "完工日期", , , , , "yyyy-MM-dd")
        Me.ExDataGridView1.AddColumn("end_man", "确认人")

        Me.ExDataGridView1.AddColumn("input_man", "录入人")
        Me.ExDataGridView1.AddColumn("input_date", "录入日期", , , , , "yyyy-MM-dd")
        Me.ExDataGridView1.AddColumn("check_man", "审核人")
        Me.ExDataGridView1.AddColumn("check_date", "审核日期", , , , , "yyyy-MM-dd")


        Me.ExDataGridView1.AddColumn("customer_no", "customer_no", , , , , , False)
        Me.ExDataGridView1.AddColumn("id", "id", , , , , , False)

        If Me.ExDataGridView1.Rows.Count > 0 Then
            cellclick(0)
        End If
    End Sub


    Private Sub ExDataGridView1_CellMouseDown(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellMouseEventArgs)
        If Me.ExDataGridView1.SelectedCells.Count = 0 Then
            Me.审核ToolStripMenuItem.Enabled = False
            Me.反审ToolStripMenuItem.Enabled = False
            Me.结案ToolStripMenuItem.Enabled = False
            Me.反结案ToolStripMenuItem.Enabled = False
        End If

        If e.RowIndex < 0 Then
            Exit Sub
        End If

        If Me.ExDataGridView1.Rows(e.RowIndex).Cells("state1").Value = "新建" Then
            Me.审核ToolStripMenuItem.Enabled = True
            Me.反审ToolStripMenuItem.Enabled = False
            Me.结案ToolStripMenuItem.Enabled = True
            Me.反结案ToolStripMenuItem.Enabled = False
        ElseIf Me.ExDataGridView1.Rows(e.RowIndex).Cells("state1").Value = "审核" Then
            Me.审核ToolStripMenuItem.Enabled = False
            Me.反审ToolStripMenuItem.Enabled = True
            Me.结案ToolStripMenuItem.Enabled = True
            Me.反结案ToolStripMenuItem.Enabled = False
        ElseIf Me.ExDataGridView1.Rows(e.RowIndex).Cells("state1").Value = "结案" Then
            Me.审核ToolStripMenuItem.Enabled = False
            Me.反审ToolStripMenuItem.Enabled = False
            Me.结案ToolStripMenuItem.Enabled = False
            Me.反结案ToolStripMenuItem.Enabled = True
        End If
    End Sub

    Private Sub 审核ToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles 审核ToolStripMenuItem.Click
        Dim sqlcon As New SqlConnection
        Dim sqlcmd As New SqlCommand

        Dim sql As String
        Dim k As int32

        sqlcon = getconn()
        sqlcmd.Connection = sqlcon

        sql = "SELECT state " & _
                "FROM project_master where project_no='" & Me.ExDataGridView1.Rows(Me.ExDataGridView1.SelectedCells(0).RowIndex).Cells("project_no").Value & "'"


        sqlcmd.CommandText = sql
        k = sqlcmd.ExecuteScalar

        If k = 2 Then
            MsgBox("该单据已经结案，请检查后操作！", MsgBoxStyle.OkOnly, "提示")
            Exit Sub
        ElseIf k = 1 Then
            MsgBox("该单据已经审核，请检查后操作！", MsgBoxStyle.OkOnly, "提示")
            Exit Sub
        End If

        sql = "update  project_master set state=1,check_date=getdate(),check_man='" & rs_name & "' where project_no='" & Me.ExDataGridView1.Rows(Me.ExDataGridView1.SelectedCells(0).RowIndex).Cells("project_no").Value & "'"
        sqlcmd.CommandText = sql
        sqlcmd.ExecuteNonQuery()

        'Refresh_ExDataGridView(Me.TreeView1.SelectedNode.Text)
        Me.ExDataGridView1.Rows(Me.ExDataGridView1.SelectedCells(0).RowIndex).Cells("state1").Value = "审核"

        MsgBox("审核成功！", MsgBoxStyle.OkOnly, "提示")
    End Sub



    Private Sub 反审ToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles 反审ToolStripMenuItem.Click
        Dim sqlcon As New SqlConnection
        Dim sqlcmd As New SqlCommand

        Dim sql As String
        Dim k As int32

        sqlcon = getconn()
        sqlcmd.Connection = sqlcon

        sql = "SELECT state " & _
                "FROM project_master where project_no='" & Me.ExDataGridView1.Rows(Me.ExDataGridView1.SelectedCells(0).RowIndex).Cells("project_no").Value & "'"


        sqlcmd.CommandText = sql
        k = sqlcmd.ExecuteScalar

        If k = 2 Then
            MsgBox("该单据已经结案，请检查后操作！", MsgBoxStyle.OkOnly, "提示")
            Exit Sub
        ElseIf k = 0 Then
            MsgBox("该单据状态为新建，请检查后操作！", MsgBoxStyle.OkOnly, "提示")
            Exit Sub
        End If


        sql = "SELECT count(*) " & _
                       "FROM project_master where project_no='" & Me.ExDataGridView1.Rows(Me.ExDataGridView1.SelectedCells(0).RowIndex).Cells("project_no").Value & "' and fp_money>0"
        sqlcmd.CommandText = sql

        If sqlcmd.ExecuteScalar > 0 Then
            MsgBox("该单据已经有开票数量，请检查后操作！", MsgBoxStyle.OkOnly, "提示")
            Exit Sub
        End If

        sql = "update  project_master set state=0 where project_no='" & Me.ExDataGridView1.Rows(Me.ExDataGridView1.SelectedCells(0).RowIndex).Cells("project_no").Value & "'"
        sqlcmd.CommandText = sql
        sqlcmd.ExecuteNonQuery()

        'Refresh_ExDataGridView(Me.TreeView1.SelectedNode.Text)
        Me.ExDataGridView1.Rows(Me.ExDataGridView1.SelectedCells(0).RowIndex).Cells("state1").Value = "新建"
        MsgBox("反审核成功！", MsgBoxStyle.OkOnly, "提示")
    End Sub

    Private Sub 结案ToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles 结案ToolStripMenuItem.Click
        Dim sqlcon As New SqlConnection
        Dim sqlcmd As New SqlCommand

        Dim sql As String
        Dim k As int32

        sqlcon = getconn()
        sqlcmd.Connection = sqlcon

        sql = "SELECT state " & _
                "FROM project_master where project_no='" & Me.ExDataGridView1.Rows(Me.ExDataGridView1.SelectedCells(0).RowIndex).Cells("project_no").Value & "'"


        sqlcmd.CommandText = sql
        k = sqlcmd.ExecuteScalar

        If k = 2 Then
            MsgBox("该单据已经结案，请检查后操作！", MsgBoxStyle.OkOnly, "提示")
            Exit Sub
        End If

        sql = "update  project_master set state=2 where project_no='" & Me.ExDataGridView1.Rows(Me.ExDataGridView1.SelectedCells(0).RowIndex).Cells("project_no").Value & "'"
        sqlcmd.CommandText = sql
        sqlcmd.ExecuteNonQuery()

        'Refresh_ExDataGridView(Me.TreeView1.SelectedNode.Text)
        Me.ExDataGridView1.Rows(Me.ExDataGridView1.SelectedCells(0).RowIndex).Cells("state1").Value = "结案"
        MsgBox("结案成功！", MsgBoxStyle.OkOnly, "提示")
    End Sub



    Private Sub cellclick(ByVal index As int32)
        Dim sqlcon As New SqlConnection
        Dim sqlcmd As New SqlCommand
        Dim sqlad As New SqlDataAdapter

        Dim dt As New DataTable
        Dim sql As String
        Dim s As String = ""

        sqlcon = getconn()


        'sql = "select * from  project_detail where project_no='" & Me.ExDataGridView1.Rows(index).Cells("project_no").Value & "' order by id"
        If index <> -1 Then

            sql = "select * from  project_detail where project_no='" & Me.ExDataGridView1.Rows(index).Cells("project_no").Value & "' order by id"

        Else
            sql = "select * from  project_detail where id=-1 order by id"
        End If
        sqlcmd.Connection = sqlcon
        sqlcmd.CommandText = sql
        sqlad.SelectCommand = sqlcmd
        dt.Rows.Clear()
        sqlad.Fill(dt)

        Me.ExDataGridView2.SetDataSource(dt)
        Me.ExDataGridView2.Columns.Clear()




        Me.ExDataGridView2.AddColumn("tz_money", "增项金额", , , , DataGridViewContentAlignment.MiddleRight, "#0")
        Me.ExDataGridView2.AddColumn("tz_man", "增项人")
        Me.ExDataGridView2.AddColumn("tz_date", "增项时间")

        Me.ExDataGridView2.AddColumn("memo", "说明")


        Me.ExDataGridView2.AddColumn("id", "id", , , , , , False)
    End Sub

    Private Sub ExDataGridView2_CellContentClick(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles ExDataGridView2.CellContentClick

    End Sub

    Private Sub ExDataGridView1_CellClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles ExDataGridView1.CellClick
        If e.RowIndex < 0 Then
            Exit Sub
        Else
            cellclick(e.RowIndex)
        End If
    End Sub

    Private Sub ExDataGridView1_CellContentClick(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles ExDataGridView1.CellContentClick

    End Sub

    Private Sub 反结案ToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles 反结案ToolStripMenuItem.Click
        Dim sqlcon As New SqlConnection
        Dim sqlcmd As New SqlCommand

        Dim sql As String
        Dim k As int32

        sqlcon = getconn()
        sqlcmd.Connection = sqlcon

        sql = "SELECT state " & _
                "FROM project_master where project_no='" & Me.ExDataGridView1.Rows(Me.ExDataGridView1.SelectedCells(0).RowIndex).Cells("project_no").Value & "'"


        sqlcmd.CommandText = sql
        k = sqlcmd.ExecuteScalar

        If k = 1 Then
            MsgBox("该单据状态为审核，请检查后操作！", MsgBoxStyle.OkOnly, "提示")
            Exit Sub
        ElseIf k = 0 Then
            MsgBox("该单据状态为新建，请检查后操作！", MsgBoxStyle.OkOnly, "提示")
            Exit Sub
        End If

        sql = "update  project_master set state=1,check_date=getdate(),check_man='" & rs_name & "' where project_no='" & Me.ExDataGridView1.Rows(Me.ExDataGridView1.SelectedCells(0).RowIndex).Cells("project_no").Value & "'"
        sqlcmd.CommandText = sql
        sqlcmd.ExecuteNonQuery()

        'Refresh_ExDataGridView(Me.TreeView1.SelectedNode.Text)
        Me.ExDataGridView1.Rows(Me.ExDataGridView1.SelectedCells(0).RowIndex).Cells("state1").Value = "审核"
        MsgBox("结案成功！", MsgBoxStyle.OkOnly, "提示")
    End Sub

    Private Sub ToolStripButton_show_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripButton_show.Click
        Dim printView As New printView

        Dim CG_RPT As New CG_RPT

        Dim sqlcon As New SqlConnection
        Dim sqlcmd As New SqlCommand


        Dim sql As String



        If Me.ExDataGridView1.SelectedCells.Count = 0 Then
            MsgBox("请选择要打印的单据", MsgBoxStyle.OkOnly, "提示")
            Exit Sub
        End If

        sqlcon = getconn()
        sqlcmd.Connection = sqlcon

        sql = "select  count(*) from project_master where state=1 and project_no='" & Me.ExDataGridView1.Rows(Me.ExDataGridView1.SelectedCells(0).RowIndex).Cells("project_no").Value & "'"
        sqlcmd.CommandText = sql


        If sqlcmd.ExecuteScalar = 0 Then
            MsgBox("该单据不是审核状态，不可以打印！", MsgBoxStyle.OkOnly, "提示")
            Exit Sub
        End If

        ShowCG_RPT(CG_RPT, Me.ExDataGridView1.Rows(Me.ExDataGridView1.SelectedCells(0).RowIndex).Cells("project_no").Value)

        printView.CrystalReportViewer1.ReportSource = CG_RPT
        printView.CrystalReportViewer1.RefreshReport()
        printView.ShowDialog()
    End Sub


    Private Sub ShowCG_RPT(ByRef CG_RPT As CG_RPT, ByVal billno As String)
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

        CG_RPT.PrintOptions.PaperSize = rawKind

        If rawKind = 11 Then
            CG_RPT.PrintOptions.PaperOrientation = CrystalDecisions.Shared.PaperOrientation.Landscape
        Else
            CG_RPT.PrintOptions.PaperOrientation = CrystalDecisions.Shared.PaperOrientation.Portrait
        End If





        ' CType(CrystalDecisions.Shared.PaperSize rawKind)

        sql = "SELECT a.*,b.customer_name,'" & rs_name & "' as print_man,getdate() as print_date " & _
              "FROM project_master a left join " & _
              " t_customer b on a.customer_no=b.customer_no" & _
              " where a.project_no='" & billno & "'"

        sqlcmd.CommandText = sql
        sqlad.SelectCommand = sqlcmd
        sqlad.Fill(ds, "project_master")


        sql = "select top 1 a.*   " & _
                     "from project_detail a " & _
                     " where a.project_no='" & billno & "' order by a.id desc "
        sqlcmd.CommandText = sql
        sqlad.SelectCommand = sqlcmd
        sqlad.Fill(ds, "project_detail")

        CG_RPT.SetDataSource(ds)
        CG_RPT.Refresh()

        save_pdf(CG_RPT, billno)
    End Sub

    Private Sub ToolStripButton_print_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripButton_print.Click

        Dim CG_RPT As New CG_RPT

        Dim sqlcon As New SqlConnection
        Dim sqlcmd As New SqlCommand


        Dim sql As String



        If Me.ExDataGridView1.SelectedCells.Count = 0 Then
            MsgBox("请选择要打印的单据", MsgBoxStyle.OkOnly, "提示")
            Exit Sub
        End If

        sqlcon = getconn()
        sqlcmd.Connection = sqlcon

        sql = "select  count(*) from project_master where state=1 and project_no='" & Me.ExDataGridView1.Rows(Me.ExDataGridView1.SelectedCells(0).RowIndex).Cells("project_no").Value & "'"
        sqlcmd.CommandText = sql


        If sqlcmd.ExecuteScalar = 0 Then
            MsgBox("该单据不是审核状态，不可以打印！", MsgBoxStyle.OkOnly, "提示")
            Exit Sub
        End If

        ShowCG_RPT(CG_RPT, Me.ExDataGridView1.Rows(Me.ExDataGridView1.SelectedCells(0).RowIndex).Cells("project_no").Value)
        CG_RPT.PrintToPrinter(1, True, 0, 0)

        MsgBox("打印完成！", MsgBoxStyle.OkOnly, "提示")

    End Sub

    Private Sub 延期ToolStripMenuItem1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles 延期ToolStripMenuItem1.Click
        Dim project_YQ As New project_YQ
        project_YQ.billno = Me.ExDataGridView1.Rows(Me.ExDataGridView1.SelectedCells(0).RowIndex).Cells("project_no").Value
        project_YQ.DateTimePicker1.Value = Me.ExDataGridView1.Rows(Me.ExDataGridView1.SelectedCells(0).RowIndex).Cells("effective_date").Value

        project_YQ.Owner = Me
        project_YQ.ShowDialog()
    End Sub
End Class