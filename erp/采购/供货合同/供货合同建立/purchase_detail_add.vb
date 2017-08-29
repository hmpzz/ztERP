Imports System.Windows.Forms

Public Class purchase_detail_add

    Private Sub OK_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OK_Button.Click
        If Not IsNumeric(Me.TextBox1.Text.Trim) Then
            MsgBox("数量必须是数值，请检查后保存！", MsgBoxStyle.OkOnly, "提示")
            Exit Sub
        End If

        If Not IsNumeric(Me.TextBox2.Text.Trim) Then
            MsgBox("单价必须是数值，请检查后保存！", MsgBoxStyle.OkOnly, "提示")
            Exit Sub
        End If



        CType(Me.Owner, purchase_add).ExDataGridView1.Rows(CType(Me.Owner, purchase_add).ExDataGridView1.SelectedCells(0).RowIndex).Cells("amount").Value = Me.TextBox1.Text.Trim
        CType(Me.Owner, purchase_add).ExDataGridView1.Rows(CType(Me.Owner, purchase_add).ExDataGridView1.SelectedCells(0).RowIndex).Cells("price").Value = Me.TextBox2.Text.Trim
        CType(Me.Owner, purchase_add).ExDataGridView1.Rows(CType(Me.Owner, purchase_add).ExDataGridView1.SelectedCells(0).RowIndex).Cells("money").Value = Me.TextBox3.Text.Trim


        Me.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.Close()
    End Sub

    Private Sub Cancel_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cancel_Button.Click
        Me.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub

    Private Sub TextBox1_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles TextBox1.LostFocus
        If Not IsNumeric(Me.TextBox1.Text.Trim) Then
            MsgBox("数量必须是数字！", MsgBoxStyle.OkOnly, "提示")
            Exit Sub
        End If
        Me.TextBox3.Text = Convert.ToDecimal(Me.TextBox1.Text) * Convert.ToDecimal(Me.TextBox2.Text)
    End Sub

 
    Private Sub TextBox1_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TextBox1.TextChanged

    End Sub

    Private Sub TextBox2_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles TextBox2.LostFocus
        If Not IsNumeric(Me.TextBox2.Text.Trim) Then
            MsgBox("单价必须是数字！", MsgBoxStyle.OkOnly, "提示")
            Exit Sub
        End If
        Me.TextBox3.Text = Convert.ToDecimal(Me.TextBox1.Text) * Convert.ToDecimal(Me.TextBox2.Text)
    End Sub

  
    Private Sub TextBox2_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TextBox2.TextChanged

    End Sub

    Private Sub TextBox3_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles TextBox3.LostFocus
        If Not IsNumeric(Me.TextBox3.Text.Trim) Then
            MsgBox("数量必须是数字！", MsgBoxStyle.OkOnly, "提示")
            Exit Sub
        End If
        Me.TextBox2.Text = Format(Convert.ToDecimal(Me.TextBox3.Text) / Convert.ToDecimal(Me.TextBox1.Text), "#0.0000")
    End Sub

   

   
End Class
