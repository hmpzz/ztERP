Imports System.Data.SqlClient
Public Class project_detail_quality
    Public cha As String = ""
    Private Sub ToolStripButton_exit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripButton_exit.Click
        Me.Close()
    End Sub


    Private Sub ToolStripButton_refresh_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripButton_refresh.Click
        Me.cha = ""
        Me.Detail_check.Checked = False
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


        sql = "SELECT a.*,case state when 0 then '�½�' when 1 then '���' when 2 then '�᰸' end as state1 " & _
                "FROM project_master a " & _
                "where a.factory_code in (select c.factory_code " & _
                "                       from sys_user a inner join  " & _
                "                       	t_user_factory b on a.login_id=b.login_id inner join  " & _
                "                       	t_factory c on b.factory_code=c.factory_code " & _
                "                       where a.stop_login=1 and c.stop=1 and a.login_id='" & login_id & "' " & _
                "                       ) " & s & s1 & s2 & "  order by a.id "
        sqlcmd.CommandText = sql
        sqlad.SelectCommand = sqlcmd
        dt.Rows.Clear()
        sqlad.Fill(dt)

        Me.ExDataGridView1.SetDataSource(dt)
        Me.ExDataGridView1.Columns.Clear()

        'Me.ExDataGridView1.AddColumn("check", "", 20, False, New DataGridViewCheckBoxColumn)
        Me.ExDataGridView1.AddColumn("project_no", "���̺�")
        Me.ExDataGridView1.AddColumn("project_name", "��������")
        Me.ExDataGridView1.AddColumn("project_money", "���̽��", , , , , "#0", False)

        Me.ExDataGridView1.AddColumn("FP_money", "��Ʊ���", , , , , "#0", False)

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
            cellclick(0, cha)
        End If
    End Sub

    Private Sub ExDataGridView1_CellClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles ExDataGridView1.CellClick
        If e.RowIndex < 0 Then
            Exit Sub
        Else
            cellclick(e.RowIndex, cha)
        End If
    End Sub


    Private Sub cellclick(ByVal index As Int32, Optional ByVal ss As String = "")
        Dim sqlcon As New SqlConnection
        Dim sqlcmd As New SqlCommand
        Dim sqlad As New SqlDataAdapter

        Dim dt As New DataTable
        Dim dt2 As New DataTable
        Dim dt3 As New DataTable
        Dim sql As String
        Dim sql1 As String

        Dim s As String = ""

        sqlcon = getconn()


        'sql = "select * from  t_product_FY_detail where fy_id='" & Me.ExDataGridView1.Rows(index).Cells("id").Value & "' order by id"
        If index <> -1 Then
            s = ""
            If ss.Trim.Length > 0 Then
                s = " and input_date between " & ss
            End If

            sql = "select b.*,c.item_name,c.item_standard,c.unit ,d.depot_name,isnull(e.summoney,0) as summoney,isnull(e.summoney,0)/case ok_amount when 0 then 1 else ok_amount end as avgprice,isnull(SUMamount,0) as SUMamount " & _
                "from T_part_out_master a left join  " & _
                "	T_part_out_detail b on a.billno=b.billno left join  " & _
                "	t_item c on b.item_code=c.item_code left join  " & _
                "	t_depot d on a.depot_code=d.depot_code left join  " & _
                "(select aa.resourceno,ab.resourceid,sum(ab.money) as SUMmoney,sum(ab.amount) as SUMamount from  " & _
                " (select * from t_stockbill_master where tempflag=0 and billkindcode=12 " & s & " union all  " & _
                "select * from t_stockbill_master_history where tempflag=0 and billkindcode=12 " & s & ") aa left join  " & _
                "(select * from t_stockbill_detail  union all  " & _
                "	select * from t_stockbill_detail_history) ab on aa.billno=ab.billno " & _
                "group by aa.resourceno,ab.resourceid) e on b.billno=e.resourceno and b.id=e.resourceid " & _
                "where a.project_no ='" & Me.ExDataGridView1.Rows(index).Cells("project_no").Value & "'"

            sql1 = " select isnull(sum(summoney),0) from (" & sql & " ) as mx "
            sql = sql & "  order by c.item_name "

        Else
            sql = "select b.*,c.item_name,c.item_standard,c.unit ,d.depot_nameisnull(e.summoney,0) as summoney,isnull(e.summoney,0)/case ok_amount when 0 then 1 else ok_amount end as avgprice,isnull(SUMamount,0) as SUMamount " & _
                "from T_part_out_master a left join  " & _
                "	T_part_out_detail b on a.billno=b.billno left join  " & _
                "	t_item c on b.item_code=c.item_code left join  " & _
                "	t_depot d on a.depot_code=d.depot_code left join " & _
                 "(select aa.resourceno,ab.resourceid,sum(ab.money) as SUMmoney,sum(ab.amount) as SUMamount from  " & _
                " (select * from t_stockbill_master where tempflag=0 and billkindcode=12 union all  " & _
                "select * from t_stockbill_master_history where tempflag=0 and billkindcode=12) aa left join  " & _
                "(select * from t_stockbill_detail  union all  " & _
                "	select * from t_stockbill_detail_history) ab on aa.billno=ab.billno " & _
                "group by aa.resourceno,ab.resourceid) e on b.billno=e.resourceno and b.id=e.resourceid " & _
                "where a.id=-1 "

            sql1 = " select isnull(sum(summoney),0) from (" & sql & " ) as mx "
            sql = sql & "  order by c.item_name "

        End If
        sqlcmd.Connection = sqlcon
        sqlcmd.CommandText = sql
        sqlad.SelectCommand = sqlcmd
        dt.Rows.Clear()
        dt.Columns.Clear()
        sqlad.Fill(dt)


        Me.ExDataGridView2.SetDataSource(dt)
        Me.ExDataGridView2.Columns.Clear()

        Me.ExDataGridView2.AddColumn("billno", "���ϵ���")

        Me.ExDataGridView2.AddColumn("item_code", "���ϱ���")
        Me.ExDataGridView2.AddColumn("item_name", "��������")
        Me.ExDataGridView2.AddColumn("item_standard", "���")
        Me.ExDataGridView2.AddColumn("unit", "��λ")
        Me.ExDataGridView2.AddColumn("depot_name", "���ϲֿ�")

        Me.ExDataGridView2.AddColumn("amount", "����", , , , DataGridViewContentAlignment.MiddleRight, "#0.0000")
        Me.ExDataGridView2.AddColumn("OK_amount", "�ѳ��������ܼ�", 120, , , DataGridViewContentAlignment.MiddleRight, "#0.0000")
        Me.ExDataGridView2.AddColumn("sumamount", "������������", 120, , , DataGridViewContentAlignment.MiddleRight, "#0.0000")



        If price_state = 1 Then
            Me.ExDataGridView2.AddColumn("avgprice", "����", , , , DataGridViewContentAlignment.MiddleRight, "#0.0000")
            Me.ExDataGridView2.AddColumn("summoney", "���", , , , DataGridViewContentAlignment.MiddleRight, "#0.0000")
        End If
        Me.ExDataGridView2.AddColumn("id", "id", , , , , , False)

        sqlcmd.CommandText = sql1
        Me.Label2.Text = "���Ͻ���ܼƣ�" & Format(sqlcmd.ExecuteScalar, "#0.0000") & "Ԫ"



        If index <> -1 Then
            s = ""
            If ss.Trim.Length > 0 Then
                s = " and a.print_date between " & ss
            End If

            sql = "select b.*,a.print_date " & _
                    "from t_outlay_master a left join  " & _
                    "	t_outlay_detail b on a.billno=b.billno  " & _
                    "where a.project_no ='" & Me.ExDataGridView1.Rows(index).Cells("project_no").Value & "' and a.state=1  " & s & " "

            sql1 = " select isnull(sum(money),0) from (" & sql & " ) as mx "


        Else
            sql = "select b.* " & _
                "from t_outlay_master a left join  " & _
                "	t_outlay_detail b on a.billno=b.billno " & _
