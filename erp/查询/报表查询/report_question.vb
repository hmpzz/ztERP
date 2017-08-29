Imports System.Data.SqlClient
Public Class report_question

    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button2.Click
        Dim sqlcon As New SqlConnection
        Dim sqlcmd As New SqlCommand
        Dim sqlad As New SqlDataAdapter

        Dim dr As SqlDataReader

        sqlcon = getconn()
        sqlcmd.Connection = sqlcon

        Dim picbytes() As Byte = {}

        If Me.ComboBox1.SelectedIndex < 0 Then
            MsgBox("请选择打印的次数！", MsgBoxStyle.OkOnly, "提示")
            Exit Sub
        End If

        sqlcmd.CommandText = "select * from t_reportPDF where billno='" & Me.TextBox1.Text.Trim & "' and b_no=" & Me.ComboBox1.Text.Trim
        dr = sqlcmd.ExecuteReader()

        If dr.Read Then
            picbytes = dr.Item("pdf")
        End If

        Me.TextBox2.Text = dr.Item("print_man")
        Me.TextBox3.Text = dr.Item("print_date")


        Dim ms = New System.IO.MemoryStream
        ms.Write(picbytes, 0, picbytes.Length)


        Dim data() As Byte

        Dim fs As IO.FileStream

        data = dr.Item("pdf")

        Try


            If IO.File.Exists(Application.StartupPath + "\pdf.pdf") Then
                IO.File.Delete(Application.StartupPath + "\pdf.pdf")
            End If
            fs = IO.File.Create(Application.StartupPath + "\pdf.pdf")
            fs.Write(data, 0, data.GetUpperBound(0))
            fs.Close()

            System.Diagnostics.Process.Start(Application.StartupPath + "\pdf.pdf")
        Catch ex As Exception
            MsgBox("打开出错！", MsgBoxStyle.OkOnly, "提示")
            Exit Sub
        End Try
    End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        Dim sqlcon As New SqlConnection
        Dim sqlcmd As New SqlCommand
        Dim sqlad As New SqlDataAdapter

        Dim dt As New DataTable
        Dim sql As String
        Dim i As Int32

        sqlcon = getconn()
        sqlcmd.Connection = sqlcon

        sql = "select b_no from t_reportPDF where billno='" & Me.TextBox1.Text.Trim & "' order by B_no"
        sqlcmd.CommandText = sql
        sqlad.SelectCommand = sqlcmd
        sqlad.Fill(dt)

        Me.ComboBox1.Items.Clear()

        For i = 0 To dt.Rows.Count - 1
            Me.ComboBox1.Items.Add(dt.Rows(i)("b_no"))
        Next
        Me.TextBox2.Text = ""
        Me.TextBox3.Text = ""
    End Sub

    Private Sub Button3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button3.Click
        Dim sqlcon As New SqlConnection
        Dim sqlcmd As New SqlCommand



        Dim sql As String
        Dim s As String

        sqlcon = getconn()
        sqlcmd.Connection = sqlcon


        If Me.RadioButton1.Checked Then

            sql = "select count(*) " & _
                "from t_stockbill_master  " & _
                "where billno='" & Me.TextBox4.Text.Trim & "'"
            sqlcmd.CommandText = sql
            If sqlcmd.ExecuteScalar > 0 Then

                s = Print_Out(Me.TextBox4.Text.Trim)

                If s = "" Then
                    MsgBox("保存成功！", MsgBoxStyle.OkOnly, "提示")
                Else
                    MsgBox(s, MsgBoxStyle.OkOnly, "提示")
                End If
            Else
                sql = "select count(*) " & _
                       "from t_stockbill_master_history  " & _
                       "where billno='" & Me.TextBox4.Text.Trim & "'"
                sqlcmd.CommandText = sql
                If sqlcmd.ExecuteScalar > 0 Then

                    s = Print_out_history(Me.TextBox4.Text.Trim)

                    If s = "" Then
                        MsgBox("保存成功！", MsgBoxStyle.OkOnly, "提示")
                    Else
                        MsgBox(s, MsgBoxStyle.OkOnly, "提示")
                    End If
                Else
                    MsgBox("没有找到要打印的单据!", MsgBoxStyle.OkOnly, "提示")
                End If
            End If



        ElseIf Me.RadioButton2.Checked Then
            sql = "select count(*) " & _
               "from t_stockbill_master  " & _
               "where billno='" & Me.TextBox4.Text.Trim & "'"
            sqlcmd.CommandText = sql
            If sqlcmd.ExecuteScalar > 0 Then

                s = Print_in(Me.TextBox4.Text.Trim)

                If s = "" Then
                    MsgBox("保存成功！", MsgBoxStyle.OkOnly, "提示")
                Else
                    MsgBox(s, MsgBoxStyle.OkOnly, "提示")
                End If
            Else
                sql = "select count(*) " & _
                       "from t_stockbill_master_history  " & _
                       "where billno='" & Me.TextBox4.Text.Trim & "'"
                sqlcmd.CommandText = sql
                If sqlcmd.ExecuteScalar > 0 Then

                    s = Print_in_history(Me.TextBox4.Text.Trim)

                    If s = "" Then
                        MsgBox("保存成功！", MsgBoxStyle.OkOnly, "提示")
                    Else
                        MsgBox(s, MsgBoxStyle.OkOnly, "提示")
                    End If
                Else
                    MsgBox("没有找到要打印的单据!", MsgBoxStyle.OkOnly, "提示")
                End If


            End If






        End If
    End Sub
End Class