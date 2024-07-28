using System;
using System.Windows;
using UchetGIC.DataFiles;

namespace UchetGIC.MiniWindows
{
    public partial class AddClothingWindow : Window
    {
        public SpecialClothing NewClothing { get; private set; }
        private int _employeeId;

        public AddClothingWindow(int employeeId)
        {
            InitializeComponent();
            _employeeId = employeeId;
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(TxtClothingType.Text) || DpIssueDate.SelectedDate == null || string.IsNullOrEmpty(TxtCondition.Text))
            {
                MessageBox.Show("Пожалуйста, заполните все поля.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            NewClothing = new SpecialClothing
            {
                EmployeeID = _employeeId,
                ClothingType = TxtClothingType.Text,
                IssueDate = DpIssueDate.SelectedDate.Value,
                Condition = TxtCondition.Text
            };

            DialogResult = true;
        }
    }
}
