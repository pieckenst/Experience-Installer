using System.Windows;

namespace Windows_Installation
{
    /// <summary>
    /// Interaktionslogik für apply.xaml
    /// </summary>
    public partial class apply : Window
    {

        public apply()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            new format().Show();
            this.Close();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            new bootloader().Show();
            this.Close();
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            lblOut.Content = "";

            Cmd command = new Cmd("imagex", "/info " + txtWimPath.Text);
            command.attachLabel(lblOut);
            command.setClearOutput(false);
            command.execute();
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            Cmd command = new Cmd("imagex", "/apply " + txtWimPath.Text + " 1 c:");
            command.attachLabel(lblOut);
            command.attachProgressBar(pgrBar);
            command.execute();
        }
    }
}
