Imports System.Data.SqlClient
Public Class TZD

    Private Sub TZD_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Dim sqlcon As New SqlConnection
        Dim sqlcmd As New SqlCommand
        Dim sqlad As New SqlDataAdapter


        Dim dt As New DataTable
        Dim sql As String
        Dim i As Integer

        Me.ComboBox1.Items.Clear()

        sqlcon = getconn()
        sqlcmd.Connection = sqlcon

        Sql = "select a.* from t_depot a inner join t_user_depot b on a.depot_code=b.depot_code where a.stop=1 and b.login_id='" & login_id & "' order by a.depot_code"
        sqlcmd.CommandText = Sql
        sqlad.SelectCommand = sqlcmd

        dt.Clear()
        sqlad.Fill(dt)

        For i = 0 To dt.Rows.Count - 1
            Me.ComboBox1.Items.Add(dt.Rows(i)("depot_name"))
            Me.ComboBox2.Items.Add(dt.Rows(i)("depot_code"))
        Next

    End Sub

    Private Sub ToolStripButton_exit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripButton_exit.Click
        Me.Close()
    End Sub

    Private Sub ToolStripButton_refresh_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripButton_refresh.Click
        Refresh_ExDataGridView_master()
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


        If Me.CheckBox1.Checked Then
            s1 = " and a.amount<>0 "
        End If

        sql = "SELECT a.*, b.item_name ,b.item_standard,b.unit ,e.depot_name,b.max_amount,b.min_amount " & _
            "FROM T_part a LEFT  JOIN " & _
            "      t_item b ON a.item_code = b.item_code left join  " & _
            "	t_depot e on a.depot_code=e.depot_code " & _
            " where  a.depot_code='" & Me.ComboBox2.Text & "'  " & s1 & s


        sqlcmd.CommandText = sql
        sqlad.SelectCommand = sqlcmd
        dt.Rows.Clear()
        sqlad.Fill(dt)

        Me.ExDataGridView1.SetDataSource(dt)


        Me.ExDataGridView1.AddColumn("depot_name", "仓库")
        Me.ExDataGridView1.AddColumn("item_code", "物料编码")
        Me.ExDataGridView1.AddColumn("item_name", "物料名称")
        Me.ExDataGridView1.AddColumn("item_standard", "规格")
        Me.ExDataGridView1.AddColumn("unit", "单位")

        Me.ExDataGridView1.AddColumn("amount", "当前库存", , , , DataGridViewContentAlignment.MiddleRight, "#0.000")
        Me.ExDataGridView1.AddColumn("qc_amount", "期初库存", , , , DataGridViewContentAlignment.MiddleRight, "#0.000")

        Me.ExDataGridView1.AddColumn("max_amount", "库存上限", , , , DataGridViewContentAlignment.MiddleRight, "#0.000")

        Me.ExDataGridView1.AddColumn("min_amount", "库存下限", , , , DataGridViewContentAlignment.MiddleRight, "#0.000")


        Me.ExDataGridView1.AddColumn("price", "单价", , , , DataGridViewContentAlignment.MiddleRight, "#0.0000")

        Me.ExDataGridView1.AddColumn("money", "金额", , , , DataGridViewContentAlignment.MiddleRight, "#0.0000")






    End Sub

    Private Sub ToolStripButton_excel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripButton_excel.Click
        Dim ToExcel As New BaseClass.TOEXCEL

        If ToExcel.ExToExcel(Me.ExDataGridView1, Me.SaveFileDialog1, True, "当前库存", "期初库存") Then
            MsgBox("导出成功！", MsgBoxStyle.OkOnly, "提示")
        End If
    End Sub

    Private Sub 调整数据ToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles 调整数据ToolStripMenuItem.Click
        Dim tzd_mod As New tzd_mod

        If Me.ComboBox1.SelectedIndex < 0 Then
            MsgBox("请选择仓库！", MsgBoxStyle.OkOnly, "提示")
            Exit Sub
        End If

        If Me.ExDataGridView1.SelectedCells.Count = 0 Then
            MsgBox("请选择要调整的行！", MsgBoxStyle.OkOnly, "提示")
            Exit Sub
        End If


        tzd_mod.Owner = Me
        tzd_mod.TextBox1.Text = Me.ExDataGridView1.Rows(Me.ExDataGridView1.SelectedCells(0).RowIndex).Cells("item_code").Value
        tzd_mod.TextBox2.Text = Me.ExDataGridView1.Rows(Me.ExDataGridView1.SelectedCells(0).RowIndex).Cells("item_name").Value
        tzd_mod.TextBox3.Text = Me.ExDataGridView1.Rows(Me.ExDataGridView1.SelectedCells(0).RowIndex).Cells("amount").Value
        tzd_mod.TextBox4.Text = Me.ExDataGridView1.Rows(Me.ExDataGridView1.SelectedCells(0).RowIndex).Cells("price").Value
        tzd_mod.TextBox5.Text = Me.ExDataGridView1.Rows(Me.ExDataGridView1.SelectedCells(0).RowIndex).Cells("money").Value
        tzd_mod.depot_code = Me.ComboBox2.Text.Trim
        tzd_mod.ShowDialog()

    End Sub

    Private Sub ComboBox1_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ComboBox1.SelectedIndexChanged
        Me.ExDataGridView1.ClearDSDM()
        Me.ExDataGridView1.Rows.Clear()
        Me.ComboBox2.SelectedIndex = Me.ComboBox1.SelectedIndex
    End Sub


End Class