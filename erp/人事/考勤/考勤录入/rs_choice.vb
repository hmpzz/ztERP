Imports System.Data.SqlClient
Public Class rs_choice

    Private Sub rs_choice_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Refresh_TreeView()
    End Sub

    Public Sub Refresh_TreeView(Optional ByVal s As String = "")

        Dim sqlcon As New SqlConnection
        Dim sqlcmd As New SqlCommand
        Dim sqlad As New SqlDataAdapter


        Dim dt As New DataTable
        Dim sql As String = ""
        Dim i As Int32
        Dim bmno As String = ""
        Dim ss As String = ""




        ss = " and b.stop=1 "

        sqlcon = getconn()
        sql = "select b.*,c.bmmc,b.rs_name,c.bmno  " & _
                "from t_rs b  left join  " & _
                "	t_bm c on b.bm_id=c.id  " & _
                "where 1=1 " & ss & s & _
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

            Unode = Bnode.Nodes.Add("U" & dt.Rows(i)("rs_no").ToString.Trim, dt.Rows(i)("rs_name").ToString.Trim, 2, 2)
            Unode.Tag = dt.Rows(i)("rs_no").ToString.Trim

            bmno = IIf(IsDBNull(dt.Rows(i)("bmno").ToString.Trim), "", dt.Rows(i)("bmno").ToString.Trim)
        Next
        Me.TreeView1.Nodes(0).Expand()


        Me.TreeView1.SelectedNode = Me.TreeView1.Nodes("alluser")

    End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        Me.Owner.Tag = Me.TreeView1.SelectedNode.Tag
        Me.DialogResult = Windows.Forms.DialogResult.OK
    End Sub

    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button2.Click
        Me.DialogResult = Windows.Forms.DialogResult.Cancel
    End Sub

    Private Sub Button3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button3.Click
        Dim s As String = ""
        If Me.CheckBox1.Checked Then
            s = s & " and b.rs_name like '%" & Me.TextBox1.Text.Trim & "%'"
        End If

        If Me.CheckBox2.Checked Then
            s = s & " and b.rs_no like '%" & Me.TextBox2.Text.Trim & "%'"
        End If

        Refresh_TreeView(s)
    End Sub

    Private Sub TextBox1_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles TextBox1.LostFocus
        If Me.TextBox1.Text.Trim.Length > 0 Then
            Me.CheckBox1.Checked = True
        Else
            Me.CheckBox1.Checked = False
        End If
    End Sub

    Private Sub TextBox1_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TextBox1.TextChanged

    End Sub

    Private Sub TextBox2_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TextBox2.TextChanged
        If Me.TextBox2.Text.Trim.Length > 0 Then
            Me.CheckBox2.Checked = True
        Else
            Me.CheckBox2.Checked = False
        End If
    End Sub

    Private Sub CheckBox1_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CheckBox1.CheckedChanged
        If Me.CheckBox1.Checked = False Then
            Me.TextBox1.Text = ""
        End If
    End Sub

    Private Sub CheckBox2_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CheckBox2.CheckedChanged
        If Me.CheckBox2.Checked = False Then
            Me.TextBox2.Text = ""
        End If
    End Sub
End Class