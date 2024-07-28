using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using UchetGIC.DataFiles;

namespace UchetGIC.ControllPages
{
    public partial class ProfileEmployeePage : Page
    {
        private Employees _employee;

        public ProfileEmployeePage(Employees employee)
        {
            InitializeComponent();
            _employee = employee;
            LoadComboBoxes();
            LoadEmployeeData();
        }

        private void LoadEmployeeData()
        {
            if (_employee == null) return;

            TxtFirstName.Text = _employee.FirstName;
            TxtLastName.Text = _employee.LastName;
            CmbDepartment.SelectedValue = _employee.DepartmentID;
            CmbPosition.SelectedValue = _employee.IDUser;
            DpDateOfBirth.SelectedDate = _employee.DateOfBirth;
            DpHireDate.SelectedDate = _employee.HireDate;
            var salary = OdbConnectHelper.DbEntities.Salary.FirstOrDefault(s => s.SalaryID == _employee.Salary);
            TxtSalary.Text = salary?.SalaryCount.ToString();
            CmbCompany.SelectedValue = _employee.Company;
        }

        private void LoadComboBoxes()
        {
            CmbDepartment.ItemsSource = OdbConnectHelper.DbEntities.Departments.ToList();
            CmbDepartment.DisplayMemberPath = "DepartmentName";
            CmbDepartment.SelectedValuePath = "DepartmentID";

            CmbPosition.ItemsSource = OdbConnectHelper.DbEntities.Role.ToList();
            CmbPosition.DisplayMemberPath = "Name";
            CmbPosition.SelectedValuePath = "ID";

            CmbCompany.ItemsSource = OdbConnectHelper.DbEntities.Company.ToList();
            CmbCompany.DisplayMemberPath = "NameCompany";
            CmbCompany.SelectedValuePath = "CompanyID";
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            if (_employee == null) return;

            _employee.FirstName = TxtFirstName.Text;
            _employee.LastName = TxtLastName.Text;
            _employee.DepartmentID = (int?)CmbDepartment.SelectedValue;
            _employee.IDUser = (int?)CmbPosition.SelectedValue;
            _employee.DateOfBirth = DpDateOfBirth.SelectedDate;
            _employee.HireDate = DpHireDate.SelectedDate;
            _employee.Company = (int?)CmbCompany.SelectedValue;

            // Сохранение данных о зарплате
            var salary = OdbConnectHelper.DbEntities.Salary.FirstOrDefault(s => s.SalaryID == _employee.Salary);
            if (salary == null)
            {
                salary = new Salary
                {
                    SalaryID = _employee.Salary ?? 0,
                    SalaryCount = decimal.TryParse(TxtSalary.Text, out var salaryCount) ? salaryCount : 0
                };
                OdbConnectHelper.DbEntities.Salary.Add(salary);
            }
            else
            {
                salary.SalaryCount = decimal.TryParse(TxtSalary.Text, out var salaryCount) ? salaryCount : 0;
            }

            try
            {
                OdbConnectHelper.DbEntities.SaveChanges();
                MessageBox.Show("Данные успешно сохранены!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при сохранении данных: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
