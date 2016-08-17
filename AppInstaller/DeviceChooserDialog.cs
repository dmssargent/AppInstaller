using System;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Threading;
using System.Windows.Forms;
//using Microsoft.VisualBasic;

//using MaterialSkin;

namespace APKInstaller
{
    /// <summary>
    ///     Provides a list of devices if more than one Android phone is connected
    /// </summary>
    public partial class DeviceChooserDialog
    {
        private Thread _autoUpdateThread;

        private int _deviceIndex = -1;

        public DeviceChooserDialog()
        {
            InitializeComponent();
            Closed += dlgDeviceChoose_Closed;
            Closing += dlgDeviceChoose_Closing;
            Load += DeviceChooser_Load;
        }

        /// <summary>
        ///     The selected device
        /// </summary>
        /// <returns>the string identifier of a Android device</returns>
        public string Device
        {
            get
            {
                if (IsReady())
                {
                    return CleanupOutput(lstDevices.SelectedItem.ToString());
                }
                return null;
            }
        }

        private void UpdateDeviceList()
        {
            using (var adb = AndroidTools.RunAdb("devices", true, true, true))
            {
                dynamic numberOfDevicesLastFound = lstDevices.Items.Count;
                dynamic data = adb.StandardOutput.ReadLine();
                dynamic numberOfDevices = 0;
                dynamic parserInDeviceList = false;
                while (data != null)
                {
                    data = data.Trim();
                    if (parserInDeviceList)
                    {
                        if (object.ReferenceEquals(data, ""))
                        {
                            data = adb.StandardOutput.ReadLine();
                            continue;
                        }
                        dynamic details = "";
                        dynamic rcVersion = CheckForFtcRobotController(CleanupOutput(data));
                        dynamic dsVersion = CheckForFtcDriverStation(CleanupOutput(data));
                        if (rcVersion > 0)
                        {
                            details += "[FTC Robot Controller " + rcVersion + "]";
                        }

                        if (dsVersion > 0)
                        {
                            details = string.IsNullOrEmpty(details)
                                ? "["
                                : "; " + " FTC Driver Station " + dsVersion + "]";
                        }

                        UpdateDeviceListEntry(numberOfDevices, CleanupOutput(data) + "\t" + details);
                        numberOfDevices += 1;
                    }

                    if (data.Trim() == "List of devices attached")
                    {
                        parserInDeviceList = true;
                    }

                    data = adb.StandardOutput.ReadLine();
                }


                for (int i = numberOfDevices; i <= numberOfDevicesLastFound - 1; i++)
                {
                    UpdateDeviceListEntry(i, null);
                }

                if (numberOfDevices == 1)
                {
                    if (!InvokeRequired)
                    {
                        lstDevices.SelectedIndex = 0;
                    }

                    SetVisibilityForNoPromptSingleDevice(true);
                }
                else
                {
                    SetVisibilityForNoPromptSingleDevice(false);
                }
            }
        }


