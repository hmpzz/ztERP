Imports System.Data.SqlClient
Public Class employee

    Private Sub TextBox1_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles TextBox1.KeyUp
        If e.KeyCode = 13 Then
            Refresh_ExexDataGridView1(" and (a.rs_no like '%" & Me.TextBox1.Text.Trim & "%' or a.rs_name like '%" & Me.TextBox1.Text.Trim & "%') ")
            Me.TextBox1.Text = ""
        End If
    End Sub



    '用于刷新部门编码与部门名称
    Public Sub Refresh_ExexDataGridView1(Optional ByVal s As String = "")
        Dim sqlcon As New SqlConnection
        Dim sqlcmd As New SqlCommand
        Dim sqlad As New SqlDataAdapter


        Dim dt As New DataTable
        Dim sql As String
        Dim ss As String = ""


        If Me.ToolStripButton_OK.Checked = True Then
            ss = " and a.stop=1 "
        ElseIf Me.ToolStripButton_stop.Checked = True Then
            ss = " and a.stop=0 "
        End If

        sqlcon = getconn()
        If s = "" Then
            sql = "select a.id as rsid,*  " & _
                    "from t_rs a left join  " & _
                    "	t_bm b on a.bm_id=b.id	left join " & _
                    "	t_xl c on a.xl_id=c.id " & _
                    "where 1=1 " & ss & _
                    "order by a.rs_no "

        Else
            sql = "select a.id as rsid,* " & _
                    "from t_rs a left join  " & _
                    "	t_bm b on a.bm_id=b.id	left join " & _
                    "	t_xl c on a.xl_id=c.id " & _
                    "where (a.rs_no like '%" & Me.TextBox1.Text.Trim & "%' or a.rs_name like '%" & Me.TextBox1.Text.Trim & "%' )" & ss & s & _
                    "order by a.rs_no "
        End If


        sqlcmd.Connection = sqlcon
        sqlcmd.CommandText = sql
        sqlad.SelectCommand = sqlcmd
        dt.Rows.Clear()

        sqlad.Fill(dt)



        Me.ExDataGridView1.SetDataSource(dt)
        Me.ExDataGridView1.Columns.Clear()

        Me.ExDataGridView1.AddColumn("rs_no", "员工编号")
        Me.ExDataGridView1.AddColumn("rs_name", "员工姓名")
        Me.ExDataGridView1.AddColumn("id", "id", , , , , , False)


        If Me.ExDataGridView1.Rows.Count > 0 Then
            Me.ExDataGridView1.Rows(0).Cells("rs_no").Selected = True
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

        sql = "select * from t_rs a left join t_bm b on a.bm_id=b.id	left join t_xl c on a.xl_id=c.id " & _
              "where a.id='" & Me.ExDataGridView1.Rows(index).Cells("id").Value & "'"


        sqlcmd.Connection = sqlcon
        sqlcmd.CommandText = sql
        sqlad.SelectCommand = sqlcmd
        dt.Rows.Clear()
        sqlad.Fill(dt)



        Me.TextBox3.Text = dt.Rows(0)("rs_name")
        Me.TextBox2.Text = dt.Rows(0)("rs_no")

        Me.TextBox8.Text = dt.Rows(0)("Tel")
        Me.TextBox10.Text = dt.Rows(0)("Home_Address")



        Me.TextBox5.Text = dt.Rows(0)("memo")
        Me.TextBox4.Text = dt.Rows(0)("card")


        If dt.Rows(0)("stop") = 0 Then
            Me.CheckBox1.Checked = True
        ElseIf dt.Rows(0)("stop") = 1 Then
            Me.CheckBox1.Checked = False
        End If





        If dt.Rows(0)("hf") = 0 Then
            Me.CheckBox2.Checked = True
        ElseIf dt.Rows(0)("hf") = 1 Then
            Me.CheckBox2.Checked = False
        End If

        Me.ComboBox1.Text = dt.Rows(0)("xb")
        Me.ComboBox3.Text = dt.Rows(0)("bm_id")
        Me.ComboBox5.Text = dt.Rows(0)("xl_id")



    End Sub

    Private Sub employee_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim sqlcon As New SqlConnection
        Dim sqlcmd As New SqlCommand
        Dim sqlad As New SqlDataAdapter


        Dim dt As New DataTable
        Dim sql As String
        Dim i As Int32

        sqlcon = getconn()

        Me.ComboBox1.DropDownStyle = ComboBoxStyle.DropDownList
        Me.ComboBox2.DropDownStyle = ComboBoxStyle.DropDownList
        Me.ComboBox4.DropDownStyle = ComboBoxStyle.DropDownList

        Me.ComboBox1.Items.Clear()
        Me.ComboBox1.Items.Add("男")
        Me.ComboBox1.Items.Add("女")



        sql = "select *  from t_bm"

        sqlcmd.Connection = sqlcon
        sqlcmd.CommandText = sql
        sqlad.SelectCommand = sqlcmd
        dt.Rows.Clear()
        sqlad.Fill(dt)

        Me.ComboBox2.Items.Clear()
        Me.ComboBox3.Items.Clear()

        For i = 0 To dt.Rows.Count - 1
            Me.ComboBox2.Items.Add(dt.Rows(i)("bmmc").ToString)
            Me.ComboBox3.Items.Add(dt.Rows(i)("id").ToString)
        Next


        sql = "select *  from t_xl"

        sqlcmd.Connection = sqlcon
        sqlcmd.CommandText = sql
        sqlad.SelectCommand = sqlcmd
        dt.Rows.Clear()
        sqlad.Fill(dt)

        Me.ComboBox4.Items.Clear()
        Me.ComboBox5.Items.Clear()

        For i = 0 To dt.Rows.Count - 1
            Me.ComboBox4.Items.Add(dt.Rows(i)("xl_name").ToString)
            Me.ComboBox5.Items.Add(dt.Rows(i)("id").ToString)
        Next


    End Sub

    Private Sub ComboBox2_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ComboBox2.SelectedIndexChanged
        Me.ComboBox3.SelectedIndex = Me.ComboBox2.SelectedIndex
    End Sub

    Private Sub ComboBox4_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ComboBox4.SelectedIndexChanged
        Me.ComboBox5.SelectedIndex = Me.ComboBox4.SelectedIndex
    End Sub


    Private Sub ComboBox3_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ComboBox3.SelectedIndexChanged
        Me.ComboBox2.SelectedIndex = Me.ComboBox3.SelectedIndex
    End Sub

    Private Sub ComboBox5_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ComboBox5.SelectedIndexChanged
        Me.ComboBox4.SelectedIndex = Me.ComboBox5.SelectedIndex
    End Sub

    Private Sub ExDataGridView1_CellClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles ExDataGridView1.CellClick
        If e.RowIndex < 0 Then
            Exit Sub
        Else
            cellclick(e.RowIndex)
        End If

    End Sub

    Private Sub Refresh_add()
        Me.TextBox1.Enabled = True
        Me.TextBox2.Enabled = True
        Me.TextBox3.Enabled = True
        Me.TextBox4.Enabled = True
        Me.TextBox5.Enabled = True
        Me.TextBox8.Enabled = True
        Me.TextBox10.Enabled = True

        Me.ComboBox1.Enabled = True
        Me.ComboBox2.Enabled = True
        Me.ComboBox3.Enabled = True
        Me.ComboBox4.Enabled = True
        Me.ComboBox5.Enabled = True

        Me.CheckBox1.Enabled = True
        Me.CheckBox2.Enabled = True


        Me.ToolStripButton_save.Enabled = True
        Me.ToolStripButton_cancel.Enabled = True

        Me.ToolStripButton_add.Enabled = False
        Me.ToolStripButton_mod.Enabled = False
        Me.ToolStripButton_refresh.Enabled = False
        Me.ToolStripButton_exit.Enabled = False
        Me.ToolStripButton_question.Enabled = False

        Me.ToolStripButton_stop.Enabled = False
        Me.ToolStripButton_OK.Enabled = False


        Me.ExDataGridView1.Enabled = False
    End Sub

    Private Sub Refresh_save()
        Me.TextBox1.Enabled = False
        Me.TextBox2.Enabled = False
        Me.TextBox3.Enabled = False
        Me.TextBox4.Enabled = False
        Me.TextBox5.Enabled = False
        Me.TextBox8.Enabled = False
        Me.TextBox10.Enabled = False

        Me.ComboBox1.Enabled = False
        Me.ComboBox2.Enabled = False
        Me.ComboBox3.Enabled = False
        Me.ComboBox4.Enabled = False
        Me.ComboBox5.Enabled = False


        Me.CheckBox1.Enabled = False
        Me.CheckBox2.Enabled = False



        Me.ToolStripButton_save.Enabled = False
        Me.ToolStripButton_cancel.Enabled = False

        Me.ToolStripButton_add.Enabled = True
        Me.ToolStripButton_mod.Enabled = True
        Me.ToolStripButton_refresh.Enabled = True
        Me.ToolStripButton_exit.Enabled = True

        Me.ToolStripButton_stop.Enabled = True
        Me.ToolStripButton_OK.Enabled = True
        Me.ToolStripButton_question.Enabled = True


        Me.ExDataGridView1.Enabled = True

    End Sub

    Private Sub ToolStripButton_add_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripButton_add.Click
        Refresh_add()

        Me.TextBox2.Text = ""
        Me.TextBox3.Text = ""
        Me.TextBox4.Text = ""
        Me.TextBox5.Text = ""
        Me.TextBox8.Text = ""
        Me.TextBox10.Text = ""

        Me.ComboBox1.SelectedIndex = 0

        Me.ComboBox2.SelectedIndex = 0
        Me.ComboBox4.SelectedIndex = 0

        Me.CheckBox1.Checked = False
        Me.CheckBox2.Checked = False
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

    Private Sub ToolStripButton_save_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripButton_save.Click
        Dim sqlcon As New SqlConnection
        Dim sqlcmd As New SqlCommand



        Dim dt As New DataTable
        Dim sql As String = ""
        Dim mytrans As SqlTransaction


        If Me.ComboBox2.SelectedIndex < 0 Then
            MsgBox("请输入部门！", MsgBoxStyle.OkOnly, "提示")
            Exit Sub
        End If

        sqlcon = getconn()

        Try
            If Me.TextBox2.Enabled = True Then '说明是新建
                mytrans = sqlcon.BeginTransaction
                Try
                    sqlcmd.Transaction = mytrans

                    'sql = "select isnull( (max(rs_no)),0) from t_rs "

                    'sqlcmd.Connection = sqlcon
                    'sqlcmd.CommandText = sql
                    'Me.TextBox2.Text = sqlcmd.ExecuteScalar + 1

                    sql = "select count(*) from t_rs where rs_no='" & Me.TextBox2.Text.Trim & "'"
                    sqlcmd.Connection = sqlcon
                    sqlcmd.CommandText = sql
                    If sqlcmd.ExecuteScalar > 0 Then
                        Throw New System.Exception("员工编号有重,请重新输入！")
                    End If




                    sql = "INSERT INTO t_rs ( BM_id,Card,Hf,input_date,input_man,memo,modify_date,modify_man,rs_name,rs_no,stop,Xb,xl_id,Tel,Home_Address" & _
                                         ") values ( '" & Me.ComboBox3.Text & "','" & Me.TextBox4.Text.Trim & "','" & IIf(Me.CheckBox2.Checked, 0, 1) & "',getdate(),'" & rs_name & "'," & _
                                        "'" & Me.TextBox5.Text.Trim & "',getdate(), '" & rs_name & "','" & Me.TextBox3.Text.Trim & "','" & Me.TextBox2.Text.Trim & "','" & IIf(Me.CheckBox1.Checked, 0, 1) & "'," & _
                                        "'" & Me.ComboBox1.Text & "','" & Me.ComboBox5.Text & "','" & Me.TextBox8.Text.Trim & "'," & _
                                        "'" & Me.TextBox10.Text.Trim & "')"

                    sqlcmd.Connection = sqlcon
                    sqlcmd.CommandText = sql
                    sqlcmd.ExecuteNonQuery()

                    mytrans.Commit()

                Catch ex As Exception
                    mytrans.Rollback()
                    MsgBox(ex.Message)
                    Exit Sub

                End Try

                Refresh_save()
                Refresh_ExexDataGridView1()

            Else
                mytrans = sqlcon.BeginTransaction
                Try
                    sqlcmd.Transaction = mytrans
                    sql = "update t_rs set bm_id='" & Me.ComboBox3.Text & "',Card='" & Me.TextBox4.Text.Trim & "',Tel='" & Me.TextBox8.Text.Trim & "'," & _
                        "Home_Address='" & Me.TextBox10.Text.Trim & "',Hf='" & IIf(Me.CheckBox2.Checked, 0, 1) & "'," & _
                        "memo='" & Me.TextBox5.Text.Trim & "',modify_date=getdate(),modify_man='" & rs_name & "',rs_name='" & Me.TextBox3.Text.Trim & "',stop='" & IIf(Me.CheckBox1.Checked, 0, 1) & "',Xb='" & Me.ComboBox1.Text & "'," & _
                        "xl_id='" & Me.ComboBox5.Text & "' where rs_no='" & Me.TextBox2.Text.Trim & "'"
                    sqlcmd.Connection = sqlcon
                    sqlcmd.CommandText = sql
                    sqlcmd.ExecuteNonQuery()

                    If Me.CheckBox1.Checked Then
                        '如果停用了用户则连登陆帐户也停用
                        sql = "update sys_user set stop_login=0 where rs_id=" & Me.ExDataGridView1.Rows(Me.ExDataGridView1.SelectedCells(0).RowIndex).Cells("id").Value
                        sqlcmd.Connection = sqlcon
                        sqlcmd.CommandText = sql
                        sqlcmd.ExecuteNonQuery()

                    End If

                    mytrans.Commit()

                Catch ex As Exception
                    mytrans.Rollback()
                    MsgBox(ex.Message)
                    Exit Sub
                End Try

                Refresh_save()
                '如果修改了停用则必须刷新
                'Me.ExDataGridView1.Rows(Me.ExDataGridView1.SelectedCells.Item(0).RowIndex).Cells("rs_name").Value = Me.TextBox3.Text.Trim
                Refresh_ExexDataGridView1()
            End If



        Catch ex As Exception
            Refresh_save()
            MsgBox(ex.Message, MsgBoxStyle.OkOnly, "错误")
            Exit Sub
        End Try

        MsgBox("保存成功！", MsgBoxStyle.OkOnly, "提示")
    End Sub

    Private Sub ExDataGridView1_CellContentClick(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles ExDataGridView1.CellContentClick

    End Sub

    Private Sub ToolStripButton_cancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripButton_cancel.Click

        If Me.TextBox3.Enabled = True And Me.ExDataGridView1.Rows.Count > 0 Then
            Me.ExDataGridView1.Rows(0).Cells("rs_no").Selected = True
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





    Private Sub ToolStripButton_question_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripButton_question.Click
        Dim employee_cha As New employee_cha
        employee_cha.Owner = Me
        Me.Tag = ""
        employee_cha.ShowDialog()

        If employee_cha.DialogResult = Windows.Forms.DialogResult.OK Then

            Me.Refresh_ExexDataGridView1(Me.Tag)
            Me.Tag = ""

        End If
    End Sub


    Private Sub TextBox1_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TextBox1.TextChanged

    End Sub

    Private Sub TextBox3_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TextBox3.TextChanged

    End Sub
End Class