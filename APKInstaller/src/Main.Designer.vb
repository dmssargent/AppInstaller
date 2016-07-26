Imports System.ComponentModel
Imports MaterialSkin
Imports MaterialSkin.Controls
Imports Microsoft.VisualBasic.CompilerServices

<DesignerGenerated()> _
Partial Class Main
    Inherits MaterialForm

    'Form overrides dispose to clean up the component list.
    <DebuggerNonUserCode()> _
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
    <DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(Main))
        Me.lblStatus = New MaterialSkin.Controls.MaterialLabel()
        Me.txtFileLocation = New MaterialSkin.Controls.MaterialSingleLineTextField()
        Me.chkReinstall = New MaterialSkin.Controls.MaterialCheckBox()
        Me.chkForce = New MaterialSkin.Controls.MaterialCheckBox()
        Me.btnOpenFileDialogTrigger = New MaterialSkin.Controls.MaterialFlatButton()
        Me.btnInstall = New MaterialSkin.Controls.MaterialRaisedButton()
        Me.pgbStatus = New MaterialSkin.Controls.MaterialProgressBar()
        Me.Label1 = New MaterialSkin.Controls.MaterialLabel()
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
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
        'Label1
        '
        resources.ApplyResources(Me.Label1, "Label1")
        Me.Label1.Depth = 0
        Me.Label1.ForeColor = System.Drawing.Color.FromArgb(CType(CType(222, Byte), Integer), CType(CType(0, Byte), Integer), CType(CType(0, Byte), Integer), CType(CType(0, Byte), Integer))
        Me.Label1.MouseState = MaterialSkin.MouseState.HOVER
        Me.Label1.Name = "Label1"
        '
        'Main
        '
        Me.AllowDrop = True
        resources.ApplyResources(Me, "$this")
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.Controls.Add(Me.Label1)
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
    Friend WithEvents Label1 As MaterialLabel
    Friend WithEvents ToolTip1 As ToolTip
End Class
