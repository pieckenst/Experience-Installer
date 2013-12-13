using System.Windows;

namespace Windows_Installation
{

    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        InstallStateMachine iSM = InstallStateMachine.getISM();
        
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (iSM.canGotoState(InstallStateMachine.silentState))
            {
                iSM.gotoState(InstallStateMachine.silentState);
                iSM.setSilent(true);

                silent silentWindow = new silent();
                silentWindow.Show();

                this.Close();
            }
        }

        public InstallStateMachine getInstallMachine() {
            return this.iSM;
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            iSM.gotoState(InstallStateMachine.formatState);

            format formatWindow = new format();
            formatWindow.Show();

            this.Close();
        }
    }
}
