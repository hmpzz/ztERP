Imports System.Data.SqlClient

Public Class XDSH

    Private Sub ToolStripButton_exit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripButton_exit.Click
        Me.Close()
    End Sub

    Private Sub ToolStripButton_refresh_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripButton_refresh.Click
        Refresh_ExDataGridView_master()
    End Sub



    Private Sub Refresh_ExDataGridView_master(Optional ByVal s As String = "")
        Dim sqlcon As New SqlConnection
        Dim sqlcmd As New SqlCommand
        Dim sqlad As New SqlDataAdapter
        Dim s1 As String = ""
        Dim s2 As String = ""

        Dim dt As New DataTable
        Dim sql As String

        Me.ListView1.Items.Clear()

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
            s2 = s2 & " a.jd_state=0 or "
        End If

        If Me.CheckBox5.Checked Then
            s2 = s2 & " a.jd_state=1 or "
        End If

        If Me.CheckBox6.Checked Then
            s2 = s2 & " a.jd_state=2 or "
        End If

        If Me.CheckBox7.Checked Then
            s2 = s2 & " a.jd_state=3 or "
        End If

        If s2.Length <> 0 Then
            s2 = Microsoft.VisualBasic.Left(s2, s2.Length - 3)
            s2 = " and (" & s2 & ")"
        End If




        If BM_id = 49 Or BM_id = 78 Then
            sql = "SELECT a.*,case state when 0 then '�½�' when 1 then '���' when 2 then '�᰸' end as state1,b.bmmc as xd_bmmc,c.bmmc as jd_bmmc,case jd_state when 0 then 'δ�ӵ�' when 1 then '�ѽӵ�' when 2 then '�����' when 3 then 'ȷ�����' end as state2 " & _
                    "FROM SCD_master a left join " & _
                    "t_bm b on a.xd_bmno=b.bmno left join " & _
                    "t_bm c on a.jd_bmno=c.bmno " & _
                    "where 1=1 " & s & s1 & s2 & "  order by a.billno "
        Else
            sql = "SELECT a.*,case state when 0 then '�½�' when 1 then '���' when 2 then '�᰸' end as state1,b.bmmc as xd_bmmc,c.bmmc as jd_bmmc,case jd_state when 0 then 'δ�ӵ�' when 1 then '�ѽӵ�' when 2 then '�����' when 3 then 'ȷ�����' end as state2 " & _
                    "FROM SCD_master a left join " & _
                    "t_bm b on a.xd_bmno=b.bmno left join " & _
                    "t_bm c on a.jd_bmno=c.bmno left join " & _
                       " t_rs on a.input_man=t_rs.rs_name " & _
                    "where t_rs.bm_id= " & BM_id & s & s1 & s2 & "  order by a.billno "
        End If



        sqlcmd.CommandText = sql
        sqlad.SelectCommand = sqlcmd
        dt.Rows.Clear()
        sqlad.Fill(dt)

        Me.ExDataGridView1.SetDataSource(dt)
        Me.ExDataGridView1.Columns.Clear()

        Me.ExDataGridView1.AddColumn("billno", "����")
        'Me.ExDataGridView1.AddColumn("project_no", "���̺�")
        Me.ExDataGridView1.AddColumn("xd_bmmc", "�µ���������")
        Me.ExDataGridView1.AddColumn("jd_bmmc", "�ӵ���������")
        Me.ExDataGridView1.AddColumn("state1", "״̬")
        Me.ExDataGridView1.AddColumn("state2", "�ӵ�״̬")

        Me.ExDataGridView1.AddColumn("memo", "˵��")
        Me.ExDataGridView1.AddColumn("money", "�깤���", , , , DataGridViewContentAlignment.MiddleRight, "#0.00")


        Me.ExDataGridView1.AddColumn("input_man", "¼����")
        Me.ExDataGridView1.AddColumn("input_date", "¼������", , , , , "yyyy-MM-dd")

        Me.ExDataGridView1.AddColumn("check_man", "�����")
        Me.ExDataGridView1.AddColumn("check_date", "�������", , , , , "yyyy-MM-dd")

        Me.ExDataGridView1.AddColumn("jd_man", "�ӵ���")
        Me.ExDataGridView1.AddColumn("jd_date", "�ӵ�����", , , , , "yyyy-MM-dd")


        Me.ExDataGridView1.AddColumn("id", "id", , , , , , False)

        If Me.ExDataGridView1.Rows.Count > 0 Then
            cellclick(0)
        Else
            cellclick(-1)
        End If
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
        Dim listitem As ListViewItem
        Dim i As Integer

        sqlcon = getconn()

        Me.ListView1.Items.Clear()

        'sql = "select * from  t_product_FY_detail where fy_id='" & Me.ExDataGridView1.Rows(index).Cells("id").Value & "' order by id"
        If index <> -1 Then

            sql = " SELECT a.* " & _
