# App Installer

A simple-to-use, robust tool for installing Android apps.

## Installing
1. Go to the [releases](https://github.com/dmssargent/AppInstaller/releases "Releases") page
2. Download the latest Setup.exe release (or prerelease, if you really want to)
3. Run the file, the program will automatically open when the installer is done

## Usage
### Simple
Obtain an APK file (or a few APK files) that you want to install.
Drag-and-drop the file(s) on to the program's window.
Make sure an Android device (in development mode with USB debugging enabled) is connected to the computer.
Click the Install button

### Batch
Obtain your APK files
Drag-and-mode your APK files on to the window (you can drag more than one file on to the window at once)
If you ever make a mistake, double-click on the files to install textbox to open a "Files To Install" window
 to change the files you are going to install. Click OK on the window when you are finished with the "Files to Install" window
Make sure an Android device (in development mode with USB debugging enabled) is connected to the computer.
Click Install

## Troubleshooting
### The program can't find your device?
The device needs to be in development mode, with USB debugging enabled and connected to your computer. If you are using a ADB TCP/IP bridge, try connecting the phone with a USB cable.

### The program is having trouble with ADB TCP/IP bridges
The ADB versions in use are most likely too different to use the TCP/IP bridge. However, if you add your platform-tools to the system path, or if you correctly configure the ```ANDROID_HOME``` enviroment variable to point to your Android SDK. The program will use the Android SDK's copy of ADB.

__Important:__ The program will not use any copy of ADB located under the Windows directory (```%windir%```), as that version is most likely out-of-date.

#### Bundled ADB version
```bash
$ adb version
Android Debug Bridge version 1.0.32
Revision 09a0d98bebce-android
```

## Building
### Requirements
* Visual Studio 2015+
* Git
* An internet connection (to download NuGet dependencies)

### Get a copy
* Clone the repo ```git clone https://github.com/dmssargent/AppInstaller```
* Download a [zip](https://github.com/dmssargent/AppInstaller/archive/master.zip) 

### Open the solution in Visual Studio
Open ```APKInstaller.sln``` in Visual Studio (Ctrl+O or File | Open > File)

### Build
Press ```Ctrl+Shift+B``` or Build | Build Solution

### Run
Press ```F5``` or Debug | Start Debugging


