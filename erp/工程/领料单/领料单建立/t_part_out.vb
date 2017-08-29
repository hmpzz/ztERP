Imports System.Data.SqlClient
Public Class t_part_out

    Private Sub ToolStripButton_exit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripButton_exit.Click
        Me.Close()
    End Sub

    Private Sub ToolStripButton_refresh_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripButton_refresh.Click
        Refresh_ExDataGridView_master()
    End Sub


    Friend Sub Refresh_ExDataGridView_master(Optional ByVal s As String = "")
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


        If Me.CheckBox3.Checked Then
            s1 = s1 & " a.state=2 or "
        End If

        If s1.Length <> 0 Then
            s1 = Microsoft.VisualBasic.Left(s1, s1.Length - 3)
            s1 = " and (" & s1 & ")"
        End If

        If Me.CheckBox4.Checked Then
            s1 = s1 & " and a.billno in (select distinct b.billno  " & _
                        "from  T_part_out_master a left join  " & _
                        "	T_part_out_detail b on a.billno=b.billno " & _
                        "where b.ok_amount<b.amount and a.state=1) "
        End If


        If BM_id = 49 Or BM_id = 78 Then
            sql = "SELECT a.*,case a.state when 0 then '�½�' when 1 then '���' when 2 then '�᰸' end as state1,b.project_name,c.depot_name " & _
    "                FROM T_part_out_master a  left join   " & _
    "                	  	project_master b on a.project_no=b.project_no  left join  " & _
    "                       t_depot c on a.depot_code=c.depot_code " & _
    "                where 1=1 " & s1 & s & " order by a.billno"
        Else
            sql = "SELECT a.*,case a.state when 0 then '�½�' when 1 then '���' when 2 then '�᰸' end as state1,b.project_name,c.depot_name " & _
    "                FROM T_part_out_master a  left join   " & _
    "                	  	project_master b on a.project_no=b.project_no  left join  " & _
    "                       t_depot c on a.depot_code=c.depot_code left join  " & _
                       " t_rs on a.input_man=t_rs.rs_name " & _
    "                where t_rs.bm_id= " & BM_id & s1 & s & " order by a.billno"
        End If


        sqlcmd.CommandText = sql
        sqlad.SelectCommand = sqlcmd
        dt.Rows.Clear()
        sqlad.Fill(dt)

        Me.ExDataGridView1.SetDataSource(dt)
        Me.ExDataGridView1.Columns.Clear()

        'Me.ExDataGridView1.AddColumn("check", "", 20, False, New DataGridViewCheckBoxColumn)
        Me.ExDataGridView1.AddColumn("billno", "���ݺ�", 120)
        Me.ExDataGridView1.AddColumn("project_no", "���̺�")
        Me.ExDataGridView1.AddColumn("project_name", "��������")
        Me.ExDataGridView1.AddColumn("depot_name", "���ϲֿ�")
        Me.ExDataGridView1.AddColumn("state1", "״̬")
        Me.ExDataGridView1.AddColumn("memo", "˵��")

        Me.ExDataGridView1.AddColumn("ll_man", "������")

        Me.ExDataGridView1.AddColumn("input_man", "¼����")
        Me.ExDataGridView1.AddColumn("input_date", "¼������", , , , , "yyyy-MM-dd")
        Me.ExDataGridView1.AddColumn("check_man", "�����")
        Me.ExDataGridView1.AddColumn("check_date", "�������", , , , , "yyyy-MM-dd")
      
        Me.ExDataGridView1.AddColumn("id", "id", , , , , , False)


        If Me.ExDataGridView1.Rows.Count > 0 Then
            cellclick(0)
        Else
            cellclick(-1)
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


        'sql = "select * from  t_product_FY_detail where fy_id='" & Me.ExDataGridView1.Rows(index).Cells("id").Value & "' order by id"
        If index <> -1 Then

            sql = " SELECT a.*,b.item_name,b.item_standard,b.unit " & _
"                        FROM t_part_out_detail a left join   " & _
"                        	t_item b on a.item_code=b.item_code    " & _
"                           where a.billno='" & Me.ExDataGridView1.Rows(index).Cells("billno").Value & "' order by a.id"

        Else
            sql = " SELECT a.*,b.item_name,b.item_standard,b.unit " & _
