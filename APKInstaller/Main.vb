Imports System.IO
Imports System.Threading
Imports MaterialSkin
Imports Squirrel

Public Class Main
    ' This delegate enables asynchronous calls for setting
    ' the text property on a TextBox control.
    Delegate Sub SetTextCallback(ByRef label As MaterialSkin.Controls.MaterialLabel, ByVal [text] As String)
    Delegate Sub VisibilityCallback(visible As Boolean)
    Private singleInstall As Boolean = False
    Private stopAll As Boolean
    Private apkInstaller As Installer
    Private updateMgr As AppUpdateManager = New AppUpdateManager(Me)

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
        AddHandler Application.ThreadException, AddressOf FatalErrorWasThrownHandlerTE
        AddHandler AppDomain.CurrentDomain.UnhandledException, AddressOf FatalErrorWasThrownHandlerCD

        AppUpdateManager.HandleEvents()
        Dim SkinManager As MaterialSkinManager = MaterialSkinManager.Instance
        SkinManager.AddFormToManage(Me)
        SkinManager.Theme = MaterialSkinManager.Themes.LIGHT
        SkinManager.ColorScheme = New ColorScheme(Primary.Orange700, Primary.Orange700, Primary.Orange100, Accent.LightBlue200, TextShade.WHITE)

        Dim apkFile As String = ""
        apkInstaller = New Installer(Me, Me.lblStatus, Me.txtFileLocation)

        updateMgr.UpdateLabel = Label1


        Dim noPrompt = False
        Dim appList As New List(Of String)
        For Each arg As String In My.Application.CommandLineArgs
            If arg.StartsWith("--") Or arg.StartsWith("-") Then
                If (arg.Equals("-f") Or arg.Equals("--force")) Then
                    Me.chkForce.Checked = True
                    Me.chkReinstall.Checked = False
                    noPrompt = True
                ElseIf (arg.Equals("-r") Or arg.Equals("--update") Or arg.Equals("--reinstall")) Then
                    Me.chkReinstall.Checked = True
                    Me.chkForce.Checked = False
                    noPrompt = True
                ElseIf (arg.Equals("/?") Or arg.Equals("--help") Or arg.Equals("/h") Or arg.Equals("-h")) Then
                    MsgBox(My.Application.Info.ProductName & " v" & My.Application.Info.Version.ToString & vbCrLf & vbCrLf &
                           "Usage Options: " & vbCrLf &
                           "--force | -f:" & vbCrLf & vbTab & "Removes any existing application and wipes " & vbCrLf & vbTab & "any associated application data for that " & vbCrLf & vbTab & "application, before installing" & vbCrLf &
                           "--update | --reinstall | -r:" & vbCrLf & vbTab & "Reinstalls the app represented by the given APK." & vbCrLf & vbTab & "Does nothing if the app is not already installed" & vbCrLf &
                           "--no-prompt | -np:" & vbCrLf & vbTab & "Does not prompt user for settings" & vbCrLf & vbTab & " [Implicit with options: -r, -f]" & vbCrLf &
                           "-!np | --prompt:" & vbCrLf & vbTab & "Prompts the user about options [Overrides: -np]",
                            CType(MsgBoxStyle.Information + MsgBoxStyle.OkOnly, MsgBoxStyle), "Usage")
                    Me.Close()
                ElseIf (arg.Equals("-np") Or arg.Equals("--no-prompt")) Then
                    noPrompt = True
                ElseIf (arg.Equals("--squirrel-firstrun")) Then
                    'MsgBox("You shouldn't see this! This means something went wrong. But you can still ignore this", CType(MsgBoxStyle.OkOnly + MsgBoxStyle.Exclamation, MsgBoxStyle))
                    SetText(lblStatus, "Welcome to the App Installer! Try dragging an APK on to me to start off with.")
                ElseIf (arg.Equals("-!np") Or arg.Equals("--prompt")) Then
                    noPrompt = False
                End If

                Continue For
            End If

            If Not MsgBox("I am going to install the APK in the location """ & arg & """", CType(MsgBoxStyle.Information + MsgBoxStyle.OkCancel, MsgBoxStyle)).Equals(MsgBoxResult.Ok) Then
                MsgBox("Install Aborted.", CType(MsgBoxStyle.Information + MsgBoxStyle.OkOnly, MsgBoxStyle))
                Me.Close()
                Return
            Else
                appList.Add(arg)
            End If
        Next

        Me.CenterToScreen()
        Dim fileArgs As String() = appList.ToArray
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
            If Not noPrompt Then
                Me.chkReinstall.Checked = MsgBox("Would you like to update the installed package on the phone?", CType(MsgBoxStyle.Question + MsgBoxStyle.YesNo, MsgBoxStyle)).Equals(MsgBoxResult.Yes)
                If Not chkReinstall.Checked Then
                    Me.chkForce.Checked = MsgBox("Would you like to forcefully remove any existing package to prep for a clean re-installation?", CType(MsgBoxStyle.Question + MsgBoxStyle.YesNo, MsgBoxStyle)).Equals(MsgBoxResult.Yes)
                End If
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

    Private Sub FatalErrorWasThrownHandler(sender As Object, e As Exception)
        Dim details = ""
        If (e IsNot Nothing) Then
            details = Convert.ToBase64String(System.Text.Encoding.Unicode.GetBytes(e.Message & e.StackTrace))
        End If
        Dim sTempFileName As String = Path.GetTempFileName()
        Dim fsTemp As New FileStream(sTempFileName, FileMode.Create)
        Dim detailsDump As Byte() = System.Text.Encoding.Unicode.GetBytes(details)
        fsTemp.Write(detailsDump, 0, details.Length)
        fsTemp.Close()
        MsgBox("An application error has been encountered." & vbCrLf &
                (If(e Is Nothing, "", "The following file may be wanted by troubleshooters and/or ninja monkeys.:" & vbCrLf & sTempFileName & vbCrLf)) &
                "Do you want to restart the application?", CType(MsgBoxStyle.YesNo + MsgBoxStyle.Critical, MsgBoxStyle))
        FileIO.FileSystem.DeleteFile(sTempFileName)
    End Sub

    Private Sub FatalErrorWasThrownHandlerTE(sender As Object, e As ThreadExceptionEventArgs)
        FatalErrorWasThrownHandler(sender, e.Exception)
    End Sub

    Private Sub FatalErrorWasThrownHandlerCD(sender As Object, e As UnhandledExceptionEventArgs)

        FatalErrorWasThrownHandler(sender, CType(e.ExceptionObject, Exception))
    End Sub
End Class
