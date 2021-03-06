﻿Imports System.Diagnostics.CodeAnalysis
Imports System.Globalization
Imports System.IO
Imports System.Threading
Imports APKInstaller.My.Resources
Imports MaterialSkin.Controls

''' <summary>
''' Handler of installing Android packages
''' </summary>
Public Class Installer
    Private Delegate Sub DeviceIdCallback(ByVal deviceId As String)
    Private ReadOnly _gui As Main
    Private _lblStatus As MaterialLabel
    Private ReadOnly _txtUserInput As MaterialSingleLineTextField
    Private _stopRequested As Boolean = False
    Private _update As Boolean = True
    Private ReadOnly _filesToInstall As LinkedList(Of String) = New LinkedList(Of String)
    Private _filesToInstallDesc As String = ""

    ''' <summary>
    ''' Creates a new installer instance
    ''' </summary>
    ''' <param name="entry">The main GUI of the application</param>
    ''' <param name="statusLabel">The label to use as installer statuses</param>
    ''' <param name="userInputTextBox">The user input textbox</param>
    <SuppressMessage("Microsoft.Design", "CA1045:DoNotPassTypesByReference", MessageId:="2#")>
    <SuppressMessage("Microsoft.Design", "CA1045:DoNotPassTypesByReference", MessageId:="1#")>
    <SuppressMessage("Microsoft.Design", "CA1045:DoNotPassTypesByReference", MessageId:="0#")>
    Sub New(ByRef entry As Main, ByRef statusLabel As MaterialLabel, ByRef userInputTextBox As MaterialSingleLineTextField)
        _gui = entry
        _lblStatus = statusLabel
        _txtUserInput = userInputTextBox
    End Sub

    ''' <summary>
    ''' Determines if the operations performed are update (reinstall) operations, or not
    ''' </summary>
    ''' <returns>true if the packages are going to be reinstalled; otherwise false</returns>
    Property Reinstall As Boolean
        Get
            Return _update
        End Get
        Set
            _update = Value
        End Set
    End Property

    ''' <summary>
    ''' Whether to force the package installation or not
    ''' </summary>
    ''' <returns>true if the packages will be forcibly installed</returns>
    Public Property Force As Boolean = False

    ''' <summary>
    ''' Shows a completion message after the installation of all packages is finished
    ''' </summary>
    ''' <returns>if a completion message will be shown after the installation job is finished</returns>
    Public Property CompletionMessageWhenFinished As Boolean = True

    Public Property UseMultiFileDialog As Boolean = False

    Public ReadOnly Property FilesToInstallDescription As String
        Get
            Return _filesToInstallDesc
        End Get
    End Property

    Sub AddFilesToInstall(files() As String)
        AddFilesToInstall(files, False)
    End Sub

    Sub AddFilesToInstall(files() As String, clear As Boolean)
        AddFilesToInstall(files, clear, True)
    End Sub

    ''' <summary>
    ''' Adds files to be installed
    ''' </summary>
    ''' <param name="files">files to be installed</param>
    '<Log("App Installer Debug")>
    Sub AddFilesToInstall(files() As String, clear As Boolean, notifyUser As Boolean)
        If files Is Nothing Then
            Throw New ArgumentNullException(NameOf(files))
        End If

        If clear Then
            _filesToInstall.Clear()
        End If

        For Each path As String In files
            If path Is Nothing Then ' Prevent bugs in detection routine
                Continue For
            End If

            path = path.Trim()

            ' Determine if the file is already in the installer list, if so skip it
            If _filesToInstall.Contains(path) Then
                Continue For
            End If

            Try
                If ValidateFile(path, notifyUser) Then
                    _filesToInstall.AddLast(path)
                End If
            Catch ex As IOException
                MsgBox(ex.Message)
            End Try
        Next path

        _txtUserInput.Text = GenerateFileListSummary()
        _filesToInstallDesc = _txtUserInput.Text
    End Sub

    '<Log("App Installer Debug")>
    Public Sub RemoveFile(path As String)
        _filesToInstall.Remove(path)
    End Sub

    Public Shared Function ValidateFile(path As String) As Boolean
        Return ValidateFile(path, False)
    End Function
    ''' <summary>
    ''' Determines if the given file is a valid APK file
    ''' </summary>
    ''' <param name="path">the file location for the APK file</param>
    ''' <param name="notifyUser">true will notify the user of the problem via dialogs, false will not</param>
    ''' <returns>true if the file is a valid APK, false otherwise</returns>
    '<Log("App Installer Debug")>
    Public Shared Function ValidateFile(path As String, notifyUser As Boolean) As Boolean
         If path Is Nothing Then
            Throw New ArgumentNullException(NameOf(path))
        End If

        ' Check to see if the file exists
        If Not File.Exists(path) Then
            If notifyUser Then
                MsgBox("""" & path & """ " & If(Directory.Exists(path),
                   Strings.isDirError,
                   Strings.fileDoesNotExist))
            End If
            Return False
        End If

        Try
            ' Check for the correct extension and that the file can be correctly parsed as an APK
            If path.ToUpper(CultureInfo.CurrentCulture).EndsWith(".APK", StringComparison.CurrentCultureIgnoreCase) And
            AndroidTools.PackageName(path.trim()) Is "" Then
                If notifyUser Then
                    MsgBox("""" & path & """" & Strings.invalidApk,
                       CType(MsgBoxStyle.OkOnly + MsgBoxStyle.Exclamation, MsgBoxStyle), "Invalid File")
                End If
                Return False
            Else
                Return True
            End If
        Catch ex As IOException
            If notifyUser Then
                MsgBox("""" & path & """" & Strings.invalidApk,
                       CType(MsgBoxStyle.OkOnly + MsgBoxStyle.Exclamation, MsgBoxStyle), "Invalid File")
            End If
            Return False
        End Try
    End Function

    Private Function GenerateFileListSummary() As String
        If _filesToInstall.Count = 0 Then
            UseMultiFileDialog = False
            Return ""
        ElseIf _filesToInstall.Count = 1 Then
            UseMultiFileDialog = False
            Return _filesToInstall.First.Value
        Else
            UseMultiFileDialog = True
            Return _filesToInstall.ElementAt(0) & ", " & _filesToInstall.ElementAt(1) & If(_filesToInstall.Count > 2, "...", "")
        End If
    End Function

    ''' <summary>
    ''' Verifies that all of the files to be installed are valid
    ''' </summary>
    ''' <returns></returns>
    Function VerifyFilesToInstall(Optional excuseNoFiles As Boolean = False) As Boolean
        Dim apkFiles = GetFilesToInstall()

        ' Nothing to install
        If apkFiles.Length = 0 Then
            Return excuseNoFiles
        End If

        Return apkFiles.All(Function(apkfile) ValidateFile(apkfile))
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
        If Not waitForDeviceReturn = ErrorCode.Success Then
            HandleWaitForDeviceError(waitForDeviceReturn)
            Return
        End If


        ' Device Found, check if multiple devices are connected, and figure out the device to install to
        Dim deviceId = AcquireDeviceId()
        If deviceId Is Nothing Then
            _gui.ResetGui(Strings.installAborted)
            Return
        End If

        ' Begin device install sections
        _gui.SetText(_lblStatus, Strings.startingInstalls)
        Dim filesToInstall As String() = GetFilesToInstall()
        Dim installStatus = ""
        Dim installAborted = False
        Dim installHasFailed = False
        _gui.ShowProgressAnimation(True, True, CInt(100 / filesToInstall.Length))
        Const maxRetryCount = 3
        For Each file As String In filesToInstall
            If installAborted Then ' Check if the install has been aborted, if so stop the installs
                'Exit For
            End If

            ' Retry Loop
            Dim packageInstallSuccess = False
            For retry = 0 To maxRetryCount
                'Display a status
                _gui.SetText(_gui.lblStatus, If(retry = 0, Strings.installing, Strings.retrying_install) & " """ & file & Strings.onto_the_device & deviceId & """")
                Dim installAttempt = PackageInstallAttempt(deviceId, file)
                Select Case installAttempt
                    Case ErrorCode.Abort
                        ' ReSharper disable once RedundantAssignment
                        installAborted = True
                        Return
                    Case ErrorCode.Failure1, ErrorCode.Failure2
                        Continue For
                    Case ErrorCode.Success, ErrorCode.Ignore
                        packageInstallSuccess = True
                        Exit For
                End Select
            Next retry ' End retry loop
            If Not packageInstallSuccess Then
                MsgBox("The file """ & file & """ couldn't be installed.", CType(MsgBoxStyle.Exclamation + MsgBoxStyle.OkOnly, MsgBoxStyle))
                installHasFailed = True
            End If
        Next file ' for each file
        _gui.UseWaitCursor = False
        If installAborted Then
            If CompletionMessageWhenFinished Then
                _gui.ResetGui(Strings.failure & Strings.details & vbCrLf & installStatus)
            Else
                MsgBox(Strings.failure & Strings.details & vbCrLf & installStatus, CType((MsgBoxStyle.Exclamation + MsgBoxStyle.OkOnly), MsgBoxStyle))
                _gui.Close()
            End If
        Else
            If CompletionMessageWhenFinished Then
                Dim message = Strings.installationOf & If(filesToInstall.Length = 1, Strings.apk, Strings.apks)
                If installHasFailed Then
                    MsgBox(message & Strings.hasFinishedError)
                Else
                    MsgBox(message & Strings.hasFinishedSucess, CType(MsgBoxStyle.Information + MsgBoxStyle.OkOnly, MsgBoxStyle), Strings.apkInstallFinished)
                End If
                _gui.ResetGui(Strings.done)
            Else
                _gui.SetText(_gui.lblStatus, Strings.done)
            End If
        End If
        'FinishInstall(If(installAborted, Strings.failure & Strings.details & vbCrLf & installStatus, Strings.done))
        ' End device install section
    End Sub

    Private Function PackageInstallAttempt(deviceId As String, file As String) As ErrorCode
        If Force Then
            ForceRemovePackage(file)
        End If

        Dim adbCode = InstallSinglePackage(deviceId, file)
        If adbCode Is Nothing Then ' Install Single Package failed catastrophically when this happened
            Return ErrorCode.Failure1
        End If

        Dim result = -1
        If Integer.TryParse(adbCode, result) Then ' Checks if the result is a return code
            If result = 0 Then
                _gui.StepProgressBar()
                Return ErrorCode.Success
            Else
                Return HandleInstallFailure(file, result)
            End If
        Else ' the result is an details error message
            Return HandleInstallFailure(file, adbCode)
        End If
    End Function

    Private Sub HandleWaitForDeviceError(waitForDeviceReturn As Integer)
        Dim message = "An unknown error while waiting for the device"
        If waitForDeviceReturn = ErrorCode.FailureTimeout Then
            message = Strings.timeoutWaiting & vbCrLf &
                                 Strings.userTroubleshootingA1 & vbCrLf &
                                 Strings.userTroubleshootingA2
        ElseIf waitForDeviceReturn = ErrorCode.Abort Then
            message = Strings.installAborted
        End If

        If CompletionMessageWhenFinished Then
            _gui.ResetGui(message)
        Else
            MsgBox(message, CType((MsgBoxStyle.Exclamation + MsgBoxStyle.OkOnly), MsgBoxStyle))
            _gui.Close()
        End If
    End Sub

    Private Function WaitForDevice() As Integer
        Dim counter = 0
        Using adbWait = AndroidTools.RunAdb("wait-for-device", False, True, False)
            Dim caret As Integer = 0
            _gui.UseWaitCursor = True
            _gui.ShowProgressAnimation(True, False)
            While Not adbWait.HasExited
                Dim currentMessage As String
                If caret = 0 Then
                    currentMessage = Strings.waitForDevice1
                ElseIf caret = 1 Then
                    currentMessage = Strings.waitForDevice2
                Else
                    currentMessage = Strings.waitForDevice3
                    caret = -1
                End If
                caret += 1
                If counter > 10 Then
                    currentMessage &= vbCrLf & Strings.userTroubleshootingA1 & vbCrLf &
                                                Strings.userTroubleshootingA2
                End If
                _gui.SetText(_lblStatus, currentMessage)

                Thread.Sleep(1000)
                Thread.Yield()
                If _stopRequested Then
                    MsgBox(Strings.installAborted, CType(MsgBoxStyle.OkOnly + MsgBoxStyle.Exclamation, MsgBoxStyle), "APK Install Aborted")
                    Return ErrorCode.Abort
                End If
                counter += 1
                If counter > 60 Then
                    Return ErrorCode.FailureTimeout
                End If
            End While
            _gui.ShowProgressAnimation(False, False)
        End Using
        Return 0
    End Function

    Private Function AcquireDeviceId() As String
        _gui.SetText(_lblStatus, Strings.checkDevices)
        _gui.ShowProgressAnimation(True, False)

        Dim deviceId As String = Nothing
        GetDeviceId(Sub(id As String)
                        ' ReSharper disable once RedundantAssignment
                        deviceId = id
                    End Sub)
        While deviceId Is Nothing
            If (_stopRequested) Then
                _gui.SetText(_lblStatus, Strings.installAborted)
                _gui.ShowProgressAnimation(False, False)
                Return Nothing

            End If
            Thread.Sleep(50)
        End While

        _gui.ShowProgressAnimation(False, False)

        Return deviceId
    End Function

    Private Sub GetDeviceId(callback As DeviceIdCallback)
        Dim deviceChooser As New DeviceChooserDialog()
        Dim result = deviceChooser.GetUserInput()
        If result = DialogResult.OK Or result = Nothing Then
            If (MsgBox(Strings.correctDevice1 + deviceChooser.Device + Strings.correctDevice2, CType(MsgBoxStyle.YesNo + MsgBoxStyle.Question, MsgBoxStyle)) = MsgBoxResult.Yes) Then
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
        Return _filesToInstall.ToArray
    End Function

    Shared Function ForceRemovePackage(ByVal apkFile As String) As Boolean
        Using adb = AndroidTools.RunAdb("uninstall " & AndroidTools.PackageName(apkFile.trim()), False, True, True)
            Return adb.ExitCode = 0
        End Using
    End Function

    Private Function InstallSinglePackage(deviceId As String, file As String) As String
        Const abortKey = "ABORT"

        Using adb = AndroidTools.RunAdb("-s " & deviceId & " install " & If(_update, "-r ", "") & """" & file & """", True, True, False)
            Dim adbStandardOut = adb.StandardOutput
            Dim haltInstall = False

            ' Process aborts while the adb process is running
            ' Check for and identify failures during the adb install
            Dim currentLine = adbStandardOut.ReadLine
            While Not adb.HasExited Or currentLine IsNot Nothing
                If _stopRequested Then ' user requested stop
                    If MsgBox(Strings.abortCurrentApk, CType(MsgBoxStyle.YesNo + MsgBoxStyle.Question, MsgBoxStyle)) = MsgBoxResult.Yes Then
                        Try
                            If adb.HasExited Then
                                adb.Kill()
                            End If
                        Catch ex As InvalidOperationException

                        End Try
                        MsgBox(Strings.installAborted, CType(MsgBoxStyle.OkOnly + MsgBoxStyle.Exclamation, MsgBoxStyle), "APK Install Aborted")
                        Return abortKey
                    Else
                        haltInstall = True
                        _stopRequested = False
                    End If
                End If


                While currentLine IsNot Nothing AndAlso Not _stopRequested
                    If currentLine.StartsWith("Failure", StringComparison.OrdinalIgnoreCase) Then
                        Dim startPos = currentLine.IndexOf("[", StringComparison.OrdinalIgnoreCase)
                        Dim endPos = currentLine.IndexOf("]", StringComparison.OrdinalIgnoreCase)
                        Dim errorMessage = "Unknown Failure"
                        If startPos < endPos And startPos <> -1 And endPos <> -1 Then
                            errorMessage = currentLine.Substring(startPos + 1, endPos - startPos - 1)
                        End If
                        Return errorMessage
                    End If
                    currentLine = adbStandardOut.ReadLine()
                End While
            End While

            ' Display an abort message
            If haltInstall Then
                MsgBox(Strings.installAborted, CType(MsgBoxStyle.OkOnly + MsgBoxStyle.Exclamation, MsgBoxStyle), Strings.installAborted)
                Return abortKey
            End If

            Return adb.ExitCode.ToString()
        End Using
    End Function

    Private Function HandleInstallFailure(file As String, code As Integer) As ErrorCode
        Return HandleInstallFailure(file, Strings.adbExitErrorCode & code)
    End Function

    Private Function HandleInstallFailure(file As String, message As String) As ErrorCode
        Dim result As MsgBoxResult = MsgBox(Strings.unsuccessfulInstall1 & file & Strings.unsuccessfulInstall2 & vbCrLf & Strings.details & message, CType(MsgBoxStyle.Exclamation + MsgBoxStyle.AbortRetryIgnore, MsgBoxStyle))
        Select Case result
            Case MsgBoxResult.Abort
                Return ErrorCode.Abort
            Case MsgBoxResult.Ignore
                Return ErrorCode.Ignore ' Don't retry
            Case Else
                For second As Integer = 5 To 1 Step -1
                    Dim retryMessage = Strings.retryIn & second & If(second = 1, Strings.second, Strings.seconds)
                    _gui.SetText(_gui.lblStatus, retryMessage)
                Next second
                Return ErrorCode.Failure2
        End Select
    End Function

    ''' <summary>
    ''' Aborts the install
    ''' </summary>
    Sub Abort()
        _stopRequested = True
    End Sub

    Sub Reset()
        _stopRequested = False
    End Sub
End Class
