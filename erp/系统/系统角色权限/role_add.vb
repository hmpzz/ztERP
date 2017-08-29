Imports System.Data.SqlClient
Public Class role_add
    Public state As String

    Private Sub role_add_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

    End Sub

    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button2.Click
        Me.Close()
    End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click

        Dim sqlcon As New SqlConnection
        Dim sqlcmd As New SqlCommand
        Dim sqlad As New SqlDataAdapter
        Dim dt As New DataTable

        Dim sql As String

        sqlcon = getconn()


        If Me.state = "ADD" Then

            If Me.TextBox1.Text.Length = 0 Then Exit Sub
            sql = "select * from sys_role where role_name='" & Trim(Me.TextBox2.Text) & "' or role_no='" & Trim(Me.TextBox1.Text) & "'"

            sqlcmd.Connection = sqlcon
            sqlcmd.CommandText = sql
            sqlad.SelectCommand = sqlcmd
            dt.Clear()

            sqlad.Fill(dt)

            If dt.Rows.Count <> 0 Then
                MsgBox("角色名称或者角色编号已经存在，请检查后输入！", MsgBoxStyle.Exclamation, "提示")
                Exit Sub
            Else
                sql = "insert into sys_role(role_no,role_name) values ('" & Trim(Me.TextBox1.Text) & "','" & Trim(Me.TextBox2.Text) & "')"
                sqlcmd.CommandText = sql
                sqlcmd.ExecuteNonQuery()

                MsgBox("添加成功！", MsgBoxStyle.Exclamation, "提示")

            End If
        ElseIf Me.state = "MOD" Then

            sql = "select * from sys_role where role_name='" & Trim(Me.TextBox2.Text) & "' and role_no<>'" & Trim(Me.TextBox1.Text) & "'"

            sqlcmd.Connection = sqlcon
            sqlcmd.CommandText = sql
            sqlad.SelectCommand = sqlcmd
            dt.Clear()

            sqlad.Fill(dt)

            If dt.Rows.Count <> 0 Then
                MsgBox("角色名称已经存在，请检查后输入！", MsgBoxStyle.Exclamation, "提示")
                Exit Sub
            Else

                sql = "update sys_role set role_name='" & Me.TextBox2.Text.Trim & "' where role_no='" & Me.TextBox1.Text.Trim & "'"
                sqlcmd.CommandText = sql
                sqlcmd.ExecuteNonQuery()

                MsgBox("修改成功！", MsgBoxStyle.Exclamation, "提示")
                sys_role_right.TreeView1.SelectedNode.Text = Me.TextBox2.Text.Trim
            End If

        End If

        '        sys_role_right.FlashList()


        Me.Close()
    End Sub
End Class