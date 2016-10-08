Imports System.Diagnostics.CodeAnalysis
Imports System.IO
Imports System.Text
Imports System.Threading
Imports APKInstaller.My.Resources
Imports MaterialSkin
Imports MaterialSkin.Controls

''' <summary>
''' The main GUI of the application
''' </summary>
Public Class Main
    ' This delegate enables asynchronous calls for setting
    ' the text property on a TextBox control.
    Private Delegate Sub SetTextCallback(ByRef label As MaterialLabel, ByVal [text] As String)

    Private Delegate Sub VisibilityCallback(visible As Boolean, ByVal useStep As Boolean, stepAmount As Integer)

    Private Delegate Sub StepProgressBarDelegate()

    Private Delegate Sub ResetGuiCallback(message As String)

    Private _singleInstall As Boolean = False
    'Private stopAll As Boolean
    Private ReadOnly _apkInstaller As Installer
    Private ReadOnly _updateMgr As AppUpdateManager

    Private _dialogLock As Boolean = False
    Private _txtLock As Boolean
    Private _lastEditInTextbox As String = ""
    Private Shared _loadedApp As Boolean = False


    Sub New()
        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        _updateMgr = New AppUpdateManager(Me)
        '_updateMgr.UpdateLabel = Label1

        _apkInstaller = New Installer(Me, lblStatus, txtFileLocation)
    End Sub

    '<Log("App Installer Debug")>
    Private Sub APKInstallerMain_Load(sender As Object, e As EventArgs) Handles Me.Load
        ' Handle Fatal Errors for further diagnostics
        AddHandler Application.ThreadException, AddressOf FatalErrorWasThrownHandlerTE
        AddHandler AppDomain.CurrentDomain.UnhandledException, AddressOf FatalErrorWasThrownHandlerCD

        ' Configure app setup and updates
        AppUpdateManager.HandleEvents()
        _updateMgr.Update()

        'Configure GUI
        Dim manager = MaterialSkinManager.Instance
        manager.AddFormToManage(Me)
        SkinManager.Theme = MaterialSkinManager.Themes.LIGHT
        SkinManager.ColorScheme = New ColorScheme(Primary.Orange700, Primary.Orange700, Primary.Orange100,
                                                  Accent.LightBlue200, TextShade.WHITE)
        lnkAbout.Font = SkinManager.ROBOTO_MEDIUM_10
        lnkHelp.Font = SkinManager.ROBOTO_MEDIUM_10
        lnkAbout.ForeColor = Color.White
        lnkHelp.ForeColor = Color.White

        btnInstall.AutoSize = False
        btnInstall.Location = New Point(205, 259)
        btnInstall.Height = 83
        btnInstall.Width = 272

        CenterToScreen()

        ' Start parsing the arguments
        Dim noPrompt = False
        Dim appList As New List(Of String)
        Dim enableFileArg = True
        For Each arg As String In My.Application.CommandLineArgs
            If arg.StartsWith("--", StringComparison.Ordinal) Or arg.StartsWith("-", StringComparison.Ordinal) Then
                If arg.Contains("squirrel") AndAlso Not arg.Contains("firstrun") Then
                    enableFileArg = False
                End If
                noPrompt = ParseNonFileArgument(noPrompt, arg)
                Continue For
            End If

            If enableFileArg Then
                ParseFileArgument(appList, arg)
            End If
        Next

        If Not enableFileArg Then
            if AppUpdateManager.VerifyState(True)
                AppUpdateManager.HandleEvents() ' The next line will be only executed if everything is not working correctly
                MsgBox("The installer is broken.", CType(MsgBoxStyle.Critical + MsgBoxStyle.OkOnly, MsgBoxStyle))
            Else
                AppUpdateManager.NonSquirrelInstall()
                MsgBox("The installer is broken. However, we still managed to do things with the current state. Please send the following crash log to the developer.", CType(MsgBoxStyle.Critical + MsgBoxStyle.OkOnly, MsgBoxStyle))
                AppUpdateManager.VerifyState(False)
            End If
            Close()
        End If

        Dim fileArgs As String() = appList.ToArray
        _apkInstaller.AddFilesToInstall(fileArgs)
        If Not (fileArgs.Length = 0) And txtFileLocation.Text.Length > 0 And btnInstall.Enabled Then
            SetupSingleInstall(e, noPrompt)
        End If
        _loadedApp = true
    End Sub

    Private Sub SetupSingleInstall(e As EventArgs, noPrompt As Boolean)
        _singleInstall = True
        txtFileLocation.Visible = False
        btnInstall.Visible = False
        btnOpenFileDialogTrigger.Visible = False
        chkForce.Visible = False
        chkReinstall.Visible = False
        If Not noPrompt Then
            chkReinstall.Checked =
                MsgBox(Strings.updateQuestion, CType(MsgBoxStyle.Question + MsgBoxStyle.YesNo, MsgBoxStyle)).Equals(
                    MsgBoxResult.Yes)
            If Not chkReinstall.Checked Then
                chkForce.Checked =
                    MsgBox(Strings.forceQuestion, CType(MsgBoxStyle.Question + MsgBoxStyle.YesNo, MsgBoxStyle)).Equals(
                        MsgBoxResult.Yes)
            End If
        End If

        btnInstaller_Click(btnInstall, e)
    End Sub

    Private Sub ParseFileArgument(appList As List(Of String), arg As String)
        If _
            Not _
            MsgBox(Strings.installApkAtConfirm & """" & arg & """",
                   CType(MsgBoxStyle.Information + MsgBoxStyle.OkCancel, MsgBoxStyle)).Equals(MsgBoxResult.Ok) Then
            MsgBox(Strings.installAborted, CType(MsgBoxStyle.Information + MsgBoxStyle.OkOnly, MsgBoxStyle))
            Close()

        Else
            appList.Add(arg)
        End If
    End Sub

    Private Function ParseNonFileArgument(noPrompt As Boolean, arg As String) As Boolean
        If (arg.Equals("-f") Or arg.Equals("--force")) Then
            chkForce.Checked = True
            chkReinstall.Checked = False
            noPrompt = True
        ElseIf (arg.Equals("-r") Or arg.Equals("--update") Or arg.Equals("--reinstall")) Then
            chkReinstall.Checked = True
            chkForce.Checked = False
            noPrompt = True
        ElseIf (arg.Equals("/?") Or arg.Equals("--help") Or arg.Equals("/h") Or arg.Equals("-h")) Then
            MsgBox(My.Application.Info.ProductName & " v" & My.Application.Info.Version.ToString & vbCrLf & vbCrLf &
                   "Usage Options " & vbCrLf &
                   "--force | -f" & vbCrLf & vbTab & "Removes any existing application And wipes " & vbCrLf & vbTab &
                   "any associated application data for that " & vbCrLf & vbTab & "application, before installing" &
                   vbCrLf &
                   "--update | --reinstall | -r" & vbCrLf & vbTab & "Reinstalls the app represented by the given APK." &
                   vbCrLf & vbTab & "Does nothing if the app is not already installed" & vbCrLf &
                   "--no-prompt | -np" & vbCrLf & vbTab & "Does Not prompt user for settings" & vbCrLf & vbTab &
                   " [Implicit with options: -r, -f]" & vbCrLf &
                   "-!np | --prompt" & vbCrLf & vbTab & "Prompts the user about options [Overrides: -np]",
                   CType(MsgBoxStyle.Information + MsgBoxStyle.OkOnly, MsgBoxStyle), "Usage")
            Close()
        ElseIf (arg.Equals("-np") Or arg.Equals("--no-prompt")) Then
            noPrompt = True
        ElseIf (arg.Equals("--squirrel-firstrun")) Then
            SetText(lblStatus, Strings.firstRunMessage)
        ElseIf (arg.Equals("-!np") Or arg.Equals("--prompt")) Then
            noPrompt = False
        End If

        Return noPrompt
    End Function

    Private Sub btnOpenFileDialogTrigger_Click(sender As Object, e As EventArgs) Handles btnOpenFileDialogTrigger.Click
        Using fileDialog = New OpenFileDialog()
            If (txtFileLocation.Text.Length > 0) Then
                If txtFileLocation.Text.Contains(Path.PathSeparator) Then
                    fileDialog.FileName =
                        txtFileLocation.Text.Substring(
                            txtFileLocation.Text.LastIndexOf(Path.PathSeparator.ToString,
                                                             StringComparison.OrdinalIgnoreCase),
                            txtFileLocation.Text.Length -
                            txtFileLocation.Text.LastIndexOf(Path.PathSeparator.ToString,
                                                             StringComparison.OrdinalIgnoreCase))
                Else
                    fileDialog.FileName = txtFileLocation.Text
                End If
            End If
            fileDialog.AutoUpgradeEnabled = True
            fileDialog.CheckFileExists = True
            fileDialog.DefaultExt = ".apk"
            fileDialog.Title = Strings.openFileDialogTitle
            fileDialog.Multiselect = True
            fileDialog.ValidateNames = True
            fileDialog.Filter = Strings.openFileDialogFilter
            fileDialog.ShowDialog()

            _apkInstaller.AddFilesToInstall(fileDialog.FileNames)
        End Using
    End Sub

    Private Sub Me_DragDrop(sender As Object, e As DragEventArgs) Handles lblStatus.DragDrop, Me.DragDrop
        Dim files() = CType(e.Data.GetData(DataFormats.FileDrop), String())
        _apkInstaller.AddFilesToInstall(files)
    End Sub

    Private Sub Me_DragEnter(sender As Object, e As DragEventArgs) Handles lblStatus.DragEnter, Me.DragEnter
        If e.Data.GetDataPresent(DataFormats.FileDrop) Then
            e.Effect = DragDropEffects.Copy
        Else
            e.Effect = DragDropEffects.None
        End If
    End Sub

    Private Sub btnInstaller_Click(sender As Object, e As EventArgs) Handles btnInstall.Click
        btnInstall.Visible = False

        _apkInstaller.Reinstall = chkReinstall.Checked
        _apkInstaller.Force = chkForce.Checked
        _apkInstaller.CompletionMessageWhenFinished = Not _singleInstall

        Dim installerThread As New Thread(New ThreadStart(AddressOf _apkInstaller.StartInstall))
        installerThread.Start()
    End Sub

    Private Sub txtFileLocation_TextChanged(sender As Object, e As EventArgs) Handles txtFileLocation.TextChanged
        If _apkInstaller Is Nothing Then
            Exit Sub
        End If
        
        'If Not txtFileLocation.Focused Then
        '    exit Sub
        'End if

            Dim pos = txtFileLocation.SelectionStart
            Dim currentData = txtFileLocation.Text
            Dim comma = ","
            If currentData.Contains(comma) Then
                Dim firstIndex = currentData.IndexOf(comma, StringComparison.Ordinal)
                If firstIndex > pos Then ' Cursor is within first index
                    currentData = currentData.Substring(0, firstIndex + 1)
                Else ' Cursor is not
                    If pos >= currentData.Length Then
                        currentData = currentData.Substring(currentData.LastIndexOf(comma, StringComparison.Ordinal) + 1)
                    Else
                        Dim index = - 1
                        'DIm tempString = currentData
                        While True
                            Dim temp = currentData.IndexOf(comma, index + 1, StringComparison.Ordinal)
                            Dim tempString = currentData.Substring(Math.Max(index, Math.Max(- 1, temp)) + 1)
                            If temp >= pos Then
                                Exit While
                            Elseif Not tempString.Contains(comma)
                                index = temp + 1
                                exit while
                            else
                                index = temp
                            End If
                        End While
                        currentData = currentData.Substring(index )
                        If currentData.Contains(comma) Then
                            currentData = currentData.Substring(0, currentData.IndexOf(comma, StringComparison.Ordinal) + 1)
                        End If
                    End If
                End If
            End If

            ' Detect removed files
            Dim oldFiles = ExtractFilesFromString(_lastEditInTextbox)
            Dim currentFiles = ExtractFilesFromString(txtFileLocation.Text)
            If Not oldFiles.Equals(currentFiles)
                For Each file In oldFiles
                    If Not currentFiles.Contains(file)
                        _apkInstaller.RemoveFile(file)
                    End If
                Next
            End If


            ' Detect user typing
            If _lastEditInTextbox.Length < txtFileLocation.Text.Length
                if currentData.Contains(Path.DirectorySeparatorChar)
                    Dim folder = currentData.Substring(0, currentData.LastIndexOf(Path.DirectorySeparatorChar) + 1)
                    Dim file = currentData.Replace(folder, "")
                    if Directory.Exists(folder) And file.length > 1 _
                        ' 2 is how many characters the user needs to type before autocomplete
                        for each possibleFile in Directory.EnumerateFileSystemEntries(folder)
                            'Dim pFile = possibleFile.Substring(0, possibleFile.LastIndexOf(Path.DirectorySeparatorChar) + 1)
                            if possibleFile.StartsWith(currentData)
                                ' Found a solution to what the user is typing
                                Dim selStart = txtFileLocation.Text.IndexOf(currentData, StringComparison.Ordinal) +
                                               currentData.Length
                                if Directory.Exists(possibleFile)
                                    possibleFile &= Path.DirectorySeparatorChar
                                End If
                                Dim selLength = possibleFile.Replace(currentData, "").Length
                                txtFileLocation.Text = txtFileLocation.Text.Replace(currentData, possibleFile)
                                txtFileLocation.SelectionStart = selStart
                                txtFileLocation.SelectionLength = selLength
                                'currentData = possibleFile
                                Exit For
                            End If
                        Next
                    End If

                End if
            End If

            if Installer.ValidateFile(currentData)
                _apkInstaller.AddFilesToInstall(New String() {currentData})
                CheckInstallerConfig(True)
            else
                CheckInstallerConfig(False)
            End If
        

        if _lastEditInTextbox <> txtFileLocation.Text ' Keep last actual edit
            _lastEditInTextbox = txtFileLocation.Text
        End If
    End Sub

    Private Sub Main_GotFocus(sender As Object, e As EventArgs) Handles Me.GotFocus
        CheckInstallerConfig()
    End Sub

    Private Sub CheckInstallerConfig(Optional optionalCondition As Boolean = True)
        If _apkInstaller Is Nothing Then
            'Exit Sub
        End If

        If _apkInstaller.VerifyFilesToInstall(True) And optionalCondition Then
            btnInstall.Visible = True
            lblStatus.Text = Strings.readyToInstall
            ConfigureAppListFromTextbox()
        Else
            btnInstall.Visible = False
            lblStatus.Text = If(txtFileLocation.Text.Length = 0, Strings.dragFile, Strings.fileCanNotBeInstalled)
        End If
    End Sub

    Friend Sub ResetGui(done As String)
        If InvokeRequired Then
            Invoke(New ResetGuiCallback(AddressOf ResetGui), done)
        Else
            Dim message As String
            If _apkInstaller.VerifyFilesToInstall Then
                btnInstall.Visible = _apkInstaller.VerifyFilesToInstall
                message = Strings.readyToInstall
            Else
                message = If(txtFileLocation.Text.Length = 0, Strings.dragFile, Strings.fileCanNotBeInstalled)
            End If
            lblStatus.Text = If(done Is Nothing, message, done & vbCrLf & message)
            chkForce.Visible = True
            chkReinstall.Visible = True
            UseWaitCursor = False
            pgbStatus.Visible = False
            pgbStatus.Step = 0
            pgbStatus.Value = 0
            _apkInstaller.Reset()
        End If
    End Sub

    Private Sub chkReinstall_CheckedChanged(sender As Object, e As EventArgs) Handles chkReinstall.CheckedChanged
        If chkReinstall.Checked Then
            chkForce.Checked = False
            chkForce.Enabled = False
        Else
            chkForce.Enabled = True
        End If
    End Sub

    Private Sub chkForce_CheckedChanged(sender As Object, e As EventArgs) Handles chkForce.CheckedChanged
        If chkForce.Checked Then
            chkForce.Checked =
                MsgBox(Strings.forceConfirm, CType(MsgBoxStyle.Exclamation + MsgBoxStyle.YesNo, MsgBoxStyle)) =
                MsgBoxResult.Yes
        End If
    End Sub

    Private Sub Form1_Closing(sender As Object, e As EventArgs) Handles Me.Closing
        _apkInstaller.Abort()
    End Sub

    '<Log("App Installer Debug")>
    Private Sub Form1_Closed(sender As Object, e As EventArgs) Handles Me.Closed
        IoUtilities.Cleanup() ' Cleanup all of the tools we used
        AppUpdateManager.CleanUp()
    End Sub

    '<Log("App Installer Debug")>
    <SuppressMessage("Microsoft.Design", "CA1045:DoNotPassTypesByReference", MessageId := "0#")>
    Sub SetText(ByRef label As MaterialLabel, message As String)
        If label Is Nothing Then
            Throw New ArgumentNullException(NameOf(label))
        End If

        If label.InvokeRequired Then
            Dim d As New SetTextCallback(AddressOf SetText)
            Invoke(d, New Object() {label, message})
        Else
            label.Text = message
        End If
    End Sub

    '<Log("App Installer Debug")>
    Sub ShowProgressAnimation(isVisible As Boolean, useStep As Boolean)
        ShowProgressAnimation(isVisible, useStep, 0)
    End Sub

    '<SuppressMessage("Microsoft.Design", "CA1026:DefaultParametersShouldNotBeUsed")>
    '<Log("App Installer Debug")>
    Sub ShowProgressAnimation(isVisible As Boolean, useStep As Boolean, Optional stepAmount As Integer = 0)
        If pgbStatus.InvokeRequired Then
            Dim d As New VisibilityCallback(AddressOf ShowProgressAnimation)
            Invoke(d, New Object() {isVisible, useStep, stepAmount})
        Else
            pgbStatus.Visible = isVisible
            If (useStep) Then
                pgbStatus.Style = ProgressBarStyle.Continuous
                pgbStatus.Step = stepAmount
            Else
                pgbStatus.Style = ProgressBarStyle.Marquee
            End If
        End If
    End Sub

    '<Log("App Installer Debug")>
    Sub StepProgressBar()
        If pgbStatus.InvokeRequired Then
            Dim d As New StepProgressBarDelegate(AddressOf StepProgressBar)
            Invoke(d, New Object() {})
        Else
            pgbStatus.PerformStep()
        End If
    End Sub

    '<Log("App Installer Debug")>
    Private Shared Sub FatalErrorWasThrownHandler(e As Exception)
        Dim details = "App Installer " & Application.ProductVersion & " Crash Dump :(" & vbCrLf &
                            e.Message & e.StackTrace
        If (e Is Nothing) Then
            Return
            'details = Convert.ToBase64String(Encoding.Unicode.GetBytes(e.Message & e.StackTrace))
        End If
        Dim sTempFileName As String = Path.GetTempFileName()
        Using fsTemp As New FileStream(sTempFileName, FileMode.Create)
            Dim detailsDump As Byte() = Encoding.Unicode.GetBytes(details)
            Dim i = 0
            For j = 0 To detailsDump.Length - 1
                If detailsDump(j) = 0
                    Continue For
                End If
                detailsDump(i) = detailsDump(j)
                i += 1
            Next
            fsTemp.Write(detailsDump, 0, i)
        End Using
        Dim restartApp = False
        if MsgBox(if(_loadedApp, Strings.appError, "A problem starting the app has occurred.") & vbCrLf &
               (If(e Is Nothing, "", Strings.appErrorFileWanted & vbCrLf & sTempFileName & vbCrLf)) &
               if(_loadedApp, "Do you want to disable the update mechanism and restart the app?", Strings.wantToRestart), CType(MsgBoxStyle.YesNo + MsgBoxStyle.Critical, MsgBoxStyle)) = MsgBoxResult.Yes Then
            restartApp = True
        End if

        Try
            File.Delete(sTempFileName)
        Catch ex As Exception When TypeOf ex Is ArgumentException Or TypeOf ex Is ArgumentNullException
            Throw New IOException("The diagnostics file name is invalid. This shouldn't happen ever." & vbCrLf &
                                  "File name: " & sTempFileName)
        Catch ex As Exception When TypeOf ex Is PathTooLongException Or TypeOf ex Is DirectoryNotFoundException _
                                   Or TypeOf ex Is UnauthorizedAccessException Or TypeOf ex Is IOException

            Throw New IOException("The diagnostic file can't be automatically removed at runtime.", ex)
        End Try

        If restartApp
            If _loadedApp
                My.Settings.enableUpdates = False
            End If
            Application.Restart()
        End If
    End Sub

    '<Log("App Installer Debug")>
    Private Shared Sub FatalErrorWasThrownHandlerTE(sender As Object, e As ThreadExceptionEventArgs)
        FatalErrorWasThrownHandler(e.Exception)
    End Sub

    '<Log("App Installer Debug")>
    Private Shared Sub FatalErrorWasThrownHandlerCD(sender As Object, e As UnhandledExceptionEventArgs)
        FatalErrorWasThrownHandler(CType(e.ExceptionObject, Exception))
    End Sub

    '<Log("App Installer Debug")>
    Private Sub txtFileLocation_GotFocus(sender As Object, e As EventArgs) Handles txtFileLocation.GotFocus
        If _apkInstaller IsNot Nothing Then
            If _apkInstaller.UseMultiFileDialog Then
                txtFileLocation.Enabled = False

                'Dim test = ""
                'For Each file In _apkInstaller.GetFilesToInstall
                '    test &= file & Path.PathSeparator
                'Next file

                Using dialog = MultiPackageDialog.Create(_apkInstaller.GetFilesToInstall)
                    If Not _dialogLock Then
                        _dialogLock = True
                        If dialog.ShowDialog = DialogResult.OK Then
                            _apkInstaller.AddFilesToInstall(dialog.GetFiles(), True)
                            CheckInstallerConfig()
                        End If
                        _dialogLock = False
                    End If
                End Using

                'MsgBox("Use multifile dialog" & test)
            End If
        End If
    End Sub

    Private Sub txtFileLocation_LostFocus(sender As Object, e As EventArgs) Handles txtFileLocation.LostFocus
        If _apkInstaller IsNot Nothing AndAlso _apkInstaller.UseMultiFileDialog Then
            txtFileLocation.Enabled = True
        Else
            ConfigureAppListFromTextbox()
        End If
    End Sub

    Private Sub ConfigureAppListFromTextbox()
        If Not _txtLock Then
            _txtLock = True
            Dim files As List(Of String) = ExtractFilesFromString(txtFileLocation.Text)

            If _apkInstaller IsNot Nothing AndAlso Not _apkInstaller.UseMultiFileDialog Then
                Dim desc = ExtractFilesFromString(_apkInstaller.FilesToInstallDescription)
                For Each file As String In files
                    If Not desc.Contains(file) Then
                        _apkInstaller.RemoveFile(file)
                    End If
                Next
                Dim tempText = txtFileLocation.Text
                Dim tempPos = txtFileLocation.SelectionStart
                _apkInstaller.AddFilesToInstall(files.ToArray, False, False)
                txtFileLocation.Text = tempText
                txtFileLocation.SelectionStart = tempPos
            End If
            _txtLock = False
        End If
    End Sub

    Private Shared Function ExtractFilesFromString(data As String) As List(Of String)
        Dim files = New List(Of String)
        If data = "," Or data = "" Or data = Path.DirectorySeparatorChar
            Return files
        End If
        data = data.Replace("...", "")
        If data.Contains(",") Then
            Dim commaFiles = data.Split(CType(",", Char))
            For Each file In commaFiles
                If file <> "" AndAlso data.Contains(file) Then
                    data = data.Replace(file, "")
                End If
            Next
            data = data.Replace(",", "")
            files.AddRange(commaFiles)
        End If
        If data.Contains(Path.PathSeparator) Then
            Dim collection = data.Split(Path.PathSeparator)
            For Each file In collection
                If file <> "" AndAlso data.Contains(file) Then
                    data = data.Replace(file, "")
                End If
            Next
            data = data.Replace(Path.PathSeparator, "")
            files.AddRange(collection)
        End If
        If data IsNot "" Then
            files.Add(data)
        End If

        Return files
    End Function

    Private Sub txtFileLocation_DoubleClick(sender As Object, e As EventArgs) Handles txtFileLocation.DoubleClick
        Dim dialog = MultiPackageDialog.Create(_apkInstaller.GetFilesToInstall)
        If Not _dialogLock Then
            _dialogLock = True
            If dialog.ShowDialog = DialogResult.OK Then
                _apkInstaller.AddFilesToInstall(dialog.GetFiles(), True)
            End If
            _dialogLock = False
        End If
    End Sub

    Private Sub lnkAbout_Click(sender As Object, e As EventArgs) Handles lnkAbout.Click
        Using aboutDialog = New About
            aboutDialog.ShowDialog()
        End Using
    End Sub

    Private Sub lnkHelp_Click(sender As Object, e As EventArgs) Handles lnkHelp.Click
        UseWaitCursor = True
        Process.Start("https://cdn.rawgit.com/dmssargent/AppInstaller/v0.1.7.1-rc/docs/manual.pdf")
        UseWaitCursor = False
    End Sub

    Private Sub txtFileLocation_KeyDown(sender As Object, e As KeyEventArgs) Handles txtFileLocation.KeyDown
        If e.KeyCode = Keys.Right
            txtFileLocation_TextChanged(sender, New EventArgs())
        End If
    End Sub
End Class
