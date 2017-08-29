Imports System.Data.SqlClient
Public Class warehouse_question_cha

    Private Sub warehouse_question_cha_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Dim sqlcon As New SqlConnection
        Dim sqlcmd As New SqlCommand
        Dim sqlad As New SqlDataAdapter

        Dim dt As New DataTable
        Dim sql As String
        Dim i As Int32

        sqlcon = getconn()

        '²Ö¿â
        Me.ComboBox1.Items.Clear()
        Me.ComboBox2.Items.Clear()
        sql = "select * from t_depot where stop=1 order by depot_name"
        sqlcmd.Connection = sqlcon
        sqlcmd.CommandText = sql
        sqlad.SelectCommand = sqlcmd
        dt.Rows.Clear()
        sqlad.Fill(dt)

        For i = 0 To dt.Rows.Count - 1
            Me.ComboBox1.Items.Add(dt.Rows(i)("depot_name"))
            Me.ComboBox2.Items.Add(dt.Rows(i)("depot_code"))
        Next

    End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click

        Dim s As String = ""


        If Me.CheckBox1.Checked Then
            s = s & " and a.depot_code ='" & Me.ComboBox2.Text.Trim & "'"
        End If




        If Me.CheckBox2.Checked Then
            s = s & " and b.item_name like '%" & Me.TextBox1.Text.Trim & "%'"
        End If


        If Me.CheckBox3.Checked Then
            s = s & " and b.item_standard='" & Me.TextBox2.Text.Trim & "'"
        End If


        If Me.CheckBox4.Checked Then
            s = s & " and b.unit = '" & Me.TextBox3.Text.Trim & "'"
        End If


      

        CType(Me.Owner, warehouse_question).cha = s



        Me.DialogResult = Windows.Forms.DialogResult.OK
        Me.Close()
    End Sub

    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button2.Click
        Me.DialogResult = Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub

    Private Sub ComboBox1_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ComboBox1.SelectedIndexChanged
        Me.ComboBox2.SelectedIndex = Me.ComboBox1.SelectedIndex
        If Me.ComboBox1.SelectedIndex >= 0 Then
            Me.CheckBox1.Checked = True
        End If
    End Sub

    Private Sub TextBox1_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TextBox1.TextChanged
        If Me.TextBox1.Text.Trim.Length > 0 Then
            Me.CheckBox2.Checked = True
        End If
    End Sub

    Private Sub TextBox2_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TextBox2.TextChanged
        If Me.TextBox2.Text.Trim.Length > 0 Then
            Me.CheckBox3.Checked = True
        End If
    End Sub

    Private Sub TextBox3_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TextBox3.TextChanged
        If Me.TextBox3.Text.Trim.Length > 0 Then
            Me.CheckBox4.Checked = True
        End If
    End Sub

    Private Sub CheckBox4_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CheckBox4.CheckedChanged
        If Me.CheckBox4.Checked = False Then
            Me.TextBox3.Text = ""
        End If
    End Sub

    Private Sub CheckBox3_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CheckBox3.CheckedChanged
        If Me.CheckBox3.Checked = False Then
            Me.TextBox2.Text = ""
        End If
    End Sub

    Private Sub CheckBox2_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CheckBox2.CheckedChanged
        If Me.CheckBox2.Checked = False Then
            Me.TextBox1.Text = ""
        End If
    End Sub

    Private Sub CheckBox1_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CheckBox1.CheckedChanged
        If Me.CheckBox1.Checked = False Then
            Me.ComboBox1.SelectedIndex = -1
        End If
    End Sub
End Class