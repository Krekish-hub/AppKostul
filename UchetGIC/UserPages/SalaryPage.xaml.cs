using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using UchetGIC.DataFiles;
using UchetGIC.MiniWindows;

namespace UchetGIC.UserPages
{
    public partial class SalaryPage : Page
    {
        private Employees _currentEmployee;
        private ObservableCollection<Salary> _salaries;

        public SalaryPage(Employees currentEmployee)
        {
            InitializeComponent();
            _currentEmployee = currentEmployee;
            LoadEmployees();
            LoadSalaries(_currentEmployee.EmployeeID);
        }

        private void LoadEmployees()
        {
            CmbEmployee.ItemsSource = OdbConnectHelper.DbEntities.Employees.ToList();
            CmbEmployee.DisplayMemberPath = "LastName";
            CmbEmployee.SelectedValuePath = "EmployeeID";
            CmbEmployee.SelectedValue = _currentEmployee.EmployeeID;
        }

        private void CmbEmployee_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (CmbEmployee.SelectedValue is int employeeId)
            {
                _currentEmployee = OdbConnectHelper.DbEntities.Employees.FirstOrDefault(emp => emp.EmployeeID == employeeId);
                LoadSalaries(employeeId);
            }
        }

        private void LoadSalaries(int employeeId)
        {
            _salaries = new ObservableCollection<Salary>(OdbConnectHelper.DbEntities.Salary.Where(s => s.EmployeeID == employeeId).ToList());
            DataGridSalary.ItemsSource = _salaries;
        }

        private void BtnRequestSalary_Click(object sender, RoutedEventArgs e)
        {
            var requestSalaryChangeWindow = new RequestSalaryChangeWindow(_currentEmployee.EmployeeID);
            if (requestSalaryChangeWindow.ShowDialog() == true)
            {
                OdbConnectHelper.DbEntities.SaveChanges();
                LoadSalaries(_currentEmployee.EmployeeID);
            }
        }
    }
}
