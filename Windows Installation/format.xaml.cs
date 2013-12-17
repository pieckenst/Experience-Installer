using System;
using System.Windows;

namespace Windows_Installation
{
    /// <summary>
    /// Interaktionslogik für format.xaml
    /// </summary>
    public partial class format : Window
    {

        public format()
        {
            InitializeComponent();
            onePartition.IsChecked = true;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

            MainWindow main = new MainWindow();
            main.Show();

            this.Close();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
                apply applyWindow = new apply();
                applyWindow.Show();

                this.Close();
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
            Cmd command = new Cmd("diskpart", "/s diskpart\\listdisk.txt");
            command.attachLabel(lblOutput);
            command.setClearOutput(false);
            command.execute();
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            if ((bool)onePartition.IsChecked)
            {
                lblOutput.Content = "";
                Cmd command = new Cmd("diskpart", "/s diskpart\\allC.txt");
                command.attachLabel(lblOutput);
                command.execute();
            }
            else
            {
                MessageBox.Show("Noch nicht implementiert...");
            }
        }

        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            lblOutput.Content = "";
            Cmd command = new Cmd("diskpart", "/s diskpart\\listvol.txt");
            command.attachLabel(lblOutput);
            command.setClearOutput(false);
            command.execute();
        }
    }
}
