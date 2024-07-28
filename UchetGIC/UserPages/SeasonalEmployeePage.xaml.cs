using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using UchetGIC.DataFiles;
using UchetGIC.MiniWindows;

namespace UchetGIC.UserPages
{
    public partial class SeasonalEmployeePage : Page
    {
        private ObservableCollection<SeasonalEmployee> _seasonalEmployees;
        private Employees _currentEmployee;

        public SeasonalEmployeePage(Employees _employees)
        {
            _employees = _currentEmployee;
            InitializeComponent();
            LoadEmployees();
        }

        private void LoadEmployees()
        {
            CmbEmployee.ItemsSource = OdbConnectHelper.DbEntities.Employees.ToList();
            CmbEmployee.DisplayMemberPath = "LastName";
            CmbEmployee.SelectedValuePath = "EmployeeID";
        }

        private void CmbEmployee_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (CmbEmployee.SelectedValue is int employeeId)
            {
                LoadSeasonalEmployees(employeeId);
            }
        }

        private void LoadSeasonalEmployees(int employeeId)
        {
            _seasonalEmployees = new ObservableCollection<SeasonalEmployee>(OdbConnectHelper.DbEntities.SeasonalEmployee.Where(se => se.EmployeeID == employeeId).ToList());
            DataGridSeasonal.ItemsSource = _seasonalEmployees;
        }

        private void BtnAddSeasonal_Click(object sender, RoutedEventArgs e)
        {
            if (_currentEmployee == null)
            {
                MessageBox.Show("Сотрудник не выбран.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            var addSeasonalWindow = new AddSeasonalWindow(_currentEmployee.EmployeeID);
            if (addSeasonalWindow.ShowDialog() == true)
            {
                var newSeasonal = addSeasonalWindow.NewSeasonal;
                OdbConnectHelper.DbEntities.SeasonalEmployee.Add(newSeasonal);
                OdbConnectHelper.DbEntities.SaveChanges();
                LoadSeasonalEmployees(_currentEmployee.EmployeeID);
            }
        }
    }
}
