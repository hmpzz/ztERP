Imports System.Windows.Forms
Imports System.Data.SqlClient

Public Class newpassword


    Private Sub OK_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OK_Button.Click
        Dim sqlcon As New SqlConnection
        Dim sqlcmd As New SqlCommand
        Dim sqlad As New SqlDataAdapter
        Dim dt As New DataTable

        Dim sql As String

       

        sqlcon = getconn()
        sql = "select count(*) from view_login where login_id='" & login_id & "' and new_pwd='" & Me.TextBox1.Text.Trim & "' and stop_login=1 "
        sqlcmd.Connection = sqlcon
        sqlcmd.CommandText = sql

        If sqlcmd.ExecuteScalar() = 0 Then
            MsgBox("ԭʼ���벻��ȷ�����������룡", MsgBoxStyle.OkOnly, "��ʾ")
            Exit Sub
        End If

        If Me.TextBox2.Text.Length = 0 Then
            MsgBox("������������¼��", MsgBoxStyle.OkOnly, "��ʾ")
            Exit Sub
        End If

        If Me.TextBox2.Text <> Me.TextBox3.Text Then
            MsgBox("������������ܲ�ͬ�����������룡", MsgBoxStyle.OkOnly, "��ʾ")
            Exit Sub
        End If

        sql = "update sys_user set new_pwd='" & Me.TextBox2.Text & "' where login_id='" & login_id & "'"
        sqlcmd.Connection = sqlcon
        sqlcmd.CommandText = sql
        sqlcmd.ExecuteNonQuery()

        MsgBox("�޸���ɣ�", MsgBoxStyle.OkOnly, "��ʾ")

        'Me.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.Close()


    End Sub

    Private Sub Cancel_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cancel_Button.Click
        'Me.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub

    Private Sub newpassword_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Me.TextBox1.Text = ""
        Me.TextBox2.Text = ""
        Me.TextBox3.Text = ""

    End Sub
End Class
