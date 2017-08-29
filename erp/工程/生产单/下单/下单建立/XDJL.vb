Imports System.Data.SqlClient

Public Class XDJL

    Private Sub ListView1_MouseDown(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles ListView1.MouseDown
        Dim sqlcon As New SqlConnection
        Dim sqlcmd As New SqlCommand

        Dim sql As String

        If Me.ExDataGridView1.SelectedCells.Count = 0 Then
            Me.ContextMenuStrip1.Items("删除文件ToolStripMenuItem").Enabled = False
            Me.ContextMenuStrip1.Items("增加文件ToolStripMenuItem").Enabled = False
            Me.ContextMenuStrip1.Items("打开文件ToolStripMenuItem").Enabled = False
            Exit Sub
        End If

        sqlcon = getconn()
        sqlcmd.Connection = sqlcon


        sql = "select state from  scd_master where billno='" & Me.ExDataGridView1.Rows(Me.ExDataGridView1.SelectedCells(0).RowIndex).Cells("billno").Value & "'"

        sqlcmd.CommandText = sql
        If sqlcmd.ExecuteScalar = 0 Then
            Me.ContextMenuStrip1.Items("删除文件ToolStripMenuItem").Enabled = True
            Me.ContextMenuStrip1.Items("增加文件ToolStripMenuItem").Enabled = True
            Me.ContextMenuStrip1.Items("打开文件ToolStripMenuItem").Enabled = True
        Else
            Me.ContextMenuStrip1.Items("删除文件ToolStripMenuItem").Enabled = False
            Me.ContextMenuStrip1.Items("增加文件ToolStripMenuItem").Enabled = False
            Me.ContextMenuStrip1.Items("打开文件ToolStripMenuItem").Enabled = True
        End If

    End Sub



    Private Sub ListView1_MouseUp(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles ListView1.MouseUp
        If Me.ListView1.SelectedItems.Count = 0 Then
            Me.ContextMenuStrip1.Items("删除文件ToolStripMenuItem").Enabled = False
            Me.ContextMenuStrip1.Items("打开文件ToolStripMenuItem").Enabled = False
        Else
            Me.ContextMenuStrip1.Items("删除文件ToolStripMenuItem").Enabled = True
            Me.ContextMenuStrip1.Items("打开文件ToolStripMenuItem").Enabled = True
        End If
    End Sub

    Private Sub ToolStripButton_add_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripButton_add.Click
        Dim XDJL_add As New XDJL_add
        XDJL_add.state = "new"

        XDJL_add.Owner = Me

        If XDJL_add.ShowDialog() = Windows.Forms.DialogResult.OK Then
            Refresh_ExDataGridView_master()
        End If
    End Sub

    Private Sub ToolStripButton_refresh_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripButton_refresh.Click
        Refresh_ExDataGridView_master()
    End Sub


    Private Sub Refresh_ExDataGridView_master(Optional ByVal s As String = "")
        Dim sqlcon As New SqlConnection
        Dim sqlcmd As New SqlCommand
        Dim sqlad As New SqlDataAdapter
        Dim s1 As String = ""
        Dim s2 As String = ""

        Dim dt As New DataTable
        Dim sql As String

        Me.ListView1.Items.Clear()

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
            s2 = s2 & " a.jd_state=0 or "
        End If

        If Me.CheckBox5.Checked Then
            s2 = s2 & " a.jd_state=1 or "
        End If

        If Me.CheckBox6.Checked Then
            s2 = s2 & " a.jd_state=2 or "
        End If

        If Me.CheckBox7.Checked Then
            s2 = s2 & " a.jd_state=3 or "
        End If

        If s2.Length <> 0 Then
            s2 = Microsoft.VisualBasic.Left(s2, s2.Length - 3)
            s2 = " and (" & s2 & ")"
        End If




        If BM_id = 49 Or BM_id = 78 Then
            sql = "SELECT a.*,case state when 0 then '新建' when 1 then '审核' when 2 then '结案' end as state1,b.bmmc as xd_bmmc,c.bmmc as jd_bmmc,case jd_state when 0 then '未接单' when 1 then '已接单' when 2 then '已完成' when 3 then '确认完成' end as state2 " & _
                    "FROM SCD_master a left join " & _
                    "t_bm b on a.xd_bmno=b.bmno left join " & _
                    "t_bm c on a.jd_bmno=c.bmno " & _
                    "where 1=1 " & s & s1 & s2 & "  order by a.billno "
        Else
            sql = "SELECT a.*,case state when 0 then '新建' when 1 then '审核' when 2 then '结案' end as state1,b.bmmc as xd_bmmc,c.bmmc as jd_bmmc,case jd_state when 0 then '未接单' when 1 then '已接单' when 2 then '已完成' when 3 then '确认完成' end as state2 " & _
                    "FROM SCD_master a left join " & _
                    "t_bm b on a.xd_bmno=b.bmno left join " & _
                    "t_bm c on a.jd_bmno=c.bmno left join " & _
                       " t_rs on a.input_man=t_rs.rs_name " & _
                    "where t_rs.bm_id= " & BM_id & s & s1 & s2 & "  order by a.billno "
        End If



        sqlcmd.CommandText = sql
        sqlad.SelectCommand = sqlcmd
        dt.Rows.Clear()
        sqlad.Fill(dt)

        Me.ExDataGridView1.SetDataSource(dt)
        Me.ExDataGridView1.Columns.Clear()

        Me.ExDataGridView1.AddColumn("billno", "单号")
        'Me.ExDataGridView1.AddColumn("project_no", "工程号")
        Me.ExDataGridView1.AddColumn("xd_bmmc", "下单部门名称")
        Me.ExDataGridView1.AddColumn("jd_bmmc", "接单部门名称")
        Me.ExDataGridView1.AddColumn("state1", "状态")
        Me.ExDataGridView1.AddColumn("state2", "接单状态")

        Me.ExDataGridView1.AddColumn("memo", "说明")

        Me.ExDataGridView1.AddColumn("money", "完工金额", , , , DataGridViewContentAlignment.MiddleRight, "#0.00")


        Me.ExDataGridView1.AddColumn("input_man", "录入人")
        Me.ExDataGridView1.AddColumn("input_date", "录入日期", , , , , "yyyy-MM-dd")

        Me.ExDataGridView1.AddColumn("check_man", "审核人")
        Me.ExDataGridView1.AddColumn("check_date", "审核日期", , , , , "yyyy-MM-dd")

        Me.ExDataGridView1.AddColumn("jd_man", "接单人")
        Me.ExDataGridView1.AddColumn("jd_date", "接单日期", , , , , "yyyy-MM-dd")


        Me.ExDataGridView1.AddColumn("id", "id", , , , , , False)

        If Me.ExDataGridView1.Rows.Count > 0 Then
            cellclick(0)
        Else
            cellclick(-1)
        End If
    End Sub

    Private Sub ListView1_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ListView1.SelectedIndexChanged

    End Sub

    Private Sub ToolStripButton_exit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripButton_exit.Click
        Me.Close()
    End Sub

    Private Sub ToolStripButton_mod_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripButton_mod.Click
        Dim sqlcon As New SqlConnection
        Dim sqlcmd As New SqlCommand
        Dim sqlad As New SqlDataAdapter
        Dim dt As New DataTable

        Dim sql As String

        sqlcon = getconn()
        sqlcmd.Connection = sqlcon

        sql = "select  count(*) from scd_master where state=0 and billno='" & Me.ExDataGridView1.Rows(Me.ExDataGridView1.SelectedCells(0).RowIndex).Cells("billno").Value & "'"
        sqlcmd.CommandText = sql


        If sqlcmd.ExecuteScalar = 0 Then
            MsgBox("该单据不是新建状态，不可以修改！", MsgBoxStyle.OkOnly, "提示")
            Exit Sub
        End If

       


        XDJL_add.state = "mod"

        XDJL_add.Owner = Me

        If XDJL_add.ShowDialog() = Windows.Forms.DialogResult.OK Then
            Refresh_ExDataGridView_master()
        End If
    End Sub

    Private Sub ToolStripButton_del_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripButton_del.Click
        Dim sqlcon As New SqlConnection
        Dim sqlcmd As New SqlCommand
        Dim sqlad As New SqlDataAdapter
        Dim dt As New DataTable
        Dim mytrans As SqlTransaction

        Dim sql As String

        sqlcon = getconn()
        sqlcmd.Connection = sqlcon


        If Me.ExDataGridView1.SelectedCells.Count < 0 Then
            MsgBox("请选择要修改的行！", MsgBoxStyle.OkOnly, "提示")
            Exit Sub
        End If

        If MsgBox("确认要删除领料单号为：" & Me.ExDataGridView1.Rows(Me.ExDataGridView1.SelectedCells(0).RowIndex).Cells("billno").Value & "的单据吗？", MsgBoxStyle.YesNo, "提示") = MsgBoxResult.No Then
            Exit Sub
        End If



        sql = "select  count(*) from scd_master where state=0 and billno='" & Me.ExDataGridView1.Rows(Me.ExDataGridView1.SelectedCells(0).RowIndex).Cells("billno").Value & "'"
        sqlcmd.CommandText = sql


        If sqlcmd.ExecuteScalar = 0 Then
            MsgBox("该单据不是新建状态，不可以删除！", MsgBoxStyle.OkOnly, "提示")
            Exit Sub
        End If




        mytrans = sqlcon.BeginTransaction

        sqlcmd.Transaction = mytrans

        Try

            sql = "delete from scd_master where billno='" & Me.ExDataGridView1.Rows(Me.ExDataGridView1.SelectedCells(0).RowIndex).Cells("billno").Value & "' "
            sqlcmd.CommandText = sql
            sqlcmd.ExecuteNonQuery()

            sql = "delete from pic where billno='" & Me.ExDataGridView1.Rows(Me.ExDataGridView1.SelectedCells(0).RowIndex).Cells("billno").Value & "' "
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



    Private Sub cellclick(ByVal index As int32)
        Dim sqlcon As New SqlConnection
        Dim sqlcmd As New SqlCommand
        Dim sqlad As New SqlDataAdapter

        Dim dt As New DataTable
        Dim sql As String
        Dim s As String = ""
        Dim listitem As ListViewItem
        Dim i As Integer

        sqlcon = getconn()

        Me.ListView1.Items.Clear()

        'sql = "select * from  t_product_FY_detail where fy_id='" & Me.ExDataGridView1.Rows(index).Cells("id").Value & "' order by id"
        If index <> -1 Then

            sql = " SELECT a.* " & _
"                        FROM pic a    " & _
"                           where a.billno='" & Me.ExDataGridView1.Rows(index).Cells("billno").Value & "' order by a.id"

        Else
            sql = " SELECT a.*  " & _
"                        FROM pic a " & _
"                           where a.id=-1"
        End If
        sqlcmd.Connection = sqlcon
        sqlcmd.CommandText = sql
        sqlad.SelectCommand = sqlcmd
        dt.Rows.Clear()
        sqlad.Fill(dt)


        For i = 0 To dt.Rows.Count - 1

            listitem = Me.ListView1.Items.Add(dt.Rows(i)("filename"), 0)
            listitem.ToolTipText = dt.Rows(i)("filename")

        Next i

    End Sub

    Private Sub 增加文件ToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles 增加文件ToolStripMenuItem.Click
        Dim openfile As New OpenFileDialog
        Dim i As Integer
        Dim listitem As New ListViewItem

        Dim sqlcon As New SqlConnection
        Dim sqlcmd As New SqlCommand


        Dim sql As String = ""


        openfile.Title = "打开文件"
        openfile.Multiselect = True

        If openfile.ShowDialog() <> Windows.Forms.DialogResult.OK Then
            Exit Sub
        End If

        sqlcon = getconn()
        sqlcmd.Connection = sqlcon


        For i = 0 To openfile.FileNames.GetLength(0) - 1
            sql = "select count(*) from pic where filename='" & openfile.FileNames(i).Trim.Substring(openfile.FileNames(i).Trim.LastIndexOf("\") + 1) & "' and billno='" & Me.ExDataGridView1.Rows(Me.ExDataGridView1.SelectedCells(0).RowIndex).Cells("billno").Value & "'"
            sqlcmd.CommandText = sql
            If sqlcmd.ExecuteScalar() <> 0 Then
                MsgBox("该单据已经包括文件名为" & openfile.FileNames(i).Trim.Substring(openfile.FileNames(i).Trim.LastIndexOf("\") + 1) & "的文件，请重新选择！", MsgBoxStyle.OkOnly)
                Exit Sub
            End If
        Next

        For i = 0 To openfile.FileNames.GetLength(0) - 1
            save_file(openfile.FileNames(i).Trim, Me.ExDataGridView1.Rows(Me.ExDataGridView1.SelectedCells(0).RowIndex).Cells("billno").Value)
            listitem = Me.ListView1.Items.Add(openfile.FileNames(i).Trim.Substring(openfile.FileNames(i).Trim.LastIndexOf("\") + 1), 0)
            listitem.ToolTipText = openfile.FileNames(i).Trim.Substring(openfile.FileNames(i).Trim.LastIndexOf("\") + 1)

        Next


        MsgBox("存储成功！", MsgBoxStyle.OkOnly)


    End Sub

    Private Sub 删除文件ToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles 删除文件ToolStripMenuItem.Click
        Dim sqlcon As New SqlConnection
        Dim sqlcmd As New SqlCommand
        Dim mytrans As SqlTransaction

        Dim sql As String
        Dim i As Integer

        If MsgBox("确定要删除当前选择的文件吗？", MsgBoxStyle.YesNo) = MsgBoxResult.No Then
            Exit Sub
        End If

        sqlcon = getconn()
        sqlcmd.Connection = sqlcon
        mytrans = sqlcon.BeginTransaction

        sqlcmd.Transaction = mytrans



        Try


            For i = Me.ListView1.SelectedItems.Count - 1 To 0 Step -1
                sql = "delete from pic where billno='" & Me.ExDataGridView1.Rows(Me.ExDataGridView1.SelectedCells(0).RowIndex).Cells("billno").Value & "' and filename='" & Me.ListView1.SelectedItems(i).Text & "'"

                sqlcmd.CommandText = sql
                sqlcmd.ExecuteNonQuery()

                Me.ListView1.SelectedItems(i).Remove()
            Next



            mytrans.Commit()
        Catch ex As Exception
            mytrans.Rollback()
            MsgBox(ex.Message, MsgBoxStyle.OkOnly, "错误")
            Exit Sub
        End Try

        MsgBox("删除成功！", MsgBoxStyle.OkOnly)

    End Sub

    Private Sub 打开文件ToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles 打开文件ToolStripMenuItem.Click
        OutputFile(Me.ListView1.SelectedItems(0).Text, Me.ExDataGridView1.Rows(Me.ExDataGridView1.SelectedCells(0).RowIndex).Cells("billno").Value)
    End Sub
End Class