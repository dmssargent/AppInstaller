﻿<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Form1
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(Form1))
        Me.lblStatus = New System.Windows.Forms.Label()
        Me.txtFileLocation = New System.Windows.Forms.TextBox()
        Me.chkReinstall = New System.Windows.Forms.CheckBox()
        Me.chkForce = New System.Windows.Forms.CheckBox()
        Me.btnOpenFileDialogTrigger = New System.Windows.Forms.Button()
        Me.btnInstall = New System.Windows.Forms.Button()
        Me.pgbStatus = New System.Windows.Forms.ProgressBar()
        Me.SuspendLayout()
        '
        'lblStatus
        '
        Me.lblStatus.AllowDrop = True
        Me.lblStatus.Location = New System.Drawing.Point(8, 7)
        Me.lblStatus.Margin = New System.Windows.Forms.Padding(2, 0, 2, 0)
        Me.lblStatus.Name = "lblStatus"
        Me.lblStatus.Size = New System.Drawing.Size(526, 248)
        Me.lblStatus.TabIndex = 0
        Me.lblStatus.Text = "Drag and Drop the APK file, or use the Open File... dialog to select the file"
        Me.lblStatus.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'txtFileLocation
        '
        Me.txtFileLocation.Location = New System.Drawing.Point(10, 353)
        Me.txtFileLocation.Margin = New System.Windows.Forms.Padding(2, 2, 2, 2)
        Me.txtFileLocation.Name = "txtFileLocation"
        Me.txtFileLocation.Size = New System.Drawing.Size(450, 20)
        Me.txtFileLocation.TabIndex = 1
        '
        'chkReinstall
        '
        Me.chkReinstall.AutoSize = True
        Me.chkReinstall.Location = New System.Drawing.Point(10, 332)
        Me.chkReinstall.Margin = New System.Windows.Forms.Padding(2, 2, 2, 2)
        Me.chkReinstall.Name = "chkReinstall"
        Me.chkReinstall.Size = New System.Drawing.Size(110, 17)
        Me.chkReinstall.TabIndex = 2
        Me.chkReinstall.Text = "Update (Reinstall)"
        Me.chkReinstall.UseVisualStyleBackColor = True
        '
        'chkForce
        '
        Me.chkForce.AutoSize = True
        Me.chkForce.Location = New System.Drawing.Point(122, 332)
        Me.chkForce.Margin = New System.Windows.Forms.Padding(2, 2, 2, 2)
        Me.chkForce.Name = "chkForce"
        Me.chkForce.Size = New System.Drawing.Size(53, 17)
        Me.chkForce.TabIndex = 3
        Me.chkForce.Text = "Force"
        Me.chkForce.UseVisualStyleBackColor = True
        '
        'btnOpenFileDialogTrigger
        '
        Me.btnOpenFileDialogTrigger.Location = New System.Drawing.Point(463, 350)
        Me.btnOpenFileDialogTrigger.Margin = New System.Windows.Forms.Padding(2, 2, 2, 2)
        Me.btnOpenFileDialogTrigger.Name = "btnOpenFileDialogTrigger"
        Me.btnOpenFileDialogTrigger.Size = New System.Drawing.Size(73, 25)
        Me.btnOpenFileDialogTrigger.TabIndex = 4
        Me.btnOpenFileDialogTrigger.Text = "Open File..."
        Me.btnOpenFileDialogTrigger.UseVisualStyleBackColor = True
        '
        'btnInstall
        '
        Me.btnInstall.Enabled = False
        Me.btnInstall.Font = New System.Drawing.Font("Open Sans", 16.2!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnInstall.ForeColor = System.Drawing.Color.Green
        Me.btnInstall.Location = New System.Drawing.Point(183, 240)
        Me.btnInstall.Margin = New System.Windows.Forms.Padding(2, 2, 2, 2)
        Me.btnInstall.Name = "btnInstall"
        Me.btnInstall.Size = New System.Drawing.Size(191, 63)
        Me.btnInstall.TabIndex = 5
        Me.btnInstall.Text = "INSTALL"
        Me.btnInstall.UseVisualStyleBackColor = True
        '
        'pgbStatus
        '
        Me.pgbStatus.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.pgbStatus.Location = New System.Drawing.Point(9, 308)
        Me.pgbStatus.Margin = New System.Windows.Forms.Padding(2, 2, 2, 2)
        Me.pgbStatus.Name = "pgbStatus"
        Me.pgbStatus.Size = New System.Drawing.Size(525, 19)
        Me.pgbStatus.Style = System.Windows.Forms.ProgressBarStyle.Marquee
        Me.pgbStatus.TabIndex = 6
        Me.pgbStatus.Visible = False
        '
        'Form1
        '
        Me.AllowDrop = True
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom
        Me.ClientSize = New System.Drawing.Size(544, 381)
        Me.Controls.Add(Me.pgbStatus)
        Me.Controls.Add(Me.btnInstall)
        Me.Controls.Add(Me.btnOpenFileDialogTrigger)
        Me.Controls.Add(Me.chkForce)
        Me.Controls.Add(Me.chkReinstall)
        Me.Controls.Add(Me.txtFileLocation)
        Me.Controls.Add(Me.lblStatus)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Margin = New System.Windows.Forms.Padding(2, 2, 2, 2)
        Me.Name = "Form1"
        Me.Text = "APK Installer"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents lblStatus As Label
    Friend WithEvents txtFileLocation As TextBox
    Friend WithEvents chkReinstall As CheckBox
    Friend WithEvents chkForce As CheckBox
    Friend WithEvents btnOpenFileDialogTrigger As Button
    Friend WithEvents btnInstall As Button
    Friend WithEvents pgbStatus As ProgressBar
End Class
