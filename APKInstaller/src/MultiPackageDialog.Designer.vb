Imports System.ComponentModel
Imports MaterialSkin
Imports MaterialSkin.Controls
Imports Microsoft.VisualBasic.CompilerServices

<DesignerGenerated()>
Partial Class MultiPackageDialog
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
        Dim resources As ComponentResourceManager = New ComponentResourceManager(GetType(MultiPackageDialog))
        Me.lstFiles = New ListBox()
        Me.btnAdd = New MaterialFlatButton()
        Me.txtFile = New MaterialSingleLineTextField()
        Me.lblFile = New MaterialLabel()
        Me.lblFiles = New MaterialLabel()
        Me.btnDelete = New MaterialFlatButton()
        Me.btnOk = New MaterialRaisedButton()
        Me.btnCancel = New MaterialFlatButton()
        Me.btnBrowse = New MaterialFlatButton()
        Me.btnModify = New MaterialFlatButton()
        Me.SuspendLayout()
        '
        'lstFiles
        '
        resources.ApplyResources(Me.lstFiles, "lstFiles")
        Me.lstFiles.FormattingEnabled = True
        Me.lstFiles.Name = "lstFiles"
        '
        'btnAdd
        '
        resources.ApplyResources(Me.btnAdd, "btnAdd")
        Me.btnAdd.Depth = 0
        Me.btnAdd.MouseState = MouseState.HOVER
        Me.btnAdd.Name = "btnAdd"
        Me.btnAdd.Primary = False
        Me.btnAdd.UseVisualStyleBackColor = True
        '
        'txtFile
        '
        resources.ApplyResources(Me.txtFile, "txtFile")
        Me.txtFile.Depth = 0
        Me.txtFile.Hint = ""
        Me.txtFile.MaxLength = 32767
        Me.txtFile.MouseState = MouseState.HOVER
        Me.txtFile.Name = "txtFile"
        Me.txtFile.PasswordChar = ChrW(0)
        Me.txtFile.SelectedText = ""
        Me.txtFile.SelectionLength = 0
        Me.txtFile.SelectionStart = 0
        Me.txtFile.TabStop = False
        Me.txtFile.UseSystemPasswordChar = False
        '
        'lblFile
        '
        resources.ApplyResources(Me.lblFile, "lblFile")
        Me.lblFile.Depth = 0
        Me.lblFile.ForeColor = Color.FromArgb(CType(CType(222, Byte), Integer), CType(CType(0, Byte), Integer), CType(CType(0, Byte), Integer), CType(CType(0, Byte), Integer))
        Me.lblFile.MouseState = MouseState.HOVER
        Me.lblFile.Name = "lblFile"
        '
        'lblFiles
        '
        resources.ApplyResources(Me.lblFiles, "lblFiles")
        Me.lblFiles.Depth = 0
        Me.lblFiles.ForeColor = Color.FromArgb(CType(CType(222, Byte), Integer), CType(CType(0, Byte), Integer), CType(CType(0, Byte), Integer), CType(CType(0, Byte), Integer))
        Me.lblFiles.MouseState = MouseState.HOVER
        Me.lblFiles.Name = "lblFiles"
        '
        'btnDelete
        '
        resources.ApplyResources(Me.btnDelete, "btnDelete")
        Me.btnDelete.Depth = 0
        Me.btnDelete.MouseState = MouseState.HOVER
        Me.btnDelete.Name = "btnDelete"
        Me.btnDelete.Primary = False
        Me.btnDelete.UseVisualStyleBackColor = True
        '
        'btnOk
        '
        resources.ApplyResources(Me.btnOk, "btnOk")
        Me.btnOk.Depth = 0
        Me.btnOk.MouseState = MouseState.HOVER
        Me.btnOk.Name = "btnOk"
        Me.btnOk.Primary = True
        Me.btnOk.UseVisualStyleBackColor = True
        '
        'btnCancel
        '
        resources.ApplyResources(Me.btnCancel, "btnCancel")
        Me.btnCancel.Depth = 0
        Me.btnCancel.DialogResult = DialogResult.Yes
        Me.btnCancel.MouseState = MouseState.HOVER
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.Primary = False
        Me.btnCancel.UseVisualStyleBackColor = True
        '
        'btnBrowse
        '
        resources.ApplyResources(Me.btnBrowse, "btnBrowse")
        Me.btnBrowse.Depth = 0
        Me.btnBrowse.MouseState = MouseState.HOVER
        Me.btnBrowse.Name = "btnBrowse"
        Me.btnBrowse.Primary = False
        Me.btnBrowse.UseVisualStyleBackColor = True
        '
        'btnModify
        '
        resources.ApplyResources(Me.btnModify, "btnModify")
        Me.btnModify.Depth = 0
        Me.btnModify.MouseState = MouseState.HOVER
        Me.btnModify.Name = "btnModify"
        Me.btnModify.Primary = False
        Me.btnModify.UseVisualStyleBackColor = True
        '
        'MultiPackageDialog
        '
        Me.AcceptButton = Me.btnOk
        resources.ApplyResources(Me, "$this")
        Me.AutoScaleMode = AutoScaleMode.Font
        Me.CancelButton = Me.btnCancel
        Me.ControlBox = False
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
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "MultiPackageDialog"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents lstFiles As ListBox
    Friend WithEvents btnAdd As MaterialFlatButton
    Friend WithEvents txtFile As MaterialSingleLineTextField
    Friend WithEvents lblFile As MaterialLabel
    Friend WithEvents lblFiles As MaterialLabel
    Friend WithEvents btnDelete As MaterialFlatButton
    Friend WithEvents btnOk As MaterialRaisedButton
    Friend WithEvents btnCancel As MaterialFlatButton
    Friend WithEvents btnBrowse As MaterialFlatButton
    Friend WithEvents btnModify As MaterialFlatButton
End Class
