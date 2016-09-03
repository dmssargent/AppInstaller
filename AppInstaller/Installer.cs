using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using APKInstaller.i18n;
using MaterialSkin.Controls;

namespace APKInstaller
{
    /// <summary>
    ///     Handler of installing Android packages
    /// </summary>
    public class Installer
    {
        readonly LinkedList<string> _filesToInstall = new LinkedList<string>();
        readonly Main _gui;
        readonly MaterialSingleLineTextField _txtUserInput;
        Label _lblStatus;
        bool _stopRequested;

        /// <summary>
        ///     Creates a new installer instance
        /// </summary>
        /// <param name="entry">The main GUI of the application</param>
        /// <param name="statusLabel">The label to use as installer statuses</param>
        /// <param name="userInputTextBox">The user input textbox</param>
        [SuppressMessage("Microsoft.Design", "CA1045:DoNotPassTypesByReference", MessageId = "2#")]
        [SuppressMessage("Microsoft.Design", "CA1045:DoNotPassTypesByReference", MessageId = "1#")]
        [SuppressMessage("Microsoft.Design", "CA1045:DoNotPassTypesByReference", MessageId = "0#")]
        public Installer(ref Main entry, ref Label statusLabel, ref MaterialSingleLineTextField userInputTextBox)
        {
            _gui = entry;
            _lblStatus = statusLabel;
            _txtUserInput = userInputTextBox;
        }

        /// <summary>
        ///     Determines if the operations performed are update (reinstall) operations, or not
        /// </summary>
        /// <returns>true if the packages are going to be reinstalled; otherwise false</returns>
        public bool Reinstall { get; set; } = true;

        /// <summary>
        ///     Whether to force the package installation or not
        /// </summary>
        /// <returns>true if the packages will be forcibly installed</returns>
        public bool Force { get; set; }

        /// <summary>
        ///     Shows a completion message after the installation of all packages is finished
        /// </summary>
        /// <returns>if a completion message will be shown after the installation job is finished</returns>
        public bool CompletionMessageWhenFinished { get; set; }

        public bool UseMultiFileDialog { get; set; }

        public string FilesToInstallDescription { get; private set; } = "";

        public void AddFilesToInstall(string[] files)
        {
            AddFilesToInstall(files, false);
        }

        public void AddFilesToInstall(string[] files, bool clear)
        {
            AddFilesToInstall(files, clear, true);
        }

