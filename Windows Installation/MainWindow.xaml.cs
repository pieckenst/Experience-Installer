using System.Windows;

namespace Windows_Installation
{

    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            cOnePartition.IsChecked = true;
        }

        private void btnDiskInfo_Click(object sender, RoutedEventArgs e)
        {
            lblCmd.Content = "";

            Cmd diskInfo = new Cmd("diskpart", "-s diskpart\\listdisk.txt");
            diskInfo.attachLabel(lblCmd);
            diskInfo.disableClearOutput();
            diskInfo.execute();
        }

        private void btnVolumeInfo_Click(object sender, RoutedEventArgs e)
        {
            lblCmd.Content = "";

            Cmd volumeInfo = new Cmd("diskpart", "-s diskpart\\listvol.txt");
            volumeInfo.attachLabel(lblCmd);
            volumeInfo.disableClearOutput();
            volumeInfo.execute();
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

        private void btnBootloader_Click(object sender, RoutedEventArgs e)
        {
            lblCmd.Content = "Bootloader wird eingerichtet...";

            Cmd bootloader = new Cmd("bcdboot", " c:\\windows");
            bootloader.attachLabel(lblCmd);
            bootloader.execute();
        }

        private void btnApply_Click(object sender, RoutedEventArgs e)
        {
            Cmd apply = new Cmd("imagex", " /apply " + txtWimPath.Text + " 1 c:");
            apply.attachLabel(lblCmd);
            apply.attachProgressBar(pgrApplyProgress);
            apply.execute();
        }

        private void btnWimInfo_Click(object sender, RoutedEventArgs e)
        {
            lblCmd.Content = "";

            Cmd wimInfo = new Cmd("imagex", "/info " + txtWimPath.Text);
            wimInfo.attachLabel(lblCmd);
            wimInfo.disableClearOutput();
            wimInfo.execute();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Cmd format;

            if ((bool) cOnePartition.IsChecked)
            {
                format = new Cmd("diskpart", "/s diskpart\\allC.txt");
            }
            else
            {
                format = new Cmd("diskpart", "/s diskpart\\twoPartitions.txt");
            }

            format.attachLabel(lblCmd);
            format.execute();
        }
    }
}
