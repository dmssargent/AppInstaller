
using System.Windows.Forms;
using MaterialSkin.Controls;

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
            this.txtFileLocation = new MaterialSkin.Controls.MaterialSingleLineTextField();
            this.chkReinstall = new MaterialSkin.Controls.MaterialCheckBox();
            this.chkForce = new MaterialSkin.Controls.MaterialCheckBox();
            this.btnOpenFileDialogTrigger = new MaterialSkin.Controls.MaterialFlatButton();
            this.btnInstall = new MaterialSkin.Controls.MaterialRaisedButton();
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
            this.lblStatus.Location = new System.Drawing.Point(5, 102);
            this.lblStatus.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(630, 183);
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
            this.txtFileLocation.Depth = 0;
            this.txtFileLocation.Font = new System.Drawing.Font("Roboto", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtFileLocation.Hint = "";
            this.txtFileLocation.Location = new System.Drawing.Point(9, 428);
            this.txtFileLocation.Margin = new System.Windows.Forms.Padding(2);
            this.txtFileLocation.MaxLength = 32767;
            this.txtFileLocation.MouseState = MaterialSkin.MouseState.HOVER;
            this.txtFileLocation.Name = "txtFileLocation";
            this.txtFileLocation.PasswordChar = '\0';
            this.txtFileLocation.SelectedText = "";
            this.txtFileLocation.SelectionLength = 0;
            this.txtFileLocation.SelectionStart = 0;
            this.txtFileLocation.Size = new System.Drawing.Size(543, 28);
            this.txtFileLocation.TabIndex = 1;
            this.txtFileLocation.TabStop = false;
            this.txtFileLocation.UseSystemPasswordChar = false;
            this.txtFileLocation.DoubleClick += new System.EventHandler(this.txtFileLocation_DoubleClick);
            this.txtFileLocation.TextChanged += new System.EventHandler(this.txtFileLocation_TextChanged);
            // 
            // chkReinstall
            // 
            this.chkReinstall.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.chkReinstall.AutoSize = true;
            this.chkReinstall.Checked = true;
            this.chkReinstall.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkReinstall.Depth = 0;
            this.chkReinstall.Font = new System.Drawing.Font("Roboto", 10F);
            this.chkReinstall.Location = new System.Drawing.Point(9, 394);
            this.chkReinstall.Margin = new System.Windows.Forms.Padding(2);
            this.chkReinstall.MouseLocation = new System.Drawing.Point(-1, -1);
            this.chkReinstall.MouseState = MaterialSkin.MouseState.HOVER;
            this.chkReinstall.Name = "chkReinstall";
            this.chkReinstall.Ripple = true;
            this.chkReinstall.Size = new System.Drawing.Size(169, 30);
            this.chkReinstall.TabIndex = 6;
            this.chkReinstall.Text = "Update (Reinstall)";
            this.chkReinstall.UseVisualStyleBackColor = true;
            this.chkReinstall.CheckedChanged += new System.EventHandler(this.chkReinstall_CheckedChanged);
            // 
            // chkForce
            // 
            this.chkForce.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.chkForce.AutoSize = true;
            this.chkForce.Depth = 0;
            this.chkForce.Font = new System.Drawing.Font("Roboto", 10F);
            this.chkForce.Location = new System.Drawing.Point(163, 394);
            this.chkForce.Margin = new System.Windows.Forms.Padding(0);
            this.chkForce.MouseLocation = new System.Drawing.Point(-1, -1);
            this.chkForce.MouseState = MaterialSkin.MouseState.HOVER;
            this.chkForce.Name = "chkForce";
            this.chkForce.Ripple = true;
            this.chkForce.Size = new System.Drawing.Size(74, 30);
            this.chkForce.TabIndex = 3;
            this.chkForce.Text = "Force";
            this.chkForce.UseVisualStyleBackColor = true;
            this.chkForce.CheckedChanged += new System.EventHandler(this.chkForce_CheckedChanged);
            // 
            // btnOpenFileDialogTrigger
            // 
            this.btnOpenFileDialogTrigger.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOpenFileDialogTrigger.AutoSize = true;
            this.btnOpenFileDialogTrigger.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btnOpenFileDialogTrigger.BackColor = System.Drawing.Color.DimGray;
            this.btnOpenFileDialogTrigger.Depth = 0;
            this.btnOpenFileDialogTrigger.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.btnOpenFileDialogTrigger.FlatAppearance.BorderSize = 0;
            this.btnOpenFileDialogTrigger.Font = new System.Drawing.Font("Roboto", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnOpenFileDialogTrigger.ForeColor = System.Drawing.Color.White;
            this.btnOpenFileDialogTrigger.Icon = null;
            this.btnOpenFileDialogTrigger.Location = new System.Drawing.Point(556, 420);
            this.btnOpenFileDialogTrigger.Margin = new System.Windows.Forms.Padding(2);
            this.btnOpenFileDialogTrigger.MouseState = MaterialSkin.MouseState.HOVER;
            this.btnOpenFileDialogTrigger.Name = "btnOpenFileDialogTrigger";
            this.btnOpenFileDialogTrigger.Primary = false;
            this.btnOpenFileDialogTrigger.Size = new System.Drawing.Size(91, 36);
            this.btnOpenFileDialogTrigger.TabIndex = 4;
            this.btnOpenFileDialogTrigger.Text = "Browse";
            this.btnOpenFileDialogTrigger.UseVisualStyleBackColor = false;
            this.btnOpenFileDialogTrigger.Click += new System.EventHandler(this.btnOpenFileDialogTrigger_Click);
            // 
            // btnInstall
            // 
            this.btnInstall.AllowDrop = true;
            this.btnInstall.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnInstall.BackColor = System.Drawing.Color.DimGray;
            this.btnInstall.Depth = 0;
            this.btnInstall.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.btnInstall.FlatAppearance.BorderSize = 0;
            this.btnInstall.Font = new System.Drawing.Font("Roboto", 13.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnInstall.ForeColor = System.Drawing.Color.White;
            this.btnInstall.Icon = null;
            this.btnInstall.Location = new System.Drawing.Point(213, 287);
            this.btnInstall.Margin = new System.Windows.Forms.Padding(2);
            this.btnInstall.MouseState = MaterialSkin.MouseState.HOVER;
            this.btnInstall.Name = "btnInstall";
            this.btnInstall.Primary = true;
            this.btnInstall.Size = new System.Drawing.Size(204, 68);
            this.btnInstall.TabIndex = 3;
            this.btnInstall.Text = "INSTALL";
            this.btnInstall.UseVisualStyleBackColor = false;
            this.btnInstall.Click += new System.EventHandler(this.btnInstaller_Click);
            // 
            // pgbStatus
            // 
            this.pgbStatus.Location = new System.Drawing.Point(8, 392);
            this.pgbStatus.Margin = new System.Windows.Forms.Padding(2);
            this.pgbStatus.Name = "pgbStatus";
            this.pgbStatus.Size = new System.Drawing.Size(615, 4);
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
            this.lnkAbout.Location = new System.Drawing.Point(552, 67);
            this.lnkAbout.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lnkAbout.Name = "lnkAbout";
            this.lnkAbout.Size = new System.Drawing.Size(44, 19);
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
            this.lnkHelp.Location = new System.Drawing.Point(603, 67);
            this.lnkHelp.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
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
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(646, 461);
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
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "Main";
            this.Text = "App Installer";
            this.ResumeLayout(false);
            this.PerformLayout();

        }


        #endregion

        internal Label lblStatus;
        internal MaterialSingleLineTextField txtFileLocation;
        internal ProgressBar pgbStatus;
        internal LinkLabel lnkAbout;
        internal LinkLabel lnkHelp;
        internal MaterialCheckBox chkReinstall;
        internal MaterialCheckBox chkForce;
        internal MaterialFlatButton btnOpenFileDialogTrigger;
        internal MaterialRaisedButton btnInstall;
    }
}