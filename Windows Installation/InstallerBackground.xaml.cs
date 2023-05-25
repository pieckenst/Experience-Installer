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
using System.Windows.Threading;

namespace Windows_Installation
{
    /// <summary>
    /// Логика взаимодействия для InstallerBackground.xaml
    /// </summary>
    public partial class InstallerBackground : Window
    {
        private static Random random = new Random(); // Class member
        System.Windows.Threading.DispatcherTimer timer = new System.Windows.Threading.DispatcherTimer();
        System.Windows.Threading.DispatcherTimer timer2 = new System.Windows.Threading.DispatcherTimer();
        System.Windows.Threading.DispatcherTimer timer3 = new System.Windows.Threading.DispatcherTimer();
        System.Windows.Threading.DispatcherTimer timer4 = new System.Windows.Threading.DispatcherTimer();
        System.Windows.Threading.DispatcherTimer timer5 = new System.Windows.Threading.DispatcherTimer();
        public InstallerBackground()
        {
            InitializeComponent();
        }
        void timer_Tick(object sender, System.EventArgs e)
        {
            status1.IsChecked = false;
            status2.IsChecked = true;
            timer.Stop();
        }
        void timer2_Tick(object sender, System.EventArgs e)
        {
            status2.IsChecked = false;
            status3.IsChecked = true;
            timer2.Stop();
        }
        void timer3_Tick(object sender, System.EventArgs e)
        {
            status3.IsChecked = false;
            status4.IsChecked = true;
            timer3.Stop();
        }
        void timer4_Tick(object sender, System.EventArgs e)
        {
            status4.IsChecked = false;
            status5.IsChecked = true;
            timer4.Stop();
        }
        void timer5_Tick(object sender, System.EventArgs e)
        {
            status5.IsChecked = false;
            
            timer5.Stop();
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            timer.Tick += new EventHandler(timer_Tick);
            timer.Interval = new TimeSpan(0, 1, 0);
            timer.Start();
            status1.IsChecked = true;
            timer2.Tick += new EventHandler(timer2_Tick);
            timer2.Interval = new TimeSpan(0, 5, 0);
            timer2.Start();
            timer3.Tick += new EventHandler(timer3_Tick);
            timer3.Interval = new TimeSpan(0, 10, 0);
            timer3.Start();
            timer4.Tick += new EventHandler(timer4_Tick);
            timer4.Interval = new TimeSpan(0, 10, 0);
            timer4.Start();
            timer5.Tick += new EventHandler(timer5_Tick);
            timer5.Interval = new TimeSpan(0, 10, 0);
            timer5.Start();
        }
    }

}
