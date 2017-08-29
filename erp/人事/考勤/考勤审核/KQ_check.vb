Imports System.Data.SqlClient
Public Class KQ_check

    Private Sub ToolStripButton_refresh_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripButton_refresh.Click
        Refresh_ExDataGridView_master()
    End Sub


    Private Sub Refresh_ExDataGridView_master(Optional ByVal s As String = "")
        Dim sqlcon As New SqlConnection
        Dim sqlcmd As New SqlCommand
        Dim sqlad As New SqlDataAdapter
        Dim s1 As String = ""

        Dim dt As New DataTable
        Dim sql As String



        sqlcon = getconn()
        sqlcmd.Connection = sqlcon

        If Me.CheckBox1.Checked Then
            s1 = s1 & " a.state=0 or "
        End If

        If Me.CheckBox2.Checked Then
            s1 = s1 & " a.state=1 or "
        End If



        If s1.Length <> 0 Then
            s1 = Microsoft.VisualBasic.Left(s1, s1.Length - 3)
            s1 = " and (" & s1 & ")"
        End If



        If BM_id = 49 Or BM_id = 78 Then
            sql = "SELECT a.*,case state when 0 then '�½�' when 1 then '���' when 2 then '�᰸' end as state1,b.rs_name,c.sy_name " & _
                     "FROM t_kq a left join " & _
                     " t_rs b on a.rs_no=b.rs_no left join " & _
                     "t_sy c on a.sy_no=c.sy_no " & _
                     "where 1=1 " & s & s1 & "  order by a.billno "
        Else
            sql = "SELECT a.*,case state when 0 then '�½�' when 1 then '���' when 2 then '�᰸' end as state1,b.rs_name,c.sy_name " & _
                    "FROM t_kq a left join " & _
                    " t_rs b on a.rs_no=b.rs_no left join " & _
                    "t_sy c on a.sy_no=c.sy_no left join " & _
                       " t_rs on a.input_man=t_rs.rs_name " & _
                    "where t_rs.bm_id= " & BM_id & s & s1 & "  order by a.billno "
        End If



        sqlcmd.CommandText = sql
        sqlad.SelectCommand = sqlcmd
        dt.Rows.Clear()
        sqlad.Fill(dt)

        Me.ExDataGridView1.SetDataSource(dt)
        Me.ExDataGridView1.Columns.Clear()

        Me.ExDataGridView1.AddColumn("check", "", 20, False, New DataGridViewCheckBoxColumn)
        Me.ExDataGridView1.AddColumn("billno", "���ڵ���")
        Me.ExDataGridView1.AddColumn("rs_no", "��Ա���")
        Me.ExDataGridView1.AddColumn("rs_name", "��Ա����")
        Me.ExDataGridView1.AddColumn("FS_date", "����")
        Me.ExDataGridView1.AddColumn("sj", "ʱ��")
        Me.ExDataGridView1.AddColumn("sy_name", "����")
        Me.ExDataGridView1.AddColumn("state1", "״̬")

        Me.ExDataGridView1.AddColumn("memo", "˵��")


        Me.ExDataGridView1.AddColumn("input_man", "¼����")
        Me.ExDataGridView1.AddColumn("input_date", "¼������", , , , , "yyyy-MM-dd")

        Me.ExDataGridView1.AddColumn("check_man", "�����")
        Me.ExDataGridView1.AddColumn("check_date", "�������", , , , , "yyyy-MM-dd")


        Me.ExDataGridView1.AddColumn("sy_no", "sy_no", , , , , , False)
        Me.ExDataGridView1.AddColumn("id", "id", , , , , , False)


    End Sub

    Private Sub ToolStripButton_exit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripButton_exit.Click
        Me.Close()
    End Sub

    Private Sub ���ToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ���ToolStripMenuItem.Click
        Dim sqlcon As New SqlConnection
        Dim sqlcmd As New SqlCommand

        Dim sql As String
        Dim k As int32

        If Me.ExDataGridView1.SelectedCells.Count = 0 Then
            MsgBox("��ѡ��Ҫ��˵ĵ��ݣ�", MsgBoxStyle.OkOnly, "��ʾ")
            Exit Sub
        End If

        sqlcon = getconn()
        sqlcmd.Connection = sqlcon

        sql = "SELECT state " & _
                "FROM t_kq where billno='" & Me.ExDataGridView1.Rows(Me.ExDataGridView1.SelectedCells(0).RowIndex).Cells("billno").Value & "'"


        sqlcmd.CommandText = sql
        k = sqlcmd.ExecuteScalar

        If k = 2 Then
            MsgBox("�õ����Ѿ��᰸������������", MsgBoxStyle.OkOnly, "��ʾ")
            Exit Sub
        ElseIf k = 1 Then
            MsgBox("�õ����Ѿ���ˣ�����������", MsgBoxStyle.OkOnly, "��ʾ")
            Exit Sub
        End If

        sql = "update  t_kq set state=1,check_date=getdate(),check_man='" & rs_name & "' where billno='" & Me.ExDataGridView1.Rows(Me.ExDataGridView1.SelectedCells(0).RowIndex).Cells("billno").Value & "'"
        sqlcmd.CommandText = sql
        sqlcmd.ExecuteNonQuery()

        'Refresh_ExDataGridView(Me.TreeView1.SelectedNode.Text)
        Me.ExDataGridView1.Rows(Me.ExDataGridView1.SelectedCells(0).RowIndex).Cells("state1").Value = "���"

        Me.ExDataGridView1.Rows(Me.ExDataGridView1.SelectedCells(0).RowIndex).Cells("check_man").Value = rs_name
        Me.ExDataGridView1.Rows(Me.ExDataGridView1.SelectedCells(0).RowIndex).Cells("check_date").Value = Now

        MsgBox("��˳ɹ���", MsgBoxStyle.OkOnly, "��ʾ")
    End Sub

    Private Sub ����ToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ����ToolStripMenuItem.Click
        Dim sqlcon As New SqlConnection
        Dim sqlcmd As New SqlCommand

        Dim sql As String
        Dim k As int32

        If Me.ExDataGridView1.SelectedCells.Count = 0 Then
            MsgBox("��ѡ��Ҫ����ĵ��ݣ�", MsgBoxStyle.OkOnly, "��ʾ")
            Exit Sub
        End If

        sqlcon = getconn()
        sqlcmd.Connection = sqlcon

        sql = "SELECT state " & _
                "FROM t_kq where billno='" & Me.ExDataGridView1.Rows(Me.ExDataGridView1.SelectedCells(0).RowIndex).Cells("billno").Value & "'"


        sqlcmd.CommandText = sql
        k = sqlcmd.ExecuteScalar

        If k = 2 Then
            MsgBox("�õ����Ѿ��᰸������������", MsgBoxStyle.OkOnly, "��ʾ")
            Exit Sub
        ElseIf k = 0 Then
            MsgBox("�õ���״̬Ϊ�½�������������", MsgBoxStyle.OkOnly, "��ʾ")
            Exit Sub
        End If






        sql = "update  t_kq set state=0 where billno='" & Me.ExDataGridView1.Rows(Me.ExDataGridView1.SelectedCells(0).RowIndex).Cells("billno").Value & "'"
        sqlcmd.CommandText = sql
        sqlcmd.ExecuteNonQuery()

        'Refresh_ExDataGridView(Me.TreeView1.SelectedNode.Text)
        Me.ExDataGridView1.Rows(Me.ExDataGridView1.SelectedCells(0).RowIndex).Cells("state1").Value = "�½�"

        MsgBox("����˳ɹ���", MsgBoxStyle.OkOnly, "��ʾ")
    End Sub

    Private Sub ToolStripButton_question_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripButton_question.Click
        Dim kq_cha As New KQ_cha
        Me.Tag = ""
        kq_cha.Owner = Me
        If kq_cha.ShowDialog() = Windows.Forms.DialogResult.OK Then

            Refresh_ExDataGridView_master(Me.Tag)
            Me.Tag = ""
        End If
    End Sub

    Private Sub ToolStripButton_excel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripButton_excel.Click
        Dim ToExcel As New BaseClass.TOEXCEL

        If ToExcel.ExToExcel(Me.ExDataGridView1, Me.SaveFileDialog1, True, "��ǰ���", "�ڳ����") Then
            MsgBox("�����ɹ���", MsgBoxStyle.OkOnly, "��ʾ")
        End If
    End Sub

    Private Sub ȫѡToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ȫѡToolStripMenuItem.Click
        Dim i As int32

        For i = 0 To Me.ExDataGridView1.Rows.Count - 1
            Me.ExDataGridView1.Rows(i).Cells("check").Value = True
        Next
    End Sub

    Private Sub ��ѡToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ��ѡToolStripMenuItem.Click
        Dim i As int32

        For i = 0 To Me.ExDataGridView1.Rows.Count - 1
            Me.ExDataGridView1.Rows(i).Cells("check").Value = Not Me.ExDataGridView1.Rows(i).Cells("check").Value
        Next
    End Sub

    Private Sub ToolStripButton_save_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripButton_save.Click
        Dim sqlcon As New SqlConnection
        Dim sqlcmd As New SqlCommand
        Dim mytrans As SqlTransaction

        Dim sql As String


        Dim i As int32

        sqlcon = getconn()
        sqlcmd.Connection = sqlcon

        If Me.ExDataGridView1.Rows.Count > 0 Then
            Me.ExDataGridView1.Rows(0).Cells(1).Selected = True
        End If

        mytrans = sqlcon.BeginTransaction
        sqlcmd.Transaction = mytrans
        Try


            For i = 0 To Me.ExDataGridView1.Rows.Count - 1
                If Me.ExDataGridView1.Rows(i).Cells("check").Value = True Then
                    sql = "SELECT state " & _
                                     "FROM t_kq where billno='" & Me.ExDataGridView1.Rows(i).Cells("billno").Value & "'"

                    sqlcmd.CommandText = sql
                    If sqlcmd.ExecuteScalar = 0 Then
                        sql = "update  t_kq set state=1,check_date=getdate(),check_man='" & rs_name & "' where billno='" & Me.ExDataGridView1.Rows(i).Cells("billno").Value & "'"
                        sqlcmd.CommandText = sql
                        sqlcmd.ExecuteNonQuery()

                        Me.ExDataGridView1.Rows(i).Cells("state1").Value = "���"

                        Me.ExDataGridView1.Rows(i).Cells("check_man").Value = rs_name
                        Me.ExDataGridView1.Rows(i).Cells("check_date").Value = Now
                    End If


                End If
            Next

            mytrans.Commit()
        Catch ex As Exception
            mytrans.Rollback()
            MsgBox(ex.Message, MsgBoxStyle.OkOnly, "����")
            Exit Sub
        End Try

        MsgBox("��˳ɹ���", MsgBoxStyle.OkOnly, "��ʾ")
    End Sub
End Class