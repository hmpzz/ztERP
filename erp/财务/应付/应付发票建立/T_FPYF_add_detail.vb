Imports System.Data.SqlClient
Public Class T_FPYF_add_detail
    Friend billno As String

    Private Sub ToolStripButton_refresh_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripButton_refresh.Click
        Refresh_ExDataGridView_master(" and a.billno like '%" & Me.TextBox1.Text.Trim & "%' ")
    End Sub

    Private Sub ToolStripButton_exit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripButton_exit.Click
        Me.DialogResult = Windows.Forms.DialogResult.Cancel
    End Sub





    Private Sub Refresh_ExDataGridView_master(Optional ByVal s As String = "")
        Dim sqlcon As New SqlConnection
        Dim sqlcmd As New SqlCommand
        Dim sqlad As New SqlDataAdapter
        Dim s1 As String = ""
        Dim s2 As String = ""

        Dim dt As New DataTable
        Dim sql As String



        sqlcon = getconn()
        sqlcmd.Connection = sqlcon

        If billno.Trim.Length <> 0 Then
            s1 = " and billno not in (" & billno & ")"
        End If

        sql = "select *,money-kp_money as wkp_money from  " & _
            "(select * from t_stockbill_master " & _
            "                where type=2 and billkindcode=20 and tempflag=0 and money>kp_money " & _
            "union all " & _
            "select * from t_stockbill_master_history  " & _
            "                where type=2 and billkindcode=20 and tempflag=0 and money>kp_money) a " & _
            " where 1=1 " & s1 & s

        sqlcmd.CommandText = sql
        sqlad.SelectCommand = sqlcmd
        dt.Rows.Clear()
        sqlad.Fill(dt)

        Me.ExDataGridView1.SetDataSource(dt)
        Me.ExDataGridView1.Columns.Clear()

        Me.ExDataGridView1.AddColumn("check", "", 20, False, New DataGridViewCheckBoxColumn)

        Me.ExDataGridView1.AddColumn("billno", "入库单号", 160)
        Me.ExDataGridView1.AddColumn("money", "金额", , , , , "#0.00")
        Me.ExDataGridView1.AddColumn("kp_money", "已开票金额", , , , , "#0.00")

        Me.ExDataGridView1.AddColumn("wkp_money", "未开票金额", , , , , "#0.00")


        Me.ExDataGridView1.AddColumn("id", "id", , , , , , False)


    End Sub

    Private Sub ToolStripButton_save_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripButton_save.Click
        Dim i As Int32
        Dim s As String

        s = ""

        Me.TextBox1.Focus()
        For i = 0 To Me.ExDataGridView1.Rows.Count - 1
            If Me.ExDataGridView1.Rows(i).Cells("check").Value Then
                s = s & "'" & Me.ExDataGridView1.Rows(i).Cells("billno").Value & "',"
            End If
        Next

        If s.Length > 0 Then
            s = s.Substring(0, Len(s) - 1)
        End If

        Me.Owner.Tag = s
        Me.DialogResult = Windows.Forms.DialogResult.OK
        Me.Close()
    End Sub
End Class