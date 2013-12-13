using System.Windows;

namespace Windows_Installation
{
    /// <summary>
    /// Interaktionslogik für bootloader.xaml
    /// </summary>
    public partial class bootloader : Window
    {
        InstallStateMachine iSM = InstallStateMachine.getISM();

        public bootloader()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            iSM.gotoState(InstallStateMachine.applyState);

            apply applyWindow = new apply();
            applyWindow.Show();

            this.Close();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if (iSM.isBootloaderDone() || true) // DEBUG
            {
                iSM.gotoState(InstallStateMachine.rebootState);

                reboot rebootWindow = new reboot();
                rebootWindow.Show();

                this.Close();
            }
            else
            {
                MessageBox.Show("Bitte installieren sie zuerst den Bootloader");
            }
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            Cmd command = new Cmd("bcdboot", "c:\\windows", lblOutput);
            command.execute();
            iSM.setBootloaderDone(true);
        }
    }
}
