Imports System.Data.SqlClient
Public Class sys_role_right

    Private Sub sys_role_right_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        FlashList()
    End Sub

    Public Sub FlashList()

        Dim sqlcon As New SqlConnection
        Dim sqlcmd As New SqlCommand
        Dim sqlad As New SqlDataAdapter


        Dim dt As New DataTable
        Dim sql As String
        Dim i As Integer

        Dim Tnode As New TreeNode

        sqlcon = getconn()

        Me.TreeView1.Nodes.Clear()

        Dim node As TreeNode
        node = Me.TreeView1.Nodes.Add("角色")


        sql = "select * from sys_role order by role_name"
        sqlcmd.Connection = sqlcon
        sqlcmd.CommandText = sql
        sqlad.SelectCommand = sqlcmd

        dt.Clear()

        sqlad.Fill(dt)

        For i = 0 To dt.Rows.Count - 1
            Tnode = node.Nodes.Add(dt.Rows(i)("role_name"))
            Tnode.Tag = dt.Rows(i)("role_no")
        Next
        node.Expand()

    End Sub

    Private Sub ToolStripButton_exit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripButton_exit.Click

        Me.Close()

    End Sub

    Private Sub ToolStripButton_add_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripButton_add.Click
        Dim role_add As New role_add
        role_add.state = "ADD"
        role_add.Text = "角色添加"
        role_add.ShowDialog()

    End Sub

    Private Sub ToolStripButton_mod_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripButton_mod.Click

        If Me.TreeView1.SelectedNode Is Nothing Or Me.TreeView1.SelectedNode.Parent Is Nothing Then
            MsgBox("请选择要修改的角色！", MsgBoxStyle.OkOnly, "提示")
            Exit Sub
        End If

        Dim role_add As New role_add
        role_add.state = "MOD"
        role_add.Text = "角色修改"
        role_add.TextBox1.Enabled = False

        role_add.TextBox1.Text = Me.TreeView1.SelectedNode.Tag
        role_add.TextBox2.Text = Me.TreeView1.SelectedNode.Text

        role_add.ShowDialog()
    End Sub

    Private Sub ToolStripButton_del_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripButton_del.Click
        If Me.TreeView1.SelectedNode Is Nothing Or Me.TreeView1.SelectedNode.Parent Is Nothing Then
            MsgBox("请选择要删除的角色！", MsgBoxStyle.OkOnly, "提示")
            Exit Sub
        End If

        If MsgBox("是否确定删除角色" & Me.TreeView1.SelectedNode.Text & "？", MsgBoxStyle.YesNo, "提示") = MsgBoxResult.No Then
            Exit Sub
        End If

        Dim sqlcon As New SqlConnection
        Dim sqlcmd As New SqlCommand
        Dim mytrans As SqlTransaction
        Dim sql As String

        sqlcon = getconn()
        sqlcmd.Connection = sqlcon
        mytrans = sqlcon.BeginTransaction
        Try
            sqlcmd.Transaction = mytrans
            '删除角色权限
            sql = "delete a from  sys_role_right a left join sys_role b on a.role_no=b.role_no where b.role_no='" & Me.TreeView1.SelectedNode.Tag & "' "
            sqlcmd.CommandText = sql
            sqlcmd.ExecuteNonQuery()

            '删除角色用户
            sql = "delete a from sys_role_user a left join sys_role b on a.role_no=b.role_no where b.role_no='" & Me.TreeView1.SelectedNode.Tag & "' "
            sqlcmd.CommandText = sql
            sqlcmd.ExecuteNonQuery()

            '删除角色
            sql = "delete  from sys_role where role_no='" & Me.TreeView1.SelectedNode.Tag & "'"
            sqlcmd.CommandText = sql
            sqlcmd.ExecuteNonQuery()

            '删除用户权限
            sql = "delete bb  from sys_user_right bb left join (select a.login_id,c.menu_no from sys_user a left join sys_role_user b on a.login_id=b.login_id left join sys_role_right c on b.role_no=c.role_no ) aa on aa.login_id=bb.login_id and aa.menu_no=bb.menu_no where aa.menu_no is null"
            sqlcmd.CommandText = sql
            sqlcmd.ExecuteNonQuery()
            mytrans.Commit()

        Catch ex As Exception
            mytrans.Rollback()
            MsgBox(ex.Message)
            Exit Sub
        End Try

        MsgBox("角色删除成功！", MsgBoxStyle.Information, "提示")
        FlashList()
    End Sub

    Private Sub TreeView1_AfterSelect(ByVal sender As System.Object, ByVal e As System.Windows.Forms.TreeViewEventArgs) Handles TreeView1.AfterSelect
        If Me.TreeView1.SelectedNode Is Nothing Or Me.TreeView1.SelectedNode.Parent Is Nothing Then
            Me.TvUser1.Nodes.Clear()
            Exit Sub
        End If
        RefreshTvUser()
    End Sub

    Private Sub TvUser1_AfterCheck(ByVal sender As Object, ByVal e As System.Windows.Forms.TreeViewEventArgs)

    End Sub
    Private Sub RefreshTvUser()
        Dim sqlcon As New SqlConnection
        Dim sqlcmd As New SqlCommand
        Dim sqlad As New SqlDataAdapter

        Dim dt As New DataTable
        Dim sql As String


        sqlcon = getconn()

        sql = "select * from sys_menu where in_user=1 and menu_index is not null order by menu_index"

        sqlcmd.Connection = sqlcon
        sqlcmd.CommandText = sql
        sqlad.SelectCommand = sqlcmd
        dt.Clear()

        sqlad.Fill(dt)

        Me.TvUser1.AddTree(dt, "father_no", "menu_no", "menu_name")

        dt.Clear()
        sql = "select c.* " & _
                "from sys_role a left join  " & _
                "	sys_role_right b on a.role_no=b.role_no left join " & _
                "	sys_menu c on b.menu_no=c.menu_no  " & _
                "where a.role_no='" & Me.TreeView1.SelectedNode.Tag & "' and c.in_user=1 and menu_index is not null " & _
                "order by c.menu_index "
        sqlcmd.Connection = sqlcon
        sqlcmd.CommandText = sql
        sqlad.SelectCommand = sqlcmd
        dt.Clear()
        sqlad.Fill(dt)

        Me.TvUser1.SelectNode(dt, "menu_no", Me.TvUser1.Nodes.Item(0))
    End Sub

    Private Sub ToolStripButton_refresh_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripButton_refresh.Click
        If Me.TreeView1.SelectedNode Is Nothing Or Me.TreeView1.SelectedNode.Parent Is Nothing Then Exit Sub
        RefreshTvUser()
    End Sub

    Private Sub ToolStripButton_save_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripButton_save.Click
        Dim sqlcon As New SqlConnection
        Dim sqlcmd As New SqlCommand
        Dim sqlad As New SqlDataAdapter
        Dim mytrans As SqlTransaction
        Dim sql As String

        If Me.TreeView1.SelectedNode Is Nothing Or Me.TreeView1.SelectedNode.Parent Is Nothing Then Exit Sub

        sqlcon = getconn()
        Dim s As String


        s = SaveCheck(Me.TvUser1.Nodes.Item(0))
        s = Microsoft.VisualBasic.Left(s, s.Length - 1)

        sqlcmd.Connection = sqlcon
        mytrans = sqlcon.BeginTransaction
        Try

            sqlcmd.Transaction = mytrans
            sql = "delete a from sys_role_right a left join sys_role b on a.role_no=b.role_no where b.role_no ='" & Me.TreeView1.SelectedNode.Tag & "'"
            sqlcmd.CommandText = sql
            sqlcmd.ExecuteNonQuery()

            sql = "insert into sys_role_right(role_no,menu_no)  select (select role_no from sys_role where role_no='" & Me.TreeView1.SelectedNode.Tag & "') as role_no,menu_no from sys_menu where menu_no in (" & s & ")"
            sqlcmd.CommandText = sql
            sqlcmd.ExecuteNonQuery()

            sql = "exec save_role_right '" & Me.TreeView1.SelectedNode.Text & "'"
            sqlcmd.CommandText = sql
            sqlcmd.ExecuteNonQuery()

            mytrans.Commit()

        Catch ex As Exception
            mytrans.Rollback()
            MsgBox(ex.Message)
            Exit Sub
        End Try


        MsgBox("设置成功！", MsgBoxStyle.Information, "提示")
    End Sub

    Private Sub TvUser1_AfterCheck1(ByVal sender As Object, ByVal e As System.Windows.Forms.TreeViewEventArgs) Handles TvUser1.AfterCheck
        TvUser1.CheckNode(e.Node)
    End Sub

   
End Class