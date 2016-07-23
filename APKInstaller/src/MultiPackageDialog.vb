Imports MaterialSkin

Public Class MultiPackageDialog
    Private files As String()
    Private modifying As Boolean

    Private Sub New(ByVal files As String())

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        Me.files = files
    End Sub



    Public Shared Function Create(ByVal files As String()) As MultiPackageDialog
        Return New MultiPackageDialog(files)
    End Function

    Private Sub MultiPackageDialog_Load(sender As Object, e As EventArgs) Handles Me.Load
        lstFiles.Items.AddRange(files.ToArray)

        'Configure GUI
        Dim SkinManager As MaterialSkinManager = MaterialSkinManager.Instance
        SkinManager.AddFormToManage(Me)
        'SkinManager.Theme = MaterialSkinManager.Themes.LIGHT
        'S 'kinManager.ColorScheme = New ColorScheme(Primary.Red800, Primary.Red800, Primary.Red200, Accent.LightBlue200, TextShade.WHITE)
        Me.CenterToScreen()

        Me.btnDelete.Enabled = False
        btnModify.Enabled = False
        lstFiles.DrawMode = DrawMode.OwnerDrawFixed
    End Sub

    Function GetFiles() As String()
        Dim list(lstFiles.Items.Count) As String
        lstFiles.Items.CopyTo(list, 0)
        Return list
    End Function

    Private Sub btnOk_Click(sender As Object, e As EventArgs) Handles btnOk.Click
        DialogResult = DialogResult.OK
        Me.Close()
    End Sub

    Private Sub btnCancel_Click(sender As Object, e As EventArgs) Handles btnCancel.Click
        DialogResult = DialogResult.Cancel
        Me.Close()
    End Sub

    Private Sub lstFiles_SelectedIndexChanged(sender As Object, e As EventArgs) Handles lstFiles.SelectedIndexChanged
        If lstFiles.SelectedIndex >= 0 Then
            Me.btnModify.Enabled = True
            Me.btnDelete.Enabled = True
        Else
            Me.btnModify.Enabled = False
            Me.btnDelete.Enabled = False
        End If

        'modifying = True
    End Sub

    Private Sub btnAdd_Click(sender As Object, e As EventArgs) Handles btnAdd.Click
        If modifying And lstFiles.SelectedIndex >= 0 Then

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

        Dim width = 0
        Dim g = lstFiles.CreateGraphics()
        For Each item In lstFiles.Items
            Dim size = g.MeasureString(item.ToString, lstFiles.Font)
            If size.Width > width Then
                width = CInt(size.Width)
            End If
        Next
        lstFiles.HorizontalExtent = width
        If (e.State And DrawItemState.Selected) = DrawItemState.Selected Then
            e.Graphics.FillRectangle(MaterialSkin.MaterialSkinManager.Instance.ColorScheme.AccentBrush, e.Bounds)
        End If
        Using b As New SolidBrush(e.ForeColor)
            If e.Index >= 0 Then
                e.Graphics.DrawString(lstFiles.GetItemText(lstFiles.Items(e.Index)), e.Font, b, e.Bounds)
                'e.b
            End If

        End Using
        e.DrawFocusRectangle()
    End Sub

    Private Sub btnModify_Click(sender As Object, e As EventArgs) Handles btnModify.Click
        If lstFiles.SelectedIndex >= 0 Then
            If modifying Then
                lstFiles.Items.RemoveAt(lstFiles.SelectedIndex)
                lstFiles.Items.Add(txtFile.Text)
                lstFiles.SelectedIndex = lstFiles.Items.Count - 1

                lstFiles.Enabled = True
                btnModify.Text = "Modify"
                modifying = False
            Else
                txtFile.Text = lstFiles.SelectedItem.ToString
                lstFiles.Enabled = False
                btnModify.Text = "Confirm"
                modifying = True
            End If
        End If
    End Sub

    Private Sub btnBrowse_Click(sender As Object, e As EventArgs) Handles btnBrowse.Click
        Using fileDialog As OpenFileDialog = New OpenFileDialog()

            fileDialog.AutoUpgradeEnabled = True
            fileDialog.CheckFileExists = True
            fileDialog.DefaultExt = ".apk"
            fileDialog.Title = My.Resources.Strings.openFileDialogTitle
            fileDialog.Multiselect = False
            fileDialog.ValidateNames = True
            fileDialog.Filter = My.Resources.Strings.openFileDialogFilter
            fileDialog.ShowDialog()

            txtFile.Text = fileDialog.FileName
        End Using
    End Sub

    Private Sub txtFile_TextChanged(sender As Object, e As EventArgs) Handles txtFile.TextChanged
        btnAdd.Enabled = txtFile.Text IsNot "" And IO.File.Exists(txtFile.Text)
        If modifying Then
            btnModify.Enabled = txtFile.Text IsNot "" And IO.File.Exists(txtFile.Text)
        End If
    End Sub
End Class