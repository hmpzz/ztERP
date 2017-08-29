Imports System.Data.SqlClient
Public Class t_part_out_add_detail
    Public depot_code As String
    Public id As String
    Private Sub ToolStripButton_refresh_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripButton_refresh.Click
        Refresh_ExDataGridView_master(Me.TextBox2.Text.Trim)
    End Sub


    Private Sub Refresh_ExDataGridView_master(Optional ByVal s As String = "")
        Dim sqlcon As New SqlConnection
        Dim sqlcmd As New SqlCommand
        Dim sqlad As New SqlDataAdapter


        Dim dt As New DataTable
        Dim sql As String
        Dim s1 As String = ""




        sqlcon = getconn()
        sqlcmd.Connection = sqlcon

        If id.Length <> 0 Then
            s1 = " and a.item_code not in (" & id & ")"
        End If

        sql = "SELECT     a.* ,b.item_name,b.item_standard,b.unit ,c.depot_name " & _
                "FROM         T_part a inner join  " & _
                "                  t_item b on a.item_code=b.item_code left join  " & _
                "t_depot c on a.depot_code=c.depot_code " & _
                "where a.depot_code='" & depot_code & "' and (b.item_name like '%" & s & "%' or  b.item_standard  like '%" & s & "%' or b.item_code like '%" & s & "%') and b.stop=1 " & s1



        sqlcmd.CommandText = sql
        sqlad.SelectCommand = sqlcmd
        dt.Rows.Clear()
        sqlad.Fill(dt)

        Me.ExDataGridView1.SetDataSource(dt)



        Me.ExDataGridView1.AddColumn("check", "", 20, False, New DataGridViewCheckBoxColumn)
        Me.ExDataGridView1.AddColumn("item_code", "物料编码")
        Me.ExDataGridView1.AddColumn("item_name", "物料名称")
        Me.ExDataGridView1.AddColumn("item_standard", "规格")
        Me.ExDataGridView1.AddColumn("unit", "单位")
        Me.ExDataGridView1.AddColumn("depot_name", "仓库")

        Me.ExDataGridView1.AddColumn("amount", "库存数量", , , , DataGridViewContentAlignment.MiddleRight, "#0.000")
        

        Me.ExDataGridView1.AddColumn("item_code", "item_code", , , , , , False)
        Me.ExDataGridView1.AddColumn("depot_code", "depot_code", , , , , , False)
        
    End Sub

    Private Sub ToolStripButton_save_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripButton_save.Click



        Dim i As Int32

        If Me.ExDataGridView1.Rows.Count > 0 Then
            Me.ExDataGridView1.Rows(0).Cells(1).Selected = True
        End If

        Me.TextBox1.Focus()

        For i = 0 To Me.ExDataGridView1.Rows.Count - 1

            If Me.ExDataGridView1.Rows(i).Cells("check").Value Then

                Me.ExDataGridView1.AddColumn("item_code", "物料编码")
                Me.ExDataGridView1.AddColumn("item_name", "物料名称")
                Me.ExDataGridView1.AddColumn("item_standard", "规格")
                Me.ExDataGridView1.AddColumn("unit", "单位")

                Me.ExDataGridView1.AddColumn("amount", "数量", , , , DataGridViewContentAlignment.MiddleRight, "#0.000")


                Me.ExDataGridView1.AddColumn("item_code", "item_code", , , , , , False)


                Me.ExDataGridView1.AddColumn("id", "id", , , , , , False)



                Dim DR As DataRow = CType(Me.Owner, t_part_out_add).dt_P.NewRow

                DR("item_name") = Me.ExDataGridView1.Rows(i).Cells("item_name").Value
                DR("item_standard") = Me.ExDataGridView1.Rows(i).Cells("item_standard").Value
                DR("unit") = Me.ExDataGridView1.Rows(i).Cells("unit").Value
                DR("amount") = Me.ExDataGridView1.Rows(i).Cells("amount").Value
             
                DR("item_code") = Me.ExDataGridView1.Rows(i).Cells("item_code").Value

                DR("id") = 0

                CType(Me.Owner, t_part_out_add).dt_P.Rows.Add(DR)
            End If



        Next
        Me.DialogResult = Windows.Forms.DialogResult.OK
    End Sub

    Private Sub ToolStripButton_exit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripButton_exit.Click
        Me.DialogResult = Windows.Forms.DialogResult.Cancel
    End Sub
End Class