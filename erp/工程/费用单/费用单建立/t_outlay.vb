Imports System.Data.SqlClient
Public Class t_outlay

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

        If BM_id = 49 Or BM_id = 78 Then
            sql = "SELECT a.*,case a.state when 0 then '�½�' when 1 then '���' when 2 then '�᰸' end as state1,b.project_name " & _
    "                FROM t_outlay_master a  left join   " & _
    "                	  	project_master b on a.project_no=b.project_no  " & _
    "                where 1=1 " & s1 & s & " order by a.billno"
        Else
            sql = "SELECT a.*,case a.state when 0 then '�½�' when 1 then '���' when 2 then '�᰸' end as state1,b.project_name " & _
    "                FROM t_outlay_master a  left join   " & _
    "                	  	project_master b on a.project_no=b.project_no  left join  " & _
                       " t_rs on a.input_man=t_rs.rs_name " & _
    "                where  t_rs.bm_id= " & BM_id & s1 & s & " order by a.billno"
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
        Me.ExDataGridView1.AddColumn("state1", "״̬")
        Me.ExDataGridView1.AddColumn("memo", "˵��")

        Me.ExDataGridView1.AddColumn("sq_man", "������")

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

            sql = " SELECT a.* " & _
"                        FROM t_outlay_detail a    " & _
"                           where a.billno='" & Me.ExDataGridView1.Rows(index).Cells("billno").Value & "' order by a.id"

        Else
            sql = " SELECT a.* " & _
