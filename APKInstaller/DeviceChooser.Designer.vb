<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class DeviceChooser
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(DeviceChooser))
        Me.lstDevices = New System.Windows.Forms.ListBox()
        Me.btnChoose = New System.Windows.Forms.Button()
        Me.SuspendLayout()
        '
        'lstDevices
        '
        Me.lstDevices.FormattingEnabled = True
        Me.lstDevices.ItemHeight = 16
        Me.lstDevices.Location = New System.Drawing.Point(12, 12)
        Me.lstDevices.Name = "lstDevices"
        Me.lstDevices.Size = New System.Drawing.Size(309, 244)
        Me.lstDevices.TabIndex = 0
        '
        'btnChoose
        '
        Me.btnChoose.Location = New System.Drawing.Point(199, 269)
        Me.btnChoose.Name = "btnChoose"
        Me.btnChoose.Size = New System.Drawing.Size(122, 33)
        Me.btnChoose.TabIndex = 1
        Me.btnChoose.Text = "Install On Device"
        Me.btnChoose.UseVisualStyleBackColor = True
        '
        'DeviceChooser
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 16.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(333, 314)
        Me.Controls.Add(Me.btnChoose)
        Me.Controls.Add(Me.lstDevices)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "DeviceChooser"
        Me.Text = "Device Chooser"
        Me.TopMost = True
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents lstDevices As ListBox
    Friend WithEvents btnChoose As Button
End Class
