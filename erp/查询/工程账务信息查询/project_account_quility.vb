Imports System.Data.SqlClient
Public Class project_account_quility
    Public cha As String = ""
    Private Sub ToolStripButton_refresh_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripButton_refresh.Click
        Me.cha = ""

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


        sql = "SELECT a.*,case state when 0 then '新建' when 1 then '审核' when 2 then '结案' end as state1 ,isnull(b.jgf_money,0) as jgf_money " & _
        "	,isnull(c.ck_money,0) as ck_money,isnull(d.FY_money,0) as FY_money,a.project_money-a.FP_money as WKP_money,a.project_money-a.SK_money as WSK_money, " & _
        " isnull(b.jgf_money,0)+isnull(c.ck_money,0)+isnull(d.FY_money,0) as CB_money " & _
        "FROM project_master a  left join " & _
        "	/*以下是加工费*/ " & _
        "	(select a.project_no,sum(b.money) as JGF_money " & _
        "		from t_outwork_master a left join   " & _
        "			t_outwork_detail b on a.billno=b.billno   " & _
        "		where 1=1 " & s & " and a.state=1  " & _
        "		group by a.project_no) b  on a.project_no=b.project_no left join  " & _
        "		/*以上是加工费*/ " & _
        "		/*以下是出库*/ " & _
        "		(select a.project_no,sum(e.summoney) as CK_money " & _
        "		from T_part_out_master a left join   " & _
        "			T_part_out_detail b on a.billno=b.billno  left join  " & _
        "		(select aa.resourceno,ab.resourceid,sum(ab.money) as SUMmoney,sum(ab.amount) as SUMamount from   " & _
        "		 (select * from t_stockbill_master where tempflag=0 and billkindcode=12  union all   " & _
        "		select * from t_stockbill_master_history where tempflag=0 and billkindcode=12  ) aa left join   " & _
        "		(select * from t_stockbill_detail  union all   " & _
        "			select * from t_stockbill_detail_history) ab on aa.billno=ab.billno  " & _
        "		group by aa.resourceno,ab.resourceid) e on b.billno=e.resourceno and b.id=e.resourceid  " & _
        "		where  1=1 " & s & " " & _
        "		group by a.project_no) c on a.project_no=c.project_no left join  " & _
        "		/*以上是出库*/ " & _
        "		/*以下是费用*/ " & _
        "		(select a.project_no,sum(b.money) as FY_money " & _
        "		from t_outlay_master a left join   " & _
        "			t_outlay_detail b on a.billno=b.billno   " & _
        "		where 1=1 " & s & " and a.state=1  " & _
        "		group by a.project_no) d on a.project_no=d.project_no " & _
        "		/*以上是费用*/ " & _
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
        Me.ExDataGridView1.AddColumn("project_no", "工程号")
        Me.ExDataGridView1.AddColumn("project_name", "工程名称")
        Me.ExDataGridView1.AddColumn("project_money", "工程金额", , , , , "#0")

        Me.ExDataGridView1.AddColumn("ck_money", "领料金额", , , , , "#0.00", )
        Me.ExDataGridView1.AddColumn("JGF_money", "加工费金额", , , , , "#0.00", )
        Me.ExDataGridView1.AddColumn("FY_money", "费用金额", , , , , "#0.00", )
        Me.ExDataGridView1.AddColumn("CB_money", "成本合计", , , , , "#0.00", )

        Me.ExDataGridView1.AddColumn("SK_money", "已收款金额", , , , , "#0.00", )

        Me.ExDataGridView1.AddColumn("FP_money", "开票金额", , , , , "#0.00", )
        Me.ExDataGridView1.AddColumn("WKP_money", "未开票金额", , , , , "#0.00", )


        'Me.ExDataGridView1.AddColumn("effective_date", "有效日期", , , , , "yyyy-MM-dd")


        'Me.ExDataGridView1.AddColumn("memo", "备注")
        'Me.ExDataGridView1.AddColumn("state1", "状态")

        'Me.ExDataGridView1.AddColumn("begin_date", "开工日期", , , , , "yyyy-MM-dd")
        'Me.ExDataGridView1.AddColumn("end_date", "完工日期", , , , , "yyyy-MM-dd")
        'Me.ExDataGridView1.AddColumn("end_man", "确认人")

        'Me.ExDataGridView1.AddColumn("input_man", "录入人")
        'Me.ExDataGridView1.AddColumn("input_date", "录入日期", , , , , "yyyy-MM-dd")
        'Me.ExDataGridView1.AddColumn("check_man", "审核人")
        'Me.ExDataGridView1.AddColumn("check_date", "审核日期", , , , , "yyyy-MM-dd")

        Me.ExDataGridView1.AddColumn("id", "id", , , , , , False)

      
    End Sub

    Private Sub ToolStripButton_excel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripButton_excel.Click
        Dim ToExcel As New BaseClass.TOEXCEL

        If ToExcel.ExToExcel(Me.ExDataGridView1, Me.SaveFileDialog1, True, "工程金额", "领料金额", "加工费金额", "费用金额", "成本合计", "已收款金额", "开票金额", "未开票金额") = True Then
            MsgBox("导出成功！", MsgBoxStyle.OkOnly, "提示")
        End If
    End Sub

  
End Class