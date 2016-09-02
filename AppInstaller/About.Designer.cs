
//using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.ComponentModel;
using System.Windows.Forms;


namespace APKInstaller
{
    
    partial class About
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

        internal TableLayoutPanel TableLayoutPanel;

        internal TextBox TextBoxDescription;
        //Required by the Windows Form Designer

        private IContainer components = null;
        //NOTE: The following procedure is required by the Windows Form Designer
        //It can be modified using the Windows Form Designer.  
        //Do not modify it using the code editor.
        ///[DebuggerStepThrough()]
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(About));
            this.TableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.LabelCopyright = new System.Windows.Forms.Label();
            this.LabelCompanyName = new System.Windows.Forms.Label();
            this.Panel1 = new System.Windows.Forms.Panel();
            this.lblUpdateStatus = new System.Windows.Forms.Label();
            this.chkPrerelease = new System.Windows.Forms.CheckBox();
            this.LabelVersion = new System.Windows.Forms.Label();
            this.LogoPictureBox = new System.Windows.Forms.PictureBox();
            this.Panel2 = new System.Windows.Forms.Panel();
            this.TextBoxDescription = new System.Windows.Forms.TextBox();
            this.OKButton = new System.Windows.Forms.Button();
            this.TableLayoutPanel.SuspendLayout();
            this.Panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.LogoPictureBox)).BeginInit();
            this.Panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // TableLayoutPanel
            // 
            this.TableLayoutPanel.BackColor = System.Drawing.Color.White;
            this.TableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 127F));
            this.TableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 8F));
            this.TableLayoutPanel.Controls.Add(this.LabelCopyright, 1, 1);
            this.TableLayoutPanel.Controls.Add(this.Panel1, 1, 0);
            this.TableLayoutPanel.Controls.Add(this.Panel2, 1, 3);
            this.TableLayoutPanel.Controls.Add(this.LabelCompanyName, 1, 2);
            this.TableLayoutPanel.Controls.Add(this.LogoPictureBox, 0, 3);
            this.TableLayoutPanel.Location = new System.Drawing.Point(12, 12);
            this.TableLayoutPanel.Name = "TableLayoutPanel";
            this.TableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 77F));
            this.TableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 27F));
            this.TableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 26F));
            this.TableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 97F));
            this.TableLayoutPanel.Size = new System.Drawing.Size(454, 296);
            this.TableLayoutPanel.TabIndex = 0;
            // 
            // LabelCopyright
            // 
            this.LabelCopyright.Dock = System.Windows.Forms.DockStyle.Fill;
            this.LabelCopyright.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(222)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.LabelCopyright.Location = new System.Drawing.Point(130, 77);
            this.LabelCopyright.Name = "LabelCopyright";
            this.LabelCopyright.Size = new System.Drawing.Size(321, 27);
            this.LabelCopyright.TabIndex = 0;
            // 
            // LabelCompanyName
            // 
            this.LabelCompanyName.Dock = System.Windows.Forms.DockStyle.Fill;
            this.LabelCompanyName.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(222)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.LabelCompanyName.Location = new System.Drawing.Point(130, 104);
            this.LabelCompanyName.Name = "LabelCompanyName";
            this.LabelCompanyName.Size = new System.Drawing.Size(321, 26);
            this.LabelCompanyName.TabIndex = 1;
            // 
            // Panel1
            // 
            this.Panel1.Controls.Add(this.lblUpdateStatus);
            this.Panel1.Controls.Add(this.chkPrerelease);
            this.Panel1.Controls.Add(this.LabelVersion);
            this.Panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Panel1.Location = new System.Drawing.Point(130, 3);
            this.Panel1.Name = "Panel1";
            this.Panel1.Size = new System.Drawing.Size(321, 71);
            this.Panel1.TabIndex = 2;
            // 
            // lblUpdateStatus
            // 
            this.lblUpdateStatus.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(222)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.lblUpdateStatus.Location = new System.Drawing.Point(-3, 31);
            this.lblUpdateStatus.Name = "lblUpdateStatus";
            this.lblUpdateStatus.Size = new System.Drawing.Size(329, 28);
            this.lblUpdateStatus.TabIndex = 0;
            // 
            // chkPrerelease
            // 
            this.chkPrerelease.Location = new System.Drawing.Point(214, 4);
            this.chkPrerelease.Name = "chkPrerelease";
            this.chkPrerelease.Size = new System.Drawing.Size(115, 24);
            this.chkPrerelease.TabIndex = 1;
            this.chkPrerelease.Text = "Prerelease";
            this.chkPrerelease.UseVisualStyleBackColor = true;
            // 
            // LabelVersion
            // 
            this.LabelVersion.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(222)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.LabelVersion.Location = new System.Drawing.Point(3, 5);
            this.LabelVersion.Name = "LabelVersion";
            this.LabelVersion.Size = new System.Drawing.Size(205, 23);
            this.LabelVersion.TabIndex = 2;
            // 
            // LogoPictureBox
            // 
            this.LogoPictureBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.LogoPictureBox.BackgroundImage = global::APKInstaller.Resources.FIRSTTech_IconVert_RGB;
            this.LogoPictureBox.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.LogoPictureBox.InitialImage = global::APKInstaller.Resources.FIRSTTech_IconVert_RGB;
            this.LogoPictureBox.Location = new System.Drawing.Point(3, 190);
            this.LogoPictureBox.Name = "LogoPictureBox";
            this.LogoPictureBox.Size = new System.Drawing.Size(121, 103);
            this.LogoPictureBox.TabIndex = 4;
            this.LogoPictureBox.TabStop = false;
            // 
            // Panel2
            // 
            this.Panel2.Controls.Add(this.TextBoxDescription);
            this.Panel2.Controls.Add(this.OKButton);
            this.Panel2.Location = new System.Drawing.Point(130, 133);
            this.Panel2.Name = "Panel2";
            this.Panel2.Size = new System.Drawing.Size(321, 160);
            this.Panel2.TabIndex = 3;
            // 
            // TextBoxDescription
            // 
            this.TextBoxDescription.BackColor = System.Drawing.Color.White;
            this.TextBoxDescription.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.TextBoxDescription.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.TextBoxDescription.Enabled = false;
            this.TextBoxDescription.Location = new System.Drawing.Point(3, 3);
            this.TextBoxDescription.Multiline = true;
            this.TextBoxDescription.Name = "TextBoxDescription";
            this.TextBoxDescription.ReadOnly = true;
            this.TextBoxDescription.Size = new System.Drawing.Size(326, 100);
            this.TextBoxDescription.TabIndex = 0;
            this.TextBoxDescription.TabStop = false;
            // 
            // OKButton
            // 
            this.OKButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.OKButton.Location = new System.Drawing.Point(243, 134);
            this.OKButton.Name = "OKButton";
            this.OKButton.Size = new System.Drawing.Size(75, 23);
            this.OKButton.TabIndex = 1;
            this.OKButton.Text = "OK";
            // 
            // About
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.OKButton;
            this.ClientSize = new System.Drawing.Size(478, 320);
            this.Controls.Add(this.TableLayoutPanel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "About";
            this.ShowInTaskbar = false;
            this.Text = "About";
            this.Load += new System.EventHandler(this.About_Load);
            this.TableLayoutPanel.ResumeLayout(false);
            this.Panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.LogoPictureBox)).EndInit();
            this.Panel2.ResumeLayout(false);
            this.Panel2.PerformLayout();
            this.ResumeLayout(false);

        }

        internal Label LabelVersion;
        internal Label LabelCompanyName;
        internal Button OKButton;
        internal Label LabelCopyright;
        //Friend WithEvents Label1 As Label
        internal Panel Panel1;
        //Friend WithEvents Label3 As Label
        internal Label Label1;
        internal CheckBox chkPrerelease;
        internal Label lblUpdateStatus;
        internal PictureBox LogoPictureBox;
        internal Panel Panel2;
    }
}
