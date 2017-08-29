Imports System.Data.SqlClient
Imports ADOX

Imports CrystalDecisions.CrystalReports.Engine
Imports CrystalDecisions.Shared


Module Module1
    '读ini API函数
    Private Declare Function GetPrivateProfileString Lib "kernel32" Alias "GetPrivateProfileStringA" (ByVal lpApplicationName As String, ByVal lpKeyName As String, ByVal lpDefault As String, ByVal lpReturnedString As String, ByVal nSize As int32, ByVal lpFileName As String) As int32
    '写ini API函数
    Private Declare Function WritePrivateProfileString Lib "kernel32" Alias "WritePrivateProfileStringA" (ByVal lpApplicationName As String, ByVal lpKeyName As String, ByVal lpString As String, ByVal lpFileName As String) As int32

    't_rs表中的人员的名字
    Public rs_name As String = ""
    'sys_user中的登录名
    Public login_id As String = ""
    'sys_user中的id
    Public RS_ID As int32 = 0
    '是否有价格权限(1有权限,0没有权限)
    Public price_state As int32 = 0
    '部门ID
    Public BM_id As int32

    Public sqlserver As String
    Public databasename As String

    Public print_date As String

    '用于返回数据连接
    '周玉磬写于2010-7-8
    Public Function getconn() As SqlConnection
        
        Dim PWD As String
        'Dim sqlserver As String
        'Dim databasename As String

        'sqlserver = "192.168.0.213\erpsvr"

        'databasename = "erpsvr"
        Dim sqlcon1 As New SqlConnection
        If sqlserver.ToUpper = "H" Then
            PWD = "guochunqi"
        ElseIf sqlserver.ToUpper = "TSYPE3" Then
            PWD = "bg3721"
        ElseIf sqlserver.ToUpper = "ERP" Then
            PWD = "w1j2j4erp"
        Else 'If sqlserver.ToUpper = "server" Then
            PWD = "w1j2j4.110"
        
        End If

        sqlcon1.ConnectionString = ("password=" & PWD & ";Persist Security Info=True;user id=sa;Initial Catalog=" & databasename & ";Data Source=" & sqlserver)
        sqlcon1.Open()
        Return sqlcon1
    End Function

    '用于返回图片数据库数据连接
    '周玉磬写于2010-7-8
    Public Function getPICconn() As SqlConnection

        Dim PWD As String
        'Dim sqlserver As String
        'Dim databasename As String

        'sqlserver = "192.168.0.213\erpsvr"

        'databasename = "erpsvr"
        Dim sqlcon1 As New SqlConnection
        If sqlserver.ToUpper = "H" Then
            PWD = "guochunqi"
        ElseIf sqlserver.ToUpper = "TSYPE3" Then
            PWD = "bg3721"
        ElseIf sqlserver.ToUpper = "ERP" Then
            PWD = "w1j2j4erp"
        Else 'If sqlserver.ToUpper = "server" Then
            PWD = "w1j2j4.110"

        End If

        sqlcon1.ConnectionString = ("password=" & PWD & ";Persist Security Info=True;user id=sa;Initial Catalog=pic;Data Source=" & sqlserver)
        sqlcon1.Open()
        Return sqlcon1
    End Function

    '存储权限树的函数
    '周玉磬写于2010-7-8
    Public Function SaveCheck(ByVal node As TreeNode) As String
        If node.Checked = True Then
            SaveCheck = "'" & node.Tag & "',"
        Else
            SaveCheck = ""
            Exit Function
        End If

        If node.FirstNode Is Nothing Then
            Exit Function
        Else
            node = node.FirstNode
            SaveCheck = SaveCheck & SaveCheck(node)
        End If

        Do While Not node.NextNode Is Nothing
            node = node.NextNode
            SaveCheck = SaveCheck & SaveCheck(node)
        Loop

    End Function

    '读取ini文件内容
    '周玉磬写于2010-7-9
    Public Function GetINI(ByVal Section As String, ByVal AppName As String, ByVal lpDefault As String, ByVal FileName As String) As String
        Dim Str As String = ""
        Str = LSet(Str, 256)
        GetPrivateProfileString(Section, AppName, lpDefault, Str, Len(Str), FileName)
        Return Microsoft.VisualBasic.Left(Str, InStr(Str, Chr(0)) - 1)
    End Function

    '写ini文件操作
    '周玉磬写于2010-7-9
    Public Function WriteINI(ByVal Section As String, ByVal AppName As String, ByVal lpDefault As String, ByVal FileName As String) As Long
        WriteINI = WritePrivateProfileString(Section, AppName, lpDefault, FileName)
    End Function

    '用来判断是否是闰年
    '周玉磬写于2010-8-5
    Public Function rn(ByVal myYear As Integer) As Boolean
        If (myYear Mod 400) = 0 Or myYear Mod 4 = 0 And myYear Mod 100 <> 0 Then
            rn = True
        Else
            rn = False
        End If
    End Function

    '输入仓库名称得到仓库代码
    '周玉磬写于2010-8-16
    Public Function Get_depot_code(ByVal s As String) As String
        Dim sqlcon As New SqlConnection
        Dim sqlcmd As New SqlCommand
        Dim sql As String
        Dim s1 As String

        sqlcon = getconn()
        sqlcmd.Connection = sqlcon

        sql = "select depot_code from t_depot where depot_name='" & s & "'"
        sqlcmd.CommandText = sql
        s1 = sqlcmd.ExecuteScalar()
        If IsNothing(s1) Then
            Get_depot_code = ""
        Else
            Get_depot_code = s1
        End If

    End Function

    '输入日期得到是不是当前帐期
    '周玉磬写于2010-8-17
    Public Function Month_end(ByVal d As Date) As Boolean
        Dim sqlcon As New SqlConnection
        Dim sqlcmd As New SqlCommand
        Dim sql As String

        sqlcon = getconn()
        sqlcmd.Connection = sqlcon

        sql = "SELECT count(*) FROM t_MonthEnd_date WHERE state =2 and  ('" & d & "' >= begin_date) AND ('" & d & "' <= end_date)"
        sqlcmd.CommandText = sql
        If sqlcmd.ExecuteScalar() = 0 Then
            Month_end = False
        Else
            Month_end = True
        End If
    End Function

    '得到当前帐期
    '周玉磬写于2010-8-17
    Public Function GetAccountterm() As String
        Dim sqlcon As New SqlConnection
        Dim sqlcmd As New SqlCommand
        Dim sql As String

        sqlcon = getconn()
        sqlcmd.Connection = sqlcon

        sql = "SELECT accountterm FROM t_MonthEnd_date WHERE state =2 "
        sqlcmd.CommandText = sql
        GetAccountterm = sqlcmd.ExecuteScalar
    End Function

    

    '用来得到仓库是否停用，false停用，TRUE启用
    '周玉磬写于2010-8-19
    Public Function Get_depot_state(ByVal depot_code As String) As Boolean
        Dim sqlcon As New SqlConnection
        Dim sqlcmd As New SqlCommand
        Dim sql As String

        sqlcon = getconn()
        sqlcmd.Connection = sqlcon
        Sql = "select stop from t_depot where depot_code='" & depot_code & "'"
        sqlcmd.CommandText = Sql
        If sqlcmd.ExecuteScalar = 0 Then
            Get_depot_state = False
        Else
            Get_depot_state = True
        End If
    End Function

    '用来存储ＰＤＦ文件
    '周玉磬写于2011-2-10
    Public Sub save_pdf(ByVal report As CrystalDecisions.CrystalReports.Engine.ReportDocument, ByVal billno As String)

        Dim sql As String
        Dim sqlcon As New SqlConnection
        Dim sqlcmd As New SqlCommand

        Dim maxno As int32

        Dim PDFStream As IO.Stream = CType(report, CrystalDecisions.CrystalReports.Engine.ReportDocument).ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat)
        Dim PDFReader As IO.BinaryReader = New IO.BinaryReader(PDFStream)
        Dim PDFResult() As Byte = PDFReader.ReadBytes(PDFStream.Length)



        Dim mytrans As SqlTransaction


        sqlcon = getconn()

        mytrans = sqlcon.BeginTransaction
        sqlcmd.Transaction = mytrans

        Try

            sql = "select isnull(max(b_no),0)+1 from t_reportPDF where billno='" & billno & "' "
            sqlcmd.CommandText = sql
            sqlcmd.Connection = sqlcon
            maxno = sqlcmd.ExecuteScalar()


            sqlcmd.CommandText = "INSERT INTO t_reportPDF(billno,b_no,pdf,print_man) values (@billno,@b_no,@pdf,@name)"
            sqlcmd.Connection = sqlcon

            Dim param As SqlParameter = Nothing

            param = New SqlParameter("@billno", SqlDbType.NVarChar)
            sqlcmd.Parameters.Add(param)

            param = New SqlParameter("@b_no", SqlDbType.Int)
            sqlcmd.Parameters.Add(param)

            param = New SqlParameter("@pdf", SqlDbType.Image)
            sqlcmd.Parameters.Add(param)

            param = New SqlParameter("@name", SqlDbType.NVarChar)
            sqlcmd.Parameters.Add(param)


            sqlcmd.Parameters("@billno").Value = billno
            sqlcmd.Parameters("@b_no").Value = maxno

            sqlcmd.Parameters("@pdf").Value = PDFResult
            sqlcmd.Parameters("@pdf").Size = PDFResult.Length
            sqlcmd.Parameters("@name").Value = rs_name

            sqlcmd.ExecuteNonQuery()
            mytrans.Commit()

        Catch ex As Exception
            mytrans.Rollback()
            MsgBox("报表存储不成功！", MsgBoxStyle.OkOnly, "错误")
        End Try

    End Sub




    Public Function Print_Out(ByVal billno As String, Optional ByVal print As Boolean = True) As String

        Dim sqlcon As New SqlConnection
        Dim sqlcmd As New SqlCommand
        Dim sqlad As New SqlDataAdapter
        Dim ds As New DataSet

        Dim sql As String
        Dim i As int32

        '****************************打印报表*********************************
        Dim part_out_LL_RPT As New part_out_LL_RPT

        If price_state = 0 Then
            part_out_LL_RPT.Section5.ReportObjects("sum011").ObjectFormat.EnableSuppress = True
            part_out_LL_RPT.Section5.ReportObjects("text23").ObjectFormat.EnableSuppress = True
        Else
            part_out_LL_RPT.Section5.ReportObjects("sum011").ObjectFormat.EnableSuppress = False
            part_out_LL_RPT.Section5.ReportObjects("text23").ObjectFormat.EnableSuppress = False
        End If

        Dim doc As New System.Drawing.Printing.PrintDocument
        Dim rawKind As int32 = 11

        For i = 0 To doc.PrinterSettings.PaperSizes.Count - 1
            If doc.PrinterSettings.PaperSizes(i).PaperName.ToUpper = "ZT" Then
                rawKind = doc.PrinterSettings.PaperSizes(i).RawKind
            End If
        Next

        part_out_LL_RPT.PrintOptions.PaperSize = rawKind

        If rawKind = 11 Then
            part_out_LL_RPT.PrintOptions.PaperOrientation = CrystalDecisions.Shared.PaperOrientation.Landscape
        Else
            part_out_LL_RPT.PrintOptions.PaperOrientation = CrystalDecisions.Shared.PaperOrientation.Portrait
        End If


        sqlcon = getconn()


        sql = "select print_time from sys_setting "
        sqlcmd.Connection = sqlcon
        sqlcmd.CommandText = sql

        If sqlcmd.ExecuteScalar = 1 Then

            print_time.ShowDialog()

            sql = "select a.*,b.billkindname,c.depot_name,'" & rs_name & "' as print_man,'" & print_date & "'as print_date,d.project_no " & _
              "from t_stockbill_master a left join " & _
              "	t_stockbillkind  b on a.billkindcode=b.billkindcode left join " & _
              "	t_depot c on a.depot_code=c.depot_code left join " & _
              "   T_part_out_master d on a.ResourceNo=d.billno " & _
              "where a.billno='" & billno & "'"
        Else
            sql = "select a.*,b.billkindname,c.depot_name,'" & rs_name & "' as print_man,getdate() as print_date,d.project_no " & _
                    "from t_stockbill_master a left join " & _
                    "	t_stockbillkind  b on a.billkindcode=b.billkindcode left join " & _
                    "	t_depot c on a.depot_code=c.depot_code left join " & _
                    "   T_part_out_master d on a.ResourceNo=d.billno " & _
                    "where a.billno='" & billno & "'"
        End If






        sqlcmd.Connection = sqlcon
        sqlcmd.CommandText = sql
        sqlad.SelectCommand = sqlcmd

        sqlad.Fill(ds, "t_stockbill_master")



        sql = "SELECT a.*,b.item_name,b.item_standard,b.unit " & _
            "from t_stockbill_detail a left join " & _
             "	t_item b on a.item_code=b.item_code  " & _
             "where a.billno='" & billno & "'"

        sqlcmd.Connection = sqlcon
        sqlcmd.CommandText = sql
        sqlad.SelectCommand = sqlcmd

        sqlad.Fill(ds, "t_stockbill_detail")


        Dim text4 As TextObject

        text4 = part_out_LL_RPT.ReportDefinition.ReportObjects.Item("text4")

        If databasename.ToUpper = "ERP_2" Then
            text4.Text = "公司名称：天津市盈德展具制造有限公司"
        Else
            text4.Text = "公司名称：天津市展特装饰工程有限公司"
        End If




        part_out_LL_RPT.SetDataSource(ds)
        part_out_LL_RPT.Refresh()

        save_pdf(part_out_LL_RPT, billno)

        If print = True Then
            Try

                part_out_LL_RPT.PrintToPrinter(1, True, 0, 0)
            Catch ex As Exception
                'Try
                '    'Kill(Application.StartupPath & "\*.pdf")

                '    Dim a() As String = IO.Directory.GetFiles(Application.StartupPath)

                '    For Each a(i) In a
                '        If Microsoft.VisualBasic.Right(a(i).ToUpper, 4) = ".PDF" Then
                '            My.Computer.FileSystem.DeleteFile(a(i), FileIO.UIOption.OnlyErrorDialogs, FileIO.RecycleOption.DeletePermanently, FileIO.UICancelOption.ThrowException)
                '        End If

                '    Next

                '    part_out_LL_RPT.ExportToDisk(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, Application.StartupPath & "\" & billno & ".pdf")
                '    Print_Out = ("打印没有成功，PDF文件保存为" & Application.StartupPath & "\" & billno & ".pdf ")
                '    Exit Function
                'Catch ex1 As Exception

                '    Print_Out = "您打开了" & Application.StartupPath & "目录下的PDF文件，请关闭以后再试或者从报表查询中重新打印单据：" & billno
                '    Exit Function
                'End Try
                Print_Out = ex.Message
                Exit Function

            End Try
        End If

        Print_Out = ""

        '****************************打印报表*********************************
    End Function


    Public Function Print_in(ByVal billno As String, Optional ByVal print As Boolean = True) As String

        Dim sqlcon As New SqlConnection
        Dim sqlcmd As New SqlCommand
        Dim sqlad As New SqlDataAdapter
        Dim ds As New DataSet

        Dim sql As String
        Dim i As int32


        '****************************打印报表*********************************
        Dim part_in_CG_RPT As New part_in_CG_RPT

        Dim doc As New System.Drawing.Printing.PrintDocument
        Dim rawKind As int32 = 11

        'billno = "RK-GS-201211-0052"
        For i = 0 To doc.PrinterSettings.PaperSizes.Count - 1
            If doc.PrinterSettings.PaperSizes(i).PaperName.ToUpper = "ZT" Then
                rawKind = doc.PrinterSettings.PaperSizes(i).RawKind
            End If
        Next

        part_in_CG_RPT.PrintOptions.PaperSize = rawKind

        If rawKind = 11 Then
            part_in_CG_RPT.PrintOptions.PaperOrientation = CrystalDecisions.Shared.PaperOrientation.Landscape
        Else
            part_in_CG_RPT.PrintOptions.PaperOrientation = CrystalDecisions.Shared.PaperOrientation.Portrait
        End If




        sqlcon = getconn()

        sql = "select print_time from sys_setting "
        sqlcmd.Connection = sqlcon
        sqlcmd.CommandText = sql

        If sqlcmd.ExecuteScalar = 1 Then

            print_time.ShowDialog()
            sql = "select a.*,b.billkindname,c.depot_name,'" & rs_name & "' as print_man,'" & print_date & "' as print_date,e.supplier_name,d.check_man " & _
                "from t_stockbill_master a left join " & _
                "	t_stockbillkind  b on a.billkindcode=b.billkindcode left join " & _
                "	t_depot c on a.depot_code=c.depot_code left join " & _
                " purchase_master d on a.ResourceNo=d.billno left join  " & _
                " t_supplier e on d.supplier_no=e.supplier_no " & _
                " where a.billno='" & billno & "'"
        Else
            sql = "select a.*,b.billkindname,c.depot_name,'" & rs_name & "' as print_man,getdate() as print_date,e.supplier_name,d.check_man " & _
              "from t_stockbill_master a left join " & _
              "	t_stockbillkind  b on a.billkindcode=b.billkindcode left join " & _
              "	t_depot c on a.depot_code=c.depot_code left join " & _
              " purchase_master d on a.ResourceNo=d.billno left join  " & _
              " t_supplier e on d.supplier_no=e.supplier_no " & _
              " where a.billno='" & billno & "'"
        End If




        sqlcmd.Connection = sqlcon
        sqlcmd.CommandText = sql
        sqlad.SelectCommand = sqlcmd

        sqlad.Fill(ds, "t_stockbill_master")



        sql = "SELECT a.*,b.item_name,b.item_standard,b.unit " & _
              "from t_stockbill_detail a left join " & _
               "	t_item b on a.item_code=b.item_code  " & _
               "where a.billno='" & billno & "'"

        sqlcmd.Connection = sqlcon
        sqlcmd.CommandText = sql
        sqlad.SelectCommand = sqlcmd

        sqlad.Fill(ds, "t_stockbill_detail")


        part_in_CG_RPT.SetDataSource(ds)
        part_in_CG_RPT.Refresh()

        save_pdf(part_in_CG_RPT, billno)

        If print = True Then
            Try
                part_in_CG_RPT.PrintToPrinter(1, True, 0, 0)
            Catch ex As Exception
                Try

                    'Dim a() As String = IO.Directory.GetFiles(Application.StartupPath)

                    'For Each a(i) In a
                    '    If Microsoft.VisualBasic.Right(a(i).ToUpper, 4) = ".PDF" Then
                    '        My.Computer.FileSystem.DeleteFile(a(i), FileIO.UIOption.OnlyErrorDialogs, FileIO.RecycleOption.DeletePermanently, FileIO.UICancelOption.ThrowException)
                    '    End If

                    'Next

                    'part_in_CG_RPT.ExportToDisk(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, Application.StartupPath & "\" & billno & ".pdf")
                    'Print_in = ("打印没有成功，PDF文件保存为" & Application.StartupPath & "\" & billno & ".pdf ")
                    'Exit Function
                    Print_in = ex.Message
                    Exit Function
                Catch ex1 As Exception

                    Print_in = "您打开了" & Application.StartupPath & "目录下的PDF文件，请关闭以后再试或者从报表查询中重新打印单据：" & billno
                    Exit Function
                End Try


            End Try
        End If

        Print_in = ""

        '****************************打印报表*********************************
    End Function



    Public Function Print_in_history(ByVal billno As String, Optional ByVal print As Boolean = True) As String

        Dim sqlcon As New SqlConnection
        Dim sqlcmd As New SqlCommand
        Dim sqlad As New SqlDataAdapter
        Dim ds As New DataSet

        Dim sql As String
        Dim i As int32

        '****************************打印报表*********************************
        Dim part_in_CG_RPT As New part_in_CG_RPT

        Dim doc As New System.Drawing.Printing.PrintDocument
        Dim rawKind As int32 = 11

        ' billno = "RK-GS-201211-0052"
        For i = 0 To doc.PrinterSettings.PaperSizes.Count - 1
            If doc.PrinterSettings.PaperSizes(i).PaperName.ToUpper = "ZT" Then
                rawKind = doc.PrinterSettings.PaperSizes(i).RawKind
            End If
        Next

        part_in_CG_RPT.PrintOptions.PaperSize = rawKind

        If rawKind = 11 Then
            part_in_CG_RPT.PrintOptions.PaperOrientation = CrystalDecisions.Shared.PaperOrientation.Landscape
        Else
            part_in_CG_RPT.PrintOptions.PaperOrientation = CrystalDecisions.Shared.PaperOrientation.Portrait
        End If




        sqlcon = getconn()

        sql = "select a.*,b.billkindname,c.depot_name,'" & rs_name & "' as print_man,getdate() as print_date,e.supplier_name,d.check_man " & _
                "from t_stockbill_master_history a left join " & _
                "	t_stockbillkind  b on a.billkindcode=b.billkindcode left join " & _
                "	t_depot c on a.depot_code=c.depot_code left join " & _
                " purchase_master d on a.ResourceNo=d.billno left join  " & _
                " t_supplier e on d.supplier_no=e.supplier_no " & _
                " where a.billno='" & billno & "'"


        sqlcmd.Connection = sqlcon
        sqlcmd.CommandText = sql
        sqlad.SelectCommand = sqlcmd

        sqlad.Fill(ds, "t_stockbill_master")



        sql = "SELECT a.*,b.item_name,b.item_standard,b.unit " & _
              "from t_stockbill_detail_history a left join " & _
               "	t_item b on a.item_code=b.item_code  " & _
               "where a.billno='" & billno & "'"

        sqlcmd.Connection = sqlcon
        sqlcmd.CommandText = sql
        sqlad.SelectCommand = sqlcmd

        sqlad.Fill(ds, "t_stockbill_detail")


        part_in_CG_RPT.SetDataSource(ds)
        part_in_CG_RPT.Refresh()

        save_pdf(part_in_CG_RPT, billno)

        If print = True Then
            Try
                part_in_CG_RPT.PrintToPrinter(1, True, 0, 0)
            Catch ex As Exception
                Try

                    'Dim a() As String = IO.Directory.GetFiles(Application.StartupPath)

                    'For Each a(i) In a
                    '    If Microsoft.VisualBasic.Right(a(i).ToUpper, 4) = ".PDF" Then
                    '        My.Computer.FileSystem.DeleteFile(a(i), FileIO.UIOption.OnlyErrorDialogs, FileIO.RecycleOption.DeletePermanently, FileIO.UICancelOption.ThrowException)
                    '    End If

                    'Next

                    'part_in_CG_RPT.ExportToDisk(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, Application.StartupPath & "\" & billno & ".pdf")
                    'Print_in = ("打印没有成功，PDF文件保存为" & Application.StartupPath & "\" & billno & ".pdf ")
                    'Exit Function
                    Print_in_history = ex.Message
                    Exit Function
                Catch ex1 As Exception

                    Print_in_history = "您打开了" & Application.StartupPath & "目录下的PDF文件，请关闭以后再试或者从报表查询中重新打印单据：" & billno
                    Exit Function
                End Try


            End Try
        End If

        Print_in_history = ""

        '****************************打印报表*********************************
    End Function




    Public Function Print_Out_history(ByVal billno As String, Optional ByVal print As Boolean = True) As String

        Dim sqlcon As New SqlConnection
        Dim sqlcmd As New SqlCommand
        Dim sqlad As New SqlDataAdapter
        Dim ds As New DataSet

        Dim sql As String
        Dim i As int32

        '****************************打印报表*********************************
        Dim part_out_LL_RPT As New part_out_LL_RPT

        Dim doc As New System.Drawing.Printing.PrintDocument
        Dim rawKind As int32 = 11

        For i = 0 To doc.PrinterSettings.PaperSizes.Count - 1
            If doc.PrinterSettings.PaperSizes(i).PaperName.ToUpper = "ZT" Then
                rawKind = doc.PrinterSettings.PaperSizes(i).RawKind
            End If
        Next

        part_out_LL_RPT.PrintOptions.PaperSize = rawKind

        If rawKind = 11 Then
            part_out_LL_RPT.PrintOptions.PaperOrientation = CrystalDecisions.Shared.PaperOrientation.Landscape
        Else
            part_out_LL_RPT.PrintOptions.PaperOrientation = CrystalDecisions.Shared.PaperOrientation.Portrait
        End If


        sqlcon = getconn()

        sql = "select a.*,b.billkindname,c.depot_name,'" & rs_name & "' as print_man,getdate() as print_date,d.project_no " & _
                "from t_stockbill_master_history a left join " & _
                "	t_stockbillkind  b on a.billkindcode=b.billkindcode left join " & _
                "	t_depot c on a.depot_code=c.depot_code left join " & _
                "   T_part_out_master d on a.ResourceNo=d.billno " & _
                "where a.billno='" & billno & "'"


        sqlcmd.Connection = sqlcon
        sqlcmd.CommandText = sql
        sqlad.SelectCommand = sqlcmd

        sqlad.Fill(ds, "t_stockbill_master")



        sql = "SELECT a.*,b.item_name,b.item_standard,b.unit " & _
            "from t_stockbill_detail_history a left join " & _
             "	t_item b on a.item_code=b.item_code  " & _
             "where a.billno='" & billno & "'"

        sqlcmd.Connection = sqlcon
        sqlcmd.CommandText = sql
        sqlad.SelectCommand = sqlcmd

        sqlad.Fill(ds, "t_stockbill_detail")


        part_out_LL_RPT.SetDataSource(ds)
        part_out_LL_RPT.Refresh()

        save_pdf(part_out_LL_RPT, billno)

        If print = True Then
            Try

                part_out_LL_RPT.PrintToPrinter(1, True, 0, 0)
            Catch ex As Exception
                'Try
                '    'Kill(Application.StartupPath & "\*.pdf")

                '    Dim a() As String = IO.Directory.GetFiles(Application.StartupPath)

                '    For Each a(i) In a
                '        If Microsoft.VisualBasic.Right(a(i).ToUpper, 4) = ".PDF" Then
                '            My.Computer.FileSystem.DeleteFile(a(i), FileIO.UIOption.OnlyErrorDialogs, FileIO.RecycleOption.DeletePermanently, FileIO.UICancelOption.ThrowException)
                '        End If

                '    Next

                '    part_out_LL_RPT.ExportToDisk(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, Application.StartupPath & "\" & billno & ".pdf")
                '    Print_Out = ("打印没有成功，PDF文件保存为" & Application.StartupPath & "\" & billno & ".pdf ")
                '    Exit Function
                'Catch ex1 As Exception

                '    Print_Out = "您打开了" & Application.StartupPath & "目录下的PDF文件，请关闭以后再试或者从报表查询中重新打印单据：" & billno
                '    Exit Function
                'End Try
                Print_Out_history = ex.Message
                Exit Function

            End Try
        End If

        Print_Out_history = ""

        '****************************打印报表*********************************
    End Function




    '用来存储任意文件
    '周玉磬写于2011-2-10
    Public Sub save_file(ByVal filename As String, ByVal billno As String)

        Dim sql As String = ""
        Dim sqlcon As New SqlConnection
        Dim sqlcmd As New SqlCommand

        Dim maxno As int32 = 0




        'Dim PDFStream As IO.Stream = CType(report, CrystalDecisions.CrystalReports.Engine.ReportDocument).ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat)
        'Dim PDFReader As IO.BinaryReader = New IO.BinaryReader(PDFStream)
        'Dim PDFResult() As Byte = PDFReader.ReadBytes(PDFStream.Length)

        'Dim fs As IO.FileStream = New IO.FileStream(FileName, IO.FileMode.Open, IO.FileAccess.Read)
        'fs.Read(ib, 0, 60000)
        'sqlcmd.Parameters.Add("@pic", SqlDbType.Image, fs.Length).Value = ib
        'sqlcmd.ExecuteNonQuery()


        Dim FileStream As IO.FileStream = New IO.FileStream(filename, IO.FileMode.Open, IO.FileAccess.Read)

        Dim FileResult(FileStream.Length) As Byte
        FileStream.Read(FileResult, 0, FileStream.Length)

        sqlcon = getconn()

        sqlcmd.CommandText = "INSERT INTO pic (myfile,filename,billno ) values (@myfile,@filename,@billno)"
        sqlcmd.Connection = sqlcon

        Dim param As SqlParameter = Nothing



        param = New SqlParameter("@myfile", SqlDbType.Image)
        sqlcmd.Parameters.Add(param)


        param = New SqlParameter("@filename", SqlDbType.NVarChar)
        sqlcmd.Parameters.Add(param)

        param = New SqlParameter("@billno", SqlDbType.NVarChar)
        sqlcmd.Parameters.Add(param)

        sqlcmd.Parameters("@myfile").Value = FileResult
        sqlcmd.Parameters("@myfile").Size = FileResult.Length

        sqlcmd.Parameters("@filename").Value = filename.Trim.Substring(filename.Trim.LastIndexOf("\") + 1)

        sqlcmd.Parameters("@billno").Value = billno

        sqlcmd.ExecuteNonQuery()






        'Dim mytrans As SqlTransaction


        'sqlcon = getconn()

        'mytrans = sqlcon.BeginTransaction
        'sqlcmd.Transaction = mytrans

        'Try

        '    sql = "select isnull(max(b_no),0)+1 from t_reportPDF where billno='" & billno & "' "
        '    sqlcmd.CommandText = sql
        '    sqlcmd.Connection = sqlcon
        '    maxno = sqlcmd.ExecuteScalar()


        '    sqlcmd.CommandText = "INSERT INTO t_reportPDF(billno,b_no,pdf,print_man) values (@billno,@b_no,@pdf,@name)"
        '    sqlcmd.Connection = sqlcon

        '    Dim param As SqlParameter = Nothing

        '    param = New SqlParameter("@billno", SqlDbType.NVarChar)
        '    sqlcmd.Parameters.Add(param)

        '    param = New SqlParameter("@b_no", SqlDbType.Int)
        '    sqlcmd.Parameters.Add(param)

        '    param = New SqlParameter("@pdf", SqlDbType.Image)
        '    sqlcmd.Parameters.Add(param)

        '    param = New SqlParameter("@name", SqlDbType.NVarChar)
        '    sqlcmd.Parameters.Add(param)


        '    sqlcmd.Parameters("@billno").Value = billno
        '    sqlcmd.Parameters("@b_no").Value = maxno

        '    sqlcmd.Parameters("@pdf").Value = PDFResult
        '    sqlcmd.Parameters("@pdf").Size = PDFResult.Length
        '    sqlcmd.Parameters("@name").Value = rs_name

        '    sqlcmd.ExecuteNonQuery()
        '    mytrans.Commit()

        'Catch ex As Exception
        '    mytrans.Rollback()
        '    MsgBox("报表存储不成功！", MsgBoxStyle.OkOnly, "错误")
        'End Try

    End Sub

    '用来输出任意文件到指定目录
    '周玉磬写于2011-2-10
    Public Sub OutputFile(ByVal filename As String, ByVal billno As String)
        Dim sqlcon As New SqlConnection
        Dim sqlcmd As New SqlCommand
        Dim sqlad As New SqlDataAdapter

        Dim dr As SqlDataReader

        sqlcon = getconn()
        sqlcmd.Connection = sqlcon

        Dim picbytes() As Byte = {}



        sqlcmd.CommandText = "select * from pic where billno='" & billno & "' and filename='" & filename & "'"
        dr = sqlcmd.ExecuteReader()

        If dr.Read Then
            picbytes = dr.Item("myfile")
        End If




        Dim ms = New System.IO.MemoryStream
        ms.Write(picbytes, 0, picbytes.Length)


        Dim data() As Byte

        Dim fs As IO.FileStream

        data = dr.Item("myfile")

        Try
            If IO.File.Exists(Application.StartupPath + "\pic\" & billno & filename) Then
                IO.File.Delete(Application.StartupPath + "\pic\" & billno & filename)
            End If
            fs = IO.File.Create(Application.StartupPath + "\pic\" & billno & filename)
            fs.Write(data, 0, data.GetUpperBound(0))
            fs.Close()

            System.Diagnostics.Process.Start(Application.StartupPath + "\pic\" & billno & filename)


            'If IO.File.Exists(Application.StartupPath + "\pdf.pdf") Then
            '    IO.File.Delete(Application.StartupPath + "\pdf.pdf")
            'End If
            'fs = IO.File.Create(Application.StartupPath + "\pdf.pdf")
            'fs.Write(data, 0, data.GetUpperBound(0))
            'fs.Close()

            'System.Diagnostics.Process.Start(Application.StartupPath + "\pic\" & billno & filename)
        Catch ex As Exception
            MsgBox("打开出错！", MsgBoxStyle.OkOnly, "提示")
            Exit Sub
        End Try
    End Sub
End Module
