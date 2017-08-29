Imports System.Data.SqlClient
Public Class CGSQ_add
    Friend state As String
    Friend dt_p As New DataTable
    Friend billno As String

    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button2.Click
        Me.TextBox2.Text = ""
    End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        Dim cgsq_add_project As New CGSQ_add_project
        cgsq_add_project.Owner = Me
        If cgsq_add_project.ShowDialog() = Windows.Forms.DialogResult.Cancel Then
            Exit Sub
        End If
        Me.TextBox2.Text = Me.Tag
        Me.Tag = ""
    End Sub

    Private Sub CGSQ_add_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim sqlcon As New SqlConnection
        Dim sqlcmd As New SqlCommand
        Dim sqlad As New SqlDataAdapter


        Dim sql As String = ""
        Dim i As int32 = 0

        sqlcon = getconn()
        sqlcmd.Connection = sqlcon



        'Me.ComboBox1.Items.Clear()
        'Me.ComboBox2.Items.Clear()
        'sql = "select * from t_supplier where stop=1 order by supplier_name"

        'sqlcmd.CommandText = sql
        'sqlad.SelectCommand = sqlcmd
        'dt_p.Rows.Clear()
        'sqlad.Fill(dt_p)

        'For i = 0 To dt_p.Rows.Count - 1
        '    Me.ComboBox1.Items.Add(dt_p.Rows(i)("supplier_name"))
        '    Me.ComboBox2.Items.Add(dt_p.Rows(i)("supplier_no"))
        'Next





        If state = "new" Then


            Me.Text = "�ɹ����뵥����"


            sql = "SELECT    a.*  " & _
                    "FROM        cgd_detail AS a  " & _
                    "where a.id=-1"




        ElseIf state = "mod" Then

            Me.Text = "�ɹ����뵥�޸�"



            Me.TextBox1.Text = CType(Me.Owner, CGSQ).ExDataGridView1.Rows(CType(Me.Owner, CGSQ).ExDataGridView1.SelectedCells(0).RowIndex).Cells("billno").Value
            Me.TextBox2.Text = CType(Me.Owner, CGSQ).ExDataGridView1.Rows(CType(Me.Owner, CGSQ).ExDataGridView1.SelectedCells(0).RowIndex).Cells("project_no").Value
            Me.TextBox3.Text = CType(Me.Owner, CGSQ).ExDataGridView1.Rows(CType(Me.Owner, CGSQ).ExDataGridView1.SelectedCells(0).RowIndex).Cells("memo").Value
            'Me.ComboBox1.Text = CType(Me.Owner, CGSQ).ExDataGridView1.Rows(CType(Me.Owner, CGSQ).ExDataGridView1.SelectedCells(0).RowIndex).Cells("supplier_name").Value




            sql = "SELECT    a.* " & _
                    "FROM         cgd_detail AS a " & _
                    "where a.billno='" & Me.TextBox1.Text.Trim & "'"



        End If

        sqlcmd.CommandText = sql
        sqlad.SelectCommand = sqlcmd
        dt_p.Rows.Clear()
        sqlad.Fill(dt_p)


        Me.ExDataGridView1.SetDataSource(dt_p)
        Me.ExDataGridView1.Columns.Clear()


        'Me.ExDataGridView1.AddColumn("item_code", "���ϴ���")
        Me.ExDataGridView1.AddColumn("name", "����")
        'Me.ExDataGridView1.AddColumn("item_standard", "���")
        'Me.ExDataGridView1.AddColumn("unit", "��λ")
        Me.ExDataGridView1.AddColumn("amount", "����", , , , DataGridViewContentAlignment.MiddleRight, "#0.00")
        'Me.ExDataGridView1.AddColumn("price", "����", , , , DataGridViewContentAlignment.MiddleRight, "#0.0000")
        'Me.ExDataGridView1.AddColumn("money", "���", , , , DataGridViewContentAlignment.MiddleRight, "#0.00")
        Me.ExDataGridView1.AddColumn("memo", "˵��")
        'Me.ExDataGridView1.AddColumn("item_id", "item_id", , , , , , False)
        Me.ExDataGridView1.AddColumn("id", "id", , , , , , False)
        Me.TextBox3.Focus()
    End Sub



    Private Sub ��Ӽ�¼ToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ��Ӽ�¼ToolStripMenuItem.Click
        Dim CGSQ_add_detail As New CGSQ_add_detail

        Dim sqlcon As New SqlConnection
        Dim sqlcmd As New SqlCommand
        Dim sqlad As New SqlDataAdapter

        Dim dt As New DataTable

        Dim s As String = ""

        CGSQ_add_detail.state = "new"


        CGSQ_add_detail.Owner = Me

        CGSQ_add_detail.TextBox2.Visible = False
        CGSQ_add_detail.TextBox5.Visible = False

        CGSQ_add_detail.Label2.Visible = False
        CGSQ_add_detail.Label5.Visible = False

        CGSQ_add_detail.ShowDialog()
        If CGSQ_add_detail.DialogResult = Windows.Forms.DialogResult.Cancel Then
            CGSQ_add_detail.Dispose()
            Exit Sub
        End If


        'If Me.ExDataGridView1.Columns.Count = 0 Then
        '    dt_p = dt

        '    Me.ExDataGridView1.SetDataSource(dt_p)
        '    Me.ExDataGridView1.Columns.Clear()


        '    Me.ExDataGridView1.AddColumn("item_code", "���ϴ���")
        '    Me.ExDataGridView1.AddColumn("item_name", "��������")
        '    Me.ExDataGridView1.AddColumn("item_standard", "���")
        '    Me.ExDataGridView1.AddColumn("unit", "��λ")
        '    Me.ExDataGridView1.AddColumn("amount", "����", , , , DataGridViewContentAlignment.MiddleRight, "#0.00")
        '    Me.ExDataGridView1.AddColumn("price", "����", , , , DataGridViewContentAlignment.MiddleRight, "#0.0000")
        '    Me.ExDataGridView1.AddColumn("money", "���", , , , DataGridViewContentAlignment.MiddleRight, "#0.00")

        '    Me.ExDataGridView1.AddColumn("item_id", "item_id", , , , , , False)
        '    Me.ExDataGridView1.AddColumn("id", "id", , , , , , False)
        'Else
        '    For i = 0 To dt.Rows.Count - 1

        '        Dim DR As DataRow = Me.dt_p.NewRow

        '        DR("item_code") = dt.Rows(i)("item_code")

        '        DR("item_name") = dt.Rows(i)("item_name")
        '        DR("item_standard") = dt.Rows(i)("item_standard")
        '        DR("unit") = dt.Rows(i)("unit")
        '        DR("price") = 0
        '        DR("amount") = 0
        '        DR("money") = 0

        '        DR("item_id") = dt.Rows(i)("id")

        '        DR("id") = 0

        '        dt_p.Rows.Add(DR)


        '    Next
        'End If


        'CGSQ_warehouse.Dispose()
    End Sub

    Private Sub ɾ����¼ToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ɾ����¼ToolStripMenuItem.Click
        If Me.ExDataGridView1.SelectedCells.Count = 0 Then
            MsgBox("��ѡ��Ҫɾ�����У�", MsgBoxStyle.OkOnly, "��ʾ")
            Exit Sub
        End If

        If MsgBox("ȷʵҪɾ����ǰѡ����У�", MsgBoxStyle.YesNo, "��ʾ") = MsgBoxResult.No Then
            Exit Sub
        End If
        Me.ExDataGridView1.Rows.RemoveAt(Me.ExDataGridView1.SelectedCells(0).RowIndex)
        Me.dt_p.AcceptChanges()
    End Sub

    Private Sub �޸ļ�¼ToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles �޸ļ�¼ToolStripMenuItem.Click
        Dim CGSQ_add_detail As New CGSQ_add_detail

        If Me.ExDataGridView1.SelectedCells.Count = 0 Then
            MsgBox("��ѡ��Ҫ�޸ĵ��У�", MsgBoxStyle.OkOnly, "��ʾ")
            Exit Sub
        End If

        CGSQ_add_detail.TextBox2.Visible = False
        CGSQ_add_detail.TextBox5.Visible = False

        CGSQ_add_detail.Label2.Visible = False
        CGSQ_add_detail.Label5.Visible = False


        CGSQ_add_detail.TextBox1.Text = Me.ExDataGridView1.Rows(Me.ExDataGridView1.SelectedCells(0).RowIndex).Cells("memo").Value
        CGSQ_add_detail.TextBox2.Text = 0
        CGSQ_add_detail.TextBox3.Text = Me.ExDataGridView1.Rows(Me.ExDataGridView1.SelectedCells(0).RowIndex).Cells("name").Value
        CGSQ_add_detail.TextBox4.Text = Me.ExDataGridView1.Rows(Me.ExDataGridView1.SelectedCells(0).RowIndex).Cells("amount").Value
        CGSQ_add_detail.TextBox5.Text = 0
        CGSQ_add_detail.state = "mod"
        CGSQ_add_detail.Owner = Me
        CGSQ_add_detail.ShowDialog()
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
            MsgBox("û�вɹ����뵥���ݣ����ܱ��棡", MsgBoxStyle.OkOnly, "��ʾ")
            Exit Sub
        End If



        'If Me.ComboBox1.SelectedIndex < 0 Then
        '    MsgBox("��Ӧ��ѡ����ȷ������󱣴棡", MsgBoxStyle.OkOnly, "��ʾ")
        '    Exit Sub
        'End If





        sqlcon = getconn()
        sqlcmd.Connection = sqlcon
        mytrans = sqlcon.BeginTransaction

        sqlcmd.Transaction = mytrans



        Try
            If state = "new" Then


                sql = "update t_dj_num set num=num+1 where  tt='CGSQ'"
                sqlcmd.CommandText = sql
                sqlcmd.ExecuteNonQuery()

                sql = "select * from t_dj_num where tt='CGSQ'"
                sqlcmd.CommandText = sql
                sqlad.SelectCommand = sqlcmd
                dt.Rows.Clear()
                sqlad.Fill(dt)

                billno = dt.Rows(0)("TT") & "-" & GetAccountterm() & "-" & Format(dt.Rows(0)("num"), dt.Rows(0)("format"))



                sql = "INSERT INTO CGD_master ( billno,input_date,input_man,memo,project_no) values ( " & _
                "'" & billno & "', getdate(), '" & rs_name & "', '" & Me.TextBox4.Text.Trim & "',  '" & Me.TextBox2.Text.Trim & "') "

                sqlcmd.CommandText = sql
                sqlcmd.ExecuteNonQuery()

                For i = 0 To Me.ExDataGridView1.Rows.Count - 1
                    sql = "INSERT INTO cgd_detail ( amount,billno,name,money,price,memo) values " & _
                    "( '" & Me.ExDataGridView1.Rows(i).Cells("amount").Value & "', '" & billno & "', '" & Me.ExDataGridView1.Rows(i).Cells("name").Value & "'," & _
                    "'0', '0', '" & Me.ExDataGridView1.Rows(i).Cells("memo").Value & "') "
                    sqlcmd.CommandText = sql
                    sqlcmd.ExecuteNonQuery()


                Next

                sql = "update a set a.money=b.money " & _
                        "from cgd_master a left join	 " & _
                        "	(select sum(price*amount) as money,billno from cgd_detail where billno='" & billno & "' group by billno) b on a.billno=b.billno  " & _
                        "where a.billno='" & billno & "' "
                sqlcmd.CommandText = sql
                sqlcmd.ExecuteNonQuery()



            ElseIf state = "mod" Then



                '����Ѿ������½��Ͳ����޸�
                sql = "select count(*) from CGD_master where state=0 and billno='" & Me.TextBox1.Text.Trim & "'"
                sqlcmd.CommandText = sql
                If sqlcmd.ExecuteScalar = 0 Then
                    Throw New System.Exception("�õ��ݲ����½�״̬���������޸ģ�")
                End If

                sql = "UPDATE CGD_master " & _
                  "   SET  [project_no] = '" & Me.TextBox2.Text.Trim & "' " & _
                "      ,[memo] =  '" & Me.TextBox4.Text.Trim & "' " & _
            " WHERE billno='" & Me.TextBox1.Text.Trim & "'"

                sqlcmd.CommandText = sql
                sqlcmd.ExecuteNonQuery()



                For i = 0 To Me.ExDataGridView1.Rows.Count - 1
                    If Me.ExDataGridView1.Rows(i).Cells("id").Value <> 0 Then
                        s = s & Me.ExDataGridView1.Rows(i).Cells("id").Value & ","

                        sql = "UPDATE [CGD_detail] " & _
                                "   SET [name] = '" & Me.ExDataGridView1.Rows(i).Cells("name").Value & "' " & _
                                "      ,[amount] = '" & Me.ExDataGridView1.Rows(i).Cells("amount").Value & "' " & _
                                "      ,[memo] = '" & Me.ExDataGridView1.Rows(i).Cells("memo").Value & "' " & _
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


                sql = "delete from CGD_detail where  billno='" & Me.TextBox1.Text.Trim & "' " & s
                sqlcmd.CommandText = sql
                sqlcmd.ExecuteNonQuery()


                For i = 0 To Me.ExDataGridView1.Rows.Count - 1
                    If Me.ExDataGridView1.Rows(i).Cells("id").Value = 0 Then
                        sql = "INSERT INTO CGD_detail ( amount,billno,name,money,price,memo) values " & _
                    "( '" & Me.ExDataGridView1.Rows(i).Cells("amount").Value & "', '" & Me.TextBox1.Text.Trim & "', '" & Me.ExDataGridView1.Rows(i).Cells("name").Value & "'," & _
                    "'0', '0', '" & Me.ExDataGridView1.Rows(i).Cells("memo").Value & "') "
                        sqlcmd.CommandText = sql
                        sqlcmd.ExecuteNonQuery()

                        't_part_in(Me.ExDataGridView1.Rows(i).Cells("item_id").Value, bh_no, Me.ComboBox8.Text.Trim, Me.ExDataGridView1.Rows(i).Cells("packing_id").Value, Me.ExDataGridView1.Rows(i).Cells("standards_id").Value, sqlcmd)
                    End If
                Next

                sql = "update a set a.money=b.money " & _
                     "from cgd_master a left join	 " & _
                     "	(select sum(price*amount) as money,billno from cgd_detail where billno='" & Me.TextBox1.Text.Trim & "' group by billno) b on a.billno=b.billno  " & _
                     "where a.billno='" & Me.TextBox1.Text.Trim & "' "
                sqlcmd.CommandText = sql
                sqlcmd.ExecuteNonQuery()

            End If

            mytrans.Commit()
        Catch ex As Exception
            mytrans.Rollback()
            MsgBox(ex.Message, MsgBoxStyle.OkOnly, "����")
            Exit Sub
        End Try

        MsgBox("����ɹ���", MsgBoxStyle.OkOnly, "��ʾ")

        If state = "new" Then

        ElseIf state = "mod" Then

            CType(Me.Owner, CGSQ).ExDataGridView1.Rows(CType(Me.Owner, CGSQ).ExDataGridView1.SelectedCells(0).RowIndex).Cells("billno").Value = Me.TextBox1.Text
            CType(Me.Owner, CGSQ).ExDataGridView1.Rows(CType(Me.Owner, CGSQ).ExDataGridView1.SelectedCells(0).RowIndex).Cells("project_no").Value = Me.TextBox2.Text
            CType(Me.Owner, CGSQ).ExDataGridView1.Rows(CType(Me.Owner, CGSQ).ExDataGridView1.SelectedCells(0).RowIndex).Cells("memo").Value = Me.TextBox4.Text
            'CType(Me.Owner, CGSQ).ExDataGridView1.Rows(CType(Me.Owner, CGSQ).ExDataGridView1.SelectedCells(0).RowIndex).Cells("supplier_name").Value = Me.ComboBox1.Text


        End If
        Me.DialogResult = Windows.Forms.DialogResult.OK
    End Sub

 
End Class