Imports System.Windows.Forms
Imports System.Data.SqlClient

Public Class CK_ZHCX_cha

    Private Sub OK_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OK_Button.Click
        Dim sqlcon As New SqlConnection
        Dim sqlcmd As New SqlCommand
        Dim sqlad As New SqlDataAdapter

        Dim dt As New DataTable
        Dim sql As String
        Dim i As int32


        Dim StrGroup As String = ""
        Dim StrCol_Group As String = ""
        Dim StrCol_NOGroup As String = ""
        Dim StrDepot As String = ""
        Dim StrDepot_NO As String = ""
        Dim StrKind As String = ""


        sql = "SELECT StrCol  " & _
        "FROM " & IIf(Me.RadioButton1.Checked, "t_StockBill_Detail", "t_StockBill_Detail_history") & " a left JOIN " & _
              IIf(Me.RadioButton1.Checked, "t_StockBill_master", "t_StockBill_master_history") & " b ON a.BillNo = b.BillNo left JOIN " & _
              "	t_item c ON a.item_code = c.item_code left join   " & _
              " t_depot f on b.depot_code=f.depot_code left join  " & _
              "	T_StockBillKind g on b.billkindcode=g.billkindcode " & _
              " where  StrDepot StrKind b.Billdate between '" & Format(Me.DateTimePicker1.Value, "yyyy-MM-dd") & " 00:00:00' and '" & Format(Me.DateTimePicker2.Value, "yyyy-MM-dd") & " 23:59:59' and b.tempflag not in ('1','2') StrGroup"


        '---分组
        For i = 0 To Me.ListView2.Items.Count - 1
            If Me.ListView2.Items(i).Checked Then
                'FZ = FZ + 1
                StrGroup = StrGroup & Me.ListView2.Items(i).Name & ","
                StrCol_Group = StrCol_Group & Me.ListView2.Items(i).Tag & ","
            Else
                StrCol_NOGroup = StrCol_NOGroup & Me.ListView2.Items(i).Tag & ","
            End If
        Next

        If Len(Trim(StrGroup)) > 0 Then
            StrGroup = Microsoft.VisualBasic.Left(StrGroup, Len(StrGroup) - 1)
            If price_state = 1 Then
                StrCol_Group = StrCol_Group & "Sum(a.Amount) as 数量,sum(a.money) as 金额"
            Else
                StrCol_Group = StrCol_Group & "Sum(a.Amount) as 数量"
            End If


            sql = Replace(sql, "StrGroup", "Group by " & StrGroup)
            sql = Replace(sql, "StrCol", StrCol_Group)
        Else
            If price_state = 1 Then
                StrCol_NOGroup = StrCol_NOGroup & "a.Amount as 数量,a.price as 单价,a.money as 金额 ,resourceNO as 源单据号"
            Else
                StrCol_NOGroup = StrCol_NOGroup & "a.Amount as 数量 ,resourceNO as 源单据号"
            End If



            sql = Replace(sql, "StrGroup", " ")
            sql = Replace(sql, "StrCol", StrCol_NOGroup)
        End If

        '仓库
        For i = 0 To Me.ListView1.Items.Count - 1
            If Me.ListView1.Items(i).Checked Then
                'CK = CK + 1
                StrDepot = StrDepot & "'" & Me.ListView1.Items(i).Text & "',"
            Else
                StrDepot_NO = StrDepot_NO & "'" & Me.ListView1.Items(i).Text & "',"
            End If
        Next
        If Len(Trim(StrDepot)) > 0 Then
            StrDepot = Microsoft.VisualBasic.Left(StrDepot, Len(StrDepot) - 1)
            sql = Replace(sql, "StrDepot", "b.Depot_Code in (" & StrDepot & ") and ")
        Else
            StrDepot_NO = Microsoft.VisualBasic.Left(StrDepot_NO, Len(StrDepot_NO) - 1)
            sql = Replace(sql, "StrDepot", "b.Depot_code in (" & StrDepot_NO & ") and ")
        End If

        '--类型
        For i = 0 To Me.ListView3.Items.Count - 1
            If Me.ListView3.Items(i).Checked Then
                'LX = LX + 1
                StrKind = StrKind & "'" & Me.ListView3.Items(i).Text & "',"
            End If
        Next
        If Len(Trim(StrKind)) > 0 Then
            StrKind = Microsoft.VisualBasic.Left(StrKind, Len(StrKind) - 1)
            sql = Replace(sql, "StrKind", "b.BillKindCode in (" & StrKind & ") and ")
        Else
            sql = Replace(sql, "StrKind", " ")
        End If

        'If FZ = 0 And CK = 1 And LX = Me.ListView3.Items.Count And Me.CheckBox1.Checked And Me.CheckBox2.Checked Then
        '    sql = Replace(sql, "Col2", ",q_amount as 操作之前数量,h_amount as 操作之后数量")
        'Else
        '    sql = Replace(sql, "Col2", " ")
        'End If

        CType(Me.Owner, CK_ZHCX).cha = sql

        Me.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.Close()
    End Sub

    Private Sub Cancel_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cancel_Button.Click
        Me.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub

    Private Sub CK_ZHCX_cha_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Dim sqlcon As New SqlConnection
        Dim sqlcmd As New SqlCommand
        Dim sqlad As New SqlDataAdapter

        Dim dt As New DataTable
        Dim sql As String
        Dim i As Int32

        Dim lv1 As New ListViewItem


        Me.ListView1.Columns.Clear()
        Me.ListView1.Columns.Add("仓库编号", 68, HorizontalAlignment.Left)
        Me.ListView1.Columns.Add("仓库名称", 128, HorizontalAlignment.Left)

        Me.ListView2.Columns.Clear()
        Me.ListView2.Columns.Add("依据", 128, HorizontalAlignment.Left)



        Me.ListView3.Columns.Clear()
        Me.ListView3.Columns.Add("类型编号", 68, HorizontalAlignment.Left)
        Me.ListView3.Columns.Add("类型名称", 128, HorizontalAlignment.Left)
        sqlcon = getconn()

        '仓库
        Me.ListView1.Items.Clear()
        sql = "select * from t_depot where stop=1 order by depot_code"
        sqlcmd.Connection = sqlcon
        sqlcmd.CommandText = sql
        sqlad.SelectCommand = sqlcmd
        dt.Rows.Clear()
        sqlad.Fill(dt)

        For i = 0 To dt.Rows.Count - 1

            lv1 = Me.ListView1.Items.Add(dt.Rows(i)("depot_code"))
            lv1.SubItems.Add(dt.Rows(i)("depot_name"))

        Next


        Me.ListView2.Items.Clear()

        With Me.ListView2.Items.Add("f.depot_name", "仓库", 0)
            .Tag = "f.Depot_Name as 仓库"
        End With

        With Me.ListView2.Items.Add("g.BillKindName", "类型", 0)
            .Tag = "g.BillKindName as 类型"
        End With

        With Me.ListView2.Items.Add("a.billno", "单据号", 0)
            .Tag = "a.billno as 单据号"
        End With

        With Me.ListView2.Items.Add("b.accountterm", "帐期", 0)
            .Tag = "b.accountterm as 帐期"
        End With

        With Me.ListView2.Items.Add("b.billdate", "日期", 0)
            .Tag = "b.billdate as 日期"
        End With



        With Me.ListView2.Items.Add("c.item_code", "产品编码", 0)
            .Tag = "c.item_code as 产品编码"
        End With

        With Me.ListView2.Items.Add("c.item_name", "品名", 0)
            .Tag = "c.item_name as 品名"
        End With

        With Me.ListView2.Items.Add("c.item_standard", "规格", 0)
            .Tag = "c.item_standard as 规格"
        End With

        With Me.ListView2.Items.Add("c.unit", "单位", 0)
            .Tag = "c.unit as 单位"
        End With

    End Sub

    Private Sub CheckBox1_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CheckBox1.CheckedChanged
        Refresh_LV2()
    End Sub

    Private Sub Refresh_LV2()

        Dim sqlcon As New SqlConnection
        Dim sqlcmd As New SqlCommand
        Dim sqlad As New SqlDataAdapter

        Dim dt As New DataTable
        Dim sql As String
        Dim i As Int32
        Dim s As String = ""

        Dim lv1 As New ListViewItem



        Me.ListView3.Items.Clear()

        If Me.CheckBox1.Checked Then
            s = s & " or billtype=1 "
        End If

        If Me.CheckBox2.Checked Then
            s = s & " or billtype=2 "
        End If

        If s = "" Then
            Exit Sub
        Else
            s = s.Substring(4)
        End If

        sqlcon = getconn()

        '仓库

        sql = "SELECT * FROM T_StockBillKind where stop=1 and (" & s & ") ORDER BY BillType, BillKindCode"
        sqlcmd.Connection = sqlcon
        sqlcmd.CommandText = sql
        sqlad.SelectCommand = sqlcmd
        dt.Rows.Clear()
        sqlad.Fill(dt)

        For i = 0 To dt.Rows.Count - 1

            lv1 = Me.ListView3.Items.Add(dt.Rows(i)("billkindcode"))
            lv1.SubItems.Add(dt.Rows(i)("billkindname"))

        Next
    End Sub

    Private Sub CheckBox2_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CheckBox2.CheckedChanged
        Refresh_LV2()
    End Sub

    Private Sub 全选ToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles 全选ToolStripMenuItem.Click
        Dim i As Int32

        For i = 0 To CType(Me.ActiveControl, ListView).Items.Count - 1
            CType(Me.ActiveControl, ListView).Items(i).Checked = True
        Next
    End Sub

    Private Sub 反选ToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles 反选ToolStripMenuItem.Click
        Dim i As Int32

        For i = 0 To CType(Me.ActiveControl, ListView).Items.Count - 1
            CType(Me.ActiveControl, ListView).Items(i).Checked = Not CType(Me.ActiveControl, ListView).Items(i).Checked
        Next
    End Sub

    Private Sub ContextMenuStrip1_Opening(ByVal sender As System.Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles ContextMenuStrip1.Opening

    End Sub
End Class