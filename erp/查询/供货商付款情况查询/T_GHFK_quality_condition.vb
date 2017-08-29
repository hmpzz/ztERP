Imports System.Data.SqlClient
Public Class T_GHFK_quality_condition

    Private Sub T_GHFK_quality_condition_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        Dim sqlcon As New SqlConnection
        Dim sqlcmd As New SqlCommand
        Dim sqlad As New SqlDataAdapter

        Dim dt As New DataTable
        Dim sql As String

        sqlcon = getconn()
        sqlcmd.Connection = sqlcon
        sqlcmd.CommandTimeout = 0



        sql = "select * from t_supplier where stop=1 order by supplier_name"

        sqlcmd.CommandText = sql
        sqlad.SelectCommand = sqlcmd
        dt.Rows.Clear()
        sqlad.Fill(dt)

     
        Me.ExDataGridView1.SetDataSource(dt)
        Me.ExDataGridView1.Columns.Clear()



        Me.ExDataGridView1.AddColumn("check", "", 20, False, New DataGridViewCheckBoxColumn)
        Me.ExDataGridView1.AddColumn("supplier_no", "供应商编号")
        Me.ExDataGridView1.AddColumn("supplier_name", "供应商名字", 160)

    End Sub



    Private Sub 全选ToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles 全选ToolStripMenuItem.Click
        Dim i As Integer
        For i = 0 To Me.ExDataGridView1.RowCount - 1
            Me.ExDataGridView1.Rows(i).Cells("check").Value = True
        Next
    End Sub

    Private Sub 反选ToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles 反选ToolStripMenuItem.Click
        Dim i As Integer
        For i = 0 To Me.ExDataGridView1.RowCount - 1
            Me.ExDataGridView1.Rows(i).Cells("check").Value = Not Me.ExDataGridView1.Rows(i).Cells("check").Value
        Next
    End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        Me.DialogResult = Windows.Forms.DialogResult.Cancel
    End Sub

    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button2.Click

        Dim i As Integer
        Dim s As String = ""

        For i = 0 To Me.ExDataGridView1.Rows.Count - 1
            If Me.ExDataGridView1.Rows(i).Cells("check").Value Then
                s = s & "'" & Me.ExDataGridView1.Rows(i).Cells("supplier_no").Value.ToString & "',"
            End If

        Next

        If s.Trim.Length > 0 Then
            s = Microsoft.VisualBasic.Left(s, s.Length - 1)
        End If

        CType(Me.Owner, T_GHFK_quality).cha = s
        CType(Me.Owner, T_GHFK_quality).CheckBox1.Checked = True

        Me.DialogResult = Windows.Forms.DialogResult.OK
    End Sub

    Private Sub Button3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)

    End Sub
End Class