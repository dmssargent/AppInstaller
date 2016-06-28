﻿Imports System.IO
Imports System.Threading

''' <summary>
''' Provided utilities for operating on Android devices, such as adb and aapt
''' </summary>
Public NotInheritable Class AndroidTools
    Private Shared aaptCache As String
    Private Shared adbCache As String

    Private Sub New()
        SetupIfPossible()
    End Sub

    ''' <summary>
    ''' Returns the location of adb to be executed, if adb is in the system path the function may return the location of adb as
    ''' "adb" This automatically setups up adb if adb can't be found
    ''' </summary>
    ''' <returns>the location of adb or "adb"</returns>
    Shared ReadOnly Property Adb As String
        Get
            If adbCache IsNot Nothing And (adbCache Is "adb" Or File.Exists(adbCache)) Then
                Return adbCache
            End If

            Try
                Return SetupAdb()
            Catch ex As Exception
                Throw New InvalidOperationException("Failed to configure ADB on-demand", ex)
            End Try
        End Get
    End Property

    ''' <summary>
    ''' Returns the location of aapt; this function may setup aapt if necessary
    ''' </summary>
    ''' <returns>location of aapt</returns>
    <CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId:="Aapt")>
    Shared ReadOnly Property Aapt As String
        Get
            If (aaptCache IsNot Nothing) Then
                If (File.Exists(aaptCache)) Then
                    Return aaptCache
                End If
            End If

            Try
                Return SetupAapt()
            Catch ex As Exception
                Throw New InvalidOperationException("Failed to configure AAPT on-demand", ex)
            End Try
        End Get
    End Property

    ''' <summary>
    ''' Sets up both ADB and AAPT for later use
    ''' </summary>
    Public Shared Sub SetupIfPossible()
        If Not File.Exists(aaptCache) Then
            aaptCache = Nothing
        End If

        If adbCache IsNot "adb" And Not File.Exists(adbCache) Then
            adbCache = Nothing
        End If

        If (adbCache Is Nothing) Then
            SetupAdb()
        End If
        If aaptCache Is Nothing Then
            SetupAapt()
        End If

    End Sub

    Private Shared Function SetupAdb() As String
        Dim windir = Environment.GetEnvironmentVariable("windir")
        If Not (File.Exists(Path.Combine(windir, "adb.exe")) Or File.Exists(Path.Combine(windir, "system32", "adb.exe"))) Then
            Using adb As New Process
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
            End Using
        End If

        Dim androidHome = Environment.GetEnvironmentVariable("ANDROID_HOME")
        If androidHome Is Nothing Then
            androidHome = MostLikelyAndroidSdk(Environment.GetEnvironmentVariable("PATH"))
            If androidHome IsNot Nothing Then
                If MsgBox("The Android SDK is not completely correctly configured at this moment." & vbCrLf &
                    "Do you want to correct those configuration issues?" & vbCrLf &
                    "Details: ANDROID_HOME is not defined, when valid Android SDK is present", CType(MsgBoxStyle.YesNo + MsgBoxStyle.Exclamation, MsgBoxStyle)) = MsgBoxResult.Yes Then
                    Environment.SetEnvironmentVariable("ANDROID_HOME", androidHome) ' Change the environment variable for this process as well as the user
                    Environment.SetEnvironmentVariable("ANDROID_HOME", androidHome, EnvironmentVariableTarget.User)
                End If
            End If
        ElseIf Not IsAndroidSdk(androidHome) Then
            Dim androidHome2 = MostLikelyAndroidSdk(Environment.GetEnvironmentVariable("PATH"))
            If androidHome2 IsNot Nothing Then
                If Not androidHome = androidHome2 Then
                    If MsgBox("The Android SDK is not completely correctly configured at this moment." & vbCrLf &
                           "Do you want to correct those configuration issues?" & vbCrLf &
                            "Details: Invalid SDK defined, when valid SDK can be found", CType(MsgBoxStyle.YesNo + MsgBoxStyle.Exclamation, Global.Microsoft.VisualBasic.MsgBoxStyle)) = MsgBoxResult.Yes Then
                        Environment.SetEnvironmentVariable("ANDROID_HOME", androidHome2) ' Change the environment variable for this process as well as the user
                        Environment.SetEnvironmentVariable("ANDROID_HOME", androidHome2, EnvironmentVariableTarget.User)
                    End If
                End If
            End If
        End If

        If androidHome IsNot Nothing Then
            Dim adbLocation = Path.Combine(androidHome, "platform-tools", "adb")
            Using adb As New Process

                adb.StartInfo.Arguments = "version"
                adb.StartInfo.FileName = adbLocation
                adb.StartInfo.CreateNoWindow = True
                adb.StartInfo.UseShellExecute = False
                Try
                    adb.Start()
                    adb.WaitForExit()
                    If (adb.ExitCode = 0) Then
                        adbCache = adbLocation
                        Return adbLocation
                    End If
                Catch ex As Exception
                    ' Continue on
                End Try
            End Using
        End If

        ' Fall-back to app version
        Dim tempFileName = IOUtilities.CreateTempSession("adb")
        Dim platformToolsZip As String = tempFileName + "\platform-tools.zip"
        File.WriteAllBytes(platformToolsZip, My.Resources.platform_tools_r23_1_0_windows)
        Dim androidPlatformTools As String = tempFileName + "\platform-tools"
        Using stream As New FileStream(platformToolsZip, FileMode.Open)
            IOUtilities.UnzipFromStream(stream, androidPlatformTools)
        End Using
        adbCache = androidPlatformTools & "\platform-tools\adb.exe"
        Return adbCache
    End Function

    <CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA2204:Literals should be spelled correctly", MessageId:="aapt")>
    Private Shared Function SetupAapt() As String
        Dim aapt = ""

        Try
            Dim temp = IOUtilities.CreateTempSession("aapt")
            aapt = Path.Combine(temp, "aapt.exe")
        Catch ex As Exception

        End Try


        File.WriteAllBytes(aapt, My.Resources.aapt_23_0_3_win)
        If File.Exists(aapt) Then
            aaptCache = aapt
            Return aapt
        Else
            Throw New IOException("Failed to build aapt.exe")
        End If
    End Function


    ''' <summary>
    ''' Gets an instance of an "adb" process to customize and use. Callers should use this in a using block or dispose of the generate
    ''' process
    ''' 
    ''' Example:
    ''' Using adb = RunAapt("version", True, True, True)
    '''     ' Your code here
    ''' End Using
    ''' </summary>
    ''' <param name="args">adb arguments</param>
    ''' <param name="redirectStdOut">whether or not to redirect the standard output stream</param>
    ''' <param name="run">run this program before returning</param>
    ''' <param name="waitToReturn">wait for this program to return, run parameter must be true</param>
    ''' <returns>an adb process</returns>
    Public Shared Function RunAdb(args As String, redirectStdOut As Boolean, run As Boolean, waitToReturn As Boolean) As Process
        SetupIfPossible()
        Dim pAdb As New Process
        pAdb.StartInfo.FileName = Adb
        pAdb.StartInfo.Arguments = args
        pAdb.StartInfo.CreateNoWindow = True
        pAdb.StartInfo.UseShellExecute = False
        pAdb.StartInfo.RedirectStandardOutput = redirectStdOut

        If run Then
            pAdb.Start()
            If waitToReturn Then
                pAdb.WaitForExit()
            End If
        End If

        Return pAdb
    End Function

    ''' <summary>
    ''' Gets an instance of an "aapt" process to customize and use. Callers should use this in a using block or dispose of the generate
    ''' process
    ''' 
    ''' Example:
    ''' Using aapt = RunAapt("-help", True)
    '''     aapt.Start()
    '''     aapt.WaitForExit()
    ''' End Using
    ''' </summary>
    ''' <param name="args">aapt arguments</param>
    ''' <param name="redirectStdOut">true if the standard output stream should be redirected, otherwise false</param>
    ''' <returns>an instance of an aapt process, not started</returns>
    Friend Shared Function RunAapt(args As String, redirectStdOut As Boolean) As Process
        SetupIfPossible()
        Dim pAapt As New Process()
        pAapt.StartInfo.FileName = Aapt
        pAapt.StartInfo.Arguments = args
        pAapt.StartInfo.CreateNoWindow = True
        pAapt.StartInfo.UseShellExecute = False
        pAapt.StartInfo.RedirectStandardOutput = redirectStdOut

        Return pAapt
    End Function

    ''' <summary>
    ''' Returns the package name of a given APK file
    ''' </summary>
    ''' <param name="apkFile">the location of the APK file</param>
    ''' <returns>The package name of the APK file, or nothing on failure</returns>
    Shared Function PackageName(ByVal apkFile As String) As String
        If apkFile Is Nothing Then
            Throw New ArgumentNullException(NameOf(apkFile))
        End If

        Dim aapt = RunAapt("dump badging """ & apkFile & """", True)
        aapt.Start()

        Const parseFor = "package: name="
        Dim package As String = Nothing
        Try
            Dim line As String = aapt.StandardOutput.ReadLine.Trim
            While line IsNot Nothing
                'Detect interrupts
                Thread.Sleep(1)

                If line.Contains(parseFor) Then
                    Dim versionName As String = line.Substring(line.IndexOf(parseFor) + parseFor.Length)
                    package = versionName.Substring(versionName.IndexOf("'") + 1)
                    package = package.Substring(0, package.IndexOf("'"))
                    Exit While
                End If
                line = aapt.StandardOutput.ReadLine.Trim
            End While

            If aapt.HasExited Then
                If Not aapt.ExitCode = 0 Then
                    Throw New IOException("AAPT Failed. Exit: " & aapt.ExitCode)
                End If
            End If
        Catch ex As Exception
            Throw New IOException("Failed to acquire package name", ex)
        End Try
        Return package
    End Function

    Shared Function IsAndroidSdk(path As String) As Boolean
        Dim platformTools = IO.Path.Combine(path, "platform-tools")
        Dim tools = IO.Path.Combine(path, "tools")

        If Directory.Exists(tools) And Directory.Exists(platformTools) Then
            Dim adb = IO.Path.Combine(platformTools, "adb.exe")
            Dim androidBat = IO.Path.Combine(tools, "android.bat")
            If File.Exists(adb) And File.Exists(androidBat) Then
                Return True
            Else
                Return False
            End If
        Else
            Return False
        End If
    End Function

    ''' <summary>
    ''' Returns the most likely Android SDK out of a set of locations, or a single location. A set of locations is delimited by the IO.Path.PathSeperator
    ''' character. The parents of the given path(s) are also checked.
    ''' </summary>
    ''' <param name="path">a delimited list of paths, or a single path location</param>
    ''' <returns>Nothing if none of the paths result in an usable Android SDK, or the path of an Android SDK</returns>
    Shared Function MostLikelyAndroidSdk(path As String) As String
        If path Is Nothing Then
            Throw New ArgumentNullException(NameOf(path))
        End If
        Return MostLikelyAndroidSdk(path.Split(IO.Path.PathSeparator.ToString.ToArray))
    End Function

    ''' <summary>
    ''' Returns the most likely Android SDK out of a set of locations, this function checks both the given location and the parents of the given location
    ''' for the Android SDK location
    ''' </summary>
    ''' <param name="paths">the locations in a string format</param>
    ''' <returns>Nothing if none of the paths result in an usable Android SDK, or the path of an Android SDK</returns>
    Shared Function MostLikelyAndroidSdk(paths As String()) As String
        If paths Is Nothing Then
            Throw New ArgumentNullException(NameOf(paths))
        End If

        Dim possibleSdkPaths As New List(Of String)
        Dim androidSdkPaths As String() = {"add-ons", "build-tools", "docs", "extras", "licenses", "ndk-bundle", "platforms", "platform-tools", "samples", "skins", "sources", "system-images", "temp", "tools"}
        For Each path In paths
            If path IsNot "" And IsAndroidSdk(path) Then
                possibleSdkPaths.Add(path)
            Else
                For Each sdkPath In androidSdkPaths
                    If path.Contains(sdkPath) Then
                        Dim path2 = path.Substring(0, path.IndexOf(IO.Path.DirectorySeparatorChar & sdkPath))
                        If IsAndroidSdk(path2) Then
                            possibleSdkPaths.Add(path2)
                        End If
                        Exit For
                    Else
                        Continue For
                    End If
                Next
            End If
        Next

        Dim highScore = 0
        Dim highScoreLocation As String = Nothing

        For Each path In possibleSdkPaths
            Dim currentScore = 0
            For Each sdkPath In androidSdkPaths
                If Directory.Exists(IO.Path.Combine(path, sdkPath)) Then
                    currentScore += 1
                End If
            Next sdkPath

            If currentScore > highScore Then
                highScore = currentScore
                highScoreLocation = path
            End If
        Next path

        Return highScoreLocation
    End Function
End Class
