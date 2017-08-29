Imports System.Windows.Forms
Imports System.Data.SqlClient

Public Class MDIPForm


    Private Sub ShowForm(ByVal SubForm As Form, Optional ByVal m As int16 = 0)
        '0表示最大化，1表示不最大化，并且是弹出窗体
        For Each ChildForm As Form In Me.MdiChildren
            If ChildForm.Name = SubForm.Name Then

                ChildForm.BringToFront()
                ChildForm.Activate()
                Exit Sub
            End If
        Next

        If m = 0 Then
            SubForm.MdiParent = Me
            SubForm.WindowState = FormWindowState.Maximized
            SubForm.Show()
        Else

            SubForm.ShowDialog()
        End If
    End Sub

    Private Sub MDIPForm_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Dim sqlcon As New SqlConnection
        Dim sqlcmd As New SqlCommand
        Dim sqlad As New SqlDataAdapter
        Dim dt As New DataTable
        Dim path As String
        Dim sql As String



        Dim myArg() As String

        myArg = System.Environment.GetCommandLineArgs
        If myArg.Length <> 1 Then
            If myArg(1).ToUpper = "-A" Then
                Me.BackgroundImage = Nothing
            End If

        End If
        
        path = Application.StartupPath + "\ini.ini"

        sqlserver = GetINI("server", "server", "", path)
        databasename = GetINI("database", "database", "", path)


        UnloadMenuStrip(Me.MenuStrip1)

        Dim loginform As New LoginForm

        loginform.Owner = Me
        loginform.ShowDialog()

      



        Me.ToolStripStatusLabel1.Text = login_id
        Me.ToolStripStatusLabel4.Text = rs_name

        sqlcon = getconn()

        sqlcmd.Connection = sqlcon
        If login_id = "admin" Then

            Show_Main_menustrip()
        Else

            sql = "select b.* " & _
                        "from sys_user_right a left join  " & _
                        "	sys_menu b on a.menu_no=b.menu_no " & _
                        "where a.in_user=1 and b.in_user=1 and a.login_id='" & login_id & "' and b.menu_no<>'0' and menu_index is not null " & _
                        "order by b.menu_index "
            sqlcmd.CommandText = sql
            sqlad.SelectCommand = sqlcmd
            dt.Clear()

            sqlad.Fill(dt)


            ShowMenuStrip(dt, Me.MenuStrip1, sqlcon)
        End If
        

    End Sub




    Private Sub UnloadMenuStrip(ByVal Menu As MenuStrip)

        Dim i As int16
        For i = 0 To Menu.Items.Count - 1
            Menu.Items(i).Visible = False
            UnloadMenuItemStrip(Menu.Items(i))
        Next
    End Sub




    Private Sub UnloadMenuItemStrip(ByVal MenuItems As ToolStripMenuItem)
        Dim i As int16
        For i = 0 To MenuItems.DropDownItems.Count - 1
            MenuItems.DropDownItems(i).Visible = False
            If TypeOf MenuItems.DropDownItems(i) Is ToolStripMenuItem Then
                UnloadMenuItemStrip(MenuItems.DropDownItems(i))
            End If
        Next i
    End Sub

   

    Private Sub ShowMenuStrip(ByRef dt As DataTable, ByVal Menu As MenuStrip, ByVal sqlcon As SqlConnection, Optional ByRef j As int16 = 0)
        '显示菜单，将用户权限的adors传入，将待显示的菜单传入]
        '配合ShowMenuItem使用

        Dim s As String
        Dim T As Boolean = True '用来记录上一菜单是否则为分隔线(上来默认显示是分隔菜单，为了不显示第一个分隔菜单)
        Dim P As int16 = -1 '用来记录最后一个显示的菜单的index


        Dim i As int16

        For i = 0 To Menu.Items.Count - 1
            If dt.Rows.Count = j Then Exit Sub
            '为了将index使用化成三位字符的格式
            If i < 10 Then
                s = "00" & i
            ElseIf i < 100 Then
                s = "0" & i
            Else
                s = i
            End If

            '如果sys_menu中的menu_index项目与格式化后菜单的index相同则显示
            'If dt.Rows(j)("menu_index") = s Then
            If dt.Rows(j)("menu_id") = Menu.Items(i).Name Then
                T = False '表示最后一项不是分隔线
                P = i '记录最后一个显示的菜单的index
                Menu.Items(i).Visible = True
                j = j + 1
                '显示其子菜单
                ShowMenuItemStrip(dt, Menu.Items(i), s, sqlcon, j)
            Else
                If Not TypeOf Menu.Items(i) Is ToolStripMenuItem Then
                    If T = True Then '如果上一个显示的菜单是分隔线则不显示分隔线
                        Menu.Items(i).Visible = False
                    Else
                        T = True '表示最后一项是分隔线
                        P = i '记录最后一个显示的菜单的index
                        Menu.Items(i).Visible = True
                    End If
                Else
                    Menu.Items(i).Visible = False
                End If
            End If
        Next
        If P <> -1 Then
            If Not TypeOf Menu.Items(P) Is ToolStripMenuItem Then
                Menu.Items(P).Visible = False
            End If
        End If
    End Sub



    Private Sub ShowMenuItemStrip(ByRef dt As DataTable, ByVal MenuItems As ToolStripMenuItem, ByVal s As String, ByVal sqlcon As SqlConnection, Optional ByRef j As int16 = 0)
        Dim T As Boolean = True '用来记录上一菜单是否则为分隔线(上来默认显示是分隔菜单，为了不显示第一个分隔菜单)
        Dim P As int16 = -1 '用来记录最后一个显示的菜单的index

        Dim i As int16
        Dim k As String
        For i = 0 To MenuItems.DropDownItems.Count - 1
            If dt.Rows.Count = j Then Exit Sub
            '如果sys_menu中的menu_index项目与格式化后菜单的index相同则显示
            If i < 10 Then
                k = s & "00" & i
            ElseIf i < 100 Then
                k = s & 0 & i
            Else
                k = s & i
            End If

            '如果sys_menu中的menu_index项目与格式化后菜单的index相同则显示
            'If k = dt.Rows(j)("menu_index") Then
            If MenuItems.DropDownItems(i).Name = dt.Rows(j)("menu_id") Then
                T = False '表示最后一项不是分隔线
                P = i '记录最后一个显示的菜单的index
                MenuItems.DropDownItems(i).Visible = True
                j = j + 1
                ShowMenuItemStrip(dt, MenuItems.DropDownItems(i), k, sqlcon, j)
            Else
                If Not TypeOf MenuItems.DropDownItems(i) Is ToolStripMenuItem Then
                    If T = True Then '如果上一个显示的菜单是分隔线则不显示分隔线
                        MenuItems.DropDownItems(i).Visible = False
                    Else
                        T = True '表示最后一项是分隔线
                        P = i '记录最后一个显示的菜单的index
                        MenuItems.DropDownItems(i).Visible = True
                    End If
                Else
                    MenuItems.DropDownItems(i).Visible = False
                End If
            End If
        Next i
        If P <> -1 Then
            If Not TypeOf MenuItems.DropDownItems(P) Is ToolStripMenuItem Then
                MenuItems.DropDownItems(P).Visible = False
            End If
        End If
    End Sub

    Private Sub GetMenuStrip(ByVal Menu As MenuStrip, ByVal sqlcmd As SqlCommand)

        '将界面菜单的ID写入到sys_menu表中
        '将传入的菜单及其子项目的index写入sys_menu菜单
        '配合GetMenuItem使用
        Dim sql As String
        Dim s As String
        Dim i As int16
        sql = "update sys_menu set menu_index=null where menu_no<>'0' "
        sqlcmd.CommandText = sql
        sqlcmd.ExecuteNonQuery()

        For i = 0 To Menu.Items.Count - 1

            If i < 10 Then
                s = "00" & i
            ElseIf i < 100 Then
                s = "0" & i
            Else
                s = i
            End If

            sql = "update sys_menu set menu_index='" & s & "' where menu_id='" & Menu.Items(i).Name & "' and menu_no=(select top 1 menu_no from sys_menu where menu_id='" & Menu.Items(i).Name & "' and (menu_index='' or menu_index is null) order by menu_no  ) "
            sqlcmd.CommandText = sql
            sqlcmd.ExecuteNonQuery()
            GetMenuItemStrip(Menu.Items(i), s, sqlcmd)
        Next

    End Sub
    Private Sub GetMenuItemStrip(ByVal MenuItems As ToolStripMenuItem, ByVal s As String, ByVal sqlcmd As SqlCommand)

        '配合GetMenu使用
        '将传入的菜单项目及其子项目的index写入sys_menu菜单
        Dim i As int16
        Dim sql As String
        Dim k As String
        For i = 0 To MenuItems.DropDownItems.Count - 1
            If i < 10 Then
                k = s & "00" & i
            ElseIf i < 100 Then
                k = s & "0" & i
            Else
                k = s & i
            End If

            If TypeOf MenuItems.DropDownItems(i) Is ToolStripMenuItem Then
                Debug.WriteLine(k & "    " & MenuItems.DropDownItems(i).Text)
                sql = "update sys_menu set menu_index='" & k & "' where menu_id='" & MenuItems.DropDownItems(i).Name & "' and menu_no=(select top 1 menu_no from sys_menu where menu_id='" & MenuItems.DropDownItems(i).Name & "' and (menu_index='' or menu_index is null) order by menu_no  ) "
                sqlcmd.CommandText = sql
                sqlcmd.ExecuteNonQuery()

                GetMenuItemStrip(MenuItems.DropDownItems(i), k, sqlcmd)
            End If

        Next i

    End Sub



    Private Sub Show_Main_menustrip()
        Dim i As Int16

        For i = 0 To Me.MenuStrip1.Items.Count - 1
            MenuStrip1.Items(i).Visible = True
            '显示其子菜单
            Show_Main_item_menustrip(MenuStrip1.Items(i))

        Next
    End Sub

    Private Sub Show_Main_item_menustrip(ByVal MenuItem As ToolStripMenuItem)
        Dim i As Int16

        MenuItem.Visible = True
        If MenuItem.DropDownItems.Count > 0 Then

            For i = 0 To MenuItem.DropDownItems.Count - 1
                If TypeOf MenuItem.DropDownItems(i) Is ToolStripMenuItem Then
                    Show_Main_item_menustrip(MenuItem.DropDownItems(i))
                Else
                    MenuItem.DropDownItems(i).Visible = True
                End If
            Next


        End If
    End Sub



    Private Sub 重建sysmenuToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        'Dim sqlcon As New SqlConnection
        'Dim sqlcmd As New SqlCommand
        'Dim sqlad As New SqlDataAdapter


        'Dim dt As New DataTable
        'Dim sql As String
        'Dim i as int16

        'Dim Menus As MenuStrip


        'Dim s As String


        'sqlcon = getconn()
        'sqlcmd.Connection = sqlcon

        'sql = "delete from sys_menu"
        'sqlcmd.CommandText = sql
        'sqlcmd.ExecuteNonQuery()

        'sql = "INSERT INTO Sys_Menu ( Father_No,IN_User,Menu_ID,menu_index,Menu_Name,Menu_No) values ( '*', '1', '权限', '0', '菜单', '0')"
        'sqlcmd.CommandText = sql
        'sqlcmd.ExecuteNonQuery()

        'Menus = Me.MenuStrip1

        'For i = 0 To Menus.Items.Count - 1


        '    If i < 10 Then
        '        s = "00" & i
        '    ElseIf i < 100 Then
        '        s = "0" & i
        '    Else
        '        s = i
        '    End If




        '    Debug.Print(Menus.Items(i).Name & "   " & s)
        '    sql = "INSERT INTO Sys_Menu ( Father_No,IN_User,Menu_ID,menu_index,Menu_Name,Menu_No) values " & _
        '    "( '0', '1', '" & Menus.Items(i).Name & "', '" & s & "', '" & Menus.Items(i).Text & "', '" & s & "')"
        '    sqlcmd.CommandText = sql
        '    sqlcmd.ExecuteNonQuery()


        '    GetMenuItems(Menus.Items(i), s)

        'Next

        'sql="update sys_menu set father_no=left(menu_no,len(menu_no)-3) where len(menu_no)>3"
        '    sqlcmd.CommandText = sql
        '    sqlcmd.ExecuteNonQuery()
    End Sub

    Private Sub GetMenuItems(ByVal Menus As ToolStripMenuItem, ByVal menu_no As String)
        'Dim sqlcon As New SqlConnection
        'Dim sqlcmd As New SqlCommand


        'Dim sql As String
        'Dim i as int16

        'Dim s As String
        'Dim s1 As Integer

        'sqlcon = getconn()
        'sqlcmd.Connection = sqlcon



        'For i = 0 To Menus.DropDownItems.Count - 1


        '    If i < 10 Then
        '        s = menu_no & "00" & i
        '    ElseIf i < 100 Then
        '        s = menu_no & "0" & i
        '    Else
        '        s = menu_no & i
        '    End If

        '    If TypeOf Menus.DropDownItems(i) Is ToolStripMenuItem Then
        '        Debug.Print(Menus.DropDownItems(i).Name & "   " & s)

        '        sql = "INSERT INTO Sys_Menu ( Father_No,IN_User,Menu_ID,menu_index,Menu_Name,Menu_No) values " & _
        '            "( '0', '1', '" & Menus.DropDownItems(i).Name & "', '" & s & "', '" & Menus.DropDownItems(i).Text & "', '" & s & "')"
        '        sqlcmd.CommandText = sql
        '        sqlcmd.ExecuteNonQuery()


        '        GetMenuItems(Menus.DropDownItems(i), s)
        '    End If
        'Next

    End Sub


    Private Sub ID_月结日期设定_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ID_月结日期设定.Click
        ShowForm(sys_month)
    End Sub

   
    Private Sub ID_系统角色权限_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ID_系统角色权限.Click
        ShowForm(sys_role_right)
    End Sub

    Private Sub ID_系统用户权限_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ID_系统用户权限.Click
        ShowForm(sys_user_right)
    End Sub


    Private Sub ID_部门资料维护_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ID_部门资料维护.Click
        ShowForm(department)
        department.Refresh_ExexDataGridView1()
    End Sub

    Private Sub ID_员工资料维护_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ID_员工资料维护.Click
        ShowForm(employee)
        employee.Refresh_ExexDataGridView1()
    End Sub

  

    Private Sub ID_更改密码_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ID_更改密码.Click
        ShowForm(newpassword, 1)
    End Sub

    Private Sub ID_注销_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ID_注销.Click
        Dim sqlcon As New SqlConnection
        Dim sqlcmd As New SqlCommand
        Dim sqlad As New SqlDataAdapter
        Dim dt As New DataTable

        Dim sql As String

        Dim Cform As Form
        For Each Cform In Me.MdiChildren
            Cform.Close()
        Next

        UnloadMenuStrip(Me.MenuStrip1)

        Dim loginform As New LoginForm
        loginform.UsernameTextBox.Text = ""
        loginform.PasswordTextBox.Text = ""
        loginform.ShowDialog()

        Me.ToolStripStatusLabel1.Text = login_id
        Me.ToolStripStatusLabel4.Text = rs_name

        sqlcon = getconn()
        sqlcmd.Connection = sqlcon
        If login_id = "admin" Then

            Show_Main_menustrip()
        Else


            sql = "select b.* " & _
                        "from sys_user_right a left join  " & _
                        "	sys_menu b on a.menu_no=b.menu_no " & _
                        "where a.in_user=1 and b.in_user=1 and a.login_id='" & login_id & "' and b.menu_no<>'0' and menu_index is not null " & _
                        "order by b.menu_index "
            sqlcmd.CommandText = sql
            sqlad.SelectCommand = sqlcmd
            dt.Clear()

            sqlad.Fill(dt)


            ShowMenuStrip(dt, Me.MenuStrip1, sqlcon)
        End If
    End Sub

    Private Sub ID_退出_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ID_退出.Click
        Me.Close()
    End Sub

    Private Sub ID_水平平铺_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ID_水平平铺.Click
        Me.LayoutMdi(MdiLayout.TileHorizontal)
    End Sub

    Private Sub ID_垂直平铺_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ID_垂直平铺.Click
        Me.LayoutMdi(MdiLayout.TileVertical)
    End Sub

    Private Sub ID_层叠_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ID_层叠.Click
        Me.LayoutMdi(MdiLayout.Cascade)
    End Sub

    Private Sub ID_排列窗口_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ID_排列窗口.Click
        Me.LayoutMdi(MdiLayout.ArrangeIcons)
    End Sub

   
    Private Sub 重设系统菜单ToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles 重设系统菜单ToolStripMenuItem.Click

        Dim sqlcon As New SqlConnection
        Dim sqlcmd As New SqlCommand
        Dim mytrans As SqlTransaction

        Dim sql As String

        sqlcon = getconn()
        sqlcmd.Connection = sqlcon
        mytrans = sqlcon.BeginTransaction
