using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

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
            Cmd command = new Cmd("wpeutil", "reboot", new Label());
            command.execute();
        }
    }
}
