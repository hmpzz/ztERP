Imports System.Data.SqlClient
Public Class project_quality

    Private Sub ToolStripButton_refresh_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripButton_refresh.Click
        Refresh_ExDataGridView_master(" and a.project_no like '%" & Me.TextBox1.Text.Trim & "%' ")
    End Sub

    Private Sub ToolStripButton_exit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripButton_exit.Click
        Me.Close()
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


        sql = "SELECT a.*,case state when 0 then '�½�' when 1 then '���' when 2 then '�᰸' end as state1,b.customer_name " & _
                      "FROM project_master a left join " & _
                      " t_customer b on a.customer_no=b.customer_no " & _
                      "where factory_code in (select c.factory_code " & _
                      "                       from sys_user a inner join  " & _
                      "                       	t_user_factory b on a.login_id=b.login_id inner join  " & _
                      "                       	t_factory c on b.factory_code=c.factory_code " & _
                      "                       where a.stop_login=1 and c.stop=1 and a.login_id='" & login_id & "' " & _
                      "                       )  " & s & s1 & s2 & "  order by a.id "
       

        sqlcmd.CommandText = sql
        sqlad.SelectCommand = sqlcmd
        dt.Rows.Clear()
        sqlad.Fill(dt)

        Me.ExDataGridView1.SetDataSource(dt)
        Me.ExDataGridView1.Columns.Clear()

        'Me.ExDataGridView1.AddColumn("check", "", 20, False, New DataGridViewCheckBoxColumn)
        Me.ExDataGridView1.AddColumn("project_no", "���̺�")
        Me.ExDataGridView1.AddColumn("project_name", "��������")
        If price_state = 0 Then
            Me.ExDataGridView1.AddColumn("project_money", "���̽��", , , , , "#0", False)
            Me.ExDataGridView1.AddColumn("FP_money", "��Ʊ���", , , , , "#0", False)
            Me.ExDataGridView1.AddColumn("SK_money", "�տ���", , , , , "#0", False)
        Else
            Me.ExDataGridView1.AddColumn("project_money", "���̽��", , , , , "#0", True)
            Me.ExDataGridView1.AddColumn("FP_money", "��Ʊ���", , , , , "#0", True)
            Me.ExDataGridView1.AddColumn("SK_money", "�տ���", , , , , "#0", True)

        End If

        Me.ExDataGridView1.AddColumn("customer_name", "�ͻ�����")


        

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


        Me.ExDataGridView1.AddColumn("customer_no", "customer_no", , , , , , False)
        Me.ExDataGridView1.AddColumn("id", "id", , , , , , False)

        If Me.ExDataGridView1.Rows.Count > 0 Then
            cellclick(0)
        End If
    End Sub

    Private Sub ExDataGridView1_CellClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles ExDataGridView1.CellClick
        If e.RowIndex < 0 Then
            Exit Sub
        Else
            cellclick(e.RowIndex)
        End If
    End Sub



    Private Sub cellclick(ByVal index As int32)
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


        If price_state = 0 Then
            Me.ExDataGridView2.AddColumn("tz_money", "������", , , , DataGridViewContentAlignment.MiddleRight, "#0", False)
        Else
            Me.ExDataGridView2.AddColumn("tz_money", "������", , , , DataGridViewContentAlignment.MiddleRight, "#0", True)
        End If


        Me.ExDataGridView2.AddColumn("tz_man", "������")
        Me.ExDataGridView2.AddColumn("tz_date", "����ʱ��")

        Me.ExDataGridView2.AddColumn("memo", "˵��")


        Me.ExDataGridView2.AddColumn("id", "id", , , , , , False)
    End Sub

    Private Sub ToolStripButton_show_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripButton_show.Click
        Dim printView As New printView

        Dim CG_RPT As New CG_RPT

        Dim sqlcon As New SqlConnection
        Dim sqlcmd As New SqlCommand


        Dim sql As String



        If Me.ExDataGridView1.SelectedCells.Count = 0 Then
            MsgBox("��ѡ��Ҫ��ӡ�ĵ���", MsgBoxStyle.OkOnly, "��ʾ")
            Exit Sub
        End If

        sqlcon = getconn()
        sqlcmd.Connection = sqlcon

        sql = "select  count(*) from project_master where state=1 and project_no='" & Me.ExDataGridView1.Rows(Me.ExDataGridView1.SelectedCells(0).RowIndex).Cells("project_no").Value & "'"
        sqlcmd.CommandText = sql


        If sqlcmd.ExecuteScalar = 0 Then
            MsgBox("�õ��ݲ������״̬�������Դ�ӡ��", MsgBoxStyle.OkOnly, "��ʾ")
            Exit Sub
        End If

        ShowCG_RPT(CG_RPT, Me.ExDataGridView1.Rows(Me.ExDataGridView1.SelectedCells(0).RowIndex).Cells("project_no").Value)

        printView.CrystalReportViewer1.ReportSource = CG_RPT
        printView.CrystalReportViewer1.RefreshReport()
        printView.ShowDialog()
    End Sub



    Private Sub ShowCG_RPT(ByRef CG_RPT As CG_RPT, ByVal billno As String)
        Dim sql As String
        Dim sqlcon As New SqlConnection
        Dim sqlcmd As New SqlCommand
        Dim sqlad As New SqlDataAdapter
        Dim ds As New DataSet
        Dim i As int32

        sqlcon = getconn()
        sqlcmd.Connection = sqlcon


        ds.Tables.Clear()

        Dim doc As New System.Drawing.Printing.PrintDocument
        Dim rawKind As int32 = 11

        For i = 0 To doc.PrinterSettings.PaperSizes.Count - 1
            If doc.PrinterSettings.PaperSizes(i).PaperName.ToUpper = "ZT" Then
                rawKind = doc.PrinterSettings.PaperSizes(i).RawKind
                Exit For
            End If
        Next


        CG_RPT.PrintOptions.PaperSize = rawKind

        If rawKind = 11 Then
            CG_RPT.PrintOptions.PaperOrientation = CrystalDecisions.Shared.PaperOrientation.Landscape
        Else
            CG_RPT.PrintOptions.PaperOrientation = CrystalDecisions.Shared.PaperOrientation.Portrait
        End If




        ' CType(CrystalDecisions.Shared.PaperSize rawKind)

        sql = "SELECT a.*,b.customer_name,'" & rs_name & "' as print_man,getdate() as print_date " & _
              "FROM project_master a left join " & _
              " t_customer b on a.customer_no=b.customer_no" & _
              " where a.project_no='" & billno & "'"

        sqlcmd.CommandText = sql
        sqlad.SelectCommand = sqlcmd
        sqlad.Fill(ds, "project_master")


        sql = "select top 1 a.*   " & _
                     "from project_detail a " & _
                     " where a.project_no='" & billno & "' order by a.id desc "
        sqlcmd.CommandText = sql
        sqlad.SelectCommand = sqlcmd
        sqlad.Fill(ds, "project_detail")

        CG_RPT.SetDataSource(ds)
        CG_RPT.Refresh()

        save_pdf(CG_RPT, billno)
    End Sub

    Private Sub ToolStripButton_print_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripButton_print.Click

        Dim CG_RPT As New CG_RPT

        Dim sqlcon As New SqlConnection
        Dim sqlcmd As New SqlCommand


        Dim sql As String



        If Me.ExDataGridView1.SelectedCells.Count = 0 Then
            MsgBox("��ѡ��Ҫ��ӡ�ĵ���", MsgBoxStyle.OkOnly, "��ʾ")
            Exit Sub
        End If

        sqlcon = getconn()
        sqlcmd.Connection = sqlcon

        sql = "select  count(*) from project_master where state=1 and project_no='" & Me.ExDataGridView1.Rows(Me.ExDataGridView1.SelectedCells(0).RowIndex).Cells("project_no").Value & "'"
        sqlcmd.CommandText = sql


        If sqlcmd.ExecuteScalar = 0 Then
            MsgBox("�õ��ݲ������״̬�������Դ�ӡ��", MsgBoxStyle.OkOnly, "��ʾ")
            Exit Sub
        End If

        ShowCG_RPT(CG_RPT, Me.ExDataGridView1.Rows(Me.ExDataGridView1.SelectedCells(0).RowIndex).Cells("project_no").Value)
        CG_RPT.PrintToPrinter(1, True, 0, 0)

        MsgBox("��ӡ��ɣ�", MsgBoxStyle.OkOnly, "��ʾ")

    End Sub
End Class