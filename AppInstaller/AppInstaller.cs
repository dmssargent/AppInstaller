using System;
using System.Windows.Forms;

namespace APKInstaller
{
    public class AppInstaller
    {
        [STAThread]
        public static void Main(string[] args)
        {
            Application.Run(new Main());
        }
    }
}
