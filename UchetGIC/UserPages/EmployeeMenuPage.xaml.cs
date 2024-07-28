using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;
using UchetGIC.Controllers;
using UchetGIC.DataFiles;
using UchetGIC.StartUpPages;

namespace UchetGIC.UserPages
{
    public partial class EmployeeMenuPage : Page
    {
        private Employees _currentEmployee;

        public EmployeeMenuPage()
        {
            InitializeComponent();
            LoadCurrentUser();
        }

        private void LoadCurrentUser()
        {
            int userId = UserController.IdUser; // Получаем ID текущего пользователя из контроллера
            _currentEmployee = OdbConnectHelper.DbEntities.Employees.FirstOrDefault(e => e.IDUser == userId);

            if (_currentEmployee != null)
            {
                ContentFrame.Navigate(new LeavePage(_currentEmployee)); // Переход к LeavePage
            }
            else
            {
                MessageBox.Show("Не удалось загрузить данные пользователя.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ToggleMenu(object sender, RoutedEventArgs e)
        {
            DoubleAnimation animation = new DoubleAnimation
            {
                To = SlideMenu.Width == 160 ? 0 : 160,
                Duration = TimeSpan.FromSeconds(0.3)
            };
            SlideMenu.BeginAnimation(WidthProperty, animation);
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

        private void BtnZarplata_Click(object sender, RoutedEventArgs e)
        {
            ContentFrame.Navigate(new SalaryPage(_currentEmployee));
        }

        private void BtnSmena_Click(object sender, RoutedEventArgs e)
        {
            //ContentFrame.Navigate(new ---(_currentEmployee));
        }

        private void BtnTabel_Click(object sender, RoutedEventArgs e)
        {
            ContentFrame.Navigate(new WorkTimePage(_currentEmployee));
        }

        private void BtnLeave_Click(object sender, RoutedEventArgs e)
        {
            ContentFrame.Navigate(new LeavePage(_currentEmployee));
        }

        private void BtnEmployeeCount_Click(object sender, RoutedEventArgs e)
        {
            ContentFrame.Navigate(new EmployeeCountPage());
        }

        private void BtnMeal_Click(object sender, RoutedEventArgs e)
        {
            ContentFrame.Navigate(new MealSchedulePage(_currentEmployee));
        }

        private void BtnSpecialClothing_Click(object sender, RoutedEventArgs e)
        {
            ContentFrame.Navigate(new SpecialClothingPage(_currentEmployee));
        }

        private void BtnSeasonal_Click(object sender, RoutedEventArgs e)
        {
            ContentFrame.Navigate(new SeasonalEmployeePage(_currentEmployee));
        }

        private void BtnFeedback_Click(object sender, RoutedEventArgs e)
        {
            ContentFrame.Navigate(new FeedbackPage(_currentEmployee));
        }

        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {
            FrameApp.FrameObj.Navigate(new AutorizationPage());
            SlideMenu.Visibility = Visibility.Collapsed;
        }
    }
}
