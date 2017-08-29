Imports System.Windows.Forms

Public Class FPYF_cha

    Private Sub OK_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OK_Button.Click


        Me.Owner.Tag = " and a.billno in (SELECT     billno     FROM T_FPYF_detail WHERE     (RK_billno = '" & Me.TextBox1.Text.Trim & "'))"

        Me.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.Close()
    End Sub

    Private Sub Cancel_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cancel_Button.Click
        Me.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub

    Private Sub CheckBox1_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CheckBox1.CheckedChanged
        If Not Me.CheckBox1.Checked Then
            Me.TextBox1.Text = ""
        End If
    End Sub

    Private Sub TextBox1_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TextBox1.TextChanged
        If Me.TextBox1.Text.Trim.Length > 0 Then
            Me.CheckBox1.Checked = True
        End If
    End Sub
End Class
