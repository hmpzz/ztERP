Imports System.Data.SqlClient
Public Class t_user_factory


    Public Sub Refresh_ExexDataGridView1(Optional ByVal s As String = "")
        Dim sqlcon As New SqlConnection
        Dim sqlcmd As New SqlCommand
        Dim sqlad As New SqlDataAdapter


        Dim dt As New DataTable
        Dim sql As String = ""
        Dim ss As String = ""



        sqlcon = getconn()
        If s = "" Then
            sql = "select * from sys_user a left join  " & _
                "	t_rs b on a.rs_id=b.id  " & _
                "where a.login_id<>'admin' and (a.stop_login=1  and b.stop=1) "

        End If


        sqlcmd.Connection = sqlcon
        sqlcmd.CommandText = sql
        sqlad.SelectCommand = sqlcmd
        dt.Rows.Clear()

        sqlad.Fill(dt)



        Me.ExDataGridView1.SetDataSource(dt)
        Me.ExDataGridView1.Columns.Clear()

        Me.ExDataGridView1.AddColumn("login_id", "登录ID")
        Me.ExDataGridView1.AddColumn("rs_name", "名字")


        If Me.ExDataGridView1.Rows.Count > 0 Then
            Me.ExDataGridView1.Rows(0).Cells("login_id").Selected = True

            cellclick(0)
        End If

    End Sub


    Private Sub cellclick(ByVal index As int32)
        Dim sqlcon As New SqlConnection
        Dim sqlcmd As New SqlCommand
        Dim sqlad As New SqlDataAdapter


        Dim dt As New DataTable
        Dim sql As String

        sqlcon = getconn()


        sql = "select a.*,isnull(b.id,0) as check1 " & _
                "from  t_factory a left join   " & _
                "	(select * from t_user_factory where login_id='" & Me.ExDataGridView1.Rows(Me.ExDataGridView1.SelectedCells.Item(0).RowIndex).Cells("login_id").Value & "' ) b on a.factory_code=b.factory_code where a.stop=1 "




        sqlcmd.Connection = sqlcon
        sqlcmd.CommandText = sql
        sqlad.SelectCommand = sqlcmd
        dt.Rows.Clear()
        sqlad.Fill(dt)

        Me.ExDataGridView2.SetDataSource(dt)
        Me.ExDataGridView2.Columns.Clear()


        Me.ExDataGridView2.AddColumn("check1", " ", 20, False, New DataGridViewCheckBoxColumn)
        Me.ExDataGridView2.AddColumn("factory_code", "工厂代码")
        Me.ExDataGridView2.AddColumn("factory_name", "工厂名称")


    End Sub

    Private Sub ToolStripButton_save_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripButton_save.Click
        Dim sqlcon As New SqlConnection
        Dim sqlcmd As New SqlCommand
        Dim mytrans As SqlTransaction

        Dim dt As New DataTable
        Dim sql As String = ""
        Dim i As int32

        If Me.ExDataGridView2.Rows.Count > 0 Then
            Me.ExDataGridView2.Rows(0).Cells(1).Selected = True
        End If

        sqlcon = getconn()
        sqlcmd.Connection = sqlcon

        mytrans = sqlcon.BeginTransaction

        sqlcmd.Transaction = mytrans


        Try

            sql = "delete from t_USER_factory where login_id='" & Me.ExDataGridView1.Rows(Me.ExDataGridView1.SelectedCells.Item(0).RowIndex).Cells("login_id").Value & "'"
            sqlcmd.CommandText = sql
            sqlcmd.ExecuteNonQuery()

            For i = 0 To Me.ExDataGridView2.Rows.Count - 1
                If Not IsDBNull(Me.ExDataGridView2.Rows(i).Cells("check1").Value) Then
                    If Me.ExDataGridView2.Rows(i).Cells("check1").Value <> 0 Then
                        sql = "insert into t_user_factory(login_id,factory_code) values ('" & Me.ExDataGridView1.Rows(Me.ExDataGridView1.SelectedCells.Item(0).RowIndex).Cells("login_id").Value & "','" & Me.ExDataGridView2.Rows(i).Cells("factory_code").Value & "')"
                        sqlcmd.CommandText = sql
                        sqlcmd.ExecuteNonQuery()
                    End If
                End If


            Next

            mytrans.Commit()
        Catch ex As Exception
            mytrans.Rollback()
            MsgBox(ex.Message, MsgBoxStyle.OkOnly, "错误")
            Exit Sub
        End Try
        MsgBox("保存成功！", MsgBoxStyle.OkOnly, "提示")
    End Sub

    Private Sub ToolStripButton_refresh_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripButton_refresh.Click
        Refresh_ExexDataGridView1()
    End Sub

    Private Sub ToolStripButton_exit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripButton_exit.Click
        Me.Close()
    End Sub

    Private Sub ExDataGridView1_CellClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles ExDataGridView1.CellClick
        If e.RowIndex < 0 Then
            Exit Sub
        Else
            cellclick(e.RowIndex)
        End If
    End Sub

   
End Class