"                        FROM t_part_out_detail a left join   " & _
"                        	t_item b on a.item_code=b.item_code    " & _
"                           where a.id=-1"
        End If
        sqlcmd.Connection = sqlcon
        sqlcmd.CommandText = sql
        sqlad.SelectCommand = sqlcmd
        dt.Rows.Clear()
        sqlad.Fill(dt)

        Me.ExDataGridView2.SetDataSource(dt)
        Me.ExDataGridView2.Columns.Clear()



        Me.ExDataGridView2.AddColumn("item_code", "���ϱ���")
        Me.ExDataGridView2.AddColumn("item_name", "��������")
        Me.ExDataGridView2.AddColumn("item_standard", "���")
        Me.ExDataGridView2.AddColumn("unit", "��λ")

        Me.ExDataGridView2.AddColumn("amount", "����", , , , DataGridViewContentAlignment.MiddleRight, "#0.0000")
        Me.ExDataGridView2.AddColumn("OK_amount", "�ѳ�������", 120, , , DataGridViewContentAlignment.MiddleRight, "#0.0000")


        Me.ExDataGridView2.AddColumn("item_code", "item_code", , , , , , False)


        Me.ExDataGridView2.AddColumn("id", "id", , , , , , False)
    End Sub

    Private Sub ToolStripButton_add_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripButton_add.Click
        Dim t_part_out_add As New t_part_out_add
        t_part_out_add.Owner = Me
        t_part_out_add.state = "new"
        t_part_out_add.billno = ""

        t_part_out_add.ShowDialog()
        If t_part_out_add.DialogResult = Windows.Forms.DialogResult.OK Then
            Refresh_ExDataGridView_master()
        End If
    End Sub

    Private Sub ToolStripButton_mod_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripButton_mod.Click
        Dim sqlcon As New SqlConnection
        Dim sqlcmd As New SqlCommand
        Dim sqlad As New SqlDataAdapter
        Dim dt As New DataTable

        Dim sql As String

        sqlcon = getconn()
        sqlcmd.Connection = sqlcon

        sql = "select  count(*) from T_part_out_master where state=0 and billno='" & Me.ExDataGridView1.Rows(Me.ExDataGridView1.SelectedCells(0).RowIndex).Cells("billno").Value & "'"
        sqlcmd.CommandText = sql


        If sqlcmd.ExecuteScalar = 0 Then
            MsgBox("�õ��ݲ����½�״̬���������޸ģ�", MsgBoxStyle.OkOnly, "��ʾ")
            Exit Sub
        End If

        sql = "select  count(*) from T_part_out_detail where ok_amount<>0 and billno='" & Me.ExDataGridView1.Rows(Me.ExDataGridView1.SelectedCells(0).RowIndex).Cells("billno").Value & "'"
        sqlcmd.CommandText = sql

        If sqlcmd.ExecuteScalar > 0 Then
            MsgBox("�õ����Ѿ��г�����Ϣ���������޸ģ�", MsgBoxStyle.OkOnly, "��ʾ")
            Exit Sub
        End If


        Dim t_part_out_add As New t_part_out_add
        t_part_out_add.Owner = Me
        t_part_out_add.state = "mod"
        t_part_out_add.billno = Me.ExDataGridView1.Rows(Me.ExDataGridView1.SelectedCells(0).RowIndex).Cells("billno").Value



        t_part_out_add.ShowDialog()
        If t_part_out_add.DialogResult = Windows.Forms.DialogResult.OK Then
            cellclick(Me.ExDataGridView1.SelectedCells(0).RowIndex)

        End If
    End Sub

    Private Sub ExDataGridView1_CellClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles ExDataGridView1.CellClick
        If e.RowIndex < 0 Then
            Exit Sub
        Else
            cellclick(e.RowIndex)
        End If
    End Sub

    Private Sub ExDataGridView1_CellContentClick(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles ExDataGridView1.CellContentClick

    End Sub

    Private Sub ToolStripButton_del_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripButton_del.Click

        Dim sqlcon As New SqlConnection
        Dim sqlcmd As New SqlCommand
        Dim sqlad As New SqlDataAdapter
        Dim mytrans As SqlTransaction

        Dim dt As New DataTable
        Dim sql As String
        Dim i As Int32 = 0


        If Me.ExDataGridView1.SelectedCells.Count < 0 Then
            MsgBox("��ѡ��Ҫ�޸ĵ��У�", MsgBoxStyle.OkOnly, "��ʾ")
            Exit Sub
        End If

        If MsgBox("ȷ��Ҫɾ�����ϵ���Ϊ��" & Me.ExDataGridView1.Rows(Me.ExDataGridView1.SelectedCells(0).RowIndex).Cells("billno").Value & "�ĵ�����", MsgBoxStyle.YesNo, "��ʾ") = MsgBoxResult.No Then
            Exit Sub
        End If



        sqlcon = getconn()
        sqlcmd.Connection = sqlcon

        mytrans = sqlcon.BeginTransaction

        sqlcmd.Transaction = mytrans

        Try




            sql = "SELECT count(*)  " & _
                           "FROM T_part_out_master  where state<>0 and billno='" & Me.ExDataGridView1.Rows(Me.ExDataGridView1.SelectedCells(0).RowIndex).Cells("billno").Value & "' "


            sqlcmd.CommandText = sql
            If sqlcmd.ExecuteScalar <> 0 Then
                Throw New System.Exception("�˵��ݲ����½�״̬������ɾ����")
            End If



            sql = "delete from T_part_out_master where billno='" & Me.ExDataGridView1.Rows(Me.ExDataGridView1.SelectedCells(0).RowIndex).Cells("billno").Value & "' "
            sqlcmd.CommandText = sql
            sqlcmd.ExecuteNonQuery()

            sql = "delete from T_part_out_detail where billno='" & Me.ExDataGridView1.Rows(Me.ExDataGridView1.SelectedCells(0).RowIndex).Cells("billno").Value & "' "
            sqlcmd.CommandText = sql
            sqlcmd.ExecuteNonQuery()

            mytrans.Commit()


        Catch ex As Exception
            mytrans.Rollback()
            MsgBox(ex.Message, MsgBoxStyle.OkOnly, "����")
            Exit Sub
        End Try

        MsgBox("ɾ���ɹ���", MsgBoxStyle.OkOnly, "��ʾ")
        Me.Refresh_ExDataGridView_master()
    End Sub

    Private Sub ToolStripButton1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripButton1.Click

    End Sub
End Class