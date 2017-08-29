Imports System.Data.SqlClient
Public Class CGSQ_JGD_detail

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        Dim sqlcon As New SqlConnection
        Dim sqlcmd As New SqlCommand
        Dim sqlad As New SqlDataAdapter
        Dim s1 As String = ""

        Dim dt As New DataTable
        Dim sql As String



        sqlcon = getconn()
        sqlcmd.Connection = sqlcon


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



        CType(Me.Owner, CGSQ_JGD).ExDataGridView2.Rows(CType(Me.Owner, CGSQ_JGD).ExDataGridView2.SelectedCells(0).RowIndex).Cells("memo").Value = Me.TextBox1.Text.Trim

        CType(Me.Owner, CGSQ_JGD).ExDataGridView2.Rows(CType(Me.Owner, CGSQ_JGD).ExDataGridView2.SelectedCells(0).RowIndex).Cells("money").Value = Me.TextBox2.Text.Trim

        CType(Me.Owner, CGSQ_JGD).ExDataGridView2.Rows(CType(Me.Owner, CGSQ_JGD).ExDataGridView2.SelectedCells(0).RowIndex).Cells("name").Value = Me.TextBox3.Text.Trim
        CType(Me.Owner, CGSQ_JGD).ExDataGridView2.Rows(CType(Me.Owner, CGSQ_JGD).ExDataGridView2.SelectedCells(0).RowIndex).Cells("price").Value = Me.TextBox5.Text.Trim
        CType(Me.Owner, CGSQ_JGD).ExDataGridView2.Rows(CType(Me.Owner, CGSQ_JGD).ExDataGridView2.SelectedCells(0).RowIndex).Cells("amount").Value = Me.TextBox4.Text.Trim

        CType(Me.Owner, CGSQ_JGD).ExDataGridView2.Rows(CType(Me.Owner, CGSQ_JGD).ExDataGridView2.SelectedCells(0).RowIndex).Cells("supplier_name").Value = Me.ComboBox1.Text
        CType(Me.Owner, CGSQ_JGD).ExDataGridView2.Rows(CType(Me.Owner, CGSQ_JGD).ExDataGridView2.SelectedCells(0).RowIndex).Cells("supplier_no").Value = Me.ComboBox2.Text



        sql = " UPDATE [CGD_detail] " & _
                                "   SET [price] = '" & Me.TextBox5.Text.Trim & "' " & _
                                "      ,[money] = '" & Me.TextBox2.Text.Trim & "' " & _
                                "      ,[supplier_no] = '" & Me.ComboBox2.Text & "' " & _
                                "  where id=" & CType(Me.Owner, CGSQ_JGD).ExDataGridView2.Rows(CType(Me.Owner, CGSQ_JGD).ExDataGridView2.SelectedCells(0).RowIndex).Cells("id").Value
        sqlcmd.CommandText = sql
        sqlcmd.ExecuteNonQuery()




        Me.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.Close()
    End Sub

    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button2.Click
        Me.DialogResult = Windows.Forms.DialogResult.Cancel
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

    Private Sub CGSQ_JGD_detail_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Dim sqlcon As New SqlConnection
        Dim sqlcmd As New SqlCommand
        Dim sqlad As New SqlDataAdapter

        Dim sql As String
        Dim i As Int32
        Dim dt As New DataTable

        sqlcon = getconn()
        sqlcmd.Connection = sqlcon



        Me.ComboBox1.Items.Clear()
        Me.ComboBox2.Items.Clear()
        sql = "select * from t_supplier where stop=1 order by supplier_name"

        sqlcmd.CommandText = sql
        sqlad.SelectCommand = sqlcmd
        dt.Rows.Clear()
        sqlad.Fill(dt)

        For i = 0 To dt.Rows.Count - 1
            Me.ComboBox1.Items.Add(dt.Rows(i)("supplier_name"))
            Me.ComboBox2.Items.Add(dt.Rows(i)("supplier_no"))
        Next

        If CType(Me.Owner, CGSQ_JGD).ExDataGridView2.Rows(CType(Me.Owner, CGSQ_JGD).ExDataGridView2.SelectedCells(0).RowIndex).Cells("supplier_no").Value.ToString.Trim.Length <> 0 Then
            Me.ComboBox2.Text = CType(Me.Owner, CGSQ_JGD).ExDataGridView2.Rows(CType(Me.Owner, CGSQ_JGD).ExDataGridView2.SelectedCells(0).RowIndex).Cells("supplier_no").Value
        End If
    End Sub

    Private Sub ComboBox1_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ComboBox1.SelectedIndexChanged
        Me.ComboBox2.SelectedIndex = Me.ComboBox1.SelectedIndex
    End Sub

    Private Sub ComboBox2_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ComboBox2.SelectedIndexChanged
        Me.ComboBox1.SelectedIndex = Me.ComboBox2.SelectedIndex
    End Sub
End Class