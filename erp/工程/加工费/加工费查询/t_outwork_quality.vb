Imports System.Data.SqlClient
Public Class t_outwork_quality

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
            s2 = s2 & " a.print_state=0 or "
        End If

        If Me.CheckBox5.Checked Then
            s2 = s2 & " a.print_state=1 or "
        End If

        If s2.Length <> 0 Then
            s2 = Microsoft.VisualBasic.Left(s2, s2.Length - 3)
            s2 = " and (" & s2 & ")"
        End If



        sql = "SELECT a.*,case a.state when 0 then '�½�' when 1 then '���' when 2 then '�᰸' end as state1,b.supplier_name,case print_state when 0 then 'δ��ӡ' when 1 then '�Ѵ�ӡ' end as print_state1,c.project_name " & _
                "FROM t_outwork_master a left join " & _
                "t_supplier b on a.supplier_no=b.supplier_no left join " & _
                "   project_master c on a.project_no=c.project_no " & _
                "where 1=1 " & s & s1 & s2 & "  order by a.billno "
      



        sqlcmd.CommandText = sql
        sqlad.SelectCommand = sqlcmd
        dt.Rows.Clear()
        sqlad.Fill(dt)

        Me.ExDataGridView1.SetDataSource(dt)
        Me.ExDataGridView1.Columns.Clear()

        Me.ExDataGridView1.AddColumn("billno", "�ӹ��ѵ���")
        Me.ExDataGridView1.AddColumn("project_no", "���̺�")
        Me.ExDataGridView1.AddColumn("project_name", "��������")
        Me.ExDataGridView1.AddColumn("supplier_name", "�ӹ���")

        Me.ExDataGridView1.AddColumn("state1", "״̬")
        Me.ExDataGridView1.AddColumn("print_state1", "��ӡ״̬")



        Me.ExDataGridView1.AddColumn("input_man", "¼����")
        Me.ExDataGridView1.AddColumn("input_date", "¼������", , , , , "yyyy-MM-dd")

        Me.ExDataGridView1.AddColumn("check_man", "�����")
        Me.ExDataGridView1.AddColumn("check_date", "�������", , , , , "yyyy-MM-dd")

        Me.ExDataGridView1.AddColumn("money", "�ܽ��", , , , DataGridViewContentAlignment.MiddleRight, "#0.00")
        Me.ExDataGridView1.AddColumn("KP_money", "��Ʊ���", , , , DataGridViewContentAlignment.MiddleRight, "#0.00")


        Me.ExDataGridView1.AddColumn("id", "id", , , , , , False)

        If Me.ExDataGridView1.Rows.Count > 0 Then
            cellclick(0)
        Else
            cellclick(-1)
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
            sql = "select a.*  " & _
                    "from t_outwork_detail a  " & _
                     "where a.billno='" & Me.ExDataGridView1.Rows(index).Cells("billno").Value & "' order by a.billno "
        Else
            sql = "select a.*  " & _
                     "from t_outwork_detail a  " & _
                      "where a.id=-1 order by a.billno "
        End If
        sqlcmd.Connection = sqlcon
        sqlcmd.CommandText = sql
        sqlad.SelectCommand = sqlcmd
        dt.Rows.Clear()
        sqlad.Fill(dt)

        Me.ExDataGridView2.SetDataSource(dt)
        Me.ExDataGridView2.Columns.Clear()




        Me.ExDataGridView2.AddColumn("name", "����")
        Me.ExDataGridView2.AddColumn("memo", "˵��")

        Me.ExDataGridView2.AddColumn("amount", "����", , , , DataGridViewContentAlignment.MiddleRight, "#0.00")
        Me.ExDataGridView2.AddColumn("price", "����", , , , DataGridViewContentAlignment.MiddleRight, "#0.0000")
        Me.ExDataGridView2.AddColumn("money", "���", , , , DataGridViewContentAlignment.MiddleRight, "#0.00")

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

    Private Sub ToolStripButton_show_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripButton_show.Click
        Dim printView As New printView

        Dim t_outwork_RPT As New t_outwork_RPT

        Dim sqlcon As New SqlConnection
        Dim sqlcmd As New SqlCommand


        Dim sql As String



        If Me.ExDataGridView1.SelectedCells.Count = 0 Then
            MsgBox("��ѡ��Ҫ��ӡ�ĵ���", MsgBoxStyle.OkOnly, "��ʾ")
            Exit Sub
        End If

        sqlcon = getconn()
        sqlcmd.Connection = sqlcon

        sql = "select  count(*) from t_outwork_master where state=1 and print_state=0 and billno='" & Me.ExDataGridView1.Rows(Me.ExDataGridView1.SelectedCells(0).RowIndex).Cells("billno").Value & "'"
        sqlcmd.CommandText = sql


        If sqlcmd.ExecuteScalar = 0 Then
            MsgBox("�õ��ݲ������״̬�����Ѿ���ӡ���������Դ�ӡ��", MsgBoxStyle.OkOnly, "��ʾ")
            Exit Sub
        End If

        ShowOUTWORK_RPT(t_outwork_RPT, Me.ExDataGridView1.Rows(Me.ExDataGridView1.SelectedCells(0).RowIndex).Cells("billno").Value)

        printView.CrystalReportViewer1.ReportSource = t_outwork_RPT
        printView.CrystalReportViewer1.RefreshReport()
        printView.ShowDialog()
    End Sub

    Private Sub ToolStripButton_print_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripButton_print.Click

        Dim t_outwork_RPT As New t_outwork_RPT
        Dim sqlcon As New SqlConnection
        Dim sqlcmd As New SqlCommand


        Dim sql As String

        If Me.ExDataGridView1.SelectedCells.Count = 0 Then
            MsgBox("��ѡ��Ҫ��ӡ�ĵ���", MsgBoxStyle.OkOnly, "��ʾ")
            Exit Sub
        End If



        sqlcon = getconn()
        sqlcmd.Connection = sqlcon

        sql = "select  count(*) from t_outwork_master where state=1 and print_state=0 and billno='" & Me.ExDataGridView1.Rows(Me.ExDataGridView1.SelectedCells(0).RowIndex).Cells("billno").Value & "'"
        sqlcmd.CommandText = sql


        If sqlcmd.ExecuteScalar = 0 Then
            MsgBox("�õ��ݲ������״̬�����Ѿ���ӡ���������Դ�ӡ��", MsgBoxStyle.OkOnly, "��ʾ")
            Exit Sub
        End If


        ShowOUTWORK_RPT(t_outwork_RPT, Me.ExDataGridView1.Rows(Me.ExDataGridView1.SelectedCells(0).RowIndex).Cells("billno").Value)
        t_outwork_RPT.PrintToPrinter(1, True, 0, 0)

        MsgBox("��ӡ��ɣ�", MsgBoxStyle.OkOnly, "��ʾ")
    End Sub



    Private Sub ShowOUTWORK_RPT(ByRef t_outwork_RPT As t_outwork_RPT, ByVal billno As String)
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

        t_outwork_RPT.PrintOptions.PaperSize = rawKind

        If rawKind = 11 Then
            t_outwork_RPT.PrintOptions.PaperOrientation = CrystalDecisions.Shared.PaperOrientation.Landscape
        Else
            t_outwork_RPT.PrintOptions.PaperOrientation = CrystalDecisions.Shared.PaperOrientation.Portrait
        End If



    
        ' CType(CrystalDecisions.Shared.PaperSize rawKind)

        sql = "SELECT a.*,b.supplier_name,'" & rs_name & "' as print_man,getdate() as print_date1 " & _
              "FROM t_outwork_master a left join " & _
              " t_supplier b on a.supplier_no=b.supplier_no" & _
              " where a.billno='" & billno & "'"

        sqlcmd.CommandText = sql
        sqlad.SelectCommand = sqlcmd
        sqlad.Fill(ds, "t_outwork_master")


        sql = "select a.*   " & _
                     "from t_outwork_detail a " & _
                     " where a.billno='" & billno & "' order by a.id "
        sqlcmd.CommandText = sql
        sqlad.SelectCommand = sqlcmd
        sqlad.Fill(ds, "t_outwork_detail")

        t_outwork_RPT.SetDataSource(ds)
        t_outwork_RPT.Refresh()

        save_pdf(t_outwork_RPT, billno)

        sql = "update t_outwork_master set print_state=1,print_date=getdate()  where billno='" & billno & "'"
        sqlcmd.CommandText = sql
        sqlcmd.ExecuteNonQuery()
    End Sub

    Private Sub ToolStripButton_question_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripButton_question.Click

    End Sub
End Class