Public Class t_outwork_detail_add
    Friend state As String

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        If Me.TextBox2.Text.Trim.Length = 0 Then
            MsgBox("金额不能为空！", MsgBoxStyle.OkOnly, "提示")
            Exit Sub
        ElseIf Not IsNumeric(Me.TextBox2.Text.Trim) Then
            MsgBox("金额必须是数值！", MsgBoxStyle.OkOnly, "提示")
            Exit Sub
        ElseIf Me.TextBox2.Text.Trim < 0 Then
            MsgBox("金额必须大于等于0！", MsgBoxStyle.OkOnly, "提示")
            Exit Sub
        End If

        If state = "new" Then
            Dim DR As DataRow = CType(Me.Owner, t_outwork_add).dt_p.NewRow
            DR("name") = Me.TextBox3.Text
            DR("memo") = Me.TextBox1.Text
            DR("amount") = Me.TextBox4.Text
            DR("price") = Me.TextBox5.Text
            DR("money") = Me.TextBox2.Text.Trim
            DR("ID") = 0
            CType(Me.Owner, t_outwork_add).dt_p.Rows.Add(DR)

        ElseIf state = "mod" Then

            CType(Me.Owner, t_outwork_add).ExDataGridView1.Rows(CType(Me.Owner, t_outwork_add).ExDataGridView1.SelectedCells(0).RowIndex).Cells("memo").Value = Me.TextBox1.Text.Trim

            CType(Me.Owner, t_outwork_add).ExDataGridView1.Rows(CType(Me.Owner, t_outwork_add).ExDataGridView1.SelectedCells(0).RowIndex).Cells("money").Value = Me.TextBox2.Text.Trim

            CType(Me.Owner, t_outwork_add).ExDataGridView1.Rows(CType(Me.Owner, t_outwork_add).ExDataGridView1.SelectedCells(0).RowIndex).Cells("name").Value = Me.TextBox3.Text.Trim
            CType(Me.Owner, t_outwork_add).ExDataGridView1.Rows(CType(Me.Owner, t_outwork_add).ExDataGridView1.SelectedCells(0).RowIndex).Cells("price").Value = Me.TextBox5.Text.Trim
            CType(Me.Owner, t_outwork_add).ExDataGridView1.Rows(CType(Me.Owner, t_outwork_add).ExDataGridView1.SelectedCells(0).RowIndex).Cells("amount").Value = Me.TextBox4.Text.Trim



           
        End If

        Me.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.Close()
    End Sub

    Private Sub t_outwork_detail_add_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If state = "new" Then
            Me.Text = "新增"

            Me.TextBox5.Text = 0
            Me.TextBox2.Text = 0
            Me.TextBox4.Text = 0

        ElseIf state = "mod" Then
            Me.Text = "修改"
        End If
    End Sub

    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button2.Click
        Me.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Close()

    End Sub

    Private Sub TextBox4_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles TextBox4.LostFocus
        If Not IsNumeric(Me.TextBox4.Text.Trim) Then
            MsgBox("数量必须是数字！", MsgBoxStyle.OkOnly, "提示")
            Exit Sub
        End If
        Me.TextBox2.Text = Convert.ToDecimal(Me.TextBox4.Text) * Convert.ToDecimal(Me.TextBox5.Text)
    End Sub

    Private Sub TextBox4_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TextBox4.TextChanged

    End Sub

    Private Sub TextBox5_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles TextBox5.LostFocus
        If Not IsNumeric(Me.TextBox5.Text.Trim) Then
            MsgBox("单价必须是数字！", MsgBoxStyle.OkOnly, "提示")
            Exit Sub
        End If
        Me.TextBox2.Text = Convert.ToDecimal(Me.TextBox4.Text) * Convert.ToDecimal(Me.TextBox5.Text)
    End Sub

    Private Sub TextBox5_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TextBox5.TextChanged

     
    End Sub

    Private Sub TextBox2_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles TextBox2.LostFocus
        If Not IsNumeric(Me.TextBox2.Text.Trim) Then
            MsgBox("金额必须是数字！", MsgBoxStyle.OkOnly, "提示")
            Exit Sub
        End If
        If Me.TextBox4.Text = 0 Or Me.TextBox4.Text.Trim.Length = 0 Then
            MsgBox("请先填写数量!", MsgBoxStyle.OkOnly, "提示")
            Me.TextBox2.Text = 0
        Else
            Me.TextBox5.Text = Convert.ToDecimal(Me.TextBox2.Text) / Convert.ToDecimal(Me.TextBox4.Text)
        End If

    End Sub

    Private Sub TextBox2_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TextBox2.TextChanged

    End Sub
End Class