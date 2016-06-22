
Imports MaterialSkin.Controls
Imports Squirrel

Public Class AppUpdateManager
    Private Delegate Sub SetUpdateCallback()
    Private Shared updateManager As UpdateManager
    Dim _updateLabel As MaterialLabel = Nothing
    Dim GUI As Form

    Sub New(ByRef GUI As Form)
        Try
            updateManager = New UpdateManager("C:\SquirrelTest\AppInstaller")
        Catch e As Exception

        End Try

        If (GUI Is Nothing) Then
            Throw New NullReferenceException
        End If
        Me.GUI = GUI
    End Sub

    Shared Sub HandleEvents()
        If (updateManager Is Nothing) Then
            Return
        End If

        SquirrelAwareApp.HandleEvents(
                onInitialInstall:=Sub(v) SquirrelInstall(),
                onAppUpdate:=Sub(v) updateManager.CreateShortcutForThisExe(),
                onAppUninstall:=Sub(v) updateManager.RemoveShortcutForThisExe()
            )
    End Sub

    Private Shared Sub SquirrelInstall()
        updateManager.CreateShortcutForThisExe()

        Dim icon = IO.Path.Combine(IO.Path.GetDirectoryName(Application.ExecutablePath), "APK.ico")
        My.Resources.android_app.Save(New IO.FileStream(icon, IO.FileMode.Create))

        Const APP_CLASS_KEY As String = "AppInstaller"
        Const APP_EXT As String = ".apk"

        Dim classes = Microsoft.Win32.Registry.CurrentUser.OpenSubKey("Software", True)

        classes.CreateSubKey("Classes")
        classes = classes.OpenSubKey("Classes", True)
        Dim key1 = classes.CreateSubKey(APP_EXT)

        'classes.CreateSubKey(APP_EXT)
        'key1 = classes.OpenSubKey(APP_EXT, True)
        key1.SetValue("", APP_CLASS_KEY) 'Set Default key value
        key1.Close()


        classes.CreateSubKey(APP_CLASS_KEY)
        Dim key2 = classes.OpenSubKey(APP_CLASS_KEY, True)

        key2.CreateSubKey("DefaultIcon")
        key2 = key2.OpenSubKey("DefaultIcon", True)
        key2.SetValue("", """" & icon & """") ' Set Default key value

        key2.Close()


        classes.CreateSubKey(APP_CLASS_KEY)
        Dim key3 = classes.OpenSubKey(APP_CLASS_KEY, True)

        key3.CreateSubKey("shell")
        key3 = key3.OpenSubKey("shell", True)

        key3.CreateSubKey("open")
        key3 = key3.OpenSubKey("open", True)

        key3.CreateSubKey("command")
        key3 = key3.OpenSubKey("command", True)
        key3.SetValue("", """" & Application.ExecutablePath & """" & " ""%1""") ' Set Default key value

        key3.Close()
    End Sub

    Private Shared Sub SquirrelUninstall()
        updateManager.RemoveShortcutForThisExe()

        Dim icon = IO.Path.Combine(IO.Path.GetDirectoryName(Application.ExecutablePath), "APK.ico")
        IO.File.Delete(icon)

        Const APP_CLASS_KEY As String = "AppInstaller"

        Dim classes = Microsoft.Win32.Registry.CurrentUser.OpenSubKey("Software", True)

        classes.CreateSubKey("Classes")
        classes = classes.OpenSubKey("Classes", True)

        classes.DeleteSubKey(APP_CLASS_KEY)

    End Sub

    Public Property UpdateLabel As MaterialLabel
        Get
            Return _updateLabel
        End Get
        Set(value As MaterialLabel)

            _updateLabel = value
            If (_updateLabel IsNot Nothing) Then
                AddHandler UpdateLabel.Click, AddressOf UpdateClick
                UpdateLabel.Visible = False
            End If
            'NotifyOfUpdate()
        End Set
    End Property

    Private Async Sub StartUpdate()
        If updateManager Is Nothing Then
            Exit Sub
        End If
        Dim updateInfo = Await updateManager.CheckForUpdate
        If (updateInfo.FutureReleaseEntry().Version.CompareTo(updateInfo.CurrentlyInstalledVersion.Version) > 0) Then
            Await updateManager.DownloadReleases(updateInfo.ReleasesToApply())
            Await updateManager.ApplyReleases(updateInfo)

            If (Me._updateLabel IsNot Nothing) Then
                NotifyOfUpdate()
            End If
        End If
    End Sub

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
            UpdateLabel.Text = "Update Ready! Click to restart whenever you are ready"
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
