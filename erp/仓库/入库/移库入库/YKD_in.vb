Imports System.Data.SqlClient
Public Class YKD_in

    Private Sub ToolStripButton_exit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripButton_exit.Click
        Me.Close()

    End Sub

    Private Sub ToolStripButton_refresh_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripButton_refresh.Click

        Refresh_JM()
    End Sub

    Private Sub Refresh_JM()
        Dim sqlcon As New SqlConnection
        Dim sqlcmd As New SqlCommand
        Dim sqlad As New SqlDataAdapter


        Dim dt As New DataTable
        Dim sql As String
        Dim i As Integer

        Me.TreeView1.Nodes.Clear()
        Me.TreeView1.Nodes.Add("all", "移库单", 0, 1)

        Me.ComboBox1.Items.Clear()

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

    Private Sub Refresh_TreeView(Optional ByVal s As String = "")
        Dim sqlcon As New SqlConnection
        Dim sqlcmd As New SqlCommand
        Dim sqlad As New SqlDataAdapter


        Dim dt As New DataTable
        Dim sql As String
        Dim i As Integer
        Dim ny As String = ""

        sqlcon = getconn()

        sqlcmd.Connection = sqlcon

        sql = "select * from B_TransferBill_master where  aim_depot='" & s & "' and state=1 and billno in (select billno from B_TransferBill_detail where ck_amount>rk_amount)"
        sqlcmd.CommandText = sql
        sqlad.SelectCommand = sqlcmd

        dt.Clear()
        sqlad.Fill(dt)

        Me.TreeView1.Nodes.Clear()

        Dim Tnode As TreeNode
        Dim Fnode As TreeNode = Nothing


        Me.TreeView1.Nodes.Clear()
        Tnode = Me.TreeView1.Nodes.Add("all", "移库单", 0, 1)


        For i = 0 To dt.Rows.Count - 1

            Fnode = Tnode.Nodes.Add("F" & dt.Rows(i)("billno"), dt.Rows(i)("billno"), 2, 2)

        Next
        Me.TreeView1.Nodes(0).Expand()

    End Sub

    Private Sub YKD_in_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Refresh_JM()
    End Sub

    Private Sub ComboBox1_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ComboBox1.SelectedIndexChanged
        Refresh_TreeView(Get_depot_code(Me.ComboBox1.Text.Trim))
    End Sub

    Private Sub TreeView1_AfterSelect(ByVal sender As System.Object, ByVal e As System.Windows.Forms.TreeViewEventArgs) Handles TreeView1.AfterSelect

    End Sub

    Private Sub TreeView1_NodeMouseClick(ByVal sender As Object, ByVal e As System.Windows.Forms.TreeNodeMouseClickEventArgs) Handles TreeView1.NodeMouseClick
        If e.Node.Name.Substring(0, 1).ToUpper <> "F" Then
            Me.ExDataGridView1.Columns.Clear()
            'Me.ExDataGridView1.DataSource = Nothing
            'Me.ExDataGridView1.DataMember = Nothing
            Me.ExDataGridView1.ClearDSDM()
            Exit Sub

        End If

        Refresh_ExDataGridView(e.Node.Text)
    End Sub

    Private Sub Refresh_ExDataGridView(ByVal s As String)
        Dim sqlcon As New SqlConnection
        Dim sqlcmd As New SqlCommand
        Dim sqlad As New SqlDataAdapter


        Dim dt As New DataTable
        Dim sql As String


        sqlcon = getconn()



        sql = "SELECT a.*,b.item_name,b.item_standard,b.unit ,e.amount as kc_amount,a.ck_amount-a.rk_amount as wrk_amount,a.ck_amount-a.rk_amount as bc_amount,g.depot_name as CK_depot,h.depot_name as RK_depot,'入库' as rkzzname,0 as rkzz " & _
                        "FROM B_TransferBill_detail a left join " & _
                        "t_item b on a.item_code=b.item_code left join  " & _
                        " B_TransferBill_master f on a.billno=f.billno left join " & _
                        " t_depot g on f.source_depot=g.depot_code left join " & _
                        " t_depot h on f.aim_depot=h.depot_code left join " & _
                        "   t_part e on a.item_code=e.item_code  and e.depot_code=f.aim_depot " & _
                        " where a.rk_amount<a.ck_amount and a.billno='" & s & "'"


        sqlcmd.Connection = sqlcon
        sqlcmd.CommandText = sql
        sqlad.SelectCommand = sqlcmd
        dt.Clear()
        sqlad.Fill(dt)


        Me.ExDataGridView1.SetDataSource(dt)

        Me.ExDataGridView1.AddColumn("billno", "单据号", 120)
        Me.ExDataGridView1.AddColumn("item_code", "物料编码", 120)
        Me.ExDataGridView1.AddColumn("item_name", "物料名称")
        Me.ExDataGridView1.AddColumn("item_standard", "规格")
        Me.ExDataGridView1.AddColumn("unit", "单位")

        Me.ExDataGridView1.AddColumn("ck_depot", "移出仓库")
        Me.ExDataGridView1.AddColumn("rk_depot", "移入仓库")

        Me.ExDataGridView1.AddColumn("amount", "调拨数量", , , , DataGridViewContentAlignment.MiddleRight, "#0.0000")
        Me.ExDataGridView1.AddColumn("ck_amount", "已出库数量", 100, , , DataGridViewContentAlignment.MiddleRight, "#0.0000")
        Me.ExDataGridView1.AddColumn("rk_amount", "已入库数量", 100, , , DataGridViewContentAlignment.MiddleRight, "#0.0000")
        Me.ExDataGridView1.AddColumn("wrk_amount", "未入库数量", 100, , , DataGridViewContentAlignment.MiddleRight, "#0.0000")
        Me.ExDataGridView1.AddColumn("bc_amount", "将入库数量", 100, , , DataGridViewContentAlignment.MiddleRight, "#0.0000")
        'Me.ExDataGridView1.AddColumn("kc_amount", "库存数量", , , , DataGridViewContentAlignment.MiddleRight, "#0.0000")

        Me.ExDataGridView1.AddColumn("RKZZname", "入库类型")

        Me.ExDataGridView1.AddColumn("source_depot", "source_depot", , , , , , False)
        Me.ExDataGridView1.AddColumn("aun_depot", "aun_depot", , , , , , False)

        Me.ExDataGridView1.AddColumn("price", "price", , , , , , False)
        Me.ExDataGridView1.AddColumn("money", "money", , , , , , False)


        Me.ExDataGridView1.AddColumn("id", "id", , , , , , False)
    End Sub

    Private Sub ToolStripButton_save_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripButton_save.Click
        Dim i As int32
        Dim b As Boolean = False

        Dim sqlcon As New SqlConnection
        Dim sqlcmd As New SqlCommand
        Dim sqlad As New SqlDataAdapter


        Dim mytrans As SqlTransaction

        Dim dt As New DataTable
        Dim sql As String = ""

        Dim accountterm As String = ""
        Dim depot_code As String = ""
        Dim billno As String = ""

        Dim B_price As Decimal = 0
        Dim B_money As Decimal = 0


        If Month_end(Format(Me.DateTimePicker1.Value, "yyyy-MM-dd")) = False Then
            MsgBox("您所选择的入库日期不在本帐期内！", MsgBoxStyle.OkOnly, "提示")
            Exit Sub
        End If

        If Me.ExDataGridView1.Rows.Count = 0 Then
            MsgBox("请选择需要入库的数据！", MsgBoxStyle.OkOnly, "提示")
            Exit Sub
        End If

        For i = 0 To Me.ExDataGridView1.Rows.Count - 1
            If Me.ExDataGridView1.Rows(i).Cells("bc_amount").Value <> 0 Then
                b = True
                Exit For
            End If
        Next

        If b = False Then
            MsgBox("请输入要入库的数量！", MsgBoxStyle.OkOnly, "提示")
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

        sql = "select state from B_TransferBill_master where billno='" & Me.TreeView1.SelectedNode.Text.Trim & "'  "
        sqlcmd.CommandText = sql
        i = sqlcmd.ExecuteScalar
        If i = 0 Then
            MsgBox("此单据没有审核，请查证后再入库！", MsgBoxStyle.OkOnly, "提示")
            Exit Sub
        ElseIf i = 2 Then
            MsgBox("此单据已经结案，请查证后再入库！", MsgBoxStyle.OkOnly, "提示")
            Exit Sub
        End If

        Me.ToolStripButton_save.Enabled = False

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
            "( '" & accountterm & "', '" & Format(Me.DateTimePicker1.Value, "yyyy-MM-dd") & "', '22', '" & billno & "', '" & depot_code & "', '" & rs_name & "', '0', '2', '" & Me.TreeView1.SelectedNode.Text.Trim & "')"
            sqlcmd.CommandText = sql
            sqlcmd.ExecuteNonQuery()

            For i = 0 To Me.ExDataGridView1.Rows.Count - 1
                If Me.ExDataGridView1.Rows(i).Cells("bc_amount").Value <> 0 Then
                    sql = "select count(*) from t_part where item_code='" & Me.ExDataGridView1.Rows(i).Cells("item_code").Value & "'  and depot_code='" & depot_code & "'"
                    sqlcmd.CommandText = sql

                    If sqlcmd.ExecuteScalar() = 0 Then
                        sql = "INSERT INTO T_part ( AccountTerm,amount,Depot_Code,item_code,qc_amount,end_date,price,money) values " & _
                            "( '" & accountterm & "', '" & Me.ExDataGridView1.Rows(i).Cells("bc_amount").Value & "','" & depot_code & "'," & _
                            "'" & Me.ExDataGridView1.Rows(i).Cells("item_code").Value & "', '0',getdate(),'" & Me.ExDataGridView1.Rows(i).Cells("price").Value & "','" & Me.ExDataGridView1.Rows(i).Cells("bc_amount").Value * Me.ExDataGridView1.Rows(i).Cells("price").Value & "')"
                        sqlcmd.CommandText = sql
                        sqlcmd.ExecuteNonQuery()

                       
                    Else
                        sql = "update t_part set amount=amount+" & Me.ExDataGridView1.Rows(i).Cells("bc_amount").Value & ",money=money + '" & Me.ExDataGridView1.Rows(i).Cells("bc_amount").Value * Me.ExDataGridView1.Rows(i).Cells("price").Value & "',end_date=getdate() where item_code='" & Me.ExDataGridView1.Rows(i).Cells("item_code").Value & "'  and depot_code='" & depot_code & "'"
                        sqlcmd.CommandText = sql
                        sqlcmd.ExecuteNonQuery()

                        sql = "update t_part set price=(case when money<0 then 0 else money end)/(case when amount=0 then 1 else amount end ) where item_code='" & Me.ExDataGridView1.Rows(i).Cells("item_code").Value & _
                                 "' and depot_code='" & depot_code & "'"
                        sqlcmd.CommandText = sql
                        sqlcmd.ExecuteNonQuery()


                    End If



                    sql = "INSERT INTO t_stockbill_detail ( Amount,BillNo,item_code,ResourceID,sourceflag,price,money) values " & _
                                                         "( '" & Me.ExDataGridView1.Rows(i).Cells("bc_amount").Value & "', " & _
                                                         "'" & billno & "',  '" & Me.ExDataGridView1.Rows(i).Cells("item_code").Value & "', " & _
                                                         "'" & Me.ExDataGridView1.Rows(i).Cells("id").Value & "', '1','" & Me.ExDataGridView1.Rows(i).Cells("price").Value & "','" & Me.ExDataGridView1.Rows(i).Cells("bc_amount").Value * Me.ExDataGridView1.Rows(i).Cells("price").Value & "')"
                    sqlcmd.CommandText = sql
                    sqlcmd.ExecuteNonQuery()


                    sql = "update B_TransferBill_detail set rk_amount=rk_amount + " & Me.ExDataGridView1.Rows(i).Cells("bc_amount").Value & "  where id=" & Me.ExDataGridView1.Rows(i).Cells("id").Value
                    sqlcmd.CommandText = sql
                    sqlcmd.ExecuteNonQuery()
                End If
            Next

            mytrans.Commit()
        Catch ex As Exception
            mytrans.Rollback()
            Me.ToolStripButton_save.Enabled = True
            MsgBox(ex.Message, MsgBoxStyle.OkOnly, "提示")
            Exit Sub
        End Try

        Refresh_ExDataGridView(Me.TreeView1.SelectedNode.Text)


        Dim s As String
        s = Print_in(billno)
        If s = "" Then
            Me.ToolStripButton_save.Enabled = True
            MsgBox("保存成功！", MsgBoxStyle.OkOnly, "提示")
        Else
            Me.ToolStripButton_save.Enabled = True
            MsgBox(s, MsgBoxStyle.OkOnly, "提示")
        End If
    End Sub
End Class