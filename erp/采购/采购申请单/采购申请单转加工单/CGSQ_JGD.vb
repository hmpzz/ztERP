Imports System.Data.SqlClient
Public Class CGSQ_JGD

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

        If Me.CheckBox4.Checked Then
            s1 = s1 & " and billno in  (select billno from cgd_detail where jgd_no ='') "
        End If

        If BM_id = 49 Or BM_id = 78 Or BM_id = 53 Then
            sql = "SELECT a.*,case state when 0 then '�½�' when 1 then '���' when 2 then '�᰸' end as state1 " & _
                        "FROM CGD_master a  " & _
                        "where 1=1 " & s & s1 & "  order by a.billno "
        Else
            sql = "SELECT a.*,case state when 0 then '�½�' when 1 then '���' when 2 then '�᰸' end as state1 " & _
                        "FROM CGD_master a left join " & _
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



        Me.ExDataGridView1.AddColumn("id", "id", , , , , , False)

        If Me.ExDataGridView1.Rows.Count > 0 Then
            cellclick(0)
        Else
            cellclick(-1)
        End If
    End Sub

    Private Sub ExDataGridView1_CellClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles ExDataGridView1.CellClick
        If e.RowIndex < 0 Then
            Exit Sub
        Else

            cellclick(e.RowIndex)
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
            sql = "select a.*,b.supplier_name  " & _
                    "from CGD_detail a left join  " & _
                    "t_supplier b on a.supplier_no=b.supplier_no  " & _
                     "where a.billno='" & Me.ExDataGridView1.Rows(index).Cells("billno").Value & "' order by a.billno "
        Else
            sql = "select a.*,b.supplier_name  " & _
                     "from CGD_detail a left join " & _
                     "t_supplier b on a.supplier_no=b.supplier_no  " & _
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
        Me.ExDataGridView2.AddColumn("supplier_name", "��Ӧ��")



        'Me.ExDataGridView2.AddColumn("item_standard", "���")
        'Me.ExDataGridView2.AddColumn("unit", "��λ")


        Me.ExDataGridView2.AddColumn("amount", "����", , , , DataGridViewContentAlignment.MiddleRight, "#0.00")
        Me.ExDataGridView2.AddColumn("price", "����", , , , DataGridViewContentAlignment.MiddleRight, "#0.0000")
        Me.ExDataGridView2.AddColumn("money", "���", , , , DataGridViewContentAlignment.MiddleRight, "#0.00")
        Me.ExDataGridView2.AddColumn("jgd_no", "��Ӧ�ӹ�����")
        'Me.ExDataGridView2.AddColumn("OK_amount", "���������", 120, , , DataGridViewContentAlignment.MiddleRight, "#0.000")
        Me.ExDataGridView2.AddColumn("memo", "˵��")

        Me.ExDataGridView2.AddColumn("supplier_no", "��Ӧ�̱��", , , , , , False)
        Me.ExDataGridView2.AddColumn("id", "id", , , , , , False)
    End Sub

    Private Sub ToolStripButton_save_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripButton_save.Click

    End Sub

    Private Sub ExDataGridView1_CellContentClick(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles ExDataGridView1.CellContentClick

    End Sub

    Private Sub ExDataGridView1_CellMouseDown(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellMouseEventArgs) Handles ExDataGridView1.CellMouseDown

    End Sub




    Private Sub ��Ӽ�¼ToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)

    End Sub

    Private Sub �޸ļ�¼ToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles �޸ļ�¼ToolStripMenuItem.Click
        Dim CGSQ_JGD_detail As New CGSQ_JGD_detail

        If Me.ExDataGridView2.SelectedCells.Count = 0 Then
            MsgBox("��ѡ��Ҫ�޸ĵ��У�", MsgBoxStyle.OkOnly, "��ʾ")
            Exit Sub
        End If


        Dim sqlcon As New SqlConnection
        Dim sqlcmd As New SqlCommand

        Dim sql As String


        sqlcon = getconn()
        sqlcmd.Connection = sqlcon

        sql = "SELECT jgd_no " & _
           "FROM CGD_detail where id='" & Me.ExDataGridView2.Rows(Me.ExDataGridView2.SelectedCells(0).RowIndex).Cells("id").Value & "'"

        sqlcmd.CommandText = sql
        If sqlcmd.ExecuteScalar.ToString.Trim <> "" Then
            MsgBox("�����Ѿ����ɼӹ��ѣ������޸ĵ��ۣ�", MsgBoxStyle.OkOnly, "��ʾ")
            Exit Sub
        End If

        CGSQ_JGD_detail.TextBox1.Text = Me.ExDataGridView2.Rows(Me.ExDataGridView2.SelectedCells(0).RowIndex).Cells("memo").Value
        CGSQ_JGD_detail.TextBox2.Text = Me.ExDataGridView2.Rows(Me.ExDataGridView2.SelectedCells(0).RowIndex).Cells("money").Value
        CGSQ_JGD_detail.TextBox3.Text = Me.ExDataGridView2.Rows(Me.ExDataGridView2.SelectedCells(0).RowIndex).Cells("name").Value
        CGSQ_JGD_detail.TextBox4.Text = Me.ExDataGridView2.Rows(Me.ExDataGridView2.SelectedCells(0).RowIndex).Cells("amount").Value
        CGSQ_JGD_detail.TextBox5.Text = Me.ExDataGridView2.Rows(Me.ExDataGridView2.SelectedCells(0).RowIndex).Cells("price").Value

        CGSQ_JGD_detail.ComboBox2.Text = Me.ExDataGridView2.Rows(Me.ExDataGridView2.SelectedCells(0).RowIndex).Cells("supplier_no").Value


        CGSQ_JGD_detail.Owner = Me
        CGSQ_JGD_detail.ShowDialog()
    End Sub



    Private Sub ContextMenuStrip2_Opening(ByVal sender As System.Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles ContextMenuStrip2.Opening

    End Sub

    Private Sub ExDataGridView2_CellMouseDown(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellMouseEventArgs) Handles ExDataGridView2.CellMouseDown
        If Me.ExDataGridView1.SelectedCells.Count = 0 Then
            Me.�޸ļ�¼ToolStripMenuItem.Enabled = False
            Me.����ToolStripMenuItem.Enabled = False
            Exit Sub

        End If

        If e.RowIndex < 0 Then
            Exit Sub
        End If

        Me.����ToolStripMenuItem.Enabled = True
        Me.�޸ļ�¼ToolStripMenuItem.Enabled = True
    End Sub

    Private Sub ����ToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ����ToolStripMenuItem.Click

        Dim sqlcon As New SqlConnection
        Dim sqlcmd As New SqlCommand
        Dim mytrans As SqlTransaction
        Dim sqlad As New SqlDataAdapter

        Dim dt As New DataTable
        Dim sql As String
        Dim k As Int32
        Dim billno As String


        If Me.ExDataGridView2.SelectedCells.Count = 0 Then
            MsgBox("��ѡ��Ҫ���ɵ��У�", MsgBoxStyle.OkOnly, "��ʾ")
            Exit Sub
        End If

        sqlcon = getconn()
        sqlcmd.Connection = sqlcon

        sql = "SELECT state " & _
                "FROM CGD_detail where id='" & Me.ExDataGridView2.Rows(Me.ExDataGridView2.SelectedCells(0).RowIndex).Cells("id").Value & "'"


        sqlcmd.CommandText = sql
        k = sqlcmd.ExecuteScalar

        If k = 2 Then
            MsgBox("�õ����Ѿ��᰸������������", MsgBoxStyle.OkOnly, "��ʾ")
            Exit Sub
        ElseIf k = 0 Then
            MsgBox("�õ���û����ˣ�����������", MsgBoxStyle.OkOnly, "��ʾ")
            Exit Sub
        End If


        sql = "SELECT jgd_no " & _
              "FROM CGD_detail where id='" & Me.ExDataGridView2.Rows(Me.ExDataGridView2.SelectedCells(0).RowIndex).Cells("id").Value & "'"

        sqlcmd.CommandText = sql
        If sqlcmd.ExecuteScalar.ToString.Trim <> "" Then
            MsgBox("�����Ѿ����ɼӹ��ѣ��벻Ҫ�ظ����ɣ�", MsgBoxStyle.OkOnly, "��ʾ")
            Exit Sub
        End If

        'sql = "update  CGD_master set state=1,check_date=getdate(),check_man='" & rs_name & "' where billno='" & Me.ExDataGridView1.Rows(Me.ExDataGridView1.SelectedCells(0).RowIndex).Cells("billno").Value & "'"
        'sqlcmd.CommandText = sql
        'sqlcmd.ExecuteNonQuery()

        'Me.ExDataGridView1.Rows(Me.ExDataGridView1.SelectedCells(0).RowIndex).Cells("state1").Value = "���"
        'Me.ExDataGridView1.Rows(Me.ExDataGridView1.SelectedCells(0).RowIndex).Cells("state").Value = 1
        'Me.ExDataGridView1.Rows(Me.ExDataGridView1.SelectedCells(0).RowIndex).Cells("check_man").Value = rs_name
        'Me.ExDataGridView1.Rows(Me.ExDataGridView1.SelectedCells(0).RowIndex).Cells("check_date").Value = Now



        mytrans = sqlcon.BeginTransaction

        sqlcmd.Transaction = mytrans

        Try



            sql = "update t_dj_num set num=num+1 where  tt='JGF'"
            sqlcmd.CommandText = sql
            sqlcmd.ExecuteNonQuery()

            sql = "select * from t_dj_num where tt='JGF'"
            sqlcmd.CommandText = sql
            sqlad.SelectCommand = sqlcmd
            dt.Rows.Clear()
            sqlad.Fill(dt)

            billno = dt.Rows(0)("TT") & "-" & GetAccountterm() & "-" & Format(dt.Rows(0)("num"), dt.Rows(0)("format"))



            sql = "INSERT INTO t_outwork_master ( billno,input_date,input_man,project_no,supplier_no) " & _
            " select '" & billno & "', getdate(), '" & rs_name & "',b.project_no,a.supplier_no from cgd_detail a left join cgd_master b on a.billno=b.billno where a.id='" & Me.ExDataGridView2.Rows(Me.ExDataGridView2.SelectedCells(0).RowIndex).Cells("id").Value & "'"



            sqlcmd.CommandText = sql
            sqlcmd.ExecuteNonQuery()


            sql = "INSERT INTO t_outwork_detail ( billno,money,memo,name,amount,price)  " & _
            " select  '" & billno & "',a.money,a.memo,a.name,a.amount,a.price  " & _
                    "from cgd_detail a   " & _
                    "where a.id='" & Me.ExDataGridView2.Rows(Me.ExDataGridView2.SelectedCells(0).RowIndex).Cells("id").Value & "'"

            sqlcmd.CommandText = sql
            sqlcmd.ExecuteNonQuery()



            'sql = "update a set a.money=b.summoney from t_outwork_master a inner join (select billno,sum(money) as summoney from t_outwork_detail where billno='" & billno & "' group by billno) b on a.billno=b.billno where a.billno='" & billno & "'"
            'sqlcmd.CommandText = sql
            'sqlcmd.ExecuteNonQuery()


            sql = "update  CGD_detail set jgd_no='" & billno & "' where id='" & Me.ExDataGridView2.Rows(Me.ExDataGridView2.SelectedCells(0).RowIndex).Cells("id").Value & "'"
            sqlcmd.CommandText = sql
            sqlcmd.ExecuteNonQuery()

            mytrans.Commit()
        Catch ex As Exception
            mytrans.Rollback()
            MsgBox(ex.Message, MsgBoxStyle.OkOnly, "����")
            Exit Sub
        End Try

        Me.ExDataGridView2.Rows(Me.ExDataGridView2.SelectedCells(0).RowIndex).Cells("jgd_no").Value = billno

        MsgBox("���ɳɹ���")
    End Sub
End Class