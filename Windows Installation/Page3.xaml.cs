using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Windows_Installation
{
    /// <summary>
    /// Логика взаимодействия для Page3.xaml
    /// </summary>
    public partial class Page3 : Page
    {
        public Page3()
        {
            InitializeComponent();
        }
        private void btnReboot_Click(object sender, RoutedEventArgs e)
        {
            Cmd reboot = new Cmd("wpeutil", "reboot");
            reboot.execute();
        }
        private void btnWimInfo_Click(object sender, RoutedEventArgs e)
        {
            output.Text = "";

            Cmd wimInfo = new Cmd("Dism", "/Get-ImageInfo /imagefile:" + txtWimPath.Text);
            wimInfo.attachLabel(output);
            wimInfo.disableClearOutput();
            wimInfo.execute();
        }
        private void btnApply_Click(object sender, RoutedEventArgs e)
        {
            Diskpart diskPart = new Diskpart(output);
            int indexsel = int.Parse(txtdisknum.Text);
            if (Install(indexsel))
            {
                Console.WriteLine(" (3/4) Adding BCD Boot Records");
                if (diskPart.BCDRecords())
                {
                    Console.WriteLine("Windows has been deployed!");
                }
            }
        }

        public bool Install(int index)
        {
            string textpathl = txtWimPath.Text;
            Process process = new Process();
            process.StartInfo.FileName = "dism.exe";
            process.StartInfo.Arguments = "/apply-image /imagefile:"+  textpathl +" /index:"  +index +" /ApplyDir:K:\\";
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.CreateNoWindow = true;
            process.StartInfo.RedirectStandardInput = true;
            process.StartInfo.RedirectStandardOutput = true;
            process.Start();



            string output = process.StandardOutput.ReadToEnd();
            process.WaitForExit();
            if (process.ExitCode != 0)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($" An Error occured while applying the image. \n\n DISM error {process.ExitCode}");
                Console.ForegroundColor = ConsoleColor.White;

                return false;
            }

            return true;
        }
        private void txtWimPath_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            if (Directory.Exists(txtWimPath.Text))
            {
                lstWims.Items.Clear();

                foreach (string directory in Directory.GetFiles(txtWimPath.Text, "*.wim"))
                {
                    lstWims.Items.Add(directory);
                }

                if (lstWims.Items.Count > 0)
                {
                    lstWims.SelectedIndex = 0;
                }
            }
        }

        
    }
}

