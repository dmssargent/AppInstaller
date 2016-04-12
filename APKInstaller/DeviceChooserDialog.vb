Imports System.ComponentModel
Imports System.Windows.Forms

Public Class dlgDeviceChoose
    Private Delegate Function userInputCallback(ByVal adb As String) As DialogResult
    Private Delegate Sub deviceListUpdateCallback(ByVal index As Integer, ByVal device As String)
    Private autoUpdateThread As Threading.Thread

    Private deviceIndex As Integer = -1
    Private adbExec As String


    Private Sub OK_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OK_Button.Click
        Me.DialogResult = DialogResult.OK
        Me.Close()
    End Sub

    Private Sub Cancel_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cancel_Button.Click
        Me.DialogResult = DialogResult.Cancel
        Me.Close()
    End Sub

    Private Sub lstDevices_SelectedIndexChanged(sender As Object, e As EventArgs) Handles lstDevices.SelectedIndexChanged
        If lstDevices.SelectedIndices.Count > 0 Then
            OK_Button.Enabled = True
            deviceIndex = lstDevices.SelectedIndex
        Else
            OK_Button.Enabled = False
        End If
    End Sub

    Private Sub updateDeviceList()
        Dim adb As New Process
        adb.StartInfo.FileName = adbExec
        adb.StartInfo.Arguments = "devices"
        adb.StartInfo.CreateNoWindow = True
        adb.StartInfo.UseShellExecute = False
        adb.StartInfo.RedirectStandardOutput = True
        adb.Start()
        adb.WaitForExit()

        Dim numberOfDevicesLastFound = lstDevices.Items.Count
        Dim data = adb.StandardOutput.ReadLine
        Dim output = ""
        Dim numberOfDevices = 0
        Dim parserInDeviceList = False
        While (data IsNot Nothing)
            data = data.Trim()
            output &= data & vbCrLf
            If parserInDeviceList Then
                If data Is "" Then
                    data = adb.StandardOutput.ReadLine
                    Continue While
                End If
                Dim device = cleanupOutput(data)
                Dim details = ""
                Dim rcVersion = CheckForFtcRobotController(adbExec, device)
                Dim dsVersion = CheckForFtcDriverStation(adbExec, device)
                If rcVersion > 0 Then
                    details &= "[FTC Robot Controller " & rcVersion
                End If

                If dsVersion > 0 Then
                    If details = "" Then
                        details &= "["
                    Else
                        details &= ";"
                    End If
                    details &= " FTC Driver Station " & dsVersion
                End If

                If Not details = "" Then
                    details &= "]"
                End If
                updateDeviceListEntry(numberOfDevices, device & vbTab & details)
                numberOfDevices += 1
            End If

            If (data.Trim = "List of devices attached") Then
                parserInDeviceList = True
            End If
            data = adb.StandardOutput.ReadLine
        End While


        For i As Integer = numberOfDevices To numberOfDevicesLastFound - 1
            updateDeviceListEntry(i, Nothing)
        Next i

        If numberOfDevices > 1 Then
            'MsgBox("More than one device detected")
        Else
            deviceIndex = 0
        End If
    End Sub

    Public Function getDevice() As String
        If isReady() Then
            'Me.lstDevices.SelectedItems.

            Return cleanupOutput(lstDevices.SelectedItem.ToString)
        End If

        Return Nothing
    End Function

    Public Function getUserInput(adb As String) As DialogResult
        If (adb Is Nothing) Then
            Throw New ArgumentException("ADB Path not specified")
        End If
        If Me.InvokeRequired Then
            Return CType(Me.Invoke(New userInputCallback(AddressOf getUserInput), New Object() {adb}), DialogResult)
        Else
            adbExec = adb
            updateDeviceList()
            If isReady() Then
                Return Nothing
            End If
            Return Me.ShowDialog()
        End If
    End Function

    Public Function isReady() As Boolean
        Return deviceIndex >= 0
    End Function

    Private Sub DeviceChooser_Load(sender As Object, e As EventArgs) Handles Me.Load
        autoUpdateThread = New Threading.Thread(
            New Threading.ThreadStart(Sub()
                                          Do
                                              updateDeviceList()
                                              Try
                                                  Threading.Thread.Sleep(1000)
                                              Catch ex As Threading.ThreadInterruptedException
                                                  Exit Do
                                              End Try
                                          Loop While Me.Visible
                                      End Sub))
        autoUpdateThread.Start()
        Me.lstDevices.Font = MaterialSkin.MaterialSkinManager.Instance.ROBOTO_MEDIUM_12
        Me.lblDevices.Font = MaterialSkin.MaterialSkinManager.Instance.ROBOTO_MEDIUM_12
        lstDevices.DrawMode = DrawMode.OwnerDrawFixed

        'SkinManager.AddFormToManage(Me)
        Me.Focus()
        Me.CenterToScreen()
    End Sub

    Private Sub updateDeviceListEntry(ByVal index As Integer, ByVal device As String)
        If lstDevices.InvokeRequired Then
            Try
                If Me.IsDisposed Then
                    Return
                End If
                Me.Invoke(New deviceListUpdateCallback(AddressOf updateDeviceListEntry), New Object() {index, device})
            Catch ex As ObjectDisposedException
                Return
            End Try

        Else

            Dim oldIndex = lstDevices.SelectedIndex


            If lstDevices.Items.Count > index Then
                lstDevices.Items.RemoveAt(index)
            End If

            If device IsNot Nothing Then
                lstDevices.Items.Insert(index, device)
            End If

            If oldIndex < lstDevices.Items.Count Then
                If oldIndex >= 0 Then
                    lstDevices.SelectedIndex = oldIndex
                End If
            End If
        End If
    End Sub

    Private Function cleanupOutput(ByVal inputValue As String) As String
        If inputValue.Contains(" ") Then
            inputValue = inputValue.Substring(0, inputValue.IndexOf(" "))
        End If

        If inputValue.Contains(vbTab) Then
            inputValue = inputValue.Substring(0, inputValue.IndexOf(vbTab))
        End If

        Return inputValue
    End Function

    Private Sub dlgDeviceChoose_Closing(sender As Object, e As CancelEventArgs) Handles Me.Closing
        autoUpdateThread.Interrupt()
    End Sub

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="adb"></param>
    ''' <param name="device"></param>
    ''' <returns></returns>
    Public Shared Function CheckForFtcRobotController(ByVal adb As String, ByVal device As String) As Double
        Return CheckPackageVersion(adb, device, "com.qualcomm.ftcrobotcontroller")
    End Function

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="adb"></param>
    ''' <param name="device"></param>
    ''' <returns></returns>
    Public Shared Function CheckForFtcDriverStation(ByVal adb As String, ByVal device As String) As Double
        Return CheckPackageVersion(adb, device, "com.qualcomm.ftcdriverstation")
    End Function

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="adbLocation"></param>
    ''' <param name="device"></param>
    ''' <param name="package"></param>
    ''' <returns></returns>
    Public Shared Function CheckPackageVersion(ByVal adbLocation As String, ByVal device As String, ByVal package As String) As Double
        Dim adb As New Process
        adb.StartInfo.FileName = adbLocation
        adb.StartInfo.Arguments = "-s " & device & " shell pm dump """ & package & """"
        adb.StartInfo.CreateNoWindow = True
        adb.StartInfo.UseShellExecute = False
        adb.StartInfo.RedirectStandardOutput = True

        adb.Start()

        Const VERSION_NAME As String = "versionName="
        Dim version As Double = -1
        Dim inSection As Boolean = False
        Dim line As String = adb.StandardOutput.ReadLine
        Try
            While line IsNot Nothing
                'Detect interrupts
                Threading.Thread.Sleep(1)

                line = line.Trim
                If adb.HasExited Then
                    If Not adb.ExitCode = 0 Then
                        Return -2
                    End If
                End If
                If inSection And line.Contains(VERSION_NAME) Then
                    Dim versionName As String = line.Substring(line.IndexOf(VERSION_NAME) + VERSION_NAME.Length)
                    version = Double.Parse(versionName)
                    Exit While
                ElseIf line.Contains(package) And line.Contains("Package ") Then
                    inSection = True
                End If
                line = adb.StandardOutput.ReadLine
            End While
        Catch ex As Exception
            Return version
        End Try
        Return version
    End Function

    Private Sub dlgDeviceChoose_Closed(sender As Object, e As EventArgs) Handles Me.Closed
        If DialogResult = DialogResult.None Or DialogResult = Nothing Then
            DialogResult = DialogResult.Cancel
        End If
    End Sub

    Private Sub listBox1_DrawItem(sender As Object, e As DrawItemEventArgs) Handles lstDevices.DrawItem
        e.DrawBackground()
        If (e.State And DrawItemState.Selected) = DrawItemState.Selected Then
            e.Graphics.FillRectangle(MaterialSkin.MaterialSkinManager.Instance.ColorScheme.AccentBrush, e.Bounds)
        End If
        Using b As New SolidBrush(e.ForeColor)
            e.Graphics.DrawString(lstDevices.GetItemText(lstDevices.Items(e.Index)), e.Font, b, e.Bounds)
        End Using
        e.DrawFocusRectangle()
    End Sub
End Class
