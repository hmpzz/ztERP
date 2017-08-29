Imports System.Data.SqlClient
Public Class add_user
    Public state As String = ""

    Private Sub add_user_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        Dim sqlcon As New SqlConnection
        Dim sqlcmd As New SqlCommand
        Dim sqlad As New SqlDataAdapter


        Dim dt As New DataTable
        Dim sql As String = ""
        Dim i As int32

        sqlcon = getconn()
        sqlcmd.Connection = sqlcon

        If state = "add" Then
            Me.Text = "新建用户"

            sql = "select role_name from  sys_role order by role_name "
            dt.Clear()
            sqlcmd.CommandText = sql
            sqlad.SelectCommand = sqlcmd

            sqlad.Fill(dt)
            Me.ListBox2.Items.Clear()
            For i = 0 To dt.Rows.Count - 1
                Me.ListBox2.Items.Add(dt.Rows(i)("role_name"))
            Next

        ElseIf state = "mod" Then
            Me.Text = "修改用户"

            Me.TextBox1.Text = CType(Me.Owner, sys_user_right).TreeView1.SelectedNode.Tag.ToString.Trim
            Me.TextBox2.Text = CType(Me.Owner, sys_user_right).ToolStripLabel2.Text.Trim

            sql = "select stop_login from sys_user where login_id='" & CType(Me.Owner, sys_user_right).ToolStripLabel2.Text.Trim & "'"
            sqlcmd.CommandText = sql
            'If sqlcmd.ExecuteScalar() = 1 Then
            '    Me.CheckBox1.Checked = False
            'Else

            'End If
            Me.CheckBox1.Checked = IIf(sqlcmd.ExecuteScalar() = 1, False, True)

            sql = "select price from sys_user where login_id='" & CType(Me.Owner, sys_user_right).ToolStripLabel2.Text.Trim & "'"
            sqlcmd.CommandText = sql
            Me.CheckBox2.Checked = IIf(sqlcmd.ExecuteScalar() = 0, False, True)



            Me.TextBox1.Enabled = False
            Me.TextBox2.Enabled = False
            Me.TextBox3.Enabled = False
            Me.TextBox4.Enabled = False
            Me.Button1.Enabled = False



            sql = "select role_name from Sys_Role_User a left join sys_role b on a.role_no=b.role_no where a.login_id='" & Me.TextBox2.Text.Trim & "'"

            sqlcmd.CommandText = sql
            sqlad.SelectCommand = sqlcmd
            dt.Clear()
            sqlad.Fill(dt)

            Me.ListBox1.Items.Clear()

            For i = 0 To dt.Rows.Count - 1
                Me.ListBox1.Items.Add(dt.Rows(i)("role_name"))
            Next


            sql = "select role_name from  sys_role where  role_no not in (select role_no from Sys_Role_User where login_id='" & Me.TextBox2.Text.Trim & "')"

            sqlcmd.CommandText = sql
            sqlad.SelectCommand = sqlcmd
            dt.Clear()
            sqlad.Fill(dt)

            Me.ListBox2.Items.Clear()
            For i = 0 To dt.Rows.Count - 1
                Me.ListBox2.Items.Add(dt.Rows(i)("role_name"))
            Next
        End If
    End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        Dim rs_list As New rs_list
        rs_list.Top = Me.Top + 20
        rs_list.Left = Me.Left
        rs_list.Owner = Me

        rs_list.ShowDialog(Me)



        '        主窗体中()
        '        Dim frm As New subform()
        '        frm.Owner = Me
        '        frm.showdiaolog()
        '        子窗体中()
        'ctype(me.owner,mainform).myControl.......
    End Sub


    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button2.Click
        If Me.ListBox2.SelectedItem Is Nothing Then Exit Sub
        Me.ListBox1.Items.Add(Me.ListBox2.SelectedItem)
        Me.ListBox2.Items.Remove(Me.ListBox2.SelectedItem)
        If Me.ListBox2.Items.Count <> 0 Then
            Me.ListBox2.SelectedItem = Me.ListBox2.Items.Item(0)
        End If
    End Sub

    Private Sub Button3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button3.Click
        If Me.ListBox1.SelectedItem Is Nothing Then Exit Sub
        Me.ListBox2.Items.Add(Me.ListBox1.SelectedItem)
        Me.ListBox1.Items.Remove(Me.ListBox1.SelectedItem)
        If Me.ListBox1.Items.Count <> 0 Then
            Me.ListBox1.SelectedItem = Me.ListBox1.Items.Item(0)
        End If
    End Sub

    Private Sub Button4_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button4.Click
        Dim sqlcon As New SqlConnection
        Dim sqlcmd As New SqlCommand
        Dim sqlad As New SqlDataAdapter
        Dim mytrans As SqlTransaction


        Dim dt As New DataTable
        Dim sql As String = ""

        Dim rsid As Int32 = 0
        Dim B As String = ""
        Dim U As String = ""

        sqlcon = getconn()
        sqlcmd.Connection = sqlcon

        If state = "add" Then
            sql = "select count(*) from t_rs where rs_no='" & Me.TextBox1.Text & "'"

            sqlcmd.CommandText = sql
            If sqlcmd.ExecuteScalar() = 0 Then
                MsgBox("员工编号不正确，请检查后录入！", MsgBoxStyle.OkOnly, "提示")
                Exit Sub
            Else
                sql = "select id from t_rs  where rs_no='" & Me.TextBox1.Text & "'"
                sqlcmd.CommandText = sql
                rsid = sqlcmd.ExecuteScalar()
            End If

            If Me.TextBox2.Text.Trim.Length = 0 Then
                MsgBox("登录帐号不能为空，请检查后录入！", MsgBoxStyle.OkOnly, "提示")
                Exit Sub
            End If

            sql = "select count(*) from sys_user where login_id='" & Me.TextBox2.Text.Trim & "'"
            sqlcmd.CommandText = sql
            If sqlcmd.ExecuteScalar() <> 0 Then
                MsgBox("登录帐号已经存在，请更改后录入！", MsgBoxStyle.OkOnly, "提示")
                Exit Sub
            End If


            If Me.TextBox3.Text.Trim <> Me.TextBox4.Text.Trim Then
                MsgBox("两次密码不同，请重新录入！", MsgBoxStyle.OkOnly, "提示")
                Exit Sub
            End If



            mytrans = sqlcon.BeginTransaction

            sqlcmd.Transaction = mytrans
            Try
                sql = "INSERT INTO Sys_User ( BeginDate,Login_ID,New_pwd,Old_pwd,rs_ID,Stop_Login,Term,price) values " & _
                        "( getdate(), '" & Me.TextBox2.Text.Trim & "', '" & Me.TextBox3.Text.Trim & "', null, '" & rsid & "', " & IIf(Me.CheckBox1.Checked, 0, 1) & ", '30'," & IIf(Me.CheckBox2.Checked, 1, 0) & ")"
                sqlcmd.CommandText = sql
                sqlcmd.ExecuteNonQuery()
                Call save_right(Me.TextBox2.Text.Trim, sqlcmd)
                mytrans.Commit()


            Catch ex As Exception
                mytrans.Rollback()
                MsgBox(ex.Message, MsgBoxStyle.OkOnly, "错误")
                Exit Sub
            End Try




        ElseIf state = "mod" Then

            sql = "select id from t_rs  where rs_no='" & Me.TextBox1.Text & "'"
            sqlcmd.CommandText = sql
            rsid = sqlcmd.ExecuteScalar()

            mytrans = sqlcon.BeginTransaction

            sqlcmd.Transaction = mytrans
            Try

                sql = "update sys_user set stop_login=" & IIf(Me.CheckBox1.Checked = True, 0, 1) & ",price=" & IIf(Me.CheckBox2.Checked, 1, 0) & " where login_id='" & CType(Me.Owner, sys_user_right).ToolStripLabel2.Text.Trim & "'"
                sqlcmd.CommandText = sql
                sqlcmd.ExecuteNonQuery()

                Call save_right(Me.TextBox2.Text.Trim, sqlcmd)
                mytrans.Commit()


            Catch ex As Exception
                mytrans.Rollback()
                MsgBox(ex.Message, MsgBoxStyle.OkOnly, "错误")
                Exit Sub
            End Try

        End If

        MsgBox("保存成功！", MsgBoxStyle.Information, "提示")
        CType(Me.Owner, sys_user_right).Refresh_TreeView()


        If CType(Me.Owner, sys_user_right).ToolStripButton_stop.Checked = Me.CheckBox1.Checked Then

            sql = "select b.bmno from t_rs a left join t_bm b on a.bm_id=b.id where a.id=" & rsid
            sqlcmd.CommandText = sql
            B = "B" & sqlcmd.ExecuteScalar()
            U = "U" & Me.TextBox2.Text.Trim
            CType(Me.Owner, sys_user_right).TreeView1.SelectedNode = CType(Me.Owner, sys_user_right).TreeView1.Nodes("alluser").Nodes(B).Nodes(U)

        End If

        Me.Close()
    End Sub

    Private Sub save_right(ByVal loginid As String, ByVal sqlcmd As SqlCommand)

        Dim sql As String
        Dim s As String = ""
        Dim i As Int32


        For i = 0 To Me.ListBox1.Items.Count - 1
            s = s & "'" & Me.ListBox1.Items.Item(i) & "',"
        Next



        sql = "delete from sys_role_user where login_id='" & loginid & "'"
        sqlcmd.CommandText = sql
        sqlcmd.ExecuteNonQuery()


        If s <> "" Then
            s = Microsoft.VisualBasic.Left(s, s.Length - 1)
            sql = "insert into sys_role_user (login_id,role_no) select  '" & loginid & "' as login_id,role_no from sys_role where role_name in (" & s & ")"
            sqlcmd.CommandText = sql
            sqlcmd.ExecuteNonQuery()

            sql = "delete from sys_user_right where login_id='" & loginid & "' and menu_no not in (SELECT DISTINCT b.Menu_No FROM Sys_Role_User a LEFT OUTER JOIN Sys_Role_Right b ON a.Role_No = b.Role_No WHERE (a.Login_ID = '" & loginid & "') ) "
            sqlcmd.CommandText = sql
            sqlcmd.ExecuteNonQuery()

            sql = "insert into sys_user_right (menu_no,login_id) SELECT DISTINCT b.Menu_No, '" & loginid & "' AS login_id FROM Sys_Role_User a LEFT OUTER JOIN Sys_Role_Right b ON a.Role_No = b.Role_No WHERE (a.Login_ID = '" & loginid & "') and menu_no not in (select menu_no from sys_user_right where login_id='" & loginid & "')"
            sqlcmd.CommandText = sql
            sqlcmd.ExecuteNonQuery()

        End If

    End Sub

    Private Sub Button5_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button5.Click
        Me.Close()
    End Sub
End Class