Imports System.Data.SqlClient
Public Class T_JEYS_add
    Friend state As String
    Friend billno As String
    Friend dt_p As New DataTable

    Private Sub ToolStripButton_exit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripButton_exit.Click
        Me.DialogResult = Windows.Forms.DialogResult.Cancel
    End Sub

    Private Sub ��Ӽ�¼ToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ��Ӽ�¼ToolStripMenuItem.Click
        Dim T_JEYS_add_detail As New T_JEYS_add_detail

        Dim sqlcon As New SqlConnection
        Dim sqlcmd As New SqlCommand
        Dim sqlad As New SqlDataAdapter

        Dim dt As New DataTable
        Dim sql As String
        Dim i As Int32
        Dim s As String = ""



        T_JEYS_add_detail.Owner = Me
        T_JEYS_add_detail.ShowDialog()

        If T_JEYS_add_detail.DialogResult = Windows.Forms.DialogResult.Cancel Then
            T_JEYS_add_detail.Dispose()
            Exit Sub
        End If


        If Me.Tag = "" Then
            Exit Sub
        End If



        sqlcon = getconn()
        sqlcmd.Connection = sqlcon

        sql = "SELECT a.*,case state when 0 then '�½�' when 1 then '���' when 2 then '�᰸' end as state1,project_money-SK_money as WSK_money,project_money-SK_money as BC_money " & _
                     "FROM project_master a " & _
                     "where project_no in (" & Me.Tag & ") order by a.id "
        sqlcmd.CommandText = sql

        sqlad.SelectCommand = sqlcmd
        sqlad.Fill(dt)

        If Me.ExDataGridView1.Columns.Count = 0 Then
            dt_p = dt

            Me.ExDataGridView1.SetDataSource(dt_p)
            Me.ExDataGridView1.Columns.Clear()

            Me.ExDataGridView1.AddColumn("project_no", "���̺�")
            Me.ExDataGridView1.AddColumn("project_name", "��������")
            Me.ExDataGridView1.AddColumn("project_money", "���̽��", , , , , "#0")
            Me.ExDataGridView1.AddColumn("SK_money", "���տ���", , , , , "#0")
            Me.ExDataGridView1.AddColumn("WSK_money", "δ�տ���", , , , , "#0")
            Me.ExDataGridView1.AddColumn("BC_money", "�����տ���", , False, , , "#0")


            Me.ExDataGridView1.AddColumn("id", "id", , , , , , False)
        Else
            For i = 0 To dt.Rows.Count - 1

                Dim DR As DataRow = Me.dt_p.NewRow

                DR("project_no") = dt.Rows(i)("project_no")

                DR("project_name") = dt.Rows(i)("project_name")
                DR("project_money") = dt.Rows(i)("project_money")
                DR("SK_money") = dt.Rows(i)("SK_money")
                DR("WSK_money") = dt.Rows(i)("WSK_money")
                DR("BC_money") = dt.Rows(i)("BC_money")



                DR("id") = 0

                dt_p.Rows.Add(DR)


            Next
        End If

    End Sub

    Private Sub ɾ����¼ToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ɾ����¼ToolStripMenuItem.Click

        If MsgBox("ȷ��Ҫɾ���˼�¼��", MsgBoxStyle.YesNo, "��ʾ") = MsgBoxResult.No Then
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




        Me.TextBox3.Focus()




        sqlcon = getconn()
        sqlcmd.Connection = sqlcon
        mytrans = sqlcon.BeginTransaction

        sqlcmd.Transaction = mytrans



        Try
            If state = "new" Then


                sql = "update t_dj_num set num=num+1 where  tt='SK'"
                sqlcmd.CommandText = sql
                sqlcmd.ExecuteNonQuery()

                sql = "select * from t_dj_num where tt='SK'"
                sqlcmd.CommandText = sql
                sqlad.SelectCommand = sqlcmd
                dt.Rows.Clear()
                sqlad.Fill(dt)

                billno = dt.Rows(0)("TT") & "-" & GetAccountterm() & "-" & Format(dt.Rows(0)("num"), dt.Rows(0)("format"))




                sql = "INSERT INTO [T_SKYS_master] " & _
                            "           ([billno] " & _
                            "           ,[memo] " & _
                            "           ,[input_man] " & _
                            "           ,[input_date] " & _
                            "           ,[state] ) " & _
                            "     VALUES " & _
                            "           ('" & billno & "'" & _
                            "           ,'" & Me.TextBox3.Text.Trim & "' " & _
                            "           ,'" & rs_name & "' " & _
                            "           ,getdate() " & _
                            "           ,0) "

                sqlcmd.CommandText = sql
                sqlcmd.ExecuteNonQuery()

                For i = 0 To Me.ExDataGridView1.Rows.Count - 1

                    sql = "INSERT INTO [T_SKYS_detail] " & _
                        "           ([billno] " & _
                        "           ,[project_no] " & _
                        "           ,[money]) " & _
                        "     VALUES " & _
                        "           ('" & billno & "'" & _
                        "           ,'" & Me.ExDataGridView1.Rows(i).Cells("project_no").Value & "' " & _
                        "           ," & Me.ExDataGridView1.Rows(i).Cells("bc_money").Value & ") "


                    sqlcmd.CommandText = sql
                    sqlcmd.ExecuteNonQuery()

                    sql = "update a " & _
                            "set a.sk_money=b.smoney " & _
                            "from  " & _
                            " project_master  a left join  " & _
                            "(SELECT    project_no, SUM(money) Smoney " & _
                            "FROM         T_SKYS_detail " & _
                            "WHERE     (project_no = '" & Me.ExDataGridView1.Rows(i).Cells("project_no").Value & "') " & _
                            "group by project_no) b on a.project_no=b.project_no " & _
                            "where a.project_no= '" & Me.ExDataGridView1.Rows(i).Cells("project_no").Value & "'"

                    sqlcmd.CommandText = sql
                    sqlcmd.ExecuteNonQuery()


                Next

            ElseIf state = "mod" Then



                '����Ѿ������½��Ͳ����޸�
                sql = "select count(*) from T_SKYS_master where state=0 and billno='" & billno & "'"
                sqlcmd.CommandText = sql
                If sqlcmd.ExecuteScalar = 0 Then
                    Throw New System.Exception("�õ��ݲ����½�״̬���������޸ģ�")
                End If

                sql = "update T_SKYS_master set memo='" & Me.TextBox3.Text.Trim & "'" & _
                    " where billno='" & billno & "'"
                sqlcmd.CommandText = sql
                sqlcmd.ExecuteNonQuery()


                sql = "update b set b.SK_money=b.SK_money- a.smoney " & _
                        "from (select project_no,sum(money) as smoney from  T_SKYS_detail where billno='" & billno & "' group by project_no) a left join  " & _
                        "	project_master b on a.project_no=b.project_NO "
                sqlcmd.CommandText = sql
                sqlcmd.ExecuteNonQuery()


                For i = 0 To Me.ExDataGridView1.Rows.Count - 1
                    If Me.ExDataGridView1.Rows(i).Cells("id").Value <> 0 Then
                        s = s & Me.ExDataGridView1.Rows(i).Cells("id").Value & ","



                        sql = "update T_SKYS_detail set money='" & Me.ExDataGridView1.Rows(i).Cells("bc_money").Value & "'" & _
                                                            "  where id=" & Me.ExDataGridView1.Rows(i).Cells("id").Value
                        sqlcmd.CommandText = sql
                        sqlcmd.ExecuteNonQuery()

                        sql = "update a " & _
                            "set a.sk_money=b.smoney " & _
                            "from  " & _
                            " project_master  a left join  " & _
                            "(SELECT    project_no, SUM(money) Smoney " & _
                            "FROM         T_SKYS_detail " & _
                            "WHERE     (project_no = '" & Me.ExDataGridView1.Rows(i).Cells("project_no").Value & "') " & _
                            "group by project_no) b on a.project_no=b.project_no " & _
                            "where a.project_no= '" & Me.ExDataGridView1.Rows(i).Cells("project_no").Value & "'"

                        sqlcmd.CommandText = sql
                        sqlcmd.ExecuteNonQuery()

                    End If
                Next

                If s.Length <> 0 Then
                    s = s.Substring(0, s.Length - 1)
                    s = " and  id not in (" & s & ")"
                End If


                sql = "delete from T_SKYS_detail where  billno='" & billno & "' " & s
                sqlcmd.CommandText = sql
                sqlcmd.ExecuteNonQuery()


                For i = 0 To Me.ExDataGridView1.Rows.Count - 1
                    If Me.ExDataGridView1.Rows(i).Cells("id").Value = 0 Then
                        sql = "INSERT INTO [T_SKYS_detail] " & _
                                "           ([billno] " & _
                                "           ,[project_no] " & _
                                "           ,[money]) " & _
                                "     VALUES " & _
                                "           ('" & billno & "'" & _
                                "           ,'" & Me.ExDataGridView1.Rows(i).Cells("project_no").Value & "' " & _
                                "           ," & Me.ExDataGridView1.Rows(i).Cells("bc_money").Value & ") "


                        sqlcmd.CommandText = sql
                        sqlcmd.ExecuteNonQuery()

                        sql = "update a " & _
                            "set a.sk_money=b.smoney " & _
                            "from  " & _
                            " project_master  a left join  " & _
                            "(SELECT    project_no, SUM(money) Smoney " & _
                            "FROM         T_SKYS_detail " & _
                            "WHERE     (project_no = '" & Me.ExDataGridView1.Rows(i).Cells("project_no").Value & "') " & _
                            "group by project_no) b on a.project_no=b.project_no " & _
                            "where a.project_no= '" & Me.ExDataGridView1.Rows(i).Cells("project_no").Value & "'"

                        sqlcmd.CommandText = sql
                        sqlcmd.ExecuteNonQuery()

                    End If
                Next
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

            CType(Me.Owner, T_JEYS).ExDataGridView1.Rows(CType(Me.Owner, T_JEYS).ExDataGridView1.SelectedCells(0).RowIndex).Cells("memo").Value = Me.TextBox3.Text

        End If
        Me.DialogResult = Windows.Forms.DialogResult.OK
    End Sub

    Private Sub T_JEYS_add_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim sql As String = ""
        Dim sqlcon As New SqlConnection
        Dim sqlcmd As New SqlCommand
        Dim sqlad As New SqlDataAdapter

        Dim dt As New DataTable
        sqlcon = getconn()
        sqlcmd.Connection = sqlcon

        If state = "new" Then


            Me.Text = "����Ӧ�ս��"

            sql = "SELECT a.*,case state when 0 then '�½�' when 1 then '���' when 2 then '�᰸' end as state1,project_money-SK_money as WSK_money,project_money-SK_money as BC_money " & _
                     "FROM project_master a " & _
                     "where a.id=-1"




        ElseIf state = "mod" Then

            Me.Text = "�޸�Ӧ�ս��"

            Me.TextBox1.Enabled = False

            Me.TextBox1.Text = CType(Me.Owner, T_JEYS).ExDataGridView1.Rows(CType(Me.Owner, T_JEYS).ExDataGridView1.SelectedCells(0).RowIndex).Cells("billno").Value

            Me.TextBox3.Text = CType(Me.Owner, T_JEYS).ExDataGridView1.Rows(CType(Me.Owner, T_JEYS).ExDataGridView1.SelectedCells(0).RowIndex).Cells("memo").Value

    

            sql = "select a.*,b.project_no,b.project_name,b.project_money,b.SK_money,b.effective_date,b.memo,b.project_money-b.SK_money as WSK_money,a.money as bc_money " & _
                    "from t_SKys_detail a left join  " & _
                    "	project_master b on a.project_no=b.project_no " & _
                    " where a.billno='" & billno & "'"



        End If

        sqlcmd.CommandText = sql
        sqlad.SelectCommand = sqlcmd
        dt_p.Rows.Clear()
        sqlad.Fill(dt_p)

        Me.ExDataGridView1.SetDataSource(dt_p)
        Me.ExDataGridView1.Columns.Clear()

        Me.ExDataGridView1.AddColumn("project_no", "���̺�")
        Me.ExDataGridView1.AddColumn("project_name", "��������")
        Me.ExDataGridView1.AddColumn("project_money", "���̽��", , , , , "#0")
        Me.ExDataGridView1.AddColumn("SK_money", "���տ���", , , , , "#0")
        Me.ExDataGridView1.AddColumn("WSK_money", "δ�տ���", , , , , "#0")
        Me.ExDataGridView1.AddColumn("bc_money", "�����տ���", , False, , , "#0")


        Me.ExDataGridView1.AddColumn("id", "id", , , , , , False)
    End Sub
End Class