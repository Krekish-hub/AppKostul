using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;
using UchetGIC.Controllers;
using UchetGIC.ControllPages;
using UchetGIC.DataFiles;
using UchetGIC.StartUpPages;

namespace UchetGIC.MenuController
{
    /// <summary>
    /// Логика взаимодействия для MenuControllPage.xaml
    /// </summary>
    public partial class MenuControllPage : Page
    {
        private Employees _currentEmployee;
        private int _pendingRequestsCount;
        
        public MenuControllPage()
        {
            InitializeComponent();
            LoadCurrentUser();
            UpdatePendingRequestsCount();
        }

        public void UpdatePendingRequestsCount()
        {
            _pendingRequestsCount = OdbConnectHelper.DbEntities.Leave.Count(lr => lr.Status == "Ожидание") +
                                    OdbConnectHelper.DbEntities.Salary.Count(sr => sr.Status == "Ожидание");

            PendingRequestsTextBlock.Content = _pendingRequestsCount.ToString();
            PendingRequestsTextBlock.Visibility = _pendingRequestsCount > 0 ? Visibility.Visible : Visibility.Collapsed;
        }

        private void ToggleMenu(object sender, RoutedEventArgs e)
        {
            DoubleAnimation animation = new DoubleAnimation
            {
                To = SlideMenu.Width == 0 ? 160 : 0,
                Duration = TimeSpan.FromSeconds(0.3)
            };
            SlideMenu.BeginAnimation(Border.WidthProperty, animation);
        }

        private void LoadCurrentUser()
        {
            int userId = UserController.IdUser;
            _currentEmployee = OdbConnectHelper.DbEntities.Employees.FirstOrDefault(e => e.IDUser == userId);

            if (_currentEmployee == null)
            {
                MessageBox.Show("Не удалось загрузить данные пользователя.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void MenuCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            var selectedCheckBox = sender as CheckBox;

            foreach (var child in MenuStackPanel.Children)
            {
                if (child is CheckBox checkBox && checkBox != selectedCheckBox)
                {
                    checkBox.IsChecked = false;
                }
            }
        }

        private void BtnProfile_Click(object sender, RoutedEventArgs e)
        {
            ContentFrame.Navigate(new ProfileEmployeePage(_currentEmployee));
        }

        private void BtnEmployee_Click(object sender, RoutedEventArgs e)
        {
            ContentFrame.Navigate(new UserPage());
        }

        private void BtnTabel_Click(object sender, RoutedEventArgs e)
        {
            //ContentFrame.Navigate(new WorkTimePage());
        }

        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {
            FrameApp.FrameObj.Navigate(new AutorizationPage());
            SlideMenu.Visibility = Visibility.Collapsed;
        }

        private void BtnRequests_OnClick(object sender, RoutedEventArgs e)
        {
            ContentFrame.Navigate(new ApproveRequestsPage());
        }
    }
}