"                        FROM pic a    " & _
"                           where a.billno='" & Me.ExDataGridView1.Rows(index).Cells("billno").Value & "' order by a.id"

        Else
            sql = " SELECT a.*  " & _
"                        FROM pic a " & _
"                           where a.id=-1"
        End If
        sqlcmd.Connection = sqlcon
        sqlcmd.CommandText = sql
        sqlad.SelectCommand = sqlcmd
        dt.Rows.Clear()
        sqlad.Fill(dt)


        For i = 0 To dt.Rows.Count - 1

            listitem = Me.ListView1.Items.Add(dt.Rows(i)("filename"), 0)
            listitem.ToolTipText = dt.Rows(i)("filename")

        Next i

    End Sub

    Private Sub ListView1_MouseUp(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles ListView1.MouseUp
        If Me.ListView1.SelectedItems.Count = 0 Then

            Me.ContextMenuStrip1.Items("���ļ�ToolStripMenuItem").Enabled = False
        Else
            Me.ContextMenuStrip1.Items("���ļ�ToolStripMenuItem").Enabled = True
        End If
    End Sub

    
    Private Sub ListView1_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ListView1.SelectedIndexChanged

    End Sub

    Private Sub ���ļ�ToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ���ļ�ToolStripMenuItem.Click
        OutputFile(Me.ListView1.SelectedItems(0).Text, Me.ExDataGridView1.Rows(Me.ExDataGridView1.SelectedCells(0).RowIndex).Cells("billno").Value)
    End Sub

   

    Private Sub ExDataGridView1_CellMouseDown(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellMouseEventArgs) Handles ExDataGridView1.CellMouseDown
        If Me.ExDataGridView1.SelectedCells.Count = 0 Then
            Me.���ToolStripMenuItem.Enabled = False
            Me.����ToolStripMenuItem.Enabled = False
            Me.�᰸ToolStripMenuItem.Enabled = False
            Me.���᰸ToolStripMenuItem.Enabled = False
        End If

        If e.RowIndex < 0 Then
            Exit Sub
        End If

        If Me.ExDataGridView1.Rows(e.RowIndex).Cells("state1").Value = "�½�" Then
            Me.���ToolStripMenuItem.Enabled = True
            Me.����ToolStripMenuItem.Enabled = False
            Me.�᰸ToolStripMenuItem.Enabled = True
            Me.���᰸ToolStripMenuItem.Enabled = False
        ElseIf Me.ExDataGridView1.Rows(e.RowIndex).Cells("state1").Value = "���" Then
            Me.���ToolStripMenuItem.Enabled = False
            Me.����ToolStripMenuItem.Enabled = True
            Me.�᰸ToolStripMenuItem.Enabled = True
            Me.���᰸ToolStripMenuItem.Enabled = False
        ElseIf Me.ExDataGridView1.Rows(e.RowIndex).Cells("state1").Value = "�᰸" Then
            Me.���ToolStripMenuItem.Enabled = False
            Me.����ToolStripMenuItem.Enabled = False
            Me.�᰸ToolStripMenuItem.Enabled = False
            Me.���᰸ToolStripMenuItem.Enabled = True
        End If

        If Me.ExDataGridView1.Rows(e.RowIndex).Cells("state2").Value <> "δ�ӵ�" Then
            Me.���ToolStripMenuItem.Enabled = False
            Me.����ToolStripMenuItem.Enabled = False
            Me.�᰸ToolStripMenuItem.Enabled = False
            Me.���᰸ToolStripMenuItem.Enabled = False
        End If

    End Sub

   
    Private Sub ExDataGridView1_CellContentClick(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles ExDataGridView1.CellContentClick

    End Sub

    Private Sub ���ToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ���ToolStripMenuItem.Click
        Dim sqlcon As New SqlConnection
        Dim sqlcmd As New SqlCommand
        Dim sqlad As New SqlDataAdapter

        Dim dt As New DataTable

        Dim sql As String


        If Me.ExDataGridView1.SelectedCells.Count = 0 Then
            MsgBox("��ѡ��Ҫ��˵ĵ��ݣ�", MsgBoxStyle.OkOnly, "��ʾ")
            Exit Sub
        End If

        sqlcon = getconn()
        sqlcmd.Connection = sqlcon


        sql = "SELECT state,jd_state " & _
                "FROM scd_master where billno='" & Me.ExDataGridView1.Rows(Me.ExDataGridView1.SelectedCells(0).RowIndex).Cells("billno").Value & "'"

        sqlcmd.CommandText = sql
        sqlad.SelectCommand = sqlcmd
        dt.Rows.Clear()
        sqlad.Fill(dt)


        If dt.Rows(0)("jd_state") <> 0 Then
            MsgBox("�õ����Ѿ��ӵ������ɽ�����˽᰸������������", MsgBoxStyle.OkOnly, "��ʾ")
            Exit Sub
        End If



        If dt.Rows(0)("state") = 2 Then
            MsgBox("�õ����Ѿ��᰸������������", MsgBoxStyle.OkOnly, "��ʾ")
            Exit Sub
        ElseIf dt.Rows(0)("state") = 1 Then
            MsgBox("�õ����Ѿ���ˣ�����������", MsgBoxStyle.OkOnly, "��ʾ")
            Exit Sub
        End If

        sql = "update  scd_master set state=1,check_date=getdate(),check_man='" & rs_name & "' where billno='" & Me.ExDataGridView1.Rows(Me.ExDataGridView1.SelectedCells(0).RowIndex).Cells("billno").Value & "'"
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
        Dim sqlad As New SqlDataAdapter

        Dim dt As New DataTable
        Dim sql As String


        If Me.ExDataGridView1.SelectedCells.Count = 0 Then
            MsgBox("��ѡ��Ҫ����ĵ��ݣ�", MsgBoxStyle.OkOnly, "��ʾ")
            Exit Sub
        End If

        sqlcon = getconn()
        sqlcmd.Connection = sqlcon

        sql = "SELECT state,jd_state " & _
               "FROM scd_master where billno='" & Me.ExDataGridView1.Rows(Me.ExDataGridView1.SelectedCells(0).RowIndex).Cells("billno").Value & "'"

        sqlcmd.CommandText = sql
        sqlad.SelectCommand = sqlcmd
        dt.Rows.Clear()
        sqlad.Fill(dt)


        If dt.Rows(0)("jd_state") <> 0 Then
            MsgBox("�õ����Ѿ��ӵ������ɽ�����˽᰸������������", MsgBoxStyle.OkOnly, "��ʾ")
            Exit Sub
        End If

        If dt.Rows(0)("state") = 2 Then
            MsgBox("�õ����Ѿ��᰸������������", MsgBoxStyle.OkOnly, "��ʾ")
            Exit Sub
        ElseIf dt.Rows(0)("state") = 0 Then
            MsgBox("�õ���״̬Ϊ�½�������������", MsgBoxStyle.OkOnly, "��ʾ")
            Exit Sub
        End If




        sql = "update  scd_master set state=0,check_date=getdate(),check_man='" & rs_name & "' where billno='" & Me.ExDataGridView1.Rows(Me.ExDataGridView1.SelectedCells(0).RowIndex).Cells("billno").Value & "'"
        sqlcmd.CommandText = sql
        sqlcmd.ExecuteNonQuery()

        'Refresh_ExDataGridView(Me.TreeView1.SelectedNode.Text)
        Me.ExDataGridView1.Rows(Me.ExDataGridView1.SelectedCells(0).RowIndex).Cells("state1").Value = "�½�"

        Me.ExDataGridView1.Rows(Me.ExDataGridView1.SelectedCells(0).RowIndex).Cells("check_man").Value = rs_name
        Me.ExDataGridView1.Rows(Me.ExDataGridView1.SelectedCells(0).RowIndex).Cells("check_date").Value = Now
        MsgBox("����˳ɹ���", MsgBoxStyle.OkOnly, "��ʾ")
    End Sub

    Private Sub �᰸ToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles �᰸ToolStripMenuItem.Click
        Dim sqlcon As New SqlConnection
        Dim sqlcmd As New SqlCommand
        Dim sqlad As New SqlDataAdapter

        Dim dt As New DataTable
        Dim sql As String


        If Me.ExDataGridView1.SelectedCells.Count = 0 Then
            MsgBox("��ѡ��Ҫ�᰸�ĵ��ݣ�", MsgBoxStyle.OkOnly, "��ʾ")
            Exit Sub
        End If


        sqlcon = getconn()
        sqlcmd.Connection = sqlcon

        sql = "SELECT state,jd_state " & _
               "FROM scd_master where billno='" & Me.ExDataGridView1.Rows(Me.ExDataGridView1.SelectedCells(0).RowIndex).Cells("billno").Value & "'"

        sqlcmd.CommandText = sql
        sqlad.SelectCommand = sqlcmd
        dt.Rows.Clear()
        sqlad.Fill(dt)


        If dt.Rows(0)("state") <> 0 Then
            MsgBox("�õ����Ѿ��ӵ������ɽ�����˽᰸������������", MsgBoxStyle.OkOnly, "��ʾ")
            Exit Sub
        End If

        If dt.Rows(0)("state") = 2 Then
            MsgBox("�õ����Ѿ��᰸������������", MsgBoxStyle.OkOnly, "��ʾ")
            Exit Sub
        End If

        sql = "update  scd_master set state=2,check_date=getdate(),check_man='" & rs_name & "' where billno='" & Me.ExDataGridView1.Rows(Me.ExDataGridView1.SelectedCells(0).RowIndex).Cells("billno").Value & "'"
        sqlcmd.CommandText = sql
        sqlcmd.ExecuteNonQuery()

        'Refresh_ExDataGridView(Me.TreeView1.SelectedNode.Text)
        Me.ExDataGridView1.Rows(Me.ExDataGridView1.SelectedCells(0).RowIndex).Cells("state1").Value = "�᰸"

        Me.ExDataGridView1.Rows(Me.ExDataGridView1.SelectedCells(0).RowIndex).Cells("check_man").Value = rs_name
        Me.ExDataGridView1.Rows(Me.ExDataGridView1.SelectedCells(0).RowIndex).Cells("check_date").Value = Now
        MsgBox("�᰸�ɹ���", MsgBoxStyle.OkOnly, "��ʾ")
    End Sub

    Private Sub ���᰸ToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ���᰸ToolStripMenuItem.Click
        Dim sqlcon As New SqlConnection
        Dim sqlcmd As New SqlCommand
        Dim sqlad As New SqlDataAdapter

        Dim dt As New DataTable
        Dim sql As String


        If Me.ExDataGridView1.SelectedCells.Count = 0 Then
            MsgBox("��ѡ��Ҫ���᰸�ĵ��ݣ�", MsgBoxStyle.OkOnly, "��ʾ")
            Exit Sub
        End If

        sqlcon = getconn()
        sqlcmd.Connection = sqlcon

        sql = "SELECT state,jd_state " & _
               "FROM scd_master where billno='" & Me.ExDataGridView1.Rows(Me.ExDataGridView1.SelectedCells(0).RowIndex).Cells("billno").Value & "'"

        sqlcmd.CommandText = sql
        sqlad.SelectCommand = sqlcmd
        dt.Rows.Clear()
        sqlad.Fill(dt)


        If dt.Rows(0)("jd_state") <> 0 Then
            MsgBox("�õ����Ѿ��ӵ������ɽ�����˽᰸������������", MsgBoxStyle.OkOnly, "��ʾ")
            Exit Sub
        End If

        If dt.Rows(0)("state") = 1 Then
            MsgBox("�õ���״̬Ϊ��ˣ�����������", MsgBoxStyle.OkOnly, "��ʾ")
            Exit Sub
        ElseIf dt.Rows(0)("state") = 0 Then
            MsgBox("�õ���״̬Ϊ�½�������������", MsgBoxStyle.OkOnly, "��ʾ")
            Exit Sub
        End If

        sql = "update  scd_master set state=1,check_date=getdate(),check_man='" & rs_name & "' where billno='" & Me.ExDataGridView1.Rows(Me.ExDataGridView1.SelectedCells(0).RowIndex).Cells("billno").Value & "'"
        sqlcmd.CommandText = sql
        sqlcmd.ExecuteNonQuery()

        'Refresh_ExDataGridView(Me.TreeView1.SelectedNode.Text)

        Me.ExDataGridView1.Rows(Me.ExDataGridView1.SelectedCells(0).RowIndex).Cells("state1").Value = "���"
        Me.ExDataGridView1.Rows(Me.ExDataGridView1.SelectedCells(0).RowIndex).Cells("check_man").Value = rs_name
        Me.ExDataGridView1.Rows(Me.ExDataGridView1.SelectedCells(0).RowIndex).Cells("check_date").Value = Now
        MsgBox("���᰸�ɹ���", MsgBoxStyle.OkOnly, "��ʾ")
    End Sub
End Class