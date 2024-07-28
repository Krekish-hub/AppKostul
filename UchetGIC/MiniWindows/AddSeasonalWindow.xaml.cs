using System;
using System.Windows;
using UchetGIC.DataFiles;

namespace UchetGIC.MiniWindows
{
    public partial class AddSeasonalWindow : Window
    {
        public SeasonalEmployee NewSeasonal { get; private set; }
        private int _employeeId;

        public AddSeasonalWindow(int employeeId)
        {
            InitializeComponent();
            _employeeId = employeeId;
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(TxtWorkType.Text) || DpStartDate.SelectedDate == null || DpEndDate.SelectedDate == null || string.IsNullOrEmpty(TxtSchedule.Text))
            {
                MessageBox.Show("Пожалуйста, заполните все поля.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            NewSeasonal = new SeasonalEmployee
            {
                EmployeeID = _employeeId,
                WorkType = TxtWorkType.Text,
                StartDate = DpStartDate.SelectedDate.Value,
                EndDate = DpEndDate.SelectedDate.Value,
                Schedule = TxtSchedule.Text
            };

            DialogResult = true;
        }
    }
}
