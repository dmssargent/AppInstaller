
Imports System.Diagnostics.CodeAnalysis
Imports System.IO
Imports System.Net.Http
Imports System.Net.Http.Headers
Imports System.Text
Imports System.Threading
Imports APKInstaller.My.Resources
Imports IWshRuntimeLibrary
Imports MaterialSkin.Controls
Imports Microsoft.Win32
Imports Squirrel
Imports Squirrel.UpdateManager

''' <summary>
''' Core Updating system of APKInstaller. This handles app installs, updates, and uninstalls
''' Usage:
''' Create a new instance, then call HandleEvents before the app has fully loaded. HandleEvents may result
''' in app termination due to a special event being triggered i.e. app install, so the GUI shouldn't be shown
''' 
''' Calling Update() will update the app, if possible (no-op if not possible), and if a new update has been released
''' </summary>
Public Class AppUpdateManager
    Private Delegate Sub SetUpdateCallback()
    Private Shared _updateManager As UpdateManager
    Private _updateLabel As MaterialLabel = Nothing
    Private Shared _instance As AppUpdateManager
    Private ReadOnly _gui As Form
    Private Shared _configured As Boolean = False
    'Private Shared _updateStatusText As String = "Everything is up-to-date"
    Private Shared hasCheckedForUpdate As Boolean = False
    Private Shared installerCode As Integer = -1
    'Private _updatesEnabled As Boolean

    ''' <summary>
    ''' The label that update notification should be applied to; must be a child of the GUI form used when creating the instance
    ''' </summary>
    ''' <returns>the current update notification label</returns>
    Public Property UpdateLabel As MaterialLabel
        Get
            Return _updateLabel
        End Get
        Set
            _updateLabel = Value
            If (_updateLabel IsNot Nothing) Then
                ' Add our click handler on the fly
                AddHandler UpdateLabel.Click, AddressOf UpdateClick
                UpdateLabel.Visible = False
            End If
        End Set
    End Property

    Public Shared ReadOnly Property UpdateStatusText As String = "Everything is up-to-date" '_updateStatusText

    ''' <summary>
    ''' Creates a new AppUpdateManager instance binded to a specific GUI
    ''' </summary>
    ''' <param name="gui">The main window form, and contains UpdateLabel, cannot be null</param>
    <SuppressMessage("Microsoft.Design", "CA1045:DoNotPassTypesByReference", MessageId:="0#")>
    Sub New(ByRef gui As Form)
        'MsgBox("foo")
        _instance = Me

        Dim updateManager = New Thread(New ThreadStart(AddressOf GetUpdateManager))
        updateManager.Name = "Update Manager"
        updateManager.Start()

        If gui Is Nothing Then
            Throw New ArgumentNullException(NameOf(gui))
        End If
        _gui = gui
    End Sub

    Private Shared Async Sub GetUpdateManager()
        Try
            'Const updatePath = "https://github.com/dmssargent/AppInstaller"

            If My.Settings.enableUpdates
                Dim githubMgr = Await GitHubUpdateManager("https://github.com/dmssargent/AppInstaller")
                'Dim githubMgr = New UpdateManager("https://github.com/dmssargent/AppInstaller/releases/download/v0.1.7.1-rc/")
                _updateManager = githubMgr
                installerCode = 0
            End If
        Catch e As Exception
            installerCode = 1
            Console.Write(e.ToString())
        End Try

        _configured = True
    End Sub

    Private Shared Async Function GitHubUpdateManager(repoUrl As String) As Task(Of UpdateManager)
        Dim repoUri = New Uri(repoUrl)
        Dim userAgent = New ProductInfoHeaderValue("Squirrel", "1.4.4.0")

        If (repoUri.Segments.Length <> 3) Then
            Throw New Exception("Repo URL must be to the root URL of the repo e.g. https://github.com/myuser/myrepo")
        End If

        Dim releasesApiBuilder = New StringBuilder("repos").Append(repoUri.AbsolutePath).Append("/releases")



        Dim baseAddress As Uri

        If (repoUri.Host.EndsWith("github.com", StringComparison.OrdinalIgnoreCase)) Then
            baseAddress = New Uri("https://api.github.com/")
        Else
            ' if it's not github.com, it's probably an Enterprise server
            ' now the problem with Enterprise is that the API doesn't come prefixed
            ' it comes suffixed
            ' so the API path of http://internal.github.server.local API location is
            ' http://interal.github.server.local/api/v3. 
            baseAddress = New Uri(String.Format("{0}{1}{2}/api/v3/", repoUri.Scheme, Uri.SchemeDelimiter, repoUri.Host))
        End If

        ' above ^^ notice the end slashes for the baseAddress, explained here: http : //stackoverflow.com/a/23438417/162694

        Using client = New HttpClient() With {.BaseAddress = baseAddress}
            client.DefaultRequestHeaders.UserAgent.Add(userAgent)
            Dim releasesApi = releasesApiBuilder.ToString()
            Dim response = Await client.GetAsync(releasesApi)
            response.EnsureSuccessStatusCode()

            Dim releases = Json.SimpleJson.DeserializeObject(Of List(Of Release))(Await response.Content.ReadAsStringAsync())
            Dim latestRelease = releases.First()


            Dim latestReleaseUrl = latestRelease.HtmlUrl.Replace("/tag/", "/download/")

            Return New UpdateManager(latestReleaseUrl, Nothing, Nothing, Nothing)
        End Using


    End Function


    Private Shared Sub GetBackupUpdateManager()
        installerCode = -2
        Try
            'Const updatePath = "https://github.com/dmssargent/AppInstaller"

            If My.Settings.enableUpdates
                'Dim githubMgr = Await UpdateManager.GitHubUpdateManager("https://github.com/dmssargent/AppInstaller", prerelease:=True)
                Dim githubMgr = New UpdateManager("https://github.com/dmssargent/AppInstaller/releases/download/v0.1.7.1-rc/")
                _updateManager = githubMgr
                installerCode = 0
            End If
        Catch e As Exception
            installerCode = 2
            Console.Write(e.ToString())
        End Try

        _configured = True
    End Sub

    ''' <summary>
    ''' Handles installer events, some of which may result in termination without the main form being loaded
    ''' </summary>
    Shared Sub HandleEvents()
        'MsgBox("Squirrel Init")
        If Not VerifyState() Then
            Return
        End If

        'MsgBox("Squirrel Handle")
        SquirrelAwareApp.HandleEvents(
                onInitialInstall:=Sub(v) SquirrelInstall(),
                onAppUpdate:=Sub(v)
                                 SquirrelUninstall()
                                 SquirrelInstall()
                             End Sub,
                onAppUninstall:=Sub(v) SquirrelUninstall()
            )

    End Sub

    Shared Function VerifyState(Optional noThrow As Boolean = True) As Boolean
        If Not My.Settings.enableUpdates
            _UpdateStatusText = "Updates are disabled"
        End If

        If (_updateManager Is Nothing) And My.Settings.enableUpdates Then
            Dim i = 0
            While Not (_updateManager Is Nothing) And My.Settings.enableUpdates
                Thread.Sleep(10)
                i += 1
                If installerCode = 1
                    If noThrow
                        _UpdateStatusText = "Updates are broken :("
                        Return False
                    End If
                    Throw New InvalidOperationException("The update components are broken!")
                ElseIf i > 100 And installerCode = -1
                    GetBackupUpdateManager()
                    Continue While ' for another loop to detect conditions
                    'If noThrow
                    '    _UpdateStatusText = "Updates are broken :("
                    '    Return False
                    'End If
                    'Throw New TimeoutException("Check for update component timed out during init")
                ElseIf i > 500 And installerCode = -2
                    If noThrow
                        _UpdateStatusText = "Updates are broken :("
                        Return False
                    End If
                    Throw New TimeoutException("Check for update component (1st and 2nd) timed out during init")
                ElseIf installerCode = 2
                    If noThrow
                        _UpdateStatusText = "Updates are really broken :("
                        Return False
                    End If
                    Throw New InvalidOperationException("The update components (2nd) are broken!")
                ElseIf installerCode = 0
                    _UpdateStatusText = "Everything is up-to-date"
                End If
            End While
        End If
        Return True
    End Function

    Private Shared Sub SquirrelInstall()
        'MsgBox("Squirrel Install")
        _updateManager.CreateShortcutForThisExe()
        InstallStage2()
    End Sub

    ''' <summary>
    ''' DO NOT CALL if Squirrel is working, verify this by calling VerifyState
    ''' </summary>
    Shared Sub NonSquirrelInstall()
        Dim shell = New WshShell()
        CreateExecutableShortcut(shell, Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory)))
        Dim startMenuPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Microsoft", "Windows", "Start Menu", "Programs", My.Application.Info.CompanyName)
        Directory.CreateDirectory(startMenuPath)
        CreateExecutableShortcut(shell, startMenuPath)

        InstallStage2()
    End Sub

    Private Shared Sub InstallStage2()
        ' Create a default icon
        Dim icon = Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), "APK.ico")
        Using file As New FileStream(icon, FileMode.Create)
            android_app.Save(file)
        End Using

        ' Register the APK extension to this app
        Const classKey = "AppInstaller"
        Const appExt = ".apk"

        Dim classes = Registry.CurrentUser.OpenSubKey("Software", True)

        classes.CreateSubKey("Classes")
        classes = classes.OpenSubKey("Classes", True)
        Dim appExtKey = classes.CreateSubKey(appExt)

        appExtKey.SetValue("", classKey) 'Set Default key value
        appExtKey.Close()


        classes.CreateSubKey(classKey)
        Dim appClassKey = classes.OpenSubKey(classKey, True)

        appClassKey.CreateSubKey("DefaultIcon")
        Dim appDefaultIcon = appClassKey.OpenSubKey("DefaultIcon", True)
        appDefaultIcon.SetValue("", """" & Application.ExecutablePath & ",1""") ' Set Default key value
        appDefaultIcon.Close()

        appClassKey.CreateSubKey("shell")
        Dim shellKey = appClassKey.OpenSubKey("shell", True)

        shellKey.CreateSubKey("open")
        shellKey = shellKey.OpenSubKey("open", True)

        shellKey.CreateSubKey("command")
        shellKey = shellKey.OpenSubKey("command", True)
        shellKey.SetValue("", """" & Application.ExecutablePath & """" & " ""%1""") ' Set Default key value
        shellKey.Close()

        appClassKey.Close()
    End Sub

    Private Shared Sub SquirrelUninstall()
        _updateManager.RemoveShortcutForThisExe()

        Dim icon = Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), "APK.ico")
        IO.File.Delete(icon)

        Const appClassKey = "AppInstaller"

        Dim classes = Registry.CurrentUser.OpenSubKey("Software", True)

        classes.CreateSubKey("Classes")
        classes = classes.OpenSubKey("Classes", True)

        ' According to MSDN, do not delete the file extension key, only delete the Class that the file extension is registered with
        classes.DeleteSubKey(appClassKey)
    End Sub

    Private Shared Sub CreateExecutableShortcut(shell As WshShell, shortcutPath As String)
        If Not Directory.Exists(shortcutPath)
            Throw New FileNotFoundException(shortcutPath)
        End If
        Dim shortcut As WshShortcut = CType(shell.CreateShortcut(Path.Combine(shortcutPath, "App Installer.lnk")), WshShortcut)
        shortcut.IconLocation = Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), "APK.ico")
        shortcut.TargetPath = Application.ExecutablePath
        shortcut.Description = My.Application.Info.Description
        shortcut.Arguments = ""
        shortcut.Save
    End Sub



    Private Async Sub StartUpdate()
        If My.Settings.enableUpdates = False Or IO.File.Exists(Application.StartupPath & "noupdate")
            Exit Sub
        End If

        If Not VerifyState() Then
            Exit Sub
        End If

        If Not hasCheckedForUpdate
            Try 
                Dim updateInfo = Await _updateManager.CheckForUpdate
                If (updateInfo.ReleasesToApply.Count > 0) Then
                    Await _updateManager.UpdateApp()
                    NotifyOfUpdate()
                End If
            Catch ex As Exception
                    Console.Write(ex.ToString)
            End Try
        End If

    End Sub

    ''' <summary>
    ''' Updates the current app asynchronously
    ''' </summary>
    Public Sub Update()
        Dim update = New Thread(New ThreadStart(AddressOf StartUpdate))
        update.Start()
    End Sub

    Public Shared Sub UpdateApp()
        _instance.Update()
    End Sub

    Private Sub NotifyOfUpdate()
        _UpdateStatusText = Strings.updateReady

        If (_updateLabel Is Nothing) Then
            Exit Sub
        End If

        If (UpdateLabel.InvokeRequired) Then
            UpdateLabel.Invoke(New SetUpdateCallback(AddressOf NotifyOfUpdate))
        Else
            UpdateLabel.Text = Strings.updateReady 'Strings.ResourceManager.GetString("updateReady")
            UpdateLabel.Font = New Font(UpdateLabel.Font, FontStyle.Underline)
            UpdateLabel.Visible = True
            If (Not _gui.InvokeRequired) Then
                _gui.Height += 20
            End If
        End If

    End Sub

    Private Sub UpdateClick(sender As Object, e As EventArgs)
        If (_updateManager IsNot Nothing) Then
            UpdateManager.RestartApp()
        End If
    End Sub

    Shared Sub CleanUp()
        If _updateManager IsNot Nothing Then
            _updateManager.Dispose()
        End If
    End Sub
End Class
