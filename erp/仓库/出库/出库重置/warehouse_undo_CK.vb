Imports System.Data.SqlClient
Public Class warehouse_undo_CK

    Private Sub ToolStripButton_exit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripButton_exit.Click
        Me.Close()
    End Sub

    Private Sub Refresh_all()
        Dim sqlcon As New SqlConnection
        Dim sqlcmd As New SqlCommand
        Dim sqlad As New SqlDataAdapter


        Dim dt As New DataTable
        Dim sql As String
        Dim i As Integer


        Me.ComboBox1.Items.Clear()
        Me.ComboBox2.Items.Clear()
        Me.ComboBox3.Items.Clear()
        Me.ComboBox4.Items.Clear()


        sqlcon = getconn()
        sqlcmd.Connection = sqlcon

        sql = "select a.* from t_depot a inner join t_user_depot b on a.depot_code=b.depot_code where a.stop=1 and b.login_id='" & login_id & "' order by a.depot_code"
        sqlcmd.CommandText = sql
        sqlad.SelectCommand = sqlcmd

        dt.Clear()
        sqlad.Fill(dt)

        For i = 0 To dt.Rows.Count - 1
            Me.ComboBox1.Items.Add(dt.Rows(i)("depot_name"))
        Next

        Me.ExDataGridView1.Columns.Clear()
        'Me.ExDataGridView1.DataSource = Nothing
        'Me.ExDataGridView1.DataMember = Nothing
        Me.ExDataGridView1.ClearDSDM()


    End Sub


    Private Sub warehouse_undo_CK_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Refresh_all()
    End Sub

    Private Sub ToolStripButton_refresh_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripButton_refresh.Click
        Refresh_all()
    End Sub

    Private Sub ComboBox1_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ComboBox1.SelectedIndexChanged
        Dim sqlcon As New SqlConnection
        Dim sqlcmd As New SqlCommand
        Dim sqlad As New SqlDataAdapter

        Dim dt As New DataTable
        Dim sql As String

        Dim i As Int32

        If Me.ComboBox1.SelectedIndex <> -1 Then
            sqlcon = getconn()
            sqlcmd.Connection = sqlcon

            sql = "SELECT distinct b.billkindname,a.billkindcode " & _
                    "FROM t_stockbill_master a left join  " & _
                    "	t_stockbillkind b on a.billkindcode=b.billkindcode where a.depot_code='" & Get_depot_code(Me.ComboBox1.Text.Trim) & "' and tempflag=0 and type=1"

            sqlcmd.CommandText = sql
            sqlad.SelectCommand = sqlcmd

            dt.Clear()
            sqlad.Fill(dt)

            Me.ComboBox2.Items.Clear()
            Me.ComboBox4.Items.Clear()

            Me.ComboBox3.Items.Clear()
            For i = 0 To dt.Rows.Count - 1
                Me.ComboBox2.Items.Add(dt.Rows(i)("billkindname"))
                Me.ComboBox4.Items.Add(dt.Rows(i)("billkindcode"))
            Next

            Me.ExDataGridView1.Columns.Clear()
            'Me.ExDataGridView1.DataSource = Nothing
            'Me.ExDataGridView1.DataMember = Nothing
            Me.ExDataGridView1.ClearDSDM()
        End If
    End Sub

    Private Sub ComboBox2_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ComboBox2.SelectedIndexChanged
        Dim sqlcon As New SqlConnection
        Dim sqlcmd As New SqlCommand
        Dim sqlad As New SqlDataAdapter

        Dim dt As New DataTable
        Dim sql As String

        Dim i As Int32

        If Me.ComboBox2.SelectedIndex <> -1 Then
            Me.ComboBox4.SelectedIndex = Me.ComboBox2.SelectedIndex

            sqlcon = getconn()
            sqlcmd.Connection = sqlcon

            sql = "SELECT distinct a.billno " & _
                    "FROM t_stockbill_master a left join  " & _
                    "	t_stockbillkind b on a.billkindcode=b.billkindcode where a.depot_code='" & Get_depot_code(Me.ComboBox1.Text.Trim) & "' and tempflag=0 and b.billkindcode='" & Me.ComboBox4.Text.Trim & "' "

            sqlcmd.CommandText = sql
            sqlad.SelectCommand = sqlcmd

            dt.Clear()
            sqlad.Fill(dt)

            Me.ComboBox3.Items.Clear()
            For i = 0 To dt.Rows.Count - 1
                Me.ComboBox3.Items.Add(dt.Rows(i)("billno"))
            Next

            Me.ExDataGridView1.Columns.Clear()
            'Me.ExDataGridView1.DataSource = Nothing
            'Me.ExDataGridView1.DataMember = Nothing
            Me.ExDataGridView1.ClearDSDM()
        End If
    End Sub

    Private Sub ComboBox3_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ComboBox3.SelectedIndexChanged
        Refresh_ExDataGridView()
    End Sub


    Private Sub Refresh_ExDataGridView()

        Dim sqlcon As New SqlConnection
        Dim sqlcmd As New SqlCommand
        Dim sqlad As New SqlDataAdapter


        Dim dt As New DataTable
        Dim sql As String

        If Me.ComboBox3.SelectedIndex = -1 Then
            Exit Sub
        End If



        sqlcon = getconn()
        sqlcmd.Connection = sqlcon



        sql = "select a.*,e.amount as kc_amount,b.item_name,b.item_standard,b.unit,f.Resourceno  " & _
            "FROM t_stockbill_detail a left join  " & _
            "             	t_item b on a.item_code=b.item_code left join  " & _
            " t_stockbill_master f on a.billno=f.billno left join " & _
            "		t_part e on a.item_code=e.item_code and  e.depot_code=f.depot_code  " & _
            "where a.billno='" & Me.ComboBox3.Text.Trim & "' and f.tempflag=0 and f.type=1"

        sqlcmd.CommandText = sql
        sqlad.SelectCommand = sqlcmd
        dt.Clear()
        sqlad.Fill(dt)


        Me.ExDataGridView1.SetDataSource(dt)

        Me.ExDataGridView1.AddColumn("billno", "���ݺ�", 120)
        Me.ExDataGridView1.AddColumn("item_code", "���ϱ���")
        Me.ExDataGridView1.AddColumn("item_name", "��������")
        Me.ExDataGridView1.AddColumn("item_standard", "���")
        Me.ExDataGridView1.AddColumn("unit", "��λ")

        Me.ExDataGridView1.AddColumn("Resourceno", "Դ���ݺ�", 120)
        Me.ExDataGridView1.AddColumn("Resourceid", "Դ����ID", 120, , , , , False)

        Me.ExDataGridView1.AddColumn("amount", "��������", 100, , , DataGridViewContentAlignment.MiddleRight, "#0.000")


        Me.ExDataGridView1.AddColumn("kc_amount", "�������", 100, , , DataGridViewContentAlignment.MiddleRight, "#0.000")
        Me.ExDataGridView1.AddColumn("price", "����", 100, , , DataGridViewContentAlignment.MiddleRight, "#0.000", False)


        Me.ExDataGridView1.AddColumn("item_code", "item_code", , , , , , False)


        Me.ExDataGridView1.AddColumn("id", "id", , , , , , False)
    End Sub

    Private Sub ToolStripButton_undo_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripButton_undo.Click
        If Me.ExDataGridView1.Rows.Count = 0 Then
            MsgBox("û�����ݣ��޷����ã�", MsgBoxStyle.OkOnly, "��ʾ")
            Exit Sub
        End If

        undo()
    End Sub


    Private Sub undo()
        Dim i As Int32
        Dim b As Boolean = False
        Dim k As Int32

        Dim resourceid As String

        Dim sqlcon As New SqlConnection
        Dim sqlcmd As New SqlCommand
        Dim sqlad As New SqlDataAdapter
        Dim mytrans As SqlTransaction

        Dim dt As New DataTable
        Dim sql As String = ""

        Dim accountterm As String = ""
        Dim depot_code As String = ""
        Dim billno As String = ""

        Dim q_amount As Decimal = 0
        Dim h_amount As Decimal = 0


        If MsgBox("ȷ��Ҫ���õ���Ϊ" & Me.ComboBox3.Text.Trim & "�ĵ�����", MsgBoxStyle.YesNo, "��ʾ") = MsgBoxResult.No Then
            Exit Sub
        End If

        If Month_end(Format(Me.DateTimePicker1.Value, "yyyy-MM-dd")) = False Then
            MsgBox("����ѡ���������ڲ��ڱ������ڣ�", MsgBoxStyle.OkOnly, "��ʾ")
            Exit Sub
        End If

        accountterm = GetAccountterm()
        depot_code = Get_depot_code(Me.ComboBox1.Text.Trim)

        If depot_code = "" Then
            MsgBox("�ֿ�ѡ����ȷ��ˢ�º����ԣ�", MsgBoxStyle.OkOnly, "��ʾ")
            Exit Sub
        End If

        sqlcon = getconn()
        sqlcmd.Connection = sqlcon


        Sql = "select stop from t_depot where depot_code='" & depot_code & "'"
        sqlcmd.CommandText = Sql
        If sqlcmd.ExecuteScalar = 0 Then
            MsgBox("�ֿ�Ϊͣ��״̬����������⣡", MsgBoxStyle.OkOnly, "��ʾ")
            Exit Sub
        End If






        mytrans = sqlcon.BeginTransaction()
        sqlcmd.Transaction = mytrans

        Try
            sql = "update t_dj_num set num=num+1 where  tt='CK-" & depot_code & "'"
            sqlcmd.CommandText = sql
            sqlcmd.ExecuteNonQuery()

            sql = "select * from t_dj_num where tt='CK-" & depot_code & "'"
            sqlcmd.CommandText = sql
            sqlad.SelectCommand = sqlcmd
            dt.Rows.Clear()
            sqlad.Fill(dt)

            billno = dt.Rows(0)("TT") & "-" & GetAccountterm() & "-" & Format(dt.Rows(0)("num"), dt.Rows(0)("format"))



            sql = "INSERT INTO t_stockbill_master ( AccountTerm,BillDate,BillKindCode,BillNo,Depot_Code,input_man,TempFlag,Type,Resourceno) values " & _
            "( '" & accountterm & "', '" & Format(Me.DateTimePicker1.Value, "yyyy-MM-dd") & "', '" & Me.ComboBox4.Text.Trim & "', '" & billno & "', '" & depot_code & "', '" & rs_name & "', '2', '1','" & Me.ComboBox3.Text.Trim & "')"

            sqlcmd.CommandText = sql
            sqlcmd.ExecuteNonQuery()

            sql = "update t_stockbill_master set tempflag=1 where  billno='" & Me.ComboBox3.Text.Trim & "'"
            sqlcmd.CommandText = sql
            sqlcmd.ExecuteNonQuery()



            For i = 0 To Me.ExDataGridView1.Rows.Count - 1


                sql = "update t_part set amount=amount + " & Me.ExDataGridView1.Rows(i).Cells("amount").Value & ",money=money + " & Me.ExDataGridView1.Rows(i).Cells("amount").Value * Me.ExDataGridView1.Rows(i).Cells("price").Value & " where item_code='" & Me.ExDataGridView1.Rows(i).Cells("item_code").Value & "'  and depot_code='" & depot_code & "'"
                sqlcmd.CommandText = sql
                sqlcmd.ExecuteNonQuery()





                sql = "INSERT INTO t_stockbill_detail ( Amount,BillNo,item_code,ResourceID,sourceflag,price,money) values " & _
                                                     "( '-" & Me.ExDataGridView1.Rows(i).Cells("amount").Value & "', " & _
                                                     "'" & billno & "',  '" & Me.ExDataGridView1.Rows(i).Cells("item_code").Value & "', " & _
                                                     "'" & Me.ExDataGridView1.Rows(i).Cells("id").Value & "',  '1','" & Me.ExDataGridView1.Rows(i).Cells("price").Value & "','" & Me.ExDataGridView1.Rows(i).Cells("price").Value * Me.ExDataGridView1.Rows(i).Cells("amount").Value & "')"
                sqlcmd.CommandText = sql
                sqlcmd.ExecuteNonQuery()

                sql = "select resourceid from t_stockbill_detail where id=" & Me.ExDataGridView1.Rows(i).Cells("id").Value
                sqlcmd.CommandText = sql
                resourceid = sqlcmd.ExecuteScalar

                '���ⵥ����
                If Me.ComboBox4.Text.Trim = "12" Then '���ⵥ�����ϣ�

                    sql = "select state from T_part_out_master where billno='" & Me.ExDataGridView1.Rows(i).Cells("Resourceno").Value & "'  "
                    sqlcmd.CommandText = sql
                    k = sqlcmd.ExecuteScalar()
                    If k = 0 Then
                        Throw New System.Exception("�˵���û����ˣ����֤���ٲ�����")

                    ElseIf k = 2 Then
                        Throw New System.Exception("�˵����Ѿ��᰸�����֤���ٲ�����")

                    End If

                    sql = "update T_part_out_detail set OK_amount=OK_amount - " & Me.ExDataGridView1.Rows(i).Cells("amount").Value & "  where id=" & resourceid
                    sqlcmd.CommandText = sql
                    sqlcmd.ExecuteNonQuery()
                ElseIf Me.ComboBox4.Text.Trim = "11" Then '���ⵥ���ƿ⣩

                    sql = "select state from B_TransferBill_master where billno='" & Me.ExDataGridView1.Rows(i).Cells("Resourceno").Value & "'  "
                    sqlcmd.CommandText = sql
                    k = sqlcmd.ExecuteScalar()
                    If k = 0 Then
                        Throw New System.Exception("�˵���û����ˣ����֤���ٲ�����")

                    ElseIf k = 2 Then
                        Throw New System.Exception("�˵����Ѿ��᰸�����֤���ٲ�����")

                    End If

                    sql = "update B_TransferBill_detail set CK_amount=CK_amount - " & Me.ExDataGridView1.Rows(i).Cells("amount").Value & "  where id=" & resourceid
                    sqlcmd.CommandText = sql
                    sqlcmd.ExecuteNonQuery()

                End If

            Next

            mytrans.Commit()
        Catch ex As Exception
            mytrans.Rollback()
            MsgBox(ex.Message, MsgBoxStyle.OkOnly, "��ʾ")
            Exit Sub
        End Try

        Refresh_ExDataGridView()
        MsgBox("���óɹ���", MsgBoxStyle.OkOnly, "��ʾ")


    End Sub
End Class