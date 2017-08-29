Imports System.Data.SqlClient
Public Class sys_user_right

    Private Sub ToolStripButton_add_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripButton_add.Click


        Dim add_user As New add_user
        add_user.state = "add"
        add_user.Owner = Me
        add_user.ShowDialog()

        Dim a As New TreeNodeMouseClickEventArgs(Me.TreeView1.SelectedNode, Windows.Forms.MouseButtons.Left, 1, 0, 0)

        TreeView1_NodeMouseClick(sender, a)
    End Sub

    Private Sub sys_user_right_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
     
        Refresh_TreeView()

    End Sub

    Public Sub Refresh_TreeView()

        Dim sqlcon As New SqlConnection
        Dim sqlcmd As New SqlCommand
        Dim sqlad As New SqlDataAdapter


        Dim dt As New DataTable
        Dim sql As String = ""
        Dim i As Int32
        Dim bmno As String = ""
        Dim ss As String = ""

        Me.ToolStripLabel2.Text = ""



        If Me.ToolStripButton_OK.Checked = True Then
            ss = " and a.stop_login=1 "
        ElseIf Me.ToolStripButton_stop.Checked = True Then
            ss = " and a.stop_login=0 "
        End If
        sqlcon = getconn()
        sql = "select a.*,c.bmmc,b.rs_name,c.bmno,b.rs_no  " & _
                "from sys_user a left join  " & _
                "	t_rs b on a.rs_id=b.id left join  " & _
                "	t_bm c on b.bm_id=c.id  " & _
                "where login_id<>'admin' " & ss & _
                "order by bmmc,rs_name "

        sqlcmd.Connection = sqlcon
        sqlcmd.CommandText = sql
        sqlad.SelectCommand = sqlcmd
        dt.Rows.Clear()
        sqlad.Fill(dt)

        Dim Tnode As TreeNode
        Dim Bnode As TreeNode = Nothing
        Dim Unode As TreeNode = Nothing

        Me.TreeView1.Nodes.Clear()
        Tnode = Me.TreeView1.Nodes.Add("alluser", "全部用户", 0, 1)


        For i = 0 To dt.Rows.Count - 1
            If bmno <> dt.Rows(i)("bmno") Then
                Bnode = Tnode.Nodes.Add("B" & dt.Rows(i)("bmno").ToString.Trim, dt.Rows(i)("bmmc").ToString.Trim, 0, 1)
            End If

            Unode = Bnode.Nodes.Add("U" & dt.Rows(i)("login_id").ToString.Trim, dt.Rows(i)("rs_name").ToString.Trim, 2, 2)
            Unode.Tag = dt.Rows(i)("rs_no").ToString.Trim

            bmno = IIf(IsDBNull(dt.Rows(i)("bmno").ToString.Trim), "", dt.Rows(i)("bmno").ToString.Trim)
        Next
        Me.TreeView1.Nodes(0).Expand()
        Me.TvUser1.Nodes.Clear()

        Me.TreeView1.SelectedNode = Me.TreeView1.Nodes("alluser")
        Refresh_ToolButton(False)
    End Sub
    Private Sub Refresh_ToolButton(ByVal show As Boolean)

        Me.ToolStripButton_mod.Enabled = show
        Me.ToolStripButton_save.Enabled = show
        Me.ToolStripButton_copy.Enabled = show
        Me.ToolStripButton_begin.Enabled = show

        If Me.ToolStripButton_stop.Checked = True Then
            Me.ToolStripButton_save.Enabled = False
            Me.ToolStripButton_copy.Enabled = False
        End If
    End Sub


    Private Sub TreeView1_NodeMouseClick(ByVal sender As Object, ByVal e As System.Windows.Forms.TreeNodeMouseClickEventArgs) Handles TreeView1.NodeMouseClick

        Dim sqlcon As New SqlConnection
        Dim sqlcmd As New SqlCommand
        Dim sqlad As New SqlDataAdapter


        Dim dt As New DataTable
        Dim sql As String

        Me.TvUser1.Nodes.Clear()

        If e.Node.Name.Substring(0, 1).ToUpper <> "U" Then
            Me.ToolStripLabel2.Text = ""
            Refresh_ToolButton(False)
            Exit Sub
        Else
            Me.ToolStripLabel2.Text = e.Node.Name.Substring(1, e.Node.Name.Length - 1)
            Refresh_ToolButton(True)

        End If

        sqlcon = getconn()



        sql = "select distinct d.menu_no,d.menu_name,d.father_no,d.menu_index " & _
                "from sys_user a inner join " & _
                "	sys_role_user b on a.login_id=b.login_id left join  " & _
                "	sys_role_right c on b.role_no=c.role_no left join " & _
                "	sys_menu d on c.menu_no=d.menu_no " & _
                "where a.login_id='" & e.Node.Name.Substring(1, e.Node.Name.Length - 1) & "' and d.in_user=1 and d.menu_index is not null " & _
                "order by d.menu_index"

        sqlcmd.Connection = sqlcon
        sqlcmd.CommandText = sql
        sqlad.SelectCommand = sqlcmd
        dt.Clear()
        sqlad.Fill(dt)

        If dt.Rows.Count = 0 Then Exit Sub


        Me.TvUser1.AddTree(dt, "father_no", "menu_no", "menu_name")

        dt.Clear()
        sql = "select b.* " & _
                "from sys_user_right a left join  " & _
                "	sys_menu b on a.menu_no=b.menu_no " & _
                "where b.menu_index is not null and a.in_user=1 and b.in_user=1 and a.login_id='" & e.Node.Name.Substring(1, e.Node.Name.Length - 1) & "' " & _
                "order by b.menu_index "
        sqlcmd.CommandText = sql
        sqlad.SelectCommand = sqlcmd
        dt.Clear()
        sqlad.Fill(dt)

        Me.TvUser1.SelectNode(dt, "menu_no", Me.TvUser1.Nodes.Item(0))
    End Sub

    Private Sub ToolStripButton_OK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripButton_OK.Click
        Me.ToolStripButton_OK.Checked = True
        Me.ToolStripButton_stop.Checked = False
        Refresh_TreeView()

    End Sub

    Private Sub ToolStripButton_stop_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripButton_stop.Click
        Me.ToolStripButton_OK.Checked = False
        Me.ToolStripButton_stop.Checked = True
        Refresh_TreeView()
    End Sub




    Private Sub TvUser1_AfterCheck(ByVal sender As Object, ByVal e As System.Windows.Forms.TreeViewEventArgs) Handles TvUser1.AfterCheck
        TvUser1.CheckNode(e.Node)
    End Sub

    Private Sub ToolStripButton_exit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripButton_exit.Click
        Me.Close()
    End Sub

    Private Sub ToolStripButton_mod_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripButton_mod.Click


        Dim add_user As New add_user
        add_user.state = "mod"
        add_user.Owner = Me
        add_user.ShowDialog()

        Dim a As New TreeNodeMouseClickEventArgs(Me.TreeView1.SelectedNode, Windows.Forms.MouseButtons.Left, 1, 0, 0)
        TreeView1_NodeMouseClick(sender, a)

    End Sub

    Private Sub TreeView1_AfterSelect(ByVal sender As System.Object, ByVal e As System.Windows.Forms.TreeViewEventArgs) Handles TreeView1.AfterSelect

    End Sub

    Private Sub ToolStripButton_refresh_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripButton_refresh.Click
        Refresh_TreeView()
    End Sub

    Private Sub ToolStripButton_save_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripButton_save.Click
        Dim sqlcon As New SqlConnection
        Dim sqlcmd As New SqlCommand
        Dim sqlad As New SqlDataAdapter

        Dim mytrans As SqlTransaction

        Dim dt As New DataTable
        Dim sql As String
        Dim s As String



        sqlcon = getconn()
        s = SaveCheck(Me.TvUser1.Nodes.Item(0))
        s = Microsoft.VisualBasic.Left(s, s.Length - 1)



        sqlcmd.Connection = sqlcon
        mytrans = sqlcon.BeginTransaction


        Try

            sqlcmd.Transaction = mytrans
            sql = "update sys_user_right set in_user=0 where login_id='" & Me.ToolStripLabel2.Text.Trim & "'"
            sqlcmd.CommandText = sql
            sqlcmd.ExecuteNonQuery()

            sql = "update a set in_user=1 from sys_user_right a left join sys_menu b on a.menu_no=b.menu_no where b.menu_no in (" & s & ") and a.login_id='" & Me.ToolStripLabel2.Text.Trim & "'"
            sqlcmd.CommandText = sql
            sqlcmd.ExecuteNonQuery()

            mytrans.Commit()

        Catch ex As Exception
            mytrans.Rollback()
            MsgBox(ex.Message)
            Exit Sub
        End Try
        MsgBox("保存成功！")
    End Sub

    
    Private Sub ToolStripButton_copy_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripButton_copy.Click
        Dim copy_user_right As New copy_user_right
        copy_user_right.Owner = Me

        copy_user_right.ShowDialog()
    End Sub

    Private Sub ToolStripButton_begin_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripButton_begin.Click
        Dim sqlcon As New SqlConnection
        Dim sqlcmd As New SqlCommand
        Dim sql As String
        Dim s As String

        s = InputBox("请输入初始化密码！", "初始化密码", "")
        sqlcon = getconn()
        sqlcmd.Connection = sqlcon

        If s = "" Then Exit Sub

        sql = "update sys_user set old_pwd=new_pwd,new_pwd='" & s & "' where login_id='" & Me.ToolStripLabel2.Text.Trim & "'"
        sqlcmd.CommandText = sql
        sqlcmd.ExecuteNonQuery()

        MsgBox("初始化完成！", MsgBoxStyle.OkOnly, "提示")



    End Sub

    Private Sub TvUser1_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TvUser1.Load

    End Sub

    Private Sub ToolStrip1_ItemClicked(ByVal sender As System.Object, ByVal e As System.Windows.Forms.ToolStripItemClickedEventArgs) Handles ToolStrip1.ItemClicked

    End Sub
End Class