        /// <summary>
        ///     Adds files to be installed
        /// </summary>
        /// <param name="files">files to be installed</param>
        /// <param name="clear"></param>
        /// <param name="notifyUser"></param>
        //[Log("App Installer Debug")]
        public void AddFilesToInstall(string[] files, bool clear, bool notifyUser)
        {
            if (files == null)
                throw new ArgumentNullException(nameof(files));

            if (clear)
                _filesToInstall.Clear();

            foreach (var path in files)
            {
                // Prevent bugs in detection routine
                if (path == null)
                    continue;

                var path2 = path.Trim();

                // Determine if the file is already in the installer list, if so skip it
                if (_filesToInstall.Contains(path2))
                    continue;

                try
                {
                    if (ValidateFile(path, notifyUser))
                        _filesToInstall.AddLast(path);
                }
                catch (IOException ex)
                {
                    MessageBox.Show(ex.Message, UIStrings.Invalid_File, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
            }

            _txtUserInput.Text = GenerateFileListSummary();
            FilesToInstallDescription = _txtUserInput.Text;
        }

        //[Log("App Installer Debug")]
        public void RemoveFile(string path)
        {
            _filesToInstall.Remove(path);
        }

        public static bool ValidateFile(string path) => ValidateFile(path, false);

        /// <summary>
        ///     Determines if the given file is a valid APK file
        /// </summary>
        /// <param name="path">the file location for the APK file</param>
        /// <param name="notifyUser">true will notify the user of the problem via dialogs, false will not</param>
        /// <returns>true if the file is a valid APK, false otherwise</returns>
        //[Log("App Installer Debug")]
        public static bool ValidateFile(string path, bool notifyUser)
        {
            if (path == null)
                throw new ArgumentNullException(nameof(path));

            // Check to see if the file exists
            if (!File.Exists(path))
            {
                if (notifyUser)
                    MessageBox.Show(
                        Directory.Exists(path) ? "\"" + path + "\" " + UIStrings.isDirError : UIStrings.fileDoesNotExist,
                        UIStrings.Invalid_File, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            try
            {
                // Check for the correct extension and that the file can be correctly parsed as an APK
                if (path.ToUpper(CultureInfo.CurrentCulture)
                        .EndsWith(".APK", StringComparison.CurrentCultureIgnoreCase) &
                    (AndroidTools.PackageName(path) != "")) return true;
                if (notifyUser)
                    MessageBox.Show($@"""{path}"" {UIStrings.invalidApk}", UIStrings.Invalid_File, MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);
                return false;
            }
            catch (IOException ex)
            {
                if (notifyUser)
                    MessageBox.Show($@"""{path}"" {UIStrings.invalidApk}", UIStrings.Invalid_File, MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);
                return false;
            }
        }

        string GenerateFileListSummary()
        {
            switch (_filesToInstall.Count)
            {
                case 0:
                    UseMultiFileDialog = false;
                    return "";
                case 1:
                    UseMultiFileDialog = false;
                    return _filesToInstall.First.Value;
            }
            UseMultiFileDialog = true;
            return _filesToInstall.ElementAt(0) + ", " + _filesToInstall.ElementAt(1) +
                   (_filesToInstall.Count > 2 ? "..." : "");
        }

        /// <summary>
        ///     Verifies that all of the files to be installed are valid
        /// </summary>
        /// <returns></returns>
        public bool VerifyFilesToInstall(bool excuseNoFiles = false)
        {
            dynamic apkFiles = GetFilesToInstall();

            // Nothing to install
            if (apkFiles.Length == 0)
                return excuseNoFiles;

            foreach (var apkFile in apkFiles)
                if (!ValidateFile(apkFile))
                    return false;
            return true;
        }

        /// <summary>
        ///     Starts the install of the APK files
        /// </summary>
        public void StartInstall()
        {
            var thread = new Thread(Install);
            thread.Start();
        }

        /// <summary>
        ///     Installs the APK packages
        /// </summary>
        void Install()
        {
            // Wait until an Android device is connected
            dynamic waitForDeviceReturn = WaitForDevice();
            if (waitForDeviceReturn != ErrorCode.Success)
            {
                HandleWaitForDeviceError(waitForDeviceReturn);
                return;
            }


            // Device Found, check if multiple devices are connected, and figure out the device to install to
            dynamic deviceId = AcquireDeviceId();
            if (deviceId == null)
            {
                _gui.ResetGui(UIStrings.installAborted);
                return;
            }

            // Begin device install sections
            _gui.SetText(ref _lblStatus, UIStrings.startingInstalls);
            var filesToInstall = GetFilesToInstall();
            dynamic installStatus = "";
            dynamic installAborted = false;
            dynamic installHasFailed = false;
            _gui.ShowProgressAnimation(true, true, Convert.ToInt32(100 / filesToInstall.Length));
            const int maxRetryCount = 3;
            foreach (var file in filesToInstall)
            {
                // Check if the install has been aborted, if so stop the installs
                if (installAborted)
                {
                    break;
                }

                // Retry Loop
                dynamic packageInstallSuccess = false;
                for (var retry = 0; (retry <= maxRetryCount) && !packageInstallSuccess; retry++)
                {
                    //Display a status
                    _gui.SetText(ref _gui.lblStatus,
                        retry == 0
                            ? UIStrings.installing
                            : UIStrings.retrying_install + " \"" + file + UIStrings.onto_the_device + deviceId + "\"");
                    ErrorCode installAttempt = PackageInstallAttempt(deviceId, file);
                    switch (installAttempt)
                    {
                        case ErrorCode.Abort:
                            // ReSharper disable once RedundantAssignment
                            installAborted = true;
                            return;
                        case ErrorCode.Failure1:
                        case ErrorCode.Failure2:
                            continue;
                        case ErrorCode.Success:
                        case ErrorCode.Ignore:
                            packageInstallSuccess = true;
                            break;
                    }
                }
                // End retry loop
                if (packageInstallSuccess) continue;
                MessageBox.Show($@"The file ""{file}"" couldn't be installed.", UIStrings.Installer_Install_Failed_install,
                    MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                installHasFailed = true;
            }
            // for each file
            _gui.UseWaitCursor = false;
            if (installAborted)
            {
                if (CompletionMessageWhenFinished)
                {
                    _gui.ResetGui(UIStrings.failure + UIStrings.details + "\n" + installStatus);
                }
                else
                {
                    MessageBox.Show(UIStrings.failure + UIStrings.details + "\n" + installStatus, "Install Aborted",
                        MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    _gui.Close();
                }
            }
            else
            {
                if (CompletionMessageWhenFinished)
                {
                    dynamic message = UIStrings.installationOf +
                                      (filesToInstall.Length == 1 ? UIStrings.apk : UIStrings.apks);
                    if (installHasFailed)
                        MessageBox.Show(message + UIStrings.hasFinishedError);
                    else
                        MessageBox.Show(message + UIStrings.hasFinishedSucess, UIStrings.apkInstallFinished,
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                    _gui.ResetGui(UIStrings.done);
                }
                else
                {
                    _gui.SetText(ref _gui.lblStatus, UIStrings.done);
                }
            }
            //FinishInstall(If(installAborted, UIStrings.failure & UIStrings.details & vbCrLf & installStatus, UIStrings.done))
            // End device install section
        }

        ErrorCode PackageInstallAttempt(string deviceId, string file)
        {
            if (Force)
                ForceRemovePackage(file);

            dynamic adbCode = InstallSinglePackage(deviceId, file);
            // Install Single Package failed catastrophically when this happened
            if (adbCode == null)
                return ErrorCode.Failure1;

            int result;
            // Checks if the result is a return code
            if (!int.TryParse(adbCode, out result)) return HandleInstallFailure(file, adbCode);

            if (result != 0) return HandleInstallFailure(file, result);
            _gui.StepProgressBar();
            return ErrorCode.Success;
            // the result is an details error message
        }

        void HandleWaitForDeviceError(ErrorCode waitForDeviceReturn)
        {
            dynamic message = "An unknown error while waiting for the device";
            switch (waitForDeviceReturn)
            {
                case ErrorCode.FailureTimeout:
                    message = $"{UIStrings.timeoutWaiting}\n" +
                              $"{UIStrings.userTroubleshootingA1}\n" +
                              $"{UIStrings.userTroubleshootingA2}";
                    break;
                case ErrorCode.Abort:
                    message = UIStrings.installAborted;
                    break;
                case ErrorCode.Success:
                    break;
                case ErrorCode.Ignore:
                    break;
                case ErrorCode.Failure1:
                    break;
                case ErrorCode.Failure2:
                    break;
                case ErrorCode.Failure3:
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(waitForDeviceReturn), waitForDeviceReturn, null);
            }

            if (CompletionMessageWhenFinished)
            {
                _gui.ResetGui(message);
            }
            else
            {
                MessageBox.Show(message, UIStrings.Device_Error, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                _gui.Close();
            }
        }

        ErrorCode WaitForDevice()
        {
            dynamic counter = 0;
            using (var adbWait = AndroidTools.RunAdb("wait-for-device", false, true, false))
            {
                var caret = 0;
                _gui.UseWaitCursor = true;
                _gui.ShowProgressAnimation(true, false);
                while (!adbWait.HasExited)
                {
                    string currentMessage = null;
                    switch (caret)
                    {
                        case 0:
                            currentMessage = UIStrings.waitForDevice1;
                            break;
                        case 1:
                            currentMessage = UIStrings.waitForDevice2;
                            break;
                        default: // caret == 2
                            currentMessage = UIStrings.waitForDevice3;
                            caret = -1;
                            break;
                    }
                    caret++;

                    // Display help message after 10 seconds
                    if (counter > 10)
                        currentMessage += $"\n{UIStrings.userTroubleshootingA1}\n{UIStrings.userTroubleshootingA2}";
                    _gui.SetText(ref _lblStatus, currentMessage);

                    Thread.Sleep(1000);
                    Thread.Yield();
                    if (_stopRequested)
                    {
                        MessageBox.Show(UIStrings.installAborted, UIStrings.APK_Install_Aborted, MessageBoxButtons.OK,
                            MessageBoxIcon.Exclamation);
                        return ErrorCode.Abort;
                    }
                    counter++;
                
                    if (counter > 60) // Timeout after 60 seconds
                        return ErrorCode.FailureTimeout;
                }
                _gui.ShowProgressAnimation(false, false);
            }
            return 0;
        }

        string AcquireDeviceId()
        {
            _gui.SetText(ref _lblStatus, UIStrings.checkDevices);
            _gui.ShowProgressAnimation(true, false);

            string deviceId = null;
            // ReSharper disable once RedundantAssignment
            GetDeviceId(id => { deviceId = id; });
            while (deviceId == null)
            {
                if (_stopRequested)
                {
                    _gui.SetText(ref _lblStatus, UIStrings.installAborted);
                    _gui.ShowProgressAnimation(false, false);
                    return null;
                }
                Thread.Sleep(50);
            }

            _gui.ShowProgressAnimation(false, false);

            return deviceId;
        }

        void GetDeviceId(DeviceIdCallback callback)
        {
            while (true)
            {
                var deviceChooser = new DeviceChooserDialog();
                dynamic result = deviceChooser.GetUserInput();
                if ((result == DialogResult.OK) | (result == null))
                    if (
                        MessageBox.Show(UIStrings.correctDevice1 + deviceChooser.Device + UIStrings.correctDevice2,
                            UIStrings.Verify_Device, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                        callback(deviceChooser.Device);
                    else
                        continue;
                else if (result == DialogResult.Cancel)
                    Abort();
                else
                    callback(deviceChooser.Device);
                //End Using
                break;
            }
        }

        public string[] GetFilesToInstall()
        {
            return _filesToInstall.ToArray();
        }

        public static bool ForceRemovePackage(string apkFile)
        {
            using (var adb = AndroidTools.RunAdb("uninstall " + AndroidTools.PackageName(apkFile), false, true, true))
            {
                return adb.ExitCode == 0;
            }
        }

        string InstallSinglePackage(string deviceId, string file)
        {
            const string abortKey = "ABORT";

            using (
                var adb =
                    AndroidTools.RunAdb($@"-s {deviceId} install {(Reinstall ? "-r " : "")}""{file}""",
                        true, true, false))
            {
                dynamic adbStandardOut = adb.StandardOutput;
                dynamic haltInstall = false;

                // Process aborts while the adb process is running
                // Check for and identify failures during the adb install
                dynamic currentLine = adbStandardOut.ReadLine();
                while (!adb.HasExited | (currentLine != null))
                {
                    // user requested stop
                    if (_stopRequested)
                    {
                        if (
                            MessageBox.Show(UIStrings.abortCurrentApk, UIStrings.APK_Install_Aborted, MessageBoxButtons.YesNo,
                                MessageBoxIcon.Question) == DialogResult.Yes)
                        {
                            try
                            {
                                if (adb.HasExited)
                                    adb.Kill();
                            }
                            catch (InvalidOperationException ex)
                            {
                            }
                            MessageBox.Show(UIStrings.installAborted, UIStrings.APK_Install_Aborted, MessageBoxButtons.OK,
                                MessageBoxIcon.Exclamation);
                            return abortKey;
                        }
                        haltInstall = true;
                        _stopRequested = false;
                    }


                    while ((currentLine != null) && !_stopRequested)
                    {
                        if (currentLine.StartsWith("Failure", StringComparison.OrdinalIgnoreCase))
                        {
                            dynamic startPos = currentLine.IndexOf("[", StringComparison.OrdinalIgnoreCase);
                            dynamic endPos = currentLine.IndexOf("]", StringComparison.OrdinalIgnoreCase);
                            dynamic errorMessage = "Unknown Failure";
                            if ((startPos < endPos) & (startPos != -1) & (endPos != -1))
                                errorMessage = currentLine.Substring(startPos + 1, endPos - startPos - 1);
                            return errorMessage;
                        }
                        currentLine = adbStandardOut.ReadLine();
                    }
                }

                // Display an abort message
                if (!haltInstall) return adb.ExitCode.ToString();

                MessageBox.Show(UIStrings.installAborted, UIStrings.installAborted, MessageBoxButtons.OK,
                    MessageBoxIcon.Exclamation);
                return abortKey;
            }
        }

        ErrorCode HandleInstallFailure(string file, int code)
        {
            return HandleInstallFailure(file, UIStrings.adbExitErrorCode + code);
        }

        ErrorCode HandleInstallFailure(string file, string message)
        {
            var result =
                MessageBox.Show(
                    UIStrings.unsuccessfulInstall1 + file + UIStrings.unsuccessfulInstall2 + "\n" + UIStrings.details +
                    message, "Install Failed", MessageBoxButtons.AbortRetryIgnore, MessageBoxIcon.Question);
            switch (result)
            {
                case DialogResult.Abort:
                    return ErrorCode.Abort;
                case DialogResult.Ignore:
                    // Don't retry
                    return ErrorCode.Ignore;
                default:
                    for (var second = 5; second >= 1; second += -1)
                    {
                        dynamic retryMessage = UIStrings.retryIn + second +
                                               (second == 1 ? UIStrings.second : UIStrings.seconds);
                        _gui.SetText(ref _gui.lblStatus, retryMessage);
                    }

                    return ErrorCode.Failure2;
            }
        }

        /// <summary>
        ///     Aborts the install
        /// </summary>
        public void Abort()
        {
            _stopRequested = true;
        }

        public void Reset()
        {
            _stopRequested = false;
        }

        delegate void DeviceIdCallback(string deviceId);
    }
}