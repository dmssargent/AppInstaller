using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace APKInstaller
{
    //[DesignerGenerated()]
    partial class DeviceChooserDialog : Form
    {

        //Form overrides dispose to clean up the component list.
        //[DebuggerNonUserCode()]
        protected override void Dispose(bool disposing)
        {
            try
            {
                if (disposing && components != null)
                {
                    components.Dispose();
                }
            }
            finally
            {
                base.Dispose(disposing);
            }
        }

        //Required by the Windows Form Designer

        private IContainer components = null;
        //NOTE: The following procedure is required by the Windows Form Designer
        //It can be modified using the Windows Form Designer.  
        //Do not modify it using the code editor.
       // [DebuggerStepThrough()]
        private void InitializeComponent()
        {
            this.TableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.Cancel_Button = new System.Windows.Forms.Button();
            this.OK_Button = new System.Windows.Forms.Button();
            this.lblDevices = new System.Windows.Forms.Label();
            this.lstDevices = new System.Windows.Forms.ListBox();
            this.chkNoPromptSingleDevice = new System.Windows.Forms.CheckBox();
            this.TableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // TableLayoutPanel1
            // 
            this.TableLayoutPanel1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.TableLayoutPanel1.ColumnCount = 2;
            this.TableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 88F));
            this.TableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 112F));
            this.TableLayoutPanel1.Controls.Add(this.Cancel_Button, 0, 0);
            this.TableLayoutPanel1.Controls.Add(this.OK_Button, 1, 0);
            this.TableLayoutPanel1.Location = new System.Drawing.Point(446, 329);
            this.TableLayoutPanel1.Margin = new System.Windows.Forms.Padding(4);
            this.TableLayoutPanel1.Name = "TableLayoutPanel1";
            this.TableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.TableLayoutPanel1.Size = new System.Drawing.Size(195, 36);
            this.TableLayoutPanel1.TabIndex = 3;
            // 
            // Cancel_Button
            // 
            this.Cancel_Button.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.Cancel_Button.AutoSize = true;
            this.Cancel_Button.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.Cancel_Button.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.Cancel_Button.Location = new System.Drawing.Point(13, 4);
            this.Cancel_Button.Name = "Cancel_Button";
            this.Cancel_Button.Size = new System.Drawing.Size(61, 27);
            this.Cancel_Button.TabIndex = 1;
            this.Cancel_Button.Text = "Cancel";
            this.Cancel_Button.Click += new System.EventHandler(this.Cancel_Button_Click);
            // 
            // OK_Button
            // 
            this.OK_Button.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.OK_Button.AutoSize = true;
            this.OK_Button.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.OK_Button.Location = new System.Drawing.Point(125, 4);
            this.OK_Button.Name = "OK_Button";
            this.OK_Button.Size = new System.Drawing.Size(38, 27);
            this.OK_Button.TabIndex = 0;
            this.OK_Button.Text = "OK";
            this.OK_Button.Click += new System.EventHandler(this.OK_Button_Click);
            // 
            // lblDevices
            // 
            this.lblDevices.AutoSize = true;
            this.lblDevices.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(222)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.lblDevices.Location = new System.Drawing.Point(9, 9);
            this.lblDevices.Name = "lblDevices";
            this.lblDevices.Size = new System.Drawing.Size(103, 17);
            this.lblDevices.TabIndex = 2;
            this.lblDevices.Text = "Choose Device";
            // 
            // lstDevices
            // 
            this.lstDevices.BackColor = System.Drawing.Color.White;
            this.lstDevices.FormattingEnabled = true;
            this.lstDevices.ItemHeight = 16;
            this.lstDevices.Location = new System.Drawing.Point(12, 29);
            this.lstDevices.Name = "lstDevices";
            this.lstDevices.Size = new System.Drawing.Size(630, 292);
            this.lstDevices.TabIndex = 5;
            this.lstDevices.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.listBox1_DrawItem);
            this.lstDevices.SelectedIndexChanged += new System.EventHandler(this.lstDevices_SelectedIndexChanged);
            // 
            // chkNoPromptSingleDevice
            // 
            this.chkNoPromptSingleDevice.AutoSize = true;
            this.chkNoPromptSingleDevice.Location = new System.Drawing.Point(12, 344);
            this.chkNoPromptSingleDevice.Name = "chkNoPromptSingleDevice";
            this.chkNoPromptSingleDevice.Size = new System.Drawing.Size(307, 21);
            this.chkNoPromptSingleDevice.TabIndex = 0;
            this.chkNoPromptSingleDevice.Text = "Do not prompt if only one device is attached";
            this.chkNoPromptSingleDevice.UseVisualStyleBackColor = true;
            this.chkNoPromptSingleDevice.CheckedChanged += new System.EventHandler(this.chkNoPromptSingleDevice_CheckedChanged);
            // 
            // DeviceChooserDialog
            // 
            this.AcceptButton = this.OK_Button;
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Window;
            this.CancelButton = this.Cancel_Button;
            this.ClientSize = new System.Drawing.Size(654, 378);
            this.ControlBox = false;
            this.Controls.Add(this.chkNoPromptSingleDevice);
            this.Controls.Add(this.lstDevices);
            this.Controls.Add(this.lblDevices);
            this.Controls.Add(this.TableLayoutPanel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "DeviceChooserDialog";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Choose Devices";
            this.TopMost = true;
            this.TableLayoutPanel1.ResumeLayout(false);
            this.TableLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        internal TableLayoutPanel TableLayoutPanel1;
        internal Button OK_Button;
        internal Button Cancel_Button;
        internal Label lblDevices;
        internal ListBox lstDevices;
        internal CheckBox chkNoPromptSingleDevice;
    }
}
