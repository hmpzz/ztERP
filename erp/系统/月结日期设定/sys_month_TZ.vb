Imports System.Data.SqlClient

Public Class sys_month_TZ
    Public accountterm As String = ""

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        Dim sqlcon As New SqlConnection
        Dim sqlcmd As New SqlCommand
        Dim mytrans As SqlTransaction

        Dim bdate As String
        Dim edate As String
        Dim Enddate As Boolean


        Dim sql As String = ""

        Dim i As Int32

        If Me.DateTimePicker1.Value > Me.DateTimePicker2.Value Then
            MsgBox("结束日期小于开始日期，请重新输入！", vbExclamation, "提示")
            Exit Sub
        End If



        If MsgBox("确认日期修改吗？", vbQuestion + vbYesNo, "提示") = vbNo Then
            Exit Sub
        End If




        bdate = Format(DateTimePicker1.Value, "yyyy-MM-dd")
        edate = Format(DateTimePicker2.Value, "yyyy-MM-dd")

        If edate = CDate(CDate(edate).AddMonths(1).Year.ToString + "/" + CDate(edate).AddMonths(1).Month.ToString + "/01").AddDays(-1) Then
            Enddate = True
        Else
            Enddate = False
        End If

        sqlcon = getconn()
        sqlcmd.Connection = sqlcon
        mytrans = sqlcon.BeginTransaction()

        sqlcmd.Transaction = mytrans

        Try
            For i = CInt(Microsoft.VisualBasic.Right(accountterm, 2)) To 12
                sql = "update t_MonthEnd_date set begin_date='" & bdate & "',end_date='" & edate & "' where accountterm='" & accountterm & "'"
                sqlcmd.CommandText = sql
                sqlcmd.ExecuteNonQuery()

                bdate = CDate(edate).AddDays(1)

                If Enddate = True Then
                    edate = CDate(CDate(edate).AddMonths(1).Year.ToString + "/" + CDate(edate).AddMonths(2).Month.ToString + "/01").AddDays(-1)
                Else
                    edate = CDate(edate).AddMonths(1)
                End If


                accountterm = Microsoft.VisualBasic.Left(accountterm, 4) + Format(i + 1, "00")
            Next i
            mytrans.Commit()

        Catch ex As Exception
            mytrans.Rollback()
            MsgBox(ex.Message, MsgBoxStyle.OkOnly, "提示")
            Exit Sub
        End Try


        MsgBox("修改成功！", vbExclamation, "提示")

        Me.DialogResult = Windows.Forms.DialogResult.OK
        Me.Close()

    End Sub

    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button2.Click
        Me.DialogResult = Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub

    Private Sub sys_month_TZ_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Microsoft.VisualBasic.Right(accountterm, 2) = "01" Then
            Me.DateTimePicker1.Enabled = True
        Else
            Me.DateTimePicker1.Enabled = False
        End If
    End Sub
End Class