Imports System.Data.SqlClient

Public Class XDJL_add
    Friend state As String
    Friend dt_p As New DataTable
    Friend billno As String


    Private Sub XDJL_add_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Dim sqlcon As New SqlConnection
        Dim sqlcmd As New SqlCommand
        Dim sqlad As New SqlDataAdapter

        Dim dt As New DataTable
        Dim sql As String
        Dim i As Int32

        sqlcon = getconn()
        sqlcmd.Connection = sqlcon



        sql = "select * from t_bm where stop=1 order by bmmc"

        sqlcmd.CommandText = sql
        sqlad.SelectCommand = sqlcmd
        dt.Rows.Clear()
        sqlad.Fill(dt)

        For i = 0 To dt.Rows.Count - 1
            Me.ComboBox1.Items.Add(dt.Rows(i)("bmmc"))
            Me.ComboBox2.Items.Add(dt.Rows(i)("bmno"))

            Me.ComboBox3.Items.Add(dt.Rows(i)("bmmc"))
            Me.ComboBox4.Items.Add(dt.Rows(i)("bmno"))
        Next





        If state = "new" Then


            Me.Text = "下单建立"


            sql = "SELECT    * " & _
                    "FROM     scd_master a " & _
                    "where a.id=-1"

            sql = "select bmmc from t_bm where id=" & BM_id & ""

            sqlcmd.CommandText = sql
            Me.ComboBox1.Text = sqlcmd.ExecuteScalar


        ElseIf state = "mod" Then

            Me.Text = "下单修改"



            Me.TextBox1.Text = CType(Me.Owner, XDJL).ExDataGridView1.Rows(CType(Me.Owner, XDJL).ExDataGridView1.SelectedCells(0).RowIndex).Cells("billno").Value
            Me.TextBox2.Text = CType(Me.Owner, XDJL).ExDataGridView1.Rows(CType(Me.Owner, XDJL).ExDataGridView1.SelectedCells(0).RowIndex).Cells("memo").Value

            Me.ComboBox2.Text = CType(Me.Owner, XDJL).ExDataGridView1.Rows(CType(Me.Owner, XDJL).ExDataGridView1.SelectedCells(0).RowIndex).Cells("xd_bmno").Value
            Me.ComboBox4.Text = CType(Me.Owner, XDJL).ExDataGridView1.Rows(CType(Me.Owner, XDJL).ExDataGridView1.SelectedCells(0).RowIndex).Cells("jd_bmno").Value

        End If

        Me.ComboBox1.Enabled = False
    End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)

    End Sub

    Private Sub ToolStripButton_exit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripButton_exit.Click
        Me.Close()
    End Sub

    Private Sub ToolStripButton_save_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripButton_save.Click


        Dim sqlcon As New SqlConnection
        Dim sqlcmd As New SqlCommand
        Dim sqlad As New SqlDataAdapter
        Dim mytrans As SqlTransaction

        Dim dt As New DataTable
        Dim sql As String
        Dim i As Int32 = 0

        Dim s As String = ""
        Dim billno As String = ""

        Me.TextBox2.Focus()

        If Me.ComboBox1.SelectedIndex < 0 Then
            MsgBox("没有下单部门不能保存！", MsgBoxStyle.OkOnly, "提示")
            Exit Sub
        End If



        If Me.ComboBox3.SelectedIndex < 0 Then
            MsgBox("没有接单部门不能保存！", MsgBoxStyle.OkOnly, "提示")
            Exit Sub
        End If





        sqlcon = getconn()
        sqlcmd.Connection = sqlcon
        mytrans = sqlcon.BeginTransaction

        sqlcmd.Transaction = mytrans



        Try
            If state = "new" Then


                sql = "update t_dj_num set num=num+1 where  tt='scd'"
                sqlcmd.CommandText = sql
                sqlcmd.ExecuteNonQuery()

                sql = "select * from t_dj_num where tt='scd'"
                sqlcmd.CommandText = sql
                sqlad.SelectCommand = sqlcmd
                dt.Rows.Clear()
                sqlad.Fill(dt)

                billno = dt.Rows(0)("TT") & "-" & GetAccountterm() & "-" & Format(dt.Rows(0)("num"), dt.Rows(0)("format"))



                sql = "INSERT INTO [erpsvr].[dbo].[SCD_master] " & _
                        " ([billno],[XD_BMno],[JD_BMNO],[memo],[state],[JD_state],[input_man],[input_date],[money])     VALUES " & _
                        "('" & billno & "','" & Me.ComboBox2.Text & "','" & Me.ComboBox4.Text & "','" & Me.TextBox2.Text.Trim & "',0,0,'" & rs_name & "',getdate(),0)"


                sqlcmd.CommandText = sql
                sqlcmd.ExecuteNonQuery()




            ElseIf state = "mod" Then



                '如果已经不是新建就不能修改
                sql = "select count(*) from SCD_master where state=0 and billno='" & Me.TextBox1.Text.Trim & "'"
                sqlcmd.CommandText = sql
                If sqlcmd.ExecuteScalar = 0 Then
                    Throw New System.Exception("该单据不是新建状态，不可以修改！")
                End If

                sql = "UPDATE [erpsvr].[dbo].[SCD_master] " & vbCrLf & _
                        "   SET [XD_BMno] = '" & Me.ComboBox2.Text & "' " & vbCrLf & _
                        "      ,[JD_BMNO] ='" & Me.ComboBox4.Text & "' " & vbCrLf & _
                        "      ,[memo] = '" & Me.TextBox2.Text.Trim & "' " & vbCrLf & _
                        "      ,[input_man] = '" & rs_name & "' " & vbCrLf & _
                        " WHERE billno='" & Me.TextBox1.Text.Trim & "'"

                sqlcmd.CommandText = sql
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


            'CType(Me.Owner, purchase).ExDataGridView1.Rows(CType(Me.Owner, purchase).ExDataGridView1.SelectedCells(0).RowIndex).Cells("project_no").Value = Me.TextBox2.Text
            CType(Me.Owner, XDJL).ExDataGridView1.Rows(CType(Me.Owner, XDJL).ExDataGridView1.SelectedCells(0).RowIndex).Cells("memo").Value = Me.TextBox2.Text
            CType(Me.Owner, XDJL).ExDataGridView1.Rows(CType(Me.Owner, XDJL).ExDataGridView1.SelectedCells(0).RowIndex).Cells("xd_bmmc").Value = Me.ComboBox1.Text
            CType(Me.Owner, XDJL).ExDataGridView1.Rows(CType(Me.Owner, XDJL).ExDataGridView1.SelectedCells(0).RowIndex).Cells("jd_bmmc").Value = Me.ComboBox3.Text


        End If
        Me.DialogResult = Windows.Forms.DialogResult.OK
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