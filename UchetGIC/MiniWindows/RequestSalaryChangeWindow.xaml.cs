using System;
using System.Linq;
using System.Windows;
using UchetGIC.DataFiles;

namespace UchetGIC.MiniWindows
{
    public partial class RequestSalaryChangeWindow : Window
    {
        private int _employeeId;

        public RequestSalaryChangeWindow(int employeeId)
        {
            InitializeComponent();
            _employeeId = employeeId;
            LoadCurrentSalary();
        }

        private void LoadCurrentSalary()
        {
            var salary = OdbConnectHelper.DbEntities.Salary.FirstOrDefault(s => s.EmployeeID == _employeeId && s.Status == "Текущая");
            if (salary != null)
            {
                TxtCurrentSalary.Text = salary.SalaryCount?.ToString() ?? "0";
            }
            else
            {
                TxtCurrentSalary.Text = "0";
            }
        }

        private void BtnSubmitRequest_Click(object sender, RoutedEventArgs e)
        {
            if (decimal.TryParse(TxtNewSalary.Text, out var newSalaryCount))
            {
                var currentSalary = OdbConnectHelper.DbEntities.Salary.FirstOrDefault(s => s.EmployeeID == _employeeId && s.Status == "Текущая");

                if (currentSalary != null)
                {
                    currentSalary.NewSalary = newSalaryCount;
                    currentSalary.Notes = TxtNotes.Text;
                    currentSalary.Status = "Ожидание";

                    OdbConnectHelper.DbEntities.SaveChanges();
                    DialogResult = true;
                }
                else
                {
                    MessageBox.Show("Текущая зарплата не найдена.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                MessageBox.Show("Пожалуйста, введите корректное значение зарплаты.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }
    }
}
