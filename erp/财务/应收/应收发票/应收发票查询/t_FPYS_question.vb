Imports System.Data.SqlClient
Public Class t_FPYS_question

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


        sql = "select a.*,case state when 0 then '�½�' when 1 then '���' when 2 then '�᰸' end as state1, case hx when 0 then 'δ����' when 1 then '�Ѻ���' end as hx1,case kp_state when 0 then 'δ��Ʊ' when 1 then '�ѿ�Ʊ' end as kp_state1  " & _
                "from t_fpys_master a  " & _
                " where 1=1 " & s & s1 & "  order by a.billno "

        sqlcmd.CommandText = sql
        sqlad.SelectCommand = sqlcmd
        dt.Rows.Clear()
        sqlad.Fill(dt)

        Me.ExDataGridView1.SetDataSource(dt)
        Me.ExDataGridView1.Columns.Clear()

        'Me.ExDataGridView1.AddColumn("check", "", 20, False, New DataGridViewCheckBoxColumn)
        Me.ExDataGridView1.AddColumn("billno", "Ӧ��Ʊ�ݺ�", 120)
        Me.ExDataGridView1.AddColumn("FP_NO", "��Ʊ��")

        Me.ExDataGridView1.AddColumn("memo", "˵��")


        Me.ExDataGridView1.AddColumn("state1", "״̬")

        Me.ExDataGridView1.AddColumn("kp_state1", "��Ʊ״̬")
        Me.ExDataGridView1.AddColumn("kp_type", "��Ʊ����")

        Me.ExDataGridView1.AddColumn("HX1", "����״̬")
        Me.ExDataGridView1.AddColumn("HX_man", "������")
        Me.ExDataGridView1.AddColumn("hx_date", "��������", , , , , "yyyy-MM-dd")

        Me.ExDataGridView1.AddColumn("input_man", "¼����")
        Me.ExDataGridView1.AddColumn("input_date", "¼������", , , , , "yyyy-MM-dd")
        Me.ExDataGridView1.AddColumn("check_man", "�����")
        Me.ExDataGridView1.AddColumn("check_date", "�������", , , , , "yyyy-MM-dd")

        Me.ExDataGridView1.AddColumn("customer_no", "customer_no", , , , , , False)


        Me.ExDataGridView1.AddColumn("HW_name1", "HW_name1", , , , , , False)
        Me.ExDataGridView1.AddColumn("HW_unit1", "HW_unit1", , , , , , False)
        Me.ExDataGridView1.AddColumn("HW_price1", "HW_price1", , , , , , False)
        Me.ExDataGridView1.AddColumn("HW_amount1", "HW_amount1", , , , , "#0.00", False)
        Me.ExDataGridView1.AddColumn("HW_money1", "HW_money1", , , , , "#0.00", False)


        Me.ExDataGridView1.AddColumn("HW_name2", "HW_name2", , , , , , False)
        Me.ExDataGridView1.AddColumn("HW_unit2", "HW_unit2", , , , , , False)
        Me.ExDataGridView1.AddColumn("HW_price2", "HW_price2", , , , , , False)
        Me.ExDataGridView1.AddColumn("HW_amount2", "HW_amount2", , , , , "#0.00", False)
        Me.ExDataGridView1.AddColumn("HW_money2", "HW_money2", , , , , "#0.00", False)


        Me.ExDataGridView1.AddColumn("HW_name3", "HW_name3", , , , , , False)
        Me.ExDataGridView1.AddColumn("HW_unit3", "HW_unit3", , , , , , False)
        Me.ExDataGridView1.AddColumn("HW_price3", "HW_price3", , , , , , False)
        Me.ExDataGridView1.AddColumn("HW_amount3", "HW_amount3", , , , , "#0.00", False)
        Me.ExDataGridView1.AddColumn("HW_money3", "HW_money3", , , , , "#0.00", False)


        Me.ExDataGridView1.AddColumn("HW_name4", "HW_name4", , , , , , False)
        Me.ExDataGridView1.AddColumn("HW_unit4", "HW_unit4", , , , , , False)
        Me.ExDataGridView1.AddColumn("HW_price4", "HW_price4", , , , , , False)
        Me.ExDataGridView1.AddColumn("HW_amount4", "HW_amount4", , , , , "#0.00", False)
        Me.ExDataGridView1.AddColumn("HW_money4", "HW_money4", , , , , "#0.00", False)


        Me.ExDataGridView1.AddColumn("HW_name5", "HW_name5", , , , , , False)
        Me.ExDataGridView1.AddColumn("HW_unit5", "HW_unit5", , , , , , False)
        Me.ExDataGridView1.AddColumn("HW_price5", "HW_price5", , , , , , False)
        Me.ExDataGridView1.AddColumn("HW_amount5", "HW_amount5", , , , , "#0.00", False)
        Me.ExDataGridView1.AddColumn("HW_money5", "HW_money5", , , , , "#0.00", False)

        Me.ExDataGridView1.AddColumn("HW_name6", "HW_name6", , , , , , False)
        Me.ExDataGridView1.AddColumn("HW_unit6", "HW_unit6", , , , , , False)
        Me.ExDataGridView1.AddColumn("HW_price6", "HW_price6", , , , , , False)
        Me.ExDataGridView1.AddColumn("HW_amount6", "HW_amount6", , , , , "#0.00", False)
        Me.ExDataGridView1.AddColumn("HW_money6", "HW_money6", , , , , "#0.00", False)

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
            sql = "select a.*,b.project_no,b.project_name,b.project_money,b.fp_money,b.effective_date,b.memo " & _
                    "from t_fpys_detail a left join  " & _
                    "	project_master b on a.project_no=b.project_no " & _
                    "  where a.billno='" & Me.ExDataGridView1.Rows(index).Cells("billno").Value & "' order by a.id "
        Else
            sql = "select a.*,b.project_no,b.project_name,b.project_money,b.fp_money,b.effective_date,b.memo " & _
                    "from t_fpys_detail a left join  " & _
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
        Me.ExDataGridView2.AddColumn("money", "��Ʊ���", , , , , "#0")


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

    Private Sub ToolStripButton1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripButton1.Click
        Dim printView As New printView

        Dim FPYS_RPT As New FPYS_RPT

        Dim sqlcon As New SqlConnection
        Dim sqlcmd As New SqlCommand




        If Me.ExDataGridView1.SelectedCells.Count = 0 Then
            MsgBox("��ѡ��Ҫ��ӡ�ĵ���", MsgBoxStyle.OkOnly, "��ʾ")
            Exit Sub
        End If


        sqlcon = getconn()
        sqlcmd.Connection = sqlcon

        'sql = "select  count(*) from t_outlay_master where state=1 and billno='" & Me.ExDataGridView1.Rows(Me.ExDataGridView1.SelectedCells(0).RowIndex).Cells("billno").Value & "'"
        'sqlcmd.CommandText = sql


        'If sqlcmd.ExecuteScalar = 0 Then
        '    MsgBox("�õ��ݲ������״̬�������Դ�ӡ��", MsgBoxStyle.OkOnly, "��ʾ")
        '    Exit Sub
        'End If





        ShowFPYS_RPT(FPYS_RPT, Me.ExDataGridView1.Rows(Me.ExDataGridView1.SelectedCells(0).RowIndex).Cells("billno").Value)

        printView.CrystalReportViewer1.ReportSource = FPYS_RPT
        printView.CrystalReportViewer1.RefreshReport()
        printView.ShowDialog()
    End Sub


    Public Sub ShowFPYS_RPT(ByRef FPYS_RPT As FPYS_RPT, ByVal billno As String)
        Dim sql As String
        Dim sqlcon As New SqlConnection
        Dim sqlcmd As New SqlCommand
        Dim sqlad As New SqlDataAdapter
        Dim ds As New DataSet


        sqlcon = getconn()
        sqlcmd.Connection = sqlcon


        ds.Tables.Clear()

        Dim doc As New System.Drawing.Printing.PrintDocument
        Dim rawKind As Int32 = 1

        'For i = 0 To doc.PrinterSettings.PaperSizes.Count - 1
        '    If doc.PrinterSettings.PaperSizes(i).PaperName.ToUpper = "ZT" Then
        '        rawKind = doc.PrinterSettings.PaperSizes(i).RawKind
        '        Exit For
        '    End If
        'Next

        'FPYS_RPT.PrintOptions.PaperSize = rawKind

        'If rawKind = 11 Then
        '    FPYS_RPT.PrintOptions.PaperOrientation = CrystalDecisions.Shared.PaperOrientation.Landscape
        'Else
        '    FPYS_RPT.PrintOptions.PaperOrientation = CrystalDecisions.Shared.PaperOrientation.Portrait
        'End If




        ' CType(CrystalDecisions.Shared.PaperSize rawKind)

        sql = "select a.*,case state when 0 then '�½�' when 1 then '���' when 2 then '�᰸' end as state1, case hx when 0 then 'δ����' when 1 then '�Ѻ���' end as hx1,case kp_state when 0 then 'δ��Ʊ' when 1 then '�ѿ�Ʊ' end as kp_state1,a.HW_amount1+a.HW_amount2+a.HW_amount3+a.HW_amount4+a.HW_amount5+a.hw_amount6 as HJ_amount,a.HW_money1+a.HW_money2+a.HW_money3+a.HW_money4+a.HW_money5+a.HW_money6 as HJ_money,'" & rs_name & "' as print_man,getdate() as print_date  " & _
              " ,isnull(b.customer_name,'') as customer_name,isnull(b.address+' ' +b.tel,'') as at,isnull(b.nsr_no,'') as nsr_no,isnull(b.khh_no,'') as khh_no " & _
                "from t_fpys_master a  left join " & _
                "    t_customer b on a.customer_no=b.customer_no " & _
                " where billno='" & billno & "'  order by a.billno "

        sqlcmd.CommandText = sql
        sqlad.SelectCommand = sqlcmd
        sqlad.Fill(ds, "t_fpys_master")


        sql = "select a.*,b.project_no,b.project_name,b.project_money,b.fp_money,b.effective_date,b.memo,b.project_money-b.fp_money as wkp_money " & _
             "from t_fpys_detail a left join  " & _
             "	project_master b on a.project_no=b.project_no " & _
             "  where a.billno='" & billno & "' order by a.id "
        sqlcmd.CommandText = sql
        sqlad.SelectCommand = sqlcmd
        sqlad.Fill(ds, "t_fpys_detail")

        FPYS_RPT.SetDataSource(ds)
        FPYS_RPT.Refresh()

        save_pdf(FPYS_RPT, billno)
    End Sub

    Private Sub ToolStripButton2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripButton2.Click

        Dim FPYS_RPT As New FPYS_RPT

        Dim sqlcon As New SqlConnection
        Dim sqlcmd As New SqlCommand




        If Me.ExDataGridView1.SelectedCells.Count = 0 Then
            MsgBox("��ѡ��Ҫ��ӡ�ĵ���", MsgBoxStyle.OkOnly, "��ʾ")
            Exit Sub
        End If


        sqlcon = getconn()
        sqlcmd.Connection = sqlcon

        'sql = "select  count(*) from t_outlay_master where state=1 and billno='" & Me.ExDataGridView1.Rows(Me.ExDataGridView1.SelectedCells(0).RowIndex).Cells("billno").Value & "'"
        'sqlcmd.CommandText = sql


        'If sqlcmd.ExecuteScalar = 0 Then
        '    MsgBox("�õ��ݲ������״̬�������Դ�ӡ��", MsgBoxStyle.OkOnly, "��ʾ")
        '    Exit Sub
        'End If





        ShowFPYS_RPT(FPYS_RPT, Me.ExDataGridView1.Rows(Me.ExDataGridView1.SelectedCells(0).RowIndex).Cells("billno").Value)
        FPYS_RPT.PrintToPrinter(1, True, 0, 0)

        MsgBox("��ӡ��ɣ�", MsgBoxStyle.OkOnly, "��ʾ")
    End Sub
End Class