using System.Windows;

namespace Windows_Installation
{
    /// <summary>
    /// Interaktionslogik für bootloader.xaml
    /// </summary>
    public partial class bootloader : Window
    {

        public bootloader()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

            apply applyWindow = new apply();
            applyWindow.Show();

            this.Close();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {

            reboot rebootWindow = new reboot();
            rebootWindow.Show();

            this.Close();
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            Cmd command = new Cmd("bcdboot", "c:\\windows");
            command.attachLabel(lblOutput);
            command.execute();
        }
    }
}
