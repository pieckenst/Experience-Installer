using System;
using System.Collections.Generic;
using System.Diagnostics;
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
        public void formatOnePartition(int disknumber)
        { 
            if(isUEFI())
            {
                clearOutput = true;
                diskPart(new List<String>(new String[] {
                "sel disk" + disknumber,
                "clean",
                "convert gpt",
                "create part efi size=500",
                "format fs=fat32",
                "assign letter=s",
                "format quick fs=ntfs",
                "act",
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

        public bool BCDRecords()
        {
            Process process = new Process();
            if (isUEFI())
            {

                process.StartInfo.UseShellExecute = false;
                process.StartInfo.CreateNoWindow = true;
                process.StartInfo.RedirectStandardInput = true;
                process.StartInfo.RedirectStandardOutput = true;
                process.StartInfo.FileName = "x:\\windows\\system32\\bcdboot.exe";
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
