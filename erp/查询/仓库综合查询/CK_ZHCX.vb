Imports System.Data.SqlClient
Public Class CK_ZHCX
    Public cha As String


    Private Sub ToolStripButton_exit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripButton_exit.Click
        Me.Close()
    End Sub


    Private Sub ToolStripButton_excel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripButton_excel.Click
        Dim ToExcel As New BaseClass.TOEXCEL

        If ToExcel.ExToExcel(Me.ExDataGridView1, Me.SaveFileDialog1, True, "数量") = True Then
            MsgBox("导出成功！", MsgBoxStyle.OkOnly, "提示")
        End If
    End Sub



    Private Sub ToolStripButton_question_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripButton_question.Click
        Dim CK_ZHCX_cha As New CK_ZHCX_cha
        CK_ZHCX_cha.Owner = Me
        cha = ""
        CK_ZHCX_cha.ShowDialog()

        If CK_ZHCX_cha.DialogResult = Windows.Forms.DialogResult.OK Then
            Refresh_ExDataGridView_master(cha)
        Else
            Exit Sub
        End If
    End Sub

    Private Sub Refresh_ExDataGridView_master(Optional ByVal s As String = "")
        Dim sqlcon As New SqlConnection
        Dim sqlcmd As New SqlCommand
        Dim sqlad As New SqlDataAdapter


        Dim dt As New DataTable
        Dim sql As String
        Dim i As Int32



        sqlcon = getconn()
        sqlcmd.Connection = sqlcon


        sql = s

        sqlcmd.CommandText = sql
        sqlad.SelectCommand = sqlcmd
        dt.Rows.Clear()
        sqlad.Fill(dt)

        Me.ExDataGridView1.SetDataSource(dt)

        For i = 0 To dt.Columns.Count - 1
            If dt.Columns.Item(i).ColumnName = "日期" Then
                Me.ExDataGridView1.AddColumn(dt.Columns.Item(i).ColumnName, dt.Columns.Item(i).ColumnName, , , , , "yyyy-MM-dd")
            ElseIf dt.Columns.Item(i).ColumnName = "数量" Then
                Me.ExDataGridView1.AddColumn(dt.Columns.Item(i).ColumnName, dt.Columns.Item(i).ColumnName, , , , DataGridViewContentAlignment.MiddleRight, "#0.0000")
            ElseIf dt.Columns.Item(i).ColumnName = "单价" Then
                Me.ExDataGridView1.AddColumn(dt.Columns.Item(i).ColumnName, dt.Columns.Item(i).ColumnName, , , , DataGridViewContentAlignment.MiddleRight, "#0.0000")
            ElseIf dt.Columns.Item(i).ColumnName = "金额" Then
                Me.ExDataGridView1.AddColumn(dt.Columns.Item(i).ColumnName, dt.Columns.Item(i).ColumnName, , , , DataGridViewContentAlignment.MiddleRight, "#0.00")
            Else
                Me.ExDataGridView1.AddColumn(dt.Columns.Item(i).ColumnName, dt.Columns.Item(i).ColumnName)
            End If

        Next

    End Sub
End Class