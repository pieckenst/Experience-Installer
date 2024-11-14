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
            try
            {
                if (isUEFI())
                {
                    Console.WriteLine("--Start of format-- ");
                    process.StartInfo.FileName = "diskpart.exe";
                    process.StartInfo.UseShellExecute = false;
                    process.StartInfo.CreateNoWindow = true;
                    process.StartInfo.RedirectStandardInput = true;
                    process.StartInfo.RedirectStandardOutput = true;
                    process.Start();

                    // Select the disk
                    process.StandardInput.WriteLine("select disk " + index);
                    Console.WriteLine("  Formatting Drive " + index);

                    // Clean the disk
                    process.StandardInput.WriteLine("clean");
                    Console.WriteLine("  Disk cleaned");

                    // Convert to GPT
                    process.StandardInput.WriteLine("convert gpt");
                    Console.WriteLine("  Converted to GPT");

                    // Create EFI partition
                    process.StandardInput.WriteLine("create part efi size=500");
                    Console.WriteLine("  Created EFI partition");

                    // Format EFI partition as FAT32
                    process.StandardInput.WriteLine("format fs=fat32");
                    Console.WriteLine("  Formatted EFI partition as FAT32");

                    // Assign letter to EFI partition
                    process.StandardInput.WriteLine("assign letter=s");
                    Console.WriteLine("  Assigned letter S to EFI partition");

                    // Create primary partition for Windows
                    process.StandardInput.WriteLine("create part pri");
                    Console.WriteLine("  Created primary partition for Windows");

                    // Format primary partition as NTFS
                    process.StandardInput.WriteLine("format fs=ntfs");
                    Console.WriteLine("  Formatted primary partition as NTFS");

                    // Assign letter to primary partition
                    process.StandardInput.WriteLine("assign letter=k");
                    Console.WriteLine("  Assigned letter K to primary partition");

                    // Exit diskpart
                    process.StandardInput.WriteLine("exit");
                    Console.WriteLine("  Exited diskpart");

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

                    // Select the disk
                    process.StandardInput.WriteLine("select disk " + index);
                    Console.WriteLine("  Formatting Drive " + index);

                    // Clean the disk
                    process.StandardInput.WriteLine("clean");
                    Console.WriteLine("  Disk cleaned");

                    // Create system partition
                    process.StandardInput.WriteLine("create partition primary size=100");
                    Console.WriteLine("  Created system partition");

                    // Format system partition as NTFS
                    process.StandardInput.WriteLine("format fs=ntfs label=System");
                    Console.WriteLine("  Formatted system partition as NTFS");

                    // Assign letter to system partition
                    process.StandardInput.WriteLine("assign letter=s");
                    Console.WriteLine("  Assigned letter S to system partition");

                    // Create primary partition for Windows
                    process.StandardInput.WriteLine("create partition primary");
                    Console.WriteLine("  Created primary partition for Windows");

                    // Shrink primary partition by 650 MB
                    process.StandardInput.WriteLine("shrink minimum=650");
                    Console.WriteLine("  Shrunk primary partition by 650 MB");

                    // Format primary partition as NTFS
                    process.StandardInput.WriteLine("format fs=ntfs label=Windows");
                    Console.WriteLine("  Formatted primary partition as NTFS");

                    // Assign letter to primary partition
                    process.StandardInput.WriteLine("assign letter=k");
                    Console.WriteLine("  Assigned letter K to primary partition");

                    // Create recovery partition
                    process.StandardInput.WriteLine("create partition primary");
                    Console.WriteLine("  Created recovery partition");

                    // Format recovery partition as NTFS
                    process.StandardInput.WriteLine("format fs=ntfs label=Recovery");
                    Console.WriteLine("  Formatted recovery partition as NTFS");

                    // Assign letter to recovery partition
                    process.StandardInput.WriteLine("assign letter=r");
                    Console.WriteLine("  Assigned letter R to recovery partition");

                    // Mark recovery partition as hidden
                    process.StandardInput.WriteLine("set id=27");
                    Console.WriteLine("  Marked recovery partition as hidden");

                    // Exit diskpart
                    process.StandardInput.WriteLine("exit");
                    Console.WriteLine("  Exited diskpart");

                    string outputs = process.StandardOutput.ReadToEnd();
                    process.WaitForExit();
                    Console.WriteLine(outputs);
                    Console.WriteLine("--End of format-- ");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred: " + ex.Message);
            }
        }

        public bool BCDRecords()
        {
            Process process = new Process();
            try
            {
                if (isUEFI())
                {
                    process.StartInfo.UseShellExecute = false;
                    process.StartInfo.CreateNoWindow = true;
                    process.StartInfo.RedirectStandardInput = true;
                    process.StartInfo.RedirectStandardOutput = true;
                    process.StartInfo.FileName = "bcdboot.exe";
                    process.StartInfo.Arguments = "k:\\Windows /s S: /f UEFI";
                    process.Start();
                    string outputs = process.StandardOutput.ReadToEnd();
                    process.WaitForExit();
                    Console.WriteLine(outputs);
                    return true;
                }
                else
                {
                    process.StartInfo.UseShellExecute = false;
                    process.StartInfo.CreateNoWindow = true;
                    process.StartInfo.RedirectStandardInput = true;
                    process.StartInfo.RedirectStandardOutput = true;
                    process.StartInfo.FileName = "bcdboot.exe";
                    process.StartInfo.Arguments = "k:\\windows /s S: /f BIOS";
                    process.Start();
                    string outputs = process.StandardOutput.ReadToEnd();
                    process.WaitForExit();
                    Console.WriteLine(outputs);
                    return true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred: " + ex.Message);
                return false;
            }
        }

        public int GetIndexOfDrive(string drive)
        {
            drive = drive.Replace(":", "").Replace(@"\", "");

            // execute DiskPart programmatically
            Process process = new Process();
            process.StartInfo.FileName = "diskpart.exe";
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.CreateNoWindow = true;
            process.StartInfo.RedirectStandardInput = true;
            process.StartInfo.RedirectStandardOutput = true;
            process.Start();
            process.StandardInput.WriteLine("list volume");
            process.StandardInput.WriteLine("exit");
            string outputs = process.StandardOutput.ReadToEnd();
            process.WaitForExit();

            // extract information from output
            string table = outputs.Split(new string[] { "DISKPART>" }, StringSplitOptions.None)[1];
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
            try
            {
                if (isUEFI())
                {
                    clearOutput = true;
                    diskPart(new List<String>(new String[] {
                        "sel disk " + disknumber,
                        "clean",
                        "convert gpt",
                        "create part efi size=500",
                        "format fs=fat32",
                        "assign letter=s",
                        "create part prim size=" + Gb * 1024,
                        "format fs=ntfs",
                        "assign letter=k"
                    }));
                }
                else
                {
                    clearOutput = true;
                    diskPart(new List<String>(new String[] {
                        "sel disk " + disknumber,
                        "clean",
                        "create partition primary size=100",
                        "format fs=ntfs label=System",
                        "assign letter=s",
                        "create partition primary",
                        "format fs=ntfs label=Windows",
                        "assign letter=k"
                    }));
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred: " + ex.Message);
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
                    Console.WriteLine("Executed command: " + command);
                }

                cmdProcess.StandardInput.WriteLine("exit");
                Console.WriteLine("Executed command: exit");
            }
            catch (Exception ex)
            {
                output.Text = ex.ToString();
                Console.WriteLine("An error occurred: " + ex.Message);
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
