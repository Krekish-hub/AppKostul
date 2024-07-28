using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using UchetGIC.DataFiles;
using UchetGIC.MiniWindows;

namespace UchetGIC.UserPages
{
    public partial class MealSchedulePage : Page
    {
        private ObservableCollection<MealSchedule> _meals;
        private Employees _currentEmployee;

        public MealSchedulePage(Employees _employees)
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
                LoadMeals(employeeId);
            }
        }

        private void LoadMeals(int employeeId)
        {
            _meals = new ObservableCollection<MealSchedule>(OdbConnectHelper.DbEntities.MealSchedule.Where(m => m.EmployeeID == employeeId).ToList());
            DataGridMeals.ItemsSource = _meals;
        }

        private void BtnAddMeal_Click(object sender, RoutedEventArgs e)
        {
            if (_currentEmployee == null)
            {
                MessageBox.Show("Сотрудник не выбран.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            var addMealWindow = new AddMealWindow(_currentEmployee.EmployeeID);
            if (addMealWindow.ShowDialog() == true)
            {
                var newMeal = addMealWindow.NewMeal;
                OdbConnectHelper.DbEntities.MealSchedule.Add(newMeal);
                OdbConnectHelper.DbEntities.SaveChanges();
                LoadMeals(_currentEmployee.EmployeeID);
            }
        }
    }
}