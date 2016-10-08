Imports System.IO
Imports APKInstaller.My.Resources
Imports MaterialSkin

Public NotInheritable Class About
    Private Sub About_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        MaterialSkinManager.Instance.AddFormToManage(Me)
        ' Set the title of the form.
        Dim applicationTitle As String
        If My.Application.Info.Title <> "" Then
            applicationTitle = My.Application.Info.Title
        Else
            applicationTitle = Path.GetFileNameWithoutExtension(My.Application.Info.AssemblyName)
        End If
        Text = $"About {applicationTitle}"
        ' Initialize all of the text displayed on the About Box.
        '    properties dialog (under the "Project" menu).
        'Me.LabelProductName.Text = My.Application.Info.ProductName
        LabelVersion.Text = $"Version {My.Application.Info.Version.ToString}"
        LabelCopyright.Text = My.Application.Info.Copyright
        LabelCompanyName.Text = Strings.Built_by & My.Application.Info.CompanyName
        TextBoxDescription.Font = LabelVersion.Font
        TextBoxDescription.Text = My.Application.Info.Description

        chkPrerelease.Checked = My.Settings.enableUpdates
        chkPrerelease.Location = New Point(131, 0)

        lblUpdateStatus.Text = AppUpdateManager.UpdateStatusText
    End Sub

    Private Sub chkPrerelease_CheckedChanged(sender As Object, e As EventArgs) Handles chkPrerelease.CheckedChanged
        My.Settings.enableUpdates = chkPrerelease.Checked
        AppUpdateManager.UpdateApp()
    End Sub

    Private Sub OKButton_Click(ByVal sender As Object, ByVal e As EventArgs) Handles OKButton.Click
        Close()
    End Sub

    Private Sub LogoPictureBox_Click(sender As Object, e As EventArgs) Handles LogoPictureBox.DoubleClick
        UseWaitCursor = True
        Process.Start("http://www.firstinspires.org/robotics/ftc")
        UseWaitCursor = False
    End Sub
End Class
