Imports System.ComponentModel
Imports MaterialSkin
Imports MaterialSkin.Controls
Imports Microsoft.VisualBasic.CompilerServices

<DesignerGenerated()>
Partial Class Main
    Inherits MaterialForm

    'Form overrides dispose to clean up the component list.
    <DebuggerNonUserCode()>
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
    Private components As IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(Main))
        Me.lblStatus = New MaterialSkin.Controls.MaterialLabel()
        Me.txtFileLocation = New MaterialSkin.Controls.MaterialSingleLineTextField()
        Me.chkReinstall = New MaterialSkin.Controls.MaterialCheckBox()
        Me.chkForce = New MaterialSkin.Controls.MaterialCheckBox()
        Me.btnOpenFileDialogTrigger = New MaterialSkin.Controls.MaterialFlatButton()
        Me.btnInstall = New MaterialSkin.Controls.MaterialRaisedButton()
        Me.pgbStatus = New MaterialSkin.Controls.MaterialProgressBar()
        Me.lnkAbout = New System.Windows.Forms.LinkLabel()
        Me.lnkHelp = New System.Windows.Forms.LinkLabel()
        Me.SuspendLayout()
        '
        'lblStatus
        '
        Me.lblStatus.AllowDrop = True
        resources.ApplyResources(Me.lblStatus, "lblStatus")
        Me.lblStatus.Depth = 0
        Me.lblStatus.ForeColor = System.Drawing.Color.FromArgb(CType(CType(222, Byte), Integer), CType(CType(0, Byte), Integer), CType(CType(0, Byte), Integer), CType(CType(0, Byte), Integer))
        Me.lblStatus.MouseState = MaterialSkin.MouseState.HOVER
        Me.lblStatus.Name = "lblStatus"
        '
        'txtFileLocation
        '
        resources.ApplyResources(Me.txtFileLocation, "txtFileLocation")
        Me.txtFileLocation.Depth = 0
        Me.txtFileLocation.Hint = ""
        Me.txtFileLocation.MaxLength = 32767
        Me.txtFileLocation.MouseState = MaterialSkin.MouseState.HOVER
        Me.txtFileLocation.Name = "txtFileLocation"
        Me.txtFileLocation.PasswordChar = Global.Microsoft.VisualBasic.ChrW(0)
        Me.txtFileLocation.SelectedText = ""
        Me.txtFileLocation.SelectionLength = 0
        Me.txtFileLocation.SelectionStart = 0
        Me.txtFileLocation.TabStop = False
        Me.txtFileLocation.UseSystemPasswordChar = False
        '
        'chkReinstall
        '
        resources.ApplyResources(Me.chkReinstall, "chkReinstall")
        Me.chkReinstall.Checked = True
        Me.chkReinstall.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkReinstall.Depth = 0
        Me.chkReinstall.MouseLocation = New System.Drawing.Point(-1, -1)
        Me.chkReinstall.MouseState = MaterialSkin.MouseState.HOVER
        Me.chkReinstall.Name = "chkReinstall"
        Me.chkReinstall.Ripple = True
        Me.chkReinstall.UseVisualStyleBackColor = True
        '
        'chkForce
        '
        resources.ApplyResources(Me.chkForce, "chkForce")
        Me.chkForce.Depth = 0
        Me.chkForce.MouseLocation = New System.Drawing.Point(-1, -1)
        Me.chkForce.MouseState = MaterialSkin.MouseState.HOVER
        Me.chkForce.Name = "chkForce"
        Me.chkForce.Ripple = True
        Me.chkForce.UseVisualStyleBackColor = True
        '
        'btnOpenFileDialogTrigger
        '
        resources.ApplyResources(Me.btnOpenFileDialogTrigger, "btnOpenFileDialogTrigger")
        Me.btnOpenFileDialogTrigger.Depth = 0
        Me.btnOpenFileDialogTrigger.MouseState = MaterialSkin.MouseState.HOVER
        Me.btnOpenFileDialogTrigger.Name = "btnOpenFileDialogTrigger"
        Me.btnOpenFileDialogTrigger.Primary = False
        Me.btnOpenFileDialogTrigger.UseVisualStyleBackColor = True
        '
        'btnInstall
        '
        Me.btnInstall.AllowDrop = True
        resources.ApplyResources(Me.btnInstall, "btnInstall")
        Me.btnInstall.Depth = 0
        Me.btnInstall.ForeColor = System.Drawing.Color.Green
        Me.btnInstall.MouseState = MaterialSkin.MouseState.HOVER
        Me.btnInstall.Name = "btnInstall"
        Me.btnInstall.Primary = True
        Me.btnInstall.UseVisualStyleBackColor = True
        '
        'pgbStatus
        '
        resources.ApplyResources(Me.pgbStatus, "pgbStatus")
        Me.pgbStatus.Depth = 0
        Me.pgbStatus.MouseState = MaterialSkin.MouseState.HOVER
        Me.pgbStatus.Name = "pgbStatus"
        Me.pgbStatus.Style = System.Windows.Forms.ProgressBarStyle.Marquee
        '
        'lnkAbout
        '
        Me.lnkAbout.ActiveLinkColor = System.Drawing.Color.White
        resources.ApplyResources(Me.lnkAbout, "lnkAbout")
        Me.lnkAbout.BackColor = System.Drawing.Color.FromArgb(CType(CType(245, Byte), Integer), CType(CType(124, Byte), Integer), CType(CType(0, Byte), Integer))
        Me.lnkAbout.ForeColor = System.Drawing.Color.FromArgb(CType(CType(222, Byte), Integer), CType(CType(0, Byte), Integer), CType(CType(0, Byte), Integer), CType(CType(0, Byte), Integer))
        Me.lnkAbout.LinkBehavior = System.Windows.Forms.LinkBehavior.HoverUnderline
        Me.lnkAbout.LinkColor = System.Drawing.Color.White
        Me.lnkAbout.Name = "lnkAbout"
        Me.lnkAbout.TabStop = True
        Me.lnkAbout.VisitedLinkColor = System.Drawing.Color.White
        '
        'lnkHelp
        '
        Me.lnkHelp.ActiveLinkColor = System.Drawing.Color.White
        resources.ApplyResources(Me.lnkHelp, "lnkHelp")
        Me.lnkHelp.BackColor = System.Drawing.Color.FromArgb(CType(CType(245, Byte), Integer), CType(CType(124, Byte), Integer), CType(CType(0, Byte), Integer))
        Me.lnkHelp.ForeColor = System.Drawing.Color.FromArgb(CType(CType(222, Byte), Integer), CType(CType(0, Byte), Integer), CType(CType(0, Byte), Integer), CType(CType(0, Byte), Integer))
        Me.lnkHelp.LinkBehavior = System.Windows.Forms.LinkBehavior.HoverUnderline
        Me.lnkHelp.LinkColor = System.Drawing.Color.White
        Me.lnkHelp.Name = "lnkHelp"
        Me.lnkHelp.TabStop = True
        Me.lnkHelp.VisitedLinkColor = System.Drawing.Color.White
        '
        'Main
        '
        Me.AllowDrop = True
        resources.ApplyResources(Me, "$this")
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.Controls.Add(Me.lnkHelp)
        Me.Controls.Add(Me.lnkAbout)
        Me.Controls.Add(Me.pgbStatus)
        Me.Controls.Add(Me.btnInstall)
        Me.Controls.Add(Me.btnOpenFileDialogTrigger)
        Me.Controls.Add(Me.chkForce)
        Me.Controls.Add(Me.chkReinstall)
        Me.Controls.Add(Me.txtFileLocation)
        Me.Controls.Add(Me.lblStatus)
        Me.Name = "Main"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents lblStatus As MaterialLabel
    Friend WithEvents txtFileLocation As MaterialSingleLineTextField
    Friend WithEvents chkReinstall As MaterialCheckBox
    Friend WithEvents chkForce As MaterialCheckBox
    Friend WithEvents btnOpenFileDialogTrigger As MaterialFlatButton
    Friend WithEvents btnInstall As MaterialRaisedButton
    Friend WithEvents pgbStatus As MaterialProgressBar
    Friend WithEvents lnkAbout As LinkLabel
    Friend WithEvents lnkHelp As LinkLabel
End Class
