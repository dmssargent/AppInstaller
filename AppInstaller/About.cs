// Decompiled with JetBrains decompiler
// Type: APKInstaller.About
// Assembly: AppInstaller, Version=0.1.7.1, Culture=neutral, PublicKeyToken=null
// MVID: 47EE20FC-A5A0-42AE-9843-329E79F78F9B
// Assembly location: C:\Users\dmssa_000\Documents\Visual Studio 2015\Projects\APKInstallerMono\APKInstaller\bin\Debug\AppInstaller.exe

//using Microsoft.VisualBasic.CompilerServices;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Windows.Forms;

namespace APKInstaller
{

    public partial class About : Form { 
  //public partial class About
  //{
  //  private IContainer components;
  //    private PictureBox _LogoPictureBox;
  //    private Button _OKButton;
  //    private CheckBox _chkPrerelease;

  //    internal virtual TableLayoutPanel TableLayoutPanel { get; [MethodImpl(MethodImplOptions.Synchronized)] set; }

  //  internal virtual TextBox TextBoxDescription { get; [MethodImpl(MethodImplOptions.Synchronized)] set; }

  //  internal virtual Label LabelVersion { get; [MethodImpl(MethodImplOptions.Synchronized)] set; }

  //  internal virtual Label LabelCompanyName { get; [MethodImpl(MethodImplOptions.Synchronized)] set; }

  //  internal virtual Button OKButton
  //  {
  //    get
  //    {
  //      return this._OKButton;
  //    }
  //    [MethodImpl(MethodImplOptions.Synchronized)] set
  //    {
  //      EventHandler eventHandler = new EventHandler(this.OKButton_Click);
  //      Button button1 = this._OKButton;
  //      if (button1 != null)
  //        button1.Click -= eventHandler;
  //      this._OKButton = value;
  //      Button button2 = this._OKButton;
  //      if (button2 == null)
  //        return;
  //      button2.Click += eventHandler;
  //    }
  //  }

  //  internal virtual Label LabelCopyright { get; [MethodImpl(MethodImplOptions.Synchronized)] set; }

  //  internal virtual Panel Panel1 { get; [MethodImpl(MethodImplOptions.Synchronized)] set; }

  //  internal virtual Label Label1 { get; [MethodImpl(MethodImplOptions.Synchronized)] set; }

  //  internal virtual CheckBox chkPrerelease
  //  {
  //    get
  //    {
  //      return this._chkPrerelease;
  //    }
  //    [MethodImpl(MethodImplOptions.Synchronized)] set
  //    {
  //      EventHandler eventHandler = new EventHandler(this.chkPrerelease_CheckedChanged);
  //      CheckBox checkBox1 = this._chkPrerelease;
  //      if (checkBox1 != null)
  //        checkBox1.CheckedChanged -= eventHandler;
  //      this._chkPrerelease = value;
  //      CheckBox checkBox2 = this._chkPrerelease;
  //      if (checkBox2 == null)
  //        return;
  //      checkBox2.CheckedChanged += eventHandler;
  //    }
  //  }

  //  internal virtual Label lblUpdateStatus { get; [MethodImpl(MethodImplOptions.Synchronized)] set; }

  //  internal  PictureBox LogoPictureBox
  //  {
  //    get
  //    {
  //      return this._LogoPictureBox;
  //    }
  //    set
  //    {
  //      EventHandler eventHandler = new EventHandler(this.LogoPictureBox_Click);
  //      PictureBox pictureBox1 = this._LogoPictureBox;
  //      if (pictureBox1 != null)
  //        pictureBox1.DoubleClick -= eventHandler;
  //      this._LogoPictureBox = value;
  //      PictureBox pictureBox2 = this._LogoPictureBox;
  //      if (pictureBox2 == null)
  //        return;
  //      pictureBox2.DoubleClick += eventHandler;
  //    }
  //  }

  //  internal virtual Panel Panel2 { get; [MethodImpl(MethodImplOptions.Synchronized)] set; }

  //  public About()
  //  {
  //    this.Load += new EventHandler(this.About_Load);
  //    this.InitializeComponent();
  //  }

  //  [DebuggerNonUserCode]
  //  protected override void Dispose(bool disposing)
  //  {
  //    try
  //    {
  //      if (!disposing || this.components == null)
  //        return;
  //      this.components.Dispose();
  //    }
  //    finally
  //    {
  //      base.Dispose(disposing);
  //    }
  //  }

