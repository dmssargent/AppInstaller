﻿Imports System.IO
Imports System.Threading

''' <summary>
''' Handler of installing Android packages
''' </summary>
Public Class Installer
    Private Delegate Sub deviceIdCallback(ByVal deviceId As String)
    Private GUI As Main
    Private lblStatus As MaterialSkin.Controls.MaterialLabel
    Private txtUserInput As MaterialSkin.Controls.MaterialSingleLineTextField
    Private stopRequested As Boolean = False
    Private update As Boolean = True
    Private showCompletionMessage As Boolean = True
    Private pForce As Boolean = False
    Private filesToInstall As LinkedList(Of String) = New LinkedList(Of String)
    Private filesToInstallDesc As String = ""
    Private multiFileDialog As Boolean = False

    ''' <summary>
    ''' Creates a new installer instance
    ''' </summary>
    ''' <param name="entry">The main GUI of the application</param>
    ''' <param name="statusLabel">The label to use as installer statuses</param>
    ''' <param name="userInputTextBox">The user input textbox</param>
    <CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1045:DoNotPassTypesByReference", MessageId:="2#")>
    <CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1045:DoNotPassTypesByReference", MessageId:="1#")>
    <CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1045:DoNotPassTypesByReference", MessageId:="0#")>
    Sub New(ByRef entry As Main, ByRef statusLabel As MaterialSkin.Controls.MaterialLabel, ByRef userInputTextBox As MaterialSkin.Controls.MaterialSingleLineTextField)
        GUI = entry
        lblStatus = statusLabel
        txtUserInput = userInputTextBox
    End Sub

    ''' <summary>
    ''' Determines if the operations performed are update (reinstall) operations, or not
    ''' </summary>
    ''' <returns>true if the packages are going to be reinstalled; otherwise false</returns>
    Property Reinstall As Boolean
        Get
            Return update
        End Get
        Set(value As Boolean)
            update = value
        End Set
    End Property

    ''' <summary>
    ''' Whether to force the package installation or not
    ''' </summary>
    ''' <returns>true if the packages will be forcibly installed</returns>
    Property Force As Boolean
        Get
            Return pForce
        End Get
        Set(value As Boolean)
            pForce = value
        End Set
    End Property

    ''' <summary>
    ''' Shows a completion message after the installation of all packages is finished
    ''' </summary>
    ''' <returns>if a completion message will be shown after the installation job is finished</returns>
    Property CompletionMessageWhenFinished As Boolean
        Get
            Return showCompletionMessage
        End Get
        Set(value As Boolean)
            showCompletionMessage = value
        End Set
    End Property

    Public Property UseMultiFileDialog As Boolean
        Get
            Return multiFileDialog
        End Get
        Set(value As Boolean)
            multiFileDialog = value
        End Set
    End Property

    Public ReadOnly Property FilesToInstallDescription As String
        Get
            Return filesToInstallDesc
        End Get
    End Property

    ''' <summary>
    ''' Adds files to be installed
    ''' </summary>
    ''' <param name="files">files to be installed</param>
    Sub AddFilesToInstall(files() As String, Optional clear As Boolean = False, Optional notifyUser As Boolean = True)
        If files Is Nothing Then
            Throw New ArgumentNullException(NameOf(files))
        End If

        If clear Then
            filesToInstall.Clear()
        End If

        For Each path As String In files
            If path Is Nothing Then ' Prevent bugs in detection routine
                Continue For
            End If

            path = path.Trim()

            ' Determine if the file is already in the installer list, if so skip it
            If filesToInstall.Contains(path) Then
                Continue For
            End If

            Try
                If ValidateFile(path, notifyUser) Then
                    filesToInstall.AddLast(path)
                End If
            Catch ex As IOException
                MsgBox(ex.Message)
            End Try
        Next path

        txtUserInput.Text = GenerateFileListSummary()
        filesToInstallDesc = txtUserInput.Text
    End Sub

    Public Sub RemoveFile(path As String)
        filesToInstall.Remove(path)
    End Sub

    ''' <summary>
    ''' Determines if the given file is a valid APK file
    ''' </summary>
    ''' <param name="path">the file location for the APK file</param>
    ''' <param name="notifyUser">true will notify the user of the problem via dialogs, false will not</param>
    ''' <returns>true if the file is a valid APK, false otherwise</returns>
    Public Function ValidateFile(path As String, Optional notifyUser As Boolean = False) As Boolean
        ' Check to see if the file exists
        If Not File.Exists(path) Then
            If notifyUser Then
                MsgBox("""" & path & """ " & If(Directory.Exists(path),
                   My.Resources.Strings.isDirError,
                   My.Resources.Strings.fileDoesNotExist))
            End If
            Return False
        End If

        ' Check for the correct extension and that the file can be correctly parsed as an APK
        If path.ToUpper(Globalization.CultureInfo.CurrentCulture).EndsWith(".APK", StringComparison.CurrentCultureIgnoreCase) And
            AndroidTools.PackageName(path) Is "" Then
            If notifyUser Then
                MsgBox("""" & path & """" & My.Resources.Strings.invalidApk,
                   CType(MsgBoxStyle.OkOnly + MsgBoxStyle.Exclamation, MsgBoxStyle), "Invalid File")
            End If
            Return False
        Else
            Return True
        End If
    End Function

    Private Function GenerateFileListSummary() As String
        If filesToInstall.Count = 0 Then
            multiFileDialog = False
            Return ""
        ElseIf filesToInstall.Count = 1 Then
            multiFileDialog = False
            Return filesToInstall.First.Value
        Else
            multiFileDialog = True
            Return filesToInstall.ElementAt(0) & ", " & filesToInstall.ElementAt(1) & If(filesToInstall.Count > 2, "...", "")
        End If
    End Function

    ''' <summary>
    ''' Verifies that all of the files to be installed are valid
    ''' </summary>
    ''' <returns></returns>
    Function VerifyFilesToInstall() As Boolean
        Dim apkFiles = GetFilesToInstall()

        ' Nothing to install
        If apkFiles.Length = 0 Then
            Return False
        End If

        For Each apkfile As String In apkFiles
            If Not ValidateFile(apkfile) Then
                Return False
            End If
        Next

        Return True
    End Function

    ''' <summary>
    ''' Starts the install of the APK files
    ''' </summary>
    Sub StartInstall()
        Dim thread As New Thread(New ThreadStart(AddressOf Install))
        thread.Start()
    End Sub

    ''' <summary>
    ''' Installs the APK packages
    ''' </summary>
    Private Sub Install()
        ' Wait until an Android device is connected
        Dim waitForDeviceReturn = WaitForDevice()
        If Not waitForDeviceReturn = 0 Then
            HandleWaitForDeviceError(waitForDeviceReturn)
            Return
        End If


        ' Device Found, check if multiple devices are connected, and figure out the device to install to
        Dim deviceId = AcquireDeviceId()
        If deviceId Is Nothing Then
            GUI.ResetGUI(My.Resources.Strings.installAborted)
            Return
        End If

        ' Begin device install sections
        GUI.SetText(lblStatus, My.Resources.Strings.startingInstalls)
        Dim filesToInstall As String() = GetFilesToInstall()
        Dim installStatus As String = ""
        Dim installAborted = False
        GUI.ShowProgressAnimation(True, True, CInt(100 / filesToInstall.Length))
        Const MAX_RETRY_COUNT = 3
        For Each file As String In filesToInstall
            If installAborted Then ' Check if the install has been aborted, if so stop the installs
                'Exit For
            End If
            ' Retry Loop
            For retry = 0 To MAX_RETRY_COUNT
                Select Case PackageInstallAttempt(deviceId, installStatus, file)
                    Case ErrorCode.ABORT
                        installAborted = True
                        Return
                    Case ErrorCode.FAILURE_1
                    Case ErrorCode.FAILURE_2 ' Fall-through
                        Continue For
                    Case ErrorCode.SUCCESS
                    Case ErrorCode.IGNORE ' Fall-through
                        Exit For
                End Select
            Next retry ' End retry loop
        Next file ' for each file
        GUI.UseWaitCursor = False
        If Not installAborted Then
            If showCompletionMessage Then
                MsgBox(My.Resources.Strings.installationOf & If(filesToInstall.Length = 1, My.Resources.Strings.apk, My.Resources.Strings.apks) & My.Resources.Strings.hasFinishedSucess, CType(MsgBoxStyle.Information + MsgBoxStyle.OkOnly, MsgBoxStyle), My.Resources.Strings.apkInstallFinished)
            End If
            GUI.ResetGUI(My.Resources.Strings.done)
        Else
            GUI.ResetGUI(My.Resources.Strings.failure & My.Resources.Strings.details & vbCrLf & installStatus)
        End If
        ' End device install section
    End Sub

    Private Function PackageInstallAttempt(deviceId As String, installStatus As String, file As String) As ErrorCode
        If Force Then
            ForceRemovePackage(file)
        End If

        Dim installAborted = False
        Dim success = False
        GUI.SetText(GUI.lblStatus, installStatus)
        Using adb As Process = InstallSinglePackage(deviceId, file)
            If adb Is Nothing Then ' Install Single Package failed catastrophically when this happened
                Return ErrorCode.FAILURE_1
            End If

            If Not adb.ExitCode = 0 Then
                Select Case HandleInstallFailure(file, adb)
                    Case MsgBoxResult.Abort
                        Return ErrorCode.ABORT
                    Case MsgBoxResult.Ignore
                        Return ErrorCode.IGNORE ' Don't retry
                    Case Else
                        For second As Integer = 5 To 1 Step -1
                            Dim message = My.Resources.Strings.retryIn & second & If(second = 1, My.Resources.Strings.second, My.Resources.Strings.seconds)
                            GUI.SetText(GUI.lblStatus, message)
                        Next second
                        Return ErrorCode.FAILURE_2
                End Select
            Else
                GUI.StepProgressBar()
                Return ErrorCode.SUCCESS ' Go to next file b/c this file is a success
            End If
        End Using
    End Function

    Private Sub HandleWaitForDeviceError(waitForDeviceReturn As Integer)
        If waitForDeviceReturn = ErrorCode.FAILURE_TIMEOUT Then
            GUI.ResetGUI(My.Resources.Strings.timeoutWaiting & vbCrLf &
                                My.Resources.Strings.userTroubleshootingA1 & vbCrLf &
                                My.Resources.Strings.userTroubleshootingA2)
        ElseIf waitForDeviceReturn = ErrorCode.ABORT Then
            GUI.ResetGUI(My.Resources.Strings.installAborted)
        End If
    End Sub

    Private Function WaitForDevice() As Integer
        Dim counter = 0
        Using adbWait = AndroidTools.RunAdb("wait-for-device", False, True, False)
            Dim caret As Integer = 0
            GUI.UseWaitCursor = True
            GUI.ShowProgressAnimation(True, False)
            While Not adbWait.HasExited
                Dim currentMessage = ""
                If caret = 0 Then
                    currentMessage = My.Resources.Strings.waitForDevice1
                ElseIf caret = 1 Then
                    currentMessage = My.Resources.Strings.waitForDevice2
                Else
                    currentMessage = My.Resources.Strings.waitForDevice3
                    caret = -1
                End If
                caret += 1
                If counter > 10 Then
                    currentMessage &= vbCrLf & My.Resources.Strings.userTroubleshootingA1 & vbCrLf &
                                                My.Resources.Strings.userTroubleshootingA2
                End If
                GUI.SetText(lblStatus, currentMessage)

                Thread.Sleep(1000)
                Thread.Yield()
                If stopRequested Then
                    MsgBox(My.Resources.Strings.installAborted, CType(MsgBoxStyle.OkOnly + MsgBoxStyle.Exclamation, MsgBoxStyle), "APK Install Aborted")
                    Return ErrorCode.ABORT
                End If
                counter += 1
                If counter > 60 Then
                    Return ErrorCode.FAILURE_2
                End If
            End While
            GUI.ShowProgressAnimation(False, False)
        End Using
        Return 0
    End Function

    Private Function AcquireDeviceId() As String
        GUI.SetText(lblStatus, My.Resources.Strings.checkDevices)
        GUI.ShowProgressAnimation(True, False)

        Dim deviceId = ""
        GetDeviceId(Sub(ByVal _DeviceId As String)
                        deviceId = _DeviceId
                    End Sub)
        While deviceId Is ""
            If (stopRequested) Then
                GUI.SetText(lblStatus, My.Resources.Strings.installAborted)
                GUI.ShowProgressAnimation(False, False)
                Return Nothing

            End If
            Thread.Sleep(50)
        End While

        GUI.ShowProgressAnimation(False, False)

        Return deviceId
    End Function

    Private Sub GetDeviceId(callback As deviceIdCallback)
        'Using 
        Dim deviceChooser As New DeviceChooserDialog()
        Dim result = deviceChooser.GetUserInput()
            If result = DialogResult.OK Or result = Nothing Then
                If (MsgBox(My.Resources.Strings.correctDevice1 + deviceChooser.Device + My.Resources.Strings.correctDevice2, CType(MsgBoxStyle.YesNo + MsgBoxStyle.Question, MsgBoxStyle)) = MsgBoxResult.Yes) Then
                    callback(deviceChooser.Device)
                Else
                    GetDeviceId(callback)
                End If
            ElseIf result = DialogResult.Cancel Then
                Abort()
            Else
                callback(deviceChooser.Device)
            End If
        'End Using
    End Sub

    Function GetFilesToInstall() As String()
        Return filesToInstall.ToArray
    End Function

    Shared Function ForceRemovePackage(ByVal apkFile As String) As Boolean
        Using adb = AndroidTools.RunAdb("uninstall " & AndroidTools.PackageName(apkFile), False, True, True)
            Return adb.ExitCode = 0
        End Using
    End Function

    Private Function InstallSinglePackage(deviceId As String, file As String) As Process
        Dim adb = AndroidTools.RunAdb("-s " & deviceId & " install " & If(update, "-r ", "") & """" & file & """", True, True, False)

        Dim haltInstall = False
        Dim t = ""
        While Not adb.HasExited
            t &= adb.StandardOutput.ReadToEnd
            If stopRequested Then
                If MsgBox(My.Resources.Strings.abortCurrentApk, CType(MsgBoxStyle.YesNo + MsgBoxStyle.Question, Global.Microsoft.VisualBasic.MsgBoxStyle)) = MsgBoxResult.No Then
                    haltInstall = True
                    stopRequested = False
                Else
                    Try
                        If adb.HasExited Then
                            adb.Kill()
                        End If
                    Catch ex As InvalidOperationException

                    End Try
                    MsgBox(My.Resources.Strings.installAborted, CType(MsgBoxStyle.OkOnly + MsgBoxStyle.Exclamation, MsgBoxStyle), "APK Install Aborted")
                    Return Nothing
                End If

            End If
            Thread.Sleep(10)
        End While

        If haltInstall Then
            MsgBox(My.Resources.Strings.installAborted, CType(MsgBoxStyle.OkOnly + MsgBoxStyle.Exclamation, MsgBoxStyle), My.Resources.Strings.installAborted)
            Return Nothing
        End If

        Return adb
    End Function

    Private Shared Function HandleInstallFailure(file As String, adb As Process) As MsgBoxResult
        Dim result As MsgBoxResult = MsgBox(My.Resources.Strings.unsuccessfulInstall1 & file & My.Resources.Strings.unsuccessfulInstall2 & vbCrLf & My.Resources.Strings.details & My.Resources.Strings.adbExitErrorCode & adb.ExitCode, CType(MsgBoxStyle.Exclamation + MsgBoxStyle.AbortRetryIgnore, MsgBoxStyle))
        Return result
    End Function

    ''' <summary>
    ''' Aborts the install
    ''' </summary>
    Sub Abort()
        stopRequested = True
    End Sub

    Sub Reset()
        stopRequested = False
    End Sub
End Class
