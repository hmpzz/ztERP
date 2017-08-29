Imports System.Data.SqlClient

Public Class project_add
    Friend state As String

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        Dim sqlcon As New SqlConnection
        Dim sqlcmd As New SqlCommand
        Dim sqlad As New SqlDataAdapter
        Dim mytrans As SqlTransaction

        Dim dt As New DataTable
        Dim billno As String = ""
        Dim sql As String = ""


        If Not IsNumeric(Me.TextBox3.Text.Trim) Then
            MsgBox("工程金额必须是数字，请检查后输入！", MsgBoxStyle.OkOnly, "提示")
            Exit Sub
        End If

        If Me.ComboBox1.SelectedIndex = -1 Then
            MsgBox("必须选择客户！", MsgBoxStyle.OkOnly, "提示")
            Exit Sub
        End If

        If Me.ComboBox3.SelectedIndex = -1 Then
            MsgBox("必须选择工厂！", MsgBoxStyle.OkOnly, "提示")
            Exit Sub
        End If

        sqlcon = getconn()

        mytrans = sqlcon.BeginTransaction()
        sqlcmd.Transaction = mytrans

        sqlcmd.Connection = sqlcon
        Try
            If state = "new" Then

                sql = "update t_dj_num set num=num+1 where  tt='" & Me.ComboBox4.Text.Trim & "'"
                sqlcmd.CommandText = sql
                sqlcmd.ExecuteNonQuery()

                sql = "select * from t_dj_num where tt='" & Me.ComboBox4.Text.Trim & "'"
                sqlcmd.CommandText = sql
                sqlad.SelectCommand = sqlcmd
                dt.Rows.Clear()
                sqlad.Fill(dt)

                billno = dt.Rows(0)("TT") & GetAccountterm().ToString.Remove(4, 2).Remove(0, 2) & "-" & Format(dt.Rows(0)("num"), dt.Rows(0)("format"))


                'billno = Me.TextBox1.Text.Trim

                sql = "INSERT INTO project_master ( effective_date,input_date,input_man,memo,project_name,project_NO,state,begin_date,customer_no,factory_code) values " & _
                "(   '" & Format(Me.DateTimePicker1.Value, "yyyy-MM-dd") & "',getdate(), '" & rs_name & "', '" & Me.TextBox4.Text.Trim & "', '" & Me.TextBox2.Text.Trim & "', '" & billno & "', '0','" & Format(Me.DateTimePicker2.Value, "yyyy-MM-dd") & "','" & Me.ComboBox2.Text & "','" & Me.ComboBox4.Text.Trim & "')"
                sqlcmd.CommandText = sql
                sqlcmd.ExecuteNonQuery()


            ElseIf state = "mod" Then

                '如果已经不是新建就不能修改
                sql = "select count(*) from project_master where state=0 and project_no='" & Me.TextBox1.Text.Trim & "'"
                sqlcmd.CommandText = sql
                If sqlcmd.ExecuteScalar = 0 Then
                    Throw New System.Exception("该单据不是新建状态，不可以修改！")
                End If


                sql = "update project_master set effective_date='" & Format(Me.DateTimePicker1.Value, "yyyy-MM-dd") & "',project_name='" & Me.TextBox2.Text.Trim & "',memo='" & Me.TextBox4.Text.Trim & "',begin_date='" & Format(Me.DateTimePicker2.Value, "yyyy-MM-dd") & "',customer_no='" & Me.ComboBox2.Text & "' where project_NO='" & Me.TextBox1.Text.Trim & "'"
                sqlcmd.CommandText = sql
                sqlcmd.ExecuteNonQuery()

            End If


            mytrans.Commit()
        Catch ex As Exception
            mytrans.Rollback()
            MsgBox(ex.Message, MsgBoxStyle.OkOnly, "提示")
            Exit Sub
        End Try



        Me.DialogResult = Windows.Forms.DialogResult.OK
    End Sub

    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button2.Click
        Me.DialogResult = Windows.Forms.DialogResult.Cancel

    End Sub

    Private Sub project_add_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Dim sqlcon As New SqlConnection
        Dim sqlcmd As New SqlCommand
        Dim sqlad As New SqlDataAdapter

        Dim dt As New DataTable
        Dim i As int32
        Dim sql As String

        sqlcon = getconn()
        sqlcmd.Connection = sqlcon



        Sql = "select * from t_customer where stop=1 "
        sqlcmd.CommandText = Sql
        sqlad.SelectCommand = sqlcmd
        dt.Clear()
        sqlad.Fill(dt)

        Me.ComboBox1.Items.Clear()
        Me.ComboBox2.Items.Clear()

        For i = 0 To dt.Rows.Count - 1
            Me.ComboBox1.Items.Add(dt.Rows(i)("customer_name"))
            Me.ComboBox2.Items.Add(dt.Rows(i)("customer_no"))
        Next

        Me.ComboBox3.Enabled = True

        Me.ComboBox3.Items.Clear()
        Me.ComboBox4.Items.Clear()

        sql = "select c.factory_name,c.factory_code  " & _
                "from sys_user a inner join  " & _
                "	t_user_factory b on a.login_id=b.login_id inner join  " & _
                "	t_factory c on b.factory_code=c.factory_code " & _
                "where a.stop_login=1 and c.stop=1 and a.login_id='" & login_id & "'"
        sqlcmd.CommandText = sql
        sqlad.SelectCommand = sqlcmd
        dt.Clear()
        sqlad.Fill(dt)

        For i = 0 To dt.Rows.Count - 1
            Me.ComboBox3.Items.Add(dt.Rows(i)("factory_name"))
            Me.ComboBox4.Items.Add(dt.Rows(i)("factory_code"))

        Next


        If state = "new" Then


            Me.Text = "增加"
            Me.DateTimePicker1.Value = Now.AddDays(360)
      
            Me.TextBox1.Enabled = False


         
            

        ElseIf state = "mod" Then

            Me.Text = "修改"

            Me.ComboBox3.Enabled = False

            Me.TextBox1.Text = CType(Me.Owner, project).ExDataGridView1.Rows(CType(Me.Owner, project).ExDataGridView1.SelectedCells(0).RowIndex).Cells("project_no").Value
            Me.TextBox2.Text = CType(Me.Owner, project).ExDataGridView1.Rows(CType(Me.Owner, project).ExDataGridView1.SelectedCells(0).RowIndex).Cells("project_name").Value
            Me.TextBox3.Text = CType(Me.Owner, project).ExDataGridView1.Rows(CType(Me.Owner, project).ExDataGridView1.SelectedCells(0).RowIndex).Cells("project_money").Value
            Me.TextBox4.Text = CType(Me.Owner, project).ExDataGridView1.Rows(CType(Me.Owner, project).ExDataGridView1.SelectedCells(0).RowIndex).Cells("memo").Value

            Me.DateTimePicker1.Value = CType(Me.Owner, project).ExDataGridView1.Rows(CType(Me.Owner, project).ExDataGridView1.SelectedCells(0).RowIndex).Cells("effective_date").Value
            Me.DateTimePicker1.Value = CType(Me.Owner, project).ExDataGridView1.Rows(CType(Me.Owner, project).ExDataGridView1.SelectedCells(0).RowIndex).Cells("begin_date").Value

            Me.ComboBox2.Text = CType(Me.Owner, project).ExDataGridView1.Rows(CType(Me.Owner, project).ExDataGridView1.SelectedCells(0).RowIndex).Cells("customer_no").Value
            Me.ComboBox3.Text = CType(Me.Owner, project).ExDataGridView1.Rows(CType(Me.Owner, project).ExDataGridView1.SelectedCells(0).RowIndex).Cells("factory_name").Value

            Me.TextBox1.Enabled = False

        End If

    
    End Sub

    Private Sub ComboBox1_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ComboBox1.SelectedIndexChanged
        Me.ComboBox2.SelectedIndex = Me.ComboBox1.SelectedIndex
    End Sub

    Private Sub ComboBox2_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ComboBox2.SelectedIndexChanged
        Me.ComboBox1.SelectedIndex = Me.ComboBox2.SelectedIndex
    End Sub

    Private Sub ComboBox3_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ComboBox3.SelectedIndexChanged
        Me.ComboBox4.SelectedIndex = Me.ComboBox3.SelectedIndex

    End Sub

    Private Sub ComboBox4_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ComboBox4.SelectedIndexChanged
        Me.ComboBox3.SelectedIndex = Me.ComboBox4.SelectedIndex
    End Sub
End Class