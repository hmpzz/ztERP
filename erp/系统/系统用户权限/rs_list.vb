Imports System.Data.SqlClient
Public Class rs_list

    Private Sub TreeView1_AfterSelect(ByVal sender As System.Object, ByVal e As System.Windows.Forms.TreeViewEventArgs)

    End Sub

    Private Sub rs_list_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim sqlcon As New SqlConnection
        Dim sqlcmd As New SqlCommand
        Dim sqlad As New SqlDataAdapter


        Dim dt As New DataTable
        Dim sql As String = ""
        Dim i As Int32
        Dim bmno As String = ""



        sqlcon = getconn()
        sql = "select b.id,c.bmmc,b.rs_name,c.bmno,b.rs_no  " & _
                "from t_rs b  left join  " & _
                "	t_bm c on b.bm_id=c.id  " & _
                "where  b.stop=1 and b.id not in (select rs_id from sys_user)" & _
                "order by bmmc,rs_name "

        sqlcmd.Connection = sqlcon
        sqlcmd.CommandText = sql
        sqlad.SelectCommand = sqlcmd
        dt.Rows.Clear()
        sqlad.Fill(dt)

        Dim Anode As TreeNode
        Dim Bnode As TreeNode = Nothing
        Dim Unode As TreeNode = Nothing

        Me.TreeView1.Nodes.Clear()
        Anode = Me.TreeView1.Nodes.Add("alluser", "全部人员", 0, 1)


        For i = 0 To dt.Rows.Count - 1
            If bmno <> dt.Rows(i)("bmno") Then
                Bnode = Anode.Nodes.Add("B" & dt.Rows(i)("bmno").ToString.Trim, dt.Rows(i)("bmmc").ToString.Trim, 0, 1)
            End If

            Unode = Bnode.Nodes.Add("U" & dt.Rows(i)("id").ToString.Trim, dt.Rows(i)("rs_name").ToString.Trim, 2, 2)
            Unode.Tag = dt.Rows(i)("rs_no")

            bmno = IIf(IsDBNull(dt.Rows(i)("bmno").ToString.Trim), "", dt.Rows(i)("bmno").ToString.Trim)
        Next
        Me.TreeView1.Nodes(0).Expand()
    End Sub

  
    Private Sub TreeView1_AfterSelect_1(ByVal sender As System.Object, ByVal e As System.Windows.Forms.TreeViewEventArgs) Handles TreeView1.AfterSelect

    End Sub

    Private Sub TreeView1_NodeMouseDoubleClick(ByVal sender As Object, ByVal e As System.Windows.Forms.TreeNodeMouseClickEventArgs) Handles TreeView1.NodeMouseDoubleClick
        If e.Node.Name.ToString.Substring(0, 1).ToUpper <> "U" Then Exit Sub

        CType(Me.Owner, add_user).TextBox1.Text = e.Node.Tag


        Me.Close()

    End Sub
End Class