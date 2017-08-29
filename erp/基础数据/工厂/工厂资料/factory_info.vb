Imports System.Data.SqlClient
Public Class factory_info
    '用于刷新部门编码与部门名称
    Public Sub Refresh_ExexDataGridView1(Optional ByVal s As String = "")
        Dim sqlcon As New SqlConnection
        Dim sqlcmd As New SqlCommand
        Dim sqlad As New SqlDataAdapter


        Dim dt As New DataTable
        Dim sql As String
        Dim ss As String = ""

        If Me.ToolStripButton_OK.Checked = True Then
            ss = " and a.stop=1"
        ElseIf Me.ToolStripButton_stop.Checked = True Then
            ss = " and a.stop=0"
        End If

        sqlcon = getconn()
        If s = "" Then
            sql = "select * from t_factory a where 1=1 " & ss & _
            " order by factory_name"
        Else
            sql = "select * from t_factory a where (factory_name like '%" & s & "%' or factory_code like '%" & s & "%') " & ss & _
                "order by factory_name"
        End If


        sqlcmd.Connection = sqlcon
        sqlcmd.CommandText = sql
        sqlad.SelectCommand = sqlcmd
        dt.Rows.Clear()

        sqlad.Fill(dt)



        Me.ExDataGridView1.SetDataSource(dt)
        'Me.ExDataGridView1.Columns.Clear()

        Me.ExDataGridView1.AddColumn("factory_code", "工厂编号")
        Me.ExDataGridView1.AddColumn("factory_name", "工厂名称")


        If Me.ExDataGridView1.Rows.Count > 0 Then
            Me.ExDataGridView1.Rows(0).Cells("factory_code").Selected = True

            cellclick(0)
        End If

        Refresh_save()
    End Sub

    Private Sub ToolStripButton_refresh_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripButton_refresh.Click
        Refresh_ExexDataGridView1()
    End Sub

    Private Sub ToolStripButton_exit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripButton_exit.Click
        Me.Close()
    End Sub

    Private Sub TextBox1_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles TextBox1.KeyUp
        If e.KeyCode = 13 Then
            Refresh_ExexDataGridView1(Me.TextBox1.Text.Trim)
            Me.TextBox1.Text = ""
        End If
    End Sub

    Private Sub Refresh_add()
        Me.TextBox2.Enabled = True
        Me.TextBox3.Enabled = True
      

        Me.CheckBox1.Enabled = True

        Me.ToolStripButton_save.Enabled = True
        Me.ToolStripButton_cancel.Enabled = True

        Me.ToolStripButton_add.Enabled = False
        Me.ToolStripButton_mod.Enabled = False
        Me.ToolStripButton_refresh.Enabled = False
        Me.ToolStripButton_exit.Enabled = False

        Me.ToolStripButton_stop.Enabled = False
        Me.ToolStripButton_OK.Enabled = False

        Me.ExDataGridView1.Enabled = False
    End Sub



    Private Sub Refresh_save()
        Me.TextBox2.Enabled = False
        Me.TextBox3.Enabled = False
       


        Me.CheckBox1.Enabled = False

        Me.ToolStripButton_save.Enabled = False
        Me.ToolStripButton_cancel.Enabled = False

        Me.ToolStripButton_add.Enabled = True
        Me.ToolStripButton_mod.Enabled = True
        Me.ToolStripButton_refresh.Enabled = True
        Me.ToolStripButton_exit.Enabled = True

        Me.ToolStripButton_stop.Enabled = True
        Me.ToolStripButton_OK.Enabled = True


        Me.ExDataGridView1.Enabled = True

    End Sub

    Private Sub ToolStripButton_cancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripButton_cancel.Click
        If Me.TextBox2.Enabled = True And Me.ExDataGridView1.Rows.Count > 0 Then
            cellclick(0)
        End If
        Refresh_save()
    End Sub

    Private Sub ToolStripButton_save_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripButton_save.Click
        Dim sqlcon As New SqlConnection
        Dim sqlcmd As New SqlCommand
        Dim mytrans As SqlTransaction

        Dim dt As New DataTable
        Dim sql As String = ""
        Dim accountterm As String = ""

        sqlcon = getconn()
        sqlcmd.Connection = sqlcon

        mytrans = sqlcon.BeginTransaction()
        sqlcmd.Transaction = mytrans

        If Me.TextBox2.Text.Trim.Length = 0 Then
            MsgBox("请填写工厂编码！", MsgBoxStyle.OkOnly, "提示")
            Exit Sub
        End If


        If Me.TextBox3.Text.Trim.Length = 0 Then
            MsgBox("请填写仓库名称！", MsgBoxStyle.OkOnly, "提示")
            Exit Sub
        End If





        Try
            If Me.TextBox2.Enabled = True Then '说明是新建

               

                sql = "INSERT INTO t_factory ( factory_code, factory_name,input_date,input_man,stop) values " & _
                "(  '" & Me.TextBox2.Text.Trim & "', '" & Me.TextBox3.Text.Trim & "',getdate(), '" & rs_name & "', " & IIf(Me.CheckBox1.Checked, 0, 1) & ")"

                sqlcmd.CommandText = sql
                sqlcmd.ExecuteNonQuery()



                sql = "INSERT INTO t_dj_num ( DJ_name,format,FullName,num,TT) values ( '工程单', '0000', '" & Me.TextBox2.Text.Trim & "年0001', '0', '" & Me.TextBox2.Text.Trim & "')"
                sqlcmd.CommandText = sql
                sqlcmd.ExecuteNonQuery()

             



            Else
                sql = "update t_depot set Depot_name='" & Me.TextBox3.Text.Trim & "',mod_date=getdate(),mod_man='" & rs_name & "',stop=" & IIf(Me.CheckBox1.Checked, 0, 1) & "  where Depot_code='" & Me.TextBox2.Text.Trim & "'"
                sqlcmd.CommandText = sql
                sqlcmd.ExecuteNonQuery()


            End If

            mytrans.Commit()

        Catch ex As Exception
            Refresh_save()
            mytrans.Rollback()
            MsgBox(ex.Message, MsgBoxStyle.OkOnly, "错误")
            Exit Sub
        End Try
        Refresh_save()
        Refresh_ExexDataGridView1()
        MsgBox("保存成功！", MsgBoxStyle.OkOnly, "提示")
    End Sub

    Private Sub ToolStripButton_add_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripButton_add.Click
        Refresh_add()
        Me.TextBox2.Text = ""
        Me.TextBox3.Text = ""



        Me.CheckBox1.Checked = False
    End Sub

    Private Sub ToolStripButton_mod_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripButton_mod.Click
        If Me.ExDataGridView1.SelectedCells Is Nothing Then Exit Sub

        Try
            cellclick(Me.ExDataGridView1.SelectedCells.Item(0).RowIndex)


            Refresh_add()
            Me.TextBox2.Enabled = False



        Catch ex As Exception
            Refresh_save()
            MsgBox(ex.Message, MsgBoxStyle.OkOnly, "错误")
        End Try
    End Sub

    Private Sub ExDataGridView1_CellClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles ExDataGridView1.CellClick
        If e.RowIndex < 0 Then
            Exit Sub
        Else
            cellclick(e.RowIndex)
        End If
    End Sub

    '如果点击了单元格则用来刷新右边界面
    Private Sub cellclick(ByVal index As Int32)
        Dim sqlcon As New SqlConnection
        Dim sqlcmd As New SqlCommand
        Dim sqlad As New SqlDataAdapter


        Dim dt As New DataTable
        Dim sql As String

        sqlcon = getconn()


        sql = "select * from  t_factory where factory_code='" & Me.ExDataGridView1.Rows(index).Cells("factory_code").Value & "'"

        sqlcmd.Connection = sqlcon
        sqlcmd.CommandText = sql
        sqlad.SelectCommand = sqlcmd
        dt.Rows.Clear()
        sqlad.Fill(dt)


        Me.TextBox2.Text = dt.Rows(0)("factory_code")
        Me.TextBox3.Text = dt.Rows(0)("factory_name")


        If dt.Rows(0)("stop") = 0 Then
            Me.CheckBox1.Checked = True
        ElseIf dt.Rows(0)("stop") = 1 Then
            Me.CheckBox1.Checked = False
        End If
    End Sub

    Private Sub ToolStripButton_OK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripButton_OK.Click
        Me.ToolStripButton_OK.Checked = True
        Me.ToolStripButton_stop.Checked = False
        Refresh_ExexDataGridView1()
    End Sub

    Private Sub ToolStripButton_stop_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripButton_stop.Click
        Me.ToolStripButton_OK.Checked = False
        Me.ToolStripButton_stop.Checked = True
        Refresh_ExexDataGridView1()
    End Sub
End Class