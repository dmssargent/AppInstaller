
Imports System.Resources
Imports MaterialSkin.Controls
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
    Private Shared updateManager As UpdateManager
    Private _updateLabel As MaterialLabel = Nothing
    Private GUI As Form
    Private updatesEnabled As Boolean

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
    <CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1045:DoNotPassTypesByReference", MessageId:="0#")>
    Sub New(ByRef gui As Form)
        Try
            Const updatePath = "C:\SquirrelTest\AppInstaller"
            updatesEnabled = IO.Directory.Exists(updatePath)
            updateManager = New UpdateManager(updatePath)

        Catch e As Exception

        End Try

        If gui Is Nothing Then
            Throw New ArgumentNullException(NameOf(gui))
        End If
        Me.GUI = gui
    End Sub

    ''' <summary>
    ''' Handles installer events, some of which may result in termination without the main form being loaded
    ''' </summary>
    Shared Sub HandleEvents()
        If (updateManager Is Nothing) Then
            Return
        End If

        SquirrelAwareApp.HandleEvents(
                onInitialInstall:=Sub(v) SquirrelInstall(),
                onAppUpdate:=Sub(v) updateManager.CreateShortcutForThisExe(),
                onAppUninstall:=Sub(v) SquirrelUninstall()
            )

    End Sub

    Private Shared Sub SquirrelInstall()
        updateManager.CreateShortcutForThisExe()

        ' Create a default icon
        Dim icon = IO.Path.Combine(IO.Path.GetDirectoryName(Application.ExecutablePath), "APK.ico")
        Using file As New IO.FileStream(icon, IO.FileMode.Create)
            My.Resources.android_app.Save(file)
        End Using

        ' Register the APK extension to this app
        Const APP_CLASS_KEY As String = "AppInstaller"
        Const APP_EXT As String = ".apk"

        Dim classes = Microsoft.Win32.Registry.CurrentUser.OpenSubKey("Software", True)

        classes.CreateSubKey("Classes")
        classes = classes.OpenSubKey("Classes", True)
        Dim appExtKey = classes.CreateSubKey(APP_EXT)

        appExtKey.SetValue("", APP_CLASS_KEY) 'Set Default key value
        appExtKey.Close()


        classes.CreateSubKey(APP_CLASS_KEY)
        Dim appClassKey = classes.OpenSubKey(APP_CLASS_KEY, True)

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
        updateManager.RemoveShortcutForThisExe()

        Dim icon = IO.Path.Combine(IO.Path.GetDirectoryName(Application.ExecutablePath), "APK.ico")
        IO.File.Delete(icon)

        Const APP_CLASS_KEY As String = "AppInstaller"

        Dim classes = Microsoft.Win32.Registry.CurrentUser.OpenSubKey("Software", True)

        classes.CreateSubKey("Classes")
        classes = classes.OpenSubKey("Classes", True)

        ' According to MSDN, do not delete the file extension key, only delete the Class that the file extension is registered with
        classes.DeleteSubKey(APP_CLASS_KEY)
    End Sub



    Private Async Sub StartUpdate()
        If updateManager Is Nothing Or Not updatesEnabled Then
            Exit Sub
        End If

        Dim updateInfo = Await updateManager.CheckForUpdate
        If (updateInfo.FutureReleaseEntry().Version.CompareTo(updateInfo.CurrentlyInstalledVersion.Version) > 0) Then
            Await updateManager.UpdateApp()

            If (_updateLabel IsNot Nothing) Then
                NotifyOfUpdate()
            End If
        End If
    End Sub

    ''' <summary>
    ''' Updates the current app asynchronously
    ''' </summary>
    Public Sub Update()
        Dim update = New Threading.Thread(New Threading.ThreadStart(AddressOf StartUpdate))
        update.Start()
    End Sub

    Private Sub NotifyOfUpdate()
        If (_updateLabel Is Nothing) Then
            Exit Sub
        End If

        If (UpdateLabel.InvokeRequired) Then
            UpdateLabel.Invoke(New SetUpdateCallback(AddressOf NotifyOfUpdate))
        Else
            UpdateLabel.Text = My.Resources.Strings.ResourceManager.GetString("updateReady")
            UpdateLabel.Font = New Font(UpdateLabel.Font, FontStyle.Underline)
            UpdateLabel.Visible = True
            If (Not GUI.InvokeRequired) Then
                GUI.Height += 20
            End If
        End If

    End Sub

    Private Sub UpdateClick(sender As Object, e As EventArgs)
        If (updateManager IsNot Nothing) Then
            UpdateManager.RestartApp()
        End If
    End Sub
End Class
