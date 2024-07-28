using System;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using UchetGIC.Controllers;
using UchetGIC.DataFiles;
using UchetGIC.MenuController;
using UchetGIC.UserPages;

namespace UchetGIC.StartUpPages
{
    /// <summary>
    /// Логика взаимодействия для AutorizationPage.xaml
    /// </summary>
    public partial class AutorizationPage : Page
    {
        public AutorizationPage()
        {
            InitializeComponent();

            if (Properties.Settings.Default.EventSaveLogin != string.Empty)
            {
                txtLogin.Text = Properties.Settings.Default.EventSaveLogin;
            }

            FrameApp.NavigationUIVisibility = System.Windows.Navigation.NavigationUIVisibility.Hidden;
        }

        private async void NavigateToPage(Page page)
        {
            var nextPageContent = page.Content;
            var sb = new Storyboard();
            var fadeOutAnimation = new DoubleAnimation
            {
                From = 1,
                To = 0,
                Duration = TimeSpan.FromSeconds(0.3),
                FillBehavior = FillBehavior.HoldEnd
            };
            Storyboard.SetTarget(fadeOutAnimation, FrameApp.FrameObj);
            Storyboard.SetTargetProperty(fadeOutAnimation, new PropertyPath(OpacityProperty));
            sb.Children.Add(fadeOutAnimation);

            sb.Completed += (s, e) =>
            {
                FrameApp.FrameObj.Navigate(page);
                var fadeInAnimation = new DoubleAnimation
                {
                    From = 0,
                    To = 1,
                    Duration = TimeSpan.FromSeconds(0.5),
                    FillBehavior = FillBehavior.HoldEnd
                };
                Storyboard.SetTarget(fadeInAnimation, FrameApp.FrameObj);
                Storyboard.SetTargetProperty(fadeInAnimation, new PropertyPath(OpacityProperty));
                var newSb = new Storyboard();
                newSb.Children.Add(fadeInAnimation);
                newSb.Begin();
            };
            sb.Begin();
        }

        public class AuthorizationService
        {
            public async Task<User> LoginAsync(string login, string password)
            {
                var user = await OdbConnectHelper.DbEntities.User
                    .Include(u => u.Role)
                    .Include(u => u.Employees)
                    .FirstOrDefaultAsync(u => u.Login == login && u.Password == password);

                if (user == null)
                {
                    throw new Exception("Invalid login or password.");
                }

                return user;
            }
        }

        private async void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            var loadingAnimation = (Storyboard)FindResource("LoadingAnimation");
            loadingAnimation.Begin(btnLogin);

            try
            {
                var authorizationService = new AuthorizationService();
                var user = await authorizationService.LoginAsync(txtLogin.Text, psbPassword.Password);

                UserController.IdUser = user.ID;

                loadingAnimation.Stop(btnLogin);

                if (user.IDRole == 1)
                {
                    NavigateToPage(new MenuControllPage());
                }
                else if (user.IDRole == 2)
                {
                    var employee = user.Employees.FirstOrDefault();
                    NavigateToPage(new EmployeeMenuPage());
                }
                else
                {
                    MessageBox.Show("Для данного сотрудника нет учётной записи");
                }
            }
            catch (Exception ex)
            {
                loadingAnimation.Stop(btnLogin);
                MessageBox.Show($"Ошибка: {ex.Message}", "Ошибка авторизации", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void TxtLogin_GotFocus(object sender, RoutedEventArgs e)
        {
            if (txtLogin.Text == "Логин")
            {
                txtLogin.Text = string.Empty;
                txtLogin.Foreground = new SolidColorBrush(Colors.Black);
            }
        }

        private void TxtLogin_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtLogin.Text))
            {
                txtLogin.Text = "Логин";
                txtLogin.Foreground = new SolidColorBrush(Colors.Gray);
            }
        }

        private void TxtPassword_GotFocus(object sender, RoutedEventArgs e)
        {
            if (psbPassword.Password == "Пароль")
            {
                psbPassword.Password = string.Empty;
                psbPassword.Foreground = new SolidColorBrush(Colors.Black);
            }
        }

        private void TxtPassword_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(psbPassword.Password))
            {
                psbPassword.Password = "Пароль";
                psbPassword.Foreground = new SolidColorBrush(Colors.Gray);
            }
        }
    }
}
