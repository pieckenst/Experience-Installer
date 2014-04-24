using System;
using System.Diagnostics;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows;
using System.Windows.Controls;

class Cmd
{
    Process proc = new Process();
    TextBox lblOutput;
    ProgressBar progressBar;

    string command, commandOptions;
    bool clearOutput = true;

    public Cmd(string command, string commandOptions)
    {
        this.command = command;
        this.commandOptions = commandOptions;
    }

    public void showMessageWhenFinished(String messageToShow)
    {
        this.proc.Exited += (s, e) =>
        {
            if (lblOutput != null)
            {
                lblOutput.Dispatcher.BeginInvoke((Action)(() => 
                {
                    lblOutput.Text += messageToShow + "\n";
                }));
            }
        };
    }

    public void executeAfterExit(Cmd nextProcess)
    {
        this.proc.Exited += (s, e) =>
        {
            System.Threading.Thread.Sleep(500);
            nextProcess.execute();
        };
    }

    public void attachLabel(TextBox label)
    {
        this.lblOutput = label;
    }

    public void attachProgressBar(ProgressBar progressBar)
    {
        this.progressBar = progressBar;
    }

    public void disableClearOutput()
    {
        this.clearOutput = false;
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
            Console.Write(ex.Data);
        }
    }

    public bool isFinished()
    {
        return proc.HasExited;
    }

    private void WorkThreadFunction()
    {
        try
        {
            ExecuteCommandSync();
        }
        catch (Exception ex)
        {
            MessageBox.Show("Konnte nicht ausgeführt werden: " + ex.Data, "Fehler");
        }
    }

    private void ExecuteCommandSync()
    {
        try
        {
            ProcessStartInfo procStartInfo = new ProcessStartInfo(command, commandOptions);

            procStartInfo.RedirectStandardOutput = true;
            procStartInfo.UseShellExecute = false;
            procStartInfo.CreateNoWindow = true;

            proc.OutputDataReceived += (s, e) =>
            {
                if (e.Data == null) return;

                // Show output on a label?
                if (lblOutput != null)
                {
                    lblOutput.Dispatcher.BeginInvoke((Action)(() =>
                   {
                       if ((clearOutput) && (e.Data != "")) lblOutput.Text = e.Data;
                       else lblOutput.Text += e.Data + "\n";
                   }));
                }

                // Find percentage and show it on a progressBar
                if (this.progressBar != null)
                {
                    Match match = Regex.Match(e.Data, @"\d+");

                    if (match.Success)
                    {
                        double value = double.Parse(match.Groups[0].Value);

                        progressBar.Dispatcher.BeginInvoke((Action)(() =>
                       {
                           if (value >= progressBar.Value) progressBar.Value = value;
                           else progressBar.Value = 0.0; // Catches the time displayed after apply etc
                       }));
                    }
                }

            };

            proc.StartInfo = procStartInfo;
            proc.EnableRaisingEvents = true;
            proc.Start();
            proc.BeginOutputReadLine();

        }
        catch (Exception objException)
        {
            MessageBox.Show("Error: " + objException.Message, "Konnte nicht ausgeführt werden");
        }
    }
}