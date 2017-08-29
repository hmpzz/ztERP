
Imports System.Data.SqlClient
Public Class project_WG



    Private Sub ToolStripButton_refresh_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripButton_refresh.Click
        Refresh_ExDataGridView_master(" and a.project_no like '%" & Me.TextBox1.Text.Trim & "%' ")
    End Sub


    Private Sub Refresh_ExDataGridView_master(Optional ByVal s As String = "")
        Dim sqlcon As New SqlConnection
        Dim sqlcmd As New SqlCommand
        Dim sqlad As New SqlDataAdapter
        Dim s1 As String = ""
        Dim s2 As String = ""

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

        If Me.CheckBox3.Checked Then
            s1 = s1 & " a.state=2 or "
        End If


        If s1.Length <> 0 Then
            s1 = Microsoft.VisualBasic.Left(s1, s1.Length - 3)
            s1 = " and (" & s1 & ")"
        End If


        If Me.CheckBox4.Checked Then
            s2 = s2 & " and a.effective_date>=getdate()"
        End If



        If BM_id = 49 Or BM_id = 78 Then
            sql = "SELECT a.*,case state when 0 then '�½�' when 1 then '���' when 2 then '�᰸' end as state1 " & _
                    "FROM project_master a " & _
                    "where factory_code in (select c.factory_code " & _
                    "                       from sys_user a inner join  " & _
                    "                       	t_user_factory b on a.login_id=b.login_id inner join  " & _
                    "                       	t_factory c on b.factory_code=c.factory_code " & _
                    "                       where a.stop_login=1 and c.stop=1 and a.login_id='" & login_id & "' " & _
                    "                       )  " & s & s1 & s2 & "  order by a.id "
        Else
            sql = "SELECT a.*,case state when 0 then '�½�' when 1 then '���' when 2 then '�᰸' end as state1 " & _
                    "FROM project_master a left join  " & _
                       " t_rs on a.input_man=t_rs.rs_name " & _
                    "where factory_code in (select c.factory_code " & _
                    "                       from sys_user a inner join  " & _
                    "                       	t_user_factory b on a.login_id=b.login_id inner join  " & _
                    "                       	t_factory c on b.factory_code=c.factory_code " & _
                    "                       where a.stop_login=1 and c.stop=1 and a.login_id='" & login_id & "' " & _
                    "                       )  and t_rs.bm_id= " & BM_id & s & s1 & s2 & "  order by a.id "
        End If

        sqlcmd.CommandText = sql
        sqlad.SelectCommand = sqlcmd
        dt.Rows.Clear()
        sqlad.Fill(dt)

        Me.ExDataGridView1.SetDataSource(dt)
        Me.ExDataGridView1.Columns.Clear()

        'Me.ExDataGridView1.AddColumn("check", "", 20, False, New DataGridViewCheckBoxColumn)
        Me.ExDataGridView1.AddColumn("project_no", "���̺�")
        Me.ExDataGridView1.AddColumn("project_name", "��������")
        Me.ExDataGridView1.AddColumn("project_money", "���̽��", , , , , "#0")
        Me.ExDataGridView1.AddColumn("FP_money", "��Ʊ���", , , , , "#0")
        Me.ExDataGridView1.AddColumn("SK_money", "�տ���", , , , , "#0")


        Me.ExDataGridView1.AddColumn("effective_date", "��Ч����", , , , , "yyyy-MM-dd")


        Me.ExDataGridView1.AddColumn("memo", "��ע")
        Me.ExDataGridView1.AddColumn("state1", "״̬")

        Me.ExDataGridView1.AddColumn("begin_date", "��������", , , , , "yyyy-MM-dd")
        Me.ExDataGridView1.AddColumn("end_date", "�깤����", , , , , "yyyy-MM-dd")
        Me.ExDataGridView1.AddColumn("end_man", "ȷ����")

        Me.ExDataGridView1.AddColumn("input_man", "¼����")
        Me.ExDataGridView1.AddColumn("input_date", "¼������", , , , , "yyyy-MM-dd")
        Me.ExDataGridView1.AddColumn("check_man", "�����")
        Me.ExDataGridView1.AddColumn("check_date", "�������", , , , , "yyyy-MM-dd")

        Me.ExDataGridView1.AddColumn("id", "id", , , , , , False)

        If Me.ExDataGridView1.Rows.Count > 0 Then
            cellclick(0)
        End If
    End Sub

    Private Sub ToolStripButton_exit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripButton_exit.Click
        Me.Close()
    End Sub

    Private Sub ExDataGridView1_CellClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles ExDataGridView1.CellClick
        If e.RowIndex < 0 Then
            Exit Sub
        Else
            cellclick(e.RowIndex)
        End If
    End Sub


    Private Sub cellclick(ByVal index As Int32)
        Dim sqlcon As New SqlConnection
        Dim sqlcmd As New SqlCommand
        Dim sqlad As New SqlDataAdapter

        Dim dt As New DataTable
        Dim sql As String
        Dim s As String = ""

        sqlcon = getconn()


        'sql = "select * from  project_detail where project_no='" & Me.ExDataGridView1.Rows(index).Cells("project_no").Value & "' order by id"
        If index <> -1 Then

            sql = "select * from  project_detail where project_no='" & Me.ExDataGridView1.Rows(index).Cells("project_no").Value & "' order by id"

        Else
            sql = "select * from  project_detail where id=-1 order by id"
        End If
        sqlcmd.Connection = sqlcon
        sqlcmd.CommandText = sql
        sqlad.SelectCommand = sqlcmd
        dt.Rows.Clear()
        sqlad.Fill(dt)

        Me.ExDataGridView2.SetDataSource(dt)
        Me.ExDataGridView2.Columns.Clear()




        Me.ExDataGridView2.AddColumn("tz_money", "������", , , , DataGridViewContentAlignment.MiddleRight, "#0")
        Me.ExDataGridView2.AddColumn("tz_man", "������")
        Me.ExDataGridView2.AddColumn("tz_date", "����ʱ��")

        Me.ExDataGridView2.AddColumn("memo", "˵��")


        Me.ExDataGridView2.AddColumn("id", "id", , , , , , False)
    End Sub

    Private Sub ToolStripButton_mod_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripButton_mod.Click
        Dim project_add As New project_add

        Dim sqlcon As New SqlConnection
        Dim sqlcmd As New SqlCommand
        Dim sql As String = ""
        Dim mytrans As SqlTransaction




        If Me.ExDataGridView1.SelectedCells.Count < 0 Then
            MsgBox("��ѡ��Ҫ�깤���У�", MsgBoxStyle.OkOnly, "��ʾ")
            Exit Sub
        End If

        sqlcon = getconn()



        If MsgBox("ȷ��Ҫ�����̺�Ϊ" & Me.ExDataGridView1.Rows(Me.ExDataGridView1.SelectedCells(0).RowIndex).Cells("project_no").Value & "�Ĺ����깤��", MsgBoxStyle.YesNo, "��ʾ") = MsgBoxResult.No Then
            Exit Sub
        End If

        sqlcon = getconn()

        sqlcmd.Connection = sqlcon
        mytrans = sqlcon.BeginTransaction()
        sqlcmd.Transaction = mytrans
        Try


            sql = "update  project_master set end_date=getdate() ,end_man ='" & rs_name & "' where project_no='" & Me.ExDataGridView1.Rows(Me.ExDataGridView1.SelectedCells(0).RowIndex).Cells("project_no").Value & "'"
            sqlcmd.CommandText = sql
            sqlcmd.ExecuteNonQuery()


            sql = "update  T_part_out_master set state=2,check_date=getdate(),check_man='" & rs_name & "' where project_no='" & Me.ExDataGridView1.Rows(Me.ExDataGridView1.SelectedCells(0).RowIndex).Cells("project_no").Value & "'"
            sqlcmd.CommandText = sql
            sqlcmd.ExecuteNonQuery()

            mytrans.Commit()
        Catch ex As Exception
            mytrans.Rollback()
            MsgBox(ex.Message, MsgBoxStyle.OkOnly, "��ʾ")
            Exit Sub
        End Try

        MsgBox("�깤�ɹ���", MsgBoxStyle.OkOnly, "��ʾ")

        Me.ExDataGridView1.Rows(Me.ExDataGridView1.SelectedCells(0).RowIndex).Cells("end_date").Value = Now
        Me.ExDataGridView1.Rows(Me.ExDataGridView1.SelectedCells(0).RowIndex).Cells("end_man").Value = rs_name
    End Sub
End Class