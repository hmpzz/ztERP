Imports System.Data.SqlClient
Public Class sys_month_SC

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        Dim sqlcon As New SqlConnection
        Dim sqlcmd As New SqlCommand
        Dim sqlad As New SqlDataAdapter
        Dim mytrans As SqlTransaction

        Dim sql As String = ""
        Dim dt As New DataTable

        
        Dim i As Long

        sqlcon = getconn()
        sqlcmd.Connection = sqlcon

        sql = "select count(*) from t_MonthEnd_date where left(accountterm,4)='" & Me.NumericUpDown1.Value & "'"
        sqlcmd.CommandText = sql

        If sqlcmd.ExecuteScalar = 0 Then

            mytrans = sqlcon.BeginTransaction

            sqlcmd.Transaction = mytrans


            Try

                For i = 1 To 12

                    Select Case i
                        Case 1, 3, 5, 7, 8, 10, 12
                            sql = "INSERT INTO t_MonthEnd_date (accountTerm,Begin_Date,End_Date,state) values " & _
                                                                   "( '" & Me.NumericUpDown1.Value & Format(i, "00") & "', '" & Me.NumericUpDown1.Value & "-" & Format(i, "0") & "-1', '" & Me.NumericUpDown1.Value & "-" & Format(i, "0") & "-31',0)"
                        Case 4, 6, 9, 11
                            sql = "INSERT INTO t_MonthEnd_date (accountTerm,Begin_Date,End_Date,state) values " & _
                                                                    "( '" & Me.NumericUpDown1.Value & Format(i, "00") & "', '" & Me.NumericUpDown1.Value & "-" & Format(i, "0") & "-1', '" & Me.NumericUpDown1.Value & "-" & Format(i, "0") & "-30',0)"
                        Case 2
                            If rn(Val(Me.NumericUpDown1.Value)) Then
                                sql = "INSERT INTO t_MonthEnd_date (accountTerm,Begin_Date,End_Date,state) values " & _
                                                                   "( '" & Me.NumericUpDown1.Value & Format(i, "00") & "', '" & Me.NumericUpDown1.Value & "-" & Format(i, "0") & "-1', '" & Me.NumericUpDown1.Value & "-" & Format(i, "0") & "-29',0)"
                            Else
                                sql = "INSERT INTO t_MonthEnd_date (accountTerm,Begin_Date,End_Date,state) values " & _
                                                                   "( '" & Me.NumericUpDown1.Value & Format(i, "00") & "', '" & Me.NumericUpDown1.Value & "-" & Format(i, "0") & "-1', '" & Me.NumericUpDown1.Value & "-" & Format(i, "0") & "-28',0)"
                            End If
                    End Select
                    sqlcmd.CommandText = sql
                    sqlcmd.ExecuteNonQuery()
                Next i

                mytrans.Commit()
            Catch ex As Exception
                mytrans.Rollback()
                MsgBox(ex.Message, MsgBoxStyle.OkOnly, "提示")
                Exit Sub
            End Try

            MsgBox(Me.NumericUpDown1.Value & "年 会计期间已经成功生成！", vbExclamation, "提示")
        Else
            MsgBox("已经生成该年的会计期间，无须再生成！", vbExclamation, "提示")
        End If

    End Sub
End Class