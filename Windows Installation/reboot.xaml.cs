using System.Windows;

namespace Windows_Installation
{
    /// <summary>
    /// Interaktionslogik für reboot.xaml
    /// </summary>
    public partial class reboot : Window
    {
        public reboot()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Cmd command = new Cmd("wpeutil", "reboot");
            command.execute();
        }
    }
}
