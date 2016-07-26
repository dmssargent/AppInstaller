Imports System.ComponentModel
Imports System.Diagnostics.CodeAnalysis
Imports System.Globalization
Imports System.IO
Imports System.Threading
Imports MaterialSkin

''' <summary>
''' Provides a list of devices if more than one Android phone is connected
''' </summary>
Public Class DeviceChooserDialog
    Private Delegate Function UserInputCallback() As DialogResult
    Private Delegate Sub DeviceListUpdateCallback(ByVal index As Integer, ByVal device As String)
    Private Delegate Sub ChkNoPromptSingleDeviceVisibleCallback(visible As Boolean)
    Private _autoUpdateThread As Thread

    Private _deviceIndex As Integer = -1

    ''' <summary>
    ''' The selected device
    ''' </summary>
    ''' <returns>the string identifier of a Android device</returns>
    Public ReadOnly Property Device As String
        Get
            If IsReady() Then
                Return CleanupOutput(lstDevices.SelectedItem.ToString)
            End If
            Return Nothing
        End Get
    End Property

    Private Sub UpdateDeviceList()
        Using adb = AndroidTools.RunAdb("devices", True, True, True)

            Dim numberOfDevicesLastFound = lstDevices.Items.Count
            Dim data = adb.StandardOutput.ReadLine
            Dim numberOfDevices = 0
            Dim parserInDeviceList = False
            While (data IsNot Nothing)
                data = data.Trim()
                If parserInDeviceList Then
                    If data Is "" Then
                        data = adb.StandardOutput.ReadLine
                        Continue While
                    End If
                    Dim details = ""
                    Dim rcVersion = CheckForFtcRobotController(CleanupOutput(data))
                    Dim dsVersion = CheckForFtcDriverStation(CleanupOutput(data))
                    If rcVersion > 0 Then
                        details &= "[FTC Robot Controller " & rcVersion & "]"
                    End If

                    If dsVersion > 0 Then
                        details = If(details = "", "[", "; ") & " FTC Driver Station " & dsVersion & "]"
                    End If

                    UpdateDeviceListEntry(numberOfDevices, CleanupOutput(data) & vbTab & details)
                    numberOfDevices += 1
                End If

                If (data.Trim = "List of devices attached") Then
                    parserInDeviceList = True
                End If

                data = adb.StandardOutput.ReadLine
            End While



            For i As Integer = numberOfDevices To numberOfDevicesLastFound - 1
                UpdateDeviceListEntry(i, Nothing)
            Next i

            If numberOfDevices = 1 Then
                If Not InvokeRequired Then
                    lstDevices.SelectedIndex = 0
                End If

                SetVisibilityForNoPromptSingleDevice(True)
            Else
                SetVisibilityForNoPromptSingleDevice(False)
            End If
        End Using
    End Sub


    ''' <summary>
    ''' Gets the user input if necessary, otherwise provides the only attached device to be used
    ''' </summary>
    ''' <returns>Nothing if no user input is needed, otherwise a DialogResult is returned from the GUI</returns>
    <SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate")>
    Public Function GetUserInput() As DialogResult
        If InvokeRequired Then
            Return CType(Invoke(New UserInputCallback(AddressOf GetUserInput)), DialogResult)
        Else

            UpdateDeviceList()
            If IsReady() And My.Settings.noPromptSingleDevice Then
                Return Nothing
            End If
            Return ShowDialog()
        End If
    End Function

    ''' <summary>
    ''' If the device chooser dialog has finished
    ''' </summary>
    ''' <returns>true if a device has been selected, false otherwise</returns>
    Public Function IsReady() As Boolean
        Return _deviceIndex >= 0
    End Function

    Private Sub DeviceChooser_Load(sender As Object, e As EventArgs) Handles Me.Load
        _autoUpdateThread = New Thread(
            New ThreadStart(Sub()
                                Do
                                    UpdateDeviceList()
                                    Try
                                        Thread.Sleep(1000)
                                    Catch ex As ThreadInterruptedException
                                        Exit Do
                                    End Try
                                Loop While Visible
                            End Sub))
        _autoUpdateThread.Start()
        chkNoPromptSingleDevice.Checked = My.Settings.noPromptSingleDevice
        lstDevices.Font = MaterialSkinManager.Instance.ROBOTO_MEDIUM_12
        lblDevices.Font = MaterialSkinManager.Instance.ROBOTO_MEDIUM_12
        lstDevices.DrawMode = DrawMode.OwnerDrawFixed

        Focus()
        CenterToScreen()
    End Sub

    Private Sub UpdateDeviceListEntry(ByVal index As Integer, deviceName As String)
        If lstDevices.InvokeRequired Then
            Try
                If IsDisposed Then
                    Return
                End If
                Invoke(New DeviceListUpdateCallback(AddressOf UpdateDeviceListEntry), New Object() {index, deviceName})
            Catch ex As ObjectDisposedException
                Return
            End Try

        Else

            Dim oldIndex = lstDevices.SelectedIndex

            If lstDevices.Items.Count > index Then
                lstDevices.Items.RemoveAt(index)
            End If

            If deviceName IsNot Nothing Then
                lstDevices.Items.Insert(index, deviceName)
            End If

            If oldIndex < lstDevices.Items.Count Then
                If oldIndex >= 0 Then
                    lstDevices.SelectedIndex = oldIndex
                End If
            End If
        End If
    End Sub

    Private Sub SetVisibilityForNoPromptSingleDevice(isVisible As Boolean)
        If chkNoPromptSingleDevice.InvokeRequired Then
            Invoke(New ChkNoPromptSingleDeviceVisibleCallback(AddressOf SetVisibilityForNoPromptSingleDevice), isVisible)
        Else
            chkNoPromptSingleDevice.Visible = isVisible
        End If
    End Sub

    Private Shared Function CleanupOutput(ByVal inputValue As String) As String
        If inputValue.Contains(" ") Then
            inputValue = inputValue.Substring(0, inputValue.IndexOf(" ", StringComparison.CurrentCulture))
        End If

        If inputValue.Contains(vbTab) Then
            inputValue = inputValue.Substring(0, inputValue.IndexOf(vbTab, StringComparison.CurrentCulture))
        End If

        Return inputValue
    End Function



    ''' <summary>
    ''' Checks the given device for the FTC Robot Controller
    ''' </summary>
    ''' <param name="device">adb device specifier</param>
    ''' <returns>0 if no FTC Robot Controller can be found or the version name</returns>
    <SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId:="Ftc")>
    Public Shared Function CheckForFtcRobotController(ByVal device As String) As Double
        Return CheckPackageVersion(device, "com.qualcomm.ftcrobotcontroller")
    End Function

    ''' <summary>
    ''' Checks the given device for the FTC Robot Controller
    ''' </summary>
    ''' <param name="device">adb device specifier</param>
    ''' <returns>0 if no FTC Driver Station can be found or the version name</returns>
    Public Shared Function CheckForFtcDriverStation(ByVal device As String) As Double
        Return CheckPackageVersion(device, "com.qualcomm.ftcdriverstation")
    End Function

    ''' <summary>
    ''' Checks the package version of a given app installed on a specified device
    ''' </summary>
    ''' <param name="device">adb device specifier</param>
    ''' <param name="package">package name of the app</param>
    ''' <returns>0 upon error, or the package version</returns>
    Public Shared Function CheckPackageVersion(ByVal device As String, ByVal package As String) As Double
        Using adb = AndroidTools.RunAdb("-s " & device & " shell pm dump """ & package & """", True, True, False)

            Const versionName = "versionName="
            Dim version As Double = -1
            Dim inSection As Boolean = False
            Dim line As String = adb.StandardOutput.ReadLine
            Try
                While line IsNot Nothing
                    'Detect interrupts
                    Try
                        line = line.Trim
                        If adb.HasExited Then
                            If Not adb.ExitCode = 0 Then
                                Return -2
                            End If
                        End If
                    Catch ex As ThreadInterruptedException
                        Return version
                    End Try


                    If inSection And line.Contains(versionName) Then
                        Dim versionTemp = line.Substring(line.IndexOf(versionName, StringComparison.Ordinal) + versionName.Length)
                        version = Double.Parse(versionTemp, CultureInfo.InvariantCulture)
                        Exit While
                    ElseIf line.Contains(package) And line.Contains("Package ") Then
                        inSection = True
                    End If
                    line = adb.StandardOutput.ReadLine
                End While
            Catch ex As IOException
                Return version
            End Try

            Return version
        End Using
    End Function

    Private Sub OK_Button_Click(ByVal sender As Object, ByVal e As EventArgs) Handles OK_Button.Click
        DialogResult = DialogResult.OK
        Close()
    End Sub

    Private Sub Cancel_Button_Click(ByVal sender As Object, ByVal e As EventArgs) Handles Cancel_Button.Click
        DialogResult = DialogResult.Cancel
        Close()
    End Sub

    Private Sub lstDevices_SelectedIndexChanged(sender As Object, e As EventArgs) Handles lstDevices.SelectedIndexChanged
        If lstDevices.SelectedIndices.Count > 0 Then
            OK_Button.Enabled = True
            _deviceIndex = lstDevices.SelectedIndex
        Else
            OK_Button.Enabled = False
        End If
    End Sub

    Private Sub listBox1_DrawItem(sender As Object, e As DrawItemEventArgs) Handles lstDevices.DrawItem
        e.DrawBackground()
        If (e.State And DrawItemState.Selected) = DrawItemState.Selected Then
            e.Graphics.FillRectangle(MaterialSkinManager.Instance.ColorScheme.AccentBrush, e.Bounds)
        End If
        Using b As New SolidBrush(e.ForeColor)
            If e.Index >= 0 Then
                e.Graphics.DrawString(lstDevices.GetItemText(lstDevices.Items(e.Index)), e.Font, b, e.Bounds)
            End If

        End Using
        e.DrawFocusRectangle()
    End Sub

    Private Sub dlgDeviceChoose_Closing(sender As Object, e As CancelEventArgs) Handles Me.Closing
        _autoUpdateThread.Interrupt()
    End Sub

    Private Sub dlgDeviceChoose_Closed(sender As Object, e As EventArgs) Handles Me.Closed
        If DialogResult = DialogResult.None Or DialogResult = Nothing Then
            DialogResult = DialogResult.Cancel
        End If
    End Sub

    Private Sub chkNoPromptSingleDevice_CheckedChanged(sender As Object, e As EventArgs) Handles chkNoPromptSingleDevice.CheckedChanged
        My.Settings.noPromptSingleDevice = chkNoPromptSingleDevice.Checked
    End Sub
End Class
