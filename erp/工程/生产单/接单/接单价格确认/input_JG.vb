Public Class input_JG

    Private Sub input_JG_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

    End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click

        If Me.TextBox1.Text.Trim.Length = 0 Then
            MsgBox("��¼��۸�", MsgBoxStyle.OkOnly)
            Exit Sub
        End If

        If Not IsNumeric(Me.TextBox1.Text.Trim) Then
            MsgBox("��۸�ֻ�������֣�", MsgBoxStyle.OkOnly)
            Exit Sub
        End If

        If CDec(Me.TextBox1.Text.Trim) < 0 Then
            MsgBox("��۸������ڵ���0��", MsgBoxStyle.OkOnly)
            Exit Sub
        End If

        Me.Owner.Tag = Me.TextBox1.Text
        Me.DialogResult = Windows.Forms.DialogResult.OK
    End Sub

    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button2.Click
        Me.DialogResult = Windows.Forms.DialogResult.Cancel
    End Sub
End Class