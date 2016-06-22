﻿Imports System.IO
Imports System.Threading
Imports ICSharpCode.SharpZipLib.Zip

Public Class Installer
    Private Delegate Sub deviceIdCallback(ByVal deviceId As String)
    Private GUI As Main
    Private lblStatus As MaterialSkin.Controls.MaterialLabel
    Private txtUserInput As MaterialSkin.Controls.MaterialSingleLineTextField
    Private stopRequested As Boolean = False
    Private update As Boolean = True
    Private showCompletionMessage As Boolean = True
    Private adbCache As String
    Private aaptLocation As String
    Private force As Boolean

    Sub New(ByRef entry As Main, ByRef statusLabel As MaterialSkin.Controls.MaterialLabel, ByRef userInputTextbox As MaterialSkin.Controls.MaterialSingleLineTextField)
        GUI = entry
        lblStatus = statusLabel
        txtUserInput = userInputTextbox
    End Sub

    Private Function GetShell32Folder(folderPath As String) As Shell32.Folder
        Dim shellAppType As Type = Type.GetTypeFromProgID("Shell.Application")
        Dim Shell = Activator.CreateInstance(shellAppType)
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
            GUI.ShowProgressAnimation(True, False)
            If caret = 0 Then
                GUI.SetText(lblStatus, "Waiting for device.")
            ElseIf caret = 1 Then
                GUI.SetText(lblStatus, "Waiting for device..")
            Else
                GUI.SetText(lblStatus, "Waiting for device...")
                caret = -1
            End If
            caret += 1

            Thread.Sleep(1000)
            Threading.Thread.Yield()
            If stopRequested Then
                MsgBox("The install has been aborted", CType(MsgBoxStyle.OkOnly + MsgBoxStyle.Exclamation, MsgBoxStyle), "APK Install Aborted")
                Exit Sub
            End If
        End While
        GUI.ShowProgressAnimation(False, False)
        adbWait.Close()
    End Sub

    Function GetAdbExecutable() As String
        If adbCache IsNot Nothing And (adbCache Is "adb" Or File.Exists(adbCache)) Then
            Return adbCache
        End If

        Dim adb As New Process
        adb.StartInfo.Arguments = "version"
        adb.StartInfo.FileName = "adb"
        adb.StartInfo.CreateNoWindow = True
        adb.StartInfo.UseShellExecute = False
        Try
            adb.Start()
            adb.WaitForExit()
            If (adb.ExitCode = 0) Then
                adbCache = "adb"
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
        UnzipFromStream(New FileStream(platformToolsZip, FileMode.Open), androidPlatformTools)
        adbCache = androidPlatformTools & "\platform-tools\adb.exe"
        Return adbCache
    End Function



    Public Sub UnzipFromStream(zipStream As FileStream, outFolder As String)
        Dim zipInputStream = New ZipInputStream(zipStream)
        Dim zipEntry = zipInputStream.GetNextEntry()
        Dim buffer(4096) As Byte    ' 4K Is optimum
        While (zipEntry IsNot Nothing)
            Dim entryFileName = zipEntry.Name
            ' Convert UNIX paths to the current platform path
            entryFileName = entryFileName.Replace("/", Path.DirectorySeparatorChar)
            Dim fullZipToPath = Path.Combine(outFolder, entryFileName)
            Dim directoryName = Path.GetDirectoryName(fullZipToPath)

            If (directoryName.Length > 0) Then
                Directory.CreateDirectory(directoryName)
            End If
            If Not (directoryName & "\").Equals(fullZipToPath) Then
                Using _streamWriter As FileStream = File.Create(fullZipToPath)
                    ICSharpCode.SharpZipLib.Core.StreamUtils.Copy(zipInputStream, _streamWriter, buffer)
                End Using
            End If


            zipEntry = zipInputStream.GetNextEntry()
        End While
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
                location = txtUserInput.Text & ";"
            End If
        End If

        For Each path As String In files
            If path Is Nothing Then
                Continue For
            End If

            Dim invalidPackage = Not path.ToLower.EndsWith(".apk")

            Try
                If (Not invalidPackage And packageName(path) Is "") Then
                    invalidPackage = True
                End If
            Catch ex As Exception
                MsgBox(ex.Message)
            End Try

            If Not invalidPackage Then
                location += path + ";"
            Else
                MsgBox("""" & path & """" & " is not a valid Android app. Please verify that the file ends with "".APK"".", CType(MsgBoxStyle.OkOnly + MsgBoxStyle.Exclamation, MsgBoxStyle), "Invalid File")
            End If
        Next

        If location.EndsWith(";") Then
            location = location.Substring(0, location.Length - 1)
        End If
        txtUserInput.Text = location
    End Sub

    Function VerifyFilesToInstall() As Boolean
        Dim apkFiles = GetFilesToInstall()
        If apkFiles.Length = 0 Then
            Return False
        End If

        For Each apkfile As String In apkFiles
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

    Sub ConfigureForce(enabled As Boolean)
        force = enabled
    End Sub

    Sub ShowCompletionMessageWhenFinished(show As Boolean)
        showCompletionMessage = show
    End Sub

    Private Sub Install()
        Dim adbLocation As String = GetAdbExecutable()

        ' Wait until an Android device is connected
        WaitForDevice(adbLocation)
        If stopRequested Then
            Return
        End If

        ' Device Found, check if multiple devices are connected, and figure out the device to install to
        Dim deviceId = ""
        GUI.SetText(lblStatus, "Checking device(s)...This may be a moment or two")
        GUI.ShowProgressAnimation(True, False)
        GetDeviceId(Sub(ByVal _DeviceId As String)
                        deviceId = _DeviceId
                    End Sub)
        While deviceId Is ""
            If (stopRequested) Then
                GUI.SetText(lblStatus, "The install did not complete successfully :(")
                GUI.ShowProgressAnimation(False, False)
                Return

            End If
            Thread.Sleep(50)
        End While
        GUI.ShowProgressAnimation(False, False)

        GUI.SetText(lblStatus, "Starting Installs...")
        Dim filesToInstall As String() = GetFilesToInstall()
        'pgbStatus.Step = 100 / filesToInstall.Length
        Dim installStatus As String = ""
        Dim installAborted = False
        GUI.ShowProgressAnimation(True, True, CInt(100 / filesToInstall.Length))

        For Each file As String In filesToInstall
            While True
                If (force) Then
                    ForceRemovePackage(file)
                End If

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
                Else
                    GUI.StepProgressBar()
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

    Function ForceRemovePackage(ByVal apkFile As String) As Boolean
        Dim adb As New Process()
        adb.StartInfo.FileName = GetAdbExecutable()
        adb.StartInfo.Arguments = "uninstall " & packageName(apkFile)
        adb.StartInfo.CreateNoWindow = True
        adb.StartInfo.UseShellExecute = False
        adb.Start()
        adb.WaitForExit()

        Return adb.ExitCode = 0
    End Function

    Function getAaptLocation() As String
        If (aaptLocation IsNot Nothing) Then
            If (File.Exists(aaptLocation)) Then
                Return aaptLocation
            End If
        End If
        Dim aapt = ""
        While aapt Is ""
            Try
                Dim temp = Path.GetTempFileName()
                File.Delete(temp)
                Directory.CreateDirectory(temp)
                aapt = Path.Combine(temp, "aapt.exe")
            Catch ex As Exception

            End Try
        End While

        File.WriteAllBytes(aapt, My.Resources.aapt_23_0_3_win)
        If File.Exists(aapt) Then
            aaptLocation = aapt
            Return aapt
        Else
            Throw New IOException("Failed to build aapt.exe")
        End If
    End Function

    Function packageName(ByVal apkFile As String) As String
        Dim aapt As New Process()
        aapt.StartInfo.FileName = getAaptLocation()
        aapt.StartInfo.Arguments = "dump badging """ & apkFile & """"
        aapt.StartInfo.CreateNoWindow = True
        aapt.StartInfo.UseShellExecute = False
        aapt.StartInfo.RedirectStandardOutput = True
        aapt.Start()

        Const parseFor = "package: name="
        Dim package = ""
        Dim line As String = aapt.StandardOutput.ReadLine
        Try
            While line IsNot Nothing
                'Detect interrupts
                Threading.Thread.Sleep(1)

                line = line.Trim
                If aapt.HasExited Then
                    If Not aapt.ExitCode = 0 Then
                        Throw New Exception("AAPT Failed. Exit: " & aapt.ExitCode)
                    End If
                End If
                If line.Contains(parseFor) Then
                    Dim versionName As String = line.Substring(line.IndexOf(parseFor) + parseFor.Length)
                    package = versionName.Substring(versionName.IndexOf("'") + 1)
                    package = package.Substring(0, package.IndexOf("'"))
                    Exit While

                End If
                line = aapt.StandardOutput.ReadLine
            End While
            If aapt.HasExited Then
                If Not aapt.ExitCode = 0 Then
                    Throw New Exception("AAPT Failed. Exit: " & aapt.ExitCode)
                End If
            End If
        Catch ex As Exception
            Dim exception As New IOException("Failed to acquire package name", ex)
            Throw exception
        End Try
        Return package
    End Function
End Class
