Imports MaterialSkin.Controls

<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class About
    Inherits MaterialForm

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

    Friend WithEvents TableLayoutPanel As System.Windows.Forms.TableLayoutPanel
    Friend WithEvents TextBoxDescription As System.Windows.Forms.TextBox

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(About))
        Me.TableLayoutPanel = New System.Windows.Forms.TableLayoutPanel()
        Me.LabelCopyright = New MaterialSkin.Controls.MaterialLabel()
        Me.LabelCompanyName = New MaterialSkin.Controls.MaterialLabel()
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.lblUpdateStatus = New MaterialSkin.Controls.MaterialLabel()
        Me.chkPrerelease = New MaterialSkin.Controls.MaterialCheckBox()
        Me.LabelVersion = New MaterialSkin.Controls.MaterialLabel()
        Me.Panel2 = New System.Windows.Forms.Panel()
        Me.TextBoxDescription = New System.Windows.Forms.TextBox()
        Me.OKButton = New MaterialSkin.Controls.MaterialRaisedButton()
        Me.LogoPictureBox = New System.Windows.Forms.PictureBox()
        Me.TableLayoutPanel.SuspendLayout()
        Me.Panel1.SuspendLayout()
        Me.Panel2.SuspendLayout()
        CType(Me.LogoPictureBox, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'TableLayoutPanel
        '
        Me.TableLayoutPanel.BackColor = System.Drawing.Color.White
        Me.TableLayoutPanel.ColumnCount = 2
        Me.TableLayoutPanel.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 32.29927!))
        Me.TableLayoutPanel.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 67.70073!))
        Me.TableLayoutPanel.Controls.Add(Me.LabelCopyright, 1, 1)
        Me.TableLayoutPanel.Controls.Add(Me.LabelCompanyName, 1, 2)
        Me.TableLayoutPanel.Controls.Add(Me.Panel1, 1, 0)
        Me.TableLayoutPanel.Controls.Add(Me.Panel2, 1, 3)
        Me.TableLayoutPanel.Controls.Add(Me.LogoPictureBox, 0, 3)
        Me.TableLayoutPanel.Location = New System.Drawing.Point(12, 85)
        Me.TableLayoutPanel.Margin = New System.Windows.Forms.Padding(4)
        Me.TableLayoutPanel.Name = "TableLayoutPanel"
        Me.TableLayoutPanel.RowCount = 5
        Me.TableLayoutPanel.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 27.75665!))
        Me.TableLayoutPanel.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 9.125475!))
        Me.TableLayoutPanel.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 10.26616!))
        Me.TableLayoutPanel.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 52.76753!))
        Me.TableLayoutPanel.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me.TableLayoutPanel.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
        Me.TableLayoutPanel.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
        Me.TableLayoutPanel.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
        Me.TableLayoutPanel.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
        Me.TableLayoutPanel.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
        Me.TableLayoutPanel.Size = New System.Drawing.Size(528, 265)
        Me.TableLayoutPanel.TabIndex = 0
        '
        'LabelCopyright
        '
        Me.LabelCopyright.Depth = 0
        Me.LabelCopyright.Dock = System.Windows.Forms.DockStyle.Fill
        Me.LabelCopyright.Font = New System.Drawing.Font("Roboto", 11.0!)
        Me.LabelCopyright.ForeColor = System.Drawing.Color.FromArgb(CType(CType(222, Byte), Integer), CType(CType(0, Byte), Integer), CType(CType(0, Byte), Integer), CType(CType(0, Byte), Integer))
        Me.LabelCopyright.Location = New System.Drawing.Point(178, 73)
        Me.LabelCopyright.Margin = New System.Windows.Forms.Padding(8, 0, 4, 0)
        Me.LabelCopyright.MaximumSize = New System.Drawing.Size(0, 23)
        Me.LabelCopyright.MouseState = MaterialSkin.MouseState.HOVER
        Me.LabelCopyright.Name = "LabelCopyright"
        Me.LabelCopyright.Size = New System.Drawing.Size(346, 23)
        Me.LabelCopyright.TabIndex = 0
        Me.LabelCopyright.Text = "Copyright"
        Me.LabelCopyright.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'LabelCompanyName
        '
        Me.LabelCompanyName.Depth = 0
        Me.LabelCompanyName.Dock = System.Windows.Forms.DockStyle.Fill
        Me.LabelCompanyName.Font = New System.Drawing.Font("Roboto", 11.0!)
        Me.LabelCompanyName.ForeColor = System.Drawing.Color.FromArgb(CType(CType(222, Byte), Integer), CType(CType(0, Byte), Integer), CType(CType(0, Byte), Integer), CType(CType(0, Byte), Integer))
        Me.LabelCompanyName.Location = New System.Drawing.Point(178, 97)
        Me.LabelCompanyName.Margin = New System.Windows.Forms.Padding(8, 0, 4, 0)
        Me.LabelCompanyName.MaximumSize = New System.Drawing.Size(0, 23)
        Me.LabelCompanyName.MouseState = MaterialSkin.MouseState.HOVER
        Me.LabelCompanyName.Name = "LabelCompanyName"
        Me.LabelCompanyName.Size = New System.Drawing.Size(346, 23)
        Me.LabelCompanyName.TabIndex = 0
        Me.LabelCompanyName.Text = "Company Name"
        Me.LabelCompanyName.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Panel1
        '
        Me.Panel1.Controls.Add(Me.lblUpdateStatus)
        Me.Panel1.Controls.Add(Me.chkPrerelease)
        Me.Panel1.Controls.Add(Me.LabelVersion)
        Me.Panel1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Panel1.Location = New System.Drawing.Point(173, 3)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(352, 67)
        Me.Panel1.TabIndex = 3
        '
        'lblUpdateStatus
        '
        Me.lblUpdateStatus.AutoSize = True
        Me.lblUpdateStatus.Depth = 0
        Me.lblUpdateStatus.Font = New System.Drawing.Font("Roboto", 11.0!)
        Me.lblUpdateStatus.ForeColor = System.Drawing.Color.FromArgb(CType(CType(222, Byte), Integer), CType(CType(0, Byte), Integer), CType(CType(0, Byte), Integer), CType(CType(0, Byte), Integer))
        Me.lblUpdateStatus.Location = New System.Drawing.Point(6, 36)
        Me.lblUpdateStatus.MouseState = MaterialSkin.MouseState.HOVER
        Me.lblUpdateStatus.Name = "lblUpdateStatus"
        Me.lblUpdateStatus.Size = New System.Drawing.Size(207, 24)
        Me.lblUpdateStatus.TabIndex = 3
        Me.lblUpdateStatus.Text = "Everything is up-to-date"
        '
        'chkPrerelease
        '
        Me.chkPrerelease.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.chkPrerelease.AutoSize = True
        Me.chkPrerelease.Depth = 0
        Me.chkPrerelease.Font = New System.Drawing.Font("Roboto", 10.0!)
        Me.chkPrerelease.Location = New System.Drawing.Point(195, 1)
        Me.chkPrerelease.Margin = New System.Windows.Forms.Padding(0)
        Me.chkPrerelease.MouseLocation = New System.Drawing.Point(-1, -1)
        Me.chkPrerelease.MouseState = MaterialSkin.MouseState.HOVER
        Me.chkPrerelease.Name = "chkPrerelease"
        Me.chkPrerelease.Ripple = True
        Me.chkPrerelease.Size = New System.Drawing.Size(156, 30)
        Me.chkPrerelease.TabIndex = 2
        Me.chkPrerelease.Text = "Use Prereleases"
        Me.chkPrerelease.UseVisualStyleBackColor = True
        '
        'LabelVersion
        '
        Me.LabelVersion.Depth = 0
        Me.LabelVersion.Font = New System.Drawing.Font("Roboto", 11.0!)
        Me.LabelVersion.ForeColor = System.Drawing.Color.FromArgb(CType(CType(222, Byte), Integer), CType(CType(0, Byte), Integer), CType(CType(0, Byte), Integer), CType(CType(0, Byte), Integer))
        Me.LabelVersion.Location = New System.Drawing.Point(5, 6)
        Me.LabelVersion.Margin = New System.Windows.Forms.Padding(8, 0, 4, 0)
        Me.LabelVersion.MaximumSize = New System.Drawing.Size(222, 21)
        Me.LabelVersion.MouseState = MaterialSkin.MouseState.HOVER
        Me.LabelVersion.Name = "LabelVersion"
        Me.LabelVersion.Size = New System.Drawing.Size(160, 21)
        Me.LabelVersion.TabIndex = 0
        Me.LabelVersion.Text = "Version"
        Me.LabelVersion.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Panel2
        '
        Me.Panel2.Controls.Add(Me.TextBoxDescription)
        Me.Panel2.Controls.Add(Me.OKButton)
        Me.Panel2.Location = New System.Drawing.Point(173, 127)
        Me.Panel2.Name = "Panel2"
        Me.Panel2.Size = New System.Drawing.Size(352, 133)
        Me.Panel2.TabIndex = 4
        '
        'TextBoxDescription
        '
        Me.TextBoxDescription.BackColor = System.Drawing.Color.White
        Me.TextBoxDescription.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.TextBoxDescription.Cursor = System.Windows.Forms.Cursors.Arrow
        Me.TextBoxDescription.Enabled = False
        Me.TextBoxDescription.Location = New System.Drawing.Point(7, 4)
        Me.TextBoxDescription.Margin = New System.Windows.Forms.Padding(8, 4, 4, 4)
        Me.TextBoxDescription.Multiline = True
        Me.TextBoxDescription.Name = "TextBoxDescription"
        Me.TextBoxDescription.ReadOnly = True
        Me.TextBoxDescription.Size = New System.Drawing.Size(338, 77)
        Me.TextBoxDescription.TabIndex = 0
        Me.TextBoxDescription.TabStop = False
        Me.TextBoxDescription.Text = resources.GetString("TextBoxDescription.Text")
        '
        'OKButton
        '
        Me.OKButton.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.OKButton.Depth = 0
        Me.OKButton.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.OKButton.Location = New System.Drawing.Point(247, 104)
        Me.OKButton.Margin = New System.Windows.Forms.Padding(4)
        Me.OKButton.MouseState = MaterialSkin.MouseState.HOVER
        Me.OKButton.Name = "OKButton"
        Me.OKButton.Primary = True
        Me.OKButton.Size = New System.Drawing.Size(100, 25)
        Me.OKButton.TabIndex = 0
        Me.OKButton.Text = "OK"
        '
        'LogoPictureBox
        '
        Me.LogoPictureBox.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.LogoPictureBox.Image = Global.APKInstaller.My.Resources.Resources.FIRSTTech_IconVert_RGB
        Me.LogoPictureBox.Location = New System.Drawing.Point(4, 128)
        Me.LogoPictureBox.Margin = New System.Windows.Forms.Padding(4)
        Me.LogoPictureBox.Name = "LogoPictureBox"
        Me.LogoPictureBox.Size = New System.Drawing.Size(162, 131)
        Me.LogoPictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom
        Me.LogoPictureBox.TabIndex = 1
        Me.LogoPictureBox.TabStop = False
        '
        'About
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 16.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.CancelButton = Me.OKButton
        Me.ClientSize = New System.Drawing.Size(552, 358)
        Me.Controls.Add(Me.TableLayoutPanel)
        Me.Margin = New System.Windows.Forms.Padding(4)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "About"
        Me.Padding = New System.Windows.Forms.Padding(12, 11, 12, 11)
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "About"
        Me.TableLayoutPanel.ResumeLayout(False)
        Me.Panel1.ResumeLayout(False)
        Me.Panel1.PerformLayout()
        Me.Panel2.ResumeLayout(False)
        Me.Panel2.PerformLayout()
        CType(Me.LogoPictureBox, System.ComponentModel.ISupportInitialize).EndInit()
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
