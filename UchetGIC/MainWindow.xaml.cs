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
using System.Windows.Navigation;
using System.Windows.Shapes;
using UchetGIC.DataFiles;
using UchetGIC.StartUpPages;

namespace UchetGIC
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            OdbConnectHelper.DbEntities = new UchetDBEntities();

            FrameApp.FrameObj = MainFrame;
            FrameApp.FrameObj.Navigate(new AutorizationPage());
            
            MainFrame.Navigated += MainFrame_Navigated;
        }

        private void MainFrame_Navigated(object sender, NavigationEventArgs e)
        {
            if (e.Content is AutorizationPage)
            {
                this.ResizeMode = ResizeMode.NoResize;
            }
            else
            {
                this.ResizeMode = ResizeMode.CanResize;
            }
        }
    }
}
