Imports System.Data.SqlClient

Public Class purchase_add
    Friend state As String
    Friend dt_p As New DataTable
    Friend billno As String

    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button2.Click
        Me.TextBox2.Text = ""
    End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        Dim purchase_add_project As New purchase_add_project
        purchase_add_project.Owner = Me
        purchase_add_project.ShowDialog()
    End Sub

    Private Sub purchase_add_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim sqlcon As New SqlConnection
        Dim sqlcmd As New SqlCommand
        Dim sqlad As New SqlDataAdapter

        Dim dt As New DataTable
        Dim sql As String
        Dim i As int32

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

        'sql = "select a.* from t_depot a inner join t_user_depot b on a.depot_code=b.depot_code where a.stop=1 and b.login_id='" & login_id & "' order by a.depot_code"
        sql = "select * from t_depot where stop=1 "
        sqlcmd.CommandText = sql
        sqlad.SelectCommand = sqlcmd

        dt.Clear()
        sqlad.Fill(dt)

        For i = 0 To dt.Rows.Count - 1
            Me.ComboBox3.Items.Add(dt.Rows(i)("depot_name"))
            Me.ComboBox4.Items.Add(dt.Rows(i)("depot_code"))
        Next

        

        If state = "new" Then


            Me.Text = "供货合同建立"


            sql = "SELECT    a.*,b.item_name,b.item_standard,b.unit,b.id as item_id " & _
                    "FROM         purchase_detail AS a LEFT OUTER JOIN " & _
                    "                      t_item AS b ON a.item_code = b.item_code " & _
                    "where a.id=-1"




        ElseIf state = "mod" Then

            Me.Text = "供货合同修改"



            Me.TextBox1.Text = CType(Me.Owner, purchase).ExDataGridView1.Rows(CType(Me.Owner, purchase).ExDataGridView1.SelectedCells(0).RowIndex).Cells("billno").Value
            'Me.TextBox2.Text = CType(Me.Owner, purchase).ExDataGridView1.Rows(CType(Me.Owner, purchase).ExDataGridView1.SelectedCells(0).RowIndex).Cells("project_no").Value
            Me.TextBox3.Text = CType(Me.Owner, purchase).ExDataGridView1.Rows(CType(Me.Owner, purchase).ExDataGridView1.SelectedCells(0).RowIndex).Cells("memo").Value
            Me.ComboBox1.Text = CType(Me.Owner, purchase).ExDataGridView1.Rows(CType(Me.Owner, purchase).ExDataGridView1.SelectedCells(0).RowIndex).Cells("supplier_name").Value
            Me.ComboBox3.Text = CType(Me.Owner, purchase).ExDataGridView1.Rows(CType(Me.Owner, purchase).ExDataGridView1.SelectedCells(0).RowIndex).Cells("depot_name").Value



            sql = "SELECT    a.*,b.item_name,b.item_standard,b.unit,b.id as item_id " & _
                    "FROM         purchase_detail AS a LEFT OUTER JOIN " & _
                    "                      t_item AS b ON a.item_code = b.item_code " & _
                    "where a.billno='" & Me.TextBox1.Text.Trim & "'"



        End If

        sqlcmd.CommandText = sql
        sqlad.SelectCommand = sqlcmd
        dt_p.Rows.Clear()
        sqlad.Fill(dt_p)


        Me.ExDataGridView1.SetDataSource(dt_p)
        Me.ExDataGridView1.Columns.Clear()


        Me.ExDataGridView1.AddColumn("item_code", "物料代码")
        Me.ExDataGridView1.AddColumn("item_name", "物料名称")
        Me.ExDataGridView1.AddColumn("item_standard", "规格")
        Me.ExDataGridView1.AddColumn("unit", "单位")
        Me.ExDataGridView1.AddColumn("amount", "数量", , , , DataGridViewContentAlignment.MiddleRight, "#0.00")
        Me.ExDataGridView1.AddColumn("price", "单价", , , , DataGridViewContentAlignment.MiddleRight, "#0.0000")
        Me.ExDataGridView1.AddColumn("money", "金额", , , , DataGridViewContentAlignment.MiddleRight, "#0.00")

        Me.ExDataGridView1.AddColumn("item_id", "item_id", , , , , , False)
        Me.ExDataGridView1.AddColumn("id", "id", , , , , , False)
        Me.TextBox3.Focus()
    End Sub

    Private Sub ComboBox1_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ComboBox1.SelectedIndexChanged
        Me.ComboBox2.SelectedIndex = Me.ComboBox1.SelectedIndex
    End Sub

    Private Sub 添加记录ToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles 添加记录ToolStripMenuItem.Click
        Dim purchase_warehouse As New purchase_warehouse

        Dim sqlcon As New SqlConnection
        Dim sqlcmd As New SqlCommand
        Dim sqlad As New SqlDataAdapter

        Dim dt As New DataTable
        Dim sql As String
        Dim i As int32
        Dim s As String = ""

        For i = 0 To Me.ExDataGridView1.Rows.Count - 1
            s = s & Me.ExDataGridView1.Rows(i).Cells("item_id").Value & ","
        Next

        If s.Length > 0 Then
            s = s.Substring(0, s.Length - 1)
        End If

        purchase_warehouse.id = s
        purchase_warehouse.depot_code = Me.ComboBox4.Text
        purchase_warehouse.Owner = Me


        purchase_warehouse.ShowDialog()
        If purchase_warehouse.DialogResult = Windows.Forms.DialogResult.Cancel Then
            purchase_warehouse.Dispose()
            Exit Sub
        End If


        If Me.Tag = "" Then
            Exit Sub
        End If

        sqlcon = getconn()
        sqlcmd.Connection = sqlcon

        sql = "select * from " & _
                "t_item  where id in (" & Me.Tag & ")"
        sqlcmd.CommandText = sql

        sqlad.SelectCommand = sqlcmd
        sqlad.Fill(dt)

        If Me.ExDataGridView1.Columns.Count = 0 Then
            dt_p = dt

            Me.ExDataGridView1.SetDataSource(dt_p)
            Me.ExDataGridView1.Columns.Clear()


            Me.ExDataGridView1.AddColumn("item_code", "物料代码")
            Me.ExDataGridView1.AddColumn("item_name", "物料名称")
            Me.ExDataGridView1.AddColumn("item_standard", "规格")
            Me.ExDataGridView1.AddColumn("unit", "单位")
            Me.ExDataGridView1.AddColumn("amount", "数量", , , , DataGridViewContentAlignment.MiddleRight, "#0.00")
            Me.ExDataGridView1.AddColumn("price", "单价", , , , DataGridViewContentAlignment.MiddleRight, "#0.0000")
            Me.ExDataGridView1.AddColumn("money", "金额", , , , DataGridViewContentAlignment.MiddleRight, "#0.00")

            Me.ExDataGridView1.AddColumn("item_id", "item_id", , , , , , False)
            Me.ExDataGridView1.AddColumn("id", "id", , , , , , False)
        Else
            For i = 0 To dt.Rows.Count - 1

                Dim DR As DataRow = Me.dt_p.NewRow

                DR("item_code") = dt.Rows(i)("item_code")

                DR("item_name") = dt.Rows(i)("item_name")
                DR("item_standard") = dt.Rows(i)("item_standard")
                DR("unit") = dt.Rows(i)("unit")
                DR("price") = 0
                DR("amount") = 0
                DR("money") = 0
                DR("OK_amount") = 0
                DR("item_id") = dt.Rows(i)("id")

                DR("id") = 0

                dt_p.Rows.Add(DR)


            Next
        End If


        purchase_warehouse.Dispose()
    End Sub

    Private Sub 删除记录ToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles 删除记录ToolStripMenuItem.Click
        If Me.ExDataGridView1.SelectedCells.Count = 0 Then
            MsgBox("请选择要删除的行！", MsgBoxStyle.OkOnly, "提示")
            Exit Sub
        End If

        If MsgBox("确实要删除物料编号为" & Me.ExDataGridView1.Rows(Me.ExDataGridView1.SelectedCells(0).RowIndex).Cells("item_code").Value & "的行？", MsgBoxStyle.OkOnly, "提示") = MsgBoxResult.No Then
            Exit Sub
        End If
        Me.ExDataGridView1.Rows.RemoveAt(Me.ExDataGridView1.SelectedCells(0).RowIndex)
        Me.dt_p.AcceptChanges()


    End Sub

    Private Sub 修改记录ToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles 修改记录ToolStripMenuItem.Click
        Dim purchase_detail_add As New purchase_detail_add

        If Me.ExDataGridView1.SelectedCells.Count = 0 Then
            MsgBox("请选择要修改的行！", MsgBoxStyle.YesNo, "提示")
            Exit Sub
        End If



        purchase_detail_add.TextBox1.Text = Me.ExDataGridView1.Rows(Me.ExDataGridView1.SelectedCells(0).RowIndex).Cells("amount").Value
        purchase_detail_add.TextBox2.Text = Me.ExDataGridView1.Rows(Me.ExDataGridView1.SelectedCells(0).RowIndex).Cells("price").Value
        purchase_detail_add.TextBox3.Text = Me.ExDataGridView1.Rows(Me.ExDataGridView1.SelectedCells(0).RowIndex).Cells("money").Value
        purchase_detail_add.Owner = Me
        purchase_detail_add.ShowDialog()
    End Sub

    Private Sub ToolStripButton_exit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripButton_exit.Click
        Me.Close()
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
        Dim billno As String = ""

        Me.TextBox3.Focus()

        If Me.ExDataGridView1.Rows.Count = 0 Then
            MsgBox("没有采购单内容，不能保存！", MsgBoxStyle.OkOnly, "提示")
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


                sql = "update t_dj_num set num=num+1 where  tt='CG-" & Me.ComboBox4.Text.Trim & "'"
                sqlcmd.CommandText = sql
                sqlcmd.ExecuteNonQuery()

                sql = "select * from t_dj_num where tt='CG-" & Me.ComboBox4.Text.Trim & "'"
                sqlcmd.CommandText = sql
                sqlad.SelectCommand = sqlcmd
                dt.Rows.Clear()
                sqlad.Fill(dt)

                billno = dt.Rows(0)("TT") & "-" & GetAccountterm() & "-" & Format(dt.Rows(0)("num"), dt.Rows(0)("format"))



                sql = "INSERT INTO purchase_master ( billno,input_date,input_man,memo,project_no,supplier_no,depot_code) values ( " & _
                "'" & billno & "', getdate(), '" & rs_name & "', '" & Me.TextBox3.Text.Trim & "',  '" & Me.TextBox2.Text.Trim & "', '" & Me.ComboBox2.Text.Trim & "','" & Me.ComboBox4.Text.Trim & "') "

                sqlcmd.CommandText = sql
                sqlcmd.ExecuteNonQuery()

                For i = 0 To Me.ExDataGridView1.Rows.Count - 1
                    sql = "INSERT INTO purchase_detail ( amount,billno,item_code,money,OK_amount,price) values " & _
                    "( '" & Me.ExDataGridView1.Rows(i).Cells("amount").Value & "', '" & billno & "', '" & Me.ExDataGridView1.Rows(i).Cells("item_code").Value & "'," & _
                    "'" & Me.ExDataGridView1.Rows(i).Cells("money").Value & "', '0', '" & Me.ExDataGridView1.Rows(i).Cells("price").Value & "') "
                    sqlcmd.CommandText = sql
                    sqlcmd.ExecuteNonQuery()


                Next



            ElseIf state = "mod" Then



                '如果已经不是新建就不能修改
                sql = "select count(*) from purchase_master where state=0 and billno='" & Me.TextBox1.Text.Trim & "'"
                sqlcmd.CommandText = sql
                If sqlcmd.ExecuteScalar = 0 Then
                    Throw New System.Exception("该单据不是新建状态，不可以修改！")
                End If

                sql = "UPDATE purchase_master " & _
                  "   SET  [project_no] = '" & Me.TextBox2.Text.Trim & "' " & _
                "      ,[supplier_no] = '" & Me.ComboBox2.Text.Trim & "' " & _
                "      ,[memo] =  '" & Me.TextBox3.Text.Trim & "' " & _
                "      ,[depot_code]='" & Me.ComboBox4.Text.Trim & "'" & _
            " WHERE billno='" & Me.TextBox1.Text.Trim & "'"

                sqlcmd.CommandText = sql
                sqlcmd.ExecuteNonQuery()



                For i = 0 To Me.ExDataGridView1.Rows.Count - 1
                    If Me.ExDataGridView1.Rows(i).Cells("id").Value <> 0 Then
                        s = s & Me.ExDataGridView1.Rows(i).Cells("id").Value & ","

                        sql = "UPDATE [purchase_detail] " & _
                                "   SET [item_code] = '" & Me.ExDataGridView1.Rows(i).Cells("item_code").Value & "' " & _
                                "      ,[price] =  '" & Me.ExDataGridView1.Rows(i).Cells("price").Value & "' " & _
                                "      ,[money] =  '" & Me.ExDataGridView1.Rows(i).Cells("money").Value & "' " & _
                                "      ,[amount] = '" & Me.ExDataGridView1.Rows(i).Cells("amount").Value & "' " & _
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


                sql = "delete from purchase_detail where  billno='" & Me.TextBox1.Text.Trim & "' " & s
                sqlcmd.CommandText = sql
                sqlcmd.ExecuteNonQuery()


                For i = 0 To Me.ExDataGridView1.Rows.Count - 1
                    If Me.ExDataGridView1.Rows(i).Cells("id").Value = 0 Then
                        sql = "INSERT INTO purchase_detail ( amount,billno,item_code,money,OK_amount,price) values " & _
                    "( '" & Me.ExDataGridView1.Rows(i).Cells("amount").Value & "', '" & Me.TextBox1.Text.Trim & "', '" & Me.ExDataGridView1.Rows(i).Cells("item_code").Value & "'," & _
                    "'" & Me.ExDataGridView1.Rows(i).Cells("money").Value & "', '0', '" & Me.ExDataGridView1.Rows(i).Cells("price").Value & "') "
                        sqlcmd.CommandText = sql
                        sqlcmd.ExecuteNonQuery()

                        't_part_in(Me.ExDataGridView1.Rows(i).Cells("item_id").Value, bh_no, Me.ComboBox8.Text.Trim, Me.ExDataGridView1.Rows(i).Cells("packing_id").Value, Me.ExDataGridView1.Rows(i).Cells("standards_id").Value, sqlcmd)
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

            CType(Me.Owner, purchase).ExDataGridView1.Rows(CType(Me.Owner, purchase).ExDataGridView1.SelectedCells(0).RowIndex).Cells("billno").Value = Me.TextBox1.Text
            'CType(Me.Owner, purchase).ExDataGridView1.Rows(CType(Me.Owner, purchase).ExDataGridView1.SelectedCells(0).RowIndex).Cells("project_no").Value = Me.TextBox2.Text
            CType(Me.Owner, purchase).ExDataGridView1.Rows(CType(Me.Owner, purchase).ExDataGridView1.SelectedCells(0).RowIndex).Cells("memo").Value = Me.TextBox3.Text
            CType(Me.Owner, purchase).ExDataGridView1.Rows(CType(Me.Owner, purchase).ExDataGridView1.SelectedCells(0).RowIndex).Cells("supplier_name").Value = Me.ComboBox1.Text


        End If
        Me.DialogResult = Windows.Forms.DialogResult.OK
    End Sub

    Private Sub ComboBox2_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ComboBox2.SelectedIndexChanged
        Me.ComboBox1.SelectedIndex = Me.ComboBox2.SelectedIndex
    End Sub


    Private Sub ComboBox3_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ComboBox3.SelectedIndexChanged
        Me.ComboBox4.SelectedIndex = Me.ComboBox3.SelectedIndex
    End Sub
End Class