using System;
using System.Windows;
using System.Windows.Controls;
using UchetGIC.DataFiles;

namespace UchetGIC.MiniWindows
{
    public partial class AddLeaveWindow : Window
    {
        public Leave NewLeave { get; private set; }
        private int _employeeId;

        public AddLeaveWindow(int employeeId)
        {
            InitializeComponent();
            _employeeId = employeeId;
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
                LeaveType = (CmbLeaveType.SelectedItem as ComboBoxItem)?.Content.ToString(),
                StartDate = DpStartDate.SelectedDate.Value,
                EndDate = DpEndDate.SelectedDate.Value,
                Notes = TxtNotes.Text
            };

            DialogResult = true;
        }
    }
}