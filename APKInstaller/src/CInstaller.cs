using System.Runtime.Remoting.Messaging;

public class CInstaller
{
    private delegate void deviceIdCallback(string deviceId);

    private readonly Main GUI;
    private readonly MaterialSkin.Controls.MaterialLabel lblStatus;
    private readonly MaterialSkin.Controls.MaterialSingleLineTextField txtUserInput;
    private boolean stopRequested = false;
    private boolean update = true;
    private boolean showCompletionMessage = true;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="GUI"></param>
    /// <param name="statusLabel"></param>
    /// <param name="userInputTextbox"></param>
    public CInstaller(Main GUI, ref MaterialLabel statusLabel, ref MaterialTextbox userInputTextbox)
    {
        this.GUI = GUI;
        this.lblStatus = statusLabel;
        this.txtUserInput = userInputTextbox;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="folderPath"></param>
    /// <returns></returns>
    private static Shell32.Folder GetShell32Folder(String folderPath)
    {
        Type shellAppType = Type.GetTypeFromProgID("Shell.Application");
        dynamic Shell = Activator.CreateInstance(shellAppType);
        return
            CType(
                shellAppType.InvokeMember("NameSpace", Reflection.BindingFlags.InvokeMethod, Nothing, Shell,
                    new Object[] { folderPath }), Shell32.Folder);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="adbLocation"></param>
    private void WaitForDevice(String adbLocation)
    {
        Process adbWait = new Process();
        adbWait.StartInfo.FileName = adbLocation;
        adbWait.StartInfo.Arguments = "wait-for-device";
        adbWait.StartInfo.UseShellExecute = False;
        adbWait.StartInfo.CreateNoWindow = True;
        GUI.SetText(lblStatus, "Waiting for device.");
        adbWait.Start();

        int caret = 0;
        while (!adbWait.HasExited)
        {
            GUI.UseWaitCursor = True;
            GUI.ShowProgressAnimation(True);
            if (caret == 0)
            {
                GUI.SetText(lblStatus, "Waiting for device.");
            }
            else if (caret = 1)
            {
                GUI.SetText(lblStatus, "Waiting for device..");
            }
            else
            {
                GUI.SetText(lblStatus, "Waiting for device...");
                caret = -1;
            }
            caret++;

            if (stopRequested)
            {
                MsgBox("The install has been aborted", CType(MsgBoxStyle.OkOnly + MsgBoxStyle.Exclamation, MsgBoxStyle),
                    "APK Install Aborted");
                return;
            }
        }
        GUI.ShowProgressAnimation(False);
        adbWait.Close();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    string GetAdbExecutable()
    {
        Process adb = new Process();
        adb.StartInfo.Arguments = "version";
        adb.StartInfo.FileName = "adb";
        adb.StartInfo.CreateNoWindow = true;
        adb.StartInfo.UseShellExecute = false;
        try
        {
            adb.Start();
            adb.WaitForExit();
            if (adb.ExitCode = 0)
                return "adb";
        }
        catch (Exception ex)
        {
        }

        // Fall-back to packaged version
        string tempFileName = Path.GetTempFileName();
        File.Delete(tempFileName);
        Directory.CreateDirectory(tempFileName);
        string platformToolsZip = Path.combine(tempFileName, "platform-tools.zip");
        File.WriteAllBytes(platformToolsZip, My.Resources.platform_tools_r23_1_0_windows);
        string androidPlatformTools = Path.combine(tempFileName, "platform-tools");
        UnZip(platformToolsZip, androidPlatformTools);

        return androidPlatformTools + Path.seperator + "platform-tools" + Path.seperator + "adb.exe";
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="zipName"></param>
    /// <param name="path"></param>
    private void UnZip(string zipName, string path)
    {
        // Create directory to unzip
        IO.Directory.CreateDirectory(path);
        Shell32.Folder input = GetShell32Folder(zipName);
        Shell32.Folder output = GetShell32Folder(path);
        // Extract the files from the zip file using the CopyHere command .
        output.CopyHere(input.Items, 4);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="callback"></param>
    private void GetDeviceId(deviceIdCallback callback)
    {
        DeviceChooserDialog deviceChooser = new DeviceChooserDialog();
        Dim result = deviceChooser.getUserInput(GetAdbExecutable);
        if (result = DialogResult.OK)
        {
            if (
                MsgBox("Is the device \"" + deviceChooser.getDevice + "\" correct?",
                    CType(MsgBoxStyle.YesNo + MsgBoxStyle.Question, MsgBoxStyle)) == MsgBoxResult.Yes)
                callback(deviceChooser.getDevice);
            else
                GetDeviceId(callback);
        }
        else if (result == DialogResult.Cancel)
            abort();
        else
            callback(deviceChooser.getDevice);

    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="files"></param>
    void AddFilesToInstall(string[] files)
    {
        string location = "";
        if (GetFilesToInstall.Length > 0 && GetFilesToInstall(0) != "")
        {
            if (
                MsgBox("There are other APK files to install. Do you want to keep them and add this to install?",
                    CType(MsgBoxStyle.Question + MsgBoxStyle.YesNo, MsgBoxStyle)) == MsgBoxResult.Yes)
                location = txtUserInput.Text + ";";
        }

        foreach (String path in files)
        {
            if (path == null)
                continue;

            if (path.ToLower.EndsWith(".apk"))
                location += path + ";";
            else
                MsgBox(
                    "\"" + path + "\"" + " is not a valid Android app. Please verify that the file ends with \".APK\"",
                    CType(MsgBoxStyle.OkOnly + MsgBoxStyle.Exclamation, MsgBoxStyle), "Invalid File");
        }

        if (location.EndsWith(";"))
            location = location.Substring(0, location.Length - 1);

        txtUserInput.Text = location;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    bool VerifyFilesToInstall()
    {
        foreach (string apkfile in GetFilesToInstall())
        {
            var exists = File.Exists(apkfile);
            if (!exists)
                return false;
        }

        return true;
    }

    /// <summary>
    /// 
    /// </summary>
    void StartInstall()
    {
        Thread thread = new Thread(new ThreadStart(Install));
        thread.Start();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="enabled"></param>
    void ConfigureReInstall(bool enabled)
    {
        update = enabled;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="show"></param>
    void ShowCompletionMessageWhenFinished(bool show)
    {
        showCompletionMessage = show;
    }

    /// <summary>
    /// 
    /// </summary>
    private void Install()
    {
        string adbLocation = GetAdbExecutable();

        // Wait until an Android device is connected
        WaitForDevice(adbLocation);

        // Device Found, check if multiple devices are connected, and figure out the device to install to

        GUI.SetText(lblStatus, "Checking device(s)...This may be a moment or two");
        GUI.ShowProgressAnimation(True);

        var deviceId = "";
        GetDeviceId(new deviceIdCallback()
        {
            deviceId = _DeviceId
        });

        while (deviceId == "")
        {
            if (stopRequested)
            {
                GUI.SetText(lblStatus, "The install did not complete successfully :(");
                GUI.ShowProgressAnimation(False);
                return;
            }

            Thread.Sleep(50);
        }
        GUI.ShowProgressAnimation(False);

        GUI.SetText(lblStatus, "Starting Installs...");
        string[] filesToInstall = GetFilesToInstall();
        string installStatus = "";
        var installAborted = false;
        foreach (var file in GetFilesToInstall())
        {
            while (true)
            {
                Process adb = new Process();
                string arguments = "-s " + deviceId + " install ";
                if (update)
                {
                    arguments += "-r ";
                }
                adb.StartInfo.Arguments = arguments + " " + file;
                adb.StartInfo.CreateNoWindow = true;
                adb.StartInfo.UseShellExecute = false;
                adb.StartInfo.FileName = adbLocation;
                adb.StartInfo.RedirectStandardOutput = true;
                installStatus += adb.StartInfo.FileName + " " + adb.StartInfo.Arguments;
                installStatus += vbCrLf;
                GUI.SetText(GUI.lblStatus, installStatus);
                adb.Start();
                while (!adb.HasExited)
                {
                    if (stopRequested)
                    {
                        MsgBox("The install has been aborted!",
                            CType(MsgBoxStyle.OkOnly + MsgBoxStyle.Exclamation, MsgBoxStyle),
                            "APK Install Aborted");
                        return;
                    }
                }
            }
        }

        GUI.UseWaitCursor = false;
        if (installAborted)
        {
            GUI.SetText(lblStatus, "Failure! Details: " & vbCrLf & installStatus);
        }
        else
        {
            GUI.SetText(lblStatus, "Done!");
            if (showCompletionMessage)
            {
                MsgBox("The installation of the APK(s) has finished successfully!",
                    CType(MsgBoxStyle.Information + MsgBoxStyle.OkOnly, MsgBoxStyle), "APK Install Finished");
            }
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    string[] GetFilesToInstall()
    {
        return txtUserInput.Text.Split(";");
    }

    /// <summary>
    /// 
    /// </summary>
    void abort()
    {
        stopRequested = true;
    }

}
