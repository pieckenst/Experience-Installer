using System;
using System.Windows;
using System.Diagnostics;
using System.IO;

namespace Windows_Installation
{

    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            try
            {
                txtWimPath.Text = System.Configuration.ConfigurationManager.AppSettings["path"].ToString();
            }
            catch (Exception ex)
            {
                txtWimPath.Text = "p:\\ath\\to\\wim";
            }
            cOnePartition.IsChecked = true;
        }

        private void btnDiskInfo_Click(object sender, RoutedEventArgs e)
        {
            output.Text = "";
            new Diskpart(output).diskInfo();
            
        }

        private void btnExitToCmd_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void btnReboot_Click(object sender, RoutedEventArgs e)
        {
            Cmd reboot = new Cmd("wpeutil", "reboot");
            reboot.execute();
        }

        private void btnApply_Click(object sender, RoutedEventArgs e)
        {
            Cmd apply = new Cmd("imagex", " /apply " + lstWims.SelectedItem + " 1 k:");
            apply.attachLabel(output);
            apply.attachProgressBar(pgrApplyProgress);
            apply.showMessageWhenFinished("Apply ist fertig. _install wird kopiert...");

            Cmd xcopy = new Cmd("xcopy", "\\\\changeme\\osdeploy\\inserts\\* K:\\ /s /Y");
            xcopy.attachLabel(output);
            xcopy.disableClearOutput();
            xcopy.showMessageWhenFinished("Kopieren ist fertig. Bootloader wird eingerichtet...");

            Cmd bootloader = new Cmd("bcdboot", " k:\\windows");
            bootloader.disableClearOutput();
            bootloader.attachLabel(output);
            bootloader.showMessageWhenFinished("Bootloader wurde eingerichtet. Die Installation ist damit abgeschlossen.");

            
            apply.executeAfterExit(xcopy);
            xcopy.executeAfterExit(bootloader);

            if (lstWims.SelectedIndex > -1)
            {
                apply.execute();
            }
            else
            {
                output.Text = "Bitte zuerst ein Wim auswählen";
            }
            
        }

        private void btnWimInfo_Click(object sender, RoutedEventArgs e)
        {
            output.Text = "";

            Cmd wimInfo = new Cmd("imagex", "/info " + lstWims.SelectedItem);
            wimInfo.attachLabel(output);
            wimInfo.disableClearOutput();
            wimInfo.execute();
        }

        private void Button_Click(object sender, RoutedEventArgs events)
        {
            Diskpart diskPart = new Diskpart(output);

            if ((bool) cOnePartition.IsChecked)
            {
                diskPart.formatOnePartition();
            }
            else
            {
                try
                {
                    int gb = Convert.ToInt16(txtGb.Text);
                    diskPart.formatTwoPartitions(gb);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
            }

            Cmd echo = new Cmd("cmd.exe", "/c echo Formatieren abgeschlossen.");
            echo.attachLabel(output);
            diskPart.executeAfterExit(echo);
        }

        private void cTwoPartitions_Checked(object sender, RoutedEventArgs e)
        {
            txtGb.IsEnabled = true;
        }

        private void cOnePartition_Checked(object sender, RoutedEventArgs e)
        {
            txtGb.IsEnabled = false;
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Cmd driver = new Cmd("\\\\changeme\\osdeploy\\drivercopy.exe", "");
            driver.execute();

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
