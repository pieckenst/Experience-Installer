using System.Windows;

namespace Windows_Installation
{
    /// <summary>
    /// Interaktionslogik für silent.xaml
    /// </summary>
    public partial class silent : Window
    {

        public silent()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

            new MainWindow().Show();
            this.Close();
        }
    }
}
