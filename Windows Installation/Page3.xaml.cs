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
    /// Логика взаимодействия для Page3.xaml
    /// </summary>
    public partial class Page3 : Page
    {
        public Page3()
        {
            InitializeComponent();
        }
        private void btnReboot_Click(object sender, RoutedEventArgs e)
        {
            Cmd reboot = new Cmd("wpeutil", "reboot");
            reboot.execute();
        }
        private void btnWimInfo_Click(object sender, RoutedEventArgs e)
        {
            output.Text = "";

            Cmd wimInfo = new Cmd("imagex", "/info " + lstWims.SelectedItem);
            wimInfo.attachLabel(output);
            wimInfo.disableClearOutput();
            wimInfo.execute();
        }
        private void btnApply_Click(object sender, RoutedEventArgs e)
        {
            Cmd apply = new Cmd("imagex", " /apply " + lstWims.SelectedItem + " 1 k:");
            apply.attachLabel(output);
            apply.attachProgressBar(pgrApplyProgress);
            apply.showMessageWhenFinished("Apply is done. _install is copied...");

            Cmd xcopy = new Cmd("xcopy", "\\\\changeme\\osdeploy\\inserts\\* K:\\ /s /Y");
            xcopy.attachLabel(output);
            xcopy.disableClearOutput();
            xcopy.showMessageWhenFinished("Copying is done. Bootloader is set up...");

            Cmd bootloader = new Cmd("bcdboot", " k:\\windows");
            bootloader.disableClearOutput();
            bootloader.attachLabel(output);
            bootloader.showMessageWhenFinished("Bootloader has been set up. The installation is now complete.");


            apply.executeAfterExit(xcopy);
            xcopy.executeAfterExit(bootloader);

            if (lstWims.SelectedIndex > -1)
            {
                apply.execute();
            }
            else
            {
                output.Text = "Please select a Wim first";
            }

        }
        private void txtWimPath_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            if (Directory.Exists(txtWimPath.Text))
            {
                lstWims.Items.Clear();

                foreach (string directory in Directory.GetFiles(txtWimPath.Text, "*.wim"))
                {
                    lstWims.Items.Add(directory);
                }

                if (lstWims.Items.Count > 0)
                {
                    lstWims.SelectedIndex = 0;
                }
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Cmd driver = new Cmd("\\\\changeme\\osdeploy\\drivercopy.exe", "");
            driver.execute();

        }
    }
}
