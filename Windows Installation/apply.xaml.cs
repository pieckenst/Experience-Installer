using System.Windows;

namespace Windows_Installation
{
    /// <summary>
    /// Interaktionslogik für apply.xaml
    /// </summary>
    public partial class apply : Window
    {

        InstallStateMachine iSM = InstallStateMachine.getISM();

        public apply()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            iSM.gotoState(InstallStateMachine.formatState);

            format formatWindow = new format();
            formatWindow.Show();

            this.Close();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if (iSM.isApplyDone() || true)
            {
                iSM.gotoState(InstallStateMachine.bootloaderState);

                bootloader blWindow = new bootloader();
                blWindow.Show();

                this.Close();
            }
            else
            {
                MessageBox.Show("Apply nicht fertig! Bitte Apply durchführen.");
            }
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            lblOut.Content = "";
            Cmd command = new Cmd("imagex", "/info " + txtWimPath.Text, lblOut, false);
            command.execute();
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            Cmd command = new Cmd("imagex", "/apply " + txtWimPath.Text + " 1 c:", lblOut, true);
            command.attachProgressBar(pgrBar);
            command.execute();
        }
    }
}
