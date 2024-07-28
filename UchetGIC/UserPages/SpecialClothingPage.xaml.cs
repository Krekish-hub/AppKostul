using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using UchetGIC.DataFiles;
using UchetGIC.MiniWindows;

namespace UchetGIC.UserPages
{
    public partial class SpecialClothingPage : Page
    {
        private ObservableCollection<SpecialClothing> _clothing;
        private Employees _currentEmployee;

        public SpecialClothingPage(Employees _employees)
        {
            _currentEmployee = _employees;
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
                LoadClothing(employeeId);
            }
        }

        private void LoadClothing(int employeeId)
        {
            _clothing = new ObservableCollection<SpecialClothing>(OdbConnectHelper.DbEntities.SpecialClothing.Where(c => c.EmployeeID == employeeId).ToList());
            DataGridClothing.ItemsSource = _clothing;
        }

        private void BtnAddClothing_Click(object sender, RoutedEventArgs e)
        {
            if (_currentEmployee == null)
            {
                MessageBox.Show("Сотрудник не выбран.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            var addClothingWindow = new AddClothingWindow(_currentEmployee.EmployeeID);
            if (addClothingWindow.ShowDialog() == true)
            {
                var newClothing = addClothingWindow.NewClothing;
                OdbConnectHelper.DbEntities.SpecialClothing.Add(newClothing);
                OdbConnectHelper.DbEntities.SaveChanges();
                LoadClothing(_currentEmployee.EmployeeID);
            }
        }
    }
}
