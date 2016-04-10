Imports System.IO
Imports System.IO.Compression

Public Class Form1
    ' This delegate enables asynchronous calls for setting
    ' the text property on a TextBox control.
    Delegate Sub SetTextCallback(ByRef label As Label, ByVal [text] As String)
    Delegate Sub VisibilityCallback(visible As Boolean)

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
        For Each path In files
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
            adb.WaitForExit()
            installStatus += adb.StandardOutput.ReadToEnd
            SetText(lblStatus, installStatus)
            'pgbStatus.PerformStep()
        Next
        UseWaitCursor = False
        SetText(lblStatus, "Done!")
    End Sub

    Sub UnZip(zipName As String, path As String)
        'Create directory in which you will unzip your files .
        IO.Directory.CreateDirectory(path)
        Dim input As Shell32.Folder
        Dim output As Shell32.Folder
        Try
            Dim sc As Shell32.Shell = New Shell32.Shell()
            'Declare the folder where the files will be extracted
            output = sc.NameSpace(path)
            'Declare your input zip file as folder  .
            input = sc.NameSpace(zipName)
        Catch ex As InvalidCastException
            input = GetShell32Folder(zipName)
            output = GetShell32Folder(path)
        End Try

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
        Try
            adb.Start()
            adb.WaitForExit()
            If (adb.ExitCode = 0) Then
                Return "adb"
            End If
        Catch ex As Exception
            ' Continue on
        End Try

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
        For Each arg As String In My.Application.CommandLineArgs
            MsgBox(arg)
        Next

        Dim toArray As String() = My.Application.CommandLineArgs.ToArray
        addFilesToInstall(toArray)
        If Not (toArray.Length = 0) And txtFileLocation.Text.Length > 0 And btnInstall.Enabled Then
            Button2_Click(btnInstall, e)
        End If

    End Sub

    Private Function GetShell32Folder(folderPath As String) As Shell32.Folder
        Dim shellAppType As Type = Type.GetTypeFromProgID("Shell.Application")
        Dim Shell As Object = Activator.CreateInstance(shellAppType)
        Return shellAppType.InvokeMember("NameSpace", Reflection.BindingFlags.InvokeMethod, Nothing, Shell, New Object() {folderPath})
    End Function
End Class
