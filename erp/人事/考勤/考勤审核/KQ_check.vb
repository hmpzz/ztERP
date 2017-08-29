Imports System.Data.SqlClient
Public Class KQ_check

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



        If BM_id = 49 Or BM_id = 78 Then
            sql = "SELECT a.*,case state when 0 then '新建' when 1 then '审核' when 2 then '结案' end as state1,b.rs_name,c.sy_name " & _
                     "FROM t_kq a left join " & _
                     " t_rs b on a.rs_no=b.rs_no left join " & _
                     "t_sy c on a.sy_no=c.sy_no " & _
                     "where 1=1 " & s & s1 & "  order by a.billno "
        Else
            sql = "SELECT a.*,case state when 0 then '新建' when 1 then '审核' when 2 then '结案' end as state1,b.rs_name,c.sy_name " & _
                    "FROM t_kq a left join " & _
                    " t_rs b on a.rs_no=b.rs_no left join " & _
                    "t_sy c on a.sy_no=c.sy_no left join " & _
                       " t_rs on a.input_man=t_rs.rs_name " & _
                    "where t_rs.bm_id= " & BM_id & s & s1 & "  order by a.billno "
        End If



        sqlcmd.CommandText = sql
        sqlad.SelectCommand = sqlcmd
        dt.Rows.Clear()
        sqlad.Fill(dt)

        Me.ExDataGridView1.SetDataSource(dt)
        Me.ExDataGridView1.Columns.Clear()

        Me.ExDataGridView1.AddColumn("check", "", 20, False, New DataGridViewCheckBoxColumn)
        Me.ExDataGridView1.AddColumn("billno", "考勤单号")
        Me.ExDataGridView1.AddColumn("rs_no", "人员编号")
        Me.ExDataGridView1.AddColumn("rs_name", "人员姓名")
        Me.ExDataGridView1.AddColumn("FS_date", "日期")
        Me.ExDataGridView1.AddColumn("sj", "时间")
        Me.ExDataGridView1.AddColumn("sy_name", "事由")
        Me.ExDataGridView1.AddColumn("state1", "状态")

        Me.ExDataGridView1.AddColumn("memo", "说明")


        Me.ExDataGridView1.AddColumn("input_man", "录入人")
        Me.ExDataGridView1.AddColumn("input_date", "录入日期", , , , , "yyyy-MM-dd")

        Me.ExDataGridView1.AddColumn("check_man", "审核人")
        Me.ExDataGridView1.AddColumn("check_date", "审核日期", , , , , "yyyy-MM-dd")


        Me.ExDataGridView1.AddColumn("sy_no", "sy_no", , , , , , False)
        Me.ExDataGridView1.AddColumn("id", "id", , , , , , False)


    End Sub

    Private Sub ToolStripButton_exit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripButton_exit.Click
        Me.Close()
    End Sub

    Private Sub 审核ToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles 审核ToolStripMenuItem.Click
        Dim sqlcon As New SqlConnection
        Dim sqlcmd As New SqlCommand

        Dim sql As String
        Dim k As int32

        If Me.ExDataGridView1.SelectedCells.Count = 0 Then
            MsgBox("请选择要审核的单据！", MsgBoxStyle.OkOnly, "提示")
            Exit Sub
        End If

        sqlcon = getconn()
        sqlcmd.Connection = sqlcon

        sql = "SELECT state " & _
                "FROM t_kq where billno='" & Me.ExDataGridView1.Rows(Me.ExDataGridView1.SelectedCells(0).RowIndex).Cells("billno").Value & "'"


        sqlcmd.CommandText = sql
        k = sqlcmd.ExecuteScalar

        If k = 2 Then
            MsgBox("该单据已经结案，请检查后操作！", MsgBoxStyle.OkOnly, "提示")
            Exit Sub
        ElseIf k = 1 Then
            MsgBox("该单据已经审核，请检查后操作！", MsgBoxStyle.OkOnly, "提示")
            Exit Sub
        End If

        sql = "update  t_kq set state=1,check_date=getdate(),check_man='" & rs_name & "' where billno='" & Me.ExDataGridView1.Rows(Me.ExDataGridView1.SelectedCells(0).RowIndex).Cells("billno").Value & "'"
        sqlcmd.CommandText = sql
        sqlcmd.ExecuteNonQuery()

        'Refresh_ExDataGridView(Me.TreeView1.SelectedNode.Text)
        Me.ExDataGridView1.Rows(Me.ExDataGridView1.SelectedCells(0).RowIndex).Cells("state1").Value = "审核"

        Me.ExDataGridView1.Rows(Me.ExDataGridView1.SelectedCells(0).RowIndex).Cells("check_man").Value = rs_name
        Me.ExDataGridView1.Rows(Me.ExDataGridView1.SelectedCells(0).RowIndex).Cells("check_date").Value = Now

        MsgBox("审核成功！", MsgBoxStyle.OkOnly, "提示")
    End Sub

    Private Sub 反审ToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles 反审ToolStripMenuItem.Click
        Dim sqlcon As New SqlConnection
        Dim sqlcmd As New SqlCommand

        Dim sql As String
        Dim k As int32

        If Me.ExDataGridView1.SelectedCells.Count = 0 Then
            MsgBox("请选择要反审的单据！", MsgBoxStyle.OkOnly, "提示")
            Exit Sub
        End If

        sqlcon = getconn()
        sqlcmd.Connection = sqlcon

        sql = "SELECT state " & _
                "FROM t_kq where billno='" & Me.ExDataGridView1.Rows(Me.ExDataGridView1.SelectedCells(0).RowIndex).Cells("billno").Value & "'"


        sqlcmd.CommandText = sql
        k = sqlcmd.ExecuteScalar

        If k = 2 Then
            MsgBox("该单据已经结案，请检查后操作！", MsgBoxStyle.OkOnly, "提示")
            Exit Sub
        ElseIf k = 0 Then
            MsgBox("该单据状态为新建，请检查后操作！", MsgBoxStyle.OkOnly, "提示")
            Exit Sub
        End If






        sql = "update  t_kq set state=0 where billno='" & Me.ExDataGridView1.Rows(Me.ExDataGridView1.SelectedCells(0).RowIndex).Cells("billno").Value & "'"
        sqlcmd.CommandText = sql
        sqlcmd.ExecuteNonQuery()

        'Refresh_ExDataGridView(Me.TreeView1.SelectedNode.Text)
        Me.ExDataGridView1.Rows(Me.ExDataGridView1.SelectedCells(0).RowIndex).Cells("state1").Value = "新建"

        MsgBox("反审核成功！", MsgBoxStyle.OkOnly, "提示")
    End Sub

    Private Sub ToolStripButton_question_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripButton_question.Click
        Dim kq_cha As New KQ_cha
        Me.Tag = ""
        kq_cha.Owner = Me
        If kq_cha.ShowDialog() = Windows.Forms.DialogResult.OK Then

            Refresh_ExDataGridView_master(Me.Tag)
            Me.Tag = ""
        End If
    End Sub

    Private Sub ToolStripButton_excel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripButton_excel.Click
        Dim ToExcel As New BaseClass.TOEXCEL

        If ToExcel.ExToExcel(Me.ExDataGridView1, Me.SaveFileDialog1, True, "当前库存", "期初库存") Then
            MsgBox("导出成功！", MsgBoxStyle.OkOnly, "提示")
        End If
    End Sub

    Private Sub 全选ToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles 全选ToolStripMenuItem.Click
        Dim i As int32

        For i = 0 To Me.ExDataGridView1.Rows.Count - 1
            Me.ExDataGridView1.Rows(i).Cells("check").Value = True
        Next
    End Sub

    Private Sub 反选ToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles 反选ToolStripMenuItem.Click
        Dim i As int32

        For i = 0 To Me.ExDataGridView1.Rows.Count - 1
            Me.ExDataGridView1.Rows(i).Cells("check").Value = Not Me.ExDataGridView1.Rows(i).Cells("check").Value
        Next
    End Sub

    Private Sub ToolStripButton_save_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripButton_save.Click
        Dim sqlcon As New SqlConnection
        Dim sqlcmd As New SqlCommand
        Dim mytrans As SqlTransaction

        Dim sql As String


        Dim i As int32

        sqlcon = getconn()
        sqlcmd.Connection = sqlcon

        If Me.ExDataGridView1.Rows.Count > 0 Then
            Me.ExDataGridView1.Rows(0).Cells(1).Selected = True
        End If

        mytrans = sqlcon.BeginTransaction
        sqlcmd.Transaction = mytrans
        Try


            For i = 0 To Me.ExDataGridView1.Rows.Count - 1
                If Me.ExDataGridView1.Rows(i).Cells("check").Value = True Then
                    sql = "SELECT state " & _
                                     "FROM t_kq where billno='" & Me.ExDataGridView1.Rows(i).Cells("billno").Value & "'"

                    sqlcmd.CommandText = sql
                    If sqlcmd.ExecuteScalar = 0 Then
                        sql = "update  t_kq set state=1,check_date=getdate(),check_man='" & rs_name & "' where billno='" & Me.ExDataGridView1.Rows(i).Cells("billno").Value & "'"
                        sqlcmd.CommandText = sql
                        sqlcmd.ExecuteNonQuery()

                        Me.ExDataGridView1.Rows(i).Cells("state1").Value = "审核"

                        Me.ExDataGridView1.Rows(i).Cells("check_man").Value = rs_name
                        Me.ExDataGridView1.Rows(i).Cells("check_date").Value = Now
                    End If


                End If
            Next

            mytrans.Commit()
        Catch ex As Exception
            mytrans.Rollback()
            MsgBox(ex.Message, MsgBoxStyle.OkOnly, "错误")
            Exit Sub
        End Try

        MsgBox("审核成功！", MsgBoxStyle.OkOnly, "提示")
    End Sub
End Class