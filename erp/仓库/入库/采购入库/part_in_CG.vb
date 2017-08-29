Imports System.Data.SqlClient
Public Class part_in_CG

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
        Me.TreeView1.Nodes.Add("all", "采购订单", 0, 1)

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
            Me.ComboBox2.Items.Add(dt.Rows(i)("depot_code"))
        Next

        Me.ExDataGridView1.Columns.Clear()
        ' Me.ExDataGridView1.DataSource = Nothing
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

        sql = "select * from purchase_master where state=1 and billno in (SELECT billno FROM purchase_detail WHERE amount > OK_amount) and depot_code='" & s & "'"
        sqlcmd.CommandText = sql
        sqlad.SelectCommand = sqlcmd

        dt.Clear()
        sqlad.Fill(dt)

        Me.TreeView1.Nodes.Clear()

        Dim Tnode As TreeNode
        Dim Dnode As TreeNode = Nothing
        Dim Fnode As TreeNode = Nothing

        Me.TreeView1.Nodes.Clear()
        Tnode = Me.TreeView1.Nodes.Add("all", "采购单", 0, 1)


        For i = 0 To dt.Rows.Count - 1
            If ny <> Format(dt.Rows(i)("input_date"), "yyyy-MM-dd") Then
                Dnode = Tnode.Nodes.Add("D" & Format(dt.Rows(i)("input_date"), "yyyy-MM-dd"), Format(dt.Rows(i)("input_date"), "yyyy-MM-dd"), 0, 1)
            End If

            Fnode = Dnode.Nodes.Add("F" & dt.Rows(i)("billno").ToString.Trim, dt.Rows(i)("billno").ToString.Trim, 2, 2)

            ny = Format(dt.Rows(i)("input_date"), "yyyy-MM-dd")
        Next
        Me.TreeView1.Nodes(0).Expand()

    End Sub

    Private Sub part_in_CG_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Refresh_JM()
    End Sub

    Private Sub ComboBox1_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ComboBox1.SelectedIndexChanged
        Me.ComboBox2.SelectedIndex = Me.ComboBox1.SelectedIndex
        Refresh_TreeView(Me.ComboBox2.Text)
    End Sub

    Private Sub TreeView1_MouseClick(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles TreeView1.MouseClick
        Debug.Print(e.Button)
    End Sub

   

    Private Sub TreeView1_NodeMouseClick(ByVal sender As Object, ByVal e As System.Windows.Forms.TreeNodeMouseClickEventArgs) Handles TreeView1.NodeMouseClick

        If e.Node.Name.Substring(0, 1).ToUpper <> "F" Then
            Me.ExDataGridView1.Columns.Clear()
            'Me.ExDataGridView1.DataSource = Nothing
            'Me.ExDataGridView1.DataMember = Nothing
            Me.ExDataGridView1.ClearDSDM()
            Exit Sub

        End If


        Me.TreeView1.SelectedNode = e.Node
        Refresh_ExDataGridView(e.Node.Text)
    End Sub


    Private Sub Refresh_ExDataGridView(ByVal s As String)
        Dim sqlcon As New SqlConnection
        Dim sqlcmd As New SqlCommand
        Dim sqlad As New SqlDataAdapter


        Dim dt As New DataTable

        Dim sql As String



        sqlcon = getconn()
        'sqlcmd.Connection = sqlcon

        'sql = "select * from t_rkzz where type=1"

        'sqlcmd.CommandText = sql
        'sqlad.SelectCommand = sqlcmd
        'dt1.Clear()
        'sqlad.Fill(dt1)

        'Dim cell As New DataGridViewComboBoxColumn
        'cell.DataSource = dt1
        'cell.DisplayMember = "RKZZNAME"
        'cell.ValueMember = "rkzz"





        sql = "SELECT     a.*,  b.item_name, b.item_standard,b.unit,a.amount-ok_amount as RKSL ,c.input_man " & _
                "FROM         purchase_detail AS a LEFT OUTER JOIN " & _
                "                      t_item AS b ON a.item_code = b.item_code left join " & _
                "purchase_master c on a.billno=c.billno " & _
                "WHERE     (a.amount > a.OK_amount) and a.billno='" & s & "' "



        sqlcmd.Connection = sqlcon
        sqlcmd.CommandText = sql
        sqlad.SelectCommand = sqlcmd
        dt.Clear()
        sqlad.Fill(dt)


        Me.ExDataGridView1.SetDataSource(dt)

        Me.ExDataGridView1.SetDataSource(dt)

        Me.ExDataGridView1.AddColumn("billno", "供货单号", 80)
        Me.ExDataGridView1.AddColumn("item_code", "物料编码")
        Me.ExDataGridView1.AddColumn("item_name", "物料名称", 60)

        Me.ExDataGridView1.AddColumn("item_standard", "规格")

        Me.ExDataGridView1.AddColumn("unit", "单位")

        Me.ExDataGridView1.AddColumn("amount", "数量", 60, , , DataGridViewContentAlignment.MiddleRight, "#0.0000")
        If price_state = 0 Then
            Me.ExDataGridView1.AddColumn("price", "单价", 40, , , DataGridViewContentAlignment.MiddleRight, "#0.0000", False)

            Me.ExDataGridView1.AddColumn("money", "金额", 40, , , DataGridViewContentAlignment.MiddleRight, "#0.0000", False)
        Else
            Me.ExDataGridView1.AddColumn("price", "单价", 40, , , DataGridViewContentAlignment.MiddleRight, "#0.0000", True)

            Me.ExDataGridView1.AddColumn("money", "金额", 40, , , DataGridViewContentAlignment.MiddleRight, "#0.0000", True)
        End If
       


        Me.ExDataGridView1.AddColumn("ok_amount", "已收货数量", 80, , , DataGridViewContentAlignment.MiddleRight, "#0.000")
        Me.ExDataGridView1.AddColumn("rksl", "入库数量", 80, False, , DataGridViewContentAlignment.MiddleRight, "#0.000")
        Me.ExDataGridView1.AddColumn("input_man", "制单人")


        Me.ExDataGridView1.AddColumn("item_code", "item_code", , , , , , False)

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

        Dim q_amount As Decimal = 0
        Dim h_amount As Decimal = 0
        Dim E_money As Decimal = 0
        Dim k As int32

        Me.TextBox1.Focus()

        If Month_end(Format(Me.DateTimePicker1.Value, "yyyy-MM-dd")) = False Then
            MsgBox("您所选择的入库日期不在本帐期内！", MsgBoxStyle.OkOnly, "提示")
            Exit Sub
        End If

        If Me.ExDataGridView1.Rows.Count = 0 Then
            MsgBox("请选择需要入库的数据！", MsgBoxStyle.OkOnly, "提示")
            Exit Sub
        End If

        For i = 0 To Me.ExDataGridView1.Rows.Count - 1
            If Me.ExDataGridView1.Rows(i).Cells("rksl").Value <> 0 Then
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


        sql = "SELECT state " & _
                      " FROM   purchase_master where billno='" & Me.TreeView1.SelectedNode.Text & "'"


        sqlcmd.CommandText = sql
        k = sqlcmd.ExecuteScalar

        If k = 2 Then
            MsgBox("该单据已经结案，请检查后操作！", MsgBoxStyle.OkOnly, "提示")
            Exit Sub
        ElseIf k = 0 Then
            MsgBox("该单据没有审核，请检查后操作！", MsgBoxStyle.OkOnly, "提示")
            Exit Sub
        End If

        mytrans = sqlcon.BeginTransaction()
        sqlcmd.Transaction = mytrans

        Try
            Me.ToolStripButton_save.Enabled = False

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
            "( '" & accountterm & "', '" & Format(Me.DateTimePicker1.Value, "yyyy-MM-dd") & "', '20', '" & billno & "', '" & depot_code & "', '" & rs_name & "', '0', '2', '" & Me.TreeView1.SelectedNode.Text.Trim & "')"
            sqlcmd.CommandText = sql
            sqlcmd.ExecuteNonQuery()

            For i = 0 To Me.ExDataGridView1.Rows.Count - 1
                If Me.ExDataGridView1.Rows(i).Cells("rksl").Value <> 0 Then

                    sql = "update   purchase_detail set ok_amount=OK_amount+" & Me.ExDataGridView1.Rows(i).Cells("rksl").Value & "   where id=" & Me.ExDataGridView1.Rows(i).Cells("id").Value
                    sqlcmd.CommandText = sql
                    sqlcmd.ExecuteNonQuery()

                    sql = "select ok_amount-amount from purchase_detail  where id=" & Me.ExDataGridView1.Rows(i).Cells("id").Value
                    sqlcmd.CommandText = sql


                    If sqlcmd.ExecuteScalar() = 0 Then
                        sql = "select a.money -a.ok_money " & _
                               "from purchase_detail a " & _
                               "where a.id=" & Me.ExDataGridView1.Rows(i).Cells("id").Value
                        sqlcmd.CommandText = sql
                        E_money = Format(sqlcmd.ExecuteScalar, "#0.00")

                        sql = "update   purchase_detail set ok_money=money  where id=" & Me.ExDataGridView1.Rows(i).Cells("id").Value
                        sqlcmd.CommandText = sql
                        sqlcmd.ExecuteNonQuery()

                    Else
                        E_money = Format(Me.ExDataGridView1.Rows(i).Cells("rksl").Value * Me.ExDataGridView1.Rows(i).Cells("price").Value, "#0.00")

                        sql = "update   purchase_detail set ok_money=OK_money +" & E_money & "   where id=" & Me.ExDataGridView1.Rows(i).Cells("id").Value
                        sqlcmd.CommandText = sql
                        sqlcmd.ExecuteNonQuery()
                    End If

                    


                    sql = "select count(*) from t_part where item_code='" & Me.ExDataGridView1.Rows(i).Cells("item_code").Value & "'  and depot_code='" & depot_code & "'"
                    sqlcmd.CommandText = sql

                    If sqlcmd.ExecuteScalar() = 0 Then
                        sql = "INSERT INTO T_part ( AccountTerm,amount,Depot_Code,item_code,qc_amount,price,money,end_date) values " & _
                            "( '" & accountterm & "', '" & Me.ExDataGridView1.Rows(i).Cells("rksl").Value & "', '" & depot_code & "'," & _
                            "'" & Me.ExDataGridView1.Rows(i).Cells("item_code").Value & "', '0', '" & Me.ExDataGridView1.Rows(i).Cells("price").Value & "'," & _
                            "'" & E_money & "',getdate())"
                        '"'" & Me.ExDataGridView1.Rows(i).Cells("rksl").Value * Me.ExDataGridView1.Rows(i).Cells("price").Value & "',getdate())"
                        sqlcmd.CommandText = sql
                        sqlcmd.ExecuteNonQuery()


                    Else
                        sql = "update t_part set amount=amount+" & Me.ExDataGridView1.Rows(i).Cells("rksl").Value & ",money=money+" & E_money & ",end_date=getdate() where item_code='" & Me.ExDataGridView1.Rows(i).Cells("item_code").Value & _
                                "' and depot_code='" & depot_code & "'"
                        sqlcmd.CommandText = sql
                        sqlcmd.ExecuteNonQuery()

                        sql = "update t_part set price=(case when money<0 then 0 else money end)/(case when amount=0 then 1 else amount end ) where item_code='" & Me.ExDataGridView1.Rows(i).Cells("item_code").Value & _
                                "' and depot_code='" & depot_code & "'"
                        sqlcmd.CommandText = sql
                        sqlcmd.ExecuteNonQuery()

                    End If



                    sql = "INSERT INTO t_stockbill_detail ( Amount,BillNo,item_code,ResourceID,sourceflag,price,money) values " & _
                                                         "( '" & Me.ExDataGridView1.Rows(i).Cells("rksl").Value & "'," & _
                                                         "'" & billno & "', '" & Me.ExDataGridView1.Rows(i).Cells("item_code").Value & "', " & _
                                                         "'" & Me.ExDataGridView1.Rows(i).Cells("id").Value & "', '1','" & Me.ExDataGridView1.Rows(i).Cells("price").Value & "','" & E_money & "')"
                    sqlcmd.CommandText = sql
                    sqlcmd.ExecuteNonQuery()



                End If
            Next

            sql = "update a set a.money=b.money  " & _
                "from t_stockbill_master a left join  " & _
                "	(select billno,sum(money) as money from t_stockbill_detail where billno='" & billno & "' group by billno) b on a.billno=b.billno " & _
                "where a.type=2 and a.billkindcode=20 and tempflag=0 and a.billno='" & billno & "'"
            sqlcmd.CommandText = sql
            sqlcmd.ExecuteNonQuery()


            mytrans.Commit()

        Catch ex As Exception
            mytrans.Rollback()
            Me.ToolStripButton_save.Enabled = True
            MsgBox(ex.Message & sql, MsgBoxStyle.OkOnly, "提示")
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

    Private Sub ToolStripButton_exit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripButton_exit.Click
        Me.Close()
    End Sub



End Class