Imports System.IO
Imports APKInstaller.My.Resources
Imports MaterialSkin

Public Class MultiPackageDialog
    Private ReadOnly _files As String()
    Private _modifying As Boolean

    Private Sub New(files As String())

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        _files = files
    End Sub


    Public Shared Function Create(files As String()) As MultiPackageDialog
        Return New MultiPackageDialog(files)
    End Function

    Private Sub MultiPackageDialog_Load(sender As Object, e As EventArgs) Handles Me.Load
        lstFiles.Items.AddRange(_files.ToArray)

        'Configure GUI
        Dim manager = MaterialSkinManager.Instance
        manager.AddFormToManage(Me)
        'SkinManager.Theme = MaterialSkinManager.Themes.LIGHT
        'S 'kinManager.ColorScheme = New ColorScheme(Primary.Red800, Primary.Red800, Primary.Red200, Accent.LightBlue200, TextShade.WHITE)
        CenterToScreen()

        btnDelete.Enabled = False
        btnModify.Enabled = False
        lstFiles.DrawMode = DrawMode.OwnerDrawFixed
    End Sub

    Function GetFiles() As String()
        Dim list(lstFiles.Items.Count) As String
        lstFiles.Items.CopyTo(list, 0)
        Return list
    End Function

    Private Sub btnOk_Click(sender As Object, e As EventArgs) Handles btnOk.Click
        For Each item In lstFiles.Items
            If Not Installer.ValidateFile(item.ToString(), True) Then
                If MsgBox("An invalid or nonexistent APK file was found. If you continue, the file may not be installed. Do you want to continue?", CType(MsgBoxStyle.YesNo + MsgBoxStyle.Exclamation, Global.Microsoft.VisualBasic.MsgBoxStyle)) = MsgBoxResult.No Then
                    Exit Sub
                End If
            End If
        Next
        DialogResult = DialogResult.OK
        Close()
    End Sub

    Private Sub btnCancel_Click(sender As Object, e As EventArgs) Handles btnCancel.Click
        If MsgBox("Are you really sure you want to cancel? None of the changes made will be preserved.", CType(MsgBoxStyle.Exclamation + MsgBoxStyle.YesNo, Global.Microsoft.VisualBasic.MsgBoxStyle)) = MsgBoxResult.Yes Then
            DialogResult = DialogResult.Cancel
            Close()
        End If
    End Sub

    Private Sub lstFiles_SelectedIndexChanged(sender As Object, e As EventArgs) Handles lstFiles.SelectedIndexChanged
        If lstFiles.SelectedIndex >= 0 Then
            btnModify.Enabled = True
            btnDelete.Enabled = True
        Else
            btnModify.Enabled = False
            btnDelete.Enabled = False
        End If

        'modifying = True
    End Sub

    Private Sub btnAdd_Click(sender As Object, e As EventArgs) Handles btnAdd.Click
        If _modifying And lstFiles.SelectedIndex >= 0 Then

            'btnAdd.Text = "Add"
            'modifying = False
            'tnBrowse.Visible = True
        End If

        lstFiles.Items.Add(txtFile.Text)
        lstFiles.SelectedIndex = lstFiles.Items.Count - 1
        lstFiles.Enabled = True

        txtFile.Text = ""
    End Sub

    Private Sub btnDelete_Click(sender As Object, e As EventArgs) Handles btnDelete.Click
        lstFiles.Items.RemoveAt(lstFiles.SelectedIndex)
    End Sub


    Private Sub listBox1_DrawItem(sender As Object, e As DrawItemEventArgs) Handles lstFiles.DrawItem
        e.DrawBackground()

        Dim i = 0
        Dim g = lstFiles.CreateGraphics()
        For Each item In lstFiles.Items
            Dim sizeF = g.MeasureString(item.ToString, lstFiles.Font)
            If sizeF.Width > i Then
                i = CInt(sizeF.Width)
            End If
        Next
        lstFiles.HorizontalExtent = i

        Using b As New SolidBrush(e.ForeColor)
            If e.Index >= 0 Then
                Dim itemText = lstFiles.GetItemText(lstFiles.Items(e.Index))
                'If Not Installer.ValidateFile(itemText) Then
                '    e.Graphics.FillRectangle(Brushes.OrangeRed, e.Bounds)
                'End If
                If (e.State And DrawItemState.Selected) = DrawItemState.Selected Then
                    e.Graphics.FillRectangle(MaterialSkinManager.Instance.ColorScheme.AccentBrush, e.Bounds)
                End If
                e.Graphics.DrawString(itemText, e.Font, b, e.Bounds)
                End If
        End Using

        e.DrawFocusRectangle()
    End Sub

    Private Sub btnModify_Click(sender As Object, e As EventArgs) Handles btnModify.Click
        If lstFiles.SelectedIndex >= 0 Then
            If _modifying Then
                lstFiles.Items.RemoveAt(lstFiles.SelectedIndex)
                lstFiles.Items.Add(txtFile.Text)
                lstFiles.SelectedIndex = lstFiles.Items.Count - 1

                lstFiles.Enabled = True
                btnModify.Text = Strings.Modify
                _modifying = False
            Else
                txtFile.Text = lstFiles.SelectedItem.ToString
                lstFiles.Enabled = False
                btnModify.Text = Strings.Confirm
                _modifying = True
            End If
        End If
    End Sub

    Private Sub btnBrowse_Click(sender As Object, e As EventArgs) Handles btnBrowse.Click
        Using fileDialog = New OpenFileDialog()

            fileDialog.AutoUpgradeEnabled = True
            fileDialog.CheckFileExists = True
            fileDialog.DefaultExt = ".apk"
            fileDialog.Title = Strings.openFileDialogTitle
            fileDialog.Multiselect = False
            fileDialog.ValidateNames = True
            fileDialog.Filter = Strings.openFileDialogFilter
            fileDialog.ShowDialog()

            txtFile.Text = fileDialog.FileName
        End Using
    End Sub

    Private Sub txtFile_TextChanged(sender As Object, e As EventArgs) Handles txtFile.TextChanged
        Dim validateFile = Installer.ValidateFile(txtFile.Text)
        btnAdd.Enabled = validateFile
        If _modifying Then
            btnModify.Enabled = validateFile
        End If
    End Sub
End Class