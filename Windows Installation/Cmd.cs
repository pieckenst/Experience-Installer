using System;
using System.Diagnostics;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows;
using System.Windows.Controls;

class Cmd
{
    Label lblOutput;
    ProgressBar progressBar;

    string command;
    string commandOptions;

    Process proc;

    bool clearOutput = true;

    public Cmd(string command, string commandOptions)
    {
        this.command = command;
        this.commandOptions = commandOptions;
    }

    public void attachLabel(Label label)
    {
        this.lblOutput = label;
    }

    public void attachProgressBar(ProgressBar progressBar)
    {
        this.progressBar = progressBar;
    }

    public void setClearOutput(bool clear)
    {
        this.clearOutput = clear;
    }

    public void execute()
    {
        Thread thread = new Thread(new ThreadStart(WorkThreadFunction));
        thread.Start();
    }

    public void kill()
    {
        try
        {
            proc.Kill();
        }
        catch (Exception ex)
        {
            Console.Write(ex.ToString());
        }
    }

    private void WorkThreadFunction()
    {
        try
        {
            ExecuteCommandSync();
        }
        catch (Exception ex)
        {
            MessageBox.Show("Der Vorgang konnte nicht ausgeführt werden. Fehlermeldung: " + ex.ToString());
        }
    }

    private void ExecuteCommandSync()
    {
        try
        {
            var procStartInfo = new System.Diagnostics.ProcessStartInfo(command, commandOptions);
            procStartInfo.RedirectStandardOutput = true;
            procStartInfo.UseShellExecute = false;
            procStartInfo.CreateNoWindow = true;
            proc = new Process();

            proc.OutputDataReceived += (s, e) =>
            {
                lblOutput.Dispatcher.BeginInvoke((Action)(() =>
                {
                    if ((e.Data != null) && (lblOutput != null))
                    {
                        if ((clearOutput) && (e.Data != "")) lblOutput.Content = e.Data;
                        else lblOutput.Content += e.Data + "\n";

                        if (this.progressBar != null)
                        {
                            Match match = Regex.Match(e.Data, @"\d+", RegexOptions.IgnoreCase);

                            if (match.Success)
                            {
                                double value = double.Parse(match.Groups[0].Value);
                                if (value >= progressBar.Value) progressBar.Value = value;
                                else progressBar.Value = 0.0;
                            }
                        }
                    }
                }));
            };

            proc.StartInfo = procStartInfo;
            proc.Start();
            proc.BeginOutputReadLine();
        }
        catch (Exception objException)
        {
            MessageBox.Show("Error: " + objException.Message);
        }
    }
}