"                           where a.id=-1"

            sql1 = " select isnull(sum(money),0) from (" & sql & " ) as mx "

        End If
        sqlcmd.Connection = sqlcon
        sqlcmd.CommandText = sql
        sqlad.SelectCommand = sqlcmd

        dt2.Rows.Clear()
        sqlad.Fill(dt2)

        Me.ExDataGridView3.SetDataSource(dt2)
        Me.ExDataGridView3.Columns.Clear()


        Me.ExDataGridView3.AddColumn("billno", "���õ���")

        Me.ExDataGridView3.AddColumn("memo", "˵��")


        Me.ExDataGridView3.AddColumn("money", "���", , , , DataGridViewContentAlignment.MiddleRight, "#0.0000")
        Me.ExDataGridView3.AddColumn("print_date", "��ӡ����")

        Me.ExDataGridView3.AddColumn("id", "id", , , , , , False)


        sqlcmd.CommandText = sql1
        Me.Label3.Text = "���ý���ܼƣ�" & Format(sqlcmd.ExecuteScalar, "#0.0000") & "Ԫ"





        If index <> -1 Then
            s = ""
            If ss.Trim.Length > 0 Then
                s = "and a.print_date between " & ss
            End If

            sql = "select b.*,a.print_date " & _
                    "from t_outwork_master a left join  " & _
                    "	t_outwork_detail b on a.billno=b.billno  " & _
                    "where a.project_no ='" & Me.ExDataGridView1.Rows(index).Cells("project_no").Value & "' and a.state=1  " & s & " "

            sql1 = " select isnull(sum(money),0) from (" & sql & " ) as mx "


        Else
            sql = "select b.* " & _
                "from t_outwork_master a left join  " & _
                "	t_outwork_detaill b on a.billno=b.billno " & _
