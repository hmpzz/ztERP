Imports System.Data.SqlClient
Public Class t_outwork_add
    Friend state As String
    Friend dt_p As New DataTable
    Friend billno As String

   

    Private Sub ToolStripButton_exit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripButton_exit.Click
        Me.Close()
    End Sub

    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button2.Click
        Me.TextBox2.Text = ""
    End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        Dim t_outlay_add_project As New t_outlay_add_project
        t_outlay_add_project.Owner = Me
        t_outlay_add_project.ShowDialog()
        Me.TextBox2.Text = Me.Tag
        Me.Tag = ""
    End Sub

    Private Sub t_outwork_add_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim sqlcon As New SqlConnection
        Dim sqlcmd As New SqlCommand
        Dim sqlad As New SqlDataAdapter

        Dim dt As New DataTable
        Dim sql As String
        Dim i As Int32

        sqlcon = getconn()
        sqlcmd.Connection = sqlcon



        Me.ComboBox1.Items.Clear()
        Me.ComboBox2.Items.Clear()
        sql = "select * from t_supplier where stop=1 order by supplier_name"

        sqlcmd.CommandText = sql
        sqlad.SelectCommand = sqlcmd
        dt.Rows.Clear()
        sqlad.Fill(dt)

        For i = 0 To dt.Rows.Count - 1
            Me.ComboBox1.Items.Add(dt.Rows(i)("supplier_name"))
            Me.ComboBox2.Items.Add(dt.Rows(i)("supplier_no"))
        Next




        If state = "new" Then


            Me.Text = "加工费单建立"


            sql = "SELECT    a.*  " & _
                    "FROM         t_outwork_detail a " & _
                    "where a.id=-1"




        ElseIf state = "mod" Then

            Me.Text = "加工费单修改"



            Me.TextBox1.Text = CType(Me.Owner, t_outwork).ExDataGridView1.Rows(CType(Me.Owner, t_outwork).ExDataGridView1.SelectedCells(0).RowIndex).Cells("billno").Value
            Me.TextBox2.Text = CType(Me.Owner, t_outwork).ExDataGridView1.Rows(CType(Me.Owner, t_outwork).ExDataGridView1.SelectedCells(0).RowIndex).Cells("project_no").Value
            Me.ComboBox1.Text = CType(Me.Owner, t_outwork).ExDataGridView1.Rows(CType(Me.Owner, t_outwork).ExDataGridView1.SelectedCells(0).RowIndex).Cells("supplier_name").Value




            sql = "SELECT    a.*  " & _
                    "FROM         t_outwork_detail a " & _
                    "where a.billno='" & Me.TextBox1.Text.Trim & "'"



        End If

        sqlcmd.CommandText = sql
        sqlad.SelectCommand = sqlcmd
        dt_p.Rows.Clear()
        sqlad.Fill(dt_p)


        Me.ExDataGridView1.SetDataSource(dt_p)
        Me.ExDataGridView1.Columns.Clear()


        Me.ExDataGridView1.AddColumn("name", "名称")
        Me.ExDataGridView1.AddColumn("memo", "说明")

        Me.ExDataGridView1.AddColumn("amount", "数量", , , , DataGridViewContentAlignment.MiddleRight, "#0.00")
        Me.ExDataGridView1.AddColumn("price", "单价", , , , DataGridViewContentAlignment.MiddleRight, "#0.0000")
        Me.ExDataGridView1.AddColumn("money", "金额", , , , DataGridViewContentAlignment.MiddleRight, "#0.00")


        Me.ExDataGridView1.AddColumn("id", "id", , , , , , False)
        Me.TextBox3.Focus()
    End Sub

    Private Sub ComboBox1_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ComboBox1.SelectedIndexChanged
        Me.ComboBox2.SelectedIndex = Me.ComboBox1.SelectedIndex
    End Sub

    Private Sub 添加记录ToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles 添加记录ToolStripMenuItem.Click
        Dim t_outwork_detail_add As New t_outwork_detail_add

        Dim sqlcon As New SqlConnection
        Dim sqlcmd As New SqlCommand
        Dim sqlad As New SqlDataAdapter

        Dim dt As New DataTable

        Dim s As String = ""

        t_outwork_detail_add.state = "new"


        t_outwork_detail_add.Owner = Me


        t_outwork_detail_add.ShowDialog()
        If t_outwork_detail_add.DialogResult = Windows.Forms.DialogResult.Cancel Then
            t_outwork_detail_add.Dispose()
            Exit Sub
        End If


    End Sub

    Private Sub 删除记录ToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles 删除记录ToolStripMenuItem.Click
        If Me.ExDataGridView1.SelectedCells.Count = 0 Then
            MsgBox("请选择要删除的行！", MsgBoxStyle.OkOnly, "提示")
            Exit Sub
        End If

        If MsgBox("确实要删除当前选择的行？", MsgBoxStyle.YesNo, "提示") = MsgBoxResult.No Then
            Exit Sub
        End If
        Me.ExDataGridView1.Rows.RemoveAt(Me.ExDataGridView1.SelectedCells(0).RowIndex)
        Me.dt_p.AcceptChanges()

    End Sub

    Private Sub 修改记录ToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles 修改记录ToolStripMenuItem.Click
        Dim t_outwork_detail_add As New t_outwork_detail_add

        If Me.ExDataGridView1.SelectedCells.Count = 0 Then
            MsgBox("请选择要修改的行！", MsgBoxStyle.OkOnly, "提示")
            Exit Sub
        End If



        t_outwork_detail_add.TextBox1.Text = Me.ExDataGridView1.Rows(Me.ExDataGridView1.SelectedCells(0).RowIndex).Cells("memo").Value
        t_outwork_detail_add.TextBox2.Text = Me.ExDataGridView1.Rows(Me.ExDataGridView1.SelectedCells(0).RowIndex).Cells("money").Value
        t_outwork_detail_add.TextBox3.Text = Me.ExDataGridView1.Rows(Me.ExDataGridView1.SelectedCells(0).RowIndex).Cells("name").Value
        t_outwork_detail_add.TextBox4.Text = Me.ExDataGridView1.Rows(Me.ExDataGridView1.SelectedCells(0).RowIndex).Cells("amount").Value
        t_outwork_detail_add.TextBox5.Text = Me.ExDataGridView1.Rows(Me.ExDataGridView1.SelectedCells(0).RowIndex).Cells("price").Value
        t_outwork_detail_add.state = "mod"
        t_outwork_detail_add.Owner = Me
        t_outwork_detail_add.ShowDialog()
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
        Dim billno As String = ""

        Me.TextBox3.Focus()

        If Me.ExDataGridView1.Rows.Count = 0 Then
            MsgBox("没有加工费单没有内容，不能保存！", MsgBoxStyle.OkOnly, "提示")
            Exit Sub
        End If



        If Me.ComboBox1.SelectedIndex < 0 Then
            MsgBox("供应商选择不正确，请检查后保存！", MsgBoxStyle.OkOnly, "提示")
            Exit Sub
        End If





        sqlcon = getconn()
        sqlcmd.Connection = sqlcon
        mytrans = sqlcon.BeginTransaction

        sqlcmd.Transaction = mytrans



        Try
            If state = "new" Then


                sql = "update t_dj_num set num=num+1 where  tt='JGF'"
                sqlcmd.CommandText = sql
                sqlcmd.ExecuteNonQuery()

                sql = "select * from t_dj_num where tt='JGF'"
                sqlcmd.CommandText = sql
                sqlad.SelectCommand = sqlcmd
                dt.Rows.Clear()
                sqlad.Fill(dt)

                billno = dt.Rows(0)("TT") & "-" & GetAccountterm() & "-" & Format(dt.Rows(0)("num"), dt.Rows(0)("format"))



                sql = "INSERT INTO t_outwork_master ( billno,input_date,input_man,project_no,supplier_no) values ( " & _
                "'" & billno & "', getdate(), '" & rs_name & "',   '" & Me.TextBox2.Text.Trim & "', '" & Me.ComboBox2.Text.Trim & "') "

                sqlcmd.CommandText = sql
                sqlcmd.ExecuteNonQuery()

                For i = 0 To Me.ExDataGridView1.Rows.Count - 1
                    sql = "INSERT INTO t_outwork_detail ( billno,money,memo,name,amount,price) values " & _
                    "(  '" & billno & "','" & Me.ExDataGridView1.Rows(i).Cells("money").Value & "', '" & Me.ExDataGridView1.Rows(i).Cells("memo").Value & "', '" & Me.ExDataGridView1.Rows(i).Cells("name").Value & "', '" & Me.ExDataGridView1.Rows(i).Cells("amount").Value & "', '" & Me.ExDataGridView1.Rows(i).Cells("price").Value & "') "
                    sqlcmd.CommandText = sql
                    sqlcmd.ExecuteNonQuery()


                Next

                sql = "update a set a.money=b.summoney from t_outwork_master a inner join (select billno,sum(money) as summoney from t_outwork_detail where billno='" & billno & "' group by billno) b on a.billno=b.billno where a.billno='" & billno & "'"
                sqlcmd.CommandText = sql
                sqlcmd.ExecuteNonQuery()


            ElseIf state = "mod" Then

                billno = Me.TextBox1.Text.Trim

                '如果已经不是新建就不能修改
                sql = "select count(*) from t_outwork_master where state=0 and billno='" & billno & "'"
                sqlcmd.CommandText = sql
                If sqlcmd.ExecuteScalar = 0 Then
                    Throw New System.Exception("该单据不是新建状态，不可以修改！")
                End If

                sql = "UPDATE t_outwork_master " & _
                  "   SET  [project_no] = '" & Me.TextBox2.Text.Trim & "' " & _
                "      ,[supplier_no] = '" & Me.ComboBox2.Text.Trim & "' " & _
            " WHERE billno='" & billno & "'"

                sqlcmd.CommandText = sql
                sqlcmd.ExecuteNonQuery()



                For i = 0 To Me.ExDataGridView1.Rows.Count - 1
                    If Me.ExDataGridView1.Rows(i).Cells("id").Value <> 0 Then
                        s = s & Me.ExDataGridView1.Rows(i).Cells("id").Value & ","

                        sql = "UPDATE [t_outwork_detail] " & _
                                "   SET [memo] = '" & Me.ExDataGridView1.Rows(i).Cells("memo").Value & "' " & _
                                "      ,[money] =  '" & Me.ExDataGridView1.Rows(i).Cells("money").Value & "' " & _
                                "      ,[name] =  '" & Me.ExDataGridView1.Rows(i).Cells("name").Value & "' " & _
                                "      ,[price] =  '" & Me.ExDataGridView1.Rows(i).Cells("price").Value & "' " & _
                                "      ,[amount] =  '" & Me.ExDataGridView1.Rows(i).Cells("amount").Value & "' " & _
                                "  where id=" & Me.ExDataGridView1.Rows(i).Cells("id").Value
                        sqlcmd.CommandText = sql
                        sqlcmd.ExecuteNonQuery()

                        't_part_in(Me.ExDataGridView1.Rows(i).Cells("item_id").Value, bh_no, Me.ComboBox8.Text.Trim, Me.ExDataGridView1.Rows(i).Cells("packing_id").Value, Me.ExDataGridView1.Rows(i).Cells("standards_id").Value, sqlcmd)
                    End If
                Next

                If s.Length <> 0 Then
                    s = s.Substring(0, s.Length - 1)
                    s = " and  id not in (" & s & ")"
                End If


                sql = "delete from t_outwork_detail where  billno='" & billno & "' " & s
                sqlcmd.CommandText = sql
                sqlcmd.ExecuteNonQuery()


                For i = 0 To Me.ExDataGridView1.Rows.Count - 1
                    If Me.ExDataGridView1.Rows(i).Cells("id").Value = 0 Then
                        sql = "INSERT INTO t_outwork_detail ( billno,money,memo,name,amount,price) values " & _
                      "(  '" & billno & "','" & Me.ExDataGridView1.Rows(i).Cells("money").Value & "', '" & Me.ExDataGridView1.Rows(i).Cells("memo").Value & "', '" & Me.ExDataGridView1.Rows(i).Cells("name").Value & "', '" & Me.ExDataGridView1.Rows(i).Cells("amount").Value & "', '" & Me.ExDataGridView1.Rows(i).Cells("price").Value & "') "
                        sqlcmd.CommandText = sql
                        sqlcmd.ExecuteNonQuery()
                    End If
                Next

                sql = "update a set a.money=b.summoney from t_outwork_master a inner join (select billno,sum(money) as summoney from t_outwork_detail where billno='" & billno & "' group by billno) b on a.billno=b.billno where a.billno='" & billno & "'"
                sqlcmd.CommandText = sql
                sqlcmd.ExecuteNonQuery()

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

            'CType(Me.Owner, t_outwork).ExDataGridView1.Rows(CType(Me.Owner, t_outwork).ExDataGridView1.SelectedCells(0).RowIndex).Cells("billno").Value = Me.TextBox1.Text
            CType(Me.Owner, t_outwork).ExDataGridView1.Rows(CType(Me.Owner, t_outwork).ExDataGridView1.SelectedCells(0).RowIndex).Cells("project_no").Value = Me.TextBox2.Text
            'CType(Me.Owner, t_outwork).ExDataGridView1.Rows(CType(Me.Owner, t_outwork).ExDataGridView1.SelectedCells(0).RowIndex).Cells("memo").Value = Me.TextBox3.Text
            CType(Me.Owner, t_outwork).ExDataGridView1.Rows(CType(Me.Owner, t_outwork).ExDataGridView1.SelectedCells(0).RowIndex).Cells("supplier_name").Value = Me.ComboBox1.Text


        End If
        Me.DialogResult = Windows.Forms.DialogResult.OK
    End Sub
End Class