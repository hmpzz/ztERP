Imports System.Data.SqlClient
Public Class project_detail_add

    Private Sub project_detail_add_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load


    End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        Dim sqlcon As New SqlConnection
        Dim sqlcmd As New SqlCommand
        Dim mytrans As SqlTransaction


        Dim sql As String = ""

        If Me.TextBox1.Text.Trim.Length = 0 Then
            MsgBox("请输入增项金额！", MsgBoxStyle.OkOnly, "提示")
            Exit Sub
        End If

        If Not IsNumeric(Me.TextBox1.Text.Trim) Then
            MsgBox("增项金额必须是数字！", MsgBoxStyle.OkOnly, "提示")
            Exit Sub
        End If


        sqlcon = getconn()

        mytrans = sqlcon.BeginTransaction()
        sqlcmd.Transaction = mytrans

        sqlcmd.Connection = sqlcon
        Try



            sql = "INSERT INTO project_detail ( project_no,tz_money,tz_man,memo,tz_date) values " & _
            "(   '" & CType(Me.Owner, project).ExDataGridView1.Rows(CType(Me.Owner, project).ExDataGridView1.SelectedCells(0).RowIndex).Cells("project_no").Value & "'," & Me.TextBox1.Text.Trim & ", '" & rs_name & "'," & _
            "'" & Me.TextBox2.Text.Trim & "', getdate())"
            sqlcmd.CommandText = sql
            sqlcmd.ExecuteNonQuery()

            sql = "update a set a.project_money=isnull(b.tz_money,0) from project_master a left join  " & _
                    "	(select isnull(sum(tz_money),0) as tz_money,project_no from project_detail where project_no='" & CType(Me.Owner, project).ExDataGridView1.Rows(CType(Me.Owner, project).ExDataGridView1.SelectedCells(0).RowIndex).Cells("project_no").Value & "' group by project_no) b on a.project_no=b.project_no  " & _
                    "where a.project_no='" & CType(Me.Owner, project).ExDataGridView1.Rows(CType(Me.Owner, project).ExDataGridView1.SelectedCells(0).RowIndex).Cells("project_no").Value & "' "
            sqlcmd.CommandText = sql
            sqlcmd.ExecuteNonQuery()


            mytrans.Commit()
        Catch ex As Exception
            mytrans.Rollback()
            MsgBox(ex.Message, MsgBoxStyle.OkOnly, "提示")
            Exit Sub
        End Try
  


        Me.DialogResult = Windows.Forms.DialogResult.OK
    End Sub

    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button2.Click
        Me.DialogResult = Windows.Forms.DialogResult.Cancel
    End Sub
End Class