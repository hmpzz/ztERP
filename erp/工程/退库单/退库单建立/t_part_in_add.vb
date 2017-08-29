Imports System.Data.SqlClient
Public Class t_part_in_add
    Friend dt_P As New DataTable
    Friend state As String
    Friend billno As String


    Private Sub t_part_in_add_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim sqlcon As New SqlConnection
        Dim sqlcmd As New SqlCommand
        Dim sqlad As New SqlDataAdapter


        Dim dt As New DataTable
        Dim sql As String
        Dim i As Int32

        sqlcon = getconn()
        sqlcmd.Connection = sqlcon


        sql = "select * from t_depot where stop=1 "
        sqlcmd.CommandText = sql
        sqlad.SelectCommand = sqlcmd

        dt.Clear()
        sqlad.Fill(dt)

        Me.ComboBox1.Items.Clear()
        Me.ComboBox2.Items.Clear()


        For i = 0 To dt.Rows.Count - 1
            Me.ComboBox1.Items.Add(dt.Rows(i)("depot_name"))
            Me.ComboBox2.Items.Add(dt.Rows(i)("depot_code"))
        Next


        If state = "new" Then
            Me.Text = "退库单建立"


            sql = " SELECT a.*,b.item_name,b.item_standard,b.unit   " & _
   "                        FROM t_part_in_detail a left join   " & _
   "                        	t_item b on a.item_code=b.item_code    " & _
"                             where a.billno='' order by a.id"


        ElseIf state = "mod" Then
            Me.Text = "退库单修改"


            sql = " SELECT a.*,b.item_name,b.item_standard,b.unit   " & _
   "                        FROM t_part_in_detail a left join   " & _
   "                        	t_item b on a.item_code=b.item_code    " & _
