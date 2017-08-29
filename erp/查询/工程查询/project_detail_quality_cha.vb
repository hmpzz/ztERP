Imports System.Windows.Forms

Public Class project_detail_quality_cha

    Private Sub OK_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OK_Button.Click
        Dim s As String = ""


        If Me.CheckBox1.Checked Then
            s = s & "'" & Format(Me.DateTimePicker1.Value, "yyyy-MM-dd") & " 00:00:00' and '" & Format(Me.DateTimePicker2.Value, "yyyy-MM-dd") & " 23:59:59'"
        End If




        CType(Me.Owner, project_detail_quality).cha = s
        CType(Me.Owner, project_detail_quality).Detail_check.Checked = True


        Me.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.Close()
    End Sub

    Private Sub Cancel_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cancel_Button.Click
        Me.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub

End Class
