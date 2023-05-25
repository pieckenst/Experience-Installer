using System;
using System.Collections.Generic;
using System.IO;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Windows_Installation
{
    /// <summary>
    /// Логика взаимодействия для Page2.xaml
    /// </summary>
    public partial class Page2 : Page
    {
        public Page2()
        {
            InitializeComponent();
            
        }
        private void btnDiskInfo_Click(object sender, RoutedEventArgs e)
        {
            output.Text = "";
            new Diskpart(output).diskInfo();

        }

        

        

       

        

        private void Button_Click(object sender, RoutedEventArgs events)
        {
            Diskpart diskPart = new Diskpart(output);

            if ((bool)cOnePartition.IsChecked)
            {
                diskPart.formatOnePartition(int.Parse(txtdisknum.Text));
            }
            else
            {
                diskPart.formatOnePartition(int.Parse(txtdisknum.Text));
            }

            Cmd echo = new Cmd("cmd.exe", "/c echo Formatting is complete.");
            echo.attachLabel(output);
            
        }

        

        private void NextButton_Click(object sender, RoutedEventArgs e)
        {
            Page3 secPage = new Page3();
            NavigationService.Navigate(secPage);
        }
    }
    
}
