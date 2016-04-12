Imports MaterialSkin

Public Class Main
    ' This delegate enables asynchronous calls for setting
    ' the text property on a TextBox control.
    Delegate Sub SetTextCallback(ByRef label As MaterialSkin.Controls.MaterialLabel, ByVal [text] As String)
    Delegate Sub VisibilityCallback(visible As Boolean)
    Private singleInstall As Boolean = False
    Private stopAll As Boolean
    Private apkInstaller As Installer

    Private Sub btnOpenFileDialogTrigger_Click(sender As Object, e As EventArgs) Handles btnOpenFileDialogTrigger.Click
        Dim fileDialog As OpenFileDialog = New OpenFileDialog()
        If (txtFileLocation.Text.Length > 0) Then
            If txtFileLocation.Text.Contains(";") Then
                fileDialog.FileName = txtFileLocation.Text.Substring(txtFileLocation.Text.LastIndexOf(";"), txtFileLocation.Text.Length - txtFileLocation.Text.LastIndexOf(";"))
            Else
                fileDialog.FileName = txtFileLocation.Text
            End If
        End If
        fileDialog.AutoUpgradeEnabled = True
        fileDialog.CheckFileExists = True
        fileDialog.DefaultExt = ".apk"
        fileDialog.Title = "Open Android APK files..."
        fileDialog.Multiselect = True
        fileDialog.ValidateNames = True
        fileDialog.Filter = "APK Files (*.apk)|*.apk|All Files|*.*"
        fileDialog.ShowDialog()
        apkInstaller.AddFilesToInstall(fileDialog.FileNames)
    End Sub

    Private Sub Me_DragDrop(sender As System.Object, e As DragEventArgs) Handles lblStatus.DragDrop, Me.DragDrop
        Dim files() As String = CType(e.Data.GetData(DataFormats.FileDrop), String())
        apkInstaller.AddFilesToInstall(files)
        If apkInstaller.VerifyFilesToInstall Then
            lblStatus.Text = "Ready to install? Click Install. Or just keep adding files"
        End If
    End Sub

    Private Sub Me_DragEnter(sender As Object, e As DragEventArgs) Handles lblStatus.DragEnter, Me.DragEnter
        If e.Data.GetDataPresent(DataFormats.FileDrop) Then
            e.Effect = DragDropEffects.Copy
        End If
    End Sub

    Private Sub btnInstaller_Click(sender As Object, e As EventArgs) Handles btnInstall.Click
        btnInstall.Visible = False
        apkInstaller.ConfigureReInstall(Me.chkReinstall.Checked)
        apkInstaller.ShowCompletionMessageWhenFinished(Not singleInstall)
        Dim installerThread As New Threading.Thread(New Threading.ThreadStart(AddressOf apkInstaller.StartInstall))
        installerThread.Start()
    End Sub

    Private Sub txtFileLocation_TextChanged(sender As Object, e As EventArgs) Handles txtFileLocation.TextChanged
        btnInstall.Visible = Not (txtFileLocation.Text.Length = 0)

        If btnInstall.Visible Then
            btnInstall.Visible = apkInstaller.VerifyFilesToInstall
        End If
    End Sub

    Sub SetText(ByRef label As MaterialSkin.Controls.MaterialLabel, ByVal text As String)
        If label.InvokeRequired Then
            Dim d As New SetTextCallback(AddressOf SetText)
            Me.Invoke(d, New Object() {label, text})
        Else
            label.Text = text
        End If
    End Sub

    Sub ShowProgressAnimation(visible As Boolean)
        If pgbStatus.InvokeRequired Then
            Dim d As New VisibilityCallback(AddressOf ShowProgressAnimation)
            Me.Invoke(d, New Object() {visible})
        Else
            pgbStatus.Visible = visible
        End If
    End Sub

    Private Sub APKInstallerMain_Load(sender As Object, e As EventArgs) Handles Me.Load
        Dim SkinManager As MaterialSkinManager = MaterialSkinManager.Instance
        SkinManager.AddFormToManage(Me)
        SkinManager.Theme = MaterialSkinManager.Themes.LIGHT
        SkinManager.ColorScheme = New ColorScheme(Primary.Orange700, Primary.Orange700, Primary.Orange100, Accent.LightBlue200, TextShade.WHITE)

        'Are we launching as a click-once?
        'If (Application.ApplicationDeployment.IsNetworkDeployed) Then
        Dim apkFile As String = ""
        apkInstaller = New Installer(Me, Me.lblStatus, Me.txtFileLocation)
        Try
            If Not (AppDomain.CurrentDomain.SetupInformation Is Nothing Or
                        AppDomain.CurrentDomain.SetupInformation.ActivationArguments Is Nothing Or
                        AppDomain.CurrentDomain.SetupInformation.ActivationArguments.ActivationData Is Nothing) Then
                Dim urifile As String = New Uri(AppDomain.CurrentDomain.SetupInformation.ActivationArguments.ActivationData(0)).LocalPath
                If urifile.Contains(".apk") Then
                    If Not MsgBox("I am going to install the APK in the location """ & urifile & """", CType(MsgBoxStyle.Information + MsgBoxStyle.OkCancel, MsgBoxStyle)).Equals(MsgBoxResult.Ok) Then
                        Me.Close()
                        Return
                    End If
                    apkInstaller.AddFilesToInstall(New String() {urifile})
                    apkFile = urifile
                End If
            End If
        Catch ex As Exception
            MsgBox("An error occurred with checking ClickOnce " & ex.ToString, CType(MsgBoxStyle.Exclamation + MsgBoxStyle.OkOnly, MsgBoxStyle))
        End Try
        ' End If

        For Each arg As String In My.Application.CommandLineArgs
            If Not MsgBox("I am going to install the APK in the location """ & arg & """", CType(MsgBoxStyle.Information + MsgBoxStyle.OkCancel, MsgBoxStyle)).Equals(MsgBoxResult.Ok) Then
                Me.Close()
                Return
            End If
        Next

        Me.CenterToScreen()
        Dim fileArgs As String() = My.Application.CommandLineArgs.ToArray
        If apkFile IsNot "" Then
            Dim filesToInstall(fileArgs.Length + 1) As String
            If (fileArgs.Length > 0) Then
                For i As Integer = 0 To filesToInstall.Length - 1
                    'Copy the APK file to the last element of the filesInstall array, which is one bigger than toArray
                    If (i = fileArgs.Length) Then
                        filesToInstall(i) = apkFile
                    Else
                        filesToInstall(i) = fileArgs(i)
                    End If
                Next
            Else
                filesToInstall(0) = apkFile
            End If
            If Not (filesToInstall Is Nothing) Then
                fileArgs = filesToInstall
            Else
                MsgBox("An error occurring opening the specified file!", CType(MsgBoxStyle.OkOnly + MsgBoxStyle.Critical, MsgBoxStyle))
            End If

        End If
        apkInstaller.AddFilesToInstall(fileArgs)
        If Not (fileArgs.Length = 0) And txtFileLocation.Text.Length > 0 And btnInstall.Enabled Then
            Me.singleInstall = True
            btnInstaller_Click(btnInstall, e)
            Me.txtFileLocation.Visible = False
            Me.btnInstall.Visible = False
            Me.btnOpenFileDialogTrigger.Visible = False
            Me.chkForce.Visible = False
            Me.chkReinstall.Visible = False
            Me.chkReinstall.Checked = MsgBox("Would you like to update the installed package on the phone?", CType(MsgBoxStyle.Question + MsgBoxStyle.YesNo, MsgBoxStyle)).Equals(MsgBoxResult.Yes)
            If Not chkReinstall.Checked Then
                Me.chkForce.Checked = MsgBox("Would you like to forcefully remove any existing package to prep for a clean re-installation?", CType(MsgBoxStyle.Question + MsgBoxStyle.YesNo, MsgBoxStyle)).Equals(MsgBoxResult.Yes)
            End If
        End If
    End Sub

    Private Sub Form1_Closed(sender As Object, e As EventArgs) Handles Me.Closed
        apkInstaller.abort()
    End Sub

    Private Sub chkReinstall_CheckedChanged(sender As Object, e As EventArgs) Handles chkReinstall.CheckedChanged
        If chkReinstall.Checked Then
            chkForce.Enabled = False
        Else
            chkForce.Enabled = True
        End If
    End Sub

    Private Sub chkForce_CheckedChanged(sender As Object, e As EventArgs) Handles chkForce.CheckedChanged
        If chkForce.Checked Then
            chkForce.Checked = MsgBox("Are you really, absolutely sure you want to force the installation by any means necessary? This WILL be destructive if the app is currently installed on the phone.", CType(MsgBoxStyle.Exclamation + MsgBoxStyle.YesNo, MsgBoxStyle)) = MsgBoxResult.Yes
        End If
    End Sub
End Class
