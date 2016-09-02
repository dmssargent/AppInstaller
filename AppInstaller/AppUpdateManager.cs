using System;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using System.IO;
using System.Threading;
using System.Windows.Forms;
using APKInstaller.i18n;
using Microsoft.Win32;
using Squirrel;

namespace APKInstaller
{
    /// <summary>
    ///     Core Updating system of APKInstaller. This handles app installs, updates, and uninstalls
    ///     Usage:
    ///     Create a new instance, then call HandleEvents before the app has fully loaded. HandleEvents may result
    ///     in app termination due to a special event being triggered i.e. app install, so the GUI shouldn't be shown
    ///     Calling Update() will update the app, if possible (no-op if not possible), and if a new update has been released
    /// </summary>
    public class AppUpdateManager
    {
        static UpdateManager _updateManager;
        static AppUpdateManager _instance;
        static bool _configured;
        readonly Form _gui;
        Label _updateLabel;
        //_updateStatusText

        /// <summary>
        ///     Creates a new AppUpdateManager instance binded to a specific GUI
        /// </summary>
        /// <param name="gui">The main window form, and contains UpdateLabel, cannot be null</param>
        [SuppressMessage("Microsoft.Design", "CA1045:DoNotPassTypesByReference", MessageId = "0#")]
        public AppUpdateManager(ref Form gui)
        {
            //MsgBox("foo")
            _instance = this;

            dynamic updateManager = new Thread(GetUpdateManager);
            updateManager.Name = "Update Manager";
            updateManager.Start();

            if (gui == null)
                throw new ArgumentNullException(nameof(gui));
            _gui = gui;
        }

        //Private Shared _updateStatusText As String = "Everything is up-to-date"

        //Private _updatesEnabled As Boolean

        /// <summary>
        ///     The label that update notification should be applied to; must be a child of the GUI form used when creating the
        ///     instance
        /// </summary>
        /// <returns>the current update notification label</returns>
        public Label UpdateLabel
        {
            get { return _updateLabel; }
            set
            {
                _updateLabel = value;
                if (_updateLabel == null) return;
                // Add our click handler on the fly
                UpdateLabel.Click += UpdateClick;
                UpdateLabel.Visible = false;
            }
        }

        public static string UpdateStatusText { get; private set; } = "Everything is up-to-date";


        static async void GetUpdateManager()
        {
            try
            {
                const string updatePath = "https://github.com/dmssargent/AppInstaller";
                // todo: on release allow prerelease to toggle on and off
                var githubMgr = await UpdateManager.GitHubUpdateManager(updatePath, prerelease: true);

                _updateManager = githubMgr;
            }
            catch (Exception e)
            {
                Console.Write(e.ToString());
            }

            _configured = true;
        }

        /// <summary>
        ///     Handles installer events, some of which may result in termination without the main form being loaded
        /// </summary>
        public static void HandleEvents()
        {
            //MsgBox("Squirrel Init")
            if (!VerifyState())
                return;

            //MsgBox("Squirrel Handle")
            SquirrelAwareApp.HandleEvents(v => SquirrelInstall(),
                v => _updateManager.CreateShortcutForThisExe(), onAppUninstall: v => SquirrelUninstall());
        }

        static bool VerifyState()
        {
            if (_updateManager == null)
            {
                while (!_configured)
                    Thread.Sleep(10);

                if (_updateManager == null)
                    return false;
            }
            return true;
        }

        static void SquirrelInstall()
        {
            //MsgBox("Squirrel Install")
            _updateManager.CreateShortcutForThisExe();

            // Create a default icon
            dynamic icon = Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), "APK.ico");
            using (var file = new FileStream(icon, FileMode.Create))
            {
                // todo: android_app.Save(file);
            }

            // Register the APK extension to this app
            const string classKey = "AppInstaller";
            const string appExt = ".apk";

            dynamic classes = Registry.CurrentUser.OpenSubKey("Software", true);

            if (classes == null) return;
            classes.CreateSubKey("Classes");
            classes = classes.OpenSubKey("Classes", true);
            dynamic appExtKey = classes.CreateSubKey(appExt);

            appExtKey.SetValue("", classKey);
            //Set Default key value
            appExtKey.Close();


            classes.CreateSubKey(classKey);
            dynamic appClassKey = classes.OpenSubKey(classKey, true);

            appClassKey.CreateSubKey("DefaultIcon");
            dynamic appDefaultIcon = appClassKey.OpenSubKey("DefaultIcon", true);
            appDefaultIcon.SetValue("", "\"" + Application.ExecutablePath + ",1\"");
            // Set Default key value
            appDefaultIcon.Close();

            appClassKey.CreateSubKey("shell");
            dynamic shellKey = appClassKey.OpenSubKey("shell", true);

            shellKey.CreateSubKey("open");
            shellKey = shellKey.OpenSubKey("open", true);

            shellKey.CreateSubKey("command");
            shellKey = shellKey.OpenSubKey("command", true);
            shellKey.SetValue("", "\"" + Application.ExecutablePath + "\"" + " \"%1\"");
            // Set Default key value
            shellKey.Close();

            appClassKey.Close();
        }

        static void SquirrelUninstall()
        {
            _updateManager.RemoveShortcutForThisExe();

            dynamic icon = Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), "APK.ico");
            File.Delete(icon);

            const string appClassKey = "AppInstaller";

            dynamic classes = Registry.CurrentUser.OpenSubKey("Software", true);

            if (classes == null) return;
            classes.CreateSubKey("Classes");
            classes = classes.OpenSubKey("Classes", true);

            // According to MSDN, do not delete the file extension key, only delete the Class that the file extension is registered with
            classes.DeleteSubKey(appClassKey);
        }


        async void StartUpdate()
        {
            if (!VerifyState())
                return;

            var updateInfo = await _updateManager.CheckForUpdate();
            if (updateInfo.FutureReleaseEntry.Version.CompareTo(updateInfo.CurrentlyInstalledVersion.Version) > 0)
                NotifyOfUpdate();
        }

        /// <summary>
        ///     Updates the current app asynchronously
        /// </summary>
        public void Update()
        {
            dynamic update = new Thread(StartUpdate);
            update.Start();
        }

        public static void UpdateApp()
        {
            _instance.Update();
        }

        void NotifyOfUpdate()
        {
            UpdateStatusText = UIStrings.updateReady;

            if (_updateLabel == null)
                return;

            if (UpdateLabel.InvokeRequired)
            {
                UpdateLabel.Invoke(new SetUpdateCallback(NotifyOfUpdate));
            }
            else
            {
                UpdateLabel.Text = UIStrings.updateReady;
                //Strings.ResourceManager.GetString("updateReady")
                UpdateLabel.Font = new Font(UpdateLabel.Font, FontStyle.Underline);
                UpdateLabel.Visible = true;
                if (!_gui.InvokeRequired)
                    _gui.Height += 20;
            }
        }

        void UpdateClick(object sender, EventArgs e)
        {
            if (_updateManager != null)
                UpdateManager.RestartApp();
        }

        public static void CleanUp()
        {
            _updateManager?.Dispose();
        }

        delegate void SetUpdateCallback();
    }
}