"                           where a.id=-1"

            sql1 = " select isnull(sum(money),0) from (" & sql & " ) as mx "

        End If
        sqlcmd.Connection = sqlcon
        sqlcmd.CommandText = sql
        sqlad.SelectCommand = sqlcmd

        dt3.Rows.Clear()
        sqlad.Fill(dt3)

        Me.ExDataGridView4.SetDataSource(dt3)
        Me.ExDataGridView4.Columns.Clear()


        Me.ExDataGridView4.AddColumn("billno", "�ӹ��ѵ���")
        Me.ExDataGridView4.AddColumn("name", "����")
        Me.ExDataGridView4.AddColumn("memo", "˵��")


        Me.ExDataGridView4.AddColumn("amount", "����", , , , DataGridViewContentAlignment.MiddleRight, "#0.0000")
        Me.ExDataGridView4.AddColumn("price", "����", , , , DataGridViewContentAlignment.MiddleRight, "#0.0000")
        Me.ExDataGridView4.AddColumn("money", "���", , , , DataGridViewContentAlignment.MiddleRight, "#0.00")
        Me.ExDataGridView4.AddColumn("print_date", "��ӡ����")

        Me.ExDataGridView4.AddColumn("id", "id", , , , , , False)


        sqlcmd.CommandText = sql1
        Me.Label4.Text = "�ӹ��ѽ���ܼƣ�" & Format(sqlcmd.ExecuteScalar, "#0.0000") & "Ԫ"


    End Sub

    Private Sub ExDataGridView1_CellContentClick(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles ExDataGridView1.CellContentClick

    End Sub

    Private Sub ����EXCELToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ����EXCELToolStripMenuItem.Click
        Dim ToExcel As New BaseClass.TOEXCEL

        If ToExcel.ExToExcel(Me.ExDataGridView2, Me.SaveFileDialog1, True, "����", "�ѳ�������", "����", "���") = True Then
            MsgBox("�����ɹ���", MsgBoxStyle.OkOnly, "��ʾ")
        End If
    End Sub

    Private Sub ToolStripButton_excel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripButton_excel.Click
        Dim ToExcel As New BaseClass.TOEXCEL
        Dim s As Boolean = True


        If ToExcel.ExToExcel(Me.ExDataGridView2, Me.SaveFileDialog1, True, "����", "�ѳ�������", "����", "���") Then
            If ToExcel.ExToExcel(Me.ExDataGridView3, Me.SaveFileDialog1, False, "���") Then
                If ToExcel.ExToExcel(Me.ExDataGridView4, Me.SaveFileDialog1, False, "����", "����", "���") Then
                    s = True
                Else
                    s = False
                End If
            Else
                s = False
            End If

        Else
            s = False
        End If

      


        If s Then
            MsgBox("�����ɹ���", MsgBoxStyle.OkOnly, "��ʾ")
        Else
            MsgBox("��������", MsgBoxStyle.OkOnly, "��ʾ")
        End If
    End Sub

    Private Sub ToolStripButton_question_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripButton_question.Click
        Dim project_detail_quality_cha As New project_detail_quality_cha
        project_detail_quality_cha.Owner = Me
        cha = ""
        Me.Detail_check.Checked = False

        project_detail_quality_cha.ShowDialog()

        If project_detail_quality_cha.DialogResult = Windows.Forms.DialogResult.OK Then
            Refresh_ExDataGridView_master()
        Else
            Exit Sub
        End If
    End Sub

    Private Sub project_detail_quality_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

    End Sub
End Class