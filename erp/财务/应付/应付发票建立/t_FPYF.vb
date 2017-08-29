Imports System.Data.SqlClient
Public Class t_FPYF
    Public cha As String
    Private Sub ToolStripButton_add_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripButton_add.Click
        Dim T_FPYF_add As New T_FPYF_add
        T_FPYF_add.state = "new"
        T_FPYF_add.billno = ""
        T_FPYF_add.Owner = Me
        T_FPYF_add.ShowDialog()
        Refresh_ExDataGridView_master()
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



        If s1.Length <> 0 Then
            s1 = Microsoft.VisualBasic.Left(s1, s1.Length - 3)
            s1 = " and (" & s1 & ")"
        End If


        sql = "select a.*,case state when 0 then '新建' when 1 then '审核' when 2 then '结案' end as state1, case hx when 0 then '未核销' when 1 then '已核销' end as hx1,case kp_state when 0 then '未开票' when 1 then '已开票' end as kp_state1  " & _
                ",CASE FP_type when 0 then '入库单' when 1 then '加工费' end as FP_type1,b.money  " & _
                "from t_fpyF_master a left join  " & _
                "    (select billno,sum(money) as money from  t_fpyF_detail group by billno) b on a.billno=b.billno  " & _
                " where 1=1 " & s & s1 & "  order by a.billno "

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

        Me.ExDataGridView1.AddColumn("supplier_name", "供应商名称")

        Me.ExDataGridView1.AddColumn("state1", "状态")
        Me.ExDataGridView1.AddColumn("FP_TYPE1", "发票来源")

        Me.ExDataGridView1.AddColumn("kp_state1", "开票状态")
        Me.ExDataGridView1.AddColumn("kp_type", "发票类型")




        Me.ExDataGridView1.AddColumn("HX1", "核销状态")
        Me.ExDataGridView1.AddColumn("HX_man", "核销人")
        Me.ExDataGridView1.AddColumn("hx_date", "核销日期", , , , , "yyyy-MM-dd")

        Me.ExDataGridView1.AddColumn("input_man", "录入人")
        Me.ExDataGridView1.AddColumn("input_date", "录入日期", , , , , "yyyy-MM-dd")
        Me.ExDataGridView1.AddColumn("check_man", "审核人")
        Me.ExDataGridView1.AddColumn("check_date", "审核日期", , , , , "yyyy-MM-dd")


        Me.ExDataGridView1.AddColumn("FP_TYPE", "FP_TYPE", , , , , , False)
       

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

    Private Sub ExDataGridView1_CellClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles ExDataGridView1.CellClick
        If e.RowIndex < 0 Then
            Exit Sub
        Else
            cellclick(e.RowIndex)
        End If
    End Sub

   
    Private Sub ToolStripButton_exit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripButton_exit.Click
        Me.Close()
    End Sub

    Private Sub ToolStripButton_mod_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripButton_mod.Click
        Dim T_FPYF_add As New T_FPYF_add

        Dim sqlcon As New SqlConnection
        Dim sqlcmd As New SqlCommand
        Dim sql As String = "'"




        If Me.ExDataGridView1.SelectedCells.Count < 0 Then
            MsgBox("请选择要修改的行！", MsgBoxStyle.OkOnly, "提示")
            Exit Sub
        End If

        sqlcon = getconn()

        sql = "select count(*) from t_fpyF_master where state=0 and billno='" & Me.ExDataGridView1.Rows(Me.ExDataGridView1.SelectedCells(0).RowIndex).Cells("billno").Value & "' "
        sqlcmd.Connection = sqlcon
        sqlcmd.CommandText = sql
        If sqlcmd.ExecuteScalar() = 0 Then
            MsgBox("该记录已经被删除或者是审核，请刷新后再试！", MsgBoxStyle.OkOnly, "提示")
            Exit Sub
        End If





        T_FPYF_add.Owner = Me
        T_FPYF_add.state = "mod"

        T_FPYF_add.billno = Me.ExDataGridView1.Rows(Me.ExDataGridView1.SelectedCells.Item(0).RowIndex).Cells("billno").Value
        T_FPYF_add.ShowDialog()

        sql = "select sum(money) from t_fpyF_detail where  billno='" & Me.ExDataGridView1.Rows(Me.ExDataGridView1.SelectedCells(0).RowIndex).Cells("billno").Value & "' group by billno "
        sqlcmd.Connection = sqlcon
        sqlcmd.CommandText = sql

        Me.ExDataGridView1.Rows(Me.ExDataGridView1.SelectedCells.Item(0).RowIndex).Cells("money").Value = sqlcmd.ExecuteScalar()
        cellclick(Me.ExDataGridView1.SelectedCells.Item(0).RowIndex)
    End Sub

    Private Sub ToolStripButton_del_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripButton_del.Click
        Dim sqlcon As New SqlConnection
        Dim sqlcmd As New SqlCommand
        Dim sql As String = "'"
        Dim mytrans As SqlTransaction



        If Me.ExDataGridView1.SelectedCells.Count < 0 Then
            MsgBox("请选择要修改的行！", MsgBoxStyle.OkOnly, "提示")
            Exit Sub
        End If

        If MsgBox("确定要删除单据号为:" & Me.ExDataGridView1.Rows(Me.ExDataGridView1.SelectedCells(0).RowIndex).Cells("billno").Value & "的单据吗？", MsgBoxStyle.YesNo, "提示") = MsgBoxResult.No Then
            Exit Sub
        End If

        sqlcon = getconn()
        sqlcmd.Connection = sqlcon

        mytrans = sqlcon.BeginTransaction
        sqlcmd.Transaction = mytrans

        sql = "select count(*) from t_fpyF_master where state=0 and billno='" & Me.ExDataGridView1.Rows(Me.ExDataGridView1.SelectedCells(0).RowIndex).Cells("billno").Value & "' "

        sqlcmd.CommandText = sql
        If sqlcmd.ExecuteScalar() = 0 Then
            MsgBox("该记录已经被删除或者是审核，请刷新后再试！", MsgBoxStyle.OkOnly, "提示")
            Exit Sub
        End If


        Try

         

            If Me.ExDataGridView1.Rows(Me.ExDataGridView1.SelectedCells(0).RowIndex).Cells("FP_type").Value = 0 Then
                sql = "update b set KP_money=KP_money- a.money " & _
                          "from T_FPYF_detail a inner join  " & _
                          "	t_stockbill_master b on a.RK_billno=b.billno " & _
                          "where a.billno='" & Me.ExDataGridView1.Rows(Me.ExDataGridView1.SelectedCells(0).RowIndex).Cells("billno").Value & "'"
                sqlcmd.CommandText = sql
                sqlcmd.ExecuteNonQuery()


                sql = "update b set KP_money=KP_money- a.money " & _
                        "from T_FPYF_detail a inner join  " & _
                        "	t_stockbill_master_history b on a.RK_billno=b.billno " & _
                        "where a.billno='" & Me.ExDataGridView1.Rows(Me.ExDataGridView1.SelectedCells(0).RowIndex).Cells("billno").Value & "'"
                sqlcmd.CommandText = sql
                sqlcmd.ExecuteNonQuery()


            ElseIf Me.ExDataGridView1.Rows(Me.ExDataGridView1.SelectedCells(0).RowIndex).Cells("FP_type").Value = 1 Then

                sql = "update b set KP_money=KP_money- a.money " & _
                        "from T_FPYF_detail a inner join  " & _
                        "	t_outwork_master b on a.RK_billno=b.billno " & _
                        "where a.billno='" & Me.ExDataGridView1.Rows(Me.ExDataGridView1.SelectedCells(0).RowIndex).Cells("billno").Value & "'"
                sqlcmd.CommandText = sql
                sqlcmd.ExecuteNonQuery()

            End If



            sql = "delete T_FPYF_master  where billno='" & Me.ExDataGridView1.Rows(Me.ExDataGridView1.SelectedCells(0).RowIndex).Cells("billno").Value & "' "
            sqlcmd.CommandText = sql
            sqlcmd.ExecuteNonQuery()

            sql = "delete T_FPYF_detail  where billno='" & Me.ExDataGridView1.Rows(Me.ExDataGridView1.SelectedCells(0).RowIndex).Cells("billno").Value & "' "
            sqlcmd.CommandText = sql
            sqlcmd.ExecuteNonQuery()
            mytrans.Commit()

        Catch ex As Exception
            mytrans.Rollback()
            MsgBox(ex.Message, MsgBoxStyle.OkOnly, "错误")
            Exit Sub
        End Try

        MsgBox("保存成功！", MsgBoxStyle.OkOnly, "提示")
        Refresh_ExDataGridView_master()
    End Sub

    Private Sub ExDataGridView1_CellContentClick(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles ExDataGridView1.CellContentClick

    End Sub

    Private Sub ToolStripButton_question_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripButton_question.Click
        Dim FPYF_cha As New FPYF_cha
        FPYF_cha.Owner = Me
        Me.Tag = ""
        FPYF_cha.ShowDialog()

        If FPYF_cha.DialogResult = Windows.Forms.DialogResult.OK Then
            Refresh_ExDataGridView_master(Me.Tag)
            Me.Tag = ""
        Else
            Exit Sub
        End If
    End Sub
End Class