using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using APKInstaller.i18n;
using MaterialSkin;
using MaterialSkin.Controls;

namespace APKInstaller
{
    public partial class Main : MaterialForm
    {
        //Private stopAll As Boolean
        readonly Installer _apkInstaller;

        readonly AppUpdateManager _updateMgr;
        bool _dialogLock;

        bool _singleInstall;
        //private Button button1;

        // private Label lblStatus;
        bool _txtLock;

        public Main()
        {
            InitializeComponent();

            Closed += Form1_Closed;
            Closing += Form1_Closing;
            GotFocus += Main_GotFocus;
            DragEnter += Me_DragEnter;
            DragDrop += Me_DragDrop;
            Load += APKInstallerMain_Load;


            var test = this;
            Form test2 = this;
            // Add any initialization after the InitializeComponent() call.
            _updateMgr = new AppUpdateManager(ref test2);
            //_updateMgr.UpdateLabel = Label1

            _apkInstaller = new Installer(ref test, ref lblStatus, ref txtFileLocation);

            //APKInstallerMain_Load();
        }

        //[Log("App Installer Debug")]
        void APKInstallerMain_Load(object sender, EventArgs e)
        {
            // Handle Fatal Errors for further diagnostics
            Application.ThreadException += FatalErrorWasThrownHandlerTE;
            AppDomain.CurrentDomain.UnhandledException += FatalErrorWasThrownHandlerCD;

            // Configure app setup and updates
            AppUpdateManager.HandleEvents();
            _updateMgr.Update();

            //Configure GUI
            btnInstall.Visible = false;
            pgbStatus.Visible = false;

            var manager = MaterialSkinManager.Instance;
            manager.AddFormToManage(this);
            SkinManager.Theme = MaterialSkinManager.Themes.LIGHT;
            SkinManager.ColorScheme = new ColorScheme(Primary.Orange700, Primary.Orange700, Primary.Orange100, Accent.LightBlue200, TextShade.WHITE);
            lnkAbout.Font = SkinManager.ROBOTO_MEDIUM_10;
            lnkHelp.Font = SkinManager.ROBOTO_MEDIUM_10;
            lnkAbout.ForeColor = Color.White;
            lnkHelp.ForeColor = Color.White;

            CenterToScreen();

            // Dim manager = SkinManager.Instance
            // manager.AddFormToManage(Me)
            //SkinManager.Theme = SkinManager.Themes.LIGHT
            //SkinManager.ColorScheme = New ColorScheme(Primary.Orange700, Primary.Orange700, Primary.Orange100, Accent.LightBlue200, TextShade.WHITE)
            //lnkAbout.Font = SkinManager.ROBOTO_MEDIUM_10
            //lnkHelp.Font = SkinManager.ROBOTO_MEDIUM_10

            CenterToScreen();

            // Start parsing the arguments
            dynamic noPrompt = false;
            var appList = new List<string>();
            dynamic enableFileArg = true;
            foreach (var arg in Environment.GetCommandLineArgs())
            {
                if (arg.StartsWith("--", StringComparison.Ordinal) | arg.StartsWith("-", StringComparison.Ordinal))
                {
                    if (arg.Contains("squirrel") && !arg.Contains("firstrun"))
                        enableFileArg = false;
                    noPrompt = ParseNonFileArgument(noPrompt, arg);
                    continue;
                }

                if (!enableFileArg) continue;
                if (arg.EndsWith(".exe", StringComparison.OrdinalIgnoreCase))
                    continue;
                ParseFileArgument(appList, arg);
            }

            if (!enableFileArg)
            {
                MessageBox.Show("The installer is broken.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                Close();
            }

            var fileArgs = appList.ToArray();
            _apkInstaller.AddFilesToInstall(fileArgs);
            if ((fileArgs.Length != 0) & (txtFileLocation.Text.Length > 0) & btnInstall.Enabled)
                SetupSingleInstall(e, noPrompt);
        }

        void SetupSingleInstall(EventArgs e, bool noPrompt)
        {
            _singleInstall = true;
            txtFileLocation.Visible = false;
            btnInstall.Visible = false;
            btnOpenFileDialogTrigger.Visible = false;
            chkForce.Visible = false;
            chkReinstall.Visible = false;
            if (!noPrompt)
            {
                chkReinstall.Checked =
                    MessageBox.Show(UIStrings.updateQuestion, "Reinstall", MessageBoxButtons.YesNo,
                        MessageBoxIcon.Question) == DialogResult.Yes;
                if (!chkReinstall.Checked)
                    chkForce.Checked =
                        MessageBox.Show(UIStrings.forceQuestion, "Force", MessageBoxButtons.YesNo,
                            MessageBoxIcon.Question) == DialogResult.Yes;
            }

            btnInstaller_Click(btnInstall, e);
        }

        void ParseFileArgument(List<string> appList, string arg)
        {
            if (MessageBox.Show(UIStrings.installApkAtConfirm + "\"" + arg + "\"",
                    "Confirm File", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) != DialogResult.OK)
            {
                MessageBox.Show(UIStrings.installAborted, "Aborted", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                Close();
            }
            else
            {
                appList.Add(arg);
            }
        }

        bool ParseNonFileArgument(bool noPrompt, string arg)
        {
            if (arg.Equals("-f") | arg.Equals("--force"))
            {
                chkForce.Checked = true;
                chkReinstall.Checked = false;
                noPrompt = true;
            }
            else if (arg.Equals("-r") | arg.Equals("--update") | arg.Equals("--reinstall"))
            {
                chkReinstall.Checked = true;
                chkForce.Checked = false;
                noPrompt = true;
            }
            else if (arg.Equals("/?") | arg.Equals("--help") | arg.Equals("/h") | arg.Equals("-h"))
            {
                MessageBox.Show(
                    About.AssemblyTitle + " v" + About.AssemblyVersion + "\n" +
                    "\n" + "Usage Options " + "\n" + "--force | -f" + "\n" +
                    "\t" + "Removes any existing application And wipes " + "\n" + "\t" +
                    "any associated application data for that " + "\n" + "\t" +
                    "application, before installing" + "\n" + "--update | --reinstall | -r" +
                    "\n" + "\t" + "Reinstalls the app represented by the given APK." +
                    "\n" + "\t" + "Does nothing if the app Is Not already installed" +
                    "\n" + "--no-prompt | -np" + "\n" + "\t" +
                    "Does Not prompt user for settings" + "\n" + "\t" +
                    " [Implicit with options: -r, -f]" + "\n" + "-!np | --prompt" + "\n" +
                    "\t" + "Prompts the user about options [Overrides: -np]",
                    "Usage", MessageBoxButtons.OK, MessageBoxIcon.Information);
                Close();
            }
            else if (arg.Equals("-np") | arg.Equals("--no-prompt"))
            {
                noPrompt = true;
            }
            else if (arg.Equals("--squirrel-firstrun"))
            {
                SetText(ref lblStatus, UIStrings.firstRunMessage);
            }
            else if (arg.Equals("-!np") | arg.Equals("--prompt"))
            {
                noPrompt = false;
            }

            return noPrompt;
        }

        void btnOpenFileDialogTrigger_Click(object sender, EventArgs e)
        {
            using (var fileDialog = new OpenFileDialog())
            {
                if (txtFileLocation.Text.Length > 0)
                    if (txtFileLocation.Text.Contains(Path.PathSeparator.ToString()))
                        fileDialog.FileName =
                            txtFileLocation.Text.Substring(
                                txtFileLocation.Text.LastIndexOf(Path.PathSeparator.ToString(),
                                    StringComparison.OrdinalIgnoreCase),
                                txtFileLocation.Text.Length -
                                txtFileLocation.Text.LastIndexOf(Path.PathSeparator.ToString(),
                                    StringComparison.OrdinalIgnoreCase));
                    else
                        fileDialog.FileName = txtFileLocation.Text;
                fileDialog.AutoUpgradeEnabled = true;
                fileDialog.CheckFileExists = true;
                fileDialog.DefaultExt = ".apk";
                fileDialog.Title = UIStrings.openFileDialogTitle;
                fileDialog.Multiselect = true;
                fileDialog.ValidateNames = true;
                fileDialog.Filter = UIStrings.openFileDialogFilter;
                fileDialog.ShowDialog();

                _apkInstaller.AddFilesToInstall(fileDialog.FileNames);
            }
        }

        void Me_DragDrop(object sender, DragEventArgs e)
        {
            var files = (string[])e.Data.GetData(DataFormats.FileDrop);
            _apkInstaller.AddFilesToInstall(files);
        }

        void Me_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
                e.Effect = DragDropEffects.Copy;
            else
                e.Effect = DragDropEffects.None;
        }

        void btnInstaller_Click(object sender, EventArgs e)
        {
            btnInstall.Visible = false;

            _apkInstaller.Reinstall = chkReinstall.Checked;
            _apkInstaller.Force = chkForce.Checked;
            _apkInstaller.CompletionMessageWhenFinished = !_singleInstall;

            var installerThread = new Thread(_apkInstaller.StartInstall);
            installerThread.Start();
        }

        void txtFileLocation_TextChanged(object sender, EventArgs e)
        {
            if ((_apkInstaller == null) || _apkInstaller.UseMultiFileDialog) return;
            dynamic pos = txtFileLocation.SelectionStart;
            dynamic currentData = txtFileLocation.Text;
            if (currentData.Contains(","))
            {
                dynamic firstIndex = currentData.IndexOf(",", StringComparison.Ordinal);
                if (firstIndex > pos)
                {
                    currentData = currentData.Substring(0, firstIndex + 1);
                }
                else
                {
                    if (pos >= currentData.Length)
                    {
                        currentData =
                            currentData.Substring(currentData.LastIndexOf(",", StringComparison.Ordinal) + 1);
                    }
                    else
                    {
                        dynamic index = -1;
                        while (true)
                        {
                            dynamic temp = currentData.IndexOf(",", index + 1, StringComparison.Ordinal);
                            if (temp >= pos)
                                break; // TODO: might not be correct. Was : Exit While
                            index = temp;
                        }
                        currentData = currentData.Substring(index);
                        if (currentData.Contains(","))
                            currentData = currentData.Substring(0,
                                currentData.IndexOf(",", StringComparison.Ordinal) + 1);
                    }
                }
            }

            CheckInstallerConfig(Installer.ValidateFile(currentData));
        }

        void Main_GotFocus(object sender, EventArgs e)
        {
            CheckInstallerConfig();
        }

        void CheckInstallerConfig(bool optionalConditon = true)
        {
            if (_apkInstaller == null)
            {
                //Exit Sub
            }

            if ((_apkInstaller != null) && (_apkInstaller.VerifyFilesToInstall(true) & optionalConditon))
            {
                btnInstall.Visible = true;
                lblStatus.Text = UIStrings.readyToInstall;
                ConfigureAppListFromTextbox();
            }
            else
            {
                btnInstall.Visible = false;
                lblStatus.Text = txtFileLocation.Text.Length == 0 ? UIStrings.dragFile : UIStrings.fileCanNotBeInstalled;
            }
        }

        internal void ResetGui(string done)
        {
            if (InvokeRequired)
            {
                Invoke(new ResetGuiCallback(ResetGui), done);
            }
            else
            {
                string message = null;
                if (_apkInstaller.VerifyFilesToInstall())
                {
                    btnInstall.Visible = _apkInstaller.VerifyFilesToInstall();
                    message = UIStrings.readyToInstall;
                }
                else
                {
                    message = txtFileLocation.Text.Length == 0 ? UIStrings.dragFile : UIStrings.fileCanNotBeInstalled;
                }
                lblStatus.Text = done == null ? message : done + "\n" + message;
                chkForce.Visible = true;
                chkReinstall.Visible = true;
                UseWaitCursor = false;
                pgbStatus.Visible = false;
                pgbStatus.Step = 0;
                pgbStatus.Value = 0;
                _apkInstaller.Reset();
            }
        }

        void chkReinstall_CheckedChanged(object sender, EventArgs e)
        {
            if (chkReinstall.Checked)
            {
                chkForce.Checked = false;
                chkForce.Enabled = false;
            }
            else
            {
                chkForce.Enabled = true;
            }
        }

        void chkForce_CheckedChanged(object sender, EventArgs e)
        {
            if (chkForce.Checked)
                chkForce.Checked =
                    MessageBox.Show(UIStrings.forceConfirm, "Force", MessageBoxButtons.YesNo, MessageBoxIcon.Question) ==
                    DialogResult.Yes;
        }

        void Form1_Closing(object sender, EventArgs e)
        {
            _apkInstaller.Abort();
        }

        //[Log("App Installer Debug")]
        void Form1_Closed(object sender, EventArgs e)
        {
            IoUtilities.Cleanup();
            // Cleanup all of the tools we used
            AppUpdateManager.CleanUp();
        }

        //[SuppressMessage("Microsoft.Design", "CA1045:DoNotPassTypesByReference", MessageId = "0#")]
        //[Log("App Installer Debug")]
        public void SetText(ref Label label, string message)
        {
            if (label == null)
                throw new ArgumentNullException(nameof(label));

            if (label.InvokeRequired)
            {
                SetTextCallback d = SetText;
                Invoke(d, label, message);
            }
            else
            {
                label.Text = message;
            }
        }

        //[Log("App Installer Debug")]
        public void ShowProgressAnimation(bool isVisible, bool useStep)
        {
            ShowProgressAnimation(isVisible, useStep, 0);
        }

        // [SuppressMessage("Microsoft.Design", "CA1026:DefaultParametersShouldNotBeUsed")]
        //[Log("App Installer Debug")]
        public void ShowProgressAnimation(bool isVisible, bool useStep, int stepAmount = 0)
        {
            if (pgbStatus.InvokeRequired)
            {
                VisibilityCallback d = ShowProgressAnimation;
                Invoke(d, isVisible, useStep, stepAmount);
            }
            else
            {
                pgbStatus.Visible = isVisible;
                if (useStep)
                {
                    pgbStatus.Style = ProgressBarStyle.Continuous;
                    pgbStatus.Step = stepAmount;
                }
                else
                {
                    pgbStatus.Style = ProgressBarStyle.Marquee;
                }
            }
        }

        //[Log("App Installer Debug")]
        public void StepProgressBar()
        {
            if (pgbStatus.InvokeRequired)
            {
                StepProgressBarDelegate d = StepProgressBar;
                Invoke(d);
            }
            else
            {
                pgbStatus.PerformStep();
            }
        }


        static void FatalErrorWasThrownHandler(Exception e)
        {
            dynamic details = "";
            if (e != null)
                details = Convert.ToBase64String(Encoding.Unicode.GetBytes(e.Message + e.StackTrace));
            var sTempFileName = Path.GetTempFileName();
            using (var fsTemp = new FileStream(sTempFileName, FileMode.Create))
            {
                byte[] detailsDump = Encoding.Unicode.GetBytes(details);
                fsTemp.Write(detailsDump, 0, details.Length);
            }
            MessageBox.Show(
                UIStrings.appError + "\n" +
                (e == null ? "" : UIStrings.appErrorFileWanted + "\n" + sTempFileName + "\n") +
                UIStrings.wantToRestart, "App Error", MessageBoxButtons.YesNo, MessageBoxIcon.Stop);
            try
            {
                File.Delete(sTempFileName);
            }
            catch (FileNotFoundException ex)
            {
                throw new IOException("The diagnostics file name is invalid. This shouldn't happen ever." +
                                      "\n" + "File name: " + sTempFileName);
            }
            catch (Exception ex)
            {
                throw new IOException("The diagnostic file can't be automatically removed at runtime.", ex);
            }
        }

        //[Log("App Installer Debug")]
        static void FatalErrorWasThrownHandlerTE(object sender, ThreadExceptionEventArgs e)
        {
            FatalErrorWasThrownHandler(e.Exception);
        }

        //[Log("App Installer Debug")]
        static void FatalErrorWasThrownHandlerCD(object sender, UnhandledExceptionEventArgs e)
        {
            FatalErrorWasThrownHandler((Exception)e.ExceptionObject);
        }

        //[Log("App Installer Debug")]
        void txtFileLocation_GotFocus(object sender, EventArgs e)
        {
            if (_apkInstaller == null) return;

            if (_apkInstaller.UseMultiFileDialog)
            {
                txtFileLocation.Enabled = false;

                //Dim test = ""
                //For Each file In _apkInstaller.GetFilesToInstall
                //    test &= file & Path.PathSeparator
                //Next file

                using (var dialog = MultiPackageDialog.Create(_apkInstaller.GetFilesToInstall()))
                {
                    if (_dialogLock) return;

                    _dialogLock = true;
                    if (dialog.ShowDialog() == DialogResult.OK)
                        _apkInstaller.AddFilesToInstall(dialog.GetFiles(), true);
                    _dialogLock = false;
                }

                //MsgBox("Use multifile dialog" & test)
            }
        }

        void txtFileLocation_LostFocus(object sender, EventArgs e)
        {
            if ((_apkInstaller != null) && _apkInstaller.UseMultiFileDialog)
                txtFileLocation.Enabled = true;
            else
                ConfigureAppListFromTextbox();
        }

        void ConfigureAppListFromTextbox()
        {
            if (_txtLock) return;

            _txtLock = true;
            var files = ExtractFilesFromString(txtFileLocation.Text);

            if ((_apkInstaller != null) && !_apkInstaller.UseMultiFileDialog)
            {
                dynamic desc = ExtractFilesFromString(_apkInstaller.FilesToInstallDescription);
                foreach (var file in files)
                    if (!desc.Contains(file))
                        _apkInstaller.RemoveFile(file);
                dynamic tempText = txtFileLocation.Text;
                dynamic tempPos = txtFileLocation.SelectionStart;
                _apkInstaller.AddFilesToInstall(files.ToArray(), false, false);
                txtFileLocation.Text = tempText;
                txtFileLocation.SelectionStart = tempPos;
            }
            _txtLock = false;
        }

        static List<string> ExtractFilesFromString(string data)
        {
            dynamic files = new List<string>();
            data = data.Replace("...", "");
            if (data.Contains(","))
            {
                dynamic commaFiles = data.Split(Convert.ToChar(","));
                foreach (var file in commaFiles)
                    if (data.Contains(file))
                        data = data.Replace(file, "");
                data = data.Replace(",", "");
                files.AddRange(commaFiles);
            }
            if (data.Contains(Path.PathSeparator.ToString()))
            {
                dynamic collection = data.Split(Path.PathSeparator);
                foreach (var file in collection)
                    if (data.Contains(file))
                        data = data.Replace(file, "");
                data = data.Replace(Path.PathSeparator.ToString(), "");
                files.AddRange(collection);
            }
            if (!ReferenceEquals(data, ""))
                files.Add(data);

            return files;
        }

        void txtFileLocation_DoubleClick(object sender, EventArgs e)
        {
            if (_dialogLock) return;

            dynamic dialog = MultiPackageDialog.Create(_apkInstaller.GetFilesToInstall());
            _dialogLock = true;
            if (dialog.ShowDialog() == DialogResult.OK)
                _apkInstaller.AddFilesToInstall(dialog.GetFiles(), true);
            _dialogLock = false;
        }

        void lnkAbout_Click(object sender, EventArgs e)
        {
            using (var aboutDialog = new About())
            {
                aboutDialog.ShowDialog();
            }
        }


        void lnkHelp_Click(object sender, EventArgs e)
        {
            // Using browser = New Process()
            UseWaitCursor = true;
            Process.Start("https://cdn.rawgit.com/dmssargent/AppInstaller/v0.1.7.1-rc/docs/manual.pdf");
            //MsgBox("The help feature isn't quite done.")
            UseWaitCursor = false;
            //End Using
        }

        // This delegate enables asynchronous calls for setting
        // the text property on a TextBox control.
        delegate void SetTextCallback(ref Label label, string text);

        delegate void VisibilityCallback(bool visible, bool useStep, int stepAmount);

        delegate void StepProgressBarDelegate();

        delegate void ResetGuiCallback(string message);
    }
}