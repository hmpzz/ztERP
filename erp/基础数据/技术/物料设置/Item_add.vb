Imports System.Data.SqlClient
Public Class Item_add
    Public state As String
   

    Private Sub ToolStripButton_add_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripButton_add.Click
        Refresh_add()
        Me.state = "new"
        Me.TextBox2.BackColor = Color.White
        Me.TextBox2.Text = ""
        Me.TextBox4.Text = ""
        Me.TextBox5.Text = ""
        Me.TextBox6.Text = 0
        Me.TextBox7.Text = 0

        Me.TextBox3.Text = ""
    End Sub


    Private Sub Refresh_add()

        Me.TextBox2.Enabled = True
        Me.TextBox4.Enabled = True
        Me.TextBox5.Enabled = True
        Me.TextBox6.Enabled = True
        Me.TextBox7.Enabled = True

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


    Private Sub ToolStripButton_exit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripButton_exit.Click
        Me.Close()
    End Sub

    Public Sub ToolStripButton_refresh_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripButton_refresh.Click
        Refresh_ExexDataGridView1()
    End Sub


    Public Sub Refresh_ExexDataGridView1(Optional ByVal s As String = "")
        Dim sqlcon As New SqlConnection
        Dim sqlcmd As New SqlCommand
        Dim sqlad As New SqlDataAdapter


        Dim dt As New DataTable
        Dim sql As String
        Dim ss As String = ""


        If Me.ToolStripButton_OK.Checked = True Then
            ss = " stop=1 "
        ElseIf Me.ToolStripButton_stop.Checked = True Then
            ss = " stop=0 "
        End If

        sqlcon = getconn()
        If s = "" Then
            sql = "select *  " & _
                    "from t_item where   " & ss & " order by item_name"

        Else
            sql = "select *  " & _
                    "from t_item where  " & ss & " and (item_name like '%" & s & "%' or item_code like '%" & s & "%') order by item_name"

        End If


        sqlcmd.Connection = sqlcon
        sqlcmd.CommandText = sql
        sqlad.SelectCommand = sqlcmd
        dt.Rows.Clear()

        sqlad.Fill(dt)



        Me.ExDataGridView1.SetDataSource(dt)
        Me.ExDataGridView1.Columns.Clear()

        Me.ExDataGridView1.AddColumn("item_code", "物料编码", , , , , , )
        Me.ExDataGridView1.AddColumn("item_name", "物料名称")



        If Me.ExDataGridView1.Rows.Count > 0 Then
            Me.ExDataGridView1.Rows(0).Cells("item_name").Selected = True
            cellclick(0)

        End If

        Refresh_save()
    End Sub

    Private Sub cellclick(ByVal index As Int32)
        Dim sqlcon As New SqlConnection
        Dim sqlcmd As New SqlCommand
        Dim sqlad As New SqlDataAdapter


        Dim dt As New DataTable
        Dim sql As String

        sqlcon = getconn()

        sql = "select *  " & _
              "from t_item where item_code='" & Me.ExDataGridView1.Rows(index).Cells("item_code").Value & "'"


        sqlcmd.Connection = sqlcon
        sqlcmd.CommandText = sql
        sqlad.SelectCommand = sqlcmd
        dt.Rows.Clear()
        sqlad.Fill(dt)

        Me.TextBox2.Text = dt.Rows(0)("item_code")
        Me.TextBox3.Text = dt.Rows(0)("item_name")
        Me.TextBox4.Text = dt.Rows(0)("item_standard")
        Me.TextBox5.Text = dt.Rows(0)("unit")
        Me.TextBox6.Text = dt.Rows(0)("max_amount")
        Me.TextBox7.Text = dt.Rows(0)("min_amount")

        If dt.Rows(0)("stop") = 0 Then
            Me.CheckBox1.Checked = True
        ElseIf dt.Rows(0)("stop") = 1 Then
            Me.CheckBox1.Checked = False
        End If
    End Sub

    Private Sub ExDataGridView1_CellClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles ExDataGridView1.CellClick
        If e.RowIndex < 0 Then
            Exit Sub
        Else
            cellclick(e.RowIndex)
        End If
    End Sub

    Private Sub Refresh_save()
        Me.TextBox2.BackColor = Color.Gainsboro
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

    Private Sub ToolStripButton_cancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripButton_cancel.Click

        If Me.TextBox3.Text.Trim.Length = 0 And Me.ExDataGridView1.Rows.Count > 0 Then
            Me.ExDataGridView1.Rows(0).Cells("item_name").Selected = True
            cellclick(0)
        End If
        'Me.TextBox2.Text = Me.ExDataGridView1.Rows(Me.ExDataGridView1.SelectedCells.Item(0).RowIndex).Cells("item_name").Value
        Refresh_save()
    End Sub

    Private Sub ToolStripButton_mod_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripButton_mod.Click
        If Me.ExDataGridView1.SelectedCells Is Nothing Then Exit Sub
        Me.state = "mod"

        Try
            cellclick(Me.ExDataGridView1.SelectedCells.Item(0).RowIndex)

            Refresh_add()
            Me.TextBox2.Enabled = False
            Me.TextBox2.BackColor = Color.Gainsboro

        Catch ex As Exception
            Refresh_save()
            MsgBox(ex.Message, MsgBoxStyle.OkOnly, "错误")
            Exit Sub
        End Try
    End Sub

    Private Sub ToolStripButton_save_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripButton_save.Click
        Dim sqlcon As New SqlConnection
        Dim sqlcmd As New SqlCommand



        Dim dt As New DataTable
        Dim sql As String = ""
        Dim s As String = ""

        If Me.TextBox4.Text.Trim.Length = 0 Then
            MsgBox("请填写规格！", MsgBoxStyle.OkOnly, "提示")
            Exit Sub
        End If

        If Me.TextBox5.Text.Trim.Length = 0 Then
            MsgBox("请填写单位！", MsgBoxStyle.OkOnly, "提示")
            Exit Sub
        End If

        If Me.TextBox6.Text.Trim.Length = 0 Then
            MsgBox("请填写上限！", MsgBoxStyle.OkOnly, "提示")
            Exit Sub
        End If

        

        If Me.TextBox7.Text.Trim.Length = 0 Then
            MsgBox("请填写下限！", MsgBoxStyle.OkOnly, "提示")
            Exit Sub
        End If

        If Not IsNumeric(Me.TextBox6.Text.Trim) Then
            MsgBox("上限必须为数字！", MsgBoxStyle.OkOnly, "提示")
            Exit Sub
        End If

        If Not IsNumeric(Me.TextBox7.Text.Trim) Then
            MsgBox("下限必须为数字！", MsgBoxStyle.OkOnly, "提示")
            Exit Sub
        End If

        If Val(Me.TextBox6.Text.Trim) < Val(Me.TextBox7.Text.Trim) Then
            MsgBox("上限必须大于等于下限！", MsgBoxStyle.OkOnly, "提示")
            Exit Sub
        End If


        sqlcon = getconn()
        sqlcmd.Connection = sqlcon


        Try
            If Me.state = "new" Then '说明是新建


                'sql = "select isnull(max(right(item_code,6)),0) from t_item"
                'sqlcmd.CommandText = sql
                's = sqlcmd.ExecuteScalar + 1

                'sql = "select 'WL'+REPLICATE('0',6-len('" & s & "')) +'" & s & "'"
                'sqlcmd.CommandText = sql
                's = sqlcmd.ExecuteScalar

                s = Me.TextBox2.Text.Trim

                sql = "INSERT INTO t_item ( item_code,item_name,stop,item_standard,unit,max_amount,min_amount) values " & _
                                    "( '" & s & "', '" & Me.TextBox3.Text.Trim & "', '" & IIf(Me.CheckBox1.Checked, 0, 1) & "', '" & Me.TextBox4.Text.Trim & "','" & Me.TextBox5.Text.Trim & "','" & Me.TextBox6.Text.Trim & "','" & Me.TextBox7.Text.Trim & "')"


                sqlcmd.CommandText = sql
                sqlcmd.ExecuteNonQuery()

                Refresh_save()
                Refresh_ExexDataGridView1()

            ElseIf Me.state = "mod" Then

                sql = "update t_item set item_name='" & Me.TextBox3.Text.Trim & "',stop='" & IIf(Me.CheckBox1.Checked, 0, 1) & "',item_standard='" & Me.TextBox4.Text.Trim & "',unit='" & Me.TextBox5.Text.Trim & "',max_amount=" & Me.TextBox6.Text.Trim & ",min_amount=" & Me.TextBox7.Text.Trim & _
                    " where item_code='" & Me.TextBox2.Text.Trim & "'"

                sqlcmd.CommandText = sql
                sqlcmd.ExecuteNonQuery()


                Refresh_save()
                '如果修改了停用则必须刷新
                'Me.ExDataGridView1.Rows(Me.ExDataGridView1.SelectedCells.Item(0).RowIndex).Cells("item_name").Value = Me.TextBox2.Text.Trim

                Refresh_ExexDataGridView1()


            End If



        Catch ex As Exception
            Refresh_save()
            MsgBox(ex.Message, MsgBoxStyle.OkOnly, "错误")
            Exit Sub
        End Try

        MsgBox("保存成功！", MsgBoxStyle.OkOnly, "提示")
    End Sub

    Private Sub TextBox1_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles TextBox1.KeyUp
        If e.KeyCode = 13 Then
            Refresh_ExexDataGridView1(Me.TextBox1.Text.Trim)
            Me.TextBox1.Text = ""
        End If
    End Sub

    Private Sub TextBox1_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TextBox1.TextChanged

    End Sub

    Private Sub ExDataGridView1_CellContentClick(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles ExDataGridView1.CellContentClick

    End Sub
End Class