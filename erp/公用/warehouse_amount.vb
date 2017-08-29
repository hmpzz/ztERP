Imports System.Data.SqlClient
Public Class warehouse_amount

    Friend depot_code As String = ""
    '不想显示的物料ID
    Friend id As String = ""


    Private Sub ToolStripButton_refresh_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripButton_refresh.Click
        Refresh_ExDataGridView(Me.TextBox1.Text.Trim)
    End Sub


    Private Sub Refresh_ExDataGridView(Optional ByVal s As String = "")
        Dim sqlcon As New SqlConnection
        Dim sqlcmd As New SqlCommand
        Dim sqlad As New SqlDataAdapter


        Dim dt As New DataTable
        Dim sql As String
        Dim s1 As String = ""


        sqlcon = getconn()
        sqlcmd.Connection = sqlcon



        If id.Length <> 0 Then
            s1 = " and a.id not in (" & id & ")"
        End If

        If Me.CheckBox1.Checked Then
            s1 = s1 & " and a.amount<>0 "
        End If

        If Me.depot_code.Trim.Length = 0 Then
            sql = "select a.* " & _
               "from  t_item a where a.stop=1 and (a.item_name like '%" & s & "%' or  a.item_standard  like '%" & s & "%' or a.item_code like '%" & s & "%') " & s1
        Else
            sql = "select a.* " & _
               "from  t_item a right join " & _
               " t_part b on a.item_code=b.item_code " & _
               "where b.depot_code='" & depot_code & "' and a.stop=1 and (a.item_name like '%" & s & "%' or  a.item_standard  like '%" & s & "%' or a.item_code like '%" & s & "%') " & s1

        End If

       
        sqlcmd.CommandText = sql

        sqlad.SelectCommand = sqlcmd

        dt.Clear()
        sqlad.Fill(dt)

        Me.ExDataGridView1.SetDataSource(dt)
        Me.ExDataGridView1.Columns.Clear()

        Me.ExDataGridView1.AddColumn("check", "", 20, False, New System.Windows.Forms.DataGridViewCheckBoxColumn)
        Me.ExDataGridView1.AddColumn("item_code", "物料编码")
        Me.ExDataGridView1.AddColumn("item_name", "物料名称")
        Me.ExDataGridView1.AddColumn("item_standard", "规格")
        Me.ExDataGridView1.AddColumn("unit", "单位")


        Me.ExDataGridView1.AddColumn("item_id", "item_id", , , , , , False)
        Me.ExDataGridView1.AddColumn("packing_id", "packing_id", , , , , , False)
        Me.ExDataGridView1.AddColumn("standards_id", "standards_id", , , , , , False)
        Me.ExDataGridView1.AddColumn("id", "id", , , , , , False)
    End Sub

    Private Sub ToolStripButton_save_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripButton_save.Click
        Dim i As Int32
        Dim s As String = ""

        '如果没有这句选择就会少一行
        Me.TextBox1.Focus()

        For i = 0 To Me.ExDataGridView1.Rows.Count - 1
            If Me.ExDataGridView1.Rows(i).Cells("check").Value Then
                s = s & Me.ExDataGridView1.Rows(i).Cells("id").Value & ","
            End If
        Next
        If s.Trim.Length <> 0 Then
            s = s.Substring(0, s.Length - 1)
        End If

        Me.Owner.Tag = s

        'Me.ExDataGridView1.DataSource = Nothing
        'Me.ExDataGridView1.DataMember = Nothing
        Me.ExDataGridView1.ClearDSDM()

        Me.DialogResult = Windows.Forms.DialogResult.OK
        Me.Close()
    End Sub

    Private Sub ToolStripButton_exit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripButton_exit.Click
        Me.DialogResult = Windows.Forms.DialogResult.Cancel
    End Sub
End Class