﻿<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class XDJL
    Inherits System.Windows.Forms.Form

    'Form 重写 Dispose，以清理组件列表。
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        If disposing AndAlso components IsNot Nothing Then
            components.Dispose()
        End If
        MyBase.Dispose(disposing)
    End Sub

    'Windows 窗体设计器所必需的
    Private components As System.ComponentModel.IContainer

    '注意: 以下过程是 Windows 窗体设计器所必需的
    '可以使用 Windows 窗体设计器修改它。
    '不要使用代码编辑器修改它。
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(XDJL))
        Me.CheckBox4 = New System.Windows.Forms.CheckBox
        Me.SplitContainer1 = New System.Windows.Forms.SplitContainer
        Me.CheckBox6 = New System.Windows.Forms.CheckBox
        Me.CheckBox5 = New System.Windows.Forms.CheckBox
        Me.CheckBox3 = New System.Windows.Forms.CheckBox
        Me.CheckBox2 = New System.Windows.Forms.CheckBox
        Me.CheckBox1 = New System.Windows.Forms.CheckBox
        Me.ExDataGridView1 = New BaseClass.ExDataGridView
        Me.ToolStrip1 = New System.Windows.Forms.ToolStrip
        Me.ListView1 = New System.Windows.Forms.ListView
        Me.ContextMenuStrip1 = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.增加文件ToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.删除文件ToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.打开文件ToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.ImageList1 = New System.Windows.Forms.ImageList(Me.components)
        Me.OpenFileDialog1 = New System.Windows.Forms.OpenFileDialog
        Me.ToolStripButton_refresh = New System.Windows.Forms.ToolStripButton
        Me.ToolStripButton_add = New System.Windows.Forms.ToolStripButton
        Me.ToolStripButton_mod = New System.Windows.Forms.ToolStripButton
        Me.ToolStripButton_del = New System.Windows.Forms.ToolStripButton
        Me.ToolStripButton_exit = New System.Windows.Forms.ToolStripButton
        Me.ToolStripButton_question = New System.Windows.Forms.ToolStripButton
        Me.ToolStripButton1 = New System.Windows.Forms.ToolStripButton
        Me.ToolStripButton2 = New System.Windows.Forms.ToolStripButton
        Me.CheckBox7 = New System.Windows.Forms.CheckBox
        Me.SplitContainer1.Panel1.SuspendLayout()
        Me.SplitContainer1.Panel2.SuspendLayout()
        Me.SplitContainer1.SuspendLayout()
        CType(Me.ExDataGridView1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.ToolStrip1.SuspendLayout()
        Me.ContextMenuStrip1.SuspendLayout()
        Me.SuspendLayout()
        '
        'CheckBox4
        '
        Me.CheckBox4.AutoSize = True
        Me.CheckBox4.BackColor = System.Drawing.SystemColors.Control
        Me.CheckBox4.Location = New System.Drawing.Point(535, 10)
        Me.CheckBox4.Name = "CheckBox4"
        Me.CheckBox4.Size = New System.Drawing.Size(60, 16)
        Me.CheckBox4.TabIndex = 26
        Me.CheckBox4.Text = "未接单"
        Me.CheckBox4.UseVisualStyleBackColor = False
        '
        'SplitContainer1
        '
        Me.SplitContainer1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.SplitContainer1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.SplitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel1
        Me.SplitContainer1.Location = New System.Drawing.Point(0, 0)
        Me.SplitContainer1.Name = "SplitContainer1"
        Me.SplitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal
        '
        'SplitContainer1.Panel1
        '
        Me.SplitContainer1.Panel1.Controls.Add(Me.CheckBox7)
        Me.SplitContainer1.Panel1.Controls.Add(Me.CheckBox6)
        Me.SplitContainer1.Panel1.Controls.Add(Me.CheckBox5)
        Me.SplitContainer1.Panel1.Controls.Add(Me.CheckBox4)
        Me.SplitContainer1.Panel1.Controls.Add(Me.CheckBox3)
        Me.SplitContainer1.Panel1.Controls.Add(Me.CheckBox2)
        Me.SplitContainer1.Panel1.Controls.Add(Me.CheckBox1)
        Me.SplitContainer1.Panel1.Controls.Add(Me.ExDataGridView1)
        Me.SplitContainer1.Panel1.Controls.Add(Me.ToolStrip1)
        '
        'SplitContainer1.Panel2
        '
        Me.SplitContainer1.Panel2.Controls.Add(Me.ListView1)
        Me.SplitContainer1.Size = New System.Drawing.Size(1028, 593)
        Me.SplitContainer1.SplitterDistance = 243
        Me.SplitContainer1.SplitterWidth = 1
        Me.SplitContainer1.TabIndex = 6
        '
        'CheckBox6
        '
        Me.CheckBox6.AutoSize = True
        Me.CheckBox6.BackColor = System.Drawing.SystemColors.Control
        Me.CheckBox6.Location = New System.Drawing.Point(667, 10)
        Me.CheckBox6.Name = "CheckBox6"
        Me.CheckBox6.Size = New System.Drawing.Size(60, 16)
        Me.CheckBox6.TabIndex = 28
        Me.CheckBox6.Text = "已完成"
        Me.CheckBox6.UseVisualStyleBackColor = False
        '
        'CheckBox5
        '
        Me.CheckBox5.AutoSize = True
        Me.CheckBox5.BackColor = System.Drawing.SystemColors.Control
        Me.CheckBox5.Location = New System.Drawing.Point(601, 10)
        Me.CheckBox5.Name = "CheckBox5"
        Me.CheckBox5.Size = New System.Drawing.Size(60, 16)
        Me.CheckBox5.TabIndex = 27
        Me.CheckBox5.Text = "已接单"
        Me.CheckBox5.UseVisualStyleBackColor = False
        '
        'CheckBox3
        '
        Me.CheckBox3.AutoSize = True
        Me.CheckBox3.BackColor = System.Drawing.SystemColors.Control
        Me.CheckBox3.Location = New System.Drawing.Point(431, 10)
        Me.CheckBox3.Name = "CheckBox3"
        Me.CheckBox3.Size = New System.Drawing.Size(48, 16)
        Me.CheckBox3.TabIndex = 25
        Me.CheckBox3.Text = "结案"
        Me.CheckBox3.UseVisualStyleBackColor = False
        '
        'CheckBox2
        '
        Me.CheckBox2.AutoSize = True
        Me.CheckBox2.BackColor = System.Drawing.SystemColors.Control
        Me.CheckBox2.Location = New System.Drawing.Point(377, 10)
        Me.CheckBox2.Name = "CheckBox2"
        Me.CheckBox2.Size = New System.Drawing.Size(48, 16)
        Me.CheckBox2.TabIndex = 24
        Me.CheckBox2.Text = "审核"
        Me.CheckBox2.UseVisualStyleBackColor = False
        '
        'CheckBox1
        '
        Me.CheckBox1.AutoSize = True
        Me.CheckBox1.BackColor = System.Drawing.SystemColors.Control
        Me.CheckBox1.Checked = True
        Me.CheckBox1.CheckState = System.Windows.Forms.CheckState.Checked
        Me.CheckBox1.Location = New System.Drawing.Point(323, 10)
        Me.CheckBox1.Name = "CheckBox1"
        Me.CheckBox1.Size = New System.Drawing.Size(48, 16)
        Me.CheckBox1.TabIndex = 23
        Me.CheckBox1.Text = "新建"
        Me.CheckBox1.UseVisualStyleBackColor = False
        '
        'ExDataGridView1
        '
        Me.ExDataGridView1.AllowUserToAddRows = False
        Me.ExDataGridView1.AllowUserToDeleteRows = False
        Me.ExDataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.ExDataGridView1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.ExDataGridView1.Location = New System.Drawing.Point(0, 35)
        Me.ExDataGridView1.MultiSelect = False
        Me.ExDataGridView1.Name = "ExDataGridView1"
        Me.ExDataGridView1.RowTemplate.Height = 23
        Me.ExDataGridView1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect
        Me.ExDataGridView1.Size = New System.Drawing.Size(1024, 204)
        Me.ExDataGridView1.TabIndex = 7
        '
        'ToolStrip1
        '
        Me.ToolStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ToolStripButton_refresh, Me.ToolStripButton_add, Me.ToolStripButton_mod, Me.ToolStripButton_del, Me.ToolStripButton_exit, Me.ToolStripButton_question, Me.ToolStripButton1, Me.ToolStripButton2})
        Me.ToolStrip1.Location = New System.Drawing.Point(0, 0)
        Me.ToolStrip1.Name = "ToolStrip1"
        Me.ToolStrip1.Size = New System.Drawing.Size(1024, 35)
        Me.ToolStrip1.TabIndex = 6
        Me.ToolStrip1.Text = "ToolStrip1"
        '
        'ListView1
        '
        Me.ListView1.ContextMenuStrip = Me.ContextMenuStrip1
        Me.ListView1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.ListView1.LargeImageList = Me.ImageList1
        Me.ListView1.Location = New System.Drawing.Point(0, 0)
        Me.ListView1.Name = "ListView1"
        Me.ListView1.Size = New System.Drawing.Size(1024, 345)
        Me.ListView1.SmallImageList = Me.ImageList1
        Me.ListView1.StateImageList = Me.ImageList1
        Me.ListView1.TabIndex = 0
        Me.ListView1.UseCompatibleStateImageBehavior = False
        '
        'ContextMenuStrip1
        '
        Me.ContextMenuStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.增加文件ToolStripMenuItem, Me.删除文件ToolStripMenuItem, Me.打开文件ToolStripMenuItem})
        Me.ContextMenuStrip1.Name = "ContextMenuStrip1"
        Me.ContextMenuStrip1.Size = New System.Drawing.Size(119, 70)
        '
        '增加文件ToolStripMenuItem
        '
        Me.增加文件ToolStripMenuItem.Name = "增加文件ToolStripMenuItem"
        Me.增加文件ToolStripMenuItem.Size = New System.Drawing.Size(118, 22)
        Me.增加文件ToolStripMenuItem.Text = "增加文件"
        '
        '删除文件ToolStripMenuItem
        '
        Me.删除文件ToolStripMenuItem.Name = "删除文件ToolStripMenuItem"
        Me.删除文件ToolStripMenuItem.Size = New System.Drawing.Size(118, 22)
        Me.删除文件ToolStripMenuItem.Text = "删除文件"
        '
        '打开文件ToolStripMenuItem
        '
        Me.打开文件ToolStripMenuItem.Name = "打开文件ToolStripMenuItem"
        Me.打开文件ToolStripMenuItem.Size = New System.Drawing.Size(118, 22)
        Me.打开文件ToolStripMenuItem.Text = "打开文件"
        '
        'ImageList1
        '
        Me.ImageList1.ImageStream = CType(resources.GetObject("ImageList1.ImageStream"), System.Windows.Forms.ImageListStreamer)
        Me.ImageList1.TransparentColor = System.Drawing.Color.Transparent
        Me.ImageList1.Images.SetKeyName(0, "DOC.BMP")
        '
        'OpenFileDialog1
        '
        Me.OpenFileDialog1.FileName = "OpenFileDialog1"
        Me.OpenFileDialog1.Multiselect = True
        '
        'ToolStripButton_refresh
        '
        Me.ToolStripButton_refresh.Image = CType(resources.GetObject("ToolStripButton_refresh.Image"), System.Drawing.Image)
        Me.ToolStripButton_refresh.ImageTransparentColor = System.Drawing.Color.Black
        Me.ToolStripButton_refresh.Name = "ToolStripButton_refresh"
        Me.ToolStripButton_refresh.Size = New System.Drawing.Size(33, 32)
        Me.ToolStripButton_refresh.Text = "刷新"
        Me.ToolStripButton_refresh.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        '
        'ToolStripButton_add
        '
        Me.ToolStripButton_add.Image = CType(resources.GetObject("ToolStripButton_add.Image"), System.Drawing.Image)
        Me.ToolStripButton_add.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.ToolStripButton_add.Name = "ToolStripButton_add"
        Me.ToolStripButton_add.Size = New System.Drawing.Size(33, 32)
        Me.ToolStripButton_add.Text = "新建"
        Me.ToolStripButton_add.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        '
        'ToolStripButton_mod
        '
        Me.ToolStripButton_mod.Image = CType(resources.GetObject("ToolStripButton_mod.Image"), System.Drawing.Image)
        Me.ToolStripButton_mod.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.ToolStripButton_mod.Name = "ToolStripButton_mod"
        Me.ToolStripButton_mod.Size = New System.Drawing.Size(33, 32)
        Me.ToolStripButton_mod.Text = "修改"
        Me.ToolStripButton_mod.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        '
        'ToolStripButton_del
        '
        Me.ToolStripButton_del.Image = CType(resources.GetObject("ToolStripButton_del.Image"), System.Drawing.Image)
        Me.ToolStripButton_del.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.ToolStripButton_del.Name = "ToolStripButton_del"
        Me.ToolStripButton_del.Size = New System.Drawing.Size(33, 32)
        Me.ToolStripButton_del.Text = "删除"
        Me.ToolStripButton_del.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        '
        'ToolStripButton_exit
        '
        Me.ToolStripButton_exit.Image = CType(resources.GetObject("ToolStripButton_exit.Image"), System.Drawing.Image)
        Me.ToolStripButton_exit.ImageTransparentColor = System.Drawing.Color.Black
        Me.ToolStripButton_exit.Name = "ToolStripButton_exit"
        Me.ToolStripButton_exit.Size = New System.Drawing.Size(33, 32)
        Me.ToolStripButton_exit.Text = "退出"
        Me.ToolStripButton_exit.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        '
        'ToolStripButton_question
        '
        Me.ToolStripButton_question.Image = CType(resources.GetObject("ToolStripButton_question.Image"), System.Drawing.Image)
        Me.ToolStripButton_question.ImageTransparentColor = System.Drawing.Color.Black
        Me.ToolStripButton_question.Name = "ToolStripButton_question"
        Me.ToolStripButton_question.Size = New System.Drawing.Size(33, 32)
        Me.ToolStripButton_question.Text = "查询"
        Me.ToolStripButton_question.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.ToolStripButton_question.Visible = False
        '
        'ToolStripButton1
        '
        Me.ToolStripButton1.Image = CType(resources.GetObject("ToolStripButton1.Image"), System.Drawing.Image)
        Me.ToolStripButton1.ImageTransparentColor = System.Drawing.Color.Black
        Me.ToolStripButton1.Name = "ToolStripButton1"
        Me.ToolStripButton1.Size = New System.Drawing.Size(57, 32)
        Me.ToolStripButton1.Text = "打印预览"
        Me.ToolStripButton1.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.ToolStripButton1.Visible = False
        '
        'ToolStripButton2
        '
        Me.ToolStripButton2.Image = CType(resources.GetObject("ToolStripButton2.Image"), System.Drawing.Image)
        Me.ToolStripButton2.ImageTransparentColor = System.Drawing.Color.Black
        Me.ToolStripButton2.Name = "ToolStripButton2"
        Me.ToolStripButton2.Size = New System.Drawing.Size(33, 32)
        Me.ToolStripButton2.Text = "打印"
        Me.ToolStripButton2.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.ToolStripButton2.Visible = False
        '
        'CheckBox7
        '
        Me.CheckBox7.AutoSize = True
        Me.CheckBox7.BackColor = System.Drawing.SystemColors.Control
        Me.CheckBox7.Location = New System.Drawing.Point(733, 10)
        Me.CheckBox7.Name = "CheckBox7"
        Me.CheckBox7.Size = New System.Drawing.Size(72, 16)
        Me.CheckBox7.TabIndex = 30
        Me.CheckBox7.Text = "确认完成"
        Me.CheckBox7.UseVisualStyleBackColor = False
        '
        'XDJL
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 12.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1028, 593)
        Me.Controls.Add(Me.SplitContainer1)
        Me.Name = "XDJL"
        Me.Text = "下单建立"
        Me.SplitContainer1.Panel1.ResumeLayout(False)
        Me.SplitContainer1.Panel1.PerformLayout()
        Me.SplitContainer1.Panel2.ResumeLayout(False)
        Me.SplitContainer1.ResumeLayout(False)
        CType(Me.ExDataGridView1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ToolStrip1.ResumeLayout(False)
        Me.ToolStrip1.PerformLayout()
        Me.ContextMenuStrip1.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents ToolStripButton_exit As System.Windows.Forms.ToolStripButton
    Friend WithEvents ToolStripButton_question As System.Windows.Forms.ToolStripButton
    Friend WithEvents ToolStripButton_mod As System.Windows.Forms.ToolStripButton
    Friend WithEvents ToolStripButton_del As System.Windows.Forms.ToolStripButton
    Friend WithEvents ToolStripButton2 As System.Windows.Forms.ToolStripButton
    Friend WithEvents ToolStripButton1 As System.Windows.Forms.ToolStripButton
    Friend WithEvents CheckBox4 As System.Windows.Forms.CheckBox
    Friend WithEvents ToolStripButton_add As System.Windows.Forms.ToolStripButton
    Friend WithEvents SplitContainer1 As System.Windows.Forms.SplitContainer
    Friend WithEvents CheckBox3 As System.Windows.Forms.CheckBox
    Friend WithEvents CheckBox2 As System.Windows.Forms.CheckBox
    Friend WithEvents CheckBox1 As System.Windows.Forms.CheckBox
    Friend WithEvents ExDataGridView1 As BaseClass.ExDataGridView
    Friend WithEvents ToolStrip1 As System.Windows.Forms.ToolStrip
    Friend WithEvents ToolStripButton_refresh As System.Windows.Forms.ToolStripButton
    Friend WithEvents ListView1 As System.Windows.Forms.ListView
    Friend WithEvents ImageList1 As System.Windows.Forms.ImageList
    Friend WithEvents ContextMenuStrip1 As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents 增加文件ToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents 删除文件ToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents OpenFileDialog1 As System.Windows.Forms.OpenFileDialog
    Friend WithEvents CheckBox6 As System.Windows.Forms.CheckBox
    Friend WithEvents CheckBox5 As System.Windows.Forms.CheckBox
    Friend WithEvents 打开文件ToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents CheckBox7 As System.Windows.Forms.CheckBox
End Class
