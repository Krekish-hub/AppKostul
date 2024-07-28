using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using UchetGIC.DataFiles;
using UchetGIC.MiniWindows;

namespace UchetGIC.UserPages
{
    public partial class LeavePage : Page
    {
        private Employees _currentEmployee;
        private ObservableCollection<Leave> _leaves;
        private ObservableCollection<Employees> _allEmployees;

        public LeavePage(Employees currentEmployee)
        {
            InitializeComponent();
            _currentEmployee = currentEmployee;
            LoadEmployees();
            LoadLeaveTypes();
            LoadLeaves(_currentEmployee.EmployeeID);
        }

        private void LoadEmployees()
        {
            _allEmployees = new ObservableCollection<Employees>(OdbConnectHelper.DbEntities.Employees.ToList());
            AutoCompleteTextBox.TextChanged += AutoCompleteTextBox_TextChanged;
        }

        private void AutoCompleteTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            string searchText = AutoCompleteTextBox.Text.ToLower();
            var filteredEmployees = _allEmployees.Where(emp => emp.LastName.ToLower().StartsWith(searchText)).ToList();
            AutoCompleteListBox.ItemsSource = filteredEmployees;
            AutoCompleteListBox.DisplayMemberPath = "LastName";
            AutoCompletePopup.IsOpen = filteredEmployees.Any();
        }

        private void AutoCompleteListBox_MouseUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (AutoCompleteListBox.SelectedItem is Employees selectedEmployee)
            {
                AutoCompleteTextBox.Text = selectedEmployee.LastName;
                _currentEmployee = selectedEmployee;
                AutoCompletePopup.IsOpen = false;
                LoadLeaves(_currentEmployee.EmployeeID);
            }
        }

        private void LoadLeaves(int employeeId)
        {
            _leaves = new ObservableCollection<Leave>(OdbConnectHelper.DbEntities.Leave.Where(l => l.EmployeeID == employeeId).ToList());
            ApplyFilters();
        }
        private void LoadLeaveTypes()
        {
            CmbLeaveTypeFilter.ItemsSource = new[] { "Больничный", "Декрет", "Отпуск" };
        }
        private void ApplyFilters()
        {
            var filteredLeaves = _leaves.AsQueryable();

            if (CmbLeaveTypeFilter.SelectedItem is string leaveType && !string.IsNullOrEmpty(leaveType))
            {
                filteredLeaves = filteredLeaves.Where(l => l.LeaveType == leaveType);
            }

            if (DpStartDateFilter.SelectedDate.HasValue)
            {
                filteredLeaves = filteredLeaves.Where(l => l.StartDate >= DpStartDateFilter.SelectedDate.Value);
            }

            DataGridLeave.ItemsSource = filteredLeaves.ToList();
        }

        private void FilterChanged(object sender, SelectionChangedEventArgs e)
        {
            ApplyFilters();
        }

        private void BtnResetFilters_Click(object sender, RoutedEventArgs e)
        {
            CmbLeaveTypeFilter.SelectedIndex = -1;
            DpStartDateFilter.SelectedDate = null;
            ApplyFilters();
        }

        private void BtnAddLeave_Click(object sender, RoutedEventArgs e)
        {
            if (_currentEmployee != null)
            {
                var requestLeaveWindow = new RequestLeaveWindow(_currentEmployee.EmployeeID);
                if (requestLeaveWindow.ShowDialog() == true)
                {
                    var newLeave = requestLeaveWindow.NewLeave;
                    OdbConnectHelper.DbEntities.Leave.Add(newLeave);
                    OdbConnectHelper.DbEntities.SaveChanges();
                    LoadLeaves(_currentEmployee.EmployeeID);
                }
            }
            else
            {
                MessageBox.Show("Пожалуйста, выберите сотрудника.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
