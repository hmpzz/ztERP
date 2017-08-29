Imports System.Data.SqlClient
Public Class t_FPYS_check

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



        If s1.Length <> 0 Then
            s1 = Microsoft.VisualBasic.Left(s1, s1.Length - 3)
            s1 = " and (" & s1 & ")"
        End If


        sql = "select a.*,case state when 0 then '新建' when 1 then '审核' when 2 then '结案' end as state1, case hx when 0 then '未核销' when 1 then '已核销' end as hx1,case kp_state when 0 then '未开票' when 1 then '已开票' end as kp_state1  " & _
                "from t_fpys_master a  " & _
                " where 1=1 " & s & s1 & "  order by a.billno "

        sqlcmd.CommandText = sql
        sqlad.SelectCommand = sqlcmd
        dt.Rows.Clear()
        sqlad.Fill(dt)

        Me.ExDataGridView1.SetDataSource(dt)
        Me.ExDataGridView1.Columns.Clear()

        'Me.ExDataGridView1.AddColumn("check", "", 20, False, New DataGridViewCheckBoxColumn)
        Me.ExDataGridView1.AddColumn("billno", "应收票据号", 120)
        Me.ExDataGridView1.AddColumn("FP_NO", "发票号")

        Me.ExDataGridView1.AddColumn("memo", "说明")

        Me.ExDataGridView1.AddColumn("state1", "状态")

        Me.ExDataGridView1.AddColumn("kp_state1", "开票状态")
        Me.ExDataGridView1.AddColumn("kp_type", "发票类型")

        Me.ExDataGridView1.AddColumn("HX1", "核销状态")
        Me.ExDataGridView1.AddColumn("HX_man", "核销人")
        Me.ExDataGridView1.AddColumn("hx_date", "核销日期", , , , , "yyyy-MM-dd")

        Me.ExDataGridView1.AddColumn("input_man", "录入人")
        Me.ExDataGridView1.AddColumn("input_date", "录入日期", , , , , "yyyy-MM-dd")
        Me.ExDataGridView1.AddColumn("check_man", "审核人")
        Me.ExDataGridView1.AddColumn("check_date", "审核日期", , , , , "yyyy-MM-dd")

        Me.ExDataGridView1.AddColumn("customer_no", "customer_no", , , , , , False)


        Me.ExDataGridView1.AddColumn("HW_name1", "HW_name1", , , , , , False)
        Me.ExDataGridView1.AddColumn("HW_unit1", "HW_unit1", , , , , , False)
        Me.ExDataGridView1.AddColumn("HW_price1", "HW_price1", , , , , , False)
        Me.ExDataGridView1.AddColumn("HW_amount1", "HW_amount1", , , , , "#0.00", False)
        Me.ExDataGridView1.AddColumn("HW_money1", "HW_money1", , , , , "#0.00", False)


        Me.ExDataGridView1.AddColumn("HW_name2", "HW_name2", , , , , , False)
        Me.ExDataGridView1.AddColumn("HW_unit2", "HW_unit2", , , , , , False)
        Me.ExDataGridView1.AddColumn("HW_price2", "HW_price2", , , , , , False)
        Me.ExDataGridView1.AddColumn("HW_amount2", "HW_amount2", , , , , "#0.00", False)
        Me.ExDataGridView1.AddColumn("HW_money2", "HW_money2", , , , , "#0.00", False)


        Me.ExDataGridView1.AddColumn("HW_name3", "HW_name3", , , , , , False)
        Me.ExDataGridView1.AddColumn("HW_unit3", "HW_unit3", , , , , , False)
        Me.ExDataGridView1.AddColumn("HW_price3", "HW_price3", , , , , , False)
        Me.ExDataGridView1.AddColumn("HW_amount3", "HW_amount3", , , , , "#0.00", False)
        Me.ExDataGridView1.AddColumn("HW_money3", "HW_money3", , , , , "#0.00", False)


        Me.ExDataGridView1.AddColumn("HW_name4", "HW_name4", , , , , , False)
        Me.ExDataGridView1.AddColumn("HW_unit4", "HW_unit4", , , , , , False)
        Me.ExDataGridView1.AddColumn("HW_price4", "HW_price4", , , , , , False)
        Me.ExDataGridView1.AddColumn("HW_amount4", "HW_amount4", , , , , "#0.00", False)
        Me.ExDataGridView1.AddColumn("HW_money4", "HW_money4", , , , , "#0.00", False)


        Me.ExDataGridView1.AddColumn("HW_name5", "HW_name5", , , , , , False)
        Me.ExDataGridView1.AddColumn("HW_unit5", "HW_unit5", , , , , , False)
        Me.ExDataGridView1.AddColumn("HW_price5", "HW_price5", , , , , , False)
        Me.ExDataGridView1.AddColumn("HW_amount5", "HW_amount5", , , , , "#0.00", False)
        Me.ExDataGridView1.AddColumn("HW_money5", "HW_money5", , , , , "#0.00", False)

        Me.ExDataGridView1.AddColumn("HW_name6", "HW_name6", , , , , , False)
        Me.ExDataGridView1.AddColumn("HW_unit6", "HW_unit6", , , , , , False)
        Me.ExDataGridView1.AddColumn("HW_price6", "HW_price6", , , , , , False)
        Me.ExDataGridView1.AddColumn("HW_amount6", "HW_amount6", , , , , "#0.00", False)
        Me.ExDataGridView1.AddColumn("HW_money6", "HW_money6", , , , , "#0.00", False)

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
            sql = "select a.*,b.project_no,b.project_name,b.project_money,b.fp_money,b.effective_date,b.memo " & _
                    "from t_fpys_detail a left join  " & _
                    "	project_master b on a.project_no=b.project_no " & _
                    "  where a.billno='" & Me.ExDataGridView1.Rows(index).Cells("billno").Value & "' order by a.id "
        Else
            sql = "select a.*,b.project_no,b.project_name,b.project_money,b.fp_money,b.effective_date,b.memo " & _
                    "from t_fpys_detail a left join  " & _
                    "	project_master b on a.project_no=b.project_no " & _
                    "  where a.id=-1  order by a.id "
        End If
        sqlcmd.Connection = sqlcon
        sqlcmd.CommandText = sql
        sqlad.SelectCommand = sqlcmd
        dt.Rows.Clear()
        sqlad.Fill(dt)

        Me.ExDataGridView2.SetDataSource(dt)
        Me.ExDataGridView2.Columns.Clear()




        Me.ExDataGridView2.AddColumn("project_no", "工程号", 120)
        Me.ExDataGridView2.AddColumn("project_name", "工程名称")
        Me.ExDataGridView2.AddColumn("project_money", "工程金额", , , , , "#0")
        Me.ExDataGridView2.AddColumn("money", "发票金额", , , , , "#0")


        Me.ExDataGridView2.AddColumn("effective_date", "有效日期", , , , , "yyyy-MM-dd")


        Me.ExDataGridView2.AddColumn("memo", "备注")

        Me.ExDataGridView2.AddColumn("id", "id", , , , , , False)
    End Sub

    Private Sub 审核ToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles 审核ToolStripMenuItem.Click
        Dim sqlcon As New SqlConnection
        Dim sqlcmd As New SqlCommand

        Dim sql As String
        Dim k As Int32

        sqlcon = getconn()
        sqlcmd.Connection = sqlcon

        sql = "SELECT state " & _
                "FROM T_FPYS_master where billno='" & Me.ExDataGridView1.Rows(Me.ExDataGridView1.SelectedCells(0).RowIndex).Cells("billno").Value & "'"


        sqlcmd.CommandText = sql
        k = sqlcmd.ExecuteScalar

        If k = 1 Then
            MsgBox("该单据已经审核，请检查后操作！", MsgBoxStyle.OkOnly, "提示")
            Exit Sub
        End If

        sql = "update  T_FPYS_master set state=1,check_date=getdate(),check_man='" & rs_name & "' where billno='" & Me.ExDataGridView1.Rows(Me.ExDataGridView1.SelectedCells(0).RowIndex).Cells("billno").Value & "'"
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
        Dim k As Int32

        sqlcon = getconn()
        sqlcmd.Connection = sqlcon

        sql = "SELECT state " & _
                "FROM T_FPYS_master where billno='" & Me.ExDataGridView1.Rows(Me.ExDataGridView1.SelectedCells(0).RowIndex).Cells("billno").Value & "'"


        sqlcmd.CommandText = sql
        k = sqlcmd.ExecuteScalar

        If k = 0 Then
            MsgBox("该单据状态为新建，请检查后操作！", MsgBoxStyle.OkOnly, "提示")
            Exit Sub
        End If

        sql = "SELECT count(*) " & _
                    "FROM T_FPYS_master where billno='" & Me.ExDataGridView1.Rows(Me.ExDataGridView1.SelectedCells(0).RowIndex).Cells("billno").Value & "' and HX=1"
        sqlcmd.CommandText = sql
        If sqlcmd.ExecuteScalar > 0 Then
            MsgBox("该单据已经核销不能反审，请检查后操作！", MsgBoxStyle.OkOnly, "提示")
            Exit Sub
        End If




        sql = "update  T_FPYS_master set state=0 where billno='" & Me.ExDataGridView1.Rows(Me.ExDataGridView1.SelectedCells(0).RowIndex).Cells("billno").Value & "'"
        sqlcmd.CommandText = sql
        sqlcmd.ExecuteNonQuery()

        'Refresh_ExDataGridView(Me.TreeView1.SelectedNode.Text)
        Me.ExDataGridView1.Rows(Me.ExDataGridView1.SelectedCells(0).RowIndex).Cells("state1").Value = "新建"
        MsgBox("反审核成功！", MsgBoxStyle.OkOnly, "提示")
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
End Class