Imports System.Data.SqlClient
Public Class copy_user_right

    Private Sub copy_user_right_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Dim sqlcon As New SqlConnection
        Dim sqlcmd As New SqlCommand
        Dim sqlad As New SqlDataAdapter


        Dim dt As New DataTable
        Dim sql As String = ""
        Dim i As Int32


        Me.TextBox1.Enabled = False
        Me.TextBox2.Enabled = False

        Me.TextBox1.Text = CType(Me.Owner, sys_user_right).ToolStripLabel2.Text.Trim
        Me.TextBox2.Text = CType(Me.Owner, sys_user_right).TreeView1.SelectedNode.Text


        Me.Label6.Text = ""



        sqlcon = getconn()
        sql = "select distinct c.bmmc,c.bmno  " & _
                "from sys_user a left join  " & _
                "	t_rs b on a.rs_id=b.id left join  " & _
                "	t_bm c on b.bm_id=c.id  " & _
                "where login_id<>'admin' and a.stop_login=1 and a.login_id<>'" & Me.TextBox1.Text.Trim & "' " & _
                "order by bmmc"

        sqlcmd.Connection = sqlcon
        sqlcmd.CommandText = sql
        sqlad.SelectCommand = sqlcmd
        dt.Rows.Clear()
        sqlad.Fill(dt)

        Me.ComboBox1.Items.Clear()
        Me.ComboBox4.Items.Clear()

        For i = 0 To dt.Rows.Count - 1
            Me.ComboBox1.Items.Add(dt.Rows(i)("bmmc"))
            Me.ComboBox4.Items.Add(dt.Rows(i)("bmno"))
        Next



    End Sub

    Private Sub ComboBox2_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ComboBox2.SelectedIndexChanged
        Me.ComboBox3.SelectedIndex = Me.ComboBox2.SelectedIndex
    End Sub

    Private Sub ComboBox3_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ComboBox3.SelectedIndexChanged
        Me.Label6.Text = Me.ComboBox3.Text
    End Sub

    Private Sub ComboBox1_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ComboBox1.SelectedIndexChanged
        Me.ComboBox4.SelectedIndex = Me.ComboBox1.SelectedIndex
    End Sub

    Private Sub ComboBox4_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ComboBox4.SelectedIndexChanged
        Dim sqlcon As New SqlConnection
        Dim sqlcmd As New SqlCommand
        Dim sqlad As New SqlDataAdapter


        Dim dt As New DataTable
        Dim sql As String = ""
        Dim i As Int32

        sqlcon = getconn()
        sql = "select distinct b.rs_name,a.login_id  " & _
                "from sys_user a left join  " & _
                "	t_rs b on a.rs_id=b.id left join  " & _
                "	t_bm c on b.bm_id=c.id  " & _
                "where login_id<>'admin' and a.stop_login=1 and c.bmno='" & Me.ComboBox4.Text & "' and a.login_id<>'" & Me.TextBox1.Text.Trim & "' " & _
                "order by b.rs_name"

        sqlcmd.Connection = sqlcon
        sqlcmd.CommandText = sql
        sqlad.SelectCommand = sqlcmd
        dt.Rows.Clear()
        sqlad.Fill(dt)

        Me.ComboBox2.Items.Clear()
        Me.ComboBox3.Items.Clear()

        For i = 0 To dt.Rows.Count - 1
            Me.ComboBox2.Items.Add(dt.Rows(i)("rs_name"))
            Me.ComboBox3.Items.Add(dt.Rows(i)("login_id"))
        Next
    End Sub

    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button2.Click
        Me.Close()
    End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        Dim sqlcon As New SqlConnection
        Dim sqlcmd As New SqlCommand
        Dim sqlad As New SqlDataAdapter

        Dim mytrans As SqlTransaction

        Dim sql As String = ""


        sqlcon = getconn()
        sqlcmd.Connection = sqlcon

        mytrans = sqlcon.BeginTransaction
        sqlcmd.Transaction = mytrans
        Try



            If Me.RadioButton1.Checked Then
                '继承
                inheritance_user_right(sqlcmd)


            ElseIf Me.RadioButton2.Checked Then
                '复制
            End If

            mytrans.Commit()
        Catch ex As Exception
            mytrans.Rollback()
            MsgBox(ex.Message, MsgBoxStyle.OkOnly, "提示")
            Exit Sub
        End Try

        MsgBox("操作成功！", MsgBoxStyle.OkOnly, "提示")
    End Sub

    '继承
    Private Sub inheritance_user_right(ByVal sqlcmd As SqlCommand)
        Dim sql As String

        sql = "insert into Sys_Role_User (login_id,role_no) select '" & Me.Label6.Text & "',role_no from sys_role_user where login_id='" & Me.TextBox1.Text.Trim & "' and role_no not in (select role_no from sys_role_user where login_id ='" & Me.Label6.Text & "')"
        sqlcmd.CommandText = Sql
        sqlcmd.ExecuteNonQuery()

        Sql = "insert into Sys_User_Right (login_id,menu_no,menu_id,menu_name,in_user) " & _
            "select '" & Me.Label6.Text & "',menu_no,menu_id,menu_name,in_user from sys_user_right where login_id='" & Me.TextBox1.Text.Trim & "' and menu_no not in (select menu_no from Sys_User_Right where login_id='" & Me.Label6.Text & "')"
        sqlcmd.CommandText = Sql
        sqlcmd.ExecuteNonQuery()

        sql = "update a set a.in_user=1 from sys_user_right a where login_id='" & Me.Label6.Text & "' and a.in_user=0 and a.menu_no in (select menu_no from sys_user_right where  in_user=1 and login_id='" & Me.TextBox1.Text.Trim & "' )"
        sqlcmd.CommandText = Sql
        sqlcmd.ExecuteNonQuery()
    End Sub
    '复制
    Private Sub copy_user_right(ByVal sqlcmd As SqlCommand)

    End Sub
End Class