﻿<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class warehouse_amount
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(warehouse_amount))
        Me.ToolStripButton_refresh = New System.Windows.Forms.ToolStripButton
        Me.ExDataGridView1 = New BaseClass.ExDataGridView
        Me.ToolStripButton_exit = New System.Windows.Forms.ToolStripButton
        Me.ToolStripButton_save = New System.Windows.Forms.ToolStripButton
        Me.ToolStrip1 = New System.Windows.Forms.ToolStrip
        Me.Label1 = New System.Windows.Forms.Label
        Me.TextBox1 = New System.Windows.Forms.TextBox
        Me.CheckBox1 = New System.Windows.Forms.CheckBox
        CType(Me.ExDataGridView1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.ToolStrip1.SuspendLayout()
        Me.SuspendLayout()
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
        'ExDataGridView1
        '
        Me.ExDataGridView1.AllowUserToAddRows = False
        Me.ExDataGridView1.AllowUserToDeleteRows = False
        Me.ExDataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.ExDataGridView1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.ExDataGridView1.Location = New System.Drawing.Point(0, 40)
        Me.ExDataGridView1.MultiSelect = False
        Me.ExDataGridView1.Name = "ExDataGridView1"
        Me.ExDataGridView1.RowTemplate.Height = 23
        Me.ExDataGridView1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect
        Me.ExDataGridView1.Size = New System.Drawing.Size(605, 313)
        Me.ExDataGridView1.TabIndex = 49
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
        'ToolStripButton_save
        '
        Me.ToolStripButton_save.Image = CType(resources.GetObject("ToolStripButton_save.Image"), System.Drawing.Image)
        Me.ToolStripButton_save.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.ToolStripButton_save.Name = "ToolStripButton_save"
        Me.ToolStripButton_save.Size = New System.Drawing.Size(36, 37)
        Me.ToolStripButton_save.Text = "确定"
        Me.ToolStripButton_save.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        '
        'ToolStrip1
        '
        Me.ToolStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ToolStripButton_refresh, Me.ToolStripButton_save, Me.ToolStripButton_exit})
        Me.ToolStrip1.Location = New System.Drawing.Point(0, 0)
        Me.ToolStrip1.Name = "ToolStrip1"
        Me.ToolStrip1.Size = New System.Drawing.Size(605, 40)
        Me.ToolStrip1.TabIndex = 46
        Me.ToolStrip1.Text = "ToolStrip1"
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(220, 17)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(53, 12)
        Me.Label1.TabIndex = 47
        Me.Label1.Text = "查询条件"
        '
        'TextBox1
        '
        Me.TextBox1.Location = New System.Drawing.Point(279, 12)
        Me.TextBox1.Name = "TextBox1"
        Me.TextBox1.Size = New System.Drawing.Size(100, 21)
        Me.TextBox1.TabIndex = 48
        '
        'CheckBox1
        '
        Me.CheckBox1.AutoSize = True
        Me.CheckBox1.BackColor = System.Drawing.SystemColors.Control
        Me.CheckBox1.Location = New System.Drawing.Point(420, 13)
        Me.CheckBox1.Name = "CheckBox1"
        Me.CheckBox1.Size = New System.Drawing.Size(144, 16)
        Me.CheckBox1.TabIndex = 50
        Me.CheckBox1.Text = "不显示库存为零的数据"
        Me.CheckBox1.UseVisualStyleBackColor = False
        Me.CheckBox1.Visible = False
        '
        'warehouse_amount
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 12.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(605, 353)
        Me.Controls.Add(Me.CheckBox1)
        Me.Controls.Add(Me.ExDataGridView1)
        Me.Controls.Add(Me.TextBox1)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.ToolStrip1)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
        Me.Name = "warehouse_amount"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "物料选择"
        CType(Me.ExDataGridView1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ToolStrip1.ResumeLayout(False)
        Me.ToolStrip1.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents ToolStripButton_refresh As System.Windows.Forms.ToolStripButton
    Friend WithEvents ExDataGridView1 As BaseClass.ExDataGridView
    Friend WithEvents ToolStripButton_exit As System.Windows.Forms.ToolStripButton
    Friend WithEvents ToolStripButton_save As System.Windows.Forms.ToolStripButton
    Friend WithEvents ToolStrip1 As System.Windows.Forms.ToolStrip
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents TextBox1 As System.Windows.Forms.TextBox
    Friend WithEvents CheckBox1 As System.Windows.Forms.CheckBox
End Class
