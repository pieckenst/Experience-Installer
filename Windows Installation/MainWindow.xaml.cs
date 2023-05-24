using System;
using System.Windows;
using System.Diagnostics;
using System.IO;

namespace Windows_Installation
{

    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            
        }

        

        private void PageDisplay_Initialized(object sender, EventArgs e)
        {
            Page1 secPage = new Page1();
            PageDisplay.Navigate(secPage);
        }
    }
}
