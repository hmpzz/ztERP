Imports System.Data.SqlClient
Public Class t_FPYS_HX_mod
    Friend billno As String

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click

        Dim sql As String = ""
        Dim sqlcon As New SqlConnection
        Dim sqlcmd As New SqlCommand
        Dim sqlad As New SqlDataAdapter

        If Me.TextBox2.Text.Trim.Length = 0 Then
            MsgBox("请输入发票号！", MsgBoxStyle.OkOnly, "提示")
            Exit Sub
        End If
        sqlcon = getconn()
        sqlcmd.Connection = sqlcon

        sql = "update T_FPYS_master set fp_no='" & Me.TextBox2.Text.Trim & "',kp_type='" & Me.ComboBox1.Text.Trim & "',kp_state=1 where billno='" & billno & "' "
        sqlcmd.CommandText = sql
        sqlcmd.ExecuteNonQuery()

        CType(Me.Owner, t_FPYS_HX).ExDataGridView1.Rows(CType(Me.Owner, t_FPYS_HX).ExDataGridView1.SelectedCells(0).RowIndex).Cells("fp_no").Value = Me.TextBox2.Text.Trim
        CType(Me.Owner, t_FPYS_HX).ExDataGridView1.Rows(CType(Me.Owner, t_FPYS_HX).ExDataGridView1.SelectedCells(0).RowIndex).Cells("kp_type").Value = Me.ComboBox1.Text.Trim
        CType(Me.Owner, t_FPYS_HX).ExDataGridView1.Rows(CType(Me.Owner, t_FPYS_HX).ExDataGridView1.SelectedCells(0).RowIndex).Cells("KP_state1").Value = "已开票"

        Me.Close()
    End Sub

    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button2.Click
        Me.Close()
    End Sub

    Private Sub t_FPYS_HX_mod_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Me.ComboBox1.Items.Clear()
        Me.ComboBox1.Items.Add("")
        Me.ComboBox1.Items.Add("建安票")
        Me.ComboBox1.Items.Add("增值税票")
        Me.ComboBox1.Items.Add("普通发票")
        Me.ComboBox1.Items.Add("收据")
        Me.ComboBox1.Items.Add("无收据")


        Me.TextBox2.Text = CType(Me.Owner, t_FPYS_HX).ExDataGridView1.Rows(CType(Me.Owner, t_FPYS_HX).ExDataGridView1.SelectedCells(0).RowIndex).Cells("fp_no").Value
        Me.ComboBox1.Text = CType(Me.Owner, t_FPYS_HX).ExDataGridView1.Rows(CType(Me.Owner, t_FPYS_HX).ExDataGridView1.SelectedCells(0).RowIndex).Cells("kp_type").Value

    End Sub
End Class