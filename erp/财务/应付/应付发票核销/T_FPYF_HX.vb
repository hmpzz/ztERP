Imports System.Data.SqlClient
Public Class T_FPYF_HX

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

      



        If s1.Length <> 0 Then
            s1 = Microsoft.VisualBasic.Left(s1, s1.Length - 3)
            s1 = " and (" & s1 & ")"
        End If


        sql = "select a.*,case state when 0 then '新建' when 1 then '审核' when 2 then '结案' end as state1, case hx when 0 then '未核销' when 1 then '已核销' end as hx1,case kp_state when 0 then '未开票' when 1 then '已开票' end as kp_state1,b.money  " & _
                "from t_fpyF_master a  left join  " & _
                "    (select billno,sum(money) as money from  t_fpyF_detail group by billno) b on a.billno=b.billno " & _
                " where hx=0 and a.state=1 " & s & s1 & "   order by a.billno "

        sqlcmd.CommandText = sql
        sqlad.SelectCommand = sqlcmd
        dt.Rows.Clear()
        sqlad.Fill(dt)

        Me.ExDataGridView1.SetDataSource(dt)
        Me.ExDataGridView1.Columns.Clear()

        'Me.ExDataGridView1.AddColumn("check", "", 20, False, New DataGridViewCheckBoxColumn)
        Me.ExDataGridView1.AddColumn("billno", "应付票据号", 120)
        Me.ExDataGridView1.AddColumn("FP_NO", "发票号")
        Me.ExDataGridView1.AddColumn("money", "金额总计", , , , DataGridViewContentAlignment.BottomRight, "#0.00")
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
            sql = "select a.* " & _
                    "from t_fpyF_detail a " & _
                    "  where a.billno='" & Me.ExDataGridView1.Rows(index).Cells("billno").Value & "' order by a.id "
        Else
            sql = "select a.* " & _
                    "from t_fpyF_detail a " & _
                    "  where a.id=-1  order by a.id "
        End If
        sqlcmd.Connection = sqlcon
        sqlcmd.CommandText = sql
        sqlad.SelectCommand = sqlcmd
        dt.Rows.Clear()
        sqlad.Fill(dt)

        Me.ExDataGridView2.SetDataSource(dt)
        Me.ExDataGridView2.Columns.Clear()




        Me.ExDataGridView2.AddColumn("billno", "单据号", 120)
        Me.ExDataGridView2.AddColumn("RK_billno", "入库单号", 120)

        Me.ExDataGridView2.AddColumn("money", "发票金额", , , , , "#0.00")


        Me.ExDataGridView2.AddColumn("id", "id", , , , , , False)
    End Sub

    Private Sub ToolStripButton_save_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripButton_save.Click
        Dim sqlcon As New SqlConnection
        Dim sqlcmd As New SqlCommand
        Dim sql As String = "'"




        If Me.ExDataGridView1.SelectedCells.Count < 0 Then
            MsgBox("请选择要修改的行！", MsgBoxStyle.OkOnly, "提示")
            Exit Sub
        End If

        If MsgBox("确定要核销单据号为:" & Me.ExDataGridView1.Rows(Me.ExDataGridView1.SelectedCells(0).RowIndex).Cells("billno").Value & "的单据吗？", MsgBoxStyle.YesNo, "提示") = MsgBoxResult.No Then
            Exit Sub
        End If

        sqlcon = getconn()
        sqlcmd.Connection = sqlcon



        sql = "select count(*) from t_fpyF_master where state=1 and billno='" & Me.ExDataGridView1.Rows(Me.ExDataGridView1.SelectedCells(0).RowIndex).Cells("billno").Value & "' "

        sqlcmd.CommandText = sql
        If sqlcmd.ExecuteScalar() = 0 Then
            MsgBox("该记录状态不是审核，请刷新后再试！", MsgBoxStyle.OkOnly, "提示")
            Exit Sub
        End If






        sql = "update T_FPYF_master set hx=1,hx_man='" & rs_name & "',hx_date=getdate() where billno='" & Me.ExDataGridView1.Rows(Me.ExDataGridView1.SelectedCells(0).RowIndex).Cells("billno").Value & "' "
        sqlcmd.CommandText = sql
        sqlcmd.ExecuteNonQuery()



        MsgBox("保存成功！", MsgBoxStyle.OkOnly, "提示")
        Refresh_ExDataGridView_master()
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