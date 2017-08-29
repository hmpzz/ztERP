Imports System.Data.SqlClient
Public Class YKD_check

    Private Sub ToolStripButton_exit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripButton_exit.Click
        Me.Close()
    End Sub

    Private Sub ToolStripButton_refresh_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripButton_refresh.Click
        Refresh_TreeView()
    End Sub


    Private Sub Refresh_TreeView(Optional ByVal s As String = "")
        Dim sqlcon As New SqlConnection
        Dim sqlcmd As New SqlCommand
        Dim sqlad As New SqlDataAdapter

        Dim dt As New DataTable
        Dim sql As String
        Dim i As Integer
        Dim s1 As String = ""

        sqlcon = getconn()

        sqlcmd.Connection = sqlcon

        If Me.CheckBox1.Checked Then
            s1 = s1 & " a.state=0 or"
        End If

        If Me.CheckBox2.Checked Then
            s1 = s1 & " a.state=1 or"
        End If

        If Me.CheckBox3.Checked Then
            s1 = s1 & " a.state=2 or"
        End If

        If s1 = "" Then
            MsgBox("��ѡ�񵥾�״̬��", MsgBoxStyle.OkOnly, "��ʾ")
            Exit Sub
        End If


        s1 = "(" & s1.Substring(0, s1.Length - 2) & ") "

        s1 = " source=0 and  a.input_date >='" & Format(Me.DateTimePicker1.Value, "yyyy-MM-dd") & " 00:00:00' and  a.input_date <='" & Format(Me.DateTimePicker2.Value, "yyyy-MM-dd") & " 23:59:59' and " & s1 & s

        sql = "select distinct a.billno from B_TransferBill_master a left join B_TransferBill_detail b on a.billno=b.billno where " & s1 & " order by a.billno"

        sqlcmd.CommandText = sql
        sqlad.SelectCommand = sqlcmd

        dt.Clear()
        sqlad.Fill(dt)

        Me.TreeView1.Nodes.Clear()

        Dim Tnode As TreeNode
        Dim Dnode As TreeNode = Nothing


        Me.TreeView1.Nodes.Clear()
        Tnode = Me.TreeView1.Nodes.Add("all", "�ƿⵥ", 0, 1)


        For i = 0 To dt.Rows.Count - 1

            Dnode = Tnode.Nodes.Add("D" & dt.Rows(i)("billno"), dt.Rows(i)("billno"), 2, 2)

        Next

        Me.TreeView1.Nodes(0).Expand()

        'Me.ExDataGridView1.DataSource = Nothing
        'Me.ExDataGridView1.DataMember = Nothing
        Me.ExDataGridView1.ClearDSDM()

        'Me.ExDataGridView2.DataSource = Nothing
        'Me.ExDataGridView2.DataMember = Nothing
        Me.ExDataGridView2.ClearDSDM()
    End Sub

    Private Sub TreeView1_AfterSelect(ByVal sender As System.Object, ByVal e As System.Windows.Forms.TreeViewEventArgs) Handles TreeView1.AfterSelect

    End Sub

    Private Sub TreeView1_NodeMouseClick(ByVal sender As Object, ByVal e As System.Windows.Forms.TreeNodeMouseClickEventArgs) Handles TreeView1.NodeMouseClick
        If e.Node.Name.Substring(0, 1).ToUpper <> "D" Then
            Me.ExDataGridView1.Columns.Clear()
            'Me.ExDataGridView1.DataSource = Nothing
            'Me.ExDataGridView1.DataMember = Nothing
            Me.ExDataGridView1.ClearDSDM()

            Me.ExDataGridView2.Columns.Clear()
            'Me.ExDataGridView2.DataSource = Nothing
            'Me.ExDataGridView2.DataMember = Nothing
            Me.ExDataGridView2.ClearDSDM()
            Exit Sub

        End If

        Refresh_ExDataGridView(e.Node.Text)
        Refresh_ExDataGridView2()
    End Sub

    Private Sub Refresh_ExDataGridView(ByVal billno As String)
        Dim sqlcon As New SqlConnection
        Dim sqlcmd As New SqlCommand
        Dim sqlad As New SqlDataAdapter

        Dim dt As New DataTable
        Dim sql As String

        sqlcon = getconn()



        sql = "SELECT a.*,b.depot_name as CK_depot,c.depot_name as RK_depot,case a.state when 0 then '�½�' when 1 then '���' when 2 then '�᰸' end as check_state " & _
                "FROM B_TransferBill_master a left join  " & _
                "	t_depot b on a.source_depot=b.depot_code left join  " & _
                "	t_depot c on a.aim_depot=c.depot_code where a.billno='" & billno & "'"


        sqlcmd.Connection = sqlcon
        sqlcmd.CommandText = sql
        sqlad.SelectCommand = sqlcmd
        dt.Clear()
        sqlad.Fill(dt)


        Me.ExDataGridView1.SetDataSource(dt)

        Me.ExDataGridView1.AddColumn("billno", "���ݺ�", 120)
        Me.ExDataGridView1.AddColumn("CK_depot", "����ֿ�")
        Me.ExDataGridView1.AddColumn("RK_depot", "���ֿ�")
        Me.ExDataGridView1.AddColumn("input_man", "¼����")
        Me.ExDataGridView1.AddColumn("input_date", "¼������", , , , , "yyyy-MM-dd")
        Me.ExDataGridView1.AddColumn("check_state", "���״̬")
        Me.ExDataGridView1.AddColumn("check_man", "�����")
        Me.ExDataGridView1.AddColumn("check_date", "�������", , , , , "yyyy-MM-dd")

        Me.ExDataGridView1.AddColumn("memo", "˵��")

        Me.ExDataGridView1.AddColumn("id", "id", , , , , , False)

    End Sub

    Private Sub ExDataGridView1_CellClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles ExDataGridView1.CellClick
        Refresh_ExDataGridView2()
    End Sub


    Private Sub Refresh_ExDataGridView2()
        Dim sqlcon As New SqlConnection
        Dim sqlcmd As New SqlCommand
        Dim sqlad As New SqlDataAdapter

        Dim dt As New DataTable
        Dim sql As String

        sqlcon = getconn()


        sql = "SELECT a.*,b.item_name,b.item_standard,b.unit  " & _
              "from B_TransferBill_detail a left join " & _
               "	t_item b on a.item_code=b.item_code  " & _
               "where a.billno='" & Me.ExDataGridView1.Rows(Me.ExDataGridView1.SelectedCells(0).RowIndex).Cells("billno").Value & "'"

        sqlcmd.Connection = sqlcon
        sqlcmd.CommandText = sql
        sqlad.SelectCommand = sqlcmd
        dt.Clear()
        sqlad.Fill(dt)


        Me.ExDataGridView2.SetDataSource(dt)

        Me.ExDataGridView2.AddColumn("item_code", "��������", 120)
        Me.ExDataGridView2.AddColumn("item_name", "���ϱ���")
        Me.ExDataGridView2.AddColumn("item_standard", "���")
        Me.ExDataGridView2.AddColumn("unit", "��λ")

        Me.ExDataGridView2.AddColumn("amount", "�ƿⵥ����", 100, , , DataGridViewContentAlignment.MiddleRight, "#0.000")
        Me.ExDataGridView2.AddColumn("ck_amount", "�ѳ�������", 100, , , DataGridViewContentAlignment.MiddleRight, "#0.000")
        Me.ExDataGridView2.AddColumn("Rk_amount", "���������", 100, , , DataGridViewContentAlignment.MiddleRight, "#0.000")


        Me.ExDataGridView2.AddColumn("id", "id", , , , , , False)
    End Sub

    Private Sub ���ToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ���ToolStripMenuItem.Click
        Dim sqlcon As New SqlConnection
        Dim sqlcmd As New SqlCommand

        Dim sql As String
        Dim k As Int32

        sqlcon = getconn()
        sqlcmd.Connection = sqlcon

        sql = "SELECT state " & _
                "FROM B_TransferBill_master where billno='" & Me.ExDataGridView1.Rows(Me.ExDataGridView1.SelectedCells(0).RowIndex).Cells("billno").Value & "'"


        sqlcmd.CommandText = sql
        k = sqlcmd.ExecuteScalar

        If k = 2 Then
            MsgBox("�õ����Ѿ��᰸������������", MsgBoxStyle.OkOnly, "��ʾ")
            Exit Sub
        ElseIf k = 1 Then
            MsgBox("�õ����Ѿ���ˣ�����������", MsgBoxStyle.OkOnly, "��ʾ")
            Exit Sub
        End If

        sql = "update  B_TransferBill_master set state=1,check_date=getdate(),check_man='" & rs_name & "' where billno='" & Me.ExDataGridView1.Rows(Me.ExDataGridView1.SelectedCells(0).RowIndex).Cells("billno").Value & "'"
        sqlcmd.CommandText = sql
        sqlcmd.ExecuteNonQuery()

        'Refresh_ExDataGridView(Me.TreeView1.SelectedNode.Text)
        Me.ExDataGridView1.Rows(Me.ExDataGridView1.SelectedCells(0).RowIndex).Cells("check_state").Value = "���"

        MsgBox("��˳ɹ���", MsgBoxStyle.OkOnly, "��ʾ")
    End Sub

    Private Sub ����ToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ����ToolStripMenuItem.Click
        Dim sqlcon As New SqlConnection
        Dim sqlcmd As New SqlCommand

        Dim sql As String
        Dim k As Int32

        sqlcon = getconn()
        sqlcmd.Connection = sqlcon

        sql = "SELECT state " & _
                "FROM B_TransferBill_master where billno='" & Me.ExDataGridView1.Rows(Me.ExDataGridView1.SelectedCells(0).RowIndex).Cells("billno").Value & "'"


        sqlcmd.CommandText = sql
        k = sqlcmd.ExecuteScalar

        If k = 2 Then
            MsgBox("�õ����Ѿ��᰸������������", MsgBoxStyle.OkOnly, "��ʾ")
            Exit Sub
        ElseIf k = 0 Then
            MsgBox("�õ���״̬Ϊ�½�������������", MsgBoxStyle.OkOnly, "��ʾ")
            Exit Sub
        End If

        sql = "update  B_TransferBill_master set state=0 where billno='" & Me.ExDataGridView1.Rows(Me.ExDataGridView1.SelectedCells(0).RowIndex).Cells("billno").Value & "'"
        sqlcmd.CommandText = sql
        sqlcmd.ExecuteNonQuery()

        'Refresh_ExDataGridView(Me.TreeView1.SelectedNode.Text)
        Me.ExDataGridView1.Rows(Me.ExDataGridView1.SelectedCells(0).RowIndex).Cells("check_state").Value = "�½�"
        MsgBox("����˳ɹ���", MsgBoxStyle.OkOnly, "��ʾ")
    End Sub

    Private Sub �᰸ToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles �᰸ToolStripMenuItem.Click
        Dim sqlcon As New SqlConnection
        Dim sqlcmd As New SqlCommand

        Dim sql As String
        Dim k As int32

        sqlcon = getconn()
        sqlcmd.Connection = sqlcon

        sql = "SELECT state " & _
                "FROM B_TransferBill_master where billno='" & Me.ExDataGridView1.Rows(Me.ExDataGridView1.SelectedCells(0).RowIndex).Cells("billno").Value & "'"


        sqlcmd.CommandText = sql
        k = sqlcmd.ExecuteScalar

        If k = 2 Then
            MsgBox("�õ����Ѿ��᰸������������", MsgBoxStyle.OkOnly, "��ʾ")
            Exit Sub
        End If

        sql = "update  B_TransferBill_master set state=2 where billno='" & Me.ExDataGridView1.Rows(Me.ExDataGridView1.SelectedCells(0).RowIndex).Cells("billno").Value & "'"
        sqlcmd.CommandText = sql
        sqlcmd.ExecuteNonQuery()

        'Refresh_ExDataGridView(Me.TreeView1.SelectedNode.Text)
        Me.ExDataGridView1.Rows(Me.ExDataGridView1.SelectedCells(0).RowIndex).Cells("check_state").Value = "�᰸"
        MsgBox("�᰸�ɹ���", MsgBoxStyle.OkOnly, "��ʾ")
    End Sub

    Private Sub ���᰸ToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ���᰸ToolStripMenuItem.Click
        Dim sqlcon As New SqlConnection
        Dim sqlcmd As New SqlCommand

        Dim sql As String
        Dim k As int32

        sqlcon = getconn()
        sqlcmd.Connection = sqlcon

        sql = "SELECT state " & _
                "FROM B_TransferBill_master where billno='" & Me.ExDataGridView1.Rows(Me.ExDataGridView1.SelectedCells(0).RowIndex).Cells("billno").Value & "'"


        sqlcmd.CommandText = sql
        k = sqlcmd.ExecuteScalar

        If k = 1 Then
            MsgBox("�õ���״̬Ϊ��ˣ�����������", MsgBoxStyle.OkOnly, "��ʾ")
            Exit Sub
        ElseIf k = 0 Then
            MsgBox("�õ���״̬Ϊ�½�������������", MsgBoxStyle.OkOnly, "��ʾ")
            Exit Sub
        End If

        sql = "update  B_TransferBill_master set state=1,check_date=getdate(),check_man='" & rs_name & "' where billno='" & Me.ExDataGridView1.Rows(Me.ExDataGridView1.SelectedCells(0).RowIndex).Cells("billno").Value & "'"
        sqlcmd.CommandText = sql
        sqlcmd.ExecuteNonQuery()

        'Refresh_ExDataGridView(Me.TreeView1.SelectedNode.Text)

        Me.ExDataGridView1.Rows(Me.ExDataGridView1.SelectedCells(0).RowIndex).Cells("check_state").Value = "���"
        MsgBox("���᰸�ɹ���", MsgBoxStyle.OkOnly, "��ʾ")
    End Sub

    Private Sub ExDataGridView1_CellContentClick(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles ExDataGridView1.CellContentClick

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

        If Me.ExDataGridView1.Rows(e.RowIndex).Cells("check_state").Value = "�½�" Then
            Me.���ToolStripMenuItem.Enabled = True
            Me.����ToolStripMenuItem.Enabled = False
            Me.�᰸ToolStripMenuItem.Enabled = True
            Me.���᰸ToolStripMenuItem.Enabled = False
        ElseIf Me.ExDataGridView1.Rows(e.RowIndex).Cells("check_state").Value = "���" Then
            Me.���ToolStripMenuItem.Enabled = False
            Me.����ToolStripMenuItem.Enabled = True
            Me.�᰸ToolStripMenuItem.Enabled = True
            Me.���᰸ToolStripMenuItem.Enabled = False
        ElseIf Me.ExDataGridView1.Rows(e.RowIndex).Cells("check_state").Value = "�᰸" Then
            Me.���ToolStripMenuItem.Enabled = False
            Me.����ToolStripMenuItem.Enabled = False
            Me.�᰸ToolStripMenuItem.Enabled = False
            Me.���᰸ToolStripMenuItem.Enabled = True
        End If
    End Sub
End Class