Imports System.Data.SqlClient
Public Class purchase_cha

    Private Sub purchase_cha_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Dim sqlcon As New SqlConnection
        Dim sqlcmd As New SqlCommand
        Dim sqlad As New SqlDataAdapter

        Dim dt As New DataTable
        Dim sql As String
        Dim i As Int32

        sqlcon = getconn()
        sqlcmd.Connection = sqlcon



        Me.ComboBox1.Items.Clear()
        Me.ComboBox2.Items.Clear()
        sql = "select * from t_supplier where stop=1 order by supplier_name"

        sqlcmd.CommandText = sql
        sqlad.SelectCommand = sqlcmd
        dt.Rows.Clear()
        sqlad.Fill(dt)

        For i = 0 To dt.Rows.Count - 1
            Me.ComboBox1.Items.Add(dt.Rows(i)("supplier_name"))
            Me.ComboBox2.Items.Add(dt.Rows(i)("supplier_no"))
        Next

        'sql = "select a.* from t_depot a inner join t_user_depot b on a.depot_code=b.depot_code where a.stop=1 and b.login_id='" & login_id & "' order by a.depot_code"
        sql = "select * from t_depot where stop=1 "
        sqlcmd.CommandText = sql
        sqlad.SelectCommand = sqlcmd

        dt.Clear()
        sqlad.Fill(dt)

        For i = 0 To dt.Rows.Count - 1
            Me.ComboBox3.Items.Add(dt.Rows(i)("depot_name"))
            Me.ComboBox4.Items.Add(dt.Rows(i)("depot_code"))
        Next

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
            Me.CheckBox2.Checked = True
        End If
    End Sub

  

    Private Sub CheckBox1_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CheckBox1.CheckedChanged
        If Me.CheckBox1.Checked = False Then
            Me.ComboBox1.SelectedIndex = -1
        End If
    End Sub

  

    Private Sub CheckBox2_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CheckBox2.CheckedChanged
        If Me.CheckBox2.Checked = False Then
            Me.ComboBox3.SelectedIndex = -1
        End If
    End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click

        Dim s As String = ""


        If Me.CheckBox1.Checked Then
            s = s & " and a.supplier_no ='" & Me.ComboBox2.Text.Trim & "'"
        End If




        If Me.CheckBox2.Checked Then
            s = s & " and a.depot_code ='" & Me.ComboBox4.Text.Trim & "'"
        End If




        Me.Owner.Tag = s


        Me.DialogResult = Windows.Forms.DialogResult.OK
        Me.Close()
    End Sub

    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button2.Click
        Me.DialogResult = Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub
End Class