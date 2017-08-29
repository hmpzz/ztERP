Imports System.Data.SqlClient
Public Class t_part_out_check


    Private Sub ToolStripButton_exit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripButton_exit.Click
        Me.Close()
    End Sub

    Private Sub ToolStripButton_refresh_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripButton_refresh.Click
        Refresh_ExDataGridView_master()
    End Sub



    Friend Sub Refresh_ExDataGridView_master(Optional ByVal s As String = "")
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

        If Me.CheckBox4.Checked Then
            s1 = s1 & " and a.billno in (select distinct b.billno  " & _
                        "from  T_part_out_master a left join  " & _
                        "	T_part_out_detail b on a.billno=b.billno " & _
                        "where b.ok_amount<b.amount and a.state=1) "
        End If


        If BM_id = 49 Or BM_id = 78 Then
            sql = "SELECT a.*,case a.state when 0 then '新建' when 1 then '审核' when 2 then '结案' end as state1,b.project_name,c.depot_name " & _
    "                FROM T_part_out_master a  left join   " & _
    "                	  	project_master b on a.project_no=b.project_no  left join  " & _
    "                       t_depot c on a.depot_code=c.depot_code " & _
    "                where 1=1 " & s1 & s & " order by a.billno"
        Else
            sql = "SELECT a.*,case a.state when 0 then '新建' when 1 then '审核' when 2 then '结案' end as state1,b.project_name,c.depot_name " & _
    "                FROM T_part_out_master a  left join   " & _
    "                	  	project_master b on a.project_no=b.project_no  left join  " & _
    "                       t_depot c on a.depot_code=c.depot_code left join  " & _
                       " t_rs on a.input_man=t_rs.rs_name " & _
    "                where t_rs.bm_id= " & BM_id & s1 & s & " order by a.billno"
        End If


        sqlcmd.CommandText = sql
        sqlad.SelectCommand = sqlcmd
        dt.Rows.Clear()
        sqlad.Fill(dt)

        Me.ExDataGridView1.SetDataSource(dt)
        Me.ExDataGridView1.Columns.Clear()

        'Me.ExDataGridView1.AddColumn("check", "", 20, False, New DataGridViewCheckBoxColumn)
        Me.ExDataGridView1.AddColumn("billno", "单据号", 120)
        Me.ExDataGridView1.AddColumn("project_no", "工程号")
        Me.ExDataGridView1.AddColumn("project_name", "工程名称")
        Me.ExDataGridView1.AddColumn("depot_name", "领料仓库")
        Me.ExDataGridView1.AddColumn("state1", "状态")
        Me.ExDataGridView1.AddColumn("memo", "说明")

        Me.ExDataGridView1.AddColumn("ll_man", "领料人")

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
        Dim s As String = ""

        sqlcon = getconn()


        'sql = "select * from  t_product_FY_detail where fy_id='" & Me.ExDataGridView1.Rows(index).Cells("id").Value & "' order by id"
        If index <> -1 Then

            sql = " SELECT a.*,b.item_name,b.item_standard,b.unit " & _
"                        FROM t_part_out_detail a left join   " & _
"                        	t_item b on a.item_code=b.item_code    " & _
"                           where a.billno='" & Me.ExDataGridView1.Rows(index).Cells("billno").Value & "' order by a.id"

        Else
            sql = " SELECT a.*,b.item_name,b.item_standard,b.unit " & _
