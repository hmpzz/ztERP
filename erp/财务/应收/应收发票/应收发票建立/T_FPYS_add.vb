Imports System.Data.SqlClient

Public Class T_FPYS_add
    Friend state As String
    Friend billno As String
    Friend dt_p As New DataTable


    Private Sub ToolStripButton_exit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripButton_exit.Click
        Me.DialogResult = Windows.Forms.DialogResult.Cancel
    End Sub

    Private Sub 添加记录ToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles 添加记录ToolStripMenuItem.Click
        Dim T_FPYS_add_detail As New T_FPYS_add_detail

        Dim sqlcon As New SqlConnection
        Dim sqlcmd As New SqlCommand
        Dim sqlad As New SqlDataAdapter

        Dim dt As New DataTable
        Dim sql As String
        Dim i As int32
        Dim s As String = ""

       

        T_FPYS_add_detail.Owner = Me
        T_FPYS_add_detail.ShowDialog()

        If T_FPYS_add_detail.DialogResult = Windows.Forms.DialogResult.Cancel Then
            T_FPYS_add_detail.Dispose()
            Exit Sub
        End If


        If Me.Tag = "" Then
            Exit Sub
        End If

       

        sqlcon = getconn()
        sqlcmd.Connection = sqlcon

        sql = "SELECT a.*,case state when 0 then '新建' when 1 then '审核' when 2 then '结案' end as state1,project_money-FP_money as WKP_money,project_money-FP_money as BC_money " & _
                     "FROM project_master a " & _
                     "where project_no in (" & Me.Tag & ") order by a.id "
        sqlcmd.CommandText = sql

        sqlad.SelectCommand = sqlcmd
        sqlad.Fill(dt)

        If Me.ExDataGridView1.Columns.Count = 0 Then
            dt_p = dt

            Me.ExDataGridView1.SetDataSource(dt_p)
            Me.ExDataGridView1.Columns.Clear()

            Me.ExDataGridView1.AddColumn("project_no", "工程号")
            Me.ExDataGridView1.AddColumn("project_name", "工程名称")
            Me.ExDataGridView1.AddColumn("project_money", "工程金额", , , , , "#0")
            Me.ExDataGridView1.AddColumn("FP_money", "已开票金额", , , , , "#0")
            Me.ExDataGridView1.AddColumn("WKP_money", "未开票金额", , , , , "#0")
            Me.ExDataGridView1.AddColumn("BC_money", "本次开票金额", , False, , , "#0")


            Me.ExDataGridView1.AddColumn("id", "id", , , , , , False)
        Else
            For i = 0 To dt.Rows.Count - 1

                Dim DR As DataRow = Me.dt_p.NewRow

                DR("project_no") = dt.Rows(i)("project_no")

                DR("project_name") = dt.Rows(i)("project_name")
                DR("project_money") = dt.Rows(i)("project_money")
                DR("FP_money") = dt.Rows(i)("FP_money")
                DR("WKP_money") = dt.Rows(i)("WKP_money")
                DR("BC_money") = dt.Rows(i)("BC_money")



                DR("id") = 0

                dt_p.Rows.Add(DR)


            Next
        End If



    End Sub

    Private Sub 删除记录ToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles 删除记录ToolStripMenuItem.Click
     

        If MsgBox("确认要删除此记录吗？", MsgBoxStyle.YesNo, "提示") = MsgBoxResult.No Then
            Exit Sub
        End If




        Me.ExDataGridView1.Rows.RemoveAt(Me.ExDataGridView1.SelectedCells(0).RowIndex)
        Me.dt_p.AcceptChanges()
       
    End Sub

    Private Sub ToolStripButton_save_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripButton_save.Click


        Dim sqlcon As New SqlConnection
        Dim sqlcmd As New SqlCommand
        Dim sqlad As New SqlDataAdapter
        Dim mytrans As SqlTransaction

        Dim dt As New DataTable
        Dim sql As String
        Dim i As Int32

        Dim s As String = ""


        If Me.RadioButton1.Checked Then

            If Me.TextBox2.Text.Trim.Length = 0 Then
                MsgBox("请填写发票号！", MsgBoxStyle.OkOnly, "提示")
                Exit Sub
            End If
        End If

        Me.TextBox3.Focus()




        sqlcon = getconn()
        sqlcmd.Connection = sqlcon
        mytrans = sqlcon.BeginTransaction

        sqlcmd.Transaction = mytrans



        Try
            If state = "new" Then


                sql = "update t_dj_num set num=num+1 where  tt='FPS'"
                sqlcmd.CommandText = sql
                sqlcmd.ExecuteNonQuery()

                sql = "select * from t_dj_num where tt='FPS'"
                sqlcmd.CommandText = sql
                sqlad.SelectCommand = sqlcmd
                dt.Rows.Clear()
                sqlad.Fill(dt)

                billno = dt.Rows(0)("TT") & "-" & GetAccountterm() & "-" & Format(dt.Rows(0)("num"), dt.Rows(0)("format"))




                sql = "INSERT INTO [T_FPYS_master] " & _
                            "           ([billno] " & _
                            "           ,[FP_no] " & _
                            "           ,[memo] " & _
                            "           ,[input_man] " & _
                            "           ,[input_date] " & _
                            "           ,[KP_state] " & _
                            "           ,[KP_type] " & _
                            "           ,[customer_no] " & _
                            " ,[HW_name1] " & _
                            " ,[HW_name2] " & _
                            " ,[HW_name3] " & _
                            " ,[HW_name4] " & _
                            " ,[HW_name5] " & _
                            " ,[HW_name6] " & _
                            " ,[HW_UNIT1] " & _
                            " ,[HW_UNIT2] " & _
                            " ,[HW_UNIT3] " & _
                            " ,[HW_UNIT4] " & _
                            " ,[HW_UNIT5] " & _
                            " ,[HW_UNIT6] " & _
                            " ,[HW_amount1] " & _
                            " ,[HW_amount2] " & _
                            " ,[HW_amount3] " & _
                            " ,[HW_amount4] " & _
                            " ,[HW_amount5] " & _
                            " ,[HW_amount6] " & _
                            " ,[HW_price1] " & _
                            " ,[HW_price2] " & _
                            " ,[HW_price3] " & _
                            " ,[HW_price4] " & _
                            " ,[HW_price5] " & _
                            " ,[HW_price6] " & _
                            " ,[HW_money1] " & _
                            " ,[HW_money2] " & _
                            " ,[HW_money3] " & _
                            " ,[HW_money4] " & _
                            " ,[HW_money5] " & _
                            " ,[HW_money6] " & _
                            "           ,[state] ) " & _
                            "     VALUES " & _
                            "           ('" & billno & "'" & _
                            "           ,'" & Me.TextBox2.Text.Trim & "' " & _
                            "           ,'" & Me.TextBox3.Text.Trim & "' " & _
                            "           ,'" & rs_name & "' " & _
                            "           ,getdate() " & _
                            "           ," & IIf(Me.RadioButton1.Checked, 1, 0) & _
                            "           ,'" & Me.ComboBox1.Text.Trim & "'" & _
                            "           ,'" & Me.ComboBox4.Text & "'" & _
                            "           ,'" & Me.TextBox101.Text.Trim & "'" & _
                            "           ,'" & Me.TextBox111.Text.Trim & "'" & _
                            "           ,'" & Me.TextBox121.Text.Trim & "'" & _
                            "           ,'" & Me.TextBox131.Text.Trim & "'" & _
                            "           ,'" & Me.TextBox141.Text.Trim & "'" & _
                            "           ,'" & Me.TextBox151.Text.Trim & "'" & _
                            "           ,'" & Me.TextBox102.Text.Trim & "'" & _
                            "           ,'" & Me.TextBox112.Text.Trim & "'" & _
                            "           ,'" & Me.TextBox122.Text.Trim & "'" & _
                            "           ,'" & Me.TextBox132.Text.Trim & "'" & _
                            "           ,'" & Me.TextBox142.Text.Trim & "'" & _
                            "           ,'" & Me.TextBox152.Text.Trim & "'" & _
                            "           ,'" & Me.TextBox103.Text.Trim & "'" & _
                            "           ,'" & Me.TextBox113.Text.Trim & "'" & _
                            "           ,'" & Me.TextBox123.Text.Trim & "'" & _
                            "           ,'" & Me.TextBox133.Text.Trim & "'" & _
                            "           ,'" & Me.TextBox143.Text.Trim & "'" & _
                            "           ,'" & Me.TextBox153.Text.Trim & "'" & _
                            "           ,'" & Me.TextBox104.Text.Trim & "'" & _
                            "           ,'" & Me.TextBox114.Text.Trim & "'" & _
                            "           ,'" & Me.TextBox124.Text.Trim & "'" & _
                            "           ,'" & Me.TextBox134.Text.Trim & "'" & _
                            "           ,'" & Me.TextBox144.Text.Trim & "'" & _
                            "           ,'" & Me.TextBox154.Text.Trim & "'" & _
                            "           ,'" & Me.TextBox105.Text.Trim & "'" & _
                            "           ,'" & Me.TextBox115.Text.Trim & "'" & _
                            "           ,'" & Me.TextBox125.Text.Trim & "'" & _
                            "           ,'" & Me.TextBox135.Text.Trim & "'" & _
                            "           ,'" & Me.TextBox145.Text.Trim & "'" & _
                            "           ,'" & Me.TextBox155.Text.Trim & "'" & _
                            "           ,0) "

                sqlcmd.CommandText = sql
                sqlcmd.ExecuteNonQuery()

                For i = 0 To Me.ExDataGridView1.Rows.Count - 1

                    sql = "INSERT INTO [T_FPYS_detail] " & _
                        "           ([billno] " & _
                        "           ,[project_no] " & _
                        "           ,[money]) " & _
                        "     VALUES " & _
                        "           ('" & billno & "'" & _
                        "           ,'" & Me.ExDataGridView1.Rows(i).Cells("project_no").Value & "' " & _
                        "           ," & Me.ExDataGridView1.Rows(i).Cells("bc_money").Value & ") "


                    sqlcmd.CommandText = sql
                    sqlcmd.ExecuteNonQuery()

                    sql = "update project_master set fp_money=fp_money + " & Me.ExDataGridView1.Rows(i).Cells("bc_money").Value & _
                    " where  project_no='" & Me.ExDataGridView1.Rows(i).Cells("project_no").Value & "'"
                    sqlcmd.CommandText = sql
                    sqlcmd.ExecuteNonQuery()


                Next

            ElseIf state = "mod" Then



                '如果已经不是新建就不能修改
                sql = "select count(*) from T_FPYS_master where state=0 and billno='" & billno & "'"
                sqlcmd.CommandText = sql
                If sqlcmd.ExecuteScalar = 0 Then
                    Throw New System.Exception("该单据不是新建状态，不可以修改！")
                End If

                sql = "update T_FPYS_master set FP_no='" & Me.TextBox2.Text.Trim & "' ,memo='" & Me.TextBox3.Text.Trim & "',KP_state=" & IIf(Me.RadioButton1.Checked, 1, 0) & ",KP_type='" & Me.ComboBox1.Text.Trim & "'" & _
                ",customer_no='" & Me.ComboBox4.Text & "'" & _
                ",[HW_name1] = '" & Me.TextBox101.Text.Trim & "' " & _
                ",[HW_name2] = '" & Me.TextBox111.Text.Trim & "' " & _
                ",[HW_name3] = '" & Me.TextBox121.Text.Trim & "' " & _
                ",[HW_name4] = '" & Me.TextBox131.Text.Trim & "' " & _
                ",[HW_name5] = '" & Me.TextBox141.Text.Trim & "' " & _
                ",[HW_name6] = '" & Me.TextBox151.Text.Trim & "' " & _
                ",[HW_UNIT1] = '" & Me.TextBox102.Text.Trim & "' " & _
                ",[HW_UNIT2] = '" & Me.TextBox112.Text.Trim & "' " & _
                ",[HW_UNIT3] = '" & Me.TextBox122.Text.Trim & "' " & _
                ",[HW_UNIT4] = '" & Me.TextBox132.Text.Trim & "' " & _
                ",[HW_UNIT5] = '" & Me.TextBox142.Text.Trim & "' " & _
                ",[HW_UNIT6] = '" & Me.TextBox152.Text.Trim & "' " & _
                ",[HW_amount1] = '" & Me.TextBox103.Text.Trim & "' " & _
                ",[HW_amount2] = '" & Me.TextBox113.Text.Trim & "' " & _
                ",[HW_amount3] = '" & Me.TextBox123.Text.Trim & "' " & _
                ",[HW_amount4] = '" & Me.TextBox133.Text.Trim & "' " & _
                ",[HW_amount5] = '" & Me.TextBox143.Text.Trim & "' " & _
                ",[HW_amount6] = '" & Me.TextBox153.Text.Trim & "' " & _
                ",[HW_price1] = '" & Me.TextBox104.Text.Trim & "' " & _
                ",[HW_price2] = '" & Me.TextBox114.Text.Trim & "' " & _
                ",[HW_price3] = '" & Me.TextBox124.Text.Trim & "' " & _
                ",[HW_price4] = '" & Me.TextBox134.Text.Trim & "' " & _
                ",[HW_price5] = '" & Me.TextBox144.Text.Trim & "' " & _
                ",[HW_price6] = '" & Me.TextBox154.Text.Trim & "' " & _
                ",[HW_money1] = '" & Me.TextBox105.Text.Trim & "' " & _
                ",[HW_money2] = '" & Me.TextBox115.Text.Trim & "' " & _
                ",[HW_money3] = '" & Me.TextBox125.Text.Trim & "' " & _
                ",[HW_money4] = '" & Me.TextBox135.Text.Trim & "' " & _
                ",[HW_money5] = '" & Me.TextBox145.Text.Trim & "' " & _
                ",[HW_money6] = '" & Me.TextBox155.Text.Trim & "' " & _
                " where billno='" & billno & "'"

                sqlcmd.CommandText = sql
                sqlcmd.ExecuteNonQuery()


                sql = "update b set fp_money=fp_money- a.money " & _
                        "from (select project_no,sum(money) as money from T_FPYS_detail where billno='" & billno & "' group by project_no)  a left join  " & _
                        "	project_master b on a.project_no=b.project_NO "
                sqlcmd.CommandText = sql
                sqlcmd.ExecuteNonQuery()


                For i = 0 To Me.ExDataGridView1.Rows.Count - 1
                    If Me.ExDataGridView1.Rows(i).Cells("id").Value <> 0 Then
                        s = s & Me.ExDataGridView1.Rows(i).Cells("id").Value & ","



                        sql = "update T_FPYS_detail set money='" & Me.ExDataGridView1.Rows(i).Cells("bc_money").Value & "'" & _
                                                            "  where id=" & Me.ExDataGridView1.Rows(i).Cells("id").Value
                        sqlcmd.CommandText = sql
                        sqlcmd.ExecuteNonQuery()

                        sql = "update project_master set fp_money=fp_money + " & Me.ExDataGridView1.Rows(i).Cells("bc_money").Value & " where  project_no='" & Me.ExDataGridView1.Rows(i).Cells("project_no").Value & "'"
                        sqlcmd.CommandText = sql
                        sqlcmd.ExecuteNonQuery()

                    End If
                Next

                If s.Length <> 0 Then
                    s = s.Substring(0, s.Length - 1)
                    s = " and  id not in (" & s & ")"
                End If


                sql = "delete from T_FPYS_detail where  billno='" & billno & "' " & s
                sqlcmd.CommandText = sql
                sqlcmd.ExecuteNonQuery()


                For i = 0 To Me.ExDataGridView1.Rows.Count - 1
                    If Me.ExDataGridView1.Rows(i).Cells("id").Value = 0 Then
                        sql = "INSERT INTO [T_FPYS_detail] " & _
                                "           ([billno] " & _
                                "           ,[project_no] " & _
                                "           ,[money]) " & _
                                "     VALUES " & _
                                "           ('" & billno & "'" & _
                                "           ,'" & Me.ExDataGridView1.Rows(i).Cells("project_no").Value & "' " & _
                                "           ," & Me.ExDataGridView1.Rows(i).Cells("bc_money").Value & ") "


                        sqlcmd.CommandText = sql
                        sqlcmd.ExecuteNonQuery()

                        sql = "update project_master set fp_money=fp_money + " & Me.ExDataGridView1.Rows(i).Cells("bc_money").Value & " where  project_no='" & Me.ExDataGridView1.Rows(i).Cells("project_no").Value & "'"
                        sqlcmd.CommandText = sql
                        sqlcmd.ExecuteNonQuery()

                    End If
                Next



            End If

            mytrans.Commit()
        Catch ex As Exception
            mytrans.Rollback()
            MsgBox(ex.Message, MsgBoxStyle.OkOnly, "错误")
            Exit Sub
        End Try

        MsgBox("保存成功！", MsgBoxStyle.OkOnly, "提示")

        If state = "new" Then

        ElseIf state = "mod" Then
            CType(Me.Owner, T_FPYS).ExDataGridView1.Rows(CType(Me.Owner, T_FPYS).ExDataGridView1.SelectedCells(0).RowIndex).Cells("fp_no").Value = Me.TextBox2.Text

            CType(Me.Owner, T_FPYS).ExDataGridView1.Rows(CType(Me.Owner, T_FPYS).ExDataGridView1.SelectedCells(0).RowIndex).Cells("memo").Value = Me.TextBox3.Text

            If Me.RadioButton1.Checked Then
                CType(Me.Owner, T_FPYS).ExDataGridView1.Rows(CType(Me.Owner, T_FPYS).ExDataGridView1.SelectedCells(0).RowIndex).Cells("kp_state1").Value = "已开票"
            Else
                CType(Me.Owner, T_FPYS).ExDataGridView1.Rows(CType(Me.Owner, T_FPYS).ExDataGridView1.SelectedCells(0).RowIndex).Cells("kp_state1").Value = "未开票"
            End If

            CType(Me.Owner, T_FPYS).ExDataGridView1.Rows(CType(Me.Owner, T_FPYS).ExDataGridView1.SelectedCells(0).RowIndex).Cells("kp_type").Value = Me.ComboBox1.Text.Trim

            CType(Me.Owner, T_FPYS).ExDataGridView1.Rows(CType(Me.Owner, T_FPYS).ExDataGridView1.SelectedCells(0).RowIndex).Cells("customer_name").Value = Me.ComboBox3.Text
            CType(Me.Owner, T_FPYS).ExDataGridView1.Rows(CType(Me.Owner, T_FPYS).ExDataGridView1.SelectedCells(0).RowIndex).Cells("customer_no").Value = Me.ComboBox4.Text

            CType(Me.Owner, T_FPYS).ExDataGridView1.Rows(CType(Me.Owner, T_FPYS).ExDataGridView1.SelectedCells(0).RowIndex).Cells("nsr_no").Value = Me.TextBox4.Text
            CType(Me.Owner, T_FPYS).ExDataGridView1.Rows(CType(Me.Owner, T_FPYS).ExDataGridView1.SelectedCells(0).RowIndex).Cells("at").Value = Me.TextBox5.Text
            CType(Me.Owner, T_FPYS).ExDataGridView1.Rows(CType(Me.Owner, T_FPYS).ExDataGridView1.SelectedCells(0).RowIndex).Cells("khh_no").Value = Me.TextBox6.Text



            CType(Me.Owner, T_FPYS).ExDataGridView1.Rows(CType(Me.Owner, T_FPYS).ExDataGridView1.SelectedCells(0).RowIndex).Cells("hw_name1").Value = Me.TextBox101.Text
            CType(Me.Owner, T_FPYS).ExDataGridView1.Rows(CType(Me.Owner, T_FPYS).ExDataGridView1.SelectedCells(0).RowIndex).Cells("hw_unit1").Value = Me.TextBox102.Text
            CType(Me.Owner, T_FPYS).ExDataGridView1.Rows(CType(Me.Owner, T_FPYS).ExDataGridView1.SelectedCells(0).RowIndex).Cells("hw_amount1").Value = Me.TextBox103.Text
            CType(Me.Owner, T_FPYS).ExDataGridView1.Rows(CType(Me.Owner, T_FPYS).ExDataGridView1.SelectedCells(0).RowIndex).Cells("HW_price1").Value = Me.TextBox104.Text
            CType(Me.Owner, T_FPYS).ExDataGridView1.Rows(CType(Me.Owner, T_FPYS).ExDataGridView1.SelectedCells(0).RowIndex).Cells("hw_money1").Value = Me.TextBox105.Text

            CType(Me.Owner, T_FPYS).ExDataGridView1.Rows(CType(Me.Owner, T_FPYS).ExDataGridView1.SelectedCells(0).RowIndex).Cells("hw_name2").Value = Me.TextBox111.Text
            CType(Me.Owner, T_FPYS).ExDataGridView1.Rows(CType(Me.Owner, T_FPYS).ExDataGridView1.SelectedCells(0).RowIndex).Cells("hw_unit2").Value = Me.TextBox112.Text
            CType(Me.Owner, T_FPYS).ExDataGridView1.Rows(CType(Me.Owner, T_FPYS).ExDataGridView1.SelectedCells(0).RowIndex).Cells("hw_amount2").Value = Me.TextBox113.Text
            CType(Me.Owner, T_FPYS).ExDataGridView1.Rows(CType(Me.Owner, T_FPYS).ExDataGridView1.SelectedCells(0).RowIndex).Cells("HW_price2").Value = Me.TextBox114.Text
            CType(Me.Owner, T_FPYS).ExDataGridView1.Rows(CType(Me.Owner, T_FPYS).ExDataGridView1.SelectedCells(0).RowIndex).Cells("hw_money2").Value = Me.TextBox115.Text

            CType(Me.Owner, T_FPYS).ExDataGridView1.Rows(CType(Me.Owner, T_FPYS).ExDataGridView1.SelectedCells(0).RowIndex).Cells("hw_name3").Value = Me.TextBox121.Text
            CType(Me.Owner, T_FPYS).ExDataGridView1.Rows(CType(Me.Owner, T_FPYS).ExDataGridView1.SelectedCells(0).RowIndex).Cells("hw_unit3").Value = Me.TextBox122.Text
            CType(Me.Owner, T_FPYS).ExDataGridView1.Rows(CType(Me.Owner, T_FPYS).ExDataGridView1.SelectedCells(0).RowIndex).Cells("hw_amount3").Value = Me.TextBox123.Text
            CType(Me.Owner, T_FPYS).ExDataGridView1.Rows(CType(Me.Owner, T_FPYS).ExDataGridView1.SelectedCells(0).RowIndex).Cells("HW_price3").Value = Me.TextBox124.Text
            CType(Me.Owner, T_FPYS).ExDataGridView1.Rows(CType(Me.Owner, T_FPYS).ExDataGridView1.SelectedCells(0).RowIndex).Cells("hw_money3").Value = Me.TextBox125.Text

            CType(Me.Owner, T_FPYS).ExDataGridView1.Rows(CType(Me.Owner, T_FPYS).ExDataGridView1.SelectedCells(0).RowIndex).Cells("hw_name4").Value = Me.TextBox131.Text
            CType(Me.Owner, T_FPYS).ExDataGridView1.Rows(CType(Me.Owner, T_FPYS).ExDataGridView1.SelectedCells(0).RowIndex).Cells("hw_unit4").Value = Me.TextBox132.Text
            CType(Me.Owner, T_FPYS).ExDataGridView1.Rows(CType(Me.Owner, T_FPYS).ExDataGridView1.SelectedCells(0).RowIndex).Cells("hw_amount4").Value = Me.TextBox133.Text
            CType(Me.Owner, T_FPYS).ExDataGridView1.Rows(CType(Me.Owner, T_FPYS).ExDataGridView1.SelectedCells(0).RowIndex).Cells("HW_price4").Value = Me.TextBox134.Text
            CType(Me.Owner, T_FPYS).ExDataGridView1.Rows(CType(Me.Owner, T_FPYS).ExDataGridView1.SelectedCells(0).RowIndex).Cells("hw_money4").Value = Me.TextBox135.Text

            CType(Me.Owner, T_FPYS).ExDataGridView1.Rows(CType(Me.Owner, T_FPYS).ExDataGridView1.SelectedCells(0).RowIndex).Cells("hw_name5").Value = Me.TextBox141.Text
            CType(Me.Owner, T_FPYS).ExDataGridView1.Rows(CType(Me.Owner, T_FPYS).ExDataGridView1.SelectedCells(0).RowIndex).Cells("hw_unit5").Value = Me.TextBox142.Text
            CType(Me.Owner, T_FPYS).ExDataGridView1.Rows(CType(Me.Owner, T_FPYS).ExDataGridView1.SelectedCells(0).RowIndex).Cells("hw_amount5").Value = Me.TextBox143.Text
            CType(Me.Owner, T_FPYS).ExDataGridView1.Rows(CType(Me.Owner, T_FPYS).ExDataGridView1.SelectedCells(0).RowIndex).Cells("HW_price5").Value = Me.TextBox144.Text
            CType(Me.Owner, T_FPYS).ExDataGridView1.Rows(CType(Me.Owner, T_FPYS).ExDataGridView1.SelectedCells(0).RowIndex).Cells("hw_money5").Value = Me.TextBox145.Text

            CType(Me.Owner, T_FPYS).ExDataGridView1.Rows(CType(Me.Owner, T_FPYS).ExDataGridView1.SelectedCells(0).RowIndex).Cells("hw_name6").Value = Me.TextBox151.Text
            CType(Me.Owner, T_FPYS).ExDataGridView1.Rows(CType(Me.Owner, T_FPYS).ExDataGridView1.SelectedCells(0).RowIndex).Cells("hw_unit6").Value = Me.TextBox152.Text
            CType(Me.Owner, T_FPYS).ExDataGridView1.Rows(CType(Me.Owner, T_FPYS).ExDataGridView1.SelectedCells(0).RowIndex).Cells("hw_amount6").Value = Me.TextBox153.Text
            CType(Me.Owner, T_FPYS).ExDataGridView1.Rows(CType(Me.Owner, T_FPYS).ExDataGridView1.SelectedCells(0).RowIndex).Cells("HW_price6").Value = Me.TextBox154.Text
            CType(Me.Owner, T_FPYS).ExDataGridView1.Rows(CType(Me.Owner, T_FPYS).ExDataGridView1.SelectedCells(0).RowIndex).Cells("hw_money6").Value = Me.TextBox155.Text

        End If
        Me.DialogResult = Windows.Forms.DialogResult.OK
    End Sub

    Private Sub T_FPYS_add_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim sql As String = ""
        Dim sqlcon As New SqlConnection
        Dim sqlcmd As New SqlCommand
        Dim sqlad As New SqlDataAdapter
        Dim i As Int32

        Dim dt As New DataTable
        sqlcon = getconn()
        sqlcmd.Connection = sqlcon

        Me.ComboBox1.Items.Clear()
        Me.ComboBox1.Items.Add("")
        Me.ComboBox1.Items.Add("建安票")
        Me.ComboBox1.Items.Add("增值税票")
        Me.ComboBox1.Items.Add("普通发票")
        Me.ComboBox1.Items.Add("收据")
        Me.ComboBox1.Items.Add("无收据")


        sql = "select * from t_customer where stop=1 "
        sqlcmd.CommandText = sql
        sqlad.SelectCommand = sqlcmd
        dt.Clear()
        sqlad.Fill(dt)

        Me.ComboBox3.Items.Clear()
        Me.ComboBox4.Items.Clear()

        Me.ComboBox3.Items.Add("")
        Me.ComboBox4.Items.Add("")

        For i = 0 To dt.Rows.Count - 1
            Me.ComboBox3.Items.Add(dt.Rows(i)("customer_name"))
            Me.ComboBox4.Items.Add(dt.Rows(i)("customer_no"))
        Next


        If state = "new" Then


            Me.Text = "增加应收发票"

            sql = "SELECT a.*,case state when 0 then '新建' when 1 then '审核' when 2 then '结案' end as state1,project_money-FP_money as WKP_money,project_money-FP_money as BC_money " & _
                     "FROM project_master a " & _
                     "where a.id=-1"


            Me.TextBox103.Text = 0
            Me.TextBox104.Text = 0
            Me.TextBox105.Text = 0

            Me.TextBox113.Text = 0
            Me.TextBox114.Text = 0
            Me.TextBox115.Text = 0

            Me.TextBox123.Text = 0
            Me.TextBox124.Text = 0
            Me.TextBox125.Text = 0

            Me.TextBox133.Text = 0
            Me.TextBox134.Text = 0
            Me.TextBox135.Text = 0

            Me.TextBox143.Text = 0
            Me.TextBox144.Text = 0
            Me.TextBox145.Text = 0

            Me.TextBox153.Text = 0
            Me.TextBox154.Text = 0
            Me.TextBox155.Text = 0



        ElseIf state = "mod" Then

            Me.Text = "修改应收发票"

            Me.TextBox1.Enabled = False

            Me.TextBox1.Text = CType(Me.Owner, T_FPYS).ExDataGridView1.Rows(CType(Me.Owner, T_FPYS).ExDataGridView1.SelectedCells(0).RowIndex).Cells("billno").Value
            Me.TextBox2.Text = CType(Me.Owner, T_FPYS).ExDataGridView1.Rows(CType(Me.Owner, T_FPYS).ExDataGridView1.SelectedCells(0).RowIndex).Cells("FP_no").Value
            Me.TextBox3.Text = CType(Me.Owner, T_FPYS).ExDataGridView1.Rows(CType(Me.Owner, T_FPYS).ExDataGridView1.SelectedCells(0).RowIndex).Cells("memo").Value

            If CType(Me.Owner, T_FPYS).ExDataGridView1.Rows(CType(Me.Owner, T_FPYS).ExDataGridView1.SelectedCells(0).RowIndex).Cells("kp_state1").Value = "已开票" Then
                Me.RadioButton1.Checked = True
            Else
                Me.RadioButton2.Checked = True
            End If
            Me.ComboBox1.Text = CType(Me.Owner, T_FPYS).ExDataGridView1.Rows(CType(Me.Owner, T_FPYS).ExDataGridView1.SelectedCells(0).RowIndex).Cells("kp_type").Value


            sql = "select a.*,b.project_no,b.project_name,b.project_money,b.fp_money,b.effective_date,b.memo,b.project_money-b.fp_money as WKP_money,a.money as bc_money " & _
                    "from t_fpys_detail a left join  " & _
                    "	project_master b on a.project_no=b.project_no " & _
                    " where a.billno='" & billno & "'"


            Me.ComboBox4.Text = CType(Me.Owner, T_FPYS).ExDataGridView1.Rows(CType(Me.Owner, T_FPYS).ExDataGridView1.SelectedCells(0).RowIndex).Cells("customer_no").Value

            Me.TextBox4.Text = CType(Me.Owner, T_FPYS).ExDataGridView1.Rows(CType(Me.Owner, T_FPYS).ExDataGridView1.SelectedCells(0).RowIndex).Cells("nsr_no").Value
            Me.TextBox5.Text = CType(Me.Owner, T_FPYS).ExDataGridView1.Rows(CType(Me.Owner, T_FPYS).ExDataGridView1.SelectedCells(0).RowIndex).Cells("at").Value
            Me.TextBox6.Text = CType(Me.Owner, T_FPYS).ExDataGridView1.Rows(CType(Me.Owner, T_FPYS).ExDataGridView1.SelectedCells(0).RowIndex).Cells("khh_no").Value



            Me.TextBox101.Text = CType(Me.Owner, T_FPYS).ExDataGridView1.Rows(CType(Me.Owner, T_FPYS).ExDataGridView1.SelectedCells(0).RowIndex).Cells("hw_name1").Value
            Me.TextBox102.Text = CType(Me.Owner, T_FPYS).ExDataGridView1.Rows(CType(Me.Owner, T_FPYS).ExDataGridView1.SelectedCells(0).RowIndex).Cells("hw_unit1").Value
            Me.TextBox103.Text = Format(CType(Me.Owner, T_FPYS).ExDataGridView1.Rows(CType(Me.Owner, T_FPYS).ExDataGridView1.SelectedCells(0).RowIndex).Cells("hw_amount1").Value, "#0.00")
            Me.TextBox104.Text = CType(Me.Owner, T_FPYS).ExDataGridView1.Rows(CType(Me.Owner, T_FPYS).ExDataGridView1.SelectedCells(0).RowIndex).Cells("HW_price1").Value
            Me.TextBox105.Text = Format(CType(Me.Owner, T_FPYS).ExDataGridView1.Rows(CType(Me.Owner, T_FPYS).ExDataGridView1.SelectedCells(0).RowIndex).Cells("hw_money1").Value, "#0.00")

            Me.TextBox111.Text = CType(Me.Owner, T_FPYS).ExDataGridView1.Rows(CType(Me.Owner, T_FPYS).ExDataGridView1.SelectedCells(0).RowIndex).Cells("hw_name2").Value
            Me.TextBox112.Text = CType(Me.Owner, T_FPYS).ExDataGridView1.Rows(CType(Me.Owner, T_FPYS).ExDataGridView1.SelectedCells(0).RowIndex).Cells("hw_unit2").Value
            Me.TextBox113.Text = Format(CType(Me.Owner, T_FPYS).ExDataGridView1.Rows(CType(Me.Owner, T_FPYS).ExDataGridView1.SelectedCells(0).RowIndex).Cells("hw_amount2").Value, "#0.00")
            Me.TextBox114.Text = CType(Me.Owner, T_FPYS).ExDataGridView1.Rows(CType(Me.Owner, T_FPYS).ExDataGridView1.SelectedCells(0).RowIndex).Cells("HW_price2").Value
            Me.TextBox115.Text = Format(CType(Me.Owner, T_FPYS).ExDataGridView1.Rows(CType(Me.Owner, T_FPYS).ExDataGridView1.SelectedCells(0).RowIndex).Cells("hw_money2").Value, "#0.00")

            Me.TextBox121.Text = CType(Me.Owner, T_FPYS).ExDataGridView1.Rows(CType(Me.Owner, T_FPYS).ExDataGridView1.SelectedCells(0).RowIndex).Cells("hw_name3").Value
            Me.TextBox122.Text = CType(Me.Owner, T_FPYS).ExDataGridView1.Rows(CType(Me.Owner, T_FPYS).ExDataGridView1.SelectedCells(0).RowIndex).Cells("hw_unit3").Value
            Me.TextBox123.Text = Format(CType(Me.Owner, T_FPYS).ExDataGridView1.Rows(CType(Me.Owner, T_FPYS).ExDataGridView1.SelectedCells(0).RowIndex).Cells("hw_amount3").Value, "#0.00")
            Me.TextBox124.Text = CType(Me.Owner, T_FPYS).ExDataGridView1.Rows(CType(Me.Owner, T_FPYS).ExDataGridView1.SelectedCells(0).RowIndex).Cells("HW_price3").Value
            Me.TextBox125.Text = Format(CType(Me.Owner, T_FPYS).ExDataGridView1.Rows(CType(Me.Owner, T_FPYS).ExDataGridView1.SelectedCells(0).RowIndex).Cells("hw_money3").Value, "#0.00")

            Me.TextBox131.Text = CType(Me.Owner, T_FPYS).ExDataGridView1.Rows(CType(Me.Owner, T_FPYS).ExDataGridView1.SelectedCells(0).RowIndex).Cells("hw_name4").Value
            Me.TextBox132.Text = CType(Me.Owner, T_FPYS).ExDataGridView1.Rows(CType(Me.Owner, T_FPYS).ExDataGridView1.SelectedCells(0).RowIndex).Cells("hw_unit4").Value
            Me.TextBox133.Text = Format(CType(Me.Owner, T_FPYS).ExDataGridView1.Rows(CType(Me.Owner, T_FPYS).ExDataGridView1.SelectedCells(0).RowIndex).Cells("hw_amount4").Value, "#0.00")
            Me.TextBox134.Text = CType(Me.Owner, T_FPYS).ExDataGridView1.Rows(CType(Me.Owner, T_FPYS).ExDataGridView1.SelectedCells(0).RowIndex).Cells("HW_price4").Value
            Me.TextBox135.Text = Format(CType(Me.Owner, T_FPYS).ExDataGridView1.Rows(CType(Me.Owner, T_FPYS).ExDataGridView1.SelectedCells(0).RowIndex).Cells("hw_money4").Value, "#0.00")

            Me.TextBox141.Text = CType(Me.Owner, T_FPYS).ExDataGridView1.Rows(CType(Me.Owner, T_FPYS).ExDataGridView1.SelectedCells(0).RowIndex).Cells("hw_name5").Value
            Me.TextBox142.Text = CType(Me.Owner, T_FPYS).ExDataGridView1.Rows(CType(Me.Owner, T_FPYS).ExDataGridView1.SelectedCells(0).RowIndex).Cells("hw_unit5").Value
            Me.TextBox143.Text = Format(CType(Me.Owner, T_FPYS).ExDataGridView1.Rows(CType(Me.Owner, T_FPYS).ExDataGridView1.SelectedCells(0).RowIndex).Cells("hw_amount5").Value, "#0.00")
            Me.TextBox144.Text = CType(Me.Owner, T_FPYS).ExDataGridView1.Rows(CType(Me.Owner, T_FPYS).ExDataGridView1.SelectedCells(0).RowIndex).Cells("HW_price5").Value
            Me.TextBox145.Text = Format(CType(Me.Owner, T_FPYS).ExDataGridView1.Rows(CType(Me.Owner, T_FPYS).ExDataGridView1.SelectedCells(0).RowIndex).Cells("hw_money5").Value, "#0.00")

            Me.TextBox151.Text = CType(Me.Owner, T_FPYS).ExDataGridView1.Rows(CType(Me.Owner, T_FPYS).ExDataGridView1.SelectedCells(0).RowIndex).Cells("hw_name6").Value
            Me.TextBox152.Text = CType(Me.Owner, T_FPYS).ExDataGridView1.Rows(CType(Me.Owner, T_FPYS).ExDataGridView1.SelectedCells(0).RowIndex).Cells("hw_unit6").Value
            Me.TextBox153.Text = Format(CType(Me.Owner, T_FPYS).ExDataGridView1.Rows(CType(Me.Owner, T_FPYS).ExDataGridView1.SelectedCells(0).RowIndex).Cells("hw_amount6").Value, "#0.00")
            Me.TextBox154.Text = CType(Me.Owner, T_FPYS).ExDataGridView1.Rows(CType(Me.Owner, T_FPYS).ExDataGridView1.SelectedCells(0).RowIndex).Cells("HW_price6").Value
            Me.TextBox155.Text = Format(CType(Me.Owner, T_FPYS).ExDataGridView1.Rows(CType(Me.Owner, T_FPYS).ExDataGridView1.SelectedCells(0).RowIndex).Cells("hw_money6").Value, "#0.00")


        End If

        sqlcmd.CommandText = sql
        sqlad.SelectCommand = sqlcmd
        dt_p.Rows.Clear()
        sqlad.Fill(dt_p)

        Me.ExDataGridView1.SetDataSource(dt_p)
        Me.ExDataGridView1.Columns.Clear()

        Me.ExDataGridView1.AddColumn("project_no", "工程号")
        Me.ExDataGridView1.AddColumn("project_name", "工程名称")
        Me.ExDataGridView1.AddColumn("project_money", "工程金额", , , , , "#0")
        Me.ExDataGridView1.AddColumn("FP_money", "已开票金额", , , , , "#0")
        Me.ExDataGridView1.AddColumn("WKP_money", "未开票金额", , , , , "#0")
        Me.ExDataGridView1.AddColumn("bc_money", "本次开票金额", , False, , , "#0")


        Me.ExDataGridView1.AddColumn("id", "id", , , , , , False)
    End Sub

    Private Sub RadioButton1_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RadioButton1.CheckedChanged
        Me.TextBox2.Enabled = True
    End Sub

    Private Sub RadioButton2_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RadioButton2.CheckedChanged
        Me.TextBox2.Text = ""
        Me.TextBox2.Enabled = False
    End Sub

    Private Sub ComboBox3_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ComboBox3.SelectedIndexChanged
        Dim sql As String = ""
        Dim sqlcon As New SqlConnection
        Dim sqlcmd As New SqlCommand
        Dim sqlad As New SqlDataAdapter


        Dim dt As New DataTable
        sqlcon = getconn()
        sqlcmd.Connection = sqlcon


        Me.ComboBox4.SelectedIndex = Me.ComboBox3.SelectedIndex

        If Me.ComboBox4.SelectedIndex <> 0 Then
            sql = "select * from t_customer where customer_no='" & Me.ComboBox4.Text & "' "
            sqlcmd.CommandText = sql
            sqlad.SelectCommand = sqlcmd
            dt.Clear()
            sqlad.Fill(dt)

            Me.TextBox4.Text = dt.Rows(0)("NSR_no")
            Me.TextBox5.Text = dt.Rows(0)("address") + " " + dt.Rows(0)("tel")
            Me.TextBox6.Text = dt.Rows(0)("KHH_no")
        End If
      


    End Sub

    Private Sub ComboBox4_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ComboBox4.SelectedIndexChanged
        Me.ComboBox3.SelectedIndex = Me.ComboBox4.SelectedIndex
    End Sub

    Private Sub TextBox103_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles TextBox103.LostFocus
        If Me.TextBox103.Text.Trim.Length = 0 Then
            Me.TextBox103.Text = 0
            Me.TextBox105.Text = 0
        ElseIf Not IsNumeric(Me.TextBox103.Text.Trim) Then
            MsgBox("数量必须是数字！", MsgBoxStyle.OkOnly, "提示")
            Me.TextBox103.Text = 0
            Me.TextBox105.Text = 0
            Me.TextBox103.Focus()
            Exit Sub
        Else
            Me.TextBox105.Text = CDec(Me.TextBox103.Text.Trim) * CDec(Me.TextBox104.Text.Trim)
        End If
    End Sub

    Private Sub TextBox103_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TextBox103.TextChanged

    End Sub

    Private Sub TextBox113_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles TextBox113.LostFocus
        If Me.TextBox113.Text.Trim.Length = 0 Then
            Me.TextBox113.Text = 0
            Me.TextBox115.Text = 0
        ElseIf Not IsNumeric(Me.TextBox113.Text.Trim) Then
            MsgBox("数量必须是数字！", MsgBoxStyle.OkOnly, "提示")
            Me.TextBox113.Text = 0
            Me.TextBox115.Text = 0
            Me.TextBox113.Focus()
            Exit Sub
        Else
            Me.TextBox115.Text = CDec(Me.TextBox113.Text.Trim) * CDec(Me.TextBox114.Text.Trim)
        End If
    End Sub

    Private Sub TextBox123_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles TextBox123.LostFocus
        If Me.TextBox123.Text.Trim.Length = 0 Then
            Me.TextBox123.Text = 0
            Me.TextBox125.Text = 0
        ElseIf Not IsNumeric(Me.TextBox123.Text.Trim) Then
            MsgBox("数量必须是数字！", MsgBoxStyle.OkOnly, "提示")
            Me.TextBox123.Text = 0
            Me.TextBox125.Text = 0
            Me.TextBox123.Focus()
            Exit Sub
        Else
            Me.TextBox125.Text = CDec(Me.TextBox123.Text.Trim) * CDec(Me.TextBox124.Text.Trim)
        End If
    End Sub

    Private Sub TextBox133_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles TextBox133.LostFocus
        If Me.TextBox133.Text.Trim.Length = 0 Then
            Me.TextBox133.Text = 0
            Me.TextBox135.Text = 0
        ElseIf Not IsNumeric(Me.TextBox133.Text.Trim) Then
            MsgBox("数量必须是数字！", MsgBoxStyle.OkOnly, "提示")
            Me.TextBox133.Text = 0
            Me.TextBox135.Text = 0
            Me.TextBox133.Focus()
            Exit Sub
        Else
            Me.TextBox135.Text = CDec(Me.TextBox133.Text.Trim) * CDec(Me.TextBox134.Text.Trim)
        End If
    End Sub

    Private Sub TextBox143_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles TextBox143.LostFocus
        If Me.TextBox143.Text.Trim.Length = 0 Then
            Me.TextBox143.Text = 0
            Me.TextBox145.Text = 0
        ElseIf Not IsNumeric(Me.TextBox143.Text.Trim) Then
            MsgBox("数量必须是数字！", MsgBoxStyle.OkOnly, "提示")
            Me.TextBox143.Text = 0
            Me.TextBox145.Text = 0
            Me.TextBox143.Focus()
            Exit Sub
        Else
            Me.TextBox145.Text = CDec(Me.TextBox143.Text.Trim) * CDec(Me.TextBox144.Text.Trim)
        End If
    End Sub

    Private Sub TextBox153_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles TextBox153.LostFocus
        If Me.TextBox153.Text.Trim.Length = 0 Then
            Me.TextBox153.Text = 0
            Me.TextBox155.Text = 0
        ElseIf Not IsNumeric(Me.TextBox153.Text.Trim) Then
            MsgBox("数量必须是数字！", MsgBoxStyle.OkOnly, "提示")
            Me.TextBox153.Text = 0
            Me.TextBox155.Text = 0
            Me.TextBox153.Focus()
            Exit Sub
        Else
            Me.TextBox155.Text = CDec(Me.TextBox153.Text.Trim) * CDec(Me.TextBox154.Text.Trim)
        End If
    End Sub

    Private Sub TextBox104_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles TextBox104.LostFocus
        If Me.TextBox104.Text.Trim.Length = 0 Then
            Me.TextBox104.Text = 0
            Me.TextBox105.Text = 0
        ElseIf Not IsNumeric(Me.TextBox104.Text.Trim) Then
            MsgBox("单价必须是数字！", MsgBoxStyle.OkOnly, "提示")

            Me.TextBox104.Focus()
            Exit Sub
        Else
            Me.TextBox105.Text = CDec(Me.TextBox103.Text.Trim) * CDec(Me.TextBox104.Text.Trim)
        End If
    End Sub

    Private Sub TextBox114_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles TextBox114.LostFocus
        If Me.TextBox114.Text.Trim.Length = 0 Then
            Me.TextBox114.Text = 0
            Me.TextBox115.Text = 0
        ElseIf Not IsNumeric(Me.TextBox114.Text.Trim) Then
            MsgBox("单价必须是数字！", MsgBoxStyle.OkOnly, "提示")

            Me.TextBox114.Focus()
            Exit Sub
        Else
            Me.TextBox115.Text = CDec(Me.TextBox113.Text.Trim) * CDec(Me.TextBox114.Text.Trim)
        End If
    End Sub

    Private Sub TextBox124_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles TextBox124.LostFocus
        If Me.TextBox124.Text.Trim.Length = 0 Then
            Me.TextBox124.Text = 0
            Me.TextBox125.Text = 0
        ElseIf Not IsNumeric(Me.TextBox124.Text.Trim) Then
            MsgBox("单价必须是数字！", MsgBoxStyle.OkOnly, "提示")

            Me.TextBox124.Focus()
            Exit Sub
        Else
            Me.TextBox125.Text = CDec(Me.TextBox123.Text.Trim) * CDec(Me.TextBox124.Text.Trim)
        End If
    End Sub

    Private Sub TextBox134_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles TextBox134.LostFocus
        If Me.TextBox134.Text.Trim.Length = 0 Then
            Me.TextBox134.Text = 0
            Me.TextBox135.Text = 0
        ElseIf Not IsNumeric(Me.TextBox134.Text.Trim) Then
            MsgBox("单价必须是数字！", MsgBoxStyle.OkOnly, "提示")

            Me.TextBox134.Focus()
            Exit Sub
        Else
            Me.TextBox135.Text = CDec(Me.TextBox133.Text.Trim) * CDec(Me.TextBox134.Text.Trim)
        End If
    End Sub

    Private Sub TextBox144_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles TextBox144.LostFocus
        If Me.TextBox144.Text.Trim.Length = 0 Then
            Me.TextBox144.Text = 0
            Me.TextBox145.Text = 0
        ElseIf Not IsNumeric(Me.TextBox144.Text.Trim) Then
            MsgBox("单价必须是数字！", MsgBoxStyle.OkOnly, "提示")

            Me.TextBox144.Focus()
            Exit Sub
        Else
            Me.TextBox145.Text = CDec(Me.TextBox143.Text.Trim) * CDec(Me.TextBox144.Text.Trim)
        End If
    End Sub

    Private Sub TextBox154_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles TextBox154.LostFocus
        If Me.TextBox154.Text.Trim.Length = 0 Then
            Me.TextBox154.Text = 0
            Me.TextBox155.Text = 0
        ElseIf Not IsNumeric(Me.TextBox154.Text.Trim) Then
            MsgBox("单价必须是数字！", MsgBoxStyle.OkOnly, "提示")

            Me.TextBox154.Focus()
            Exit Sub
        Else
            Me.TextBox155.Text = CDec(Me.TextBox153.Text.Trim) * CDec(Me.TextBox154.Text.Trim)
        End If
    End Sub

    Private Sub TextBox105_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles TextBox105.LostFocus
        If Me.TextBox105.Text.Trim.Length = 0 Then
            Me.TextBox104.Text = 0
            Me.TextBox105.Text = 0
        ElseIf Not IsNumeric(Me.TextBox105.Text.Trim) Then
            MsgBox("金额必须是数字！", MsgBoxStyle.OkOnly, "提示")

            Me.TextBox105.Focus()
            Exit Sub
        Else
            If Me.TextBox103.Text.Trim <> 0 Then
                Me.TextBox104.Text = CDec(Me.TextBox105.Text.Trim) / CDec(Me.TextBox103.Text.Trim)
            Else
                Me.TextBox104.Text = 0
            End If
        End If
    End Sub

    Private Sub TextBox115_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles TextBox115.LostFocus
        If Me.TextBox115.Text.Trim.Length = 0 Then
            Me.TextBox114.Text = 0
            Me.TextBox115.Text = 0
        ElseIf Not IsNumeric(Me.TextBox115.Text.Trim) Then
            MsgBox("金额必须是数字！", MsgBoxStyle.OkOnly, "提示")

            Me.TextBox115.Focus()
            Exit Sub
        Else
            If Me.TextBox113.Text.Trim <> 0 Then
                Me.TextBox114.Text = CDec(Me.TextBox115.Text.Trim) / CDec(Me.TextBox113.Text.Trim)
            Else
                Me.TextBox114.Text = 0
            End If
        End If
    End Sub

    Private Sub TextBox125_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles TextBox125.LostFocus
        If Me.TextBox125.Text.Trim.Length = 0 Then
            Me.TextBox124.Text = 0
            Me.TextBox125.Text = 0
        ElseIf Not IsNumeric(Me.TextBox125.Text.Trim) Then
            MsgBox("金额必须是数字！", MsgBoxStyle.OkOnly, "提示")

            Me.TextBox125.Focus()
            Exit Sub
        Else
            If Me.TextBox123.Text.Trim <> 0 Then
                Me.TextBox124.Text = CDec(Me.TextBox125.Text.Trim) / CDec(Me.TextBox123.Text.Trim)
            Else
                Me.TextBox124.Text = 0
            End If
        End If
    End Sub

    Private Sub TextBox135_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles TextBox135.LostFocus
        If Me.TextBox135.Text.Trim.Length = 0 Then
            Me.TextBox134.Text = 0
            Me.TextBox135.Text = 0
        ElseIf Not IsNumeric(Me.TextBox135.Text.Trim) Then
            MsgBox("金额必须是数字！", MsgBoxStyle.OkOnly, "提示")

            Me.TextBox135.Focus()
            Exit Sub
        Else
            If Me.TextBox133.Text.Trim <> 0 Then
                Me.TextBox134.Text = CDec(Me.TextBox135.Text.Trim) / CDec(Me.TextBox133.Text.Trim)
            Else
                Me.TextBox134.Text = 0
            End If
        End If
    End Sub

    Private Sub TextBox145_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles TextBox145.LostFocus
        If Me.TextBox145.Text.Trim.Length = 0 Then
            Me.TextBox144.Text = 0
            Me.TextBox145.Text = 0
        ElseIf Not IsNumeric(Me.TextBox145.Text.Trim) Then
            MsgBox("金额必须是数字！", MsgBoxStyle.OkOnly, "提示")

            Me.TextBox145.Focus()
            Exit Sub
        Else
            If Me.TextBox143.Text.Trim <> 0 Then
                Me.TextBox144.Text = CDec(Me.TextBox145.Text.Trim) / CDec(Me.TextBox143.Text.Trim)
            Else
                Me.TextBox144.Text = 0
            End If
        End If
    End Sub

  
  
    Private Sub TextBox155_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles TextBox155.LostFocus
        If Me.TextBox155.Text.Trim.Length = 0 Then
            Me.TextBox154.Text = 0
            Me.TextBox155.Text = 0
        ElseIf Not IsNumeric(Me.TextBox155.Text.Trim) Then
            MsgBox("金额必须是数字！", MsgBoxStyle.OkOnly, "提示")

            Me.TextBox155.Focus()
            Exit Sub
        Else
            If Me.TextBox153.Text.Trim <> 0 Then
                Me.TextBox154.Text = CDec(Me.TextBox155.Text.Trim) / CDec(Me.TextBox153.Text.Trim)
            Else
                Me.TextBox154.Text = 0
            End If
        End If
    End Sub
End Class