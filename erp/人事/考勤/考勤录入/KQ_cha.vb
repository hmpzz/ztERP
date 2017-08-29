Imports System.Data.SqlClient
Public Class KQ_cha

    Private Sub KQ_cha_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Dim sqlcon As New SqlConnection
        Dim sqlcmd As New SqlCommand
        Dim sqlad As New SqlDataAdapter

        Dim dt As New DataTable

        Dim sql As String
        Dim i As Int32

        sqlcon = getconn()
        sqlcmd.Connection = sqlcon

        sql = "select * from t_SY where stop=1"
        sqlcmd.CommandText = sql
        sqlad.SelectCommand = sqlcmd

        sqlad.Fill(dt)

        Me.ComboBox3.Items.Clear()
        Me.ComboBox4.Items.Clear()

        For i = 0 To dt.Rows.Count - 1
            Me.ComboBox3.Items.Add(dt.Rows(i)("sy_name"))
            Me.ComboBox4.Items.Add(dt.Rows(i)("sy_no"))
        Next


        sql = "select * from t_bm where stop=1"
        sqlcmd.CommandText = sql
        sqlad.SelectCommand = sqlcmd
        dt.Clear()
        sqlad.Fill(dt)

        Me.ComboBox1.Items.Clear()
        Me.ComboBox2.Items.Clear()

        For i = 0 To dt.Rows.Count - 1
            Me.ComboBox1.Items.Add(dt.Rows(i)("bmmc"))
            Me.ComboBox2.Items.Add(dt.Rows(i)("id"))
        Next


    End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        Dim rs_choice As New rs_choice

        Dim sqlcon As New SqlConnection
        Dim sqlcmd As New SqlCommand

        Dim sql As String


        sqlcon = getconn()
        sqlcmd.Connection = sqlcon

        rs_choice.Owner = Me


        If rs_choice.ShowDialog = Windows.Forms.DialogResult.Cancel Then
            Exit Sub
        End If

        Me.TextBox2.Tag = Me.Tag
        Me.Tag = ""


        sqlcon = getconn()
        sql = "select rs_name from t_rs where rs_no='" & Me.TextBox2.Tag & "'"
        sqlcmd.CommandText = sql

        Me.TextBox2.Text = sqlcmd.ExecuteScalar
        Me.CheckBox2.Checked = True
    End Sub

    Private Sub ComboBox1_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ComboBox1.SelectedIndexChanged
        Me.ComboBox2.SelectedIndex = Me.ComboBox1.SelectedIndex
        If Me.ComboBox1.SelectedIndex >= 0 Then
            Me.CheckBox1.Checked = True
        End If
    End Sub

    Private Sub ComboBox3_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ComboBox3.SelectedIndexChanged
        Me.ComboBox4.SelectedIndex = Me.ComboBox3.SelectedIndex
        If Me.ComboBox3.SelectedIndex >= 0 Then
            Me.CheckBox4.Checked = True
        End If
    End Sub

    Private Sub CheckBox3_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CheckBox3.CheckedChanged

    End Sub

    Private Sub CheckBox1_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CheckBox1.CheckedChanged
        If Me.CheckBox1.Checked = False Then
            Me.ComboBox1.SelectedIndex = -1
        End If
    End Sub

    Private Sub CheckBox2_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CheckBox2.CheckedChanged
        If Me.CheckBox2.Checked = False Then
            Me.TextBox2.Text = ""
            Me.TextBox2.Tag = ""
        End If
    End Sub

    Private Sub CheckBox4_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CheckBox4.CheckedChanged
        If Me.CheckBox4.Checked = False Then
            Me.ComboBox3.SelectedIndex = -1
        End If
    End Sub

    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button2.Click
        'and b.bm_id='' and b.rs_no='' and a.sy_no=''
        Dim s As String = ""

        If Me.CheckBox1.Checked Then
            s = s & " and b.bm_id=" & Me.ComboBox2.Text
        End If

        If Me.CheckBox2.Checked Then
            s = s & " and b.rs_no='" & Me.TextBox2.Tag & "'"
        End If


        If Me.CheckBox3.Checked Then
            s = s & " and a.fs_date>='" & Format(Me.DateTimePicker1.Value, "yyyy-MM-dd") & " 00:00:00' and a.fs_date<='" & Format(Me.DateTimePicker2.Value, "yyyy-MM-dd") & "'"
        End If

        If Me.CheckBox4.Checked Then
            s = s & " and a.sy_no='" & Me.ComboBox4.Text & "'"
        End If

        Me.Owner.Tag = s

        Me.DialogResult = Windows.Forms.DialogResult.OK

    End Sub

    Private Sub Button3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button3.Click
        Me.DialogResult = Windows.Forms.DialogResult.Cancel
    End Sub
End Class