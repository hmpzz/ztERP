Imports System.Data.SqlClient
Public Class tzd_mod
    Friend depot_code As String

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        Dim sqlcon As New SqlConnection
        Dim sqlcmd As New SqlCommand
        Dim sqlad As New SqlDataAdapter

        Dim mytrans As SqlTransaction

        Dim sql As String
        Dim dt As New DataTable
        Dim billno As String
        Dim old_amount As Decimal
        Dim old_price As Decimal
        Dim old_money As Decimal


        sqlcon = getconn()
        sqlcmd.Connection = sqlcon
        mytrans = sqlcon.BeginTransaction

        sqlcmd.Transaction = mytrans



        Try

            sql = "update t_dj_num set num=num+1 where  tt='tz-" & depot_code & "'"
            sqlcmd.CommandText = sql
            sqlcmd.ExecuteNonQuery()

            sql = "select * from t_dj_num where  tt='tz-" & depot_code & "'"
            sqlcmd.CommandText = sql
            sqlad.SelectCommand = sqlcmd
            dt.Rows.Clear()
            sqlad.Fill(dt)

            billno = dt.Rows(0)("TT") & "-" & GetAccountterm() & "-" & Format(dt.Rows(0)("num"), dt.Rows(0)("format"))


            sql = "select amount,price,money from t_part where depot_code='" & depot_code & "' and item_code='" & Me.TextBox1.Text.Trim & "'"
            sqlcmd.CommandText = sql
            sqlad.SelectCommand = sqlcmd
            dt.Clear()
            sqlad.Fill(dt)

            old_amount = dt.Rows(0)("amount")
            old_price = dt.Rows(0)("price")
            old_money = dt.Rows(0)("money")


            sql = "insert into t_tzd(billno,amount,old_amount,tz_man,tz_date,depot_code,item_code,price,old_price,money,old_money) values " & _
                            "('" & billno & "'," & Me.TextBox3.Text.Trim & "," & old_amount & ",'" & rs_name & "',getdate(),'" & depot_code & "','" & Me.TextBox1.Text.Trim & "','" & Me.TextBox4.Text.Trim & "','" & old_price & "','" & Me.TextBox5.Text.Trim & "','" & old_money & "')"
            sqlcmd.CommandText = sql
            sqlcmd.ExecuteNonQuery()

            sql = "update t_part set amount= " & Me.TextBox3.Text.Trim & ",price=" & Me.TextBox4.Text.Trim & ",money=" & Me.TextBox5.Text.Trim & " where depot_code='" & depot_code & "' and item_code='" & Me.TextBox1.Text.Trim & "'"
            sqlcmd.CommandText = sql
            sqlcmd.ExecuteNonQuery()

      

            'sql = "update t_part set  price=case  when amount=0 then  0 else   money / amount end    where depot_code='" & depot_code & "' and item_code='" & Me.TextBox1.Text.Trim & "'"
            'sqlcmd.CommandText = sql
            'sqlcmd.ExecuteNonQuery()


            mytrans.Commit()
        Catch ex As Exception
            mytrans.Rollback()
            MsgBox(ex.Message, MsgBoxStyle.OkOnly, "错误")
            Exit Sub
        End Try

        MsgBox("保存成功！", MsgBoxStyle.OkOnly, "提示")


        CType(Me.Owner, TZD).ExDataGridView1.Rows(CType(Me.Owner, TZD).ExDataGridView1.SelectedCells(0).RowIndex).Cells("amount").Value = Me.TextBox3.Text.Trim


        CType(Me.Owner, TZD).ExDataGridView1.Rows(CType(Me.Owner, TZD).ExDataGridView1.SelectedCells(0).RowIndex).Cells("price").Value = Me.TextBox4.Text.Trim
        CType(Me.Owner, TZD).ExDataGridView1.Rows(CType(Me.Owner, TZD).ExDataGridView1.SelectedCells(0).RowIndex).Cells("money").Value = Me.TextBox5.Text.Trim



        Me.DialogResult = Windows.Forms.DialogResult.OK
    End Sub

    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button2.Click
        Me.DialogResult = Windows.Forms.DialogResult.Cancel
    End Sub

    Private Sub TextBox3_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles TextBox3.LostFocus
        If Not IsNumeric(Me.TextBox3.Text.Trim) Then
            MsgBox("数量必须是数字！", MsgBoxStyle.OkOnly, "提示")
            Exit Sub
        Else
            If Me.TextBox3.Text < 0 Then
                MsgBox("数量必须大于等于零！", MsgBoxStyle.OkOnly, "提示")
                Exit Sub
            End If
        End If
        Me.TextBox5.Text = Convert.ToDecimal(Me.TextBox3.Text) * Convert.ToDecimal(Me.TextBox4.Text)
    End Sub

    Private Sub TextBox3_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TextBox3.TextChanged
       
    End Sub

    Private Sub TextBox4_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles TextBox4.LostFocus
        If Not IsNumeric(Me.TextBox4.Text.Trim) Then
            MsgBox("单价必须是数字！", MsgBoxStyle.OkOnly, "提示")
            Exit Sub
        Else
            If Me.TextBox4.Text < 0 Then
                MsgBox("单价必须大于等于零！", MsgBoxStyle.OkOnly, "提示")
                Exit Sub
            End If
        End If
        Me.TextBox5.Text = Convert.ToDecimal(Me.TextBox3.Text) * Convert.ToDecimal(Me.TextBox4.Text)
    End Sub

    Private Sub TextBox4_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TextBox4.TextChanged

    End Sub

    Private Sub TextBox5_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles TextBox5.LostFocus
        If Not IsNumeric(Me.TextBox5.Text.Trim) Then
            MsgBox("金额必须是数字！", MsgBoxStyle.OkOnly, "提示")
            Exit Sub
        Else
            If Me.TextBox5.Text < 0 Then
                MsgBox("金额必须大于等于零！", MsgBoxStyle.OkOnly, "提示")
                Exit Sub
            End If
        End If
        If Me.TextBox3.Text = 0 Or Me.TextBox3.Text.Trim.Length = 0 Then
            MsgBox("请先填写数量!", MsgBoxStyle.OkOnly, "提示")
            Me.TextBox5.Text = 0
        Else
            Me.TextBox4.Text = Convert.ToDecimal(Me.TextBox5.Text) / Convert.ToDecimal(Me.TextBox3.Text)
        End If
    End Sub

    Private Sub TextBox5_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TextBox5.TextChanged

    End Sub
End Class