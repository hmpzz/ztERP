﻿<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class XZFY_check
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(XZFY_check))
        Me.ToolStripButton_question = New System.Windows.Forms.ToolStripButton
        Me.ToolStripButton_exit = New System.Windows.Forms.ToolStripButton
        Me.ToolStripButton_print = New System.Windows.Forms.ToolStripButton
        Me.ToolStripButton_show = New System.Windows.Forms.ToolStripButton
        Me.ToolStripButton_refresh = New System.Windows.Forms.ToolStripButton
        Me.CheckBox2 = New System.Windows.Forms.CheckBox
        Me.CheckBox3 = New System.Windows.Forms.CheckBox
        Me.CheckBox1 = New System.Windows.Forms.CheckBox
        Me.ContextMenuStrip1 = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.审核ToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.反审ToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.ToolStripMenuItem1 = New System.Windows.Forms.ToolStripSeparator
        Me.结案ToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.反结案ToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.SplitContainer1 = New System.Windows.Forms.SplitContainer
        Me.ExDataGridView2 = New BaseClass.ExDataGridView
        Me.ToolStrip1 = New System.Windows.Forms.ToolStrip
        Me.反选ToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.ToolStripMenuItem2 = New System.Windows.Forms.ToolStripSeparator
        Me.全选ToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.ToolStripButton_save = New System.Windows.Forms.ToolStripButton
        Me.ExDataGridView1 = New BaseClass.ExDataGridView
        Me.ContextMenuStrip1.SuspendLayout()
        Me.SplitContainer1.Panel1.SuspendLayout()
        Me.SplitContainer1.Panel2.SuspendLayout()
        Me.SplitContainer1.SuspendLayout()
        CType(Me.ExDataGridView2, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.ToolStrip1.SuspendLayout()
        CType(Me.ExDataGridView1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
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
        'ToolStripButton_exit
        '
        Me.ToolStripButton_exit.Image = CType(resources.GetObject("ToolStripButton_exit.Image"), System.Drawing.Image)
        Me.ToolStripButton_exit.ImageTransparentColor = System.Drawing.Color.Black
        Me.ToolStripButton_exit.Name = "ToolStripButton_exit"
        Me.ToolStripButton_exit.Size = New System.Drawing.Size(33, 32)
        Me.ToolStripButton_exit.Text = "退出"
        Me.ToolStripButton_exit.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        '
        'ToolStripButton_print
        '
        Me.ToolStripButton_print.Image = CType(resources.GetObject("ToolStripButton_print.Image"), System.Drawing.Image)
        Me.ToolStripButton_print.ImageTransparentColor = System.Drawing.Color.Black
        Me.ToolStripButton_print.Name = "ToolStripButton_print"
        Me.ToolStripButton_print.Size = New System.Drawing.Size(33, 32)
        Me.ToolStripButton_print.Text = "打印"
        Me.ToolStripButton_print.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.ToolStripButton_print.Visible = False
        '
        'ToolStripButton_show
        '
        Me.ToolStripButton_show.Image = CType(resources.GetObject("ToolStripButton_show.Image"), System.Drawing.Image)
        Me.ToolStripButton_show.ImageTransparentColor = System.Drawing.Color.Black
        Me.ToolStripButton_show.Name = "ToolStripButton_show"
        Me.ToolStripButton_show.Size = New System.Drawing.Size(57, 32)
        Me.ToolStripButton_show.Text = "打印预览"
        Me.ToolStripButton_show.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.ToolStripButton_show.Visible = False
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
        'CheckBox2
        '
        Me.CheckBox2.AutoSize = True
        Me.CheckBox2.BackColor = System.Drawing.SystemColors.Control
        Me.CheckBox2.Location = New System.Drawing.Point(514, 12)
        Me.CheckBox2.Name = "CheckBox2"
        Me.CheckBox2.Size = New System.Drawing.Size(48, 16)
        Me.CheckBox2.TabIndex = 42
        Me.CheckBox2.Text = "审核"
        Me.CheckBox2.UseVisualStyleBackColor = False
        '
        'CheckBox3
        '
        Me.CheckBox3.AutoSize = True
        Me.CheckBox3.BackColor = System.Drawing.SystemColors.Control
        Me.CheckBox3.Location = New System.Drawing.Point(568, 12)
        Me.CheckBox3.Name = "CheckBox3"
        Me.CheckBox3.Size = New System.Drawing.Size(48, 16)
        Me.CheckBox3.TabIndex = 43
        Me.CheckBox3.Text = "结案"
        Me.CheckBox3.UseVisualStyleBackColor = False
        '
        'CheckBox1
        '
        Me.CheckBox1.AutoSize = True
        Me.CheckBox1.BackColor = System.Drawing.SystemColors.Control
        Me.CheckBox1.Checked = True
        Me.CheckBox1.CheckState = System.Windows.Forms.CheckState.Checked
        Me.CheckBox1.Location = New System.Drawing.Point(460, 12)
        Me.CheckBox1.Name = "CheckBox1"
        Me.CheckBox1.Size = New System.Drawing.Size(48, 16)
        Me.CheckBox1.TabIndex = 41
        Me.CheckBox1.Text = "新建"
        Me.CheckBox1.UseVisualStyleBackColor = False
        '
        'ContextMenuStrip1
        '
        Me.ContextMenuStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.审核ToolStripMenuItem, Me.反审ToolStripMenuItem, Me.ToolStripMenuItem1, Me.结案ToolStripMenuItem, Me.反结案ToolStripMenuItem, Me.ToolStripMenuItem2, Me.全选ToolStripMenuItem, Me.反选ToolStripMenuItem})
        Me.ContextMenuStrip1.Name = "ContextMenuStrip1"
        Me.ContextMenuStrip1.Size = New System.Drawing.Size(107, 148)
        '
        '审核ToolStripMenuItem
        '
        Me.审核ToolStripMenuItem.Name = "审核ToolStripMenuItem"
        Me.审核ToolStripMenuItem.Size = New System.Drawing.Size(106, 22)
        Me.审核ToolStripMenuItem.Text = "审核"
        '
        '反审ToolStripMenuItem
        '
        Me.反审ToolStripMenuItem.Name = "反审ToolStripMenuItem"
        Me.反审ToolStripMenuItem.Size = New System.Drawing.Size(106, 22)
        Me.反审ToolStripMenuItem.Text = "反审"
        '
        'ToolStripMenuItem1
        '
        Me.ToolStripMenuItem1.Name = "ToolStripMenuItem1"
        Me.ToolStripMenuItem1.Size = New System.Drawing.Size(103, 6)
        '
        '结案ToolStripMenuItem
        '
        Me.结案ToolStripMenuItem.Name = "结案ToolStripMenuItem"
        Me.结案ToolStripMenuItem.Size = New System.Drawing.Size(106, 22)
        Me.结案ToolStripMenuItem.Text = "结案"
        '
        '反结案ToolStripMenuItem
        '
        Me.反结案ToolStripMenuItem.Name = "反结案ToolStripMenuItem"
        Me.反结案ToolStripMenuItem.Size = New System.Drawing.Size(106, 22)
        Me.反结案ToolStripMenuItem.Text = "反结案"
        '
        'SplitContainer1
        '
        Me.SplitContainer1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.SplitContainer1.Location = New System.Drawing.Point(0, 35)
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
        Me.SplitContainer1.Size = New System.Drawing.Size(696, 449)
        Me.SplitContainer1.SplitterDistance = 201
        Me.SplitContainer1.TabIndex = 44
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
        Me.ExDataGridView2.Size = New System.Drawing.Size(696, 244)
        Me.ExDataGridView2.TabIndex = 1
        '
        'ToolStrip1
        '
        Me.ToolStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ToolStripButton_refresh, Me.ToolStripButton_save, Me.ToolStripButton_question, Me.ToolStripButton_show, Me.ToolStripButton_print, Me.ToolStripButton_exit})
        Me.ToolStrip1.Location = New System.Drawing.Point(0, 0)
        Me.ToolStrip1.Name = "ToolStrip1"
        Me.ToolStrip1.Size = New System.Drawing.Size(696, 35)
        Me.ToolStrip1.TabIndex = 40
        Me.ToolStrip1.Text = "ToolStrip1"
        '
        '反选ToolStripMenuItem
        '
        Me.反选ToolStripMenuItem.Name = "反选ToolStripMenuItem"
        Me.反选ToolStripMenuItem.Size = New System.Drawing.Size(106, 22)
        Me.反选ToolStripMenuItem.Text = "反选"
        '
        'ToolStripMenuItem2
        '
        Me.ToolStripMenuItem2.Name = "ToolStripMenuItem2"
        Me.ToolStripMenuItem2.Size = New System.Drawing.Size(103, 6)
        '
        '全选ToolStripMenuItem
        '
        Me.全选ToolStripMenuItem.Name = "全选ToolStripMenuItem"
        Me.全选ToolStripMenuItem.Size = New System.Drawing.Size(106, 22)
        Me.全选ToolStripMenuItem.Text = "全选"
        '
        'ToolStripButton_save
        '
        Me.ToolStripButton_save.Image = CType(resources.GetObject("ToolStripButton_save.Image"), System.Drawing.Image)
        Me.ToolStripButton_save.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.ToolStripButton_save.Name = "ToolStripButton_save"
        Me.ToolStripButton_save.Size = New System.Drawing.Size(33, 32)
        Me.ToolStripButton_save.Text = "审核"
        Me.ToolStripButton_save.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
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
        Me.ExDataGridView1.RowTemplate.Height = 23
        Me.ExDataGridView1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect
        Me.ExDataGridView1.Size = New System.Drawing.Size(696, 201)
        Me.ExDataGridView1.TabIndex = 8
        '
        'XZFY_check
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 12.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(696, 484)
        Me.Controls.Add(Me.CheckBox2)
        Me.Controls.Add(Me.CheckBox3)
        Me.Controls.Add(Me.CheckBox1)
        Me.Controls.Add(Me.SplitContainer1)
        Me.Controls.Add(Me.ToolStrip1)
        Me.Name = "XZFY_check"
        Me.Text = "行政费用审核"
        Me.ContextMenuStrip1.ResumeLayout(False)
        Me.SplitContainer1.Panel1.ResumeLayout(False)
        Me.SplitContainer1.Panel2.ResumeLayout(False)
        Me.SplitContainer1.ResumeLayout(False)
        CType(Me.ExDataGridView2, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ToolStrip1.ResumeLayout(False)
        Me.ToolStrip1.PerformLayout()
        CType(Me.ExDataGridView1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents ToolStripButton_question As System.Windows.Forms.ToolStripButton
    Friend WithEvents ToolStripButton_exit As System.Windows.Forms.ToolStripButton
    Friend WithEvents ToolStripButton_print As System.Windows.Forms.ToolStripButton
    Friend WithEvents ToolStripButton_show As System.Windows.Forms.ToolStripButton
    Friend WithEvents ToolStripButton_refresh As System.Windows.Forms.ToolStripButton
    Friend WithEvents CheckBox2 As System.Windows.Forms.CheckBox
    Friend WithEvents CheckBox3 As System.Windows.Forms.CheckBox
    Friend WithEvents CheckBox1 As System.Windows.Forms.CheckBox
    Friend WithEvents SplitContainer1 As System.Windows.Forms.SplitContainer
    Friend WithEvents ExDataGridView2 As BaseClass.ExDataGridView
    Friend WithEvents ToolStrip1 As System.Windows.Forms.ToolStrip
    Friend WithEvents ContextMenuStrip1 As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents 审核ToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents 反审ToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripMenuItem1 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents 结案ToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents 反结案ToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripMenuItem2 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents 全选ToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents 反选ToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripButton_save As System.Windows.Forms.ToolStripButton
    Friend WithEvents ExDataGridView1 As BaseClass.ExDataGridView
End Class
