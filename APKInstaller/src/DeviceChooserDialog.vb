Imports System.ComponentModel

Public Class DeviceChooserDialog
    Private Delegate Function userInputCallback() As DialogResult
    Private Delegate Sub deviceListUpdateCallback(ByVal index As Integer, ByVal device As String)
    Private autoUpdateThread As Threading.Thread

    Private deviceIndex As Integer = -1

    Public ReadOnly Property Device As String
        Get
            If IsReady() Then
                Return CleanupOutput(lstDevices.SelectedItem.ToString)
            End If
            Return Nothing
        End Get
    End Property



    Private Sub UpdateDeviceList()
        Dim adb = AndroidTools.RunAdb("devices", True, True, True)

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
                Dim device = CleanupOutput(data)
                Dim details = ""
                Dim rcVersion = CheckForFtcRobotController(device)
                Dim dsVersion = CheckForFtcDriverStation(device)
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
                UpdateDeviceListEntry(numberOfDevices, device & vbTab & details)
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

        'If numberOfDevices > 1 Then
        '    MsgBox("More than one device detected")

        'End If
        If numberOfDevices = 1 Then
            lstDevices.SelectedIndex = 0
        End If

    End Sub



    <CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate")>
    Public Function GetUserInput() As DialogResult
        If InvokeRequired Then
            Return CType(Invoke(New userInputCallback(AddressOf GetUserInput)), DialogResult)
        Else

            UpdateDeviceList()
            If IsReady() Then
                Return Nothing
            End If
            Return ShowDialog()
        End If
    End Function

    Public Function IsReady() As Boolean
        Return deviceIndex >= 0
    End Function

    Private Sub DeviceChooser_Load(sender As Object, e As EventArgs) Handles Me.Load
        autoUpdateThread = New Threading.Thread(
            New Threading.ThreadStart(Sub()
                                          Do
                                              UpdateDeviceList()
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

    Private Sub UpdateDeviceListEntry(ByVal index As Integer, ByVal device As String)
        If lstDevices.InvokeRequired Then
            Try
                If Me.IsDisposed Then
                    Return
                End If
                Me.Invoke(New deviceListUpdateCallback(AddressOf UpdateDeviceListEntry), New Object() {index, device})
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

    Private Shared Function CleanupOutput(ByVal inputValue As String) As String
        If inputValue.Contains(" ") Then
            inputValue = inputValue.Substring(0, inputValue.IndexOf(" "))
        End If

        If inputValue.Contains(vbTab) Then
            inputValue = inputValue.Substring(0, inputValue.IndexOf(vbTab))
        End If

        Return inputValue
    End Function



    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="device"></param>
    ''' <returns></returns>
    <CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId:="Ftc")>
    Public Shared Function CheckForFtcRobotController(ByVal device As String) As Double
        Return CheckPackageVersion(device, "com.qualcomm.ftcrobotcontroller")
    End Function

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="device"></param>
    ''' <returns></returns>
    Public Shared Function CheckForFtcDriverStation(ByVal device As String) As Double
        Return CheckPackageVersion(device, "com.qualcomm.ftcdriverstation")
    End Function

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="device"></param>
    ''' <param name="package"></param>
    ''' <returns></returns>
    Public Shared Function CheckPackageVersion(ByVal device As String, ByVal package As String) As Double
        Dim adb = AndroidTools.RunAdb("-s " & device & " shell pm dump """ & package & """", True, True, False)

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
        Catch ex As IO.IOException
            Return version
        End Try
        Return version
    End Function

    Private Sub OK_Button_Click(ByVal sender As Object, ByVal e As EventArgs) Handles OK_Button.Click
        Me.DialogResult = DialogResult.OK
        Me.Close()
    End Sub

    Private Sub Cancel_Button_Click(ByVal sender As Object, ByVal e As EventArgs) Handles Cancel_Button.Click
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

    Private Sub dlgDeviceChoose_Closing(sender As Object, e As CancelEventArgs) Handles Me.Closing
        autoUpdateThread.Interrupt()
    End Sub

    Private Sub dlgDeviceChoose_Closed(sender As Object, e As EventArgs) Handles Me.Closed
        If DialogResult = DialogResult.None Or DialogResult = Nothing Then
            DialogResult = DialogResult.Cancel
        End If
    End Sub


End Class
