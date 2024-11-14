using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace Windows_Installation
{
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
                if (diskPart.formatOnePartition(int.Parse(txtdisknum.Text)))
                {
                    MessageBox.Show("Format completed");
                }
            }
            else
            {
                int partitionSize = int.Parse(txtGb.Text);
                int diskNumber = int.Parse(txtdisknum.Text);
                diskPart.formatTwoPartitions(partitionSize, diskNumber);
                MessageBox.Show("Format completed");
            }
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }

        private void NextButton_Click(object sender, RoutedEventArgs e)
        {
            Page3 nextPage = new Page3();
            NavigationService.Navigate(nextPage);
        }
    }
}
