Imports MaterialSkin.Controls

<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class MultiPackageDialog
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

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(MultiPackageDialog))
        Me.lstFiles = New System.Windows.Forms.ListBox()
        Me.btnAdd = New MaterialSkin.Controls.MaterialFlatButton()
        Me.txtFile = New MaterialSkin.Controls.MaterialSingleLineTextField()
        Me.lblFile = New MaterialSkin.Controls.MaterialLabel()
        Me.lblFiles = New MaterialSkin.Controls.MaterialLabel()
        Me.btnDelete = New MaterialSkin.Controls.MaterialFlatButton()
        Me.btnOk = New MaterialSkin.Controls.MaterialRaisedButton()
        Me.btnCancel = New MaterialSkin.Controls.MaterialFlatButton()
        Me.btnBrowse = New MaterialSkin.Controls.MaterialFlatButton()
        Me.btnModify = New MaterialSkin.Controls.MaterialFlatButton()
        Me.SuspendLayout()
        '
        'lstFiles
        '
        Me.lstFiles.FormattingEnabled = True
        Me.lstFiles.HorizontalScrollbar = True
        Me.lstFiles.ItemHeight = 16
        Me.lstFiles.Location = New System.Drawing.Point(12, 156)
        Me.lstFiles.Name = "lstFiles"
        Me.lstFiles.Size = New System.Drawing.Size(391, 276)
        Me.lstFiles.TabIndex = 0
        '
        'btnAdd
        '
        Me.btnAdd.AutoSize = True
        Me.btnAdd.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        Me.btnAdd.Depth = 0
        Me.btnAdd.Location = New System.Drawing.Point(410, 156)
        Me.btnAdd.Margin = New System.Windows.Forms.Padding(4, 6, 4, 6)
        Me.btnAdd.MouseState = MaterialSkin.MouseState.HOVER
        Me.btnAdd.Name = "btnAdd"
        Me.btnAdd.Primary = False
        Me.btnAdd.Size = New System.Drawing.Size(47, 36)
        Me.btnAdd.TabIndex = 1
        Me.btnAdd.Text = "Add"
        Me.btnAdd.UseVisualStyleBackColor = True
        '
        'txtFile
        '
        Me.txtFile.Depth = 0
        Me.txtFile.Hint = ""
        Me.txtFile.Location = New System.Drawing.Point(101, 93)
        Me.txtFile.MaxLength = 32767
        Me.txtFile.MouseState = MaterialSkin.MouseState.HOVER
        Me.txtFile.Name = "txtFile"
        Me.txtFile.PasswordChar = Global.Microsoft.VisualBasic.ChrW(0)
        Me.txtFile.SelectedText = ""
        Me.txtFile.SelectionLength = 0
        Me.txtFile.SelectionStart = 0
        Me.txtFile.Size = New System.Drawing.Size(393, 28)
        Me.txtFile.TabIndex = 3
        Me.txtFile.TabStop = False
        Me.txtFile.UseSystemPasswordChar = False
        '
        'lblFile
        '
        Me.lblFile.AutoSize = True
        Me.lblFile.Depth = 0
        Me.lblFile.Font = New System.Drawing.Font("Roboto", 11.0!)
        Me.lblFile.ForeColor = System.Drawing.Color.FromArgb(CType(CType(222, Byte), Integer), CType(CType(0, Byte), Integer), CType(CType(0, Byte), Integer), CType(CType(0, Byte), Integer))
        Me.lblFile.Location = New System.Drawing.Point(8, 93)
        Me.lblFile.MouseState = MaterialSkin.MouseState.HOVER
        Me.lblFile.Name = "lblFile"
        Me.lblFile.Size = New System.Drawing.Size(87, 24)
        Me.lblFile.TabIndex = 4
        Me.lblFile.Text = "APK File:"
        '
        'lblFiles
        '
        Me.lblFiles.AutoSize = True
        Me.lblFiles.Depth = 0
        Me.lblFiles.Font = New System.Drawing.Font("Roboto", 11.0!)
        Me.lblFiles.ForeColor = System.Drawing.Color.FromArgb(CType(CType(222, Byte), Integer), CType(CType(0, Byte), Integer), CType(CType(0, Byte), Integer), CType(CType(0, Byte), Integer))
        Me.lblFiles.Location = New System.Drawing.Point(8, 128)
        Me.lblFiles.MouseState = MaterialSkin.MouseState.HOVER
        Me.lblFiles.Name = "lblFiles"
        Me.lblFiles.Size = New System.Drawing.Size(51, 24)
        Me.lblFiles.TabIndex = 5
        Me.lblFiles.Text = "Files"
        '
        'btnDelete
        '
        Me.btnDelete.AutoSize = True
        Me.btnDelete.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        Me.btnDelete.Depth = 0
        Me.btnDelete.Location = New System.Drawing.Point(410, 252)
        Me.btnDelete.Margin = New System.Windows.Forms.Padding(4, 6, 4, 6)
        Me.btnDelete.MouseState = MaterialSkin.MouseState.HOVER
        Me.btnDelete.Name = "btnDelete"
        Me.btnDelete.Primary = False
        Me.btnDelete.Size = New System.Drawing.Size(73, 36)
        Me.btnDelete.TabIndex = 6
        Me.btnDelete.Text = "Delete"
        Me.btnDelete.UseVisualStyleBackColor = True
        '
        'btnOk
        '
        Me.btnOk.Depth = 0
        Me.btnOk.Location = New System.Drawing.Point(417, 450)
        Me.btnOk.MouseState = MaterialSkin.MouseState.HOVER
        Me.btnOk.Name = "btnOk"
        Me.btnOk.Primary = True
        Me.btnOk.Size = New System.Drawing.Size(75, 23)
        Me.btnOk.TabIndex = 7
        Me.btnOk.Text = "Ok"
        Me.btnOk.UseVisualStyleBackColor = True
        '
        'btnCancel
        '
        Me.btnCancel.AutoSize = True
        Me.btnCancel.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        Me.btnCancel.Depth = 0
        Me.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Yes
        Me.btnCancel.Location = New System.Drawing.Point(325, 440)
        Me.btnCancel.Margin = New System.Windows.Forms.Padding(4, 6, 4, 6)
        Me.btnCancel.MouseState = MaterialSkin.MouseState.HOVER
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.Primary = False
        Me.btnCancel.Size = New System.Drawing.Size(78, 36)
        Me.btnCancel.TabIndex = 8
        Me.btnCancel.Text = "Cancel"
        Me.btnCancel.UseVisualStyleBackColor = True
        '
        'btnBrowse
        '
        Me.btnBrowse.AutoSize = True
        Me.btnBrowse.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        Me.btnBrowse.Depth = 0
        Me.btnBrowse.Location = New System.Drawing.Point(410, 88)
        Me.btnBrowse.Margin = New System.Windows.Forms.Padding(4, 6, 4, 6)
        Me.btnBrowse.MouseState = MaterialSkin.MouseState.HOVER
        Me.btnBrowse.Name = "btnBrowse"
        Me.btnBrowse.Primary = False
        Me.btnBrowse.Size = New System.Drawing.Size(82, 36)
        Me.btnBrowse.TabIndex = 9
        Me.btnBrowse.Text = "Browse"
        Me.btnBrowse.UseVisualStyleBackColor = True
        '
        'btnModify
        '
        Me.btnModify.AutoSize = True
        Me.btnModify.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        Me.btnModify.Depth = 0
        Me.btnModify.Location = New System.Drawing.Point(410, 204)
        Me.btnModify.Margin = New System.Windows.Forms.Padding(4, 6, 4, 6)
        Me.btnModify.MouseState = MaterialSkin.MouseState.HOVER
        Me.btnModify.Name = "btnModify"
        Me.btnModify.Primary = False
        Me.btnModify.Size = New System.Drawing.Size(76, 36)
        Me.btnModify.TabIndex = 2
        Me.btnModify.Text = "Modify"
        Me.btnModify.UseVisualStyleBackColor = True
        '
        'MultiPackageDialog
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 16.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(506, 492)
        Me.Controls.Add(Me.btnAdd)
        Me.Controls.Add(Me.btnBrowse)
        Me.Controls.Add(Me.btnCancel)
        Me.Controls.Add(Me.btnOk)
        Me.Controls.Add(Me.btnDelete)
        Me.Controls.Add(Me.lblFiles)
        Me.Controls.Add(Me.lblFile)
        Me.Controls.Add(Me.txtFile)
        Me.Controls.Add(Me.btnModify)
        Me.Controls.Add(Me.lstFiles)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "MultiPackageDialog"
        Me.Text = "Choose APK Files"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents lstFiles As ListBox
    Friend WithEvents btnAdd As MaterialSkin.Controls.MaterialFlatButton
    Friend WithEvents txtFile As MaterialSkin.Controls.MaterialSingleLineTextField
    Friend WithEvents lblFile As MaterialSkin.Controls.MaterialLabel
    Friend WithEvents lblFiles As MaterialLabel
    Friend WithEvents btnDelete As MaterialFlatButton
    Friend WithEvents btnOk As MaterialRaisedButton
    Friend WithEvents btnCancel As MaterialFlatButton
    Friend WithEvents btnBrowse As MaterialFlatButton
    Friend WithEvents btnModify As MaterialFlatButton
End Class
