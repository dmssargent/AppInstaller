
Imports System.Diagnostics.CodeAnalysis
Imports System.IO
Imports System.Threading
Imports APKInstaller.My.Resources
Imports MaterialSkin.Controls
Imports Microsoft.Win32
Imports Squirrel

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
    Private ReadOnly _gui As Form
    'Private _updatesEnabled As Boolean

    ''' <summary>
    ''' The label that update notification should be applied to; must be a child of the GUI form used when creating the instance
    ''' </summary>
    ''' <returns>the current update notification label</returns>
    Public Property UpdateLabel As MaterialLabel
        Get
            Return _updateLabel
        End Get
        Set(value As MaterialLabel)

            _updateLabel = value
            If (_updateLabel IsNot Nothing) Then
                ' Add our click handler on the fly
                AddHandler UpdateLabel.Click, AddressOf UpdateClick
                UpdateLabel.Visible = False
            End If
        End Set
    End Property

    ''' <summary>
    ''' Creates a new AppUpdateManager instance binded to a specific GUI
    ''' </summary>
    ''' <param name="gui">The main window form, and contains UpdateLabel, cannot be null</param>
    <SuppressMessage("Microsoft.Design", "CA1045:DoNotPassTypesByReference", MessageId:="0#")>
    Sub New(ByRef gui As Form)
        Try
            Const updatePath = "https://github.com/dmssargent/AppInstaller"
            'updatesEnabled = IO.Directory.Exists(updatePath)
            Dim githubMgr = UpdateManager.GitHubUpdateManager(updatePath)
            githubMgr.Start()
            _updateManager = githubMgr.Result
        Catch e As Exception

        End Try

        If gui Is Nothing Then
            Throw New ArgumentNullException(NameOf(gui))
        End If
        _gui = gui
    End Sub

    ''' <summary>
    ''' Handles installer events, some of which may result in termination without the main form being loaded
    ''' </summary>
    Shared Sub HandleEvents()
        If (_updateManager Is Nothing) Then
            Return
        End If

        SquirrelAwareApp.HandleEvents(
                onInitialInstall:=Sub(v) SquirrelInstall(),
                onAppUpdate:=Sub(v) _updateManager.CreateShortcutForThisExe(),
                onAppUninstall:=Sub(v) SquirrelUninstall()
            )

    End Sub

    Private Shared Sub SquirrelInstall()
        _updateManager.CreateShortcutForThisExe()

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
        appClassKey = appClassKey.OpenSubKey("DefaultIcon", True)
        appClassKey.SetValue("", """" & Application.ExecutablePath & ",1""") ' Set Default key value

        appClassKey.Close()

        appClassKey.CreateSubKey("shell")
        Dim shellKey = appClassKey.OpenSubKey("shell", True)

        shellKey.CreateSubKey("open")
        shellKey = shellKey.OpenSubKey("open", True)

        shellKey.CreateSubKey("command")
        shellKey = shellKey.OpenSubKey("command", True)
        shellKey.SetValue("", """" & Application.ExecutablePath & """" & " ""%1""") ' Set Default key value

        shellKey.Close()
    End Sub

    Private Shared Sub SquirrelUninstall()
        _updateManager.RemoveShortcutForThisExe()

        Dim icon = Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), "APK.ico")
        File.Delete(icon)

        Const appClassKey = "AppInstaller"

        Dim classes = Registry.CurrentUser.OpenSubKey("Software", True)

        classes.CreateSubKey("Classes")
        classes = classes.OpenSubKey("Classes", True)

        ' According to MSDN, do not delete the file extension key, only delete the Class that the file extension is registered with
        classes.DeleteSubKey(appClassKey)
    End Sub



    Private Async Sub StartUpdate()
        If _updateManager Is Nothing Then
            Exit Sub
        End If

        Dim updateInfo = Await _updateManager.CheckForUpdate
        If (updateInfo.FutureReleaseEntry().Version.CompareTo(updateInfo.CurrentlyInstalledVersion.Version) > 0) Then
            Await _updateManager.UpdateApp()

            If (_updateLabel IsNot Nothing) Then
                NotifyOfUpdate()
            End If
        End If
    End Sub

    ''' <summary>
    ''' Updates the current app asynchronously
    ''' </summary>
    Public Sub Update()
        Dim update = New Thread(New ThreadStart(AddressOf StartUpdate))
        update.Start()
    End Sub

    Private Sub NotifyOfUpdate()
        If (_updateLabel Is Nothing) Then
            Exit Sub
        End If

        If (UpdateLabel.InvokeRequired) Then
            UpdateLabel.Invoke(New SetUpdateCallback(AddressOf NotifyOfUpdate))
        Else
            UpdateLabel.Text = Strings.ResourceManager.GetString("updateReady")
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
End Class