        /// <summary>
        ///     Gets the user input if necessary, otherwise provides the only attached device to be used
        /// </summary>
        /// <returns>Nothing if no user input is needed, otherwise a DialogResult is returned from the GUI</returns>
        [SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate")]
        public DialogResult? GetUserInput()
        {
            if (InvokeRequired)
            {
                return (DialogResult) Invoke(new UserInputCallback(GetUserInput));
            }
            UpdateDeviceList();
            if (IsReady() /*& My.Settings.noPromptSingleDevice*/)
            {
                return null;
            }
            return ShowDialog();
        }

        /// <summary>
        ///     If the device chooser dialog has finished
        /// </summary>
        /// <returns>true if a device has been selected, false otherwise</returns>
        public bool IsReady()
        {
            return _deviceIndex >= 0;
        }

        private void DeviceChooser_Load(object sender, EventArgs e)
        {
            _autoUpdateThread = new Thread(() =>
            {
                do
                {
                    UpdateDeviceList();
                    try
                    {
                        Thread.Sleep(1000);
                    }
                    catch (ThreadInterruptedException ex)
                    {
                        break; // TODO: might not be correct. Was : Exit Do
                    }
                } while (Visible);
            });
            _autoUpdateThread.Start();
            // todo: chkNoPromptSingleDevice.Checked = My.Settings.noPromptSingleDevice;
            //lstDevices.Font = MaterialSkinManager.Instance.ROBOTO_MEDIUM_12
            //lblDevices.Font = MaterialSkinManager.Instance.ROBOTO_MEDIUM_12
            lstDevices.DrawMode = DrawMode.OwnerDrawFixed;

            Focus();
            CenterToScreen();
        }

        private void UpdateDeviceListEntry(int index, string deviceName)
        {
            if (lstDevices.InvokeRequired)
            {
                try
                {
                    if (IsDisposed)
                    {
                        return;
                    }
                    Invoke(new DeviceListUpdateCallback(UpdateDeviceListEntry), index, deviceName);
                }
                catch (ObjectDisposedException ex)
                {
                }
            }
            else
            {
                dynamic oldIndex = lstDevices.SelectedIndex;

                if (lstDevices.Items.Count > index)
                {
                    lstDevices.Items.RemoveAt(index);
                }

                if (deviceName != null)
                {
                    lstDevices.Items.Insert(index, deviceName);
                }

                if (oldIndex >= lstDevices.Items.Count) return;
                if (oldIndex >= 0)
                {
                    lstDevices.SelectedIndex = oldIndex;
                }
            }
        }

        private void SetVisibilityForNoPromptSingleDevice(bool isVisible)
        {
            if (chkNoPromptSingleDevice.InvokeRequired)
            {
                Invoke(new ChkNoPromptSingleDeviceVisibleCallback(SetVisibilityForNoPromptSingleDevice), isVisible);
            }
            else
            {
                chkNoPromptSingleDevice.Visible = isVisible;
            }
        }

        private static string CleanupOutput(string inputValue)
        {
            if (inputValue.Contains(" "))
            {
                inputValue = inputValue.Substring(0, inputValue.IndexOf(" ", StringComparison.CurrentCulture));
            }

            if (inputValue.Contains("\t"))
            {
                inputValue = inputValue.Substring(0,
                    inputValue.IndexOf("\t", StringComparison.CurrentCulture));
            }

            return inputValue;
        }


        /// <summary>
        ///     Checks the given device for the FTC Robot Controller
        /// </summary>
        /// <param name="device">adb device specifier</param>
        /// <returns>0 if no FTC Robot Controller can be found or the version name</returns>
        [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Ftc")]
        public static double CheckForFtcRobotController(string device)
        {
            return CheckPackageVersion(device, "com.qualcomm.ftcrobotcontroller");
        }

        /// <summary>
        ///     Checks the given device for the FTC Robot Controller
        /// </summary>
        /// <param name="device">adb device specifier</param>
        /// <returns>0 if no FTC Driver Station can be found or the version name</returns>
        public static double CheckForFtcDriverStation(string device)
        {
            return CheckPackageVersion(device, "com.qualcomm.ftcdriverstation");
        }

        /// <summary>
        ///     Checks the package version of a given app installed on a specified device
        /// </summary>
        /// <param name="device">adb device specifier</param>
        /// <param name="package">package name of the app</param>
        /// <returns>0 upon error, or the package version</returns>
        public static double CheckPackageVersion(string device, string package)
        {
            using (
                var adb = AndroidTools.RunAdb("-s " + device + " shell pm dump \"" + package + "\"", true, true, false))
            {
                const string versionName = "versionName=";
                double version = -1;
                var inSection = false;
                var line = adb.StandardOutput.ReadLine();
                try
                {
                    while (line != null)
                    {
                        //Detect interrupts
                        try
                        {
                            line = line.Trim();
                            if (adb.HasExited)
                            {
                                if (adb.ExitCode != 0)
                                {
                                    return -2;
                                }
                            }
                        }
                        catch (ThreadInterruptedException ex)
                        {
                            return version;
                        }


                        if (inSection & line.Contains(versionName))
                        {
                            dynamic versionTemp =
                                line.Substring(line.IndexOf(versionName, StringComparison.Ordinal) + versionName.Length);
                            version = double.Parse(versionTemp, CultureInfo.InvariantCulture);
                            break; // TODO: might not be correct. Was : Exit While
                        }
                        if (line.Contains(package) & line.Contains("Package "))
                        {
                            inSection = true;
                        }
                        line = adb.StandardOutput.ReadLine();
                    }
                }
                catch (IOException ex)
                {
                    return version;
                }

                return version;
            }
        }

        private void OK_Button_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
            Close();
        }

        private void Cancel_Button_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void lstDevices_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lstDevices.SelectedIndices.Count > 0)
            {
                OK_Button.Enabled = true;
                _deviceIndex = lstDevices.SelectedIndex;
            }
            else
            {
                OK_Button.Enabled = false;
            }
        }

        private void listBox1_DrawItem(object sender, DrawItemEventArgs e)
        {
            e.DrawBackground();
            if ((e.State & DrawItemState.Selected) == DrawItemState.Selected)
            {
                e.Graphics.FillRectangle(Brushes.Aqua, e.Bounds);
            }
            using (var b = new SolidBrush(e.ForeColor))
            {
                if (e.Index >= 0)
                {
                    e.Graphics.DrawString(lstDevices.GetItemText(lstDevices.Items[e.Index]), e.Font, b, e.Bounds);
                }
            }
            e.DrawFocusRectangle();
        }

        private void dlgDeviceChoose_Closing(object sender, CancelEventArgs e)
        {
            _autoUpdateThread.Interrupt();
        }

        private void dlgDeviceChoose_Closed(object sender, EventArgs e)
        {
            if (DialogResult == DialogResult.None | false)
            {
                DialogResult = DialogResult.Cancel;
            }
        }

        private void chkNoPromptSingleDevice_CheckedChanged(object sender, EventArgs e)
        {
            // todo: My.Settings.noPromptSingleDevice = chkNoPromptSingleDevice.Checked;
        }

        private delegate DialogResult? UserInputCallback();

        private delegate void DeviceListUpdateCallback(int index, string device);

        private delegate void ChkNoPromptSingleDeviceVisibleCallback(bool visible);

        //private void InitializeComponent()
        //{
        //    this.SuspendLayout();
        //    // 
        //    // DeviceChooserDialog
        //    // 
        //    this.ClientSize = new System.Drawing.Size(423, 316);
        //    this.Name = "DeviceChooserDialog";
        //    this.ResumeLayout(false);

        //}
    }
}
