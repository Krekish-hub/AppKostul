using System;
using System.Windows;
using System.Windows.Controls;
using UchetGIC.DataFiles;

namespace UchetGIC.MiniWindows
{
    public partial class AddMealWindow : Window
    {
        public MealSchedule NewMeal { get; private set; }
        private int _employeeId;

        public AddMealWindow(int employeeId)
        {
            InitializeComponent();
            _employeeId = employeeId;
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            if (DpDate.SelectedDate == null || CmbMealType.SelectedItem == null || string.IsNullOrEmpty(TxtPortionCount.Text))
            {
                MessageBox.Show("Пожалуйста, заполните все поля.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            NewMeal = new MealSchedule
            {
                EmployeeID = _employeeId,
                Date = DpDate.SelectedDate.Value,
                MealType = (CmbMealType.SelectedItem as ComboBoxItem)?.Content.ToString(),
                PortionCount = int.Parse(TxtPortionCount.Text)
            };

            DialogResult = true;
        }
    }
}
