using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using APKInstaller.i18n;
//using Microsoft.VisualBasic;

namespace APKInstaller
{
    public partial class MultiPackageDialog : Form
    {
        private readonly string[] _files;
        private bool _modifying;

        public MultiPackageDialog(string[] files)
        {
            InitializeComponent();
            _files = files;
        }



        public static MultiPackageDialog Create(string[] files)
        {
            return new MultiPackageDialog(files);
        }

        private void MultiPackageDialog_Load(object sender, EventArgs e)
        {
            lstFiles.Items.AddRange(_files.ToArray());

            //Configure GUI
            //Dim manager = MaterialSkinManager.Instance
            //manager.AddFormToManage(Me)
            //SkinManager.Theme = MaterialSkinManager.Themes.LIGHT
            //S 'kinManager.ColorScheme = New ColorScheme(Primary.Red800, Primary.Red800, Primary.Red200, Accent.LightBlue200, TextShade.WHITE)
            CenterToScreen();

            btnDelete.Enabled = false;
            btnModify.Enabled = false;
            lstFiles.DrawMode = DrawMode.OwnerDrawFixed;
        }

        public string[] GetFiles()
        {
            string[] list = new string[lstFiles.Items.Count + 1];
            lstFiles.Items.CopyTo(list, 0);
            return list;
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
            Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you really sure you want to cancel? None of the changes made will be preserved.", "Confirm Cancel", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
            {
                DialogResult = DialogResult.Cancel;
                Close();
            }
        }

        private void lstFiles_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lstFiles.SelectedIndex >= 0)
            {
                btnModify.Enabled = true;
                btnDelete.Enabled = true;
            }
            else
            {
                btnModify.Enabled = false;
                btnDelete.Enabled = false;
            }

            //modifying = True
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {

            if (_modifying & lstFiles.SelectedIndex >= 0)
            {
                //btnAdd.Text = "Add"
                //modifying = False
                //tnBrowse.Visible = True
            }

            lstFiles.Items.Add(txtFile.Text);
            lstFiles.SelectedIndex = lstFiles.Items.Count - 1;
            lstFiles.Enabled = true;

            txtFile.Text = "";
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            lstFiles.Items.RemoveAt(lstFiles.SelectedIndex);
        }


        private void listBox1_DrawItem(object sender, DrawItemEventArgs e)
        {
            e.DrawBackground();

            dynamic i = 0;
            dynamic g = lstFiles.CreateGraphics();
            foreach (var item in lstFiles.Items)
            {
                //item = item_loopVariable;
                dynamic sizeF = g.MeasureString(item.ToString(), lstFiles.Font);
                if (sizeF.Width > i)
                {
                    i = Convert.ToInt32(sizeF.Width);
                }
            }
            lstFiles.HorizontalExtent = i;

            using (SolidBrush b = new SolidBrush(e.ForeColor))
            {
                if (e.Index >= 0)
                {
                    dynamic itemText = lstFiles.GetItemText(lstFiles.Items[e.Index]);
                    //If Not Installer.ValidateFile(itemText) Then
                    //    e.Graphics.FillRectangle(Brushes.OrangeRed, e.Bounds)
                    //End If
                    if ((e.State & DrawItemState.Selected) == DrawItemState.Selected)
                    {
                        e.Graphics.FillRectangle(Brushes.Aqua, e.Bounds);
                    }
                    e.Graphics.DrawString(itemText, e.Font, b, e.Bounds);
                }
            }

            e.DrawFocusRectangle();
        }

        private void btnModify_Click(object sender, EventArgs e)
        {
            if (lstFiles.SelectedIndex >= 0)
            {
                if (_modifying)
                {
                    lstFiles.Items.RemoveAt(lstFiles.SelectedIndex);
                    lstFiles.Items.Add(txtFile.Text);
                    lstFiles.SelectedIndex = lstFiles.Items.Count - 1;

                    lstFiles.Enabled = true;
                    btnModify.Text = UIStrings.Modify;
                    _modifying = false;
                }
                else
                {
                    txtFile.Text = lstFiles.SelectedItem.ToString();
                    lstFiles.Enabled = false;
                    btnModify.Text = UIStrings.Confirm;
                    _modifying = true;
                }
            }
        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            using (var fileDialog = new OpenFileDialog())
            {

                fileDialog.AutoUpgradeEnabled = true;
                fileDialog.CheckFileExists = true;
                fileDialog.DefaultExt = ".apk";
                fileDialog.Title = UIStrings.openFileDialogTitle;
                fileDialog.Multiselect = false;
                fileDialog.ValidateNames = true;
                fileDialog.Filter = UIStrings.openFileDialogFilter;
                fileDialog.ShowDialog();

                txtFile.Text = fileDialog.FileName;
            }
        }

        private void txtFile_TextChanged(object sender, EventArgs e)
        {
            dynamic validateFile = Installer.ValidateFile(txtFile.Text);
            btnAdd.Enabled = validateFile;
            if (_modifying)
            {
                btnModify.Enabled = validateFile;
            }
        }

        private void MultiPackageDialog_Load_1(object sender, EventArgs e)
        {

        }

        //=======================================================
        //Service provided by Telerik (www.telerik.com)
        //Conversion powered by NRefactory.
        //Twitter: @telerik
        //Facebook: facebook.com/telerik
        //=======================================================

    }
}
