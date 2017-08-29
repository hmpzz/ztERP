Imports System.Data.SqlClient
Public Class T_GHFK_quality
    Public cha As String = ""

    Private Sub T_GHFK_quality_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
       
    End Sub

  

    Private Sub ToolStripButton_exit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripButton_exit.Click
        Me.Close()
    End Sub

    Private Sub ToolStripButton_refresh_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripButton_refresh.Click
      Refresh_ExDataGridView_master

    End Sub

    Private Sub ToolStripButton_excel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripButton_excel.Click
        Dim ToExcel As New BaseClass.TOEXCEL

        If ToExcel.ExToExcel(Me.ExDataGridView1, Me.SaveFileDialog1, True, "数量") = True Then
            MsgBox("导出成功！", MsgBoxStyle.OkOnly, "提示")
        End If
    End Sub

    Private Sub DateTimePicker1_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles DateTimePicker1.ValueChanged

    End Sub

    Private Sub ToolStripButton_question_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripButton_question.Click
        Dim T_GHFK_quality_condition As New T_GHFK_quality_condition
        T_GHFK_quality_condition.Owner = Me
        cha = ""


        T_GHFK_quality_condition.ShowDialog()

        If T_GHFK_quality_condition.DialogResult = Windows.Forms.DialogResult.OK Then
            Refresh_ExDataGridView_master()
        Else
            Exit Sub
        End If
    End Sub

    Private Sub Refresh_ExDataGridView_master(Optional ByVal s As String = "")
        Dim sqlcon As New SqlConnection
        Dim sqlcmd As New SqlCommand
        Dim sqlad As New SqlDataAdapter

        Dim yingfu As Double = 0
        Dim yifu As Double = 0
        Dim Weifu As Double = 0

        Dim i As Integer


        Dim dt As New DataTable
        Dim sql As String


        sqlcon = getconn()
        sqlcmd.Connection = sqlcon
        sqlcmd.CommandTimeout = 0


        If Me.CheckBox1.Checked = False Then
            sql = "select isnull(sum(aaa.rk_money),0) as rk_money,sum(aaa.FPF_money) as FPF_money,isnull(sum(aaa.rk_money),0)-isnull(sum(aaa.fpf_money),0) as WF_money from  " & _
                    "(select isnull(sum(aa.rk_money),0) as rk_money,isnull(sum(bb.FPF_money),0) as FPF_money  " & _
                    "from  " & _
                    "	((select b.billno,sum(c.money) as rk_money " & _
                    "		from  	purchase_master a inner join  	  " & _
                    "    		t_StockBill_master b on a.billno=b.resourceno and b.tempflag=0 and billkindcode='20' inner join  	  " & _
                    "    		t_StockBill_detail c on b.billno=c.billno inner join      " & _
                    "    		t_supplier d on a.supplier_no=d.supplier_no " & _
                    "	where   b.input_date >='" & Format(Me.DateTimePicker1.Value, "yyyy-MM-dd") & " 00:00:00' and b.input_date<='" & Format(Me.DateTimePicker2.Value, "yyyy-MM-dd") & " 23:59:59'  " & _
                    "	group by b.billno) aa left join   " & _
                    "    			(select b.RK_billno,sum(b.money)  as FPF_money  " & _
                    "    		from t_fpyf_master  a left join   " & _
                    "    			t_fpyf_detail b on a.billno=b.billno   " & _
                    "    		where a.hx_date>='" & Format(Me.DateTimePicker1.Value, "yyyy-MM-dd") & " 00:00:00' and a.hx_date<='" & Format(Me.DateTimePicker2.Value, "yyyy-MM-dd") & " 23:59:59' and a.hx=1   " & _
                    "    		group by b.rk_billno) bb on aa.billno=bb.rk_billno ) " & _
                    "union all  " & _
                    "select isnull(sum(aa.rk_money),0) as rk_money,isnull(sum(bb.FPF_money),0) as FPF_money  " & _
                    "from  " & _
                    "	((select b.billno,sum(c.money) as rk_money " & _
                    "		from  	purchase_master a inner join  	  " & _
                    "    		t_StockBill_master_history b on a.billno=b.resourceno and b.tempflag=0 and billkindcode='20' inner join  	  " & _
                    "    		t_StockBill_detail_history c on b.billno=c.billno inner join      " & _
                    "    		t_supplier d on a.supplier_no=d.supplier_no " & _
                    "	where   b.input_date >='" & Format(Me.DateTimePicker1.Value, "yyyy-MM-dd") & " 00:00:00' and b.input_date<='" & Format(Me.DateTimePicker2.Value, "yyyy-MM-dd") & " 23:59:59'  " & _
                    "	group by b.billno) aa left join   " & _
                    "    			(select b.RK_billno,sum(b.money)  as FPF_money  " & _
                    "    		from t_fpyf_master  a left join   " & _
                    "    			t_fpyf_detail b on a.billno=b.billno   " & _
                    "    		where a.hx_date>='" & Format(Me.DateTimePicker1.Value, "yyyy-MM-dd") & " 00:00:00' and a.hx_date<='" & Format(Me.DateTimePicker2.Value, "yyyy-MM-dd") & " 23:59:59' and a.hx=1   " & _
                    "    		group by b.rk_billno) bb on aa.billno=bb.rk_billno ) " & _
                    " union all " & _
                    "/*加工费部分*/ " & _
                    "select sum(a.money) as rk_money ,sum(b.fpf_money) as FPF_money " & _
                    "from t_outwork_master a left join  " & _
                    "	(select bb.rk_billno,isnull(sum(bb.money),0) as FPF_money " & _
                    "		from t_FPYF_master aa left join  " & _
                    "			t_FPYF_detail bb on aa.billno=bb.billno " & _
                    "		where aa.hx_date>='" & Format(Me.DateTimePicker1.Value, "yyyy-MM-dd") & " 00:00:00' and aa.hx_date<='" & Format(Me.DateTimePicker2.Value, "yyyy-MM-dd") & " 23:59:59' and hx=1 and Fp_type=1 " & _
                    "		group by bb.rk_billno) b on a.billno=b.rk_billno " & _
                    "where a.input_date >='" & Format(Me.DateTimePicker1.Value, "yyyy-MM-dd") & " 00:00:00' and a.input_date<='" & Format(Me.DateTimePicker2.Value, "yyyy-MM-dd") & " 23:59:59' " & _
                    ") aaa "


            sqlcmd.CommandText = sql
            sqlad.SelectCommand = sqlcmd
            dt.Rows.Clear()
            sqlad.Fill(dt)

            Me.ExDataGridView1.SetDataSource(dt)
            Me.ExDataGridView1.Columns.Clear()



            Me.ExDataGridView1.AddColumn("rk_money", "应付款金额", , , , DataGridViewContentAlignment.BottomRight)
            Me.ExDataGridView1.AddColumn("FPF_money", "已付款金额", , , , DataGridViewContentAlignment.BottomRight)
            Me.ExDataGridView1.AddColumn("WF_money", "未付款金额", , , , DataGridViewContentAlignment.BottomRight)
        Else

            sql = "select aaa.supplier_name,isnull(sum(aaa.rk_money),0) as rk_money,isnull(sum(aaa.FPF_money),0) as FPF_money,isnull(sum(aaa.rk_money),0)-isnull(sum(aaa.fpf_money),0) as WF_money from  " & _
                "(select aa.supplier_name,isnull(sum(aa.rk_money),0) as rk_money,isnull(sum(bb.FPF_money),0) as FPF_money  " & _
                "from  " & _
                "	((select d.supplier_name,b.billno,sum(c.money) as rk_money " & _
                "		from  	purchase_master a inner join  	  " & _
                "    		t_StockBill_master b on a.billno=b.resourceno and b.tempflag=0 and billkindcode='20' inner join  	  " & _
                "    		t_StockBill_detail c on b.billno=c.billno inner join      " & _
                "    		t_supplier d on a.supplier_no=d.supplier_no " & _
                "	where  a.supplier_no in (" & cha & ") and  b.input_date >='" & Format(Me.DateTimePicker1.Value, "yyyy-MM-dd") & " 00:00:00' and b.input_date<='" & Format(Me.DateTimePicker2.Value, "yyyy-MM-dd") & " 23:59:59'  " & _
                "	group by d.supplier_name ,b.billno) aa left join   " & _
                "    			(select b.RK_billno,sum(b.money)  as FPF_money  " & _
                "    		from t_fpyf_master  a left join   " & _
                "    			t_fpyf_detail b on a.billno=b.billno   " & _
                "    		where a.hx_date>='" & Format(Me.DateTimePicker1.Value, "yyyy-MM-dd") & " 00:00:00' and a.hx_date<='" & Format(Me.DateTimePicker2.Value, "yyyy-MM-dd") & " 23:59:59' and a.hx=1   " & _
                "    		group by b.rk_billno) bb on aa.billno=bb.rk_billno ) " & _
                "group by aa.supplier_name " & _
                "union all  " & _
                "select aa.supplier_name,isnull(sum(aa.rk_money),0) as rk_money,isnull(sum(bb.FPF_money),0) as FPF_money  " & _
                "from  " & _
                "	((select d.supplier_name,b.billno,sum(c.money) as rk_money " & _
                "		from  	purchase_master a inner join  	  " & _
                "    		t_StockBill_master_history b on a.billno=b.resourceno and b.tempflag=0 and billkindcode='20' inner join  	  " & _
                "    		t_StockBill_detail_history c on b.billno=c.billno inner join      " & _
                "    		t_supplier d on a.supplier_no=d.supplier_no " & _
                "	where  a.supplier_no in (" & cha & ") and  b.input_date >='" & Format(Me.DateTimePicker1.Value, "yyyy-MM-dd") & " 00:00:00' and b.input_date<='" & Format(Me.DateTimePicker2.Value, "yyyy-MM-dd") & " 23:59:59'  " & _
                "	group by d.supplier_name ,b.billno) aa left join   " & _
                "    			(select b.RK_billno,sum(b.money)  as FPF_money  " & _
                "    		from t_fpyf_master  a left join   " & _
                "    			t_fpyf_detail b on a.billno=b.billno   " & _
                "    		where a.hx_date>='" & Format(Me.DateTimePicker1.Value, "yyyy-MM-dd") & " 00:00:00' and a.hx_date<='" & Format(Me.DateTimePicker2.Value, "yyyy-MM-dd") & " 23:59:59' and a.hx=1   " & _
                "    		group by b.rk_billno) bb on aa.billno=bb.rk_billno ) " & _
                "group by aa.supplier_name " & _
                 " union all " & _
                    "/*加工费部分*/ " & _
                    "select c.supplier_name,sum(a.money) as rk_money ,sum(b.fpf_money) as FPF_money " & _
                    "from t_outwork_master a left join  " & _
                    "	(select bb.rk_billno,isnull(sum(bb.money),0) as FPF_money " & _
                    "		from t_FPYF_master aa left join  " & _
                    "			t_FPYF_detail bb on aa.billno=bb.billno " & _
                    "		where aa.hx_date>='" & Format(Me.DateTimePicker1.Value, "yyyy-MM-dd") & " 00:00:00' and aa.hx_date<='" & Format(Me.DateTimePicker2.Value, "yyyy-MM-dd") & " 23:59:59' and hx=1 and Fp_type=1 " & _
                    "		group by bb.rk_billno) b on a.billno=b.rk_billno  left join " & _
                    "      t_supplier c on a.supplier_no=c.supplier_no " & _
                    "where a.input_date >='" & Format(Me.DateTimePicker1.Value, "yyyy-MM-dd") & " 00:00:00' and a.input_date<='" & Format(Me.DateTimePicker2.Value, "yyyy-MM-dd") & " 23:59:59' and a.supplier_no in (" & cha & ")  " & _
                    " group by c.supplier_name " & _
                ") aaa " & _
                "group by aaa.supplier_name "


            sqlcmd.CommandText = sql
            sqlad.SelectCommand = sqlcmd
            dt.Rows.Clear()
            sqlad.Fill(dt)

            Me.ExDataGridView1.SetDataSource(dt)
            Me.ExDataGridView1.Columns.Clear()


            Me.ExDataGridView1.AddColumn("supplier_name", "供应商")


            Me.ExDataGridView1.AddColumn("rk_money", "应付款金额", , , , DataGridViewContentAlignment.BottomRight)
            Me.ExDataGridView1.AddColumn("FPF_money", "已付款金额", , , , DataGridViewContentAlignment.BottomRight)
            Me.ExDataGridView1.AddColumn("WF_money", "未付款金额", , , , DataGridViewContentAlignment.BottomRight)





         
        End If

        For i = 0 To dt.Rows.Count - 1

            yingfu = yingfu + dt.Rows(i)("rk_money")
            yifu = yifu + dt.Rows(i)("FPF_money")

            Weifu = Weifu + dt.Rows(i)("WF_money")
        Next

        Dim DR As DataRow = dt.NewRow

        If Me.CheckBox1.Checked = False Then
            DR("rk_money") = yingfu
            DR("FPF_money") = yifu
            DR("WF_money") = Weifu
        Else
            DR("supplier_name") = "总计"
            DR("rk_money") = yingfu
            DR("FPF_money") = yifu
            DR("WF_money") = Weifu

        End If
    

        dt.Rows.Add(DR)




        'If price_state = 1 Then
        '    sql = "select sum(money) as Summoney from (" & sql & ") a"
        '    sqlcmd.CommandText = sql
        '    Me.Label4.Text = "金额总计：" & sqlcmd.ExecuteScalar
        'Else
        '    Me.Label4.Text = ""
        'End If
    End Sub

  
End Class