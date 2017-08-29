Imports System.Data.SqlClient
Public Class project_YQ
    Public billno As String

    Private Sub project_YQ_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

    End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        Dim sqlcon As New SqlConnection
        Dim sqlcmd As New SqlCommand

        Dim sql As String


        sqlcon = getconn()
        sqlcmd.Connection = sqlcon

        sql = "update project_master set   effective_date='" & Format(Me.DateTimePicker1.Value, "yyyy-MM-dd") & " 00:00:00' " & _
                "  where project_no='" & billno & "'"


        sqlcmd.CommandText = sql
        sqlcmd.ExecuteNonQuery()



        CType(Me.Owner, project_check).ExDataGridView1.Rows(CType(Me.Owner, project_check).ExDataGridView1.SelectedCells(0).RowIndex).Cells("effective_date").Value() = Me.DateTimePicker1.Value

        Me.DialogResult = Windows.Forms.DialogResult.OK

    End Sub

    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button2.Click
        Me.DialogResult = Windows.Forms.DialogResult.Cancel
    End Sub
End Class