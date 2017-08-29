Imports System.Data.SqlClient
Imports System.IO
Public Class LoginForm

    ' TODO: 插入代码，以使用提供的用户名和密码执行自定义的身份验证
    ' (请参见 http://go.microsoft.com/fwlink/?LinkId=35339)。 
    ' 随后自定义主体可附加到当前线程的主体，如下所示: 
    '     My.User.CurrentPrincipal = CustomPrincipal
    ' 其中 CustomPrincipal 是用于执行身份验证的 IPrincipal 实现。 
    ' 随后，My.User 将返回 CustomPrincipal 对象中封装的标识信息
    ' 如用户名、显示名等
    Dim k As Integer = 0

    Private Sub OK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OK.Click
        Dim sqlcon As New SqlConnection
        Dim sqlcmd As New SqlCommand
        Dim sqlad As New SqlDataAdapter
        Dim dt As New DataTable

        Dim sql As String

        If Me.PasswordTextBox.Text.Length = 0 Then
            MsgBox("不允许空密码登录！", MsgBoxStyle.OkOnly, "提示")
            Exit Sub
        End If


        sqlcon = getconn()
        sql = "select * from view_login where login_id='" & Me.UsernameTextBox.Text.Trim & "' and new_pwd='" & Me.PasswordTextBox.Text & "' and stop_login=1"
        sqlcmd.Connection = sqlcon
        sqlcmd.CommandText = sql
        sqlad.SelectCommand = sqlcmd
        dt.Clear()
        sqlad.Fill(dt)

        If dt.Rows.Count = 1 Then
            '登录代码

            login_id = dt.Rows(0)("login_id")

            If login_id = "admin" Then
                rs_name = "系统管理员"
                RS_ID = 0
                BM_id = 49
            Else
                rs_name = dt.Rows(0)("rs_name")
                RS_ID = dt.Rows(0)("rs_id")
                price_state = dt.Rows(0)("price")
                BM_id = dt.Rows(0)("bm_id")
            End If

            Dim path As String
            path = Application.StartupPath + "\ini.ini"
            WriteINI("user", "username", Me.UsernameTextBox.Text.Trim, path)



            If Not Directory.Exists(Application.StartupPath & "\pic") Then
                Directory.CreateDirectory(Application.StartupPath & "\pic")
            End If

        Else
            k = k + 1
            If k = 3 Then
                MsgBox("输入错三次密码，系统关闭！", MsgBoxStyle.OkOnly, "提示")
                End
            Else
                MsgBox("用户名或者密码错误，请重新输入！", MsgBoxStyle.OkOnly, "提示")
                Exit Sub
            End If
        End If


        Me.Close()

    End Sub

    Private Sub Cancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cancel.Click
        End
    End Sub


   
    Private Sub LoginForm_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Dim path As String

        Dim sqlcon As New SqlConnection
        Dim sqlcmd As New SqlCommand
        Dim sql As String

        Dim files() As String

        Dim cmd() As String
        Dim myArg() As String

        Dim b As Boolean = False
        Dim i As int32
        Dim s() As String = Nothing


        path = Application.StartupPath + "\ini.ini"



        myArg = System.Environment.GetCommandLineArgs
        'MsgBox(myArg(0))

        s = Split(myArg(0), "\")
        s(s.Length - 1).Trim()
        'Debug.Print(Application.StartupPath)
        'MsgBox(s(s.Length - 1).Trim().Replace(".exe", ""))

        If Application.StartupPath.ToUpper.IndexOf("DEBUG") <= 0 Then

            'Shell(Application.StartupPath & "\自动更新.exe /" & s(s.Length - 1).Trim.Replace(".exe", ""))


            sqlcon = getconn()
            sqlcmd.Connection = sqlcon

            sql = "select update_path from t_update where name='" & s(s.Length - 1).Trim().Replace(".exe", "") & "'"
            sqlcmd.CommandText = sql
            path = sqlcmd.ExecuteScalar()

            'MsgBox(path)

            files = System.IO.Directory.GetFiles(path)

            For i = 0 To files.Length - 1


                cmd = Split(files(i), "\")
                cmd(cmd.Length - 1).Trim()
                'application.StartupPath
                If File.Exists(Application.StartupPath.ToString & "\" & cmd(cmd.Length - 1).Trim) = True Then

                    If File.GetLastWriteTime(files(i)) > File.GetLastWriteTime(Application.StartupPath.ToString & "\" & cmd(cmd.Length - 1).Trim) Then
                        b = True
                    End If
                Else
                    b = True
                End If
            Next

            If b = True Then


                If sqlserver.ToUpper = "H" Then

                    Shell(Application.StartupPath & "\自动更新.exe /" & s(s.Length - 1).Trim.Replace(".exe", "") & ",sa,guochunqi")
                ElseIf sqlserver.ToUpper = "ERP" Then
                    Shell(Application.StartupPath & "\自动更新.exe /" & s(s.Length - 1).Trim.Replace(".exe", "") & ",sa,w1j2j4erp")
                Else 'If sqlserver.ToUpper = "SERVER" Then

                    Shell(Application.StartupPath & "\自动更新.exe /" & s(s.Length - 1).Trim.Replace(".exe", "") & ",sa,w1j2j4.110")
                End If

                End
            End If


            For i = 0 To files.Length - 1


                cmd = Split(files(i), "\")
                cmd(cmd.Length - 1).Trim()

                '用于判断是否已经更新
                If File.Exists(Application.StartupPath.ToString & "\" & cmd(cmd.Length - 1).Trim) = True Then

                    If File.GetLastWriteTime(files(i)) > File.GetLastWriteTime(Application.StartupPath.ToString & "\" & cmd(cmd.Length - 1).Trim) Then
                        '如果未更新即服务器上的文件比现在的文件时间靠后,则退出
                        MsgBox("程序未更新,不能启动!", MsgBoxStyle.OkOnly, "提示")
                        End
                    End If
                Else
                    MsgBox("程序未更新,不能启动!", MsgBoxStyle.OkOnly, "提示")
                    End
                End If
            Next i

        End If



        path = Application.StartupPath + "\ini.ini"
        Me.UsernameTextBox.Text = GetINI("user", "username", "", path)


    End Sub

    
End Class
