﻿<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class project_check
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(project_check))
        Me.CheckBox2 = New System.Windows.Forms.CheckBox
        Me.ContextMenuStrip1 = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.审核ToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.反审ToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.ToolStripMenuItem1 = New System.Windows.Forms.ToolStripSeparator
        Me.结案ToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.反结案ToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.CheckBox4 = New System.Windows.Forms.CheckBox
        Me.CheckBox3 = New System.Windows.Forms.CheckBox
        Me.CheckBox1 = New System.Windows.Forms.CheckBox
        Me.ToolStrip1 = New System.Windows.Forms.ToolStrip
        Me.TextBox1 = New System.Windows.Forms.TextBox
        Me.Label1 = New System.Windows.Forms.Label
        Me.SplitContainer1 = New System.Windows.Forms.SplitContainer
        Me.ExDataGridView1 = New BaseClass.ExDataGridView
        Me.ExDataGridView2 = New BaseClass.ExDataGridView
        Me.延期ToolStripMenuItem = New System.Windows.Forms.ToolStripSeparator
        Me.延期ToolStripMenuItem1 = New System.Windows.Forms.ToolStripMenuItem
        Me.ToolStripButton_refresh = New System.Windows.Forms.ToolStripButton
        Me.ToolStripButton_question = New System.Windows.Forms.ToolStripButton
        Me.ToolStripButton_show = New System.Windows.Forms.ToolStripButton
        Me.ToolStripButton_print = New System.Windows.Forms.ToolStripButton
        Me.ToolStripButton_exit = New System.Windows.Forms.ToolStripButton
        Me.ContextMenuStrip1.SuspendLayout()
        Me.ToolStrip1.SuspendLayout()
        Me.SplitContainer1.Panel1.SuspendLayout()
        Me.SplitContainer1.Panel2.SuspendLayout()
        Me.SplitContainer1.SuspendLayout()
        CType(Me.ExDataGridView1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.ExDataGridView2, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'CheckBox2
        '
        Me.CheckBox2.AutoSize = True
        Me.CheckBox2.BackColor = System.Drawing.SystemColors.Control
        Me.CheckBox2.Location = New System.Drawing.Point(432, 14)
        Me.CheckBox2.Name = "CheckBox2"
        Me.CheckBox2.Size = New System.Drawing.Size(48, 16)
        Me.CheckBox2.TabIndex = 31
        Me.CheckBox2.Text = "审核"
        Me.CheckBox2.UseVisualStyleBackColor = False
        '
        'ContextMenuStrip1
        '
        Me.ContextMenuStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.审核ToolStripMenuItem, Me.反审ToolStripMenuItem, Me.ToolStripMenuItem1, Me.结案ToolStripMenuItem, Me.反结案ToolStripMenuItem, Me.延期ToolStripMenuItem, Me.延期ToolStripMenuItem1})
        Me.ContextMenuStrip1.Name = "ContextMenuStrip1"
        Me.ContextMenuStrip1.Size = New System.Drawing.Size(153, 148)
        '
        '审核ToolStripMenuItem
        '
        Me.审核ToolStripMenuItem.Name = "审核ToolStripMenuItem"
        Me.审核ToolStripMenuItem.Size = New System.Drawing.Size(152, 22)
        Me.审核ToolStripMenuItem.Text = "审核"
        '
        '反审ToolStripMenuItem
        '
        Me.反审ToolStripMenuItem.Name = "反审ToolStripMenuItem"
        Me.反审ToolStripMenuItem.Size = New System.Drawing.Size(152, 22)
        Me.反审ToolStripMenuItem.Text = "反审"
        '
        'ToolStripMenuItem1
        '
        Me.ToolStripMenuItem1.Name = "ToolStripMenuItem1"
        Me.ToolStripMenuItem1.Size = New System.Drawing.Size(149, 6)
        '
        '结案ToolStripMenuItem
        '
        Me.结案ToolStripMenuItem.Name = "结案ToolStripMenuItem"
        Me.结案ToolStripMenuItem.Size = New System.Drawing.Size(152, 22)
        Me.结案ToolStripMenuItem.Text = "结案"
        '
        '反结案ToolStripMenuItem
        '
        Me.反结案ToolStripMenuItem.Name = "反结案ToolStripMenuItem"
        Me.反结案ToolStripMenuItem.Size = New System.Drawing.Size(152, 22)
        Me.反结案ToolStripMenuItem.Text = "反结案"
        '
        'CheckBox4
        '
        Me.CheckBox4.AutoSize = True
        Me.CheckBox4.BackColor = System.Drawing.SystemColors.Control
        Me.CheckBox4.Location = New System.Drawing.Point(540, 14)
        Me.CheckBox4.Name = "CheckBox4"
        Me.CheckBox4.Size = New System.Drawing.Size(48, 16)
        Me.CheckBox4.TabIndex = 34
        Me.CheckBox4.Text = "有效"
        Me.CheckBox4.UseVisualStyleBackColor = False
        '
        'CheckBox3
        '
        Me.CheckBox3.AutoSize = True
        Me.CheckBox3.BackColor = System.Drawing.SystemColors.Control
        Me.CheckBox3.Location = New System.Drawing.Point(486, 14)
        Me.CheckBox3.Name = "CheckBox3"
        Me.CheckBox3.Size = New System.Drawing.Size(48, 16)
        Me.CheckBox3.TabIndex = 32
        Me.CheckBox3.Text = "结案"
        Me.CheckBox3.UseVisualStyleBackColor = False
        '
        'CheckBox1
        '
        Me.CheckBox1.AutoSize = True
        Me.CheckBox1.BackColor = System.Drawing.SystemColors.Control
        Me.CheckBox1.Checked = True
        Me.CheckBox1.CheckState = System.Windows.Forms.CheckState.Checked
        Me.CheckBox1.Location = New System.Drawing.Point(378, 14)
        Me.CheckBox1.Name = "CheckBox1"
        Me.CheckBox1.Size = New System.Drawing.Size(48, 16)
        Me.CheckBox1.TabIndex = 30
        Me.CheckBox1.Text = "新建"
        Me.CheckBox1.UseVisualStyleBackColor = False
        '
        'ToolStrip1
        '
        Me.ToolStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ToolStripButton_refresh, Me.ToolStripButton_question, Me.ToolStripButton_show, Me.ToolStripButton_print, Me.ToolStripButton_exit})
        Me.ToolStrip1.Location = New System.Drawing.Point(0, 0)
        Me.ToolStrip1.Name = "ToolStrip1"
        Me.ToolStrip1.Size = New System.Drawing.Size(799, 40)
        Me.ToolStrip1.TabIndex = 29
        Me.ToolStrip1.Text = "ToolStrip1"
        '
        'TextBox1
        '
        Me.TextBox1.Location = New System.Drawing.Point(272, 12)
        Me.TextBox1.Name = "TextBox1"
        Me.TextBox1.Size = New System.Drawing.Size(100, 21)
        Me.TextBox1.TabIndex = 36
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(225, 17)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(41, 12)
        Me.Label1.TabIndex = 35
        Me.Label1.Text = "工程号"
        '
        'SplitContainer1
        '
        Me.SplitContainer1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.SplitContainer1.Location = New System.Drawing.Point(0, 40)
        Me.SplitContainer1.Name = "SplitContainer1"
        Me.SplitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal
        '
        'SplitContainer1.Panel1
        '
        Me.SplitContainer1.Panel1.Controls.Add(Me.ExDataGridView1)
        '
        'SplitContainer1.Panel2
        '
        Me.SplitContainer1.Panel2.Controls.Add(Me.ExDataGridView2)
        Me.SplitContainer1.Size = New System.Drawing.Size(799, 422)
        Me.SplitContainer1.SplitterDistance = 211
        Me.SplitContainer1.TabIndex = 37
        '
        'ExDataGridView1
        '
        Me.ExDataGridView1.AllowUserToAddRows = False
        Me.ExDataGridView1.AllowUserToDeleteRows = False
        Me.ExDataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.ExDataGridView1.ContextMenuStrip = Me.ContextMenuStrip1
        Me.ExDataGridView1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.ExDataGridView1.Location = New System.Drawing.Point(0, 0)
        Me.ExDataGridView1.MultiSelect = False
        Me.ExDataGridView1.Name = "ExDataGridView1"
        Me.ExDataGridView1.ReadOnly = True
        Me.ExDataGridView1.RowTemplate.Height = 23
        Me.ExDataGridView1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect
        Me.ExDataGridView1.Size = New System.Drawing.Size(799, 211)
        Me.ExDataGridView1.TabIndex = 2
        '
        'ExDataGridView2
        '
        Me.ExDataGridView2.AllowUserToAddRows = False
        Me.ExDataGridView2.AllowUserToDeleteRows = False
        Me.ExDataGridView2.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.ExDataGridView2.Dock = System.Windows.Forms.DockStyle.Fill
        Me.ExDataGridView2.Location = New System.Drawing.Point(0, 0)
        Me.ExDataGridView2.MultiSelect = False
        Me.ExDataGridView2.Name = "ExDataGridView2"
        Me.ExDataGridView2.ReadOnly = True
        Me.ExDataGridView2.RowTemplate.Height = 23
        Me.ExDataGridView2.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect
        Me.ExDataGridView2.Size = New System.Drawing.Size(799, 207)
        Me.ExDataGridView2.TabIndex = 3
        '
        '延期ToolStripMenuItem
        '
        Me.延期ToolStripMenuItem.Name = "延期ToolStripMenuItem"
        Me.延期ToolStripMenuItem.Size = New System.Drawing.Size(149, 6)
        '
        '延期ToolStripMenuItem1
        '
        Me.延期ToolStripMenuItem1.Name = "延期ToolStripMenuItem1"
        Me.延期ToolStripMenuItem1.Size = New System.Drawing.Size(152, 22)
        Me.延期ToolStripMenuItem1.Text = "延期"
        '
        'ToolStripButton_refresh
        '
        Me.ToolStripButton_refresh.Image = CType(resources.GetObject("ToolStripButton_refresh.Image"), System.Drawing.Image)
        Me.ToolStripButton_refresh.ImageTransparentColor = System.Drawing.Color.Black
        Me.ToolStripButton_refresh.Name = "ToolStripButton_refresh"
        Me.ToolStripButton_refresh.Size = New System.Drawing.Size(36, 37)
        Me.ToolStripButton_refresh.Text = "刷新"
        Me.ToolStripButton_refresh.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        '
        'ToolStripButton_question
        '
        Me.ToolStripButton_question.Image = CType(resources.GetObject("ToolStripButton_question.Image"), System.Drawing.Image)
        Me.ToolStripButton_question.ImageTransparentColor = System.Drawing.Color.Black
        Me.ToolStripButton_question.Name = "ToolStripButton_question"
        Me.ToolStripButton_question.Size = New System.Drawing.Size(36, 37)
        Me.ToolStripButton_question.Text = "查询"
        Me.ToolStripButton_question.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.ToolStripButton_question.Visible = False
        '
        'ToolStripButton_show
        '
        Me.ToolStripButton_show.Image = CType(resources.GetObject("ToolStripButton_show.Image"), System.Drawing.Image)
        Me.ToolStripButton_show.ImageTransparentColor = System.Drawing.Color.Black
        Me.ToolStripButton_show.Name = "ToolStripButton_show"
        Me.ToolStripButton_show.Size = New System.Drawing.Size(60, 37)
        Me.ToolStripButton_show.Text = "打印预览"
        Me.ToolStripButton_show.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        '
        'ToolStripButton_print
        '
        Me.ToolStripButton_print.Image = CType(resources.GetObject("ToolStripButton_print.Image"), System.Drawing.Image)
        Me.ToolStripButton_print.ImageTransparentColor = System.Drawing.Color.Black
        Me.ToolStripButton_print.Name = "ToolStripButton_print"
        Me.ToolStripButton_print.Size = New System.Drawing.Size(36, 37)
        Me.ToolStripButton_print.Text = "打印"
        Me.ToolStripButton_print.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        '
        'ToolStripButton_exit
        '
        Me.ToolStripButton_exit.Image = CType(resources.GetObject("ToolStripButton_exit.Image"), System.Drawing.Image)
        Me.ToolStripButton_exit.ImageTransparentColor = System.Drawing.Color.Black
        Me.ToolStripButton_exit.Name = "ToolStripButton_exit"
        Me.ToolStripButton_exit.Size = New System.Drawing.Size(36, 37)
        Me.ToolStripButton_exit.Text = "退出"
        Me.ToolStripButton_exit.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        '
        'project_check
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 12.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(799, 462)
        Me.Controls.Add(Me.SplitContainer1)
        Me.Controls.Add(Me.TextBox1)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.CheckBox2)
        Me.Controls.Add(Me.CheckBox4)
        Me.Controls.Add(Me.CheckBox3)
        Me.Controls.Add(Me.CheckBox1)
        Me.Controls.Add(Me.ToolStrip1)
        Me.Name = "project_check"
        Me.Text = "工程审批单审核"
        Me.ContextMenuStrip1.ResumeLayout(False)
        Me.ToolStrip1.ResumeLayout(False)
        Me.ToolStrip1.PerformLayout()
        Me.SplitContainer1.Panel1.ResumeLayout(False)
        Me.SplitContainer1.Panel2.ResumeLayout(False)
        Me.SplitContainer1.ResumeLayout(False)
        CType(Me.ExDataGridView1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.ExDataGridView2, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents CheckBox2 As System.Windows.Forms.CheckBox
    Friend WithEvents ToolStripButton_question As System.Windows.Forms.ToolStripButton
    Friend WithEvents ToolStripButton_exit As System.Windows.Forms.ToolStripButton
    Friend WithEvents ContextMenuStrip1 As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents 审核ToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents 反审ToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripMenuItem1 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents 结案ToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents 反结案ToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents CheckBox4 As System.Windows.Forms.CheckBox
    Friend WithEvents CheckBox3 As System.Windows.Forms.CheckBox
    Friend WithEvents CheckBox1 As System.Windows.Forms.CheckBox
    Friend WithEvents ToolStripButton_refresh As System.Windows.Forms.ToolStripButton
    Friend WithEvents ToolStrip1 As System.Windows.Forms.ToolStrip
    Friend WithEvents TextBox1 As System.Windows.Forms.TextBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents SplitContainer1 As System.Windows.Forms.SplitContainer
    Friend WithEvents ExDataGridView1 As BaseClass.ExDataGridView
    Friend WithEvents ExDataGridView2 As BaseClass.ExDataGridView
    Friend WithEvents ToolStripButton_show As System.Windows.Forms.ToolStripButton
    Friend WithEvents ToolStripButton_print As System.Windows.Forms.ToolStripButton
    Friend WithEvents 延期ToolStripMenuItem As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents 延期ToolStripMenuItem1 As System.Windows.Forms.ToolStripMenuItem
End Class
