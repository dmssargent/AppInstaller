using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Microsoft.VisualBasic;

namespace APKInstaller
{
    partial class Main
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Main));
            this.lblStatus = new System.Windows.Forms.Label();
            this.txtFileLocation = new System.Windows.Forms.TextBox();
            this.chkReinstall = new System.Windows.Forms.CheckBox();
            this.chkForce = new System.Windows.Forms.CheckBox();
            this.btnOpenFileDialogTrigger = new System.Windows.Forms.Button();
            this.btnInstall = new System.Windows.Forms.Button();
            this.pgbStatus = new System.Windows.Forms.ProgressBar();
            this.lnkAbout = new System.Windows.Forms.LinkLabel();
            this.lnkHelp = new System.Windows.Forms.LinkLabel();
            this.SuspendLayout();
            // 
            // lblStatus
            // 
            this.lblStatus.AllowDrop = true;
            this.lblStatus.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblStatus.Font = new System.Drawing.Font("Roboto", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblStatus.Location = new System.Drawing.Point(7, 49);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(840, 301);
            this.lblStatus.TabIndex = 8;
            this.lblStatus.Text = "Drag the APK here...";
            this.lblStatus.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblStatus.DragDrop += new System.Windows.Forms.DragEventHandler(this.Me_DragDrop);
            this.lblStatus.DragEnter += new System.Windows.Forms.DragEventHandler(this.Me_DragEnter);
            // 
            // txtFileLocation
            // 
            this.txtFileLocation.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtFileLocation.BackColor = System.Drawing.Color.White;
            this.txtFileLocation.Font = new System.Drawing.Font("Roboto", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtFileLocation.Location = new System.Drawing.Point(12, 527);
            this.txtFileLocation.Name = "txtFileLocation";
            this.txtFileLocation.Size = new System.Drawing.Size(723, 28);
            this.txtFileLocation.TabIndex = 1;
            this.txtFileLocation.TabStop = false;
            this.txtFileLocation.TextChanged += new System.EventHandler(this.txtFileLocation_TextChanged);
            this.txtFileLocation.DoubleClick += new System.EventHandler(this.txtFileLocation_DoubleClick);
            // 
            // chkReinstall
            // 
            this.chkReinstall.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.chkReinstall.AutoSize = true;
            this.chkReinstall.Checked = true;
            this.chkReinstall.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkReinstall.Font = new System.Drawing.Font("Roboto", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkReinstall.Location = new System.Drawing.Point(12, 497);
            this.chkReinstall.Name = "chkReinstall";
            this.chkReinstall.Size = new System.Drawing.Size(166, 24);
            this.chkReinstall.TabIndex = 6;
            this.chkReinstall.Text = "Update (Reinstall)";
            this.chkReinstall.UseVisualStyleBackColor = true;
            this.chkReinstall.CheckedChanged += new System.EventHandler(this.chkReinstall_CheckedChanged);
            // 
            // chkForce
            // 
            this.chkForce.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.chkForce.AutoSize = true;
            this.chkForce.Font = new System.Drawing.Font("Roboto", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkForce.Location = new System.Drawing.Point(217, 497);
            this.chkForce.Margin = new System.Windows.Forms.Padding(0);
            this.chkForce.Name = "chkForce";
            this.chkForce.Size = new System.Drawing.Size(74, 24);
            this.chkForce.TabIndex = 3;
            this.chkForce.Text = "Force";
            this.chkForce.UseVisualStyleBackColor = true;
            this.chkForce.CheckedChanged += new System.EventHandler(this.chkForce_CheckedChanged);
            // 
            // btnOpenFileDialogTrigger
            // 
            this.btnOpenFileDialogTrigger.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOpenFileDialogTrigger.Font = new System.Drawing.Font("Roboto", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnOpenFileDialogTrigger.Location = new System.Drawing.Point(743, 527);
            this.btnOpenFileDialogTrigger.Name = "btnOpenFileDialogTrigger";
            this.btnOpenFileDialogTrigger.Size = new System.Drawing.Size(104, 27);
            this.btnOpenFileDialogTrigger.TabIndex = 4;
            this.btnOpenFileDialogTrigger.Text = "Browse";
            this.btnOpenFileDialogTrigger.UseVisualStyleBackColor = true;
            this.btnOpenFileDialogTrigger.Click += new System.EventHandler(this.btnOpenFileDialogTrigger_Click);
            // 
            // btnInstall
            // 
            this.btnInstall.AllowDrop = true;
            this.btnInstall.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnInstall.Font = new System.Drawing.Font("Roboto", 13.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnInstall.ForeColor = System.Drawing.Color.Green;
            this.btnInstall.Location = new System.Drawing.Point(295, 382);
            this.btnInstall.Name = "btnInstall";
            this.btnInstall.Size = new System.Drawing.Size(271, 78);
            this.btnInstall.TabIndex = 3;
            this.btnInstall.Text = "INSTALL";
            this.btnInstall.UseVisualStyleBackColor = true;
            this.btnInstall.Click += new System.EventHandler(this.btnInstaller_Click);
            // 
            // pgbStatus
            // 
            this.pgbStatus.Location = new System.Drawing.Point(11, 482);
            this.pgbStatus.Name = "pgbStatus";
            this.pgbStatus.Size = new System.Drawing.Size(820, 5);
            this.pgbStatus.Style = System.Windows.Forms.ProgressBarStyle.Marquee;
            this.pgbStatus.TabIndex = 2;
            // 
            // lnkAbout
            // 
            this.lnkAbout.ActiveLinkColor = System.Drawing.Color.White;
            this.lnkAbout.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lnkAbout.BackColor = System.Drawing.Color.White;
            this.lnkAbout.Font = new System.Drawing.Font("Roboto", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lnkAbout.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(222)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.lnkAbout.LinkBehavior = System.Windows.Forms.LinkBehavior.HoverUnderline;
            this.lnkAbout.LinkColor = System.Drawing.Color.Black;
            this.lnkAbout.Location = new System.Drawing.Point(739, 9);
            this.lnkAbout.Name = "lnkAbout";
            this.lnkAbout.Size = new System.Drawing.Size(58, 23);
            this.lnkAbout.TabIndex = 1;
            this.lnkAbout.TabStop = true;
            this.lnkAbout.Text = "About";
            this.lnkAbout.VisitedLinkColor = System.Drawing.Color.White;
            this.lnkAbout.Click += new System.EventHandler(this.lnkAbout_Click);
            // 
            // lnkHelp
            // 
            this.lnkHelp.ActiveLinkColor = System.Drawing.Color.White;
            this.lnkHelp.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lnkHelp.AutoSize = true;
            this.lnkHelp.BackColor = System.Drawing.Color.White;
            this.lnkHelp.Font = new System.Drawing.Font("Roboto", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lnkHelp.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(222)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.lnkHelp.LinkBehavior = System.Windows.Forms.LinkBehavior.HoverUnderline;
            this.lnkHelp.LinkColor = System.Drawing.Color.Black;
            this.lnkHelp.Location = new System.Drawing.Point(803, 9);
            this.lnkHelp.Name = "lnkHelp";
            this.lnkHelp.Size = new System.Drawing.Size(44, 20);
            this.lnkHelp.TabIndex = 0;
            this.lnkHelp.TabStop = true;
            this.lnkHelp.Text = "Help";
            this.lnkHelp.VisitedLinkColor = System.Drawing.Color.White;
            this.lnkHelp.Click += new System.EventHandler(this.lnkHelp_Click);
            // 
            // Main
            // 
            this.AllowDrop = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(862, 567);
            this.Controls.Add(this.lnkHelp);
            this.Controls.Add(this.lnkAbout);
            this.Controls.Add(this.pgbStatus);
            this.Controls.Add(this.btnInstall);
            this.Controls.Add(this.btnOpenFileDialogTrigger);
            this.Controls.Add(this.chkForce);
            this.Controls.Add(this.chkReinstall);
            this.Controls.Add(this.txtFileLocation);
            this.Controls.Add(this.lblStatus);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Main";
            this.Text = "App Installer";
            this.ResumeLayout(false);
            this.PerformLayout();

        }


        #endregion

        internal Label lblStatus;
        internal TextBox txtFileLocation;
        internal CheckBox chkReinstall;
        internal CheckBox chkForce;
        internal Button btnOpenFileDialogTrigger;
        internal Button btnInstall;
        internal ProgressBar pgbStatus;
        internal LinkLabel lnkAbout;
        internal LinkLabel lnkHelp;
    }
}