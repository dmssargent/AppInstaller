<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Main
    Inherits MaterialSkin.Controls.MaterialForm

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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(Main))
        Me.lblStatus = New MaterialSkin.Controls.MaterialLabel()
        Me.txtFileLocation = New MaterialSkin.Controls.MaterialSingleLineTextField()
        Me.chkReinstall = New MaterialSkin.Controls.MaterialCheckBox()
        Me.chkForce = New MaterialSkin.Controls.MaterialCheckBox()
        Me.btnOpenFileDialogTrigger = New MaterialSkin.Controls.MaterialFlatButton()
        Me.btnInstall = New MaterialSkin.Controls.MaterialRaisedButton()
        Me.pgbStatus = New MaterialSkin.Controls.MaterialProgressBar()
        Me.SuspendLayout()
        '
        'lblStatus
        '
        Me.lblStatus.AllowDrop = True
        Me.lblStatus.Depth = 0
        Me.lblStatus.Font = New System.Drawing.Font("Roboto", 11.0!)
        Me.lblStatus.ForeColor = System.Drawing.Color.FromArgb(CType(CType(222, Byte), Integer), CType(CType(0, Byte), Integer), CType(CType(0, Byte), Integer), CType(CType(0, Byte), Integer))
        Me.lblStatus.Location = New System.Drawing.Point(8, 98)
        Me.lblStatus.MouseState = MaterialSkin.MouseState.HOVER
        Me.lblStatus.Name = "lblStatus"
        Me.lblStatus.Size = New System.Drawing.Size(824, 230)
        Me.lblStatus.TabIndex = 0
        Me.lblStatus.Text = "Drag the APK here..."
        Me.lblStatus.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'txtFileLocation
        '
        Me.txtFileLocation.Depth = 0
        Me.txtFileLocation.Hint = ""
        Me.txtFileLocation.Location = New System.Drawing.Point(13, 496)
        Me.txtFileLocation.MaxLength = 32767
        Me.txtFileLocation.MouseState = MaterialSkin.MouseState.HOVER
        Me.txtFileLocation.Name = "txtFileLocation"
        Me.txtFileLocation.PasswordChar = Global.Microsoft.VisualBasic.ChrW(0)
        Me.txtFileLocation.SelectedText = ""
        Me.txtFileLocation.SelectionLength = 0
        Me.txtFileLocation.SelectionStart = 0
        Me.txtFileLocation.Size = New System.Drawing.Size(1014, 28)
        Me.txtFileLocation.TabIndex = 1
        Me.txtFileLocation.TabStop = False
        Me.txtFileLocation.UseSystemPasswordChar = False
        '
        'chkReinstall
        '
        Me.chkReinstall.AutoSize = True
        Me.chkReinstall.Checked = True
        Me.chkReinstall.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkReinstall.Depth = 0
        Me.chkReinstall.Font = New System.Drawing.Font("Roboto", 10.0!)
        Me.chkReinstall.Location = New System.Drawing.Point(13, 454)
        Me.chkReinstall.Margin = New System.Windows.Forms.Padding(0)
        Me.chkReinstall.MouseLocation = New System.Drawing.Point(-1, -1)
        Me.chkReinstall.MouseState = MaterialSkin.MouseState.HOVER
        Me.chkReinstall.Name = "chkReinstall"
        Me.chkReinstall.Ripple = True
        Me.chkReinstall.Size = New System.Drawing.Size(169, 30)
        Me.chkReinstall.TabIndex = 2
        Me.chkReinstall.Text = "Update (Reinstall)"
        Me.chkReinstall.UseVisualStyleBackColor = True
        '
        'chkForce
        '
        Me.chkForce.AutoSize = True
        Me.chkForce.Depth = 0
        Me.chkForce.Font = New System.Drawing.Font("Roboto", 10.0!)
        Me.chkForce.Location = New System.Drawing.Point(205, 454)
        Me.chkForce.Margin = New System.Windows.Forms.Padding(0)
        Me.chkForce.MouseLocation = New System.Drawing.Point(-1, -1)
        Me.chkForce.MouseState = MaterialSkin.MouseState.HOVER
        Me.chkForce.Name = "chkForce"
        Me.chkForce.Ripple = True
        Me.chkForce.Size = New System.Drawing.Size(74, 30)
        Me.chkForce.TabIndex = 3
        Me.chkForce.Text = "Force"
        Me.chkForce.UseVisualStyleBackColor = True
        '
        'btnOpenFileDialogTrigger
        '
        Me.btnOpenFileDialogTrigger.AutoSize = True
        Me.btnOpenFileDialogTrigger.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        Me.btnOpenFileDialogTrigger.Depth = 0
        Me.btnOpenFileDialogTrigger.Location = New System.Drawing.Point(774, 488)
        Me.btnOpenFileDialogTrigger.Margin = New System.Windows.Forms.Padding(4, 6, 4, 6)
        Me.btnOpenFileDialogTrigger.MouseState = MaterialSkin.MouseState.HOVER
        Me.btnOpenFileDialogTrigger.Name = "btnOpenFileDialogTrigger"
        Me.btnOpenFileDialogTrigger.Primary = False
        Me.btnOpenFileDialogTrigger.Size = New System.Drawing.Size(58, 36)
        Me.btnOpenFileDialogTrigger.TabIndex = 4
        Me.btnOpenFileDialogTrigger.Text = "Open"
        Me.btnOpenFileDialogTrigger.UseVisualStyleBackColor = True
        '
        'btnInstall
        '
        Me.btnInstall.Depth = 0
        Me.btnInstall.Font = New System.Drawing.Font("Open Sans Extrabold", 18.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnInstall.ForeColor = System.Drawing.Color.Green
        Me.btnInstall.Location = New System.Drawing.Point(293, 308)
        Me.btnInstall.MouseState = MaterialSkin.MouseState.HOVER
        Me.btnInstall.Name = "btnInstall"
        Me.btnInstall.Primary = True
        Me.btnInstall.Size = New System.Drawing.Size(255, 78)
        Me.btnInstall.TabIndex = 5
        Me.btnInstall.Text = "INSTALL"
        Me.btnInstall.UseVisualStyleBackColor = True
        Me.btnInstall.Visible = False
        '
        'pgbStatus
        '
        Me.pgbStatus.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.pgbStatus.Depth = 0
        Me.pgbStatus.Location = New System.Drawing.Point(14, 410)
        Me.pgbStatus.MouseState = MaterialSkin.MouseState.HOVER
        Me.pgbStatus.Name = "pgbStatus"
        Me.pgbStatus.Size = New System.Drawing.Size(820, 5)
        Me.pgbStatus.Style = System.Windows.Forms.ProgressBarStyle.Marquee
        Me.pgbStatus.TabIndex = 6
        Me.pgbStatus.Visible = False
        '
        'Main
        '
        Me.AllowDrop = True
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 16.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom
        Me.ClientSize = New System.Drawing.Size(846, 539)
        Me.Controls.Add(Me.pgbStatus)
        Me.Controls.Add(Me.btnInstall)
        Me.Controls.Add(Me.btnOpenFileDialogTrigger)
        Me.Controls.Add(Me.chkForce)
        Me.Controls.Add(Me.chkReinstall)
        Me.Controls.Add(Me.txtFileLocation)
        Me.Controls.Add(Me.lblStatus)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Margin = New System.Windows.Forms.Padding(2)
        Me.Name = "Main"
        Me.Text = "App Installer"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents lblStatus As MaterialSkin.Controls.MaterialLabel
    Friend WithEvents txtFileLocation As MaterialSkin.Controls.MaterialSingleLineTextField
    Friend WithEvents chkReinstall As MaterialSkin.Controls.MaterialCheckBox
    Friend WithEvents chkForce As MaterialSkin.Controls.MaterialCheckBox
    Friend WithEvents btnOpenFileDialogTrigger As MaterialSkin.Controls.MaterialFlatButton
    Friend WithEvents btnInstall As MaterialSkin.Controls.MaterialRaisedButton
    Friend WithEvents pgbStatus As MaterialSkin.Controls.MaterialProgressBar
End Class
