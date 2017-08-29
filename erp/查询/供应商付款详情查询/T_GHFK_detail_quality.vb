Imports System.Data.SqlClient
Public Class T_GHFK_detail_quality

    Private Sub T_GHFK_detail_quality_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim sqlcon As New SqlConnection
        Dim sqlcmd As New SqlCommand
        Dim sqlad As New SqlDataAdapter

        Dim dt As New DataTable
        Dim sql As String
        Dim i As Int32



        sqlcon = getconn()
        sqlcmd.Connection = sqlcon



        Me.ComboBox1.Items.Clear()
        Me.ComboBox2.Items.Clear()
        sql = "select * from t_supplier where stop=1 order by supplier_name"

        sqlcmd.CommandText = sql
        sqlad.SelectCommand = sqlcmd
        dt.Rows.Clear()
        sqlad.Fill(dt)

        Me.ComboBox1.Items.Add("")
        Me.ComboBox2.Items.Add("")

        For i = 0 To dt.Rows.Count - 1
            Me.ComboBox1.Items.Add(dt.Rows(i)("supplier_name"))
            Me.ComboBox2.Items.Add(dt.Rows(i)("supplier_no"))
        Next
    End Sub

    Private Sub ToolStripButton_exit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripButton_exit.Click
        Me.Close()
    End Sub

    Private Sub ToolStripButton_excel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripButton_excel.Click
        Dim ToExcel As New BaseClass.TOEXCEL

        If ToExcel.ExToExcel(Me.ExDataGridView1, Me.SaveFileDialog1, True, "数量") = True Then
            MsgBox("导出成功！", MsgBoxStyle.OkOnly, "提示")
        End If
    End Sub

    Private Sub ToolStripButton_refresh_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripButton_refresh.Click
        Dim sqlcon As New SqlConnection
        Dim sqlcmd As New SqlCommand
        Dim sqlad As New SqlDataAdapter

        Dim dt As New DataTable
        Dim sql As String

        sqlcon = getconn()
        sqlcmd.Connection = sqlcon


        If Me.ComboBox1.Text = "" Then
            sql = "select aa.supplier_name,aa.billno,aa.rk_money,isnull(bb.FPF_money,0) as FPF_money,aa.rk_money - isnull(bb.FPF_money,0) as WF_money " & _
                 "                from   " & _
                 "                	((select d.supplier_name,b.billno,sum(c.money) as rk_money  " & _
                 "                		from  	purchase_master a inner join  	   " & _
                 "                    		t_StockBill_master_history b on a.billno=b.resourceno and b.tempflag=0 and billkindcode='20' inner join  	   " & _
                 "                    		t_StockBill_detail_history c on b.billno=c.billno inner join       " & _
                 "                    		t_supplier d on a.supplier_no=d.supplier_no  " & _
                 "                	where  b.money>b.kp_money " & _
                 "                	group by d.supplier_name ,b.billno) aa left join    " & _
                 "                    			(select b.RK_billno,sum(b.money)  as FPF_money   " & _
                 "                    		from t_fpyf_master  a left join    " & _
                 "                    			t_fpyf_detail b on a.billno=b.billno    " & _
                 "                    		where  a.hx=1    " & _
                 "                    		group by b.rk_billno) bb on aa.billno=bb.rk_billno ) " & _
                 "union all  " & _
                 "select aa.supplier_name,aa.billno,aa.rk_money,isnull(bb.FPF_money,0) as FPF_money,aa.rk_money - isnull(bb.FPF_money,0) as WF_money " & _
                 "                from   " & _
                 "                	((select d.supplier_name,b.billno,sum(c.money) as rk_money  " & _
                 "                		from  	purchase_master a inner join  	   " & _
                 "                    		t_StockBill_master b on a.billno=b.resourceno and b.tempflag=0 and billkindcode='20' inner join  	   " & _
                 "                    		t_StockBill_detail c on b.billno=c.billno inner join       " & _
                 "                    		t_supplier d on a.supplier_no=d.supplier_no  " & _
                 "                	where  b.money>b.kp_money " & _
                 "                	group by d.supplier_name ,b.billno) aa left join    " & _
                 "                    			(select b.RK_billno,sum(b.money)  as FPF_money   " & _
                 "                    		from t_fpyf_master  a left join    " & _
                 "                    			t_fpyf_detail b on a.billno=b.billno    " & _
                 "                    		where  a.hx=1    " & _
                 "                    		group by b.rk_billno) bb on aa.billno=bb.rk_billno ) " & _
                 "union all  " & _
                 " /*加工费部分*/" & _
                 "     select c.supplier_name,a.billno,a.money as rk_money ,isnull(b.fpf_money,0) as FPF_money,a.money - isnull(b.FPF_money,0) as WF_money  " & _
                 "                    from t_outwork_master a left join   " & _
                 "                    	(select bb.rk_billno,isnull(sum(bb.money),0) as FPF_money  " & _
                 "                    		from t_FPYF_master aa left join   " & _
                 "                    			t_FPYF_detail bb on aa.billno=bb.billno  " & _
                 "                    		where  hx=1 and Fp_type=1  " & _
                 "                    		group by bb.rk_billno) b on a.billno=b.rk_billno  left join  " & _
                 "                          t_supplier c on a.supplier_no=c.supplier_no  " & _
                 "                    where    a.money>b.fpf_money "


            sqlcmd.CommandText = sql
            sqlad.SelectCommand = sqlcmd
            dt.Rows.Clear()
            sqlad.Fill(dt)

            Me.ExDataGridView1.SetDataSource(dt)
            Me.ExDataGridView1.Columns.Clear()

            Me.ExDataGridView1.AddColumn("supplier_name", "供应商")
            Me.ExDataGridView1.AddColumn("billno", "单据号")

            Me.ExDataGridView1.AddColumn("rk_money", "应付款金额", , , , DataGridViewContentAlignment.BottomRight)
            Me.ExDataGridView1.AddColumn("FPF_money", "已付款金额", , , , DataGridViewContentAlignment.BottomRight)
            Me.ExDataGridView1.AddColumn("WF_money", "未付款金额", , , , DataGridViewContentAlignment.BottomRight)
        Else

            sql = "select aa.supplier_name,aa.billno,aa.rk_money,isnull(bb.FPF_money,0) as FPF_money,aa.rk_money - isnull(bb.FPF_money,0) as WF_money " & _
                 "                from   " & _
                 "                	((select d.supplier_name,b.billno,sum(c.money) as rk_money  " & _
                 "                		from  	purchase_master a inner join  	   " & _
                 "                    		t_StockBill_master_history b on a.billno=b.resourceno and b.tempflag=0 and billkindcode='20' inner join  	   " & _
                 "                    		t_StockBill_detail_history c on b.billno=c.billno inner join       " & _
                 "                    		t_supplier d on a.supplier_no=d.supplier_no  " & _
                 "                	where  a.supplier_no='" & Me.ComboBox2.Text.Trim & "' and b.money>b.kp_money " & _
                 "                	group by d.supplier_name ,b.billno) aa left join    " & _
                 "                    			(select b.RK_billno,sum(b.money)  as FPF_money   " & _
                 "                    		from t_fpyf_master  a left join    " & _
                 "                    			t_fpyf_detail b on a.billno=b.billno    " & _
                 "                    		where  a.hx=1    " & _
                 "                    		group by b.rk_billno) bb on aa.billno=bb.rk_billno ) " & _
                 "union all  " & _
                 "select aa.supplier_name,aa.billno,aa.rk_money,isnull(bb.FPF_money,0) as FPF_money,aa.rk_money - isnull(bb.FPF_money,0) as WF_money " & _
                 "                from   " & _
                 "                	((select d.supplier_name,b.billno,sum(c.money) as rk_money  " & _
                 "                		from  	purchase_master a inner join  	   " & _
                 "                    		t_StockBill_master b on a.billno=b.resourceno and b.tempflag=0 and billkindcode='20' inner join  	   " & _
                 "                    		t_StockBill_detail c on b.billno=c.billno inner join       " & _
                 "                    		t_supplier d on a.supplier_no=d.supplier_no  " & _
                 "                	where  a.supplier_no='" & Me.ComboBox2.Text.Trim & "' and b.money>b.kp_money " & _
                 "                	group by d.supplier_name ,b.billno) aa left join    " & _
                 "                    			(select b.RK_billno,sum(b.money)  as FPF_money   " & _
                 "                    		from t_fpyf_master  a left join    " & _
                 "                    			t_fpyf_detail b on a.billno=b.billno    " & _
                 "                    		where  a.hx=1    " & _
                 "                    		group by b.rk_billno) bb on aa.billno=bb.rk_billno ) " & _
                 "union all  " & _
                 " /*加工费部分*/" & _
                 "     select c.supplier_name,a.billno,a.money as rk_money ,isnull(b.fpf_money,0) as FPF_money,a.money - isnull(b.FPF_money,0) as WF_money  " & _
                 "                    from t_outwork_master a left join   " & _
                 "                    	(select bb.rk_billno,isnull(sum(bb.money),0) as FPF_money  " & _
                 "                    		from t_FPYF_master aa left join   " & _
                 "                    			t_FPYF_detail bb on aa.billno=bb.billno  " & _
                 "                    		where  hx=1 and Fp_type=1  " & _
                 "                    		group by bb.rk_billno) b on a.billno=b.rk_billno  left join  " & _
                 "                          t_supplier c on a.supplier_no=c.supplier_no  " & _
                 "                    where  a.supplier_no='" & Me.ComboBox2.Text.Trim & "' and a.money>b.fpf_money "



            sqlcmd.CommandText = sql
            sqlad.SelectCommand = sqlcmd
            dt.Rows.Clear()
            sqlad.Fill(dt)

            Me.ExDataGridView1.SetDataSource(dt)
            Me.ExDataGridView1.Columns.Clear()


            Me.ExDataGridView1.AddColumn("supplier_name", "供应商")
            Me.ExDataGridView1.AddColumn("billno", "单据号")

            Me.ExDataGridView1.AddColumn("rk_money", "应付款金额", , , , DataGridViewContentAlignment.BottomRight)
            Me.ExDataGridView1.AddColumn("FPF_money", "已付款金额", , , , DataGridViewContentAlignment.BottomRight)
            Me.ExDataGridView1.AddColumn("WF_money", "未付款金额", , , , DataGridViewContentAlignment.BottomRight)
        End If
        'If price_state = 1 Then
        '    sql = "select sum(money) as Summoney from (" & sql & ") a"
        '    sqlcmd.CommandText = sql
        '    Me.Label4.Text = "金额总计：" & sqlcmd.ExecuteScalar
        'Else
        '    Me.Label4.Text = ""
        'End If
    End Sub

    Private Sub ComboBox1_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ComboBox1.SelectedIndexChanged
        Me.ComboBox2.SelectedIndex = Me.ComboBox1.SelectedIndex
    End Sub
End Class