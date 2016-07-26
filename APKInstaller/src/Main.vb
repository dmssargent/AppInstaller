Imports System.IO
Imports System.Threading
Imports MaterialSkin

''' <summary>
''' The main GUI of the application
''' </summary>
Public Class Main
    ' This delegate enables asynchronous calls for setting
    ' the text property on a TextBox control.
    Private Delegate Sub SetTextCallback(ByRef label As Controls.MaterialLabel, ByVal [text] As String)
    Private Delegate Sub VisibilityCallback(visible As Boolean, ByVal useStep As Boolean, stepAmount As Integer)
    Private Delegate Sub StepProgressBarDelegate()
    Private Delegate Sub ResetGUICallback(message As String)
    Private singleInstall As Boolean = False
    'Private stopAll As Boolean
    Private apkInstaller As Installer
    Private updateMgr As AppUpdateManager

    Private dialogLock As Boolean = False


    Sub New()
        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        updateMgr = New AppUpdateManager(Me)
        updateMgr.UpdateLabel = Label1

        apkInstaller = New Installer(Me, Me.lblStatus, Me.txtFileLocation)
    End Sub

    Private Sub APKInstallerMain_Load(sender As Object, e As EventArgs) Handles Me.Load
        ' Handle Fatal Errors for further diagnostics
        AddHandler Application.ThreadException, AddressOf FatalErrorWasThrownHandlerTE
        AddHandler AppDomain.CurrentDomain.UnhandledException, AddressOf FatalErrorWasThrownHandlerCD

        ' Configure app setup and updates
        AppUpdateManager.HandleEvents()
        updateMgr.Update()

        'Configure GUI
        Dim SkinManager As MaterialSkinManager = MaterialSkinManager.Instance
        SkinManager.AddFormToManage(Me)
        SkinManager.Theme = MaterialSkinManager.Themes.LIGHT
        SkinManager.ColorScheme = New ColorScheme(Primary.Orange700, Primary.Orange700, Primary.Orange100, Accent.LightBlue200, TextShade.WHITE)
        Me.CenterToScreen()

        ' Start parsing the arguments
        Dim noPrompt = False
        Dim appList As New List(Of String)
        Dim enableFileArg = True
        For Each arg As String In My.Application.CommandLineArgs
            If arg.StartsWith("--", StringComparison.Ordinal) Or arg.StartsWith("-", StringComparison.Ordinal) Then
                If arg.Contains("squirrel") Then
                    enableFileArg = False
                End If
                noPrompt = ParseNonFileArgument(noPrompt, arg)
                Continue For
            End If

            If enableFileArg Then
                ParseFileArgument(appList, arg)
            End If
        Next

        Dim fileArgs As String() = appList.ToArray
        apkInstaller.AddFilesToInstall(fileArgs)
        If Not (fileArgs.Length = 0) And txtFileLocation.Text.Length > 0 And btnInstall.Enabled Then
            SetupSingleInstall(e, noPrompt)
        End If
    End Sub

    Private Sub SetupSingleInstall(e As EventArgs, noPrompt As Boolean)
        Me.singleInstall = True
        Me.txtFileLocation.Visible = False
        Me.btnInstall.Visible = False
        Me.btnOpenFileDialogTrigger.Visible = False
        Me.chkForce.Visible = False
        Me.chkReinstall.Visible = False
        If Not noPrompt Then
            Me.chkReinstall.Checked = MsgBox(My.Resources.Strings.updateQuestion, CType(MsgBoxStyle.Question + MsgBoxStyle.YesNo, MsgBoxStyle)).Equals(MsgBoxResult.Yes)
            If Not chkReinstall.Checked Then
                Me.chkForce.Checked = MsgBox(My.Resources.Strings.forceQuestion, CType(MsgBoxStyle.Question + MsgBoxStyle.YesNo, MsgBoxStyle)).Equals(MsgBoxResult.Yes)
            End If
        End If

        btnInstaller_Click(btnInstall, e)
    End Sub

    Private Sub ParseFileArgument(appList As List(Of String), arg As String)
        If Not MsgBox(My.Resources.Strings.installApkAtConfirm & """" & arg & """", CType(MsgBoxStyle.Information + MsgBoxStyle.OkCancel, MsgBoxStyle)).Equals(MsgBoxResult.Ok) Then
            MsgBox(My.Resources.Strings.installAborted, CType(MsgBoxStyle.Information + MsgBoxStyle.OkOnly, MsgBoxStyle))
            Me.Close()

        Else
            appList.Add(arg)
        End If
    End Sub

    Private Function ParseNonFileArgument(noPrompt As Boolean, arg As String) As Boolean
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
                   "Usage Options " & vbCrLf &
                   "--force | -f" & vbCrLf & vbTab & "Removes any existing application And wipes " & vbCrLf & vbTab & "any associated application data for that " & vbCrLf & vbTab & "application, before installing" & vbCrLf &
                   "--update | --reinstall | -r" & vbCrLf & vbTab & "Reinstalls the app represented by the given APK." & vbCrLf & vbTab & "Does nothing if the app Is Not already installed" & vbCrLf &
                   "--no-prompt | -np" & vbCrLf & vbTab & "Does Not prompt user for settings" & vbCrLf & vbTab & " [Implicit with options: -r, -f]" & vbCrLf &
                   "-!np | --prompt" & vbCrLf & vbTab & "Prompts the user about options [Overrides: -np]",
                    CType(MsgBoxStyle.Information + MsgBoxStyle.OkOnly, MsgBoxStyle), "Usage")
            Me.Close()
        ElseIf (arg.Equals("-np") Or arg.Equals("--no-prompt")) Then
            noPrompt = True
        ElseIf (arg.Equals("--squirrel-firstrun")) Then
            SetText(lblStatus, My.Resources.Strings.firstRunMessage)
        ElseIf (arg.Equals("-!np") Or arg.Equals("--prompt")) Then
            noPrompt = False
        End If

        Return noPrompt
    End Function

    Private Sub btnOpenFileDialogTrigger_Click(sender As Object, e As EventArgs) Handles btnOpenFileDialogTrigger.Click
        Using fileDialog As OpenFileDialog = New OpenFileDialog()
            If (txtFileLocation.Text.Length > 0) Then
                If txtFileLocation.Text.Contains(Path.PathSeparator) Then
                    fileDialog.FileName = txtFileLocation.Text.Substring(txtFileLocation.Text.LastIndexOf(Path.PathSeparator.ToString, StringComparison.OrdinalIgnoreCase),
                                                                         txtFileLocation.Text.Length - txtFileLocation.Text.LastIndexOf(Path.PathSeparator.ToString, StringComparison.OrdinalIgnoreCase))
                Else
                    fileDialog.FileName = txtFileLocation.Text
                End If
            End If
            fileDialog.AutoUpgradeEnabled = True
            fileDialog.CheckFileExists = True
            fileDialog.DefaultExt = ".apk"
            fileDialog.Title = My.Resources.Strings.openFileDialogTitle
            fileDialog.Multiselect = True
            fileDialog.ValidateNames = True
            fileDialog.Filter = My.Resources.Strings.openFileDialogFilter
            fileDialog.ShowDialog()

            apkInstaller.AddFilesToInstall(fileDialog.FileNames)
        End Using
    End Sub

    Private Sub Me_DragDrop(sender As Object, e As DragEventArgs) Handles lblStatus.DragDrop, Me.DragDrop
        Dim files() As String = CType(e.Data.GetData(DataFormats.FileDrop), String())
        apkInstaller.AddFilesToInstall(files)
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

        apkInstaller.Reinstall = chkReinstall.Checked
        apkInstaller.Force = chkForce.Checked
        apkInstaller.CompletionMessageWhenFinished = Not singleInstall

        Dim installerThread As New Thread(New ThreadStart(AddressOf apkInstaller.StartInstall))
        installerThread.Start()
    End Sub

    Private Sub txtFileLocation_TextChanged(sender As Object, e As EventArgs) Handles txtFileLocation.TextChanged
        If apkInstaller Is Nothing Then
            Exit Sub
        End If

        If apkInstaller IsNot Nothing AndAlso Not apkInstaller.UseMultiFileDialog Then
            Dim pos = txtFileLocation.SelectionStart
            Dim currentData = txtFileLocation.Text
            If currentData.Contains(",") Then
                Dim firstIndex = currentData.IndexOf(",")
                If firstIndex > pos Then
                    currentData = currentData.Substring(0, firstIndex + 1)
                Else
                    If pos >= currentData.Length Then
                        currentData = currentData.Substring(currentData.LastIndexOf(",") + 1)
                    Else
                        Dim index = -1
                        While True
                            Dim temp = currentData.IndexOf(",", index + 1)
                            If temp >= pos Then
                                Exit While
                            Else
                                index = temp
                            End If
                        End While
                        currentData = currentData.Substring(index)
                        If currentData.Contains(",") Then
                            currentData = currentData.Substring(0, currentData.IndexOf(",") + 1)
                        End If
                    End If
                End If
            End If

            CheckInstallerConfig(apkInstaller.ValidateFile(currentData))
        End If
    End Sub

    Private Sub Main_GotFocus(sender As Object, e As EventArgs) Handles Me.GotFocus
        CheckInstallerConfig()
    End Sub

    Private Sub CheckInstallerConfig(Optional optionalConditon As Boolean = True)
        If apkInstaller Is Nothing Then
            'Exit Sub
        End If

        If apkInstaller.VerifyFilesToInstall And optionalConditon Then
            btnInstall.Visible = apkInstaller.VerifyFilesToInstall
            lblStatus.Text = My.Resources.Strings.readyToInstall
        Else
            lblStatus.Text = If(txtFileLocation.Text.Length = 0, My.Resources.Strings.dragFile, My.Resources.Strings.fileCanNotBeInstalled)
        End If
    End Sub

    Friend Sub ResetGUI(done As String)
        If Me.InvokeRequired Then
            Me.Invoke(New ResetGUICallback(AddressOf ResetGUI), done)
        Else
            Dim message = ""
            If apkInstaller.VerifyFilesToInstall Then
                btnInstall.Visible = apkInstaller.VerifyFilesToInstall
                message = My.Resources.Strings.readyToInstall
            Else
                message = If(txtFileLocation.Text.Length = 0, My.Resources.Strings.dragFile, My.Resources.Strings.fileCanNotBeInstalled)
            End If
            lblStatus.Text = If(done Is Nothing, message, done & vbCrLf & message)
            chkForce.Visible = True
            chkReinstall.Visible = True
            UseWaitCursor = False
            apkInstaller.Reset()
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
            chkForce.Checked = MsgBox(My.Resources.Strings.forceConfirm, CType(MsgBoxStyle.Exclamation + MsgBoxStyle.YesNo, MsgBoxStyle)) = MsgBoxResult.Yes
        End If
    End Sub

    Private Sub Form1_Closing(sender As Object, e As EventArgs) Handles Me.Closing
        apkInstaller.Abort()
    End Sub

    Private Sub Form1_Closed(sender As Object, e As EventArgs) Handles Me.Closed
        IOUtilities.Cleanup() ' Cleanup all of the tools we used
    End Sub

    <CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1045:DoNotPassTypesByReference", MessageId:="0#")>
    Sub SetText(ByRef label As Controls.MaterialLabel, ByVal text As String)
        If label Is Nothing Then
            Throw New ArgumentNullException(NameOf(label))
        End If

        If label.InvokeRequired Then
            Dim d As New SetTextCallback(AddressOf SetText)
            Me.Invoke(d, New Object() {label, text})
        Else
            label.Text = text
        End If
    End Sub

    Sub ShowProgressAnimation(visible As Boolean, ByVal useStep As Boolean)
        ShowProgressAnimation(visible, useStep, 0)
    End Sub

    <CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1026:DefaultParametersShouldNotBeUsed")>
    Sub ShowProgressAnimation(visible As Boolean, ByVal useStep As Boolean, Optional stepAmount As Integer = 0)
        If pgbStatus.InvokeRequired Then
            Dim d As New VisibilityCallback(AddressOf ShowProgressAnimation)
            Me.Invoke(d, New Object() {visible, useStep, stepAmount})
        Else
            pgbStatus.Visible = visible
            If (useStep) Then
                pgbStatus.Style = ProgressBarStyle.Continuous
                pgbStatus.Step = stepAmount
            Else
                pgbStatus.Style = ProgressBarStyle.Marquee
            End If
        End If
    End Sub

    Sub StepProgressBar()
        If pgbStatus.InvokeRequired Then
            Dim d As New StepProgressBarDelegate(AddressOf StepProgressBar)
            Me.Invoke(d, New Object() {})
        Else
            pgbStatus.PerformStep()
        End If
    End Sub

    Private Shared Sub FatalErrorWasThrownHandler(e As Exception)
        Dim details = ""
        If (e IsNot Nothing) Then
            details = Convert.ToBase64String(System.Text.Encoding.Unicode.GetBytes(e.Message & e.StackTrace))
        End If
        Dim sTempFileName As String = Path.GetTempFileName()
        Using fsTemp As New FileStream(sTempFileName, FileMode.Create)
            Dim detailsDump As Byte() = System.Text.Encoding.Unicode.GetBytes(details)
            fsTemp.Write(detailsDump, 0, details.Length)
        End Using
        MsgBox(My.Resources.Strings.appError & vbCrLf &
                    (If(e Is Nothing, "", My.Resources.Strings.appErrorFileWanted & vbCrLf & sTempFileName & vbCrLf)) &
                    My.Resources.Strings.wantToRestart, CType(MsgBoxStyle.YesNo + MsgBoxStyle.Critical, MsgBoxStyle))
        Try
            File.Delete(sTempFileName)
        Catch ex As Exception When TypeOf ex Is ArgumentException Or TypeOf ex Is ArgumentNullException
            Throw New IOException("The diagnostics file name is invalid. This shouldn't happen ever." & vbCrLf &
                                    "File name: " & sTempFileName)
        Catch ex As Exception When TypeOf ex Is PathTooLongException Or TypeOf ex Is DirectoryNotFoundException _
            Or TypeOf ex Is UnauthorizedAccessException Or TypeOf ex Is IOException

            Throw New IOException("The diagnostic file can't be automatically removed at runtime.", ex)
        End Try

    End Sub

    Private Sub FatalErrorWasThrownHandlerTE(sender As Object, e As ThreadExceptionEventArgs)
        FatalErrorWasThrownHandler(e.Exception)

    End Sub

    Private Sub FatalErrorWasThrownHandlerCD(sender As Object, e As UnhandledExceptionEventArgs)
        FatalErrorWasThrownHandler(CType(e.ExceptionObject, Exception))
    End Sub

    Private Sub txtFileLocation_GotFocus(sender As Object, e As EventArgs) Handles txtFileLocation.GotFocus
        If apkInstaller IsNot Nothing Then
            If apkInstaller.UseMultiFileDialog Then
                txtFileLocation.Enabled = False

                Dim test = ""
                For Each file In apkInstaller.GetFilesToInstall
                    test &= file & Path.PathSeparator
                Next file

                Dim dialog = MultiPackageDialog.Create(apkInstaller.GetFilesToInstall)
                If Not dialogLock Then
                    dialogLock = True
                    If dialog.ShowDialog = DialogResult.OK Then
                        apkInstaller.AddFilesToInstall(dialog.GetFiles(), True)
                    End If
                    dialogLock = False
                End If

                'MsgBox("Use multifile dialog" & test)
            End If
        End If
    End Sub

    Private Sub txtFileLocation_LostFocus(sender As Object, e As EventArgs) Handles txtFileLocation.LostFocus
        If apkInstaller IsNot Nothing AndAlso apkInstaller.UseMultiFileDialog Then
            txtFileLocation.Enabled = True
        Else
            Dim files As List(Of String) = ExtractFilesFromString(txtFileLocation.Text)

            If apkInstaller IsNot Nothing AndAlso Not apkInstaller.UseMultiFileDialog Then
                Dim desc = ExtractFilesFromString(apkInstaller.FilesToInstallDescription)
                For Each file As String In files
                    If Not desc.Contains(file) Then
                        apkInstaller.RemoveFile(file)
                    End If
                Next
                Dim temp = txtFileLocation.Text
                apkInstaller.AddFilesToInstall(files.ToArray, False, False)
                txtFileLocation.Text = temp
            End If
        End If
    End Sub

    Private Function ExtractFilesFromString(data As String) As List(Of String)
        Dim files = New List(Of String)
        data.Replace("...", "")
        If data.Contains(",") Then
            files.AddRange(data.Split(CType(",", Char)))
        End If
        If data.Contains(Path.PathSeparator) Then
            files.AddRange(data.Split(Path.PathSeparator))
        End If

        Return files
    End Function

    Private Sub txtFileLocation_DoubleClick(sender As Object, e As EventArgs) Handles txtFileLocation.DoubleClick
        Dim dialog = MultiPackageDialog.Create(apkInstaller.GetFilesToInstall)
        If Not dialogLock Then
            dialogLock = True
            If dialog.ShowDialog = DialogResult.OK Then
                apkInstaller.AddFilesToInstall(dialog.GetFiles(), True)
            End If
            dialogLock = False
        End If
    End Sub

End Class
