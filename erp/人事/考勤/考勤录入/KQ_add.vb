Imports System.Data.SqlClient
Public Class KQ_add
    Friend state As String

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
    End Sub

    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button2.Click
        Dim sqlcon As New SqlConnection
        Dim sqlcmd As New SqlCommand
        Dim sqlad As New SqlDataAdapter
        Dim mytrans As SqlTransaction
        Dim dt As New DataTable

        Dim sql As String
        Dim billno As String

        If Me.TextBox2.Text.Trim.Length = 0 Then
            MsgBox("请选择员工姓名!", MsgBoxStyle.OkOnly, "提示")
            Exit Sub
        End If

        If Me.TextBox1.Text.Trim.Length = 0 Then
            MsgBox("请录入时间!", MsgBoxStyle.OkOnly, "提示")
            Exit Sub
        End If

        If Not IsNumeric(Me.TextBox1.Text) Then
            MsgBox("录入的时间必须是数字!", MsgBoxStyle.OkOnly, "提示")
            Exit Sub
        End If

        If Me.ComboBox1.SelectedIndex < 0 Then
            MsgBox("请选择事由!", MsgBoxStyle.OkOnly, "提示")
            Exit Sub
        End If

        If Me.TextBox3.Text.Trim.Length = 0 Then
            MsgBox("必须填写说明!", MsgBoxStyle.OkOnly, "提示")
            Exit Sub
        End If


        sqlcon = getconn()
        sqlcmd.Connection = sqlcon
        mytrans = sqlcon.BeginTransaction

        sqlcmd.Transaction = mytrans

        Try
            If state = "new" Then


                Sql = "update t_dj_num set num=num+1 where  tt='KQ'"
                sqlcmd.CommandText = Sql
                sqlcmd.ExecuteNonQuery()

                sql = "select * from t_dj_num where tt='KQ'"
                sqlcmd.CommandText = Sql
                sqlad.SelectCommand = sqlcmd
                dt.Rows.Clear()
                sqlad.Fill(dt)

                billno = dt.Rows(0)("TT") & "-" & GetAccountterm() & "-" & Format(dt.Rows(0)("num"), dt.Rows(0)("format"))



                sql = "INSERT INTO t_kq ( billno,input_date,input_man,memo,fs_date,sj,rs_no,sy_no) values ( " & _
                "'" & billno & "', getdate(), '" & rs_name & "', '" & Me.TextBox3.Text.Trim & "', '" & Format(Me.DateTimePicker1.Value, "yyyy-MM-dd") & "', '" & Me.TextBox1.Text.Trim & "', '" & Me.TextBox2.Tag & "','" & Me.ComboBox2.Text.Trim & "') "

                sqlcmd.CommandText = Sql
                sqlcmd.ExecuteNonQuery()

             



            ElseIf state = "mod" Then



                '如果已经不是新建就不能修改
                sql = "select count(*) from t_kq where state=0 and billno='" & CType(Me.Owner, KQ).ExDataGridView1.Rows(CType(Me.Owner, KQ).ExDataGridView1.SelectedCells(0).RowIndex).Cells("billno").Value & "'"
                sqlcmd.CommandText = Sql
                If sqlcmd.ExecuteScalar = 0 Then
                    Throw New System.Exception("该单据不是新建状态，不可以修改！")
                End If

                sql = "UPDATE t_kq " & _
                  "   SET  [rs_no] = '" & Me.TextBox2.Tag.ToString.Trim & "' " & _
                "      ,[sy_no] = '" & Me.ComboBox2.Text.Trim & "' " & _
                "      ,[memo] =  '" & Me.TextBox3.Text.Trim & "' " & _
                "      ,[fs_date]='" & Format(Me.DateTimePicker1.Value, "yyyy-MM-dd") & "'" & _
                "      ,[sj]='" & Me.TextBox1.Text.Trim & "'" & _
            " WHERE billno='" & CType(Me.Owner, KQ).ExDataGridView1.Rows(CType(Me.Owner, KQ).ExDataGridView1.SelectedCells(0).RowIndex).Cells("billno").Value & "'"

                sqlcmd.CommandText = Sql
                sqlcmd.ExecuteNonQuery()



            End If

            mytrans.Commit()
        Catch ex As Exception
            mytrans.Rollback()
            MsgBox(ex.Message, MsgBoxStyle.OkOnly, "错误")
            Exit Sub
        End Try

        MsgBox("保存成功！", MsgBoxStyle.OkOnly, "提示")

        If state = "new" Then

        ElseIf state = "mod" Then

            CType(Me.Owner, KQ).ExDataGridView1.Rows(CType(Me.Owner, KQ).ExDataGridView1.SelectedCells(0).RowIndex).Cells("rs_no").Value = Me.TextBox2.Tag
            CType(Me.Owner, KQ).ExDataGridView1.Rows(CType(Me.Owner, KQ).ExDataGridView1.SelectedCells(0).RowIndex).Cells("rs_name").Value = Me.TextBox2.Text
            CType(Me.Owner, KQ).ExDataGridView1.Rows(CType(Me.Owner, KQ).ExDataGridView1.SelectedCells(0).RowIndex).Cells("FS_date").Value = Format(Me.DateTimePicker1.Value, "yyyy-MM-dd")
            CType(Me.Owner, KQ).ExDataGridView1.Rows(CType(Me.Owner, KQ).ExDataGridView1.SelectedCells(0).RowIndex).Cells("sj").Value = Me.TextBox1.Text
            CType(Me.Owner, KQ).ExDataGridView1.Rows(CType(Me.Owner, KQ).ExDataGridView1.SelectedCells(0).RowIndex).Cells("sy_name").Value = Me.ComboBox1.Text
            CType(Me.Owner, KQ).ExDataGridView1.Rows(CType(Me.Owner, KQ).ExDataGridView1.SelectedCells(0).RowIndex).Cells("memo").Value = Me.TextBox3.Text
            CType(Me.Owner, KQ).ExDataGridView1.Rows(CType(Me.Owner, KQ).ExDataGridView1.SelectedCells(0).RowIndex).Cells("sy_no").Value = Me.ComboBox2.Text
            

        End If
        Me.DialogResult = Windows.Forms.DialogResult.OK
    End Sub

    Private Sub KQ_add_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
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

        For i = 0 To dt.Rows.Count - 1
            Me.ComboBox1.Items.Add(dt.Rows(i)("sy_name"))
            Me.ComboBox2.Items.Add(dt.Rows(i)("sy_no"))
        Next

        If state = "new" Then


            Me.Text = "考勤录入"


            Me.TextBox2.Text = ""
            Me.TextBox2.Tag = ""

            Me.TextBox1.Text = 0
            Me.ComboBox1.SelectedIndex = -1
            Me.TextBox3.Text = ""



        ElseIf state = "mod" Then

            Me.Text = "考勤修改"

            Me.TextBox2.Text = CType(Me.Owner, KQ).ExDataGridView1.Rows(CType(Me.Owner, KQ).ExDataGridView1.SelectedCells(0).RowIndex).Cells("rs_name").Value
            Me.TextBox2.Tag = CType(Me.Owner, KQ).ExDataGridView1.Rows(CType(Me.Owner, KQ).ExDataGridView1.SelectedCells(0).RowIndex).Cells("rs_no").Value

            Me.DateTimePicker1.Value = CType(Me.Owner, KQ).ExDataGridView1.Rows(CType(Me.Owner, KQ).ExDataGridView1.SelectedCells(0).RowIndex).Cells("fs_date").Value

            Me.TextBox1.Text = CType(Me.Owner, KQ).ExDataGridView1.Rows(CType(Me.Owner, KQ).ExDataGridView1.SelectedCells(0).RowIndex).Cells("sj").Value
            Me.ComboBox2.Text = CType(Me.Owner, KQ).ExDataGridView1.Rows(CType(Me.Owner, KQ).ExDataGridView1.SelectedCells(0).RowIndex).Cells("sy_no").Value

            Me.TextBox3.Text = CType(Me.Owner, KQ).ExDataGridView1.Rows(CType(Me.Owner, KQ).ExDataGridView1.SelectedCells(0).RowIndex).Cells("memo").Value



        End If


    End Sub

    Private Sub ComboBox1_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ComboBox1.SelectedIndexChanged
        Me.ComboBox2.SelectedIndex = Me.ComboBox1.SelectedIndex
    End Sub

    Private Sub ComboBox2_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ComboBox2.SelectedIndexChanged
        Me.ComboBox1.SelectedIndex = Me.ComboBox2.SelectedIndex
    End Sub

    Private Sub Button3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button3.Click
        Me.DialogResult = Windows.Forms.DialogResult.Cancel
    End Sub
End Class