using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;

namespace Windows_Installation
{
    class Diskpart
    {
        Process cmdProcess;
        TextBox output;
        bool clearOutput;

        public Diskpart(TextBox newOutput)
        {
            output = newOutput;
        }

        public void executeAfterExit(Cmd nextProcess)
        {
            this.cmdProcess.Exited += (s, e) =>
            {
                nextProcess.execute();
            };
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
        public void formatOnePartition(int index)
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

                process.StandardInput.WriteLine("format fs=ntfs");
                process.StandardInput.WriteLine("assign letter=k");



                process.StandardInput.WriteLine("exit");

               

                string output1 = process.StandardOutput.ReadToEnd();
                process.WaitForExit();
                Console.WriteLine("--End of format-- ");

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

                process.StandardInput.WriteLine("create partition primary ");
                Console.WriteLine("  Formatting System as NTFS " + index);

                process.StandardInput.WriteLine("format fs=ntfs label=System");
                process.StandardInput.WriteLine("assign letter=S");
                Console.WriteLine("  Creating Windows Partition");

                process.StandardInput.WriteLine("create partition primary");
                Console.WriteLine("  Removing 650 MB from Windows");

                process.StandardInput.WriteLine("shrink minimum=650");
                Console.WriteLine("  Formatting Windows as NTFS " + index);

                process.StandardInput.WriteLine("format fs=ntfs label=Windows");
                process.StandardInput.WriteLine("assign letter=K");
                Console.WriteLine("  Creating Recovery Partition");

                process.StandardInput.WriteLine("create partition primary");
                Console.WriteLine("  Formatting Recovery as NTFS " + index);

                process.StandardInput.WriteLine("format fs=ntfs label=Recovery");
                process.StandardInput.WriteLine("assign letter=R");
                Console.WriteLine("  Marking Recovery as Hidden " + index);

                process.StandardInput.WriteLine("set id=27");

                process.StandardInput.WriteLine("exit");
                


                string output = process.StandardOutput.ReadToEnd();
                process.WaitForExit();
                Console.WriteLine("--End of format-- ");
            }

        }

        public bool BCDRecords()
        {
            Process process = new Process();
            if (isUEFI())
            {

                process.StartInfo.UseShellExecute = false;
                process.StartInfo.CreateNoWindow = true;
                process.StartInfo.RedirectStandardInput = true;
                process.StartInfo.RedirectStandardOutput = true;
                process.StartInfo.FileName = "bcdboot.exe";
                process.StartInfo.Arguments = "k:\\Windows /s S:";
                process.Start();
                string output1 = process.StandardOutput.ReadToEnd();
                process.WaitForExit();
                return true;
            }
            else
            {

                process.StartInfo.UseShellExecute = false;
                process.StartInfo.CreateNoWindow = true;
                process.StartInfo.RedirectStandardInput = true;
                process.StartInfo.RedirectStandardOutput = true;
                process.StartInfo.FileName = "x:\\windows\\system32\\bcdboot.exe";
                process.StartInfo.Arguments = "k:\\windows /s S: /f ALL";
                process.Start();
                string output = process.StandardOutput.ReadToEnd();
                process.WaitForExit();
                return true;
            }
        }

        public int GetIndexOfDrive(string drive)
        {
            drive = drive.Replace(":", "").Replace(@"\", "");

            // execute DiskPart programatically
            Process process = new Process();
            process.StartInfo.FileName = "diskpart.exe";
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.CreateNoWindow = true;
            process.StartInfo.RedirectStandardInput = true;
            process.StartInfo.RedirectStandardOutput = true;
            process.Start();
            process.StandardInput.WriteLine("list volume");
            process.StandardInput.WriteLine("exit");
            string output = process.StandardOutput.ReadToEnd();
            process.WaitForExit();

            // extract information from output
            string table = output.Split(new string[] { "DISKPART>" }, StringSplitOptions.None)[1];
            var rows = table.Split(new string[] { "\n" }, StringSplitOptions.None);
            for (int i = 3; i < rows.Length; i++)
            {
                if (rows[i].Contains("Volume"))
                {
                    int index = Int32.Parse(rows[i].Split(new string[] { " " }, StringSplitOptions.None)[3]);
                    string label = rows[i].Split(new string[] { " " }, StringSplitOptions.None)[3];

                    if (label.Equals(drive))
                    {
                        return index;
                    }
                }
            }

            return -1;
        }


        public void formatTwoPartitions(int Gb, int disknumber)
        {
            if (isUEFI())
            {
                clearOutput = true;
                diskPart(new List<String>(new String[] {
                "sel disk" + disknumber,
                "clean",
                "convert gpt",
                "create part efi size=500",
                "format fs=fat32",
                "assign letter=s",

                "create part prim size=" + Gb * 1024,

                "format quick fs=ntfs",

                "ass letter=k"
            }));
            }
            else
            {
                clearOutput = true;
                diskPart(new List<String>(new String[] {
                "sel disk" + disknumber,
                "clean",

                "create partition primary size=100",
                "format quick fs=ntfs label=System",
                "assign letter=s",
                "create partition primary",
                "format quick fs=ntfs label=Windows",
                "act",
                "ass letter=k"
            }));
            }

        }

        public void diskInfo()
        {
            clearOutput = false;
            diskPart(new List<String>(new String[] {
                "list disk",
                "list vol"
            }));
        }

        private void diskPart(List<String> commands)
        {
            ProcessStartInfo cmdStartInfo = new ProcessStartInfo();
            cmdStartInfo.FileName = @"X:\Windows\System32\diskpart.exe";

            cmdStartInfo.RedirectStandardOutput = true;
            cmdStartInfo.RedirectStandardInput = true;
            cmdStartInfo.UseShellExecute = false;
            cmdStartInfo.CreateNoWindow = true;

            cmdProcess = new Process();
            cmdProcess.StartInfo = cmdStartInfo;
            cmdProcess.OutputDataReceived += cmd_DataReceived;
            cmdProcess.EnableRaisingEvents = true;

            try
            {
                cmdProcess.Start();
                cmdProcess.BeginOutputReadLine();

                foreach (String command in commands) {
                    cmdProcess.StandardInput.WriteLine(command);
                }

                cmdProcess.StandardInput.WriteLine("exit");     
            }

            catch (Exception ex)
            {
                output.Text = ex.ToString();
            }
        }

        private void cmd_DataReceived(object sender, DataReceivedEventArgs e)
        {
            if (e.Data != null)
            {
                output.Dispatcher.BeginInvoke(new Action(() => {
                    if (clearOutput) output.Text = "";
                    output.Text += e.Data.Trim() + Environment.NewLine; 
                }));
            }     

        }


    }
}
