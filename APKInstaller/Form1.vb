Imports System.IO
Imports System.IO.Compression
Imports System.Deployment
Public Class Form1
    ' This delegate enables asynchronous calls for setting
    ' the text property on a TextBox control.
    Delegate Sub SetTextCallback(ByRef label As Label, ByVal [text] As String)
    Delegate Sub VisibilityCallback(visible As Boolean)
    Private singleInstall As Boolean = False
    Private stopAll As Boolean

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
        addFilesToInstall(fileDialog.FileNames)
    End Sub

    Private Sub Me_DragDrop(sender As System.Object, e As System.Windows.Forms.DragEventArgs) Handles lblStatus.DragDrop, Me.DragDrop
        Dim files() As String = e.Data.GetData(DataFormats.FileDrop)
        addFilesToInstall(files)
    End Sub

    Private Sub addFilesToInstall(files() As String)
        Dim location As String = ""
        For Each path As String In files
            If path Is Nothing Then
                Continue For
            End If
            If path.ToLower.EndsWith(".apk") Then
                location += path + ";"
            Else
                MsgBox("""" + path + """" + " is not a valid Android app. Please verify that the file ends with "".APK"".", MsgBoxStyle.OkOnly + MsgBoxStyle.Exclamation, "Invalid File")
            End If
        Next

        If location.EndsWith(";") Then
            location = location.Substring(0, location.Length - 1)
        End If
        Me.txtFileLocation.Text = location
    End Sub

    Private Sub Me_DragEnter(sender As Object, e As DragEventArgs) Handles lblStatus.DragEnter, Me.DragEnter
        If e.Data.GetDataPresent(DataFormats.FileDrop) Then
            e.Effect = DragDropEffects.Copy
        End If
    End Sub

    Private Function getFilesToInstall() As String()
        Return txtFileLocation.Text.Split(";")
    End Function

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles btnInstall.Click
        btnInstall.Enabled = False
        Dim thread As New Threading.Thread(New Threading.ThreadStart(AddressOf install))
        thread.Start()
    End Sub

    Private Sub install()
        Dim adbLocation As String = getAdbExec()

        Dim adbWait As New Process()
        adbWait.StartInfo.FileName = adbLocation
        adbWait.StartInfo.Arguments = "wait-for-device"
        adbWait.StartInfo.UseShellExecute = False
        adbWait.StartInfo.CreateNoWindow = True
        SetText(lblStatus, "Waiting for device.")
        adbWait.Start()

        Dim caret As Integer = 0
        While Not adbWait.HasExited
            UseWaitCursor = True
            showProgressAnimation(True)
            If caret = 0 Then
                SetText(lblStatus, "Waiting for device.")
            ElseIf caret = 1 Then
                SetText(lblStatus, "Waiting for device..")
            Else
                SetText(lblStatus, "Waiting for device...")
                caret = -1
            End If
            caret += 1

            Threading.Thread.Sleep(1000)
            Threading.Thread.Yield()
            If Me.stopAll Then
                MsgBox("The install has been aborted", MsgBoxStyle.OkOnly + MsgBoxStyle.Exclamation, "APK Install Aborted")
                Return
            End If
        End While
        showProgressAnimation(False)
        adbWait.Close()


        SetText(lblStatus, "Starting Installs...")
        Dim filesToInstall As String() = getFilesToInstall()
        'pgbStatus.Step = 100 / filesToInstall.Length
        Dim installStatus As String = ""
        For Each file As String In getFilesToInstall()
            Dim adb As New Process()
            Dim arguments As String = "install "
            If (chkReinstall.Checked) Then
                arguments += "-r "
            End If
            adb.StartInfo.Arguments = arguments + file
            adb.StartInfo.CreateNoWindow = True
            adb.StartInfo.UseShellExecute = False
            adb.StartInfo.FileName = adbLocation
            adb.StartInfo.RedirectStandardOutput = True
            'MsgBox(adb.StartInfo.FileName + " " + adb.StartInfo.Arguments)
            installStatus += adb.StartInfo.FileName + " " + adb.StartInfo.Arguments
            installStatus += vbCrLf
            SetText(lblStatus, installStatus)
            adb.Start()
            While Not adb.HasExited
                Threading.Thread.Yield()
                If Me.stopAll Then
                    MsgBox("The install has been aborted!", MsgBoxStyle.OkOnly + MsgBoxStyle.Exclamation, "APK Install Aborted")
                    Return
                End If
            End While
            installStatus += adb.StandardOutput.ReadToEnd
            SetText(lblStatus, installStatus)
            'pgbStatus.PerformStep()
        Next
        UseWaitCursor = False
        SetText(lblStatus, "Done!")

        If singleInstall Then
            Me.Close()
        End If

        MsgBox("The installation of the APK(s) has finished successfully!", MsgBoxStyle.Information + MsgBoxStyle.OkOnly, "APK Install Finished")
    End Sub

    Sub UnZip(zipName As String, path As String)
        Dim sc As New Shell32.Shell()
        'Create directory in which you will unzip your files .
        IO.Directory.CreateDirectory(path)
        'Declare the folder where the files will be extracted
        Dim output As Shell32.Folder = sc.NameSpace(path)
        'Declare your input zip file as folder  .
        Dim input As Shell32.Folder = sc.NameSpace(zipName)
        'Extract the files from the zip file using the CopyHere command .
        output.CopyHere(input.Items, 4)
    End Sub

    Private Sub txtFileLocation_TextChanged(sender As Object, e As EventArgs) Handles txtFileLocation.TextChanged
        btnInstall.Enabled = Not (txtFileLocation.Text.Length = 0)

        If btnInstall.Enabled Then
            For Each apkfile As String In getFilesToInstall()
                Dim exists = File.Exists(apkfile)
                If Not exists Then
                    btnInstall.Enabled = False
                End If
            Next
        End If
    End Sub

    Function getAdbExec() As String
        Dim adb As New Process
        adb.StartInfo.Arguments = "version"
        adb.StartInfo.FileName = "adb"
        adb.StartInfo.CreateNoWindow = True
        adb.StartInfo.UseShellExecute = False
        adb.Start()
        adb.WaitForExit()
        If (adb.ExitCode = 0) Then
            Return "adb"
        End If
        Dim tempFileName As String = Path.GetTempFileName()
        File.Delete(tempFileName)
        Directory.CreateDirectory(tempFileName)
        Dim platformToolsZip As String = tempFileName + "\platform-tools.zip"
        File.WriteAllBytes(platformToolsZip, My.Resources.platform_tools_r23_1_0_windows)
        Dim androidPlatformTools As String = tempFileName + "\platform-tools"
        UnZip(platformToolsZip, androidPlatformTools)

        Return androidPlatformTools + "\platform-tools\adb.exe"
    End Function

    Private Sub SetText(ByRef label As Label, ByVal [text] As String)

        ' InvokeRequired required compares the thread ID of the
        ' calling thread to the thread ID of the creating thread.
        ' If these threads are different, it returns true.
        If label.InvokeRequired Then
            Dim d As New SetTextCallback(AddressOf SetText)
            Me.Invoke(d, New Object() {label, [text]})
        Else
            label.Text = [text]
        End If
    End Sub

    Private Sub showProgressAnimation(visible As Boolean)
        If pgbStatus.InvokeRequired Then
            Dim d As New VisibilityCallback(AddressOf showProgressAnimation)
            Me.Invoke(d, New Object() {visible})
        Else
            pgbStatus.Visible = visible
        End If
    End Sub

    Private Sub APKInstallerMain_Load(sender As Object, e As EventArgs) Handles Me.Load
        'Are we launching as a click-once?
        'If (Application.ApplicationDeployment.IsNetworkDeployed) Then
        Dim apkFile As String = ""
        Try
            If Not (AppDomain.CurrentDomain.SetupInformation Is Nothing Or
                        AppDomain.CurrentDomain.SetupInformation.ActivationArguments Is Nothing Or
                        AppDomain.CurrentDomain.SetupInformation.ActivationArguments.ActivationData Is Nothing) Then
                Dim urifile As String = New Uri(AppDomain.CurrentDomain.SetupInformation.ActivationArguments.ActivationData(0)).LocalPath
                If urifile.Contains(".apk") Then
                    If Not MsgBox("I am going to install the APK in the location """ & urifile & """", MsgBoxStyle.Information + MsgBoxStyle.OkCancel).Equals(MsgBoxResult.Ok) Then
                        Me.Close()
                        Return
                    End If
                    addFilesToInstall(New String() {urifile})
                    apkFile = urifile
                End If
            End If

        Catch ex As Exception
                MsgBox("An error occurred with checking ClickOnce " & ex.ToString, MsgBoxStyle.Exclamation + MsgBoxStyle.OkOnly)
            End Try
        ' End If

        For Each arg As String In My.Application.CommandLineArgs
            If Not MsgBox("I am going to install the APK in the location """ & arg & """", MsgBoxStyle.Information + MsgBoxStyle.OkCancel).Equals(MsgBoxResult.Ok) Then
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
                MsgBox("An error occurring opening the specified file!", MsgBoxStyle.OkOnly + MsgBoxStyle.Critical)
            End If

        End If
            addFilesToInstall(fileArgs)
        If Not (fileArgs.Length = 0) And txtFileLocation.Text.Length > 0 And btnInstall.Enabled Then
            Me.singleInstall = True
            Button2_Click(btnInstall, e)
            Me.txtFileLocation.Visible = False
            Me.btnInstall.Visible = False
            Me.btnOpenFileDialogTrigger.Visible = False
            Me.chkForce.Visible = False
            Me.chkReinstall.Visible = False
            Me.chkReinstall.Checked = MsgBox("Would you like to update the installed package on the phone?", MsgBoxStyle.Question + MsgBoxStyle.YesNo).Equals(MsgBoxResult.Yes)
            If Not chkReinstall.Checked Then
                Me.chkForce.Checked = MsgBox("Would you like to forcefully remove any existing package to prep for a clean re-installation?", MsgBoxStyle.Question + MsgBoxStyle.YesNo).Equals(MsgBoxResult.Yes)
            End If

        End If

    End Sub

    Private Sub Form1_Closed(sender As Object, e As EventArgs) Handles Me.Closed
        Me.stopAll = True
    End Sub
End Class
