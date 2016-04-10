Public Class DeviceChooser
    Private Delegate Sub deviceListUpdateCallback(index As Integer, ByRef device As String)
    Private deviceIndex = -1
    Private adbExec As String


    Private Sub btnChoose_Click(sender As Object, e As EventArgs) Handles btnChoose.Click
        deviceIndex = lstDevices.SelectedIndex
        If deviceIndex < 0 Then
            MsgBox("No device has been selected", MsgBoxStyle.Exclamation + MsgBoxStyle.OkOnly)
        End If
    End Sub

    Private Sub lstDevices_SelectedIndexChanged(sender As Object, e As EventArgs) Handles lstDevices.SelectedIndexChanged
        If lstDevices.SelectedIndex < 0 Then
            btnChoose.Enabled = False
        Else
            btnChoose.Enabled = True
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
                updateDeviceListEntry(numberOfDevices, cleanupOutput(data))
                numberOfDevices += 1
            End If

            If (data.Trim = "List of devices attached") Then
                parserInDeviceList = True
            End If
            data = adb.StandardOutput.ReadLine

        End While
        MsgBox(output)

        If numberOfDevices > 1 Then
            MsgBox("More than one device")
        Else
            deviceIndex = 0
        End If
    End Sub

    Public Function getDevice() As String
        If isReady() Then
            Me.lstDevices.SelectedIndex = deviceIndex
            Return Me.lstDevices.SelectedItem
        End If

        Return Nothing
    End Function

    Public Function getUserInput(adb As String) As Boolean
        If (adb Is Nothing) Then
            Throw New ArgumentException("ADB Path not specified")
        End If
        adbExec = adb
        updateDeviceList()
        If isReady() Then
            Return False
        End If
        Me.Show()
        Me.Focus()
        Return True
    End Function

    Public Function isReady() As Boolean
        Return deviceIndex >= 0
    End Function

    Private Sub DeviceChooser_Load(sender As Object, e As EventArgs) Handles Me.Load
        Dim autoDeviceUpdater As New Threading.Thread(New Threading.ThreadStart(Sub()
                                                                                    While Not Threading.Thread.CurrentThread.ThreadState = Threading.ThreadState.StopRequested
                                                                                        updateDeviceList()
                                                                                        Threading.Thread.Sleep(1000)
                                                                                    End While
                                                                                End Sub))
    End Sub

    Private Sub updateDeviceListEntry(index As Integer, ByRef device As String)
        If lstDevices.InvokeRequired Then
            Me.Invoke(New deviceListUpdateCallback(AddressOf updateDeviceListEntry), New Object() {index, device})
        Else
            lstDevices.Items.Insert(index, device)
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
End Class