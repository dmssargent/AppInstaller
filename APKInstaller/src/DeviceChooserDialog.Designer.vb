Imports System.ComponentModel
Imports MaterialSkin
Imports MaterialSkin.Controls
Imports Microsoft.VisualBasic.CompilerServices

<DesignerGenerated()> _
Partial Class DeviceChooserDialog
    Inherits Form

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
        Dim resources As ComponentResourceManager = New ComponentResourceManager(GetType(DeviceChooserDialog))
        Me.TableLayoutPanel1 = New TableLayoutPanel()
        Me.OK_Button = New MaterialRaisedButton()
        Me.Cancel_Button = New MaterialFlatButton()
        Me.lblDevices = New MaterialLabel()
        Me.lstDevices = New ListBox()
        Me.chkNoPromptSingleDevice = New MaterialCheckBox()
        Me.TableLayoutPanel1.SuspendLayout()
        Me.SuspendLayout()
        '
        'TableLayoutPanel1
        '
        resources.ApplyResources(Me.TableLayoutPanel1, "TableLayoutPanel1")
        Me.TableLayoutPanel1.Controls.Add(Me.OK_Button, 1, 0)
        Me.TableLayoutPanel1.Controls.Add(Me.Cancel_Button, 0, 0)
        Me.TableLayoutPanel1.Name = "TableLayoutPanel1"
        '
        'OK_Button
        '
        resources.ApplyResources(Me.OK_Button, "OK_Button")
        Me.OK_Button.Depth = 0
        Me.OK_Button.MouseState = MouseState.HOVER
        Me.OK_Button.Name = "OK_Button"
        Me.OK_Button.Primary = True
        '
        'Cancel_Button
        '
        resources.ApplyResources(Me.Cancel_Button, "Cancel_Button")
        Me.Cancel_Button.Depth = 0
        Me.Cancel_Button.DialogResult = DialogResult.Cancel
        Me.Cancel_Button.MouseState = MouseState.HOVER
        Me.Cancel_Button.Name = "Cancel_Button"
        Me.Cancel_Button.Primary = False
        '
        'lblDevices
        '
        resources.ApplyResources(Me.lblDevices, "lblDevices")
        Me.lblDevices.Depth = 0
        Me.lblDevices.ForeColor = Color.FromArgb(CType(CType(222, Byte), Integer), CType(CType(0, Byte), Integer), CType(CType(0, Byte), Integer), CType(CType(0, Byte), Integer))
        Me.lblDevices.MouseState = MouseState.HOVER
        Me.lblDevices.Name = "lblDevices"
        '
        'lstDevices
        '
        resources.ApplyResources(Me.lstDevices, "lstDevices")
        Me.lstDevices.FormattingEnabled = True
        Me.lstDevices.Name = "lstDevices"
        '
        'chkNoPromptSingleDevice
        '
        resources.ApplyResources(Me.chkNoPromptSingleDevice, "chkNoPromptSingleDevice")
        Me.chkNoPromptSingleDevice.Depth = 0
        Me.chkNoPromptSingleDevice.MouseLocation = New Point(-1, -1)
        Me.chkNoPromptSingleDevice.MouseState = MouseState.HOVER
        Me.chkNoPromptSingleDevice.Name = "chkNoPromptSingleDevice"
        Me.chkNoPromptSingleDevice.Ripple = True
        Me.chkNoPromptSingleDevice.UseVisualStyleBackColor = True
        '
        'DeviceChooserDialog
        '
        Me.AcceptButton = Me.OK_Button
        resources.ApplyResources(Me, "$this")
        Me.AutoScaleMode = AutoScaleMode.Font
        Me.BackColor = Color.White
        Me.CancelButton = Me.Cancel_Button
        Me.ControlBox = False
        Me.Controls.Add(Me.chkNoPromptSingleDevice)
        Me.Controls.Add(Me.lstDevices)
        Me.Controls.Add(Me.lblDevices)
        Me.Controls.Add(Me.TableLayoutPanel1)
        Me.FormBorderStyle = FormBorderStyle.FixedToolWindow
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "DeviceChooserDialog"
        Me.TopMost = True
        Me.TableLayoutPanel1.ResumeLayout(False)
        Me.TableLayoutPanel1.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents TableLayoutPanel1 As TableLayoutPanel
    Friend WithEvents OK_Button As MaterialRaisedButton
    Friend WithEvents Cancel_Button As MaterialFlatButton
    Friend WithEvents lblDevices As MaterialLabel
    Friend WithEvents lstDevices As ListBox
    Friend WithEvents chkNoPromptSingleDevice As MaterialCheckBox
End Class
