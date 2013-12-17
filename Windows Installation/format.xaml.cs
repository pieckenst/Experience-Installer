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
            new MainWindow().Show();
            this.Close();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            new apply().Show();
            this.Close();
        }

        private void sldPercent_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (lblPercentage != null)
            {
                lblPercentage.Content = Math.Round(sldPercent.Value).ToString() + "%";
            }
        }

        private void showGui(bool setEnabled)
        {
            lblPercentage.IsEnabled = setEnabled;
            sldPercent.IsEnabled = setEnabled;
            lblSize.IsEnabled = setEnabled;
        }
        
        private void RadioButton_Checked(object sender, RoutedEventArgs e)
        {
            showGui(true);
        }

        private void RadioButton_Checked_1(object sender, RoutedEventArgs e)
        {
            showGui(false);
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
            if ((bool) onePartition.IsChecked)
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
