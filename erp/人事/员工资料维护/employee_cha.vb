Imports System.Data.SqlClient
Public Class employee_cha

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        Dim s As String = ""
        Dim s1 As String = ""

        If Me.CheckBox1.Checked Then
            s = s & " and a.rs_name like '%" & Me.TextBox6.Text.Trim & "%'"
        End If

        If Me.CheckBox2.Checked Then
            s = s & " and a.rs_no like '%" & Me.TextBox3.Text.Trim & "%'"
        End If




        If Me.CheckBox4.Checked Then
            s = s & " and a.Xb = '" & Me.ComboBox3.Text.Trim & "'"
        End If

        If Me.CheckBox7.Checked Then
            s = s & " and a.Hf = " & Me.ComboBox5.SelectedIndex & ""
        End If

        If Me.CheckBox5.Checked Then
            s = s & " and a.Card like '%" & Me.TextBox7.Text.Trim & "%'"
        End If

    

        If Me.CheckBox9.Checked Then
            s = s & " and a.Tel like '%" & Me.TextBox8.Text.Trim & "%'"
        End If

     

      
        If Me.CheckBox26.Checked Then
            s = s & " and a.memo like '%" & Me.TextBox5.Text.Trim & "%'"
        End If


        Me.Owner.Tag = s
        Me.DialogResult = Windows.Forms.DialogResult.OK
        Me.Close()


    End Sub

    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button2.Click
        Me.DialogResult = Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub

  


    Private Sub TextBox3_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)
        If Me.TextBox3.Text.Trim.Length <> 0 Then
            Me.CheckBox4.Checked = True

        End If
    End Sub

    Private Sub CheckBox1_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CheckBox1.CheckedChanged
        If Not Me.CheckBox1.Checked Then
            Me.TextBox6.Text = ""
        End If
    End Sub

    Private Sub CheckBox2_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CheckBox2.CheckedChanged
        If Not Me.CheckBox2.Checked Then
            Me.TextBox3.Text = ""
        End If
    End Sub

    Private Sub CheckBox4_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CheckBox4.CheckedChanged
        If Not Me.CheckBox4.Checked Then
            Me.ComboBox3.Text = ""
        End If
    End Sub

    Private Sub employee_cha_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Dim sqlcon As New SqlConnection
        Dim sqlcmd As New SqlCommand
        Dim sqlad As New SqlDataAdapter

        Dim sql As String = ""
        Dim dt As New DataTable

        Dim i As Int32

        sqlcon = getconn()
        sqlcmd.Connection = sqlcon

        '部门
        Me.ComboBox1.Items.Clear()
        Me.ComboBox16.Items.Clear()
        sql = "select * from t_bm "
        sqlcmd.CommandText = sql
        sqlad.SelectCommand = sqlcmd
        dt.Rows.Clear()
        sqlad.Fill(dt)

        For i = 0 To dt.Rows.Count - 1
            Me.ComboBox1.Items.Add(dt.Rows(i)("id"))
            Me.ComboBox16.Items.Add(dt.Rows(i)("bmmc"))
        Next

     

        '文化程度
        Me.ComboBox11.Items.Clear()
        Me.ComboBox4.Items.Clear()

        sql = "select * from t_xl"
        sqlcmd.CommandText = sql
        sqlad.SelectCommand = sqlcmd
        dt.Rows.Clear()
        sqlad.Fill(dt)

        For i = 0 To dt.Rows.Count - 1
            Me.ComboBox11.Items.Add(dt.Rows(i)("xl_name"))
            Me.ComboBox4.Items.Add(dt.Rows(i)("id"))
        Next




       
        Me.ComboBox3.Items.Add("男")
        Me.ComboBox3.Items.Add("女")


        '婚否
        Me.ComboBox5.Items.Clear()

        'sql = "select distinct hf,case hf when 1 then '已婚' when 0 then '未婚' end from t_rs order by hf"
        'sqlcmd.CommandText = sql
        'sqlad.SelectCommand = sqlcmd
        'dt.Rows.Clear()
        'sqlad.Fill(dt)

        'For i = 0 To dt.Rows.Count - 1
        '    Me.ComboBox5.Items.Add(dt.Rows(i)("hf"))
        'Next
        Me.ComboBox5.Items.Add("已婚")
        Me.ComboBox5.Items.Add("未婚")

    End Sub

    Private Sub TextBox6_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TextBox6.TextChanged
        If Me.TextBox6.Text.Trim.Length <> 0 Then
            Me.CheckBox1.Checked = True
        End If
    End Sub

    Private Sub TextBox3_TextChanged_1(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TextBox3.TextChanged
        If Me.TextBox3.Text.Trim.Length <> 0 Then
            Me.CheckBox2.Checked = True
        End If
    End Sub

    Private Sub ComboBox3_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ComboBox3.SelectedIndexChanged
        If Me.ComboBox3.SelectedIndex <> -1 Then
            Me.CheckBox4.Checked = True
        End If
    End Sub

    Private Sub ComboBox5_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ComboBox5.SelectedIndexChanged
        If Me.ComboBox5.SelectedIndex <> -1 Then
            Me.CheckBox7.Checked = True
        End If
    End Sub

    Private Sub TextBox7_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TextBox7.TextChanged
        If Me.TextBox7.Text.Trim.Length <> 0 Then
            Me.CheckBox5.Checked = True
        End If
    End Sub

  

    Private Sub TextBox8_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TextBox8.TextChanged
        If Me.TextBox8.Text.Trim.Length <> 0 Then
            Me.CheckBox9.Checked = True
        End If
    End Sub



    Private Sub TextBox5_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TextBox5.TextChanged
        If Me.TextBox5.Text.Trim.Length <> 0 Then
            Me.CheckBox26.Checked = True
        End If
    End Sub

    Private Sub CheckBox7_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CheckBox7.CheckedChanged
        If Not Me.CheckBox7.Checked Then
            Me.ComboBox5.Text = ""
        End If
    End Sub

    Private Sub CheckBox5_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)
        If Not Me.CheckBox5.Checked Then
            Me.TextBox7.Text = ""
        End If
    End Sub


    Private Sub CheckBox9_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CheckBox9.CheckedChanged
        If Not Me.CheckBox9.Checked Then
            Me.TextBox8.Text = ""
        End If
    End Sub



    Private Sub CheckBox26_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CheckBox26.CheckedChanged
        If Not Me.CheckBox26.Checked Then
            Me.TextBox5.Text = ""
        End If
    End Sub
End Class