"                        FROM t_part_out_detail a left join   " & _
"                        	t_item b on a.item_code=b.item_code    " & _
"                           where a.id=-1"
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

        Me.ExDataGridView2.AddColumn("amount", "数量", , , , DataGridViewContentAlignment.MiddleRight, "#0.0000")
        Me.ExDataGridView2.AddColumn("OK_amount", "已出库数量", 120, , , DataGridViewContentAlignment.MiddleRight, "#0.0000")


        Me.ExDataGridView2.AddColumn("item_code", "item_code", , , , , , False)


        Me.ExDataGridView2.AddColumn("id", "id", , , , , , False)
    End Sub

    Private Sub ExDataGridView1_CellClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles ExDataGridView1.CellClick
        If e.RowIndex < 0 Then
            Exit Sub
        Else
            cellclick(e.RowIndex)
        End If
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
                "FROM T_part_out_master where billno='" & Me.ExDataGridView1.Rows(Me.ExDataGridView1.SelectedCells(0).RowIndex).Cells("billno").Value & "'"


        sqlcmd.CommandText = sql
        k = sqlcmd.ExecuteScalar

        If k = 2 Then
            MsgBox("该单据已经结案，请检查后操作！", MsgBoxStyle.OkOnly, "提示")
            Exit Sub
        ElseIf k = 1 Then
            MsgBox("该单据已经审核，请检查后操作！", MsgBoxStyle.OkOnly, "提示")
            Exit Sub
        End If

        sql = "update  T_part_out_master set state=1,check_date=getdate(),check_man='" & rs_name & "' where billno='" & Me.ExDataGridView1.Rows(Me.ExDataGridView1.SelectedCells(0).RowIndex).Cells("billno").Value & "'"
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
                "FROM T_part_out_master where billno='" & Me.ExDataGridView1.Rows(Me.ExDataGridView1.SelectedCells(0).RowIndex).Cells("billno").Value & "'"


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
                    "FROM T_part_out_detail where billno='" & Me.ExDataGridView1.Rows(Me.ExDataGridView1.SelectedCells(0).RowIndex).Cells("billno").Value & "' and ok_amount>0 "
        sqlcmd.CommandText = sql
        If sqlcmd.ExecuteScalar > 0 Then
            MsgBox("该单据已经有完成数量，请检查后操作！", MsgBoxStyle.OkOnly, "提示")
            Exit Sub
        End If



        sql = "update  T_part_out_master set state=0,check_date=getdate(),check_man='" & rs_name & "' where billno='" & Me.ExDataGridView1.Rows(Me.ExDataGridView1.SelectedCells(0).RowIndex).Cells("billno").Value & "'"
        sqlcmd.CommandText = sql
        sqlcmd.ExecuteNonQuery()

        'Refresh_ExDataGridView(Me.TreeView1.SelectedNode.Text)
        Me.ExDataGridView1.Rows(Me.ExDataGridView1.SelectedCells(0).RowIndex).Cells("state1").Value = "新建"

        Me.ExDataGridView1.Rows(Me.ExDataGridView1.SelectedCells(0).RowIndex).Cells("check_man").Value = rs_name
        Me.ExDataGridView1.Rows(Me.ExDataGridView1.SelectedCells(0).RowIndex).Cells("check_date").Value = Now
        MsgBox("反审核成功！", MsgBoxStyle.OkOnly, "提示")
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
                "FROM T_part_out_master where billno='" & Me.ExDataGridView1.Rows(Me.ExDataGridView1.SelectedCells(0).RowIndex).Cells("billno").Value & "'"


        sqlcmd.CommandText = sql
        k = sqlcmd.ExecuteScalar

        If k = 2 Then
            MsgBox("该单据已经结案，请检查后操作！", MsgBoxStyle.OkOnly, "提示")
            Exit Sub
        End If

        sql = "update  T_part_out_master set state=2,check_date=getdate(),check_man='" & rs_name & "' where billno='" & Me.ExDataGridView1.Rows(Me.ExDataGridView1.SelectedCells(0).RowIndex).Cells("billno").Value & "'"
        sqlcmd.CommandText = sql
        sqlcmd.ExecuteNonQuery()

        'Refresh_ExDataGridView(Me.TreeView1.SelectedNode.Text)
        Me.ExDataGridView1.Rows(Me.ExDataGridView1.SelectedCells(0).RowIndex).Cells("state1").Value = "结案"

        Me.ExDataGridView1.Rows(Me.ExDataGridView1.SelectedCells(0).RowIndex).Cells("check_man").Value = rs_name
        Me.ExDataGridView1.Rows(Me.ExDataGridView1.SelectedCells(0).RowIndex).Cells("check_date").Value = Now
        MsgBox("结案成功！", MsgBoxStyle.OkOnly, "提示")
    End Sub

    Private Sub 反结案ToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles 反结案ToolStripMenuItem.Click
        Dim sqlcon As New SqlConnection
        Dim sqlcmd As New SqlCommand

        Dim sql As String
        Dim k As Int32

        If Me.ExDataGridView1.SelectedCells.Count = 0 Then
            MsgBox("请选择要反结案的单据！", MsgBoxStyle.OkOnly, "提示")
            Exit Sub
        End If

        sqlcon = getconn()
        sqlcmd.Connection = sqlcon

        sql = "SELECT state " & _
                "FROM T_part_out_master where billno='" & Me.ExDataGridView1.Rows(Me.ExDataGridView1.SelectedCells(0).RowIndex).Cells("billno").Value & "'"


        sqlcmd.CommandText = sql
        k = sqlcmd.ExecuteScalar

        If k = 1 Then
            MsgBox("该单据状态为审核，请检查后操作！", MsgBoxStyle.OkOnly, "提示")
            Exit Sub
        ElseIf k = 0 Then
            MsgBox("该单据状态为新建，请检查后操作！", MsgBoxStyle.OkOnly, "提示")
            Exit Sub
        End If

        sql = "update  T_part_out_master set state=1,check_date=getdate(),check_man='" & rs_name & "' where billno='" & Me.ExDataGridView1.Rows(Me.ExDataGridView1.SelectedCells(0).RowIndex).Cells("billno").Value & "'"
        sqlcmd.CommandText = sql
        sqlcmd.ExecuteNonQuery()

        'Refresh_ExDataGridView(Me.TreeView1.SelectedNode.Text)

        Me.ExDataGridView1.Rows(Me.ExDataGridView1.SelectedCells(0).RowIndex).Cells("state1").Value = "审核"
        Me.ExDataGridView1.Rows(Me.ExDataGridView1.SelectedCells(0).RowIndex).Cells("check_man").Value = rs_name
        Me.ExDataGridView1.Rows(Me.ExDataGridView1.SelectedCells(0).RowIndex).Cells("check_date").Value = Now
        MsgBox("反结案成功！", MsgBoxStyle.OkOnly, "提示")
    End Sub

    Private Sub ExDataGridView1_CellContentClick(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles ExDataGridView1.CellContentClick

    End Sub

    Private Sub ExDataGridView1_CellMouseDown(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellMouseEventArgs) Handles ExDataGridView1.CellMouseDown
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
End Class