using System;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Controls;

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
            try
            {
                Console.WriteLine("Attempting to reboot...");
                Cmd reboot = new Cmd("wpeutil", "reboot");
                reboot.execute();
                Console.WriteLine("Reboot command executed successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error during reboot: {ex.Message}");
            }
        }

        private void btnWimInfo_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Console.WriteLine("Fetching WIM info...");
                output.Text = "";

                Cmd wimInfo = new Cmd("Dism", "/Get-ImageInfo /imagefile:" + txtWimPath.Text);
                wimInfo.attachLabel(output);
                wimInfo.disableClearOutput();
                wimInfo.execute();
                Console.WriteLine("WIM info fetched successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching WIM info: {ex.Message}");
            }
        }

        private void btnApply_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Console.WriteLine("Starting installation process...");
                Diskpart diskPart = new Diskpart(output);
                int indexsel = int.Parse(txtdisknum.Text);
                if (Install(indexsel))
                {
                    pgrApplyProgress.Value = 50;
                    Console.WriteLine(" (3/4) Adding BCD Boot Records");
                    if (diskPart.BCDRecords())
                    {
                        pgrApplyProgress.Value = 100;
                        Console.WriteLine("Windows has been deployed!");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error during installation: {ex.Message}");
            }
        }

        public bool Install(int index)
        {
            try
            {
                Console.WriteLine("Applying image...");
                string textpathl = txtWimPath.Text;
                using (Process process = new Process())
                {
                    process.StartInfo.FileName = "dism.exe";
                    process.StartInfo.Arguments = $"/apply-image /imagefile:{textpathl} /index:{index} /ApplyDir:K:\\";
                    process.StartInfo.UseShellExecute = false;
                    process.StartInfo.CreateNoWindow = true;
                    process.StartInfo.RedirectStandardInput = true;
                    process.StartInfo.RedirectStandardOutput = true;
                    process.Start();

                    string output = process.StandardOutput.ReadToEnd();
                    process.WaitForExit();
                    Console.WriteLine(output);

                    if (process.ExitCode != 0)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine($"An Error occurred while applying the image. \n\n DISM error {process.ExitCode}");
                        Console.ForegroundColor = ConsoleColor.White;
                        return false;
                    }
                }
                Console.WriteLine("Image applied successfully.");
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error applying image: {ex.Message}");
                return false;
            }
        }

        private void txtWimPath_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                Console.WriteLine("Checking WIM path...");
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
                Console.WriteLine("WIM path checked successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error checking WIM path: {ex.Message}");
            }
        }
    }
}
