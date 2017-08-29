Public Class t_outlay_add_detail

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click



        Me.TextBox1.Focus()

        If Not IsNumeric(Me.TextBox1.Text.Trim) Then
            MsgBox("金额必须是数值！", MsgBoxStyle.OkOnly, "提示")
            Exit Sub
        End If




        Dim DR As DataRow = CType(Me.Owner, t_outlay_add).dt_P.NewRow

        DR("memo") = Me.TextBox2.Text.Trim

        DR("money") = Me.TextBox1.Text.Trim
        DR("id") = 0



        CType(Me.Owner, t_outlay_add).dt_P.Rows.Add(DR)

        Me.DialogResult = Windows.Forms.DialogResult.OK
    End Sub

    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button2.Click
        Me.DialogResult = Windows.Forms.DialogResult.Cancel
    End Sub
End Class