Imports System.ComponentModel
Imports MaterialSkin
Imports MaterialSkin.Controls
Imports Microsoft.VisualBasic.CompilerServices

<DesignerGenerated()>
Partial Class About
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

    Friend WithEvents TableLayoutPanel As TableLayoutPanel
    Friend WithEvents TextBoxDescription As TextBox

    'Required by the Windows Form Designer
    Private components As IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Dim resources As ComponentResourceManager = New ComponentResourceManager(GetType(About))
        Me.TableLayoutPanel = New TableLayoutPanel()
        Me.LabelCopyright = New MaterialLabel()
        Me.LabelCompanyName = New MaterialLabel()
        Me.Panel1 = New Panel()
        Me.lblUpdateStatus = New MaterialLabel()
        Me.chkPrerelease = New MaterialCheckBox()
        Me.LabelVersion = New MaterialLabel()
        Me.Panel2 = New Panel()
        Me.TextBoxDescription = New TextBox()
        Me.OKButton = New MaterialRaisedButton()
        Me.LogoPictureBox = New PictureBox()
        Me.TableLayoutPanel.SuspendLayout()
        Me.Panel1.SuspendLayout()
        Me.Panel2.SuspendLayout()
        CType(Me.LogoPictureBox, ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'TableLayoutPanel
        '
        Me.TableLayoutPanel.BackColor = Color.White
        resources.ApplyResources(Me.TableLayoutPanel, "TableLayoutPanel")
        Me.TableLayoutPanel.Controls.Add(Me.LabelCopyright, 1, 1)
        Me.TableLayoutPanel.Controls.Add(Me.LabelCompanyName, 1, 2)
        Me.TableLayoutPanel.Controls.Add(Me.Panel1, 1, 0)
        Me.TableLayoutPanel.Controls.Add(Me.Panel2, 1, 3)
        Me.TableLayoutPanel.Controls.Add(Me.LogoPictureBox, 0, 3)
        Me.TableLayoutPanel.Name = "TableLayoutPanel"
        '
        'LabelCopyright
        '
        Me.LabelCopyright.Depth = 0
        resources.ApplyResources(Me.LabelCopyright, "LabelCopyright")
        Me.LabelCopyright.ForeColor = Color.FromArgb(CType(CType(222, Byte), Integer), CType(CType(0, Byte), Integer), CType(CType(0, Byte), Integer), CType(CType(0, Byte), Integer))
        Me.LabelCopyright.MouseState = MouseState.HOVER
        Me.LabelCopyright.Name = "LabelCopyright"
        '
        'LabelCompanyName
        '
        Me.LabelCompanyName.Depth = 0
        resources.ApplyResources(Me.LabelCompanyName, "LabelCompanyName")
        Me.LabelCompanyName.ForeColor = Color.FromArgb(CType(CType(222, Byte), Integer), CType(CType(0, Byte), Integer), CType(CType(0, Byte), Integer), CType(CType(0, Byte), Integer))
        Me.LabelCompanyName.MouseState = MouseState.HOVER
        Me.LabelCompanyName.Name = "LabelCompanyName"
        '
        'Panel1
        '
        Me.Panel1.Controls.Add(Me.lblUpdateStatus)
        Me.Panel1.Controls.Add(Me.chkPrerelease)
        Me.Panel1.Controls.Add(Me.LabelVersion)
        resources.ApplyResources(Me.Panel1, "Panel1")
        Me.Panel1.Name = "Panel1"
        '
        'lblUpdateStatus
        '
        resources.ApplyResources(Me.lblUpdateStatus, "lblUpdateStatus")
        Me.lblUpdateStatus.Depth = 0
        Me.lblUpdateStatus.ForeColor = Color.FromArgb(CType(CType(222, Byte), Integer), CType(CType(0, Byte), Integer), CType(CType(0, Byte), Integer), CType(CType(0, Byte), Integer))
        Me.lblUpdateStatus.MouseState = MouseState.HOVER
        Me.lblUpdateStatus.Name = "lblUpdateStatus"
        '
        'chkPrerelease
        '
        resources.ApplyResources(Me.chkPrerelease, "chkPrerelease")
        Me.chkPrerelease.Depth = 0
        Me.chkPrerelease.MouseLocation = New Point(-1, -1)
        Me.chkPrerelease.MouseState = MouseState.HOVER
        Me.chkPrerelease.Name = "chkPrerelease"
        Me.chkPrerelease.Ripple = True
        Me.chkPrerelease.UseVisualStyleBackColor = True
        '
        'LabelVersion
        '
        Me.LabelVersion.Depth = 0
        resources.ApplyResources(Me.LabelVersion, "LabelVersion")
        Me.LabelVersion.ForeColor = Color.FromArgb(CType(CType(222, Byte), Integer), CType(CType(0, Byte), Integer), CType(CType(0, Byte), Integer), CType(CType(0, Byte), Integer))
        Me.LabelVersion.MouseState = MouseState.HOVER
        Me.LabelVersion.Name = "LabelVersion"
        '
        'Panel2
        '
        Me.Panel2.Controls.Add(Me.TextBoxDescription)
        Me.Panel2.Controls.Add(Me.OKButton)
        resources.ApplyResources(Me.Panel2, "Panel2")
        Me.Panel2.Name = "Panel2"
        '
        'TextBoxDescription
        '
        Me.TextBoxDescription.BackColor = Color.White
        Me.TextBoxDescription.BorderStyle = BorderStyle.None
        Me.TextBoxDescription.Cursor = Cursors.Arrow
        resources.ApplyResources(Me.TextBoxDescription, "TextBoxDescription")
        Me.TextBoxDescription.Name = "TextBoxDescription"
        Me.TextBoxDescription.ReadOnly = True
        Me.TextBoxDescription.TabStop = False
        '
        'OKButton
        '
        resources.ApplyResources(Me.OKButton, "OKButton")
        Me.OKButton.Depth = 0
        Me.OKButton.DialogResult = DialogResult.Cancel
        Me.OKButton.MouseState = MouseState.HOVER
        Me.OKButton.Name = "OKButton"
        Me.OKButton.Primary = True
        '
        'LogoPictureBox
        '
        resources.ApplyResources(Me.LogoPictureBox, "LogoPictureBox")
        Me.LogoPictureBox.Image = My.Resources.Resources.FIRSTTech_IconVert_RGB
        Me.LogoPictureBox.Name = "LogoPictureBox"
        Me.LogoPictureBox.TabStop = False
        '
        'About
        '
        resources.ApplyResources(Me, "$this")
        Me.AutoScaleMode = AutoScaleMode.Font
        Me.CancelButton = Me.OKButton
        Me.Controls.Add(Me.TableLayoutPanel)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "About"
        Me.ShowInTaskbar = False
        Me.TableLayoutPanel.ResumeLayout(False)
        Me.Panel1.ResumeLayout(False)
        Me.Panel1.PerformLayout()
        Me.Panel2.ResumeLayout(False)
        Me.Panel2.PerformLayout()
        CType(Me.LogoPictureBox, ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents LabelVersion As MaterialLabel
    Friend WithEvents LabelCompanyName As MaterialLabel
    Friend WithEvents OKButton As MaterialRaisedButton
    Friend WithEvents LabelCopyright As MaterialLabel
    'Friend WithEvents MaterialLabel1 As MaterialLabel
    Friend WithEvents Panel1 As Panel
    'Friend WithEvents MaterialLabel3 As MaterialLabel
    Friend WithEvents Label1 As MaterialLabel
    Friend WithEvents chkPrerelease As MaterialCheckBox
    Friend WithEvents lblUpdateStatus As MaterialLabel
    Friend WithEvents LogoPictureBox As PictureBox
    Friend WithEvents Panel2 As Panel
End Class
