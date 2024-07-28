using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using UchetGIC.DataFiles;
using UchetGIC.MiniWindows;

namespace UchetGIC.UserPages
{
    public partial class WorkTimePage : Page
    {
        private ObservableCollection<WorkTime> _workTimes;
        private Employees _currentEmployee;

        public WorkTimePage(Employees currentEmployee)
        {
            _currentEmployee = currentEmployee;
            InitializeComponent();
            LoadEmployees();
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
                LoadWorkTimes(employeeId);
            }
        }

        private void LoadWorkTimes(int employeeId)
        {
            _workTimes = new ObservableCollection<WorkTime>(OdbConnectHelper.DbEntities.WorkTime.Where(w => w.EmployeeID == employeeId).ToList());
            DataGridWorkTime.ItemsSource = _workTimes;
        }

        private void BtnAddWorkTime_Click(object sender, RoutedEventArgs e)
        {
            if (_currentEmployee == null)
            {
                MessageBox.Show("Сотрудник не выбран.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            var addWorkTimeWindow = new AddWorkTimeWindow(_currentEmployee.EmployeeID);
            if (addWorkTimeWindow.ShowDialog() == true)
            {
                var newWorkTime = addWorkTimeWindow.NewWorkTime;
                OdbConnectHelper.DbEntities.WorkTime.Add(newWorkTime);
                OdbConnectHelper.DbEntities.SaveChanges();
                LoadWorkTimes(_currentEmployee.EmployeeID);
            }
        }
    }
}