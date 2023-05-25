using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
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
using System.Xml;

namespace Windows_Installation
{
    /// <summary>
    /// Логика взаимодействия для Page2.xaml
    /// </summary>
    public partial class Page2 : Page
    {
        public Page2()
        {
            InitializeComponent();
            
        }
        private void btnDiskInfo_Click(object sender, RoutedEventArgs e)
        {
            output.Text = "";
            new Diskpart(output).diskInfo();

        }



        public bool isUEFI()
        {
            if (Environment.GetEnvironmentVariable("firmware_type") == "UEFI" || Environment.GetEnvironmentVariable("firmware_type") == "EFI")
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool Format(int index)
        {
            Process process = new Process();
            if (isUEFI())
            {
                Console.WriteLine("--Start of format-- ");
                process.StartInfo.FileName = "diskpart.exe";
                process.StartInfo.UseShellExecute = false;
                process.StartInfo.CreateNoWindow = true;
                process.StartInfo.RedirectStandardInput = true;
                process.StartInfo.RedirectStandardOutput = true;
                process.Start();
                process.StandardInput.WriteLine("select disk " + index);
                Console.WriteLine("  Formatting Drive " + index);
                process.StandardInput.WriteLine("clean");
                Console.WriteLine("  Converting to GPT");

                process.StandardInput.WriteLine("convert gpt");
                Console.WriteLine("  Creating EFI Partition");

                process.StandardInput.WriteLine("create part efi size=500");
                Console.WriteLine("  Formatting EFI as FAT32");

                process.StandardInput.WriteLine("format fs=fat32");
                process.StandardInput.WriteLine("assign letter=s");
                Console.WriteLine("  Creating Windows Partition");

                process.StandardInput.WriteLine("create part pri");
                Console.WriteLine("  Formatting Windows Partition");

                process.StandardInput.WriteLine("format quick fs=ntfs");
                process.StandardInput.WriteLine("assign letter=w");



                process.StandardInput.WriteLine("exit");

                Console.WriteLine("--End of format-- ");

                string output1 = process.StandardOutput.ReadToEnd();
                process.WaitForExit();
                return true;
            }
            else
            {
                Console.WriteLine("--Start of format-- ");

                process.StartInfo.FileName = "diskpart.exe";
                process.StartInfo.UseShellExecute = false;
                process.StartInfo.CreateNoWindow = true;
                process.StartInfo.RedirectStandardInput = true;
                process.StartInfo.RedirectStandardOutput = true;
                process.Start();
                process.StandardInput.WriteLine("select disk " + index);
                Console.WriteLine("  Formatting Drive " + index);
                process.StandardInput.WriteLine("clean");

                Console.WriteLine("  Creating System Partition");

                process.StandardInput.WriteLine("create partition primary size=100");
                Console.WriteLine("  Formatting System as NTFS " + index);

                process.StandardInput.WriteLine("format quick fs=ntfs label=System");
                process.StandardInput.WriteLine("assign letter=S");
                Console.WriteLine("  Creating Windows Partition");

                process.StandardInput.WriteLine("create partition primary");
                Console.WriteLine("  Removing 650 MB from Windows");

                process.StandardInput.WriteLine("shrink minimum=650");
                Console.WriteLine("  Formatting Windows as NTFS " + index);

                process.StandardInput.WriteLine("format quick fs=ntfs label=Windows");
                process.StandardInput.WriteLine("assign letter=W");
                Console.WriteLine("  Creating Recovery Partition");

                process.StandardInput.WriteLine("create partition primary");
                Console.WriteLine("  Formatting Recovery as NTFS " + index);

                process.StandardInput.WriteLine("format quick fs=ntfs label=Recovery");
                process.StandardInput.WriteLine("assign letter=R");
                Console.WriteLine("  Marking Recovery as Hidden " + index);

                process.StandardInput.WriteLine("set id=27");

                process.StandardInput.WriteLine("exit");
                Console.WriteLine("--End of format-- ");


                string output = process.StandardOutput.ReadToEnd();
                process.WaitForExit();
                return true;
            }
        }




        private void Button_Click(object sender, RoutedEventArgs events)
        {
            Diskpart diskPart = new Diskpart(output);

            if ((bool)cOnePartition.IsChecked)
            {
                if (Format(int.Parse(txtdisknum.Text)))
                {
                    MessageBox.Show("Format completed");
                }
                    
            }
            else
            {
                if (Format(int.Parse(txtdisknum.Text)))
                {
                    MessageBox.Show("Format completed");
                }
            }

            
            
        }

        

        private void NextButton_Click(object sender, RoutedEventArgs e)
        {
            Page3 secPage = new Page3();
            NavigationService.Navigate(secPage);
        }
    }
    
}