"                        FROM t_outlay_detail a    " & _
"                           where a.id=-1"
        End If
        sqlcmd.Connection = sqlcon
        sqlcmd.CommandText = sql
        sqlad.SelectCommand = sqlcmd
        dt.Rows.Clear()
        sqlad.Fill(dt)

        Me.ExDataGridView2.SetDataSource(dt)
        Me.ExDataGridView2.Columns.Clear()




        Me.ExDataGridView2.AddColumn("memo", "˵��")


        Me.ExDataGridView2.AddColumn("money", "���", , , , DataGridViewContentAlignment.MiddleRight, "#0.0000")

        Me.ExDataGridView2.AddColumn("id", "id", , , , , , False)
    End Sub

    Private Sub ToolStripButton_add_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripButton_add.Click
        Dim t_outlay_add As New t_outlay_add
        t_outlay_add.Owner = Me
        t_outlay_add.state = "new"
        t_outlay_add.billno = ""

        t_outlay_add.ShowDialog()
        If t_outlay_add.DialogResult = Windows.Forms.DialogResult.OK Then
            Refresh_ExDataGridView_master()
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

    Private Sub ToolStripButton_mod_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripButton_mod.Click
        Dim sqlcon As New SqlConnection
        Dim sqlcmd As New SqlCommand
        Dim sqlad As New SqlDataAdapter
        Dim dt As New DataTable

        Dim sql As String

        If Me.ExDataGridView1.SelectedCells.Count = 0 Then
            Exit Sub
        End If

        sqlcon = getconn()
        sqlcmd.Connection = sqlcon

        sql = "select  count(*) from t_outlay_master where state=0 and billno='" & Me.ExDataGridView1.Rows(Me.ExDataGridView1.SelectedCells(0).RowIndex).Cells("billno").Value & "'"
        sqlcmd.CommandText = sql


        If sqlcmd.ExecuteScalar = 0 Then
            MsgBox("�õ��ݲ����½�״̬���������޸ģ�", MsgBoxStyle.OkOnly, "��ʾ")
            Exit Sub
        End If




        Dim t_outlay_add As New t_outlay_add
        t_outlay_add.Owner = Me
        t_outlay_add.state = "mod"
        t_outlay_add.billno = Me.ExDataGridView1.Rows(Me.ExDataGridView1.SelectedCells(0).RowIndex).Cells("billno").Value



        t_outlay_add.ShowDialog()
        If t_outlay_add.DialogResult = Windows.Forms.DialogResult.OK Then
            cellclick(Me.ExDataGridView1.SelectedCells(0).RowIndex)

        End If
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
            MsgBox("��ѡ��Ҫɾ�����У�", MsgBoxStyle.OkOnly, "��ʾ")
            Exit Sub
        End If

        If MsgBox("ȷ��Ҫɾ�����õ���Ϊ��" & Me.ExDataGridView1.Rows(Me.ExDataGridView1.SelectedCells(0).RowIndex).Cells("billno").Value & "�ĵ�����", MsgBoxStyle.YesNo, "��ʾ") = MsgBoxResult.No Then
            Exit Sub
        End If



        sqlcon = getconn()
        sqlcmd.Connection = sqlcon

        mytrans = sqlcon.BeginTransaction

        sqlcmd.Transaction = mytrans

        Try




            sql = "SELECT count(*)  " & _
                           "FROM t_outlay_master  where state<>0 and billno='" & Me.ExDataGridView1.Rows(Me.ExDataGridView1.SelectedCells(0).RowIndex).Cells("billno").Value & "' "


            sqlcmd.CommandText = sql
            If sqlcmd.ExecuteScalar <> 0 Then
                Throw New System.Exception("�˵��ݲ����½�״̬������ɾ����")
            End If



            sql = "delete from t_outlay_master where billno='" & Me.ExDataGridView1.Rows(Me.ExDataGridView1.SelectedCells(0).RowIndex).Cells("billno").Value & "' "
            sqlcmd.CommandText = sql
            sqlcmd.ExecuteNonQuery()

            sql = "delete from t_outlay_detail where billno='" & Me.ExDataGridView1.Rows(Me.ExDataGridView1.SelectedCells(0).RowIndex).Cells("billno").Value & "' "
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
        Dim printView As New printView

        Dim FYD_RPT As New FYD_RPT

        Dim sqlcon As New SqlConnection
        Dim sqlcmd As New SqlCommand


        Dim sql As String

        If Me.ExDataGridView1.SelectedCells.Count = 0 Then
            MsgBox("��ѡ��Ҫ��ӡ�ĵ���", MsgBoxStyle.OkOnly, "��ʾ")
            Exit Sub
        End If


        sqlcon = getconn()
        sqlcmd.Connection = sqlcon

        sql = "select  count(*) from t_outlay_master where state=1 and billno='" & Me.ExDataGridView1.Rows(Me.ExDataGridView1.SelectedCells(0).RowIndex).Cells("billno").Value & "'"
        sqlcmd.CommandText = sql


        If sqlcmd.ExecuteScalar = 0 Then
            MsgBox("�õ��ݲ������״̬�������Դ�ӡ��", MsgBoxStyle.OkOnly, "��ʾ")
            Exit Sub
        End If





        ShowFYD_RPT(FYD_RPT, Me.ExDataGridView1.Rows(Me.ExDataGridView1.SelectedCells(0).RowIndex).Cells("billno").Value)

        printView.CrystalReportViewer1.ReportSource = FYD_RPT
        printView.CrystalReportViewer1.RefreshReport()
        printView.ShowDialog()
    End Sub


    Private Sub ToolStripButton2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripButton2.Click

        Dim FYD_RPT As New FYD_RPT

        Dim sqlcon As New SqlConnection
        Dim sqlcmd As New SqlCommand


        Dim sql As String

        If Me.ExDataGridView1.SelectedCells.Count = 0 Then
            MsgBox("��ѡ��Ҫ��ӡ�ĵ���", MsgBoxStyle.OkOnly, "��ʾ")
            Exit Sub
        End If


        sqlcon = getconn()
        sqlcmd.Connection = sqlcon

        sql = "select  count(*) from t_outlay_master where state=1 and billno='" & Me.ExDataGridView1.Rows(Me.ExDataGridView1.SelectedCells(0).RowIndex).Cells("billno").Value & "'"
        sqlcmd.CommandText = sql


        If sqlcmd.ExecuteScalar = 0 Then
            MsgBox("�õ��ݲ������״̬�������Դ�ӡ��", MsgBoxStyle.OkOnly, "��ʾ")
            Exit Sub
        End If

        ShowFYD_RPT(FYD_RPT, Me.ExDataGridView1.Rows(Me.ExDataGridView1.SelectedCells(0).RowIndex).Cells("billno").Value)
        FYD_RPT.PrintToPrinter(1, True, 0, 0)

        MsgBox("��ӡ��ɣ�", MsgBoxStyle.OkOnly, "��ʾ")
    End Sub

    Public Sub ShowFYD_RPT(ByRef FYD_RPT As FYD_RPT, ByVal billno As String)
        Dim sql As String
        Dim sqlcon As New SqlConnection
        Dim sqlcmd As New SqlCommand
        Dim sqlad As New SqlDataAdapter
        Dim ds As New DataSet
        Dim i As Int32

        sqlcon = getconn()
        sqlcmd.Connection = sqlcon


        ds.Tables.Clear()

        Dim doc As New System.Drawing.Printing.PrintDocument
        Dim rawKind As Int32 = 11

        For i = 0 To doc.PrinterSettings.PaperSizes.Count - 1
            If doc.PrinterSettings.PaperSizes(i).PaperName.ToUpper = "ZT" Then
                rawKind = doc.PrinterSettings.PaperSizes(i).RawKind
                Exit For
            End If
        Next

        FYD_RPT.PrintOptions.PaperSize = rawKind

        If rawKind = 11 Then
            FYD_RPT.PrintOptions.PaperOrientation = CrystalDecisions.Shared.PaperOrientation.Landscape
        Else
            FYD_RPT.PrintOptions.PaperOrientation = CrystalDecisions.Shared.PaperOrientation.Portrait
        End If




        ' CType(CrystalDecisions.Shared.PaperSize rawKind)

        sql = "SELECT a.*,'" & rs_name & "' as print_man,getdate() as print_date1 " & _
              "FROM   t_outlay_master a " & _
              " where a.billno='" & billno & "'"

        sqlcmd.CommandText = sql
        sqlad.SelectCommand = sqlcmd
        sqlad.Fill(ds, "t_outlay_master")


        sql = "select a.*    " & _
                     "from   t_outlay_detail a   " & _
                     " where a.billno='" & billno & "' order by a.id "
        sqlcmd.CommandText = sql
        sqlad.SelectCommand = sqlcmd
        sqlad.Fill(ds, "t_outlay_detail")

        FYD_RPT.SetDataSource(ds)
        FYD_RPT.Refresh()

        save_pdf(FYD_RPT, billno)
    End Sub
End Class