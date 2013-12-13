using System;
using System.Windows;

namespace Windows_Installation
{
    /// <summary>
    /// Interaktionslogik für format.xaml
    /// </summary>
    public partial class format : Window
    {
        InstallStateMachine iSM = InstallStateMachine.getISM();

        public format()
        {
            InitializeComponent();
            onePartition.IsChecked = true;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            iSM.gotoState(InstallStateMachine.infoState);

            MainWindow main = new MainWindow();
            main.Show();

            this.Close();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {          
            if (iSM.isCFormattedAndAct() || true) // DEBUG
            {
                iSM.gotoState(InstallStateMachine.applyState);

                apply applyWindow = new apply();
                applyWindow.Show();

                this.Close();
            }
            else
            {
                MessageBox.Show("Festplatte muss erst formatiert werden!");
            }
        }

        private void sldPercent_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (lblPercentage != null)
            {
                lblPercentage.Content = Math.Round(sldPercent.Value).ToString() + "%";
            }
        }

        private void RadioButton_Checked(object sender, RoutedEventArgs e)
        {
            lblPercentage.IsEnabled = true;
            sldPercent.IsEnabled = true;
            lblSize.IsEnabled = true;
        }

        private void RadioButton_Checked_1(object sender, RoutedEventArgs e)
        {
            lblPercentage.IsEnabled = false;
            sldPercent.IsEnabled = false;
            lblSize.IsEnabled = false;
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            lblOutput.Content = "";
            Cmd command = new Cmd("diskpart", "/s diskpart\\listdisk.txt", lblOutput, false);
            command.execute();
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            if ((bool) onePartition.IsChecked)
            {
                lblOutput.Content = "";
                Cmd command = new Cmd("diskpart", "/s diskpart\\allC.txt", lblOutput, true);
                command.execute();
                iSM.setCFormattedAndAct(true);
            }
            else
            {
                MessageBox.Show("Noch nicht implementiert...");
            }
        }

        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            lblOutput.Content = "";
            Cmd command = new Cmd("diskpart", "/s diskpart\\listvol.txt", lblOutput, false);
            command.execute();
        }
    }
}
