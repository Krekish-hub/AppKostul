using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using UchetGIC.DataFiles;
using UchetGIC.MiniWindows;

namespace UchetGIC.AddonPages
{
    /// <summary>
    /// Логика взаимодействия для AddEmployeePage.xaml
    /// </summary>
    public partial class AddEmployeePage : Page
    {
        public static event EventHandler EmployeeAdded;

        public ObservableCollection<Company> Companies { get; set; }
        public ObservableCollection<Role> Roles { get; set; }
        public ObservableCollection<LocalServ> LocalServs { get; set; }

        public AddEmployeePage()
        {
            InitializeComponent();

            Companies = new ObservableCollection<Company>(OdbConnectHelper.DbEntities.Company.ToList());
            Roles = new ObservableCollection<Role>(OdbConnectHelper.DbEntities.Role.ToList());
            LocalServs = new ObservableCollection<LocalServ>(OdbConnectHelper.DbEntities.LocalServ.ToList());

            CmbCompany.ItemsSource = Companies;
            CmbCompany.SelectedValuePath = "CompanyID";
            CmbCompany.DisplayMemberPath = "NameCompany";

            CmbPosition.ItemsSource = Roles;
            CmbPosition.SelectedValuePath = "ID";
            CmbPosition.DisplayMemberPath = "Name";

            CmbNumber.ItemsSource = LocalServs;
            CmbNumber.SelectedValuePath = "Number";
            CmbNumber.DisplayMemberPath = "Number";

            AddCompanyWindow.CompanyAdded += OnCompanyAdded;
            AddPositionWindow.PositionAdded += OnPositionAdded;
            AddLocalWindow.LocalAdded += OnLocalAdded;
        }

        private void OnCompanyAdded(object sender, EventArgs e)
        {
            UpdateCompanies();
        }

        private void OnPositionAdded(object sender, EventArgs e)
        {
            UpdatePositions();
        }

        private void OnLocalAdded(object sender, EventArgs e)
        {
            UpdateLocal();
        }

        private void UpdateCompanies()
        {
            var updatedCompanies = OdbConnectHelper.DbEntities.Company.ToList();
            Companies.Clear();
            foreach (var company in updatedCompanies)
            {
                Companies.Add(company);
            }
        }

        private void UpdatePositions()
        {
            var updatedRoles = OdbConnectHelper.DbEntities.Role.ToList();
            Roles.Clear();
            foreach (var role in updatedRoles)
            {
                Roles.Add(role);
            }
        }

        private void UpdateLocal()
        {
            var updatedLocals = OdbConnectHelper.DbEntities.LocalServ.ToList();
            LocalServs.Clear();
            foreach (var local in updatedLocals)
            {
                LocalServs.Add(local);
            }
        }

        private void BtnAddEmployee_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var salary = new Salary
                {
                    SalaryCount = Convert.ToDecimal(TxbZarplata.Text),
                    Premium = 0,
                    Status = "Текущая"
                };

                var user = new User
                {
                    IDRole = Convert.ToInt16(CmbPosition.SelectedValue),
                    Login = TxbName.Text,
                    Password = "1",
                    Name = TxbName.Text
                };

                OdbConnectHelper.DbEntities.Salary.Add(salary);
                OdbConnectHelper.DbEntities.User.Add(user);
                OdbConnectHelper.DbEntities.SaveChanges();

                var employee = new Employees
                {
                    FirstName = TxbName.Text,
                    LastName = TxbLastName.Text,
                    HireDate = DateTime.Now,
                    Salary1 = salary,
                    IDUser = user.ID,
                    Local = Convert.ToInt16(CmbNumber.SelectedValue),
                    Company = Convert.ToInt16(CmbCompany.SelectedValue)
                };

                OdbConnectHelper.DbEntities.Employees.Add(employee);
                OdbConnectHelper.DbEntities.SaveChanges();

                MessageBox.Show("Сотрудник " + employee.LastName + " успешно добавлен!",
                    "Уведомление",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information);

                FrameApp.FrameObj.GoBack();

                EmployeeAdded?.Invoke(this, EventArgs.Empty);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Критическая работа с приложением: " + ex.Message,
                    "Уведомление",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);
            }
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            FrameApp.FrameObj.GoBack();
        }

        private void AddCompany_OnClick(object sender, RoutedEventArgs e)
        {
            var addCompanyWindow = new AddCompanyWindow();
            addCompanyWindow.ShowDialog();
        }

        private void AddPosition_OnClick(object sender, RoutedEventArgs e)
        {
            var addPositionWindow = new AddPositionWindow();
            addPositionWindow.ShowDialog();
        }

        private void AddLocal_OnClick(object sender, RoutedEventArgs e)
        {
            var addLocalWindow = new AddLocalWindow();
            addLocalWindow.ShowDialog();
        }
    }
}