  //  [DebuggerStepThrough]
  //  private void InitializeComponent()
  //  {
  //    ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (About));
  //    this.TableLayoutPanel = new TableLayoutPanel();
  //    this.LabelCopyright = new Label();
  //    this.LabelCompanyName = new Label();
  //    this.Panel1 = new Panel();
  //    this.lblUpdateStatus = new Label();
  //    this.chkPrerelease = new CheckBox();
  //    this.LabelVersion = new Label();
  //    this.Panel2 = new Panel();
  //    this.TextBoxDescription = new TextBox();
  //    this.OKButton = new Button();
  //    this.LogoPictureBox = new PictureBox();
  //    this.TableLayoutPanel.SuspendLayout();
  //    this.Panel1.SuspendLayout();
  //    this.Panel2.SuspendLayout();
  //    ((ISupportInitialize) this.LogoPictureBox).BeginInit();
  //    this.SuspendLayout();
  //    this.TableLayoutPanel.BackColor = Color.White;
  //    componentResourceManager.ApplyResources((object) this.TableLayoutPanel, "TableLayoutPanel");
  //    this.TableLayoutPanel.Controls.Add((Control) this.LabelCopyright, 1, 1);
  //    this.TableLayoutPanel.Controls.Add((Control) this.LabelCompanyName, 1, 2);
  //    this.TableLayoutPanel.Controls.Add((Control) this.Panel1, 1, 0);
  //    this.TableLayoutPanel.Controls.Add((Control) this.Panel2, 1, 3);
  //    this.TableLayoutPanel.Controls.Add((Control) this.LogoPictureBox, 0, 3);
  //    this.TableLayoutPanel.Name = "TableLayoutPanel";
  //    componentResourceManager.ApplyResources((object) this.LabelCopyright, "LabelCopyright");
  //    this.LabelCopyright.ForeColor = Color.FromArgb(222, 0, 0, 0);
  //    this.LabelCopyright.Name = "LabelCopyright";
  //    componentResourceManager.ApplyResources((object) this.LabelCompanyName, "LabelCompanyName");
  //    this.LabelCompanyName.ForeColor = Color.FromArgb(222, 0, 0, 0);
  //    this.LabelCompanyName.Name = "LabelCompanyName";
  //    this.Panel1.Controls.Add((Control) this.lblUpdateStatus);
  //    this.Panel1.Controls.Add((Control) this.chkPrerelease);
  //    this.Panel1.Controls.Add((Control) this.LabelVersion);
  //    componentResourceManager.ApplyResources((object) this.Panel1, "Panel1");
  //    this.Panel1.Name = "Panel1";
  //    componentResourceManager.ApplyResources((object) this.lblUpdateStatus, "lblUpdateStatus");
  //    this.lblUpdateStatus.ForeColor = Color.FromArgb(222, 0, 0, 0);
  //    this.lblUpdateStatus.Name = "lblUpdateStatus";
  //    componentResourceManager.ApplyResources((object) this.chkPrerelease, "chkPrerelease");
  //    this.chkPrerelease.Name = "chkPrerelease";
  //    this.chkPrerelease.UseVisualStyleBackColor = true;
  //    componentResourceManager.ApplyResources((object) this.LabelVersion, "LabelVersion");
  //    this.LabelVersion.ForeColor = Color.FromArgb(222, 0, 0, 0);
  //    this.LabelVersion.Name = "LabelVersion";
  //    this.Panel2.Controls.Add((Control) this.TextBoxDescription);
  //    this.Panel2.Controls.Add((Control) this.OKButton);
  //    componentResourceManager.ApplyResources((object) this.Panel2, "Panel2");
  //    this.Panel2.Name = "Panel2";
  //    this.TextBoxDescription.BackColor = Color.White;
  //    this.TextBoxDescription.BorderStyle = BorderStyle.None;
  //    this.TextBoxDescription.Cursor = Cursors.Arrow;
  //    componentResourceManager.ApplyResources((object) this.TextBoxDescription, "TextBoxDescription");
  //    this.TextBoxDescription.Name = "TextBoxDescription";
  //    this.TextBoxDescription.ReadOnly = true;
  //    this.TextBoxDescription.TabStop = false;
  //    componentResourceManager.ApplyResources((object) this.OKButton, "OKButton");
  //    this.OKButton.DialogResult = DialogResult.Cancel;
  //    this.OKButton.Name = "OKButton";
  //    componentResourceManager.ApplyResources((object) this.LogoPictureBox, "LogoPictureBox");
  //    this.LogoPictureBox.Image = (Image) APKInstaller.My.Resources.Resources.FIRSTTech_IconVert_RGB;
  //    this.LogoPictureBox.Name = "LogoPictureBox";
  //    this.LogoPictureBox.TabStop = false;
  //    componentResourceManager.ApplyResources((object) this, "$this");
  //    this.AutoScaleMode = AutoScaleMode.Font;
  //    this.CancelButton = (IButtonControl) this.OKButton;
  //    this.Controls.Add((Control) this.TableLayoutPanel);
  //    this.MaximizeBox = false;
  //    this.MinimizeBox = false;
  //    this.Name = "About";
  //    this.ShowInTaskbar = false;
  //    this.TableLayoutPanel.ResumeLayout(false);
  //    this.Panel1.ResumeLayout(false);
  //    this.Panel1.PerformLayout();
  //    this.Panel2.ResumeLayout(false);
  //    this.Panel2.PerformLayout();
  //    ((ISupportInitialize) this.LogoPictureBox).EndInit();
  //    this.ResumeLayout(false);
  //  }

