
Imports System.Data.SqlClient
Public Class project_WG



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
            sql = "SELECT a.*,case state when 0 then '新建' when 1 then '审核' when 2 then '结案' end as state1 " & _
                    "FROM project_master a " & _
                    "where factory_code in (select c.factory_code " & _
                    "                       from sys_user a inner join  " & _
                    "                       	t_user_factory b on a.login_id=b.login_id inner join  " & _
                    "                       	t_factory c on b.factory_code=c.factory_code " & _
                    "                       where a.stop_login=1 and c.stop=1 and a.login_id='" & login_id & "' " & _
                    "                       )  " & s & s1 & s2 & "  order by a.id "
        Else
            sql = "SELECT a.*,case state when 0 then '新建' when 1 then '审核' when 2 then '结案' end as state1 " & _
                    "FROM project_master a left join  " & _
                       " t_rs on a.input_man=t_rs.rs_name " & _
                    "where factory_code in (select c.factory_code " & _
                    "                       from sys_user a inner join  " & _
                    "                       	t_user_factory b on a.login_id=b.login_id inner join  " & _
                    "                       	t_factory c on b.factory_code=c.factory_code " & _
                    "                       where a.stop_login=1 and c.stop=1 and a.login_id='" & login_id & "' " & _
                    "                       )  and t_rs.bm_id= " & BM_id & s & s1 & s2 & "  order by a.id "
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

        Me.ExDataGridView1.AddColumn("id", "id", , , , , , False)

        If Me.ExDataGridView1.Rows.Count > 0 Then
            cellclick(0)
        End If
    End Sub

    Private Sub ToolStripButton_exit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripButton_exit.Click
        Me.Close()
    End Sub

    Private Sub ExDataGridView1_CellClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles ExDataGridView1.CellClick
        If e.RowIndex < 0 Then
            Exit Sub
        Else
            cellclick(e.RowIndex)
        End If
    End Sub


    Private Sub cellclick(ByVal index As Int32)
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

    Private Sub ToolStripButton_mod_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripButton_mod.Click
        Dim project_add As New project_add

        Dim sqlcon As New SqlConnection
        Dim sqlcmd As New SqlCommand
        Dim sql As String = ""
        Dim mytrans As SqlTransaction




        If Me.ExDataGridView1.SelectedCells.Count < 0 Then
            MsgBox("请选择要完工的行！", MsgBoxStyle.OkOnly, "提示")
            Exit Sub
        End If

        sqlcon = getconn()



        If MsgBox("确定要将工程号为" & Me.ExDataGridView1.Rows(Me.ExDataGridView1.SelectedCells(0).RowIndex).Cells("project_no").Value & "的工程完工？", MsgBoxStyle.YesNo, "提示") = MsgBoxResult.No Then
            Exit Sub
        End If

        sqlcon = getconn()

        sqlcmd.Connection = sqlcon
        mytrans = sqlcon.BeginTransaction()
        sqlcmd.Transaction = mytrans
        Try


            sql = "update  project_master set end_date=getdate() ,end_man ='" & rs_name & "' where project_no='" & Me.ExDataGridView1.Rows(Me.ExDataGridView1.SelectedCells(0).RowIndex).Cells("project_no").Value & "'"
            sqlcmd.CommandText = sql
            sqlcmd.ExecuteNonQuery()


            sql = "update  T_part_out_master set state=2,check_date=getdate(),check_man='" & rs_name & "' where project_no='" & Me.ExDataGridView1.Rows(Me.ExDataGridView1.SelectedCells(0).RowIndex).Cells("project_no").Value & "'"
            sqlcmd.CommandText = sql
            sqlcmd.ExecuteNonQuery()

            mytrans.Commit()
        Catch ex As Exception
            mytrans.Rollback()
            MsgBox(ex.Message, MsgBoxStyle.OkOnly, "提示")
            Exit Sub
        End Try

        MsgBox("完工成功！", MsgBoxStyle.OkOnly, "提示")

        Me.ExDataGridView1.Rows(Me.ExDataGridView1.SelectedCells(0).RowIndex).Cells("end_date").Value = Now
        Me.ExDataGridView1.Rows(Me.ExDataGridView1.SelectedCells(0).RowIndex).Cells("end_man").Value = rs_name
    End Sub
End Class