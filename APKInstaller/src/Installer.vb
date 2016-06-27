Imports System.IO
Imports System.Threading

Public Class Installer
    Private Delegate Sub deviceIdCallback(ByVal deviceId As String)
    Private GUI As Main
    Private lblStatus As MaterialSkin.Controls.MaterialLabel
    Private txtUserInput As MaterialSkin.Controls.MaterialSingleLineTextField
    Private stopRequested As Boolean = False
    Private update As Boolean = True
    Private showCompletionMessage As Boolean = True
    Private pForce As Boolean = False

    <CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1045:DoNotPassTypesByReference", MessageId:="2#")>
    <CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1045:DoNotPassTypesByReference", MessageId:="1#")>
    <CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1045:DoNotPassTypesByReference", MessageId:="0#")>
    Sub New(ByRef entry As Main, ByRef statusLabel As MaterialSkin.Controls.MaterialLabel, ByRef userInputTextBox As MaterialSkin.Controls.MaterialSingleLineTextField)
        GUI = entry
        lblStatus = statusLabel
        txtUserInput = userInputTextBox
    End Sub

    Property Reinstall As Boolean
        Get
            Return update
        End Get
        Set(value As Boolean)
            update = value
        End Set
    End Property

    Property Force As Boolean
        Get
            Return pForce
        End Get
        Set(value As Boolean)
            pForce = value
        End Set
    End Property

    Property CompletionMessageWhenFinished As Boolean
        Get
            Return showCompletionMessage
        End Get
        Set(value As Boolean)
            showCompletionMessage = value
        End Set
    End Property

    Sub AddFilesToInstall(files() As String)
        If files Is Nothing Then
            Throw New ArgumentNullException(NameOf(files))
        End If

        Dim location As String = ""

        ' Check for already present files
        If GetFilesToInstall.Length > 0 And GetFilesToInstall(0) IsNot "" Then
            If MsgBox("There are other APK files to install. Do you want to keep them and add this to install?", CType(MsgBoxStyle.Question + MsgBoxStyle.YesNo, MsgBoxStyle)) = MsgBoxResult.Yes Then
                location = txtUserInput.Text & Path.PathSeparator
            End If
        End If

        For Each path As String In files
            If path Is Nothing Then ' Prevent bugs in detection routine
                Continue For
            End If

            Try
                ' Check to see if the file exists
                If Not File.Exists(path) Then
                    MsgBox("""" & path & """ " & If(Directory.Exists(path),
                           "is a directory. Unfortunately, this app can't handle installing directories, for now.",
                           "doesn't seem to exist at this moment. You may want to try that file again in a moment or two."))
                    Continue For
                End If

                ' Check for the correct extension and that the file can be correctly parsed as an APK
                If path.ToLower.EndsWith(".apk") And AndroidTools.PackageName(path) Is "" Then
                    MsgBox("""" & path & """" & " is not a valid Android app. Please verify that the file ends with "".APK"".",
                           CType(MsgBoxStyle.OkOnly + MsgBoxStyle.Exclamation, MsgBoxStyle), "Invalid File")
                Else
                    location &= path & IO.Path.PathSeparator
                End If
            Catch ex As IOException
                MsgBox(ex.Message)
            End Try
        Next path

        ' Prevent verify errors by removing path separator
        If location.EndsWith(Path.PathSeparator) Then
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


    Private Sub Install()
        ' Wait until an Android device is connected
        WaitForDevice()
        If stopRequested Then
            Return
        End If

        ' Device Found, check if multiple devices are connected, and figure out the device to install to
        Dim deviceId = AcquireDeviceId()
        If deviceId Is Nothing Then
            Return
        End If

        ' Begin device install sections
        GUI.SetText(lblStatus, "Starting Installs...")
        Dim filesToInstall As String() = GetFilesToInstall()
        'pgbStatus.Step = 100 / filesToInstall.Length
        Dim installStatus As String = ""
        Dim installAborted = False
        GUI.ShowProgressAnimation(True, True, CInt(100 / filesToInstall.Length))
        Const MAX_RETRY_COUNT = 3
        For Each file As String In filesToInstall
            If installAborted Then ' Check if the install has been aborted, if so stop the installs
                Exit For
            End If
            ' Retry Loop
            For retry = 0 To MAX_RETRY_COUNT
                If Force Then
                    ForceRemovePackage(file)
                End If

                GUI.SetText(GUI.lblStatus, installStatus)
                Dim adb As Process = InstallSinglePackage(deviceId, installStatus, file)
                If adb Is Nothing Then
                    Return
                End If

                ' Configure GUI to show last install
                installStatus += adb.StandardOutput.ReadToEnd
                GUI.SetText(GUI.lblStatus, installStatus)

                If Not adb.ExitCode = 0 Then
                    Select Case HandleInstallFailure(file, adb)
                        Case MsgBoxResult.Abort
                            installAborted = True
                        Case MsgBoxResult.Ignore
                            Exit For ' Don't retry
                        Case Else
                            For second As Integer = 5 To 1 Step -1
                                Dim message = "Retrying package in " & second & If(second = 1, "second", "seconds")
                                GUI.SetText(GUI.lblStatus, message)
                            Next second
                            ' Continue For
                    End Select
                Else
                    GUI.StepProgressBar()
                    Exit For ' Go to next file b/c this file is a success
                End If
            Next retry ' End retry loop
        Next file ' for each file
        GUI.UseWaitCursor = False
        If Not installAborted Then
            GUI.SetText(lblStatus, "Done!")
            If showCompletionMessage Then
                MsgBox("The installation of the APK" & If(filesToInstall.Length = 1, "", "s") & " has finished successfully!", CType(MsgBoxStyle.Information + MsgBoxStyle.OkOnly, MsgBoxStyle), "APK Install Finished")
            End If
        Else
            GUI.SetText(lblStatus, "Failure! Details:  " & vbCrLf & installStatus)
        End If
        ' End device install section
    End Sub

    Private Sub WaitForDevice()
        Dim adbWait = AndroidTools.RunAdb("wait-for-device", False, True, False)
        ' GUI.SetText(lblStatus, "Waiting for device.")

        Dim caret As Integer = 0
        GUI.UseWaitCursor = True
        GUI.ShowProgressAnimation(True, False)
        While Not adbWait.HasExited
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

    Private Function AcquireDeviceId() As String
        GUI.SetText(lblStatus, "Checking device(s)...This may be a moment or two")
        GUI.ShowProgressAnimation(True, False)

        Dim deviceId = ""
        GetDeviceId(Sub(ByVal _DeviceId As String)
                        deviceId = _DeviceId
                    End Sub)
        While deviceId Is ""
            If (stopRequested) Then
                GUI.SetText(lblStatus, "The install did not complete successfully :(")
                GUI.ShowProgressAnimation(False, False)
                Return Nothing

            End If
            Thread.Sleep(50)
        End While

        GUI.ShowProgressAnimation(False, False)

        Return deviceId
    End Function



    Private Sub GetDeviceId(callback As deviceIdCallback)
        Dim deviceChooser As New DeviceChooserDialog()
        Dim result = deviceChooser.GetUserInput()
        If result = DialogResult.OK Then
            If (MsgBox("Is the device """ + deviceChooser.Device + """ correct?", CType(MsgBoxStyle.YesNo + MsgBoxStyle.Question, MsgBoxStyle)) = MsgBoxResult.Yes) Then
                callback(deviceChooser.Device)
            Else
                GetDeviceId(callback)
            End If
        ElseIf result = DialogResult.Cancel Then
            Abort()
        Else
            callback(deviceChooser.Device)
        End If
    End Sub

    Function GetFilesToInstall() As String()
        Return txtUserInput.Text.Split(CType(Path.PathSeparator.ToString, Char()))
    End Function

    Shared Function ForceRemovePackage(ByVal apkFile As String) As Boolean
        Dim adb = AndroidTools.RunAdb("uninstall " & AndroidTools.PackageName(apkFile), False, True, True)
        Return adb.ExitCode = 0
    End Function

    Private Function InstallSinglePackage(deviceId As String, installStatus As String, file As String) As Process
        Dim adb = AndroidTools.RunAdb("-s " & deviceId & " install " & If(update, "-r ", "") & """" & file & """", True, True, False)

        Dim haltInstall = False
        While Not adb.HasExited
            If stopRequested Then
                If MsgBox("Do you want to abort installing the current APK? The currently installing package will probably be unusable if you chose to abort.", CType(MsgBoxStyle.YesNo + MsgBoxStyle.Question, Global.Microsoft.VisualBasic.MsgBoxStyle)) = MsgBoxResult.No Then
                    haltInstall = True
                    stopRequested = False
                Else
                    adb.Kill()
                    MsgBox("The install has been aborted!", CType(MsgBoxStyle.OkOnly + MsgBoxStyle.Exclamation, MsgBoxStyle), "APK Install Aborted")
                    Return Nothing
                End If

            End If
            Thread.Sleep(10)
        End While

        If haltInstall Then
            MsgBox("The install has been aborted!", CType(MsgBoxStyle.OkOnly + MsgBoxStyle.Exclamation, MsgBoxStyle), "APK Install Aborted")
            Return Nothing
        End If

        Return adb
    End Function

    Private Shared Function HandleInstallFailure(file As String, adb As Process) As MsgBoxResult
        Dim result As MsgBoxResult = MsgBox("The installation of """ & file & """ did not succeed." & vbCrLf & "Details: ADB exited with error code " & adb.ExitCode, CType(MsgBoxStyle.Exclamation + MsgBoxStyle.AbortRetryIgnore, MsgBoxStyle))
        Return result
    End Function

    Sub Abort()
        stopRequested = True
    End Sub
End Class
