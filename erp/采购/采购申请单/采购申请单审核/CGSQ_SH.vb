Imports System.Data.SqlClient
Public Class CGSQ_SH

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

        If Me.CheckBox3.Checked Then
            s1 = s1 & " a.state=2 or "
        End If

        If s1.Length <> 0 Then
            s1 = Microsoft.VisualBasic.Left(s1, s1.Length - 3)
            s1 = " and (" & s1 & ")"
        End If


        If BM_id = 49 Or BM_id = 78 Then
            sql = "SELECT a.*,case state when 0 then '�½�' when 1 then '���' when 2 then '�᰸' end as state1 " & _
                         "FROM CGD_master a   " & _
                         "where 1=1 " & s & s1 & "  order by a.billno "
        Else
            sql = "SELECT a.*,case state when 0 then '�½�' when 1 then '���' when 2 then '�᰸' end as state1,b.supplier_name " & _
                         "FROM CGD_master a  left join  " & _
                       " t_rs on a.input_man=t_rs.rs_name " & _
                         "where  t_rs.bm_id= " & BM_id & s & s1 & "  order by a.billno "
        End If

     


        sqlcmd.CommandText = sql
        sqlad.SelectCommand = sqlcmd
        dt.Rows.Clear()
        sqlad.Fill(dt)

        Me.ExDataGridView1.SetDataSource(dt)
        Me.ExDataGridView1.Columns.Clear()

        Me.ExDataGridView1.AddColumn("billno", "�ɹ����뵥��")
        Me.ExDataGridView1.AddColumn("project_no", "���̺�")

        Me.ExDataGridView1.AddColumn("state1", "״̬")

        Me.ExDataGridView1.AddColumn("memo", "˵��")


        Me.ExDataGridView1.AddColumn("input_man", "¼����")
        Me.ExDataGridView1.AddColumn("input_date", "¼������", , , , , "yyyy-MM-dd")

        Me.ExDataGridView1.AddColumn("check_man", "�����")
        Me.ExDataGridView1.AddColumn("check_date", "�������", , , , , "yyyy-MM-dd")

        Me.ExDataGridView1.AddColumn("state", "state", , , , , , False)

        Me.ExDataGridView1.AddColumn("id", "id", , , , , , False)

        If Me.ExDataGridView1.Rows.Count > 0 Then
            cellclick(0)
        Else
            cellclick(-1)
        End If
    End Sub

    Private Sub ToolStripButton_show_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripButton_show.Click
        Dim printView As New printView

        Dim CGSQ_RPT As New CGSQ_RPT

        If Me.ExDataGridView1.SelectedCells.Count = 0 Then
            MsgBox("��ѡ��Ҫ��ӡ�ĵ���", MsgBoxStyle.OkOnly, "��ʾ")
            Exit Sub
        End If

        ShowCGSQ_RPT(CGSQ_RPT, Me.ExDataGridView1.Rows(Me.ExDataGridView1.SelectedCells(0).RowIndex).Cells("billno").Value)

        printView.CrystalReportViewer1.ReportSource = CGSQ_RPT
        printView.CrystalReportViewer1.RefreshReport()
        printView.ShowDialog()
    End Sub

    Private Sub ToolStripButton_print_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripButton_print.Click

        Dim CGSQ_RPT As New CGSQ_RPT

        If Me.ExDataGridView1.SelectedCells.Count = 0 Then
            MsgBox("��ѡ��Ҫ��ӡ�ĵ���", MsgBoxStyle.OkOnly, "��ʾ")
            Exit Sub
        End If

        ShowCGSQ_RPT(CGSQ_RPT, Me.ExDataGridView1.Rows(Me.ExDataGridView1.SelectedCells(0).RowIndex).Cells("billno").Value)
        CGSQ_RPT.PrintToPrinter(1, True, 0, 0)

        MsgBox("��ӡ��ɣ�", MsgBoxStyle.OkOnly, "��ʾ")
    End Sub



    Public Sub ShowCGSQ_RPT(ByRef CGSQ_RPT As CGSQ_RPT, ByVal billno As String)
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


        CGSQ_RPT.PrintOptions.PaperSize = rawKind

        If rawKind = 11 Then
            CGSQ_RPT.PrintOptions.PaperOrientation = CrystalDecisions.Shared.PaperOrientation.Landscape
        Else
            CGSQ_RPT.PrintOptions.PaperOrientation = CrystalDecisions.Shared.PaperOrientation.Portrait
        End If



        ' CType(CrystalDecisions.Shared.PaperSize rawKind)

        sql = "SELECT a.*,b.supplier_name,'" & rs_name & "' as print_man,getdate() as print_date " & _
              "FROM cgd_master a left join " & _
              " t_supplier b on a.supplier_no=b.supplier_no" & _
              " where a.billno='" & billno & "'"

        sqlcmd.CommandText = sql
        sqlad.SelectCommand = sqlcmd
        sqlad.Fill(ds, "CGD_master")


        sql = "select a.*   " & _
                     "from cgd_detail a  " & _
                     " where a.billno='" & billno & "' order by a.id "
        sqlcmd.CommandText = sql
        sqlad.SelectCommand = sqlcmd
        sqlad.Fill(ds, "CGD_detail")

        CGSQ_RPT.SetDataSource(ds)
        CGSQ_RPT.Refresh()

        save_pdf(CGSQ_RPT, billno)
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


        sqlcon = getconn()


        'sql = "select * from  t_product_FY_detail where fy_id='" & Me.ExDataGridView1.Rows(index).Cells("id").Value & "' order by id"
        If index <> -1 Then
            sql = "select a.* " & _
                    "from CGD_detail a  " & _
                     "where a.billno='" & Me.ExDataGridView1.Rows(index).Cells("billno").Value & "' order by a.billno "
        Else
            sql = "select a.*" & _
                     "from CGD_detail a " & _
                      "where a.id=-1 order by a.billno "
        End If
        sqlcmd.Connection = sqlcon
        sqlcmd.CommandText = sql
        sqlad.SelectCommand = sqlcmd
        dt.Rows.Clear()
        sqlad.Fill(dt)

        Me.ExDataGridView2.SetDataSource(dt)
        Me.ExDataGridView2.Columns.Clear()



        'Me.ExDataGridView2.AddColumn("item_code", "���ϱ���")
        Me.ExDataGridView2.AddColumn("name", "����")
        'Me.ExDataGridView2.AddColumn("item_standard", "���")
        'Me.ExDataGridView2.AddColumn("unit", "��λ")


        Me.ExDataGridView2.AddColumn("amount", "����", , , , DataGridViewContentAlignment.MiddleRight, "#0.00")
        'Me.ExDataGridView2.AddColumn("price", "����", , , , DataGridViewContentAlignment.MiddleRight, "#0.0000")
        'Me.ExDataGridView2.AddColumn("money", "���", , , , DataGridViewContentAlignment.MiddleRight, "#0.00")
        'Me.ExDataGridView2.AddColumn("OK_amount", "���������", 120, , , DataGridViewContentAlignment.MiddleRight, "#0.000")
        Me.ExDataGridView2.AddColumn("jgd_no", "��Ӧ�ӹ�����")
        Me.ExDataGridView2.AddColumn("memo", "˵��")

        Me.ExDataGridView2.AddColumn("id", "id", , , , , , False)
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
    End Sub

    Private Sub ExDataGridView1_CellMouseUp(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellMouseEventArgs) Handles ExDataGridView1.CellMouseUp

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
                "FROM CGD_master where billno='" & Me.ExDataGridView1.Rows(Me.ExDataGridView1.SelectedCells(0).RowIndex).Cells("billno").Value & "'"


        sqlcmd.CommandText = sql
        k = sqlcmd.ExecuteScalar

        If k = 2 Then
            MsgBox("�õ����Ѿ��᰸������������", MsgBoxStyle.OkOnly, "��ʾ")
            Exit Sub
        ElseIf k = 1 Then
            MsgBox("�õ����Ѿ���ˣ�����������", MsgBoxStyle.OkOnly, "��ʾ")
            Exit Sub
        End If

        sql = "update  CGD_master set state=1,check_date=getdate(),check_man='" & rs_name & "' where billno='" & Me.ExDataGridView1.Rows(Me.ExDataGridView1.SelectedCells(0).RowIndex).Cells("billno").Value & "'"
        sqlcmd.CommandText = sql
        sqlcmd.ExecuteNonQuery()

        'Refresh_ExDataGridView(Me.TreeView1.SelectedNode.Text)
        Me.ExDataGridView1.Rows(Me.ExDataGridView1.SelectedCells(0).RowIndex).Cells("state1").Value = "���"
        Me.ExDataGridView1.Rows(Me.ExDataGridView1.SelectedCells(0).RowIndex).Cells("state").Value = 1
        Me.ExDataGridView1.Rows(Me.ExDataGridView1.SelectedCells(0).RowIndex).Cells("check_man").Value = rs_name
        Me.ExDataGridView1.Rows(Me.ExDataGridView1.SelectedCells(0).RowIndex).Cells("check_date").Value = Now

        MsgBox("��˳ɹ���", MsgBoxStyle.OkOnly, "��ʾ")
    End Sub

    Private Sub ����ToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ����ToolStripMenuItem.Click
        Dim sqlcon As New SqlConnection
        Dim sqlcmd As New SqlCommand

        Dim sql As String
        Dim k As int32
        Dim s As String

        If Me.ExDataGridView1.SelectedCells.Count = 0 Then
            MsgBox("��ѡ��Ҫ����ĵ��ݣ�", MsgBoxStyle.OkOnly, "��ʾ")
            Exit Sub
        End If

        sqlcon = getconn()
        sqlcmd.Connection = sqlcon

        sql = "SELECT state " & _
                "FROM CGD_master where billno='" & Me.ExDataGridView1.Rows(Me.ExDataGridView1.SelectedCells(0).RowIndex).Cells("billno").Value & "'"


        sqlcmd.CommandText = sql
        k = sqlcmd.ExecuteScalar

        If k = 2 Then
            MsgBox("�õ����Ѿ��᰸������������", MsgBoxStyle.OkOnly, "��ʾ")
            Exit Sub
        ElseIf k = 0 Then
            MsgBox("�õ���״̬Ϊ�½�������������", MsgBoxStyle.OkOnly, "��ʾ")
            Exit Sub
        End If

        sql = "SELECT jgd_no " & _
              "FROM CGD_master where billno='" & Me.ExDataGridView1.Rows(Me.ExDataGridView1.SelectedCells(0).RowIndex).Cells("billno").Value & "' "

        sqlcmd.CommandText = sql
        s = sqlcmd.ExecuteScalar.ToString.Trim
        If s <> "" Then
            MsgBox("�õ����Ѿ������˼ӹ���" & s & ",��ɾ�����Ӧ�ļӹ��ѵ��ݺ��ٷ���˴˵���!", MsgBoxStyle.OkOnly, "��ʾ")
            Exit Sub
        End If



        sql = "update  CGD_master set state=0 where billno='" & Me.ExDataGridView1.Rows(Me.ExDataGridView1.SelectedCells(0).RowIndex).Cells("billno").Value & "'"
        sqlcmd.CommandText = sql
        sqlcmd.ExecuteNonQuery()

        'Refresh_ExDataGridView(Me.TreeView1.SelectedNode.Text)
        Me.ExDataGridView1.Rows(Me.ExDataGridView1.SelectedCells(0).RowIndex).Cells("state1").Value = "�½�"
        Me.ExDataGridView1.Rows(Me.ExDataGridView1.SelectedCells(0).RowIndex).Cells("state").Value = 0
        MsgBox("����˳ɹ���", MsgBoxStyle.OkOnly, "��ʾ")
    End Sub

    Private Sub �᰸ToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles �᰸ToolStripMenuItem.Click
        Dim sqlcon As New SqlConnection
        Dim sqlcmd As New SqlCommand

        Dim sql As String
        Dim k As Int32


        If Me.ExDataGridView1.SelectedCells.Count = 0 Then
            MsgBox("��ѡ��Ҫ�᰸�ĵ��ݣ�", MsgBoxStyle.OkOnly, "��ʾ")
            Exit Sub
        End If


        sqlcon = getconn()
        sqlcmd.Connection = sqlcon

        sql = "SELECT state " & _
                "FROM CGD_master where billno='" & Me.ExDataGridView1.Rows(Me.ExDataGridView1.SelectedCells(0).RowIndex).Cells("billno").Value & "'"


        sqlcmd.CommandText = sql
        k = sqlcmd.ExecuteScalar

        If k = 2 Then
            MsgBox("�õ����Ѿ��᰸������������", MsgBoxStyle.OkOnly, "��ʾ")
            Exit Sub
        End If

        sql = "update  CGD_master set state=2 where billno='" & Me.ExDataGridView1.Rows(Me.ExDataGridView1.SelectedCells(0).RowIndex).Cells("billno").Value & "'"
        sqlcmd.CommandText = sql
        sqlcmd.ExecuteNonQuery()

        'Refresh_ExDataGridView(Me.TreeView1.SelectedNode.Text)
        Me.ExDataGridView1.Rows(Me.ExDataGridView1.SelectedCells(0).RowIndex).Cells("state1").Value = "�᰸"
        Me.ExDataGridView1.Rows(Me.ExDataGridView1.SelectedCells(0).RowIndex).Cells("state").Value = 2
        MsgBox("�᰸�ɹ���", MsgBoxStyle.OkOnly, "��ʾ")
    End Sub

    Private Sub ���᰸ToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ���᰸ToolStripMenuItem.Click
        Dim sqlcon As New SqlConnection
        Dim sqlcmd As New SqlCommand

        Dim sql As String
        Dim k As Int32

        If Me.ExDataGridView1.SelectedCells.Count = 0 Then
            MsgBox("��ѡ��Ҫ���᰸�ĵ��ݣ�", MsgBoxStyle.OkOnly, "��ʾ")
            Exit Sub
        End If

        sqlcon = getconn()
        sqlcmd.Connection = sqlcon

        sql = "SELECT state " & _
                "FROM CGD_master where billno='" & Me.ExDataGridView1.Rows(Me.ExDataGridView1.SelectedCells(0).RowIndex).Cells("billno").Value & "'"


        sqlcmd.CommandText = sql
        k = sqlcmd.ExecuteScalar

        If k = 1 Then
            MsgBox("�õ���״̬Ϊ��ˣ�����������", MsgBoxStyle.OkOnly, "��ʾ")
            Exit Sub
        ElseIf k = 0 Then
            MsgBox("�õ���״̬Ϊ�½�������������", MsgBoxStyle.OkOnly, "��ʾ")
            Exit Sub
        End If

        sql = "update CGD_master set state=1,check_date=getdate(),check_man='" & rs_name & "' where billno='" & Me.ExDataGridView1.Rows(Me.ExDataGridView1.SelectedCells(0).RowIndex).Cells("billno").Value & "'"
        sqlcmd.CommandText = sql
        sqlcmd.ExecuteNonQuery()

        'Refresh_ExDataGridView(Me.TreeView1.SelectedNode.Text)

        Me.ExDataGridView1.Rows(Me.ExDataGridView1.SelectedCells(0).RowIndex).Cells("state1").Value = "���"
        Me.ExDataGridView1.Rows(Me.ExDataGridView1.SelectedCells(0).RowIndex).Cells("state").Value = 1
        MsgBox("���᰸�ɹ���", MsgBoxStyle.OkOnly, "��ʾ")
    End Sub

    Private Sub ExDataGridView1_CellContentClick(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles ExDataGridView1.CellContentClick

    End Sub
End Class