#If Not Debug Then
                Try
#End If

        sqlcmd.Transaction = mytrans

        GetMenuStrip(Me.MenuStrip1, sqlcmd)

        sql = "delete  sys_menu where menu_index is null"
        sqlcmd.CommandText = sql
        sqlcmd.ExecuteNonQuery()

        sql = "delete sys_role_right where menu_no not in (select menu_no from sys_menu)"
        sqlcmd.CommandText = sql
        sqlcmd.ExecuteNonQuery()

        sql = "delete sys_user_right where menu_no not in (select menu_no from sys_menu)"
        sqlcmd.CommandText = sql
        sqlcmd.ExecuteNonQuery()

        mytrans.Commit()

#If Not Debug Then
                Catch ex As Exception
                    mytrans.Rollback()

                    MsgBox(ex.Message)
                End Try
#End If
        MsgBox("完成")
    End Sub


    Private Sub ID_物料名称设置_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ID_物料名称设置.Click
        Dim Item_add As New Item_add
        ShowForm(Item_add)
        Item_add.Refresh_ExexDataGridView1()
    End Sub

    Private Sub ID_仓库资料_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ID_仓库资料.Click
        Dim deport_info As New deport_info
        ShowForm(deport_info)
        deport_info.Refresh_ExexDataGridView1()
    End Sub

 
    Private Sub ID_工程单建立_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ID_工程单建立.Click
        Dim project As New project
        ShowForm(project)
    End Sub

    Private Sub ID_供应商信息_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ID_供应商信息.Click
        Dim supplier As New supplier
        ShowForm(supplier)
        supplier.Refresh_ExexDataGridView1()
    End Sub

    Private Sub ID_工程单审核_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ID_工程单审核.Click
        Dim project_check As New project_check
        ShowForm(project_check)
    End Sub

    Private Sub ID_工程单查询_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ID_工程单查询.Click
        Dim project_quality As New project_quality
        ShowForm(project_quality)
    End Sub

    Private Sub ID_采购单建立_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ID_采购单建立.Click
        Dim purchase As New purchase
        ShowForm(purchase)
    End Sub

    Private Sub ID_采购单审核_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ID_采购单审核.Click
        Dim purchase_sh As New purchase_sh
        ShowForm(purchase_sh)
    End Sub

    Private Sub ID_采购单查询_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ID_采购单查询.Click
        Dim purchase_quality As New purchase_quality
        ShowForm(purchase_quality)
    End Sub

    Private Sub ID_采购入库_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ID_采购入库.Click
        Dim part_in_CG As New part_in_CG
        ShowForm(part_in_CG)
    End Sub

    Private Sub ID_入库重置_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ID_入库重置.Click
        Dim warehouse_undo As New warehouse_undo
        ShowForm(warehouse_undo)
    End Sub

    Private Sub ID_领料单建立_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ID_领料单建立.Click
        Dim t_part_out As New t_part_out
        ShowForm(t_part_out)
    End Sub

    Private Sub ID_领料单查询_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ID_领料单查询.Click
        Dim t_part_out_quality As New t_part_out_quality
        ShowForm(t_part_out_quality)
    End Sub

    Private Sub 领料出库ToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ID_领料出库.Click
        Dim part_out_LL As New part_out_LL
        ShowForm(part_out_LL)
    End Sub

    Private Sub 出库重置ToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ID_出库重置.Click
        Dim warehouse_undo_CK As New warehouse_undo_CK
        ShowForm(warehouse_undo_CK)
    End Sub

    Private Sub ID_领料单审核_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ID_领料单审核.Click
        Dim t_part_out_check As New t_part_out_check
        ShowForm(t_part_out_check)
    End Sub

    Private Sub ID_库存查询_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ID_库存查询.Click
        Dim warehouse_question As New warehouse_question
        ShowForm(warehouse_question)
    End Sub

    Private Sub ID_仓库综合查询_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ID_仓库综合查询.Click
        Dim CK_ZHCX As New CK_ZHCX
        ShowForm(CK_ZHCX)
    End Sub

   
    Private Sub ID_人员对应仓库_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ID_人员对应仓库.Click
        Dim t_user_depot As New t_user_depot
        ShowForm(t_user_depot)
    End Sub

    Private Sub ID_调帐单建立_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)

    End Sub

    Private Sub ID_调帐单_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ID_调帐单.Click
        Dim tzd As New TZD
        ShowForm(tzd)
    End Sub

    Private Sub ID_调帐记录查询_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ID_调帐记录查询.Click
        Dim t_tzd_cx As New t_tzd_cx
        ShowForm(t_tzd_cx)
    End Sub

    Private Sub ID_工程审批单完工_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ID_工程审批单完工.Click
        Dim project_WG As New project_WG
        ShowForm(project_WG)
    End Sub

    Private Sub ID_报表查询_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ID_报表查询.Click
        Dim report_question As New report_question
        ShowForm(report_question)
    End Sub

    Private Sub ID_应收发票建立_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)

    End Sub

    Private Sub ID_应收发票审核_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)

    End Sub

    Private Sub ID_应收发票查询_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)

    End Sub

    Private Sub ID_应收发票核销_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)

    End Sub

    Private Sub 移库单审核ToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ID_移库单审核.Click
        Dim YKD_check As New YKD_check
        ShowForm(YKD_check)
    End Sub

    Private Sub ID_移库单建立_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ID_移库单建立.Click
        Dim YKD As New YKD
        ShowForm(YKD)
    End Sub

    Private Sub ID_移库单查询_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ID_移库单查询.Click
        Dim YKD_quality As New YKD_quality
        ShowForm(YKD_quality)
    End Sub

    Private Sub ID_移库出库_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ID_移库出库.Click
        Dim yk_out As New yk_out
        ShowForm(yk_out)
    End Sub

    Private Sub ID_移库入库_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ID_移库入库.Click
        Dim YKD_in As New YKD_in
        ShowForm(YKD_in)
    End Sub

    Private Sub ID_应付发票建立_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ID_应付发票建立.Click
        Dim t_FPYF As New t_FPYF
        ShowForm(t_FPYF)
    End Sub

    Private Sub ID_应付发票核销_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ID_应付发票核销.Click
        Dim T_FPYF_HX As New T_FPYF_HX
        ShowForm(T_FPYF_HX)
    End Sub

    Private Sub ID_应付发票查询_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ID_应付发票查询.Click
        Dim T_FPYF_question As New t_FPYF_question
        ShowForm(T_FPYF_question)
    End Sub

    Private Sub ID_应付发票审核_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ID_应付发票审核.Click
        Dim t_FPYF_check As New t_FPYF_check
        ShowForm(t_FPYF_check)
    End Sub

    Private Sub 费用单建立ToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ID_费用单建立.Click
        Dim t_outlay As New t_outlay
        ShowForm(t_outlay)
    End Sub

    Private Sub ID_费用单查询_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ID_费用单查询.Click
        Dim t_outlay_quality As New t_outlay_quality
        ShowForm(t_outlay_quality)
    End Sub

    Private Sub ID_费用单审核_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ID_费用单审核.Click
        Dim t_outlay_check As New t_outlay_check
        ShowForm(t_outlay_check)
    End Sub

  
    Private Sub ID_工程查询_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ID_工程查询.Click
        Dim project_detail_quality As New project_detail_quality
        ShowForm(project_detail_quality)
    End Sub

    Private Sub ID_供货明细查询_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ID_供货明细查询.Click
        Dim T_GHMX_quality As New T_GHMX_quality
        ShowForm(T_GHMX_quality)
    End Sub

    Private Sub ID_退库单建立_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ID_退库单建立.Click
        Dim t_part_in As New t_part_in
        ShowForm(t_part_in)
    End Sub

    Private Sub ID_退库单查询_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ID_退库单查询.Click
        Dim t_part_in_quality As New t_part_in_quality
        ShowForm(t_part_in_quality)
    End Sub

    Private Sub ID_退库单审核_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ID_退库单审核.Click
        Dim t_part_in_check As New t_part_in_check
        ShowForm(t_part_in_check)
    End Sub

    Private Sub ID_退料入库_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ID_退料入库.Click
        Dim t_part_in_TL As New t_part_in_TL
        ShowForm(t_part_in_TL)
    End Sub

    Private Sub ID_加工费建立_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ID_加工费建立.Click
        Dim t_outwork As New t_outwork
        ShowForm(t_outwork)
    End Sub

    Private Sub ID_加工费审核_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ID_加工费审核.Click
        Dim t_outwork_check As New t_outwork_check
        ShowForm(t_outwork_check)
    End Sub

    Private Sub ID_加工费查询_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ID_加工费查询.Click
        Dim t_outwork_quality As New t_outwork_quality
        ShowForm(t_outwork_quality)
    End Sub

    Private Sub ID_应收发票核销_Click_1(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ID_应收发票核销.Click
        Dim t_FPYS_HX As New t_FPYS_HX
        ShowForm(t_FPYS_HX)
    End Sub

    Private Sub ID_应收发票查询_Click_1(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ID_应收发票查询.Click
        Dim t_FPYS_question As New t_FPYS_question
        ShowForm(t_FPYS_question)
    End Sub

    Private Sub ID_应收发票审核_Click_1(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ID_应收发票审核.Click
        Dim t_FPYS_check As New t_FPYS_check
        ShowForm(t_FPYS_check)
    End Sub

  
    Private Sub ID_应收发票建立_Click_1(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ID_应收发票建立.Click
        Dim T_FPYS As New T_FPYS
        ShowForm(T_FPYS)
    End Sub

    Private Sub ID_应收金额建立_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ID_应收金额建立.Click
        Dim T_JEYS As New T_JEYS
        ShowForm(T_JEYS)
    End Sub

    Private Sub ID_应收金额审核_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ID_应收金额审核.Click
        Dim T_jeys_check As New T_jeys_check
        ShowForm(T_jeys_check)
    End Sub

    Private Sub ID_应收金额查询_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ID_应收金额查询.Click
        Dim t_JEYS_question As New t_JEYS_question
        ShowForm(t_JEYS_question)
    End Sub

    Private Sub ID_应收金额核销_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ID_应收金额核销.Click
        Dim t_jeys_HX As New t_jeys_HX
        ShowForm(t_jeys_HX)
    End Sub

    Private Sub ID_客户信息_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ID_客户信息.Click
        Dim customer As New customer
        ShowForm(customer)
        customer.Refresh_ExexDataGridView1()
    End Sub

    Private Sub ID_加工费打印重置_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ID_加工费打印重置.Click
        Dim t_outwork_print_resetting As New t_outwork_print_resetting
        ShowForm(t_outwork_print_resetting)
    End Sub

    Private Sub ID_费用单打印重置_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ID_费用单打印重置.Click
        Dim t_outlay_print_resetting As New t_outlay_print_resetting
        ShowForm(t_outlay_print_resetting)
    End Sub

    Private Sub ID_供货商付款情况查询_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ID_供货商付款情况查询.Click
        Dim t_GHFK_quality As New T_GHFK_quality
        ShowForm(t_GHFK_quality)
    End Sub

    Private Sub ID_工程账务信息查询_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ID_工程账务信息查询.Click
        Dim project_account_quility As New project_account_quility
        ShowForm(project_account_quility)
    End Sub

    Private Sub 产值查询ToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ID_产值查询.Click
        Dim project_CZ_quality As New project_CZ_quality
        ShowForm(project_CZ_quality)
    End Sub

    Private Sub ID_收款情况查询_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ID_收款情况查询.Click
        Dim T_JEYS_query As New T_JEYS_query
        ShowForm(T_JEYS_query)
    End Sub

    Private Sub ID_采购单结案_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ID_采购单结案.Click
        Dim purchase_JA As New purchase_JA
        ShowForm(purchase_JA)
    End Sub

    Private Sub ID_工厂资料_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ID_工厂资料.Click
        Dim factory_info As New factory_info
        ShowForm(factory_info)
    End Sub

    Private Sub ID_人员对应工厂_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ID_人员对应工厂.Click
        Dim t_user_factory As New t_user_factory
        ShowForm(t_user_factory)
    End Sub

    Private Sub ID_由领料单生成移库单_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ID_由领料单生成移库单.Click
        Dim ll_ykd As New LL_YKD
        ShowForm(ll_ykd)
    End Sub

    Private Sub ID_加工费明细查询_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ID_加工费明细查询.Click
        Dim JGF_detail_question As New JGF_detail_question
        ShowForm(JGF_detail_question)
    End Sub

    Private Sub ID_供应商付款详情查询_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ID_供应商付款详情查询.Click
        Dim T_GHFK_detail_quality As New T_GHFK_detail_quality
        ShowForm(T_GHFK_detail_quality)
    End Sub

    Private Sub 库存每月结转查询ToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ID_库存每月结转查询.Click
        Dim T_KCMYJZ_question As New T_KCMYJZ_question
        ShowForm(T_KCMYJZ_question)
    End Sub

    Private Sub ID_费用单明细查询_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ID_费用单明细查询.Click
        Dim FYD_detail_question As New FYD_detail_question
        ShowForm(FYD_detail_question)
    End Sub

    Private Sub ID_物料查询供应商_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ID_物料查询供应商.Click
        Dim GoodsQuesonGYS As New GoodsQuesonGYS
        ShowForm(GoodsQuesonGYS)
    End Sub

    Private Sub ID_采购申请单建立_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ID_采购申请单建立.Click
        Dim CGSQ As New CGSQ
        ShowForm(CGSQ)
    End Sub

    Private Sub ID_采购申请单审核_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ID_采购申请单审核.Click
        Dim CGSQ_SH As New CGSQ_SH
        ShowForm(CGSQ_SH)
    End Sub

    Private Sub ID_采购申请单查询_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ID_采购申请单查询.Click
        Dim CGSQ_quality As New CGSQ_quality
        ShowForm(CGSQ_quality)
    End Sub

    Private Sub ID_采购申请单转加工单_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ID_采购申请单转加工单.Click
        Dim CGSQ_JGD As New CGSQ_JGD
        ShowForm(CGSQ_JGD)
    End Sub

    Private Sub ID_考勤录入_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ID_考勤录入.Click
        Dim KQ As New KQ
        ShowForm(KQ)
    End Sub

    Private Sub ID_考勤审核_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ID_考勤审核.Click
        Dim KQ_check As New KQ_check
        ShowForm(KQ_check)
    End Sub

    Private Sub ID_考勤查询_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ID_考勤查询.Click
        Dim KQ_quality As New KQ_quality
        ShowForm(KQ_quality)
    End Sub

    Private Sub ID_下单建立_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ID_下单建立.Click
        Dim XDJL As New XDJL
        ShowForm(XDJL)
    End Sub

    Private Sub ID_下单审核_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ID_下单审核.Click
        Dim XDSH As New XDSH
        ShowForm(XDSH)
    End Sub

    Private Sub ID_下单查询_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ID_下单查询.Click
        Dim XDCX As New XDCX
        ShowForm(XDCX)
    End Sub

    Private Sub ID_接单确认_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ID_接单确认.Click
        Dim JDQR As New JDQR
        ShowForm(JDQR)
    End Sub

    Private Sub ID_接单价格确认_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ID_接单价格确认.Click
        Dim JDJGQR As New JDJGQR
        ShowForm(JDJGQR)
    End Sub

    Private Sub ID_接单完成_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ID_接单完成.Click
        Dim JDWC As New JDWC
        ShowForm(JDWC)
    End Sub

    Private Sub ID_确认完成_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ID_确认完成.Click
        Dim QRWC As New QRWC
        ShowForm(QRWC)
    End Sub

    Private Sub ID_行政费用单建立_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ID_行政费用单建立.Click
        Dim XZFYD As New XZFYD
        ShowForm(XZFYD)
    End Sub

    Private Sub ID_行政费用单审核_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ID_行政费用单审核.Click
        Dim XZFY_check As New XZFY_check
        ShowForm(XZFY_check)
    End Sub

    Private Sub ID_行政费用单查询_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ID_行政费用单查询.Click
        Dim XZFY_CX As New XZFY_CX
        ShowForm(XZFY_CX)
    End Sub

    Private Sub 临时菜单ToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles 临时菜单ToolStripMenuItem.Click
        Print_in("RK-GS-201211-0052", False)
    End Sub
End Class
