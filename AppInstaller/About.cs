using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Windows.Forms;

namespace APKInstaller
{
    public partial class About : Form
    {
        public About()
        {
            InitializeComponent();
        }

        void About_Load(object sender, EventArgs e)
        {
            Text = $"About {AssemblyTitle}";
            //this.LabelProductName.Text = AssemblyProduct;
            LabelVersion.Text = $"Version {AssemblyVersion}";
            LabelCopyright.Text = AssemblyCopyright;
            LabelCompanyName.Text = AssemblyCompany;
            TextBoxDescription.Text = AssemblyDescription;

            // todo: this.chkPrerelease.Checked = MySettingsProperty.Settings.prerelease;
            chkPrerelease.Location = new Point(131, 0);
            lblUpdateStatus.Text = AppUpdateManager.UpdateStatusText;
        }

        void chkPrerelease_CheckedChanged(object sender, EventArgs e)
        {
            // todo: MySettingsProperty.Settings.prerelease = this.chkPrerelease.Checked;
            AppUpdateManager.UpdateApp();
        }

        void OKButton_Click(object sender, EventArgs e)
        {
            Close();
        }

        void LogoPictureBox_Click(object sender, EventArgs e)
        {
            UseWaitCursor = true;
            Process.Start("http://www.firstinspires.org/robotics/ftc");
            UseWaitCursor = false;
        }

        #region Assembly Attribute Accessors

        public static string AssemblyTitle
        {
            get
            {
                var attributes = Assembly.GetExecutingAssembly()
                    .GetCustomAttributes(typeof(AssemblyTitleAttribute), false);
                if (attributes.Length <= 0)
                    return Path.GetFileNameWithoutExtension(Assembly.GetExecutingAssembly().CodeBase);
                var titleAttribute = (AssemblyTitleAttribute) attributes[0];
                return titleAttribute.Title != "" ? titleAttribute.Title : Path.GetFileNameWithoutExtension(Assembly.GetExecutingAssembly().CodeBase);
            }
        }

        public static string AssemblyVersion => Assembly.GetExecutingAssembly().GetName().Version.ToString();

        public static string AssemblyDescription
        {
            get
            {
                var attributes =
                    Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyDescriptionAttribute), false);
                return attributes.Length == 0 ? "" : ((AssemblyDescriptionAttribute) attributes[0]).Description;
            }
        }

        public static string AssemblyProduct
        {
            get
            {
                var attributes = Assembly.GetExecutingAssembly()
                    .GetCustomAttributes(typeof(AssemblyProductAttribute), false);
                return attributes.Length == 0 ? "" : ((AssemblyProductAttribute) attributes[0]).Product;
            }
        }

        public static string AssemblyCopyright
        {
            get
            {
                var attributes = Assembly.GetExecutingAssembly()
                    .GetCustomAttributes(typeof(AssemblyCopyrightAttribute), false);
                return attributes.Length == 0 ? "" : ((AssemblyCopyrightAttribute) attributes[0]).Copyright;
            }
        }

        public static string AssemblyCompany
        {
            get
            {
                var attributes = Assembly.GetExecutingAssembly()
                    .GetCustomAttributes(typeof(AssemblyCompanyAttribute), false);
                return attributes.Length == 0 ? "" : ((AssemblyCompanyAttribute) attributes[0]).Company;
            }
        }

        #endregion
    }
}