"                             where a.billno='" & billno & "' order by a.id"




            Me.TextBox1.Text = CType(Me.Owner, t_part_in).ExDataGridView1.Rows(CType(Me.Owner, t_part_in).ExDataGridView1.SelectedCells(0).RowIndex).Cells("billno").Value
            Me.TextBox2.Text = CType(Me.Owner, t_part_in).ExDataGridView1.Rows(CType(Me.Owner, t_part_in).ExDataGridView1.SelectedCells(0).RowIndex).Cells("project_no").Value
            Me.ComboBox1.Text = CType(Me.Owner, t_part_in).ExDataGridView1.Rows(CType(Me.Owner, t_part_in).ExDataGridView1.SelectedCells(0).RowIndex).Cells("depot_name").Value
            Me.TextBox4.Text = CType(Me.Owner, t_part_in).ExDataGridView1.Rows(CType(Me.Owner, t_part_in).ExDataGridView1.SelectedCells(0).RowIndex).Cells("ll_man").Value
            Me.ComboBox1.Enabled = False
        End If

        sqlcmd.CommandText = sql

        sqlad.SelectCommand = sqlcmd
        dt.Rows.Clear()
        sqlad.Fill(dt)

        dt_P = dt

        Me.ExDataGridView1.SetDataSource(dt)
        Me.ExDataGridView1.Columns.Clear()

        Me.ExDataGridView1.AddColumn("item_code", "物料编码")
        Me.ExDataGridView1.AddColumn("item_name", "物料名称")
        Me.ExDataGridView1.AddColumn("item_standard", "规格")
        Me.ExDataGridView1.AddColumn("unit", "单位")

        Me.ExDataGridView1.AddColumn("amount", "数量", , False, , DataGridViewContentAlignment.MiddleRight, "#0.000")


        Me.ExDataGridView1.AddColumn("item_code", "item_code", , , , , , False)


        Me.ExDataGridView1.AddColumn("id", "id", , , , , , False)

        Me.TextBox3.Focus()

    End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        Dim purchase_add_project As New purchase_add_project
        purchase_add_project.Owner = Me
        Me.Tag = ""
        If purchase_add_project.ShowDialog() = Windows.Forms.DialogResult.OK Then
            Me.TextBox2.Text = Me.Tag
            Me.Tag = ""
        End If
    End Sub

    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button2.Click
        Me.TextBox2.Text = ""
    End Sub

    Private Sub 添加NEWToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles 添加NEWToolStripMenuItem.Click
        Dim t_part_in_add_detail As New t_part_in_add_detail

        If Me.ComboBox1.SelectedIndex < 0 Then
            MsgBox("请选择仓库！", MsgBoxStyle.OkOnly, "提示")
            Exit Sub
        End If


        t_part_in_add_detail.Owner = Me
        t_part_in_add_detail.depot_code = Me.ComboBox2.Text
        t_part_in_add_detail.ShowDialog()

        If Me.ExDataGridView1.Rows.Count > 0 Then
            Me.ComboBox1.Enabled = False
        Else
            Me.ComboBox1.Enabled = True
        End If
    End Sub

    Private Sub ComboBox1_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ComboBox1.SelectedIndexChanged
        Me.ComboBox2.SelectedIndex = Me.ComboBox1.SelectedIndex
    End Sub

    Private Sub ToolStripButton_save_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripButton_save.Click
        Dim sqlcon As New SqlConnection
        Dim sqlcmd As New SqlCommand
        Dim sqlad As New SqlDataAdapter
        Dim mytrans As SqlTransaction
        Dim s As String = ""

        Dim dt As New DataTable
        Dim sql As String
        Dim i As Int32
        Dim b As Boolean


        Me.TextBox3.Focus()

        If Me.ExDataGridView1.Rows.Count = 0 Then
            MsgBox("没有数据无法保存！", MsgBoxStyle.OkOnly, "提示")
            Exit Sub
        End If

        If Me.ComboBox1.SelectedIndex < 0 Then
            MsgBox("请选择退库仓库！", MsgBoxStyle.OkOnly, "提示")
            Exit Sub
        End If

        If Me.TextBox2.Text.Trim.Length = 0 Then
            MsgBox("请选择工程号！", MsgBoxStyle.OkOnly, "提示")
            Exit Sub
        End If

        b = True
        For i = 0 To Me.ExDataGridView1.Rows.Count - 1
            If Me.ExDataGridView1.Rows(i).Cells("amount").Value <= 0 Then
                b = False
                Exit For
            End If
        Next


        If b = False Then
            MsgBox("数据中有数量小于或者等于0的数据，请查正后再保存！", MsgBoxStyle.OkOnly, "提示")
            Exit Sub
        End If



        sqlcon = getconn()
        sqlcmd.Connection = sqlcon
        mytrans = sqlcon.BeginTransaction

        sqlcmd.Transaction = mytrans



        Try
            If state = "new" Then

                sql = "update t_dj_num set num=num+1 where  tt='TK-" & Me.ComboBox2.Text.Trim & "'"
                sqlcmd.CommandText = sql
                sqlcmd.ExecuteNonQuery()

                sql = "select * from t_dj_num where tt='TK-" & Me.ComboBox2.Text.Trim & "'"
                sqlcmd.CommandText = sql
                sqlad.SelectCommand = sqlcmd
                dt.Rows.Clear()
                sqlad.Fill(dt)

                billno = dt.Rows(0)("TT") & "-" & GetAccountterm() & "-" & Format(dt.Rows(0)("num"), dt.Rows(0)("format"))


                sql = "INSERT INTO [T_part_in_master] " & _
                        "           ([billno] " & _
                        "           ,[project_no] " & _
                        "           ,[input_man] " & _
                        "           ,[input_date] " & _
                        "           ,[state] " & _
                        "           ,[memo] " & _
                        "           ,ll_man " & _
                        "           ,[depot_code]) " & _
                        "     VALUES " & _
                        "           ('" & billno & "' " & _
                        "           ,'" & Me.TextBox2.Text & "'" & _
                        "           ,'" & rs_name & "' " & _
                        "           ,getdate() " & _
                        "           ,0 " & _
                        "           ,'" & Me.TextBox3.Text.Trim & "' " & _
                        "           ,'" & Me.TextBox4.Text.Trim & "' " & _
                        "           ,'" & Me.ComboBox2.Text & "') "



                sqlcmd.CommandText = sql
                sqlcmd.ExecuteNonQuery()

                For i = 0 To Me.ExDataGridView1.Rows.Count - 1
                    sql = "INSERT INTO [T_part_in_detail] " & _
                        "           ([billno] " & _
                        "           ,[item_code] " & _
                        "           ,[amount] " & _
                        "           ,[OK_amount]) " & _
                        "     VALUES " & _
                        "           ('" & billno & "' " & _
                        "           ,'" & Me.ExDataGridView1.Rows(i).Cells("item_code").Value & "' " & _
                        "           ," & Me.ExDataGridView1.Rows(i).Cells("amount").Value & " " & _
                        "           ,0) "

                    sqlcmd.CommandText = sql
                    sqlcmd.ExecuteNonQuery()

                Next

            ElseIf state = "mod" Then

                '如果已经不是新建就不能修改
                sql = "select count(*) from T_part_in_master where state=0 and billno='" & billno & "'"
                sqlcmd.CommandText = sql
                If sqlcmd.ExecuteScalar = 0 Then
                    Throw New System.Exception("该单据不是新建状态，不可以修改！")
                End If

                sql = "UPDATE [T_part_in_master] " & _
                    "   SET [project_no] ='" & Me.TextBox2.Text & "' " & _
                    "      ,[depot_code] = '" & Me.ComboBox2.Text & "' " & _
                    "      ,[memo] = '" & Me.TextBox3.Text.Trim & "' " & _
                    "      ,[ll_man] = '" & Me.TextBox4.Text.Trim & "' " & _
                    " WHERE billno='" & billno & "' "

                sqlcmd.CommandText = sql
                sqlcmd.ExecuteNonQuery()





                s = ""
                For i = 0 To Me.ExDataGridView1.Rows.Count - 1
                    If Me.ExDataGridView1.Rows(i).Cells("id").Value <> 0 Then
                        s = s & Me.ExDataGridView1.Rows(i).Cells("id").Value & ","

                        sql = "UPDATE [T_part_in_detail] " & _
                            "   SET [item_code] = '" & Me.ExDataGridView1.Rows(i).Cells("item_code").Value & "' " & _
                            "      ,[amount] = " & Me.ExDataGridView1.Rows(i).Cells("amount").Value & " " & _
                            " WHERE   billno='" & billno & "' and id=" & Me.ExDataGridView1.Rows(i).Cells("id").Value

                        sqlcmd.CommandText = sql
                        sqlcmd.ExecuteNonQuery()


                    End If
                Next

                If s.Length <> 0 Then
                    s = s.Substring(0, s.Length - 1)
                    s = " and  id not in (" & s & ")"
                End If


                sql = "delete from T_part_in_detail where  billno='" & billno & "' " & s
                sqlcmd.CommandText = sql
                sqlcmd.ExecuteNonQuery()


                For i = 0 To Me.ExDataGridView1.Rows.Count - 1
                    If Me.ExDataGridView1.Rows(i).Cells("id").Value = 0 Then
                        sql = "INSERT INTO [T_part_in_detail] " & _
                      "           ([billno] " & _
                      "           ,[item_code] " & _
                      "           ,[amount] " & _
                      "           ,[OK_amount]) " & _
                      "     VALUES " & _
                      "           ('" & billno & "' " & _
                      "           ,'" & Me.ExDataGridView1.Rows(i).Cells("item_code").Value & "' " & _
                      "           ," & Me.ExDataGridView1.Rows(i).Cells("amount").Value & " " & _
                      "           ,0) "

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
            CType(Me.Owner, t_part_in).Refresh_ExDataGridView_master()
        ElseIf state = "mod" Then

            CType(Me.Owner, t_part_in).Refresh_ExDataGridView_master()
        End If


        Me.DialogResult = Windows.Forms.DialogResult.OK
    End Sub

    Private Sub ToolStripButton_exit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripButton_exit.Click
        Me.DialogResult = Windows.Forms.DialogResult.Cancel
    End Sub

    Private Sub 删除记录ToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles 删除记录ToolStripMenuItem.Click

        If Me.ExDataGridView1.SelectedCells.Count = 0 Then
            MsgBox("请选择要删除的行！", MsgBoxStyle.OkOnly, "提示")
            Exit Sub
        End If

        If MsgBox("确实要物料编号为" & Me.ExDataGridView1.Rows(Me.ExDataGridView1.SelectedCells(0).RowIndex).Cells("item_code").Value & "的行？", MsgBoxStyle.OkOnly, "提示") = MsgBoxResult.No Then
            Exit Sub
        End If
        Me.ExDataGridView1.Rows.RemoveAt(Me.ExDataGridView1.SelectedCells(0).RowIndex)
        Me.dt_P.AcceptChanges()


        If Me.ExDataGridView1.Rows.Count > 0 Then
            Me.ComboBox1.Enabled = False
        Else
            Me.ComboBox1.Enabled = True
        End If
    End Sub
End Class