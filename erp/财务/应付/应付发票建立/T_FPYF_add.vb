Imports System.Data.SqlClient
Public Class T_FPYF_add
    Friend state As String
    Friend billno As String
    Friend dt_p As New DataTable
    Friend dt_m As New DataTable

    Private Sub 添加记录ToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles 添加记录ToolStripMenuItem.Click
        Dim T_FPYF_add_detail As New T_FPYF_add_detail
        Dim T_FPYF_add_JGD_detail As New T_FPYF_add_JGD_detail

        Dim sqlcon As New SqlConnection
        Dim sqlcmd As New SqlCommand
        Dim sqlad As New SqlDataAdapter

        Dim dt As New DataTable
        Dim sql As String
        Dim i As int32
        Dim s As String = ""

        For i = 0 To Me.ExDataGridView1.Rows.Count - 1
            s = s & "'" & Me.ExDataGridView1.Rows(i).Cells("billno").Value & "',"
        Next

        If s.Trim.Length > 0 Then
            s = s.Substring(0, s.Length - 1)
        End If

        Me.Tag = ""

        If Me.ComboBox2.SelectedIndex = 0 Then
            T_FPYF_add_detail.billno = s
            T_FPYF_add_detail.Owner = Me
            T_FPYF_add_detail.ShowDialog()

            If T_FPYF_add_detail.DialogResult = Windows.Forms.DialogResult.Cancel Then
                T_FPYF_add_detail.Dispose()
                Exit Sub
            End If


            If Me.Tag = "" Then
                Exit Sub
            End If



            sqlcon = getconn()
            sqlcmd.Connection = sqlcon

            sql = "select *,money-kp_money as wkp_money,money-kp_money as bc_money from  " & _
                "(select * from t_stockbill_master " & _
                "                where type=2 and billkindcode=20 and tempflag=0 and money>kp_money and billno in (" & Me.Tag & ") " & _
                "union all " & _
                "select * from t_stockbill_master_history  " & _
                "                where type=2 and billkindcode=20 and tempflag=0 and money>kp_money and billno in (" & Me.Tag & ") ) a " & _
                "  order by a.id "
        ElseIf Me.ComboBox2.SelectedIndex = 1 Then
            T_FPYF_add_JGD_detail.billno = s
            T_FPYF_add_JGD_detail.Owner = Me
            T_FPYF_add_JGD_detail.ShowDialog()

            If T_FPYF_add_JGD_detail.DialogResult = Windows.Forms.DialogResult.Cancel Then
                T_FPYF_add_JGD_detail.Dispose()
                Exit Sub
            End If


            If Me.Tag = "" Then
                Exit Sub
            End If



            sqlcon = getconn()
            sqlcmd.Connection = sqlcon

            sql = "select *,money-kp_money as wkp_money,money-kp_money as bc_money from  " & _
            "  t_outwork_master a " & _
            " where money>kp_money and billno in (" & Me.Tag & ") " & _
            "order by id "
        Else
            MsgBox("选择的不是已经存在的开票类型！请检查！", MsgBoxStyle.OkOnly, "提示")
            Exit Sub
        End If


        sqlcmd.CommandText = sql

        sqlad.SelectCommand = sqlcmd
        sqlad.Fill(dt)

        If Me.ExDataGridView1.Columns.Count = 0 Then
            dt_p = dt

            Me.ExDataGridView1.SetDataSource(dt_p)
            Me.ExDataGridView1.Columns.Clear()

            Me.ExDataGridView1.AddColumn("billno", "入库单号", 120)

            Me.ExDataGridView1.AddColumn("money", "单据金额", , , , , "#0.00")
            Me.ExDataGridView1.AddColumn("KP_money", "已开票金额", , , , , "#0.00")
            Me.ExDataGridView1.AddColumn("WKP_money", "未开票金额", , , , , "#0.00")
            Me.ExDataGridView1.AddColumn("BC_money", "本次开票金额", , False, , , "#0.00")


            Me.ExDataGridView1.AddColumn("id", "id", , , , , , False)
        Else
            For i = 0 To dt.Rows.Count - 1

                Dim DR As DataRow = Me.dt_p.NewRow

                DR("billno") = dt.Rows(i)("billno")

                DR("money") = dt.Rows(i)("money")
                DR("KP_money") = dt.Rows(i)("KP_money")

                DR("WKP_money") = dt.Rows(i)("WKP_money")
                DR("BC_money") = dt.Rows(i)("BC_money")



                DR("id") = 0

                dt_p.Rows.Add(DR)


            Next
        End If

        If Me.ExDataGridView1.Rows.Count = 0 Then
            Me.ComboBox2.Enabled = True
        Else
            Me.ComboBox2.Enabled = False
        End If

    End Sub

    Private Sub ToolStripButton_exit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripButton_exit.Click
        Me.DialogResult = Windows.Forms.DialogResult.Cancel
    End Sub


    Private Sub 删除记录ToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles 删除记录ToolStripMenuItem.Click

        If MsgBox("确认要删除此记录吗？", MsgBoxStyle.YesNo, "提示") = MsgBoxResult.No Then
            Exit Sub
        End If




        Me.ExDataGridView1.Rows.RemoveAt(Me.ExDataGridView1.SelectedCells(0).RowIndex)
        Me.dt_p.AcceptChanges()

        If Me.ExDataGridView1.Rows.Count = 0 Then
            Me.ComboBox2.Enabled = True
        Else
            Me.ComboBox2.Enabled = False
        End If
    End Sub

    Private Sub ToolStripButton_save_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripButton_save.Click


        Dim sqlcon As New SqlConnection
        Dim sqlcmd As New SqlCommand
        Dim sqlad As New SqlDataAdapter
        Dim mytrans As SqlTransaction

        Dim dt As New DataTable
        Dim sql As String
        Dim i As int32

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


                sql = "update t_dj_num set num=num+1 where  tt='FPF'"
                sqlcmd.CommandText = sql
                sqlcmd.ExecuteNonQuery()

                sql = "select * from t_dj_num where tt='FPF'"
                sqlcmd.CommandText = sql
                sqlad.SelectCommand = sqlcmd
                dt.Rows.Clear()
                sqlad.Fill(dt)

                billno = dt.Rows(0)("TT") & "-" & GetAccountterm() & "-" & Format(dt.Rows(0)("num"), dt.Rows(0)("format"))




                sql = "INSERT INTO [T_FPYF_master] " & _
                            "           ([billno] " & _
                            "           ,[FP_no] " & _
                            "           ,[memo] " & _
                            "           ,[input_man] " & _
                            "           ,[input_date] " & _
                            "           ,[KP_state] " & _
                            "           ,[KP_type] " & _
                            "           ,[FP_type] " & _
                            "           ,[state] ) " & _
                            "     VALUES " & _
                            "           ('" & billno & "'" & _
                            "           ,'" & Me.TextBox2.Text.Trim & "' " & _
                            "           ,'" & Me.TextBox3.Text.Trim & "' " & _
                            "           ,'" & rs_name & "' " & _
                            "           ,getdate() " & _
                            "           ," & IIf(Me.RadioButton1.Checked, 1, 0) & _
                            "           ,'" & Me.ComboBox1.Text.Trim & "'" & _
                            "           ,'" & Me.ComboBox2.SelectedIndex & "'" & _
                            "           ,0) "

                sqlcmd.CommandText = sql
                sqlcmd.ExecuteNonQuery()

                For i = 0 To Me.ExDataGridView1.Rows.Count - 1

                    sql = "INSERT INTO [T_FPYF_detail] " & _
                        "           ([billno] " & _
                        "           ,[RK_billno] " & _
                        "           ,[money]) " & _
                        "     VALUES " & _
                        "           ('" & billno & "'" & _
                        "           ,'" & Me.ExDataGridView1.Rows(i).Cells("billno").Value & "' " & _
                        "           ," & Me.ExDataGridView1.Rows(i).Cells("bc_money").Value & ") "


                    sqlcmd.CommandText = sql
                    sqlcmd.ExecuteNonQuery()

                    If Me.ComboBox2.SelectedIndex = 0 Then


                        sql = "update a set a.kp_money=kp_money + " & Me.ExDataGridView1.Rows(i).Cells("bc_money").Value & _
                            " from  t_stockbill_master a " & _
                                 "                where type=2 and billkindcode=20 and tempflag=0 and money>kp_money and billno= '" & Me.ExDataGridView1.Rows(i).Cells("billno").Value & "' "
                        sqlcmd.CommandText = sql
                        sqlcmd.ExecuteNonQuery()

                        sql = "update a set a.kp_money=kp_money + " & Me.ExDataGridView1.Rows(i).Cells("bc_money").Value & _
                                 " from  t_stockbill_master_history a  " & _
                                 "                where type=2 and billkindcode=20 and tempflag=0 and money>kp_money and billno='" & Me.ExDataGridView1.Rows(i).Cells("billno").Value & "' "
                        sqlcmd.CommandText = sql
                        sqlcmd.ExecuteNonQuery()
                    ElseIf Me.ComboBox2.SelectedIndex = 1 Then
                        sql = "update a set a.kp_money=kp_money + " & Me.ExDataGridView1.Rows(i).Cells("bc_money").Value & _
                        " from  t_outwork_master a " & _
                        " where billno= '" & Me.ExDataGridView1.Rows(i).Cells("billno").Value & "' "
                        sqlcmd.CommandText = sql
                        sqlcmd.ExecuteNonQuery()
                    Else
                        Throw New System.Exception("发票来源选择错误！")
                    End If


                Next

            ElseIf state = "mod" Then



                '如果已经不是新建就不能修改
                sql = "select count(*) from T_FPYF_master where state=0 and billno='" & billno & "'"
                sqlcmd.CommandText = sql
                If sqlcmd.ExecuteScalar = 0 Then
                    Throw New System.Exception("该单据不是新建状态，不可以修改！")
                End If

                sql = "update T_FPYF_master set FP_no='" & Me.TextBox2.Text.Trim & "' ,memo='" & Me.TextBox3.Text.Trim & "',KP_state=" & IIf(Me.RadioButton1.Checked, 1, 0) & ",KP_type='" & Me.ComboBox1.Text.Trim & "',FP_TYPE='" & Me.ComboBox2.SelectedIndex & "'" & _
                    " where billno='" & billno & "'"
                sqlcmd.CommandText = sql
                sqlcmd.ExecuteNonQuery()


                If Me.ComboBox2.SelectedIndex = 0 Then

                    sql = "update b set KP_money=KP_money- a.money " & _
                            "from T_FPYF_detail a inner join  " & _
                            "	t_stockbill_master b on a.RK_billno=b.billno " & _
                            "where a.billno='" & billno & "' "
                    sqlcmd.CommandText = sql
                    sqlcmd.ExecuteNonQuery()


                    sql = "update b set KP_money=KP_money- a.money " & _
                            "from T_FPYF_detail a inner join  " & _
                            "	t_stockbill_master_history b on a.RK_billno=b.billno " & _
                            "where a.billno='" & billno & "' "
                    sqlcmd.CommandText = sql
                    sqlcmd.ExecuteNonQuery()

                ElseIf Me.ComboBox2.SelectedIndex = 1 Then
                    sql = "update b set KP_money=KP_money- a.money " & _
                            "from T_FPYF_detail a inner join  " & _
                            "	t_outwork_master b on a.RK_billno=b.billno " & _
                            "where a.billno='" & billno & "' "

                    sqlcmd.CommandText = sql
                    sqlcmd.ExecuteNonQuery()
                Else
                    Throw New System.Exception("发票来源选择错误！")
                End If




                For i = 0 To Me.ExDataGridView1.Rows.Count - 1
                    If Me.ExDataGridView1.Rows(i).Cells("id").Value <> 0 Then
                        s = s & Me.ExDataGridView1.Rows(i).Cells("id").Value & ","



                        sql = "update T_FPYF_detail set money='" & Me.ExDataGridView1.Rows(i).Cells("bc_money").Value & "'" & _
                                                            "  where id=" & Me.ExDataGridView1.Rows(i).Cells("id").Value
                        sqlcmd.CommandText = sql
                        sqlcmd.ExecuteNonQuery()

                        If Me.ComboBox2.SelectedIndex = 0 Then


                            sql = "update t_stockbill_master set kp_money=Kp_money + " & Me.ExDataGridView1.Rows(i).Cells("bc_money").Value & " where  billno='" & Me.ExDataGridView1.Rows(i).Cells("billno").Value & "'"
                            sqlcmd.CommandText = sql
                            sqlcmd.ExecuteNonQuery()

                            sql = "update t_stockbill_master_history set kp_money=Kp_money + " & Me.ExDataGridView1.Rows(i).Cells("bc_money").Value & " where  billno='" & Me.ExDataGridView1.Rows(i).Cells("billno").Value & "'"
                            sqlcmd.CommandText = sql
                            sqlcmd.ExecuteNonQuery()
                        ElseIf Me.ComboBox2.SelectedIndex = 1 Then
                            sql = "update t_outwork_master set kp_money=Kp_money + " & Me.ExDataGridView1.Rows(i).Cells("bc_money").Value & " where  billno='" & Me.ExDataGridView1.Rows(i).Cells("billno").Value & "'"
                            sqlcmd.CommandText = sql
                            sqlcmd.ExecuteNonQuery()
                        End If

                    End If
                Next

                If s.Length <> 0 Then
                    s = s.Substring(0, s.Length - 1)
                    s = " and  id not in (" & s & ")"
                End If


                sql = "delete from T_FPYF_detail where  billno='" & billno & "' " & s
                sqlcmd.CommandText = sql
                sqlcmd.ExecuteNonQuery()


                For i = 0 To Me.ExDataGridView1.Rows.Count - 1
                    If Me.ExDataGridView1.Rows(i).Cells("id").Value = 0 Then
                        sql = "INSERT INTO [T_FPYF_detail] " & _
                       "           ([billno] " & _
                       "           ,[RK_billno] " & _
                       "           ,[money]) " & _
                       "     VALUES " & _
                       "           ('" & billno & "'" & _
                       "           ,'" & Me.ExDataGridView1.Rows(i).Cells("billno").Value & "' " & _
                       "           ," & Me.ExDataGridView1.Rows(i).Cells("bc_money").Value & ") "


                        sqlcmd.CommandText = sql
                        sqlcmd.ExecuteNonQuery()

                        If Me.ComboBox2.SelectedIndex = 0 Then


                            sql = "update a set a.kp_money=kp_money + " & Me.ExDataGridView1.Rows(i).Cells("bc_money").Value & _
                                " from  t_stockbill_master a " & _
                                     "                where type=2 and billkindcode=20 and tempflag=0 and money>kp_money and billno= '" & Me.ExDataGridView1.Rows(i).Cells("billno").Value & "' "
                            sqlcmd.CommandText = sql
                            sqlcmd.ExecuteNonQuery()

                            sql = "update a set a.kp_money=kp_money + " & Me.ExDataGridView1.Rows(i).Cells("bc_money").Value & _
                                     " from  t_stockbill_master_history a  " & _
                                     "                where type=2 and billkindcode=20 and tempflag=0 and money>kp_money and billno='" & Me.ExDataGridView1.Rows(i).Cells("billno").Value & "' "
                            sqlcmd.CommandText = sql
                            sqlcmd.ExecuteNonQuery()
                        ElseIf Me.ComboBox2.SelectedIndex = 1 Then
                            sql = "update a set a.kp_money=kp_money + " & Me.ExDataGridView1.Rows(i).Cells("bc_money").Value & _
                            " from  t_outwork_master a " & _
                            " where billno= '" & Me.ExDataGridView1.Rows(i).Cells("billno").Value & "' "
                            sqlcmd.CommandText = sql
                            sqlcmd.ExecuteNonQuery()

                        End If

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
            CType(Me.Owner, t_FPYF).ExDataGridView1.Rows(CType(Me.Owner, t_FPYF).ExDataGridView1.SelectedCells(0).RowIndex).Cells("fp_no").Value = Me.TextBox2.Text

            CType(Me.Owner, t_FPYF).ExDataGridView1.Rows(CType(Me.Owner, t_FPYF).ExDataGridView1.SelectedCells(0).RowIndex).Cells("memo").Value = Me.TextBox3.Text

            If Me.RadioButton1.Checked Then
                CType(Me.Owner, t_FPYF).ExDataGridView1.Rows(CType(Me.Owner, t_FPYF).ExDataGridView1.SelectedCells(0).RowIndex).Cells("kp_state1").Value = "已开票"
            Else
                CType(Me.Owner, t_FPYF).ExDataGridView1.Rows(CType(Me.Owner, t_FPYF).ExDataGridView1.SelectedCells(0).RowIndex).Cells("kp_state1").Value = "未开票"
            End If

            CType(Me.Owner, t_FPYF).ExDataGridView1.Rows(CType(Me.Owner, t_FPYF).ExDataGridView1.SelectedCells(0).RowIndex).Cells("kp_type").Value = Me.ComboBox1.Text.Trim
            CType(Me.Owner, t_FPYF).ExDataGridView1.Rows(CType(Me.Owner, t_FPYF).ExDataGridView1.SelectedCells(0).RowIndex).Cells("FP_type").Value = Me.ComboBox2.SelectedIndex

            If Me.ComboBox2.SelectedIndex = 0 Then
                CType(Me.Owner, t_FPYF).ExDataGridView1.Rows(CType(Me.Owner, t_FPYF).ExDataGridView1.SelectedCells(0).RowIndex).Cells("FP_type1").Value = "入库单"
            ElseIf Me.ComboBox2.SelectedIndex = 1 Then
                CType(Me.Owner, t_FPYF).ExDataGridView1.Rows(CType(Me.Owner, t_FPYF).ExDataGridView1.SelectedCells(0).RowIndex).Cells("FP_type1").Value = "加工费"
            End If

        End If
        Me.DialogResult = Windows.Forms.DialogResult.OK
    End Sub

    Private Sub T_FPYF_add_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim sql As String = ""
        Dim sqlcon As New SqlConnection
        Dim sqlcmd As New SqlCommand
        Dim sqlad As New SqlDataAdapter



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

        Me.ComboBox2.Items.Add("入库单")
        Me.ComboBox2.Items.Add("加工费单")



      



        If state = "new" Then


            Me.Text = "增加应付发票"

            Me.ComboBox2.SelectedIndex = 0
            Me.ComboBox2.Enabled = True

            sql = "select b.billno,a.money as kp_money,b.money,b.money-a.money as wkp_money,a.money as bc_money,a.id " & _
                    "from t_fpyF_detail a inner join  " & _
                    "	 t_stockbill_master b on a.rk_billno=b.billno " & _
                    "where a.id=-1 " & _
                    "union all  " & _
                    "select b.billno,a.money as kp_money,b.money,b.money-a.money as wkp_money,a.money as bc_money,a.id " & _
                    "from t_fpyF_detail a inner join  " & _
                    "	 t_stockbill_master_history b on a.rk_billno=b.billno " & _
                    "where a.id=-1 "




        ElseIf state = "mod" Then

            Me.Text = "修改应付发票"



            Me.TextBox1.Text = CType(Me.Owner, t_FPYF).ExDataGridView1.Rows(CType(Me.Owner, t_FPYF).ExDataGridView1.SelectedCells(0).RowIndex).Cells("billno").Value
            Me.TextBox2.Text = CType(Me.Owner, t_FPYF).ExDataGridView1.Rows(CType(Me.Owner, t_FPYF).ExDataGridView1.SelectedCells(0).RowIndex).Cells("FP_no").Value
            Me.TextBox3.Text = CType(Me.Owner, t_FPYF).ExDataGridView1.Rows(CType(Me.Owner, t_FPYF).ExDataGridView1.SelectedCells(0).RowIndex).Cells("memo").Value

            If CType(Me.Owner, t_FPYF).ExDataGridView1.Rows(CType(Me.Owner, t_FPYF).ExDataGridView1.SelectedCells(0).RowIndex).Cells("kp_state1").Value = "已开票" Then
                Me.RadioButton1.Checked = True
            Else
                Me.RadioButton2.Checked = True
            End If
            Me.ComboBox1.Text = CType(Me.Owner, t_FPYF).ExDataGridView1.Rows(CType(Me.Owner, t_FPYF).ExDataGridView1.SelectedCells(0).RowIndex).Cells("kp_type").Value


        
            Me.ComboBox2.SelectedIndex = CType(Me.Owner, t_FPYF).ExDataGridView1.Rows(CType(Me.Owner, t_FPYF).ExDataGridView1.SelectedCells(0).RowIndex).Cells("FP_type").Value
            Me.ComboBox2.Enabled = False

            If Me.ComboBox2.SelectedIndex = 0 Then



                sql = "select b.billno,b.kp_money,b.money,b.money-b.kp_money as wkp_money,a.money as bc_money,a.id " & _
                    "from t_fpyF_detail a inner join  " & _
                    "	 t_stockbill_master b on a.rk_billno=b.billno " & _
                    "where a.billno='" & billno & "' " & _
                    "union all  " & _
                    "select b.billno,b.kp_money,b.money,b.money-b.kp_money as wkp_money,a.money as bc_money,a.id " & _
                    "from t_fpyF_detail a inner join  " & _
                    "	 t_stockbill_master_history b on a.rk_billno=b.billno " & _
                    "where a.billno='" & billno & "' "
            ElseIf Me.ComboBox2.SelectedIndex = 1 Then
                sql = "select b.billno,b.kp_money,b.money,b.money-b.kp_money as wkp_money,a.money as bc_money,a.id " & _
                       "from t_fpyF_detail a inner join  " & _
                       "	 t_outwork_master b on a.rk_billno=b.billno " & _
                       "where a.billno='" & billno & "' "

            End If

        
        End If

        sqlcmd.CommandText = sql
        sqlad.SelectCommand = sqlcmd
        dt_p.Rows.Clear()
        sqlad.Fill(dt_p)

        Me.ExDataGridView1.SetDataSource(dt_p)
        Me.ExDataGridView1.Columns.Clear()

        Me.ExDataGridView1.AddColumn("billno", "入库单号", 160)

        Me.ExDataGridView1.AddColumn("money", "单据金额", , , , , "#0.00")
        Me.ExDataGridView1.AddColumn("KP_money", "已开票金额", , , , , "#0.00")
        Me.ExDataGridView1.AddColumn("WKP_money", "未开票金额", , , , , "#0.00")
        Me.ExDataGridView1.AddColumn("BC_money", "本次开票金额", , False, , , "#0.00")


        Me.ExDataGridView1.AddColumn("id", "id", , , , , , False)
    End Sub

    Private Sub RadioButton1_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RadioButton1.CheckedChanged
        Me.TextBox2.Enabled = True
    End Sub

    Private Sub RadioButton2_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RadioButton2.CheckedChanged
        Me.TextBox2.Text = ""
        Me.TextBox2.Enabled = False
    End Sub

  
End Class