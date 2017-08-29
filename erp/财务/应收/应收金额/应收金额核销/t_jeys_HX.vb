Imports System.Data.SqlClient
Public Class t_jeys_HX

    Private Sub ToolStripButton_refresh_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripButton_refresh.Click
        Refresh_ExDataGridView_master()
    End Sub

    Private Sub ToolStripButton_exit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripButton_exit.Click
        Me.Close()
    End Sub




    Private Sub Refresh_ExDataGridView_master(Optional ByVal s As String = "")
        Dim sqlcon As New SqlConnection
        Dim sqlcmd As New SqlCommand
        Dim sqlad As New SqlDataAdapter


        Dim dt As New DataTable
        Dim sql As String



        sqlcon = getconn()
        sqlcmd.Connection = sqlcon

       




        sql = "select a.*,case state when 0 then '�½�' when 1 then '���' when 2 then '�᰸' end as state1, case hx when 0 then 'δ����' when 1 then '�Ѻ���' end as hx1   " & _
                "from t_skys_master a  " & _
                " where a.state=1 and hx=0 " & s & "  order by a.billno "

        sqlcmd.CommandText = sql
        sqlad.SelectCommand = sqlcmd
        dt.Rows.Clear()
        sqlad.Fill(dt)

        Me.ExDataGridView1.SetDataSource(dt)
        Me.ExDataGridView1.Columns.Clear()

        'Me.ExDataGridView1.AddColumn("check", "", 20, False, New DataGridViewCheckBoxColumn)
        Me.ExDataGridView1.AddColumn("billno", "Ӧ��Ʊ�ݺ�", 120)


        Me.ExDataGridView1.AddColumn("memo", "˵��")


        Me.ExDataGridView1.AddColumn("state1", "״̬")


        Me.ExDataGridView1.AddColumn("HX1", "����״̬")
        Me.ExDataGridView1.AddColumn("HX_man", "������")
        Me.ExDataGridView1.AddColumn("hx_date", "��������", , , , , "yyyy-MM-dd")

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


        sqlcon = getconn()


        'sql = "select * from  t_product_FY_detail where fy_id='" & Me.ExDataGridView1.Rows(index).Cells("id").Value & "' order by id"
        If index <> -1 Then
            sql = "select a.*,b.project_no,b.project_name,b.project_money,b.SK_money,b.effective_date,b.memo " & _
                    "from t_skys_detail a left join  " & _
                    "	project_master b on a.project_no=b.project_no " & _
                    "  where a.billno='" & Me.ExDataGridView1.Rows(index).Cells("billno").Value & "' order by a.id "
        Else
            sql = "select a.*,b.project_no,b.project_name,b.project_money,b.SK_money,b.effective_date,b.memo " & _
                    "from t_skys_detail a left join  " & _
                    "	project_master b on a.project_no=b.project_no " & _
                    "  where a.id=-1  order by a.id "
        End If
        sqlcmd.Connection = sqlcon
        sqlcmd.CommandText = sql
        sqlad.SelectCommand = sqlcmd
        dt.Rows.Clear()
        sqlad.Fill(dt)

        Me.ExDataGridView2.SetDataSource(dt)
        Me.ExDataGridView2.Columns.Clear()




        Me.ExDataGridView2.AddColumn("project_no", "���̺�", 120)
        Me.ExDataGridView2.AddColumn("project_name", "��������")
        Me.ExDataGridView2.AddColumn("project_money", "���̽��", , , , , "#0")
        Me.ExDataGridView2.AddColumn("money", "�տ���", , , , , "#0")


        Me.ExDataGridView2.AddColumn("effective_date", "��Ч����", , , , , "yyyy-MM-dd")


        Me.ExDataGridView2.AddColumn("memo", "��ע")

        Me.ExDataGridView2.AddColumn("id", "id", , , , , , False)
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

    Private Sub ToolStripButton_save_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripButton_save.Click
        Dim sqlcon As New SqlConnection
        Dim sqlcmd As New SqlCommand
        Dim sql As String = "'"




        If Me.ExDataGridView1.SelectedCells.Count < 0 Then
            MsgBox("��ѡ��Ҫ�޸ĵ��У�", MsgBoxStyle.OkOnly, "��ʾ")
            Exit Sub
        End If

        If MsgBox("ȷ��Ҫ�������ݺ�Ϊ:" & Me.ExDataGridView1.Rows(Me.ExDataGridView1.SelectedCells(0).RowIndex).Cells("billno").Value & "�ĵ�����", MsgBoxStyle.YesNo, "��ʾ") = MsgBoxResult.No Then
            Exit Sub
        End If

        sqlcon = getconn()
        sqlcmd.Connection = sqlcon



        sql = "select count(*) from t_SKys_master where state=1 and billno='" & Me.ExDataGridView1.Rows(Me.ExDataGridView1.SelectedCells(0).RowIndex).Cells("billno").Value & "' "

        sqlcmd.CommandText = sql
        If sqlcmd.ExecuteScalar() = 0 Then
            MsgBox("�ü�¼״̬������ˣ���ˢ�º����ԣ�", MsgBoxStyle.OkOnly, "��ʾ")
            Exit Sub
        End If






        sql = "update T_SKYS_master set hx=1,hx_man='" & rs_name & "',hx_date=getdate() where billno='" & Me.ExDataGridView1.Rows(Me.ExDataGridView1.SelectedCells(0).RowIndex).Cells("billno").Value & "' "
        sqlcmd.CommandText = sql
        sqlcmd.ExecuteNonQuery()



        MsgBox("����ɹ���", MsgBoxStyle.OkOnly, "��ʾ")
        Refresh_ExDataGridView_master()
    End Sub
End Class