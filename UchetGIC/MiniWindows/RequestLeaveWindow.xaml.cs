using System;
using System.Windows;
using UchetGIC.DataFiles;

namespace UchetGIC.MiniWindows
{
    public partial class RequestLeaveWindow : Window
    {
        public Leave NewLeave { get; private set; }
        private int _employeeId;

        public RequestLeaveWindow(int employeeId)
        {
            InitializeComponent();
            _employeeId = employeeId;
            LoadLeaveTypes();
        }

        private void LoadLeaveTypes()
        {
            CmbLeaveType.Items.Clear();
            CmbLeaveType.ItemsSource = new[] { "Отпуск", "Больничный", "Декрет" };
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            if (CmbLeaveType.SelectedItem == null || DpStartDate.SelectedDate == null || DpEndDate.SelectedDate == null)
            {
                MessageBox.Show("Пожалуйста, заполните все поля.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            NewLeave = new Leave
            {
                EmployeeID = _employeeId,
                LeaveType = CmbLeaveType.SelectedItem.ToString(),
                StartDate = DpStartDate.SelectedDate.Value,
                EndDate = DpEndDate.SelectedDate.Value,
                Notes = TxtNotes.Text,
                Status = "Ожидание"
            };
            DialogResult = true;
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }
    }
}