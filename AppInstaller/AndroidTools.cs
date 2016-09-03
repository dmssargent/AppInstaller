using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using APKInstaller.i18n;

namespace APKInstaller
{
    /// <summary>
    /// Provided utilities for operating on Android devices, such as adb and aapt
    /// </summary>
    public class AndroidTools
    {
        static string _aaptCache;

        static string _adbCache;

        AndroidTools()
        {
            SetupIfPossible();
        }

        /// <summary>
        /// Returns the location of adb to be executed, if adb is in the system path the function may return the location of adb as
        /// "adb" This automatically setups up adb if adb can't be found
        /// </summary>
        /// <returns>the location of adb or "adb"</returns>
        public static string Adb
        {
            get
            {
                if (_adbCache != null & _adbCache == "adb" | File.Exists(_adbCache))
                {
                    return _adbCache;
                }

                try
                {
                    return SetupAdb();
                }
                catch (Exception ex)
                {
                    throw new InvalidOperationException("Failed to configure ADB on-demand", ex);
                }
            }
        }

        /// <summary>
        /// Returns the location of aapt; this function may setup aapt if necessary
        /// </summary>
        /// <returns>location of aapt</returns>
        [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Aapt")]
        public static string Aapt
        {
            get
            {
                if ((_aaptCache != null))
                {
                    if (File.Exists(_aaptCache))
                    {
                        return _aaptCache;
                    }
                }

                try
                {
                    return SetupAapt();
                }
                catch (Exception ex)
                {
                    throw new InvalidOperationException("Failed to configure AAPT on-demand", ex);
                }
            }
        }

        /// <summary>
        /// Sets up both ADB and AAPT for later use
        /// </summary>
        //[Log("App Installer Debug")]
        public static void SetupIfPossible()
        {
            if (!File.Exists(_aaptCache))
            {
                _aaptCache = null;
            }

            if (_adbCache != "adb" & !File.Exists(_adbCache))
            {
                _adbCache = null;
            }

            if ((_adbCache == null))
            {
                SetupAdb();
            }
            if (_aaptCache == null)
            {
                SetupAapt();
            }
        }

        // [Log("App Installer Debug")]
        static string SetupAdb()
        {
            dynamic windir = Environment.GetEnvironmentVariable("windir");

            if (windir == null || !(File.Exists(Path.Combine(windir, "adb.exe")) | File.Exists(Path.Combine(windir, "system32", "adb.exe"))))
            {
                using (var process = new Process())
                {
                    process.StartInfo.Arguments = "version";
                    process.StartInfo.FileName = "adb";
                    process.StartInfo.CreateNoWindow = true;
                    process.StartInfo.UseShellExecute = false;
                    try
                    {
                        process.Start();
                        process.WaitForExit();

                        if (process.ExitCode == 0)
                        {
                            _adbCache = "adb";
                            return "adb";
                        }
                    }
                    catch (Exception ex)
                    {
                        // Continue on
                    }
                }
            }

            dynamic androidHome = Environment.GetEnvironmentVariable("ANDROID_HOME");
            if (androidHome == null)
            {
                androidHome = MostLikelyAndroidSdk(Environment.GetEnvironmentVariable("PATH"));
                if (androidHome != null)
                {
                    if (
                        MessageBox.Show(
                            UIStrings.invalidAndroidSdkConfig + "\n" + UIStrings.correctConfigIssue +
                            "\n" + UIStrings.details +
                            "ANDROID_HOME is not defined, when valid Android SDK is present",
                            "Invalid SDK", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.Yes)
                    {
                        Environment.SetEnvironmentVariable("ANDROID_HOME", androidHome);
                        // Change the environment variable for this process as well as the user
                        Environment.SetEnvironmentVariable("ANDROID_HOME", androidHome, EnvironmentVariableTarget.User);
                    }
                }
            }
            else if (!IsAndroidSdk(androidHome))
            {
                dynamic androidHome2 = MostLikelyAndroidSdk(Environment.GetEnvironmentVariable("PATH"));
                if (androidHome2 != null)
                {
                    if (androidHome != androidHome2)
                    {
                        if (
                            MessageBox.Show(
                                UIStrings.invalidAndroidSdkConfig + "\n" + UIStrings.invalidAndroidSdkConfig,
                                "Invalid SDK", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.Yes)
                        {
                            Environment.SetEnvironmentVariable("ANDROID_HOME", androidHome2);
                            // Change the environment variable for this process as well as the user
                            Environment.SetEnvironmentVariable("ANDROID_HOME", androidHome2,
                                EnvironmentVariableTarget.User);
                        }
                    }
                }
            }

            if (androidHome != null)
            {
                dynamic adbLocation = Path.Combine(androidHome, "platform-tools", "adb");
                using (var process = new Process())
                {
                    process.StartInfo.Arguments = "version";
                    process.StartInfo.FileName = adbLocation;
                    process.StartInfo.CreateNoWindow = true;
                    process.StartInfo.UseShellExecute = false;
                    try
                    {
                        process.Start();
                        process.WaitForExit();
                        if ((process.ExitCode == 0))
                        {
                            _adbCache = adbLocation;
                            return adbLocation;
                        }
                    }
                    catch (Exception ex)
                    {
                        // Continue on
                    }
                }
            }

            // Fall-back to app version
            dynamic tempFileName = IoUtilities.CreateTempSession("adb");
            string platformToolsZip = tempFileName + Path.DirectorySeparatorChar + "platform-tools.zip";

            var env = (int)Environment.OSVersion.Platform;
            string androidPlatformTools = tempFileName + Path.DirectorySeparatorChar + "platform-tools";
            if ( (int)Environment.OSVersion.Platform == 128)
            {
                
                File.WriteAllBytes(platformToolsZip, Resources.platform_tools_r24_linux);
                _adbCache = androidPlatformTools + Path.DirectorySeparatorChar + "platform-tools" + Path.DirectorySeparatorChar + "adb";
            }
            else if (Environment.OSVersion.Platform == PlatformID.Win32Windows || Environment.OSVersion.Platform == PlatformID.Win32NT)
            {

                File.WriteAllBytes(platformToolsZip, Resources.platform_tools_r24_windows);
                _adbCache = androidPlatformTools + Path.DirectorySeparatorChar + "platform-tools" + Path.DirectorySeparatorChar + "adb.exe";
            }
            else if (Environment.OSVersion.Platform == PlatformID.Unix || env == 6)
            {
                File.WriteAllBytes(platformToolsZip, Resources.platform_tools_r24_mac);
                _adbCache = androidPlatformTools + Path.DirectorySeparatorChar + "platform-tools" + Path.DirectorySeparatorChar + "adb";
            }
            else
            {
                MessageBox.Show("Your platform is not supported just yet. You need to get the full Android SDK and point the enviroment variable \"ANDROID_HOME\" to it.",
                    "Unsupported Platform", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            //string androidPlatformTools = tempFileName + Path.DirectorySeparatorChar + "platform-tools";
            using (var stream = new FileStream(platformToolsZip, FileMode.Open))
            {
                IoUtilities.UnzipFromStream(stream, androidPlatformTools);

            }
            //_adbCache = androidPlatformTools + Path.DirectorySeparatorChar + "platform-tools" + Path.DirectorySeparatorChar + "adb.exe";
            return _adbCache;
        }

        //[SuppressMessage("Microsoft.Naming", "CA2204:Literals should be spelled correctly", MessageId = "aapt")]
        //[Log("App Installer Debug")]
        static string SetupAapt()
        {
            dynamic aaptPath = "";

            try
            {
                dynamic temp = IoUtilities.CreateTempSession("aapt");

                if ((int)Environment.OSVersion.Platform == 128)
                {
                    var aaptZip = Path.Combine(temp, "aapt.zip");
                    File.WriteAllBytes(aaptZip, Resources.aapt_r24_01_linux);
                    var aaptDirPath = Path.Combine(temp, "aapt");
                    IoUtilities.UnzipFromFile(aaptZip, aaptDirPath);
                    aaptPath = Path.Combine(aaptDirPath, "aapt", "aapt");
                }
                else if (Environment.OSVersion.Platform == PlatformID.Win32Windows || Environment.OSVersion.Platform == PlatformID.Win32NT)
                {
                    aaptPath = Path.Combine(temp, "aapt.exe");
                    File.WriteAllBytes(aaptPath, Resources.aapt_23_0_3_win);
                }
                else if (Environment.OSVersion.Platform == PlatformID.Unix || (int)Environment.OSVersion.Platform == 6) // OS X
                {
                    var aaptZip = Path.Combine(temp, "aapt.zip");
                    File.WriteAllBytes(aaptZip, Resources.aapt_r24_02_mac);
                    var aaptDirPath = Path.Combine(temp, "aapt");
                    IoUtilities.UnzipFromFile(aaptZip, aaptDirPath);
                    aaptPath = Path.Combine(aaptDirPath, "aapt");
                }
                else
                {
                    MessageBox.Show("Your platform is not supported just yet. You need to get the full Android SDK and point the enviroment variable \"ANDROID_HOME\" to it.",
                        "Unsupported Platform", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }

            }
            catch (Exception ex)
            {
				
            }

            //File.WriteAllBytes(aaptPath, Resources.aapt_23_0_3_win);
            if (File.Exists(aaptPath))
            {
#if __MONOCS__
                if (Type.GetType("Mono.Unix.Native")) {
                    Mono.Unix.Native.Syscall.chmod(aaptPath, Mono.Unix.Native.FilePermissions.S_IRWXU);
                }
#endif
                _aaptCache = aaptPath;
                return aaptPath;
            }
            else
            {
                throw new IOException("Failed to build aapt.exe");
            }
        }


        /// <summary>
        /// Gets an instance of an "adb" process to customize and use. Callers should use this in a using block or dispose of the generate
        /// process
        /// 
        /// Example:
        /// Using adb = RunAapt("version", True, True, True)
        ///     ' Your code here
        /// End Using
        /// </summary>
        /// <param name="args">adb arguments</param>
        /// <param name="redirectStdOut">whether or not to redirect the standard output stream</param>
        /// <param name="run">run this program before returning</param>
        /// <param name="waitToReturn">wait for this program to return, run parameter must be true</param>
        /// <returns>an adb process</returns>
        [SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope")]
        //[Log("App Installer Debug")]
        public static Process RunAdb(string args, bool redirectStdOut, bool run, bool waitToReturn)
        {
            SetupIfPossible();
            var pAdb = new Process();
            pAdb.StartInfo.FileName = Adb;
            pAdb.StartInfo.Arguments = args;
            pAdb.StartInfo.CreateNoWindow = true;
            pAdb.StartInfo.UseShellExecute = false;
            pAdb.StartInfo.RedirectStandardOutput = redirectStdOut;

            if (!run) return pAdb;
            pAdb.Start();
            if (waitToReturn)
            {
                pAdb.WaitForExit();
            }

            return pAdb;
        }

        /// <summary>
        /// Gets an instance of an "aapt" process to customize and use. Callers should use this in a using block or dispose of the generate
        /// process
        /// 
        /// Example:
        /// Using aapt = RunAapt("-help", True)
        ///     aapt.Start()
        ///     aapt.WaitForExit()
        /// End Using
        /// </summary>
        /// <param name="args">aapt arguments</param>
        /// <param name="redirectStdOut">true if the standard output stream should be redirected, otherwise false</param>
        /// <returns>an instance of an aapt process, not started</returns>
        // [Log("App Installer Debug")]
        internal static Process RunAapt(string args, bool redirectStdOut)
        {
            SetupIfPossible();
            var pAapt = new Process
            {
                StartInfo =
                {
                    FileName = Aapt,
                    Arguments = args,
                    CreateNoWindow = true,
                    UseShellExecute = false,
                    RedirectStandardOutput = redirectStdOut
                }
            };

            
            return pAapt;
        }

        /// <summary>
        /// Returns the package name of a given APK file
        /// </summary>
        /// <param name="apkFile">the location of the APK file</param>
        /// <returns>The package name of the APK file, or nothing on failure</returns>
        //[Log("App Installer Debug")]
        public static string PackageName(string apkFile)
        {
            if (apkFile == null)
            {
                throw new ArgumentNullException(nameof(apkFile));
            }

            using (var process = RunAapt("dump badging \"" + apkFile + "\"", true))
            {
                process.Start();

                dynamic parseFor = "package: name=";
                string package = null;
                try
                {
                    var line = process.StandardOutput.ReadLine();
                    while (line != null)
                    {
                        //Detect interrupts
                        Thread.Sleep(1);
                        line = line.Trim();
                        if (line.Contains(parseFor))
                        {
                            var versionName =
                                line.Substring(line.IndexOf(parseFor, StringComparison.Ordinal) + parseFor.Length);
                            package = versionName.Substring(versionName.IndexOf("'", StringComparison.Ordinal) + 1);
                            package = package.Substring(0, package.IndexOf("'", StringComparison.Ordinal));
                            break; // TODO: might not be correct. Was : Exit While
                        }
                        var readLine = process.StandardOutput.ReadLine();
                        if (readLine != null) line = readLine.Trim();
                    }

                    if (process.HasExited)
                    {
                        if (process.ExitCode != 0)
                        {
                            throw new IOException("AAPT Failed. Exit: " + process.ExitCode);
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw new IOException("Failed to acquire package name", ex);
                }
                return package;
            }
        }

        //[Log("App Installer Debug")]
        public static bool IsAndroidSdk(string path)
        {
            dynamic platformTools = Path.Combine(path, "platform-tools");
            dynamic tools = Path.Combine(path, "tools");

            if (!(Directory.Exists(tools) & Directory.Exists(platformTools))) return false;

            dynamic adbPath = Path.Combine(platformTools, "adb.exe");
            dynamic androidBat = Path.Combine(tools, "android.bat");
            if (Environment.OSVersion.Platform == PlatformID.Unix || (int) Environment.OSVersion.Platform == 128) {
                adbPath = Path.Combine(platformTools, "adb");
                androidBat = Path.Combine(tools, "android.bat");
            } else if (Environment.OSVersion.Platform == PlatformID.Win32Windows || Environment.OSVersion.Platform == PlatformID.Win32NT) {
                adbPath = Path.Combine(platformTools, "adb");
                androidBat = Path.Combine(tools, "android");
            } else {
                MessageBox.Show ("Your platform is not supported just yet. You need to get the full Android SDK and point the enviroment variable \"ANDROID_HOME\" to it.",
                    "Unsupported Platform", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            return File.Exists(adbPath) & File.Exists(androidBat);
        }

        /// <summary>
        /// Returns the most likely Android SDK out of a set of locations, or a single location. A set of locations is delimited by the IO.Path.PathSeperator
        /// character. The parents of the given path(s) are also checked.
        /// </summary>
        /// <param name="path">a delimited list of paths, or a single path location</param>
        /// <returns>Nothing if none of the paths result in an usable Android SDK, or the path of an Android SDK</returns>
        //[Log("App Installer Debug")]
        public static string MostLikelyAndroidSdk(string path)
        {
            if (path == null)
            {
                throw new ArgumentNullException(nameof(path));
            }
            return MostLikelyAndroidSdk(path.Split(Path.PathSeparator.ToString().ToArray()));
        }

        /// <summary>
        /// Returns the most likely Android SDK out of a set of locations, this function checks both the given location and the parents of the given location
        /// for the Android SDK location
        /// </summary>
        /// <param name="paths">the locations in a string format</param>
        /// <returns>Nothing if none of the paths result in an usable Android SDK, or the path of an Android SDK</returns>
        //[Log("App Installer Debug")]
        public static string MostLikelyAndroidSdk(string[] paths)
        {
            if (paths == null)
            {
                throw new ArgumentNullException(nameof(paths));
            }

            var possibleSdkPaths = new List<string>();
            string[] androidSdkPaths =
            {
                "add-ons",
                "build-tools",
                "docs",
                "extras",
                "licenses",
                "ndk-bundle",
                "platforms",
                "platform-tools",
                "samples",
                "skins",
                "sources",
                "system-images",
                "temp",
                "tools"
            };
            foreach (var path in paths)
            {
                //path = path_loopVariable;
                if (!ReferenceEquals(path, "") & IsAndroidSdk(path))
                {
                    possibleSdkPaths.Add(path);
                }
                else
                {
                    foreach (var sdkPath in androidSdkPaths)
                    {
                        //sdkPath = sdkPath_loopVariable;
                        dynamic value = Path.DirectorySeparatorChar + sdkPath;
                        if (path.Contains(value))
                        {
                            dynamic path2 = path.Substring(0, path.IndexOf(value, StringComparison.OrdinalIgnoreCase));
                            if (IsAndroidSdk(path2))
                            {
                                possibleSdkPaths.Add(path2);
                            }
                            break; // TODO: might not be correct. Was : Exit For
                        }
                    }
                }
            }

            dynamic highScore = 0;
            string highScoreLocation = null;

            foreach (var path in possibleSdkPaths)
            {
                
                dynamic currentScore =
                    androidSdkPaths.Count(sdkPath => Directory.Exists(Path.Combine(path, sdkPath)));

                if (currentScore <= highScore) continue;
                highScore = currentScore;
                highScoreLocation = path;
            }

            return highScoreLocation;
        }
    }
}