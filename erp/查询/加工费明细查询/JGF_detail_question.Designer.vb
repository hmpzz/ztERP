﻿<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class JGF_detail_question
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(JGF_detail_question))
        Me.Label4 = New System.Windows.Forms.Label
        Me.Label3 = New System.Windows.Forms.Label
        Me.DateTimePicker2 = New System.Windows.Forms.DateTimePicker
        Me.DateTimePicker1 = New System.Windows.Forms.DateTimePicker
        Me.Label2 = New System.Windows.Forms.Label
        Me.SaveFileDialog1 = New System.Windows.Forms.SaveFileDialog
        Me.ExDataGridView1 = New BaseClass.ExDataGridView
        Me.ComboBox1 = New System.Windows.Forms.ComboBox
        Me.ComboBox2 = New System.Windows.Forms.ComboBox
        Me.ToolStripButton_exit = New System.Windows.Forms.ToolStripButton
        Me.ToolStripButton_question = New System.Windows.Forms.ToolStripButton
        Me.ToolStripButton_refresh = New System.Windows.Forms.ToolStripButton
        Me.ToolStrip1 = New System.Windows.Forms.ToolStrip
        Me.ToolStripButton_excel = New System.Windows.Forms.ToolStripButton
        Me.Label1 = New System.Windows.Forms.Label
        CType(Me.ExDataGridView1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.ToolStrip1.SuspendLayout()
        Me.SuspendLayout()
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(741, 15)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(65, 12)
        Me.Label4.TabIndex = 65
        Me.Label4.Text = "金额总计："
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(578, 15)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(17, 12)
        Me.Label3.TabIndex = 64
        Me.Label3.Text = "至"
        '
        'DateTimePicker2
        '
        Me.DateTimePicker2.CustomFormat = "yyyy-MM-dd"
        Me.DateTimePicker2.Format = System.Windows.Forms.DateTimePickerFormat.Custom
        Me.DateTimePicker2.Location = New System.Drawing.Point(601, 12)
        Me.DateTimePicker2.Name = "DateTimePicker2"
        Me.DateTimePicker2.Size = New System.Drawing.Size(109, 21)
        Me.DateTimePicker2.TabIndex = 63
        '
        'DateTimePicker1
        '
        Me.DateTimePicker1.CustomFormat = "yyyy-MM-dd"
        Me.DateTimePicker1.Format = System.Windows.Forms.DateTimePickerFormat.Custom
        Me.DateTimePicker1.Location = New System.Drawing.Point(463, 12)
        Me.DateTimePicker1.Name = "DateTimePicker1"
        Me.DateTimePicker1.Size = New System.Drawing.Size(109, 21)
        Me.DateTimePicker1.TabIndex = 62
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(393, 15)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(65, 12)
        Me.Label2.TabIndex = 61
        Me.Label2.Text = "录入日期从"
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
        Me.ExDataGridView1.ReadOnly = True
        Me.ExDataGridView1.RowTemplate.Height = 23
        Me.ExDataGridView1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect
        Me.ExDataGridView1.Size = New System.Drawing.Size(934, 447)
        Me.ExDataGridView1.TabIndex = 60
        '
        'ComboBox1
        '
        Me.ComboBox1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.ComboBox1.FormattingEnabled = True
        Me.ComboBox1.Location = New System.Drawing.Point(248, 12)
        Me.ComboBox1.Name = "ComboBox1"
        Me.ComboBox1.Size = New System.Drawing.Size(121, 20)
        Me.ComboBox1.TabIndex = 58
        '
        'ComboBox2
        '
        Me.ComboBox2.Location = New System.Drawing.Point(273, 14)
        Me.ComboBox2.Name = "ComboBox2"
        Me.ComboBox2.Size = New System.Drawing.Size(121, 20)
        Me.ComboBox2.TabIndex = 59
        Me.ComboBox2.Visible = False
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
        'ToolStripButton_refresh
        '
        Me.ToolStripButton_refresh.Image = CType(resources.GetObject("ToolStripButton_refresh.Image"), System.Drawing.Image)
        Me.ToolStripButton_refresh.ImageTransparentColor = System.Drawing.Color.Black
        Me.ToolStripButton_refresh.Name = "ToolStripButton_refresh"
        Me.ToolStripButton_refresh.Size = New System.Drawing.Size(36, 37)
        Me.ToolStripButton_refresh.Text = "刷新"
        Me.ToolStripButton_refresh.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        '
        'ToolStrip1
        '
        Me.ToolStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ToolStripButton_refresh, Me.ToolStripButton_question, Me.ToolStripButton_excel, Me.ToolStripButton_exit})
        Me.ToolStrip1.Location = New System.Drawing.Point(0, 0)
        Me.ToolStrip1.Name = "ToolStrip1"
        Me.ToolStrip1.Size = New System.Drawing.Size(934, 40)
        Me.ToolStrip1.TabIndex = 56
        Me.ToolStrip1.Text = "ToolStrip1"
        '
        'ToolStripButton_excel
        '
        Me.ToolStripButton_excel.Image = CType(resources.GetObject("ToolStripButton_excel.Image"), System.Drawing.Image)
        Me.ToolStripButton_excel.ImageTransparentColor = System.Drawing.Color.Black
        Me.ToolStripButton_excel.Name = "ToolStripButton_excel"
        Me.ToolStripButton_excel.Size = New System.Drawing.Size(36, 37)
        Me.ToolStripButton_excel.Text = "导出"
        Me.ToolStripButton_excel.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(201, 15)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(41, 12)
        Me.Label1.TabIndex = 57
        Me.Label1.Text = "供应商"
        '
        'JGF_detail_question
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 12.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(934, 487)
        Me.Controls.Add(Me.Label4)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.DateTimePicker2)
        Me.Controls.Add(Me.DateTimePicker1)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.ExDataGridView1)
        Me.Controls.Add(Me.ComboBox1)
        Me.Controls.Add(Me.ComboBox2)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.ToolStrip1)
        Me.Name = "JGF_detail_question"
        Me.Text = "加工费明细查询"
        CType(Me.ExDataGridView1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ToolStrip1.ResumeLayout(False)
        Me.ToolStrip1.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents DateTimePicker2 As System.Windows.Forms.DateTimePicker
    Friend WithEvents DateTimePicker1 As System.Windows.Forms.DateTimePicker
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents SaveFileDialog1 As System.Windows.Forms.SaveFileDialog
    Friend WithEvents ExDataGridView1 As BaseClass.ExDataGridView
    Friend WithEvents ComboBox1 As System.Windows.Forms.ComboBox
    Friend WithEvents ComboBox2 As System.Windows.Forms.ComboBox
    Friend WithEvents ToolStripButton_exit As System.Windows.Forms.ToolStripButton
    Friend WithEvents ToolStripButton_question As System.Windows.Forms.ToolStripButton
    Friend WithEvents ToolStripButton_refresh As System.Windows.Forms.ToolStripButton
    Friend WithEvents ToolStrip1 As System.Windows.Forms.ToolStrip
    Friend WithEvents ToolStripButton_excel As System.Windows.Forms.ToolStripButton
    Friend WithEvents Label1 As System.Windows.Forms.Label
End Class