        public About()
        {
            InitializeComponent();
        }

    private void About_Load(object sender, EventArgs e)
    {
      //this.Text = string.Format("About {0}", (uint) Operators.CompareString(MyProject.Application.Info.Title, "", true) <= 0U ? (object) Path.GetFileNameWithoutExtension(MyProject.Application.Info.AssemblyName) : (object) MyProject.Application.Info.Title);
      //this.LabelVersion.Text = string.Format("Version {0}", (object) MyProject.Application.Info.Version.ToString());
      //this.LabelCopyright.Text =;
      //this.LabelCompanyName.Text = Strings.Built_by + Application.CompanyName;
      //this.TextBoxDescription.Font = this.LabelVersion.Font;
      //this.TextBoxDescription.Text = "";

            this.Text = String.Format("About {0}", AssemblyTitle);
            //this.LabelProductName.Text = AssemblyProduct;
            this.LabelVersion.Text = String.Format("Version {0}", AssemblyVersion);
            this.LabelCopyright.Text = AssemblyCopyright;
            this.LabelCompanyName.Text = AssemblyCompany;
            this.TextBoxDescription.Text = AssemblyDescription;

            // todo: this.chkPrerelease.Checked = MySettingsProperty.Settings.prerelease;
      this.chkPrerelease.Location = new Point(131, 0);
      this.lblUpdateStatus.Text = AppUpdateManager.UpdateStatusText;
    }

    private void chkPrerelease_CheckedChanged(object sender, EventArgs e)
    {
      // todo: MySettingsProperty.Settings.prerelease = this.chkPrerelease.Checked;
      AppUpdateManager.UpdateApp();
    }

    private void OKButton_Click(object sender, EventArgs e)
    {
      this.Close();
    }

    private void LogoPictureBox_Click(object sender, EventArgs e)
    {
      this.UseWaitCursor = true;
      Process.Start("http://www.firstinspires.org/robotics/ftc");
      this.UseWaitCursor = false;
    }

        #region Assembly Attribute Accessors

        public static string AssemblyTitle
        {
            get
            {
                object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyTitleAttribute), false);
                if (attributes.Length > 0)
                {
                    AssemblyTitleAttribute titleAttribute = (AssemblyTitleAttribute)attributes[0];
                    if (titleAttribute.Title != "")
                    {
                        return titleAttribute.Title;
                    }
                }
                return System.IO.Path.GetFileNameWithoutExtension(Assembly.GetExecutingAssembly().CodeBase);
            }
        }

        public static string AssemblyVersion
        {
            get
            {
                return Assembly.GetExecutingAssembly().GetName().Version.ToString();
            }
        }

        public static string AssemblyDescription
        {
            get
            {
                object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyDescriptionAttribute), false);
                if (attributes.Length == 0)
                {
                    return "";
                }
                return ((AssemblyDescriptionAttribute)attributes[0]).Description;
            }
        }

        public static string AssemblyProduct
        {
            get
            {
                object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyProductAttribute), false);
                if (attributes.Length == 0)
                {
                    return "";
                }
                return ((AssemblyProductAttribute)attributes[0]).Product;
            }
        }

        public static string AssemblyCopyright
        {
            get
            {
                object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyCopyrightAttribute), false);
                if (attributes.Length == 0)
                {
                    return "";
                }
                return ((AssemblyCopyrightAttribute)attributes[0]).Copyright;
            }
        }

        public static string AssemblyCompany
        {
            get
            {
                object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyCompanyAttribute), false);
                if (attributes.Length == 0)
                {
                    return "";
                }
                return ((AssemblyCompanyAttribute)attributes[0]).Company;
            }
        }

        #endregion

        //private void InitializeComponent()
        //{
        //    System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(About));
        //    this.SuspendLayout();
        //    // 
        //    // About
        //    // 
        //    resources.ApplyResources(this, "$this");
        //    this.Name = "About";
        //    this.ResumeLayout(false);

        //}
    }
}
