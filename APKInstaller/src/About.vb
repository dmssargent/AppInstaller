Imports APKInstaller.My.Resources
Imports MaterialSkin

Public NotInheritable Class About
    Private Sub About_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        MaterialSkinManager.Instance.AddFormToManage(Me)
        ' Set the title of the form.
        Dim applicationTitle As String
        If My.Application.Info.Title <> "" Then
            applicationTitle = My.Application.Info.Title
        Else
            applicationTitle = System.IO.Path.GetFileNameWithoutExtension(My.Application.Info.AssemblyName)
        End If
        Me.Text = $"About {applicationTitle}"
        ' Initialize all of the text displayed on the About Box.
        '    properties dialog (under the "Project" menu).
        'Me.LabelProductName.Text = My.Application.Info.ProductName
        Me.LabelVersion.Text = $"Version {My.Application.Info.Version.ToString}"
        Me.LabelCopyright.Text = My.Application.Info.Copyright
        Me.LabelCompanyName.Text = Strings.Built_by & My.Application.Info.CompanyName
        Me.TextBoxDescription.Font = LabelVersion.Font
        Me.TextBoxDescription.Text = My.Application.Info.Description

        Me.chkPrerelease.Checked = My.Settings.prerelease
        Me.lblUpdateStatus.Text = AppUpdateManager.UpdateStatusText
    End Sub

    Private Sub chkPrerelease_CheckedChanged(sender As Object, e As EventArgs) Handles chkPrerelease.CheckedChanged
        My.Settings.prerelease = chkPrerelease.Checked
        AppUpdateManager.UpdateApp()
    End Sub

    Private Sub OKButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OKButton.Click
        Close()
    End Sub

    Private Sub LogoPictureBox_Click(sender As Object, e As EventArgs) Handles LogoPictureBox.DoubleClick
        UseWaitCursor = True
        Process.Start("http://www.firstinspires.org/robotics/ftc")
        UseWaitCursor = False
    End Sub
End Class
