Imports System.Data.SqlClient
Public Class department

 

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
            sql = "select * from t_bm a where 1=1 " & ss & _
            " order by bmno"
        Else
            sql = "select * from t_bm a where (bmmc like '%" & s & "%' or bmno like '%" & s & "%') " & ss & _
                "order by bmno"
        End If


        sqlcmd.Connection = sqlcon
        sqlcmd.CommandText = sql
        sqlad.SelectCommand = sqlcmd
        dt.Rows.Clear()

        sqlad.Fill(dt)

        

        Me.ExDataGridView1.SetDataSource(dt)
        Me.ExDataGridView1.Columns.Clear()

        Me.ExDataGridView1.AddColumn("bmno", "部门编号")
        Me.ExDataGridView1.AddColumn("bmmc", "部门名称")

    
        If Me.ExDataGridView1.Rows.Count > 0 Then
            Me.ExDataGridView1.Rows(0).Cells("bmno").Selected = True
            
            cellclick(0)
        End If

        Refresh_save()
    End Sub

    Private Sub ToolStripButton_refresh_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripButton_refresh.Click
        Refresh_ExexDataGridView1()
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
        Me.TextBox4.Enabled = True
        Me.TextBox5.Enabled = True
        Me.TextBox6.Enabled = True
        Me.TextBox7.Enabled = True

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
        Me.TextBox4.Enabled = False
        Me.TextBox5.Enabled = False
        Me.TextBox6.Enabled = False
        Me.TextBox7.Enabled = False

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

    Private Sub ToolStripButton_save_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripButton_save.Click
        Dim sqlcon As New SqlConnection
        Dim sqlcmd As New SqlCommand


        Dim dt As New DataTable
        Dim sql As String = ""

        sqlcon = getconn()

        Try
            If Me.TextBox2.Enabled = True Then '说明是新建
                sql = "INSERT INTO t_BM ( bmmc,bmno,fax,memo,tel,zg,stop,input_date,input_man,modify_date,modify_man) values ( '" & Me.TextBox3.Text.Trim & "', '" & Me.TextBox2.Text.Trim & "', '" & Me.TextBox6.Text.Trim & "', '" & Me.TextBox7.Text.Trim & "', '" & Me.TextBox5.Text.Trim & "', '" & Me.TextBox4.Text.Trim & "'," & IIf(Me.CheckBox1.Checked, 0, 1) & _
                                            ",getdate(),'" & rs_name & "',getdate(),'" & rs_name & "')"
                sqlcmd.Connection = sqlcon
                sqlcmd.CommandText = sql
                sqlcmd.ExecuteNonQuery()

                Refresh_save()
                Refresh_ExexDataGridView1()

            Else
                sql = "update t_bm set bmmc='" & Me.TextBox3.Text.Trim & "',fax='" & Me.TextBox6.Text.Trim & "',memo='" & Me.TextBox7.Text.Trim & "',tel='" & Me.TextBox5.Text.Trim & "',zg='" & Me.TextBox4.Text.Trim & "',stop=" & IIf(Me.CheckBox1.Checked, 0, 1) & ",modify_man='" & rs_name & "',modify_date=getdate() where bmno='" & Me.TextBox2.Text.Trim & "'"
                sqlcmd.Connection = sqlcon
                sqlcmd.CommandText = sql
                sqlcmd.ExecuteNonQuery()

                Refresh_save()
                '如果修改了停用则必须刷新
                'Me.ExDataGridView1.Rows(Me.ExDataGridView1.SelectedCells.Item(0).RowIndex).Cells("bmmc").Value = Me.TextBox3.Text.Trim
                Refresh_ExexDataGridView1()
                


            End If



        Catch ex As Exception
            Refresh_save()
            MsgBox(ex.Message, MsgBoxStyle.OkOnly, "错误")
            Exit Sub
        End Try
        MsgBox("保存成功！", MsgBoxStyle.OkOnly, "提示")
    End Sub

    Private Sub ToolStripButton_cancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripButton_cancel.Click


        If Me.TextBox2.Enabled = True And Me.ExDataGridView1.Rows.Count > 0 Then
            cellclick(0)
        End If
        Refresh_save()

    End Sub

    Private Sub ToolStripButton_add_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripButton_add.Click
        Refresh_add()
        Me.TextBox2.Text = ""
        Me.TextBox3.Text = ""
        Me.TextBox4.Text = ""
        Me.TextBox5.Text = ""
        Me.TextBox6.Text = ""
        Me.TextBox7.Text = ""

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


        'Me.ExDataGridView1.Rows(0).Cells(0).Value = 100

        If e.RowIndex < 0 Then
            Exit Sub
        Else
            cellclick(e.RowIndex)
        End If

    End Sub

    Private Sub ToolStripButton_exit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripButton_exit.Click
        Me.Close()
    End Sub

    '如果点击了单元格则用来刷新右边界面
    Private Sub cellclick(ByVal index As Int32)
        Dim sqlcon As New SqlConnection
        Dim sqlcmd As New SqlCommand
        Dim sqlad As New SqlDataAdapter


        Dim dt As New DataTable
        Dim sql As String

        sqlcon = getconn()


        sql = "select * from  t_BM where bmno='" & Me.ExDataGridView1.Rows(index).Cells("bmno").Value & "'"

        sqlcmd.Connection = sqlcon
        sqlcmd.CommandText = sql
        sqlad.SelectCommand = sqlcmd
        dt.Rows.Clear()
        sqlad.Fill(dt)



        Me.TextBox3.Text = dt.Rows(0)("bmmc")
        Me.TextBox2.Text = dt.Rows(0)("bmno")
        Me.TextBox6.Text = dt.Rows(0)("fax")
        Me.TextBox7.Text = dt.Rows(0)("memo")
        Me.TextBox5.Text = dt.Rows(0)("tel")
        Me.TextBox4.Text = dt.Rows(0)("zg")

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