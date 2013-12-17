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
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

            silent silentWindow = new silent();
            silentWindow.Show();

            this.Close();
        }


        private void Button_Click_1(object sender, RoutedEventArgs e)
        {

            format formatWindow = new format();
            formatWindow.Show();

            this.Close();
        }
    }
}
