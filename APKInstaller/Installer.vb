Imports System.IO
Imports System.Threading
Imports APKInstaller

Public Class Installer
    Private Delegate Sub deviceIdCallback(ByVal deviceId As String)
    Private GUI As Main
    Private lblStatus As MaterialSkin.Controls.MaterialLabel
    Private txtUserInput As MaterialSkin.Controls.MaterialSingleLineTextField
    Private stopRequested As Boolean = False
    Private update As Boolean = True
    Private showCompletionMessage As Boolean = True

    Sub New(ByRef entry As Main, ByRef statusLabel As MaterialSkin.Controls.MaterialLabel, ByRef userInputTextbox As MaterialSkin.Controls.MaterialSingleLineTextField)
        GUI = entry
        lblStatus = statusLabel
        txtUserInput = userInputTextbox
    End Sub

    Private Function GetShell32Folder(folderPath As String) As Shell32.Folder
        Dim shellAppType As Type = Type.GetTypeFromProgID("Shell.Application")
        Dim Shell As Object = Activator.CreateInstance(shellAppType)
        Return CType(shellAppType.InvokeMember("NameSpace", Reflection.BindingFlags.InvokeMethod, Nothing, Shell, New Object() {folderPath}), Shell32.Folder)
    End Function

    Private Sub WaitForDevice(adbLocation As String)
        Dim adbWait As New Process()
        adbWait.StartInfo.FileName = adbLocation
        adbWait.StartInfo.Arguments = "wait-for-device"
        adbWait.StartInfo.UseShellExecute = False
        adbWait.StartInfo.CreateNoWindow = True
        GUI.SetText(lblStatus, "Waiting for device.")
        adbWait.Start()

        Dim caret As Integer = 0
        While Not adbWait.HasExited
            GUI.UseWaitCursor = True
            GUI.ShowProgressAnimation(True)
            If caret = 0 Then
                GUI.SetText(lblStatus, "Waiting for device.")
            ElseIf caret = 1 Then
                GUI.SetText(lblStatus, "Waiting for device..")
            Else
                GUI.SetText(lblStatus, "Waiting for device...")
                caret = -1
            End If
            caret += 1

            'Thread.Sleep(1000)
            'Threading.Thread.Yield()
            If stopRequested Then
                MsgBox("The install has been aborted", CType(MsgBoxStyle.OkOnly + MsgBoxStyle.Exclamation, MsgBoxStyle), "APK Install Aborted")
                Return
            End If
        End While
        GUI.ShowProgressAnimation(False)
        adbWait.Close()
    End Sub

    Function GetAdbExecutable() As String
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

        ' Fall-back to app version
        Dim tempFileName As String = Path.GetTempFileName()
        File.Delete(tempFileName)
        Directory.CreateDirectory(tempFileName)
        Dim platformToolsZip As String = tempFileName + "\platform-tools.zip"
        File.WriteAllBytes(platformToolsZip, My.Resources.platform_tools_r23_1_0_windows)
        Dim androidPlatformTools As String = tempFileName + "\platform-tools"
        UnZip(platformToolsZip, androidPlatformTools)

        Return androidPlatformTools + "\platform-tools\adb.exe"
    End Function

    Private Sub UnZip(zipName As String, path As String)
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

    Private Sub GetDeviceId(callback As deviceIdCallback)
        Dim deviceChooser As New dlgDeviceChoose()
        Dim result = deviceChooser.getUserInput(GetAdbExecutable)
        If result = DialogResult.OK Then
            If (MsgBox("Is the device """ + deviceChooser.getDevice + """ correct?", CType(MsgBoxStyle.YesNo + MsgBoxStyle.Question, MsgBoxStyle)) = MsgBoxResult.Yes) Then
                callback(deviceChooser.getDevice)
            Else
                GetDeviceId(callback)
            End If
        ElseIf result = DialogResult.Cancel Then
            abort()
        Else
            callback(deviceChooser.getDevice)
        End If
    End Sub

    Sub AddFilesToInstall(files() As String)
        Dim location As String = ""
        If GetFilesToInstall.Length > 0 And GetFilesToInstall(0) IsNot "" Then
            If MsgBox("There are other APK files to install. Do you want to keep them and add this to install?", CType(MsgBoxStyle.Question + MsgBoxStyle.YesNo, MsgBoxStyle)) = MsgBoxResult.Yes Then
                location = txtUserInput.Text + ";"
            End If
        End If

        For Each path As String In files
            If path Is Nothing Then
                Continue For
            End If

            If path.ToLower.EndsWith(".apk") Then
                location += path + ";"
            Else
                MsgBox("""" + path + """" + " is not a valid Android app. Please verify that the file ends with "".APK"".", CType(MsgBoxStyle.OkOnly + MsgBoxStyle.Exclamation, MsgBoxStyle), "Invalid File")
            End If
        Next

        If location.EndsWith(";") Then
            location = location.Substring(0, location.Length - 1)
        End If
        txtUserInput.Text = location
    End Sub

    Function VerifyFilesToInstall() As Boolean
        For Each apkfile As String In GetFilesToInstall()
            Dim exists = File.Exists(apkfile)
            If Not exists Then
                Return False
            End If
        Next

        Return True
    End Function

    Sub StartInstall()
        Dim thread As New Thread(New ThreadStart(AddressOf Install))
        thread.Start()
    End Sub

    Sub ConfigureReInstall(enabled As Boolean)
        update = enabled
    End Sub

    Sub ShowCompletionMessageWhenFinished(show As Boolean)
        showCompletionMessage = show
    End Sub

    Private Sub Install()
        Dim adbLocation As String = GetAdbExecutable()

        ' Wait until an Android device is connected
        WaitForDevice(adbLocation)

        ' Device Found, check if multiple devices are connected, and figure out the device to install to
        Dim deviceId = ""
        GUI.SetText(lblStatus, "Checking device(s)...This may be a moment or two")
        GUI.ShowProgressAnimation(True)
        GetDeviceId(Sub(ByVal _DeviceId As String)
                        deviceId = _DeviceId
                    End Sub)
        While deviceId Is ""
            If (stopRequested) Then
                GUI.SetText(lblStatus, "The install did not complete successfully :(")
                GUI.ShowProgressAnimation(False)
                Return

            End If
            Thread.Sleep(50)
        End While
        GUI.ShowProgressAnimation(False)

        GUI.SetText(lblStatus, "Starting Installs...")
        Dim filesToInstall As String() = GetFilesToInstall()
        'pgbStatus.Step = 100 / filesToInstall.Length
        Dim installStatus As String = ""
        Dim installAborted = False
        For Each file As String In GetFilesToInstall()
            While True
                Dim adb As New Process()
                Dim arguments As String = "-s " & deviceId & " install "
                If update Then
                    arguments += "-r "
                End If
                adb.StartInfo.Arguments = arguments & """" & file & """"
                adb.StartInfo.CreateNoWindow = True
                adb.StartInfo.UseShellExecute = False
                adb.StartInfo.FileName = adbLocation
                adb.StartInfo.RedirectStandardOutput = True
                installStatus += adb.StartInfo.FileName + " " + adb.StartInfo.Arguments
                installStatus += vbCrLf
                GUI.SetText(GUI.lblStatus, installStatus)
                adb.Start()
                While Not adb.HasExited
                    If stopRequested Then
                        MsgBox("The install has been aborted!", CType(MsgBoxStyle.OkOnly + MsgBoxStyle.Exclamation, MsgBoxStyle), "APK Install Aborted")
                        Return
                    End If
                End While
                installStatus += adb.StandardOutput.ReadToEnd
                GUI.SetText(GUI.lblStatus, installStatus)

                If Not adb.ExitCode = 0 Then
                    installStatus += adb.StandardOutput.ReadToEnd
                    GUI.SetText(GUI.lblStatus, installStatus)
                    Dim result As MsgBoxResult = MsgBox("The installation of """ & file & """ did not succeed." & vbCrLf & "Details: ADB exited with error code " & adb.ExitCode, CType(MsgBoxStyle.Exclamation + MsgBoxStyle.AbortRetryIgnore, MsgBoxStyle))
                    If result = MsgBoxResult.Abort Then
                        Exit For
                    ElseIf result = MsgBoxResult.Retry Then
                        Continue While
                    End If
                End If
                Exit While
            End While
        Next
        GUI.UseWaitCursor = False
        If Not installAborted Then
            GUI.SetText(lblStatus, "Done!")
        Else
            GUI.SetText(lblStatus, "Failure! Details: " & vbCrLf & installStatus)
        End If

        If Not installAborted And showCompletionMessage Then
            MsgBox("The installation of the APK(s) has finished successfully!", CType(MsgBoxStyle.Information + MsgBoxStyle.OkOnly, MsgBoxStyle), "APK Install Finished")
        End If
    End Sub

    Function GetFilesToInstall() As String()
        Return txtUserInput.Text.Split(CType(";", Char()))
    End Function

    Sub abort()
        stopRequested = True
    End Sub
End Class
