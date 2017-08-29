Imports System.Data.SqlClient
Public Class warehouse_undo

    Private Sub ToolStripButton_exit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripButton_exit.Click
        Me.Close()
    End Sub


    Private Sub warehouse_undo_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Refresh_all()
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

    Private Sub ToolStripButton_refresh_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripButton_refresh.Click
        Refresh_all()
    End Sub

    Private Sub ComboBox1_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ComboBox1.SelectedIndexChanged
        Dim sqlcon As New SqlConnection
        Dim sqlcmd As New SqlCommand
        Dim sqlad As New SqlDataAdapter

        Dim dt As New DataTable
        Dim sql As String

        Dim i As int32

        If Me.ComboBox1.SelectedIndex <> -1 Then
            sqlcon = getconn()
            sqlcmd.Connection = sqlcon

            sql = "SELECT distinct b.billkindname,a.billkindcode " & _
                    "FROM t_stockbill_master a left join  " & _
                    "	t_stockbillkind b on a.billkindcode=b.billkindcode where a.depot_code='" & Get_depot_code(Me.ComboBox1.Text.Trim) & "' and tempflag=0 and type=2"

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

        Dim i As int32

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

    Private Sub ComboBox3_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ComboBox3.SelectedIndexChanged
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
            "             	t_item b on a.item_code=b.item_code left join   " & _
            " t_stockbill_master f on a.billno=f.billno left join " & _
            "		t_part e on   a.item_code=e.item_code and  f.depot_code=e.depot_code  " & _
            " where a.billno='" & Me.ComboBox3.Text.Trim & "' and f.tempflag=0 and f.type=2 "

        sqlcmd.CommandText = sql
        sqlad.SelectCommand = sqlcmd
        dt.Clear()
        sqlad.Fill(dt)


        Me.ExDataGridView1.SetDataSource(dt)

        Me.ExDataGridView1.AddColumn("billno", "单据号", 120)
        Me.ExDataGridView1.AddColumn("item_code", "物料编码")
        Me.ExDataGridView1.AddColumn("item_name", "物料名称")

        Me.ExDataGridView1.AddColumn("item_standard", "规格")
        Me.ExDataGridView1.AddColumn("unit", "单位")


        Me.ExDataGridView1.AddColumn("Resourceno", "源单据号", 120)
        Me.ExDataGridView1.AddColumn("Resourceid", "源单据ID", 120, , , , , False)

        Me.ExDataGridView1.AddColumn("amount", "重置数量", 100, , , DataGridViewContentAlignment.MiddleRight, "#0.000")

        Me.ExDataGridView1.AddColumn("kc_amount", "库存数量", 100, , , DataGridViewContentAlignment.MiddleRight, "#0.000")

        Me.ExDataGridView1.AddColumn("price", "单价", 100, , , DataGridViewContentAlignment.MiddleRight, "#0.000", False)
        Me.ExDataGridView1.AddColumn("money", "金额", 100, , , DataGridViewContentAlignment.MiddleRight, "#0.000", False)

        Me.ExDataGridView1.AddColumn("item_code", "item_code", , , , , , False)

        Me.ExDataGridView1.AddColumn("id", "id", , , , , , False)
    End Sub

    Private Sub ToolStripButton_undo_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripButton_undo.Click
        If Me.ExDataGridView1.Rows.Count = 0 Then
            MsgBox("没有数据，无法重置！", MsgBoxStyle.OkOnly, "提示")
            Exit Sub
        End If

        undo()
    End Sub



    Private Sub undo()
        Dim i As int32
        Dim b As Boolean = False
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


        If MsgBox("确认要重置单号为" & Me.ComboBox3.Text.Trim & "的单据吗？", MsgBoxStyle.YesNo, "提示") = MsgBoxResult.No Then
            Exit Sub
        End If

        If Month_end(Format(Me.DateTimePicker1.Value, "yyyy-MM-dd")) = False Then
            MsgBox("您所选择的入库日期不在本帐期内！", MsgBoxStyle.OkOnly, "提示")
            Exit Sub
        End If

        accountterm = GetAccountterm()
        depot_code = Get_depot_code(Me.ComboBox1.Text.Trim)

        If depot_code = "" Then
            MsgBox("仓库选择不正确，刷新后再试！", MsgBoxStyle.OkOnly, "提示")
            Exit Sub
        End If

        sqlcon = getconn()
        sqlcmd.Connection = sqlcon


        sql = "select stop from t_depot where depot_code='" & depot_code & "'"
        sqlcmd.CommandText = sql
        If sqlcmd.ExecuteScalar = 0 Then
            MsgBox("仓库为停用状态，不可以入库！", MsgBoxStyle.OkOnly, "提示")
            Exit Sub
        End If

        sql = "select count(*) from t_stockbill_master where billno='" & billno & "' and kp_money>0"
        sqlcmd.CommandText = sql
        If sqlcmd.ExecuteScalar > 0 Then
            MsgBox("该单据已经有开票数量，不可以重置！", MsgBoxStyle.OkOnly, "提示")
            Exit Sub
        End If




        mytrans = sqlcon.BeginTransaction()
        sqlcmd.Transaction = mytrans

        Try
            sql = "update t_dj_num set num=num+1 where  tt='RK-" & depot_code & "'"
            sqlcmd.CommandText = sql
            sqlcmd.ExecuteNonQuery()

            sql = "select * from t_dj_num where tt='RK-" & depot_code & "'"
            sqlcmd.CommandText = sql
            sqlad.SelectCommand = sqlcmd
            dt.Rows.Clear()
            sqlad.Fill(dt)

            billno = dt.Rows(0)("TT") & "-" & GetAccountterm() & "-" & Format(dt.Rows(0)("num"), dt.Rows(0)("format"))



            sql = "INSERT INTO t_stockbill_master ( AccountTerm,BillDate,BillKindCode,BillNo,Depot_Code,input_man,TempFlag,Type,ResourceNo) values " & _
            "( '" & accountterm & "', '" & Format(Me.DateTimePicker1.Value, "yyyy-MM-dd") & "', '" & Me.ComboBox4.Text.Trim & "', '" & billno & "', '" & depot_code & "', '" & rs_name & "', '2', '2', '" & Me.ComboBox3.Text.Trim & "')"

            sqlcmd.CommandText = sql
            sqlcmd.ExecuteNonQuery()

            sql = "update t_stockbill_master set tempflag=1 where  billno='" & Me.ComboBox3.Text.Trim & "'"
            sqlcmd.CommandText = sql
            sqlcmd.ExecuteNonQuery()



            For i = 0 To Me.ExDataGridView1.Rows.Count - 1


                sql = "update t_part set amount=amount - " & Me.ExDataGridView1.Rows(i).Cells("amount").Value & ",money=case when money - " & Me.ExDataGridView1.Rows(i).Cells("amount").Value * Me.ExDataGridView1.Rows(i).Cells("price").Value & " <0 then 0 else money - " & Me.ExDataGridView1.Rows(i).Cells("amount").Value * Me.ExDataGridView1.Rows(i).Cells("price").Value & " end where item_code='" & Me.ExDataGridView1.Rows(i).Cells("item_code").Value & _
                        "' and depot_code='" & depot_code & "'"
                sqlcmd.CommandText = sql
                sqlcmd.ExecuteNonQuery()

                sql = "update t_part set money=0 where money<0"
                sqlcmd.CommandText = sql
                sqlcmd.ExecuteNonQuery()




                sql = "INSERT INTO t_stockbill_detail ( Amount,BillNo,item_code,ResourceID,sourceflag,price,money) values " & _
                                                     "( '-" & Me.ExDataGridView1.Rows(i).Cells("amount").Value & "', " & _
                                                     "'" & billno & "',   '" & Me.ExDataGridView1.Rows(i).Cells("item_code").Value & "', " & _
                                                     "'" & Me.ExDataGridView1.Rows(i).Cells("id").Value & "', '1','" & Me.ExDataGridView1.Rows(i).Cells("price").Value & "','" & Me.ExDataGridView1.Rows(i).Cells("price").Value * Me.ExDataGridView1.Rows(i).Cells("amount").Value & "')"
                sqlcmd.CommandText = sql
                sqlcmd.ExecuteNonQuery()

                sql = "select resourceid from t_stockbill_detail where id=" & Me.ExDataGridView1.Rows(i).Cells("id").Value
                sqlcmd.CommandText = sql
                resourceid = sqlcmd.ExecuteScalar

                '入库单(采购)重置
                If Me.ComboBox4.Text.Trim = "20" Then
                    sql = "update purchase_detail set ok_amount=ok_amount - " & Me.ExDataGridView1.Rows(i).Cells("amount").Value & ",ok_money=ok_money -  " & Me.ExDataGridView1.Rows(i).Cells("money").Value & "  where id=" & resourceid
                    sqlcmd.CommandText = sql
                    sqlcmd.ExecuteNonQuery()
                ElseIf Me.ComboBox4.Text.Trim = "22" Then '移库入库
                    sql = "update  B_TransferBill_detail set RK_amount=RK_amount - " & Me.ExDataGridView1.Rows(i).Cells("amount").Value & "  where id=" & resourceid
                    sqlcmd.CommandText = sql
                    sqlcmd.ExecuteNonQuery()
                ElseIf Me.ComboBox4.Text.Trim = "23" Then '退库入库
                    sql = "update  t_part_in_detail set ok_amount=ok_amount - " & Me.ExDataGridView1.Rows(i).Cells("amount").Value & "  where id=" & resourceid
                    sqlcmd.CommandText = sql
                    sqlcmd.ExecuteNonQuery()
                End If

            Next

            mytrans.Commit()
        Catch ex As Exception
            mytrans.Rollback()
            MsgBox(ex.Message, MsgBoxStyle.OkOnly, "提示")
            Exit Sub
        End Try

        Refresh_ExDataGridView()
        MsgBox("重置成功！", MsgBoxStyle.OkOnly, "提示")


    End Sub
End Class