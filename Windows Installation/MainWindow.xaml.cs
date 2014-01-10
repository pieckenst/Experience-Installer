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
            output.Text = "";

            Diskpart diskPart = new Diskpart(output);
            diskPart.diskInfo();
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
            output.Text = "Bootloader wird eingerichtet...";

            Cmd bootloader = new Cmd("bcdboot", " c:\\windows");
            bootloader.attachLabel(output);
            bootloader.execute();
        }

        private void btnApply_Click(object sender, RoutedEventArgs e)
        {
            Cmd apply = new Cmd("imagex", " /apply " + txtWimPath.Text + " 1 c:");
            apply.attachLabel(output);
            apply.attachProgressBar(pgrApplyProgress);
            apply.execute();
        }

        private void btnWimInfo_Click(object sender, RoutedEventArgs e)
        {
            output.Text = "";

            Cmd wimInfo = new Cmd("imagex", "/info " + txtWimPath.Text);
            wimInfo.attachLabel(output);
            wimInfo.disableClearOutput();
            wimInfo.execute();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Diskpart diskPart = new Diskpart(output);

            if ((bool) cOnePartition.IsChecked)
            {
                diskPart.formatOnePartition();
            }
            else
            {
                diskPart.formatTwoPartitions();
            }
        }
    }
}
