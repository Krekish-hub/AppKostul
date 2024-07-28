using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using UchetGIC.DataFiles;
using UchetGIC.MenuController;

namespace UchetGIC.MiniWindows
{
    public partial class ApproveRequestsWindow : Window
    {
        private ObservableCollection<Salary> _salaryRequests;

        public ApproveRequestsWindow()
        {
            InitializeComponent();
            LoadRequests();
        }

        private void LoadRequests()
        {
            var leaveRequests = OdbConnectHelper.DbEntities.Leave.Where(lr => lr.Status == "Ожидание").ToList();
            LeaveRequestsDataGrid.ItemsSource = leaveRequests;

            _salaryRequests = new ObservableCollection<Salary>(OdbConnectHelper.DbEntities.Salary.Where(s => s.Status == "Ожидание").ToList());
            SalaryRequestsDataGrid.ItemsSource = _salaryRequests;
        }

        private void BtnApproveLeave_Click(object sender, RoutedEventArgs e)
        {
            if (LeaveRequestsDataGrid.SelectedItem is Leave leaveRequest)
            {
                leaveRequest.Status = "Одобрено";
                OdbConnectHelper.DbEntities.SaveChanges();
                LoadRequests();
                ((MenuControllPage)Application.Current.MainWindow.Content).UpdatePendingRequestsCount();
            }
        }

        private void BtnRejectLeave_Click(object sender, RoutedEventArgs e)
        {
            if (LeaveRequestsDataGrid.SelectedItem is Leave leaveRequest)
            {
                leaveRequest.Status = "Отказано";
                OdbConnectHelper.DbEntities.SaveChanges();
                LoadRequests();
                ((MenuControllPage)Application.Current.MainWindow.Content).UpdatePendingRequestsCount();
            }
        }

        private void BtnApproveSalary_Click(object sender, RoutedEventArgs e)
        {
            if (SalaryRequestsDataGrid.SelectedItem is Salary selectedRequest)
            {
                var employee = OdbConnectHelper.DbEntities.Employees.FirstOrDefault(emp => emp.EmployeeID == selectedRequest.EmployeeID);
                if (employee != null)
                {
                    // Обновить текущую зарплату сотрудника
                    employee.Salary1.SalaryCount = selectedRequest.NewSalary;
                    selectedRequest.Status = "Одобрено";
                    OdbConnectHelper.DbEntities.SaveChanges();
                    LoadRequests();
                    ((MenuControllPage)Application.Current.MainWindow.Content).UpdatePendingRequestsCount();
                    MessageBox.Show("Заявка одобрена.", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            else
            {
                MessageBox.Show("Пожалуйста, выберите заявку для одобрения.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void BtnRejectSalary_Click(object sender, RoutedEventArgs e)
        {
            if (SalaryRequestsDataGrid.SelectedItem is Salary selectedRequest)
            {
                selectedRequest.Status = "Отклонено";
                OdbConnectHelper.DbEntities.SaveChanges();
                LoadRequests();
                ((MenuControllPage)Application.Current.MainWindow.Content).UpdatePendingRequestsCount();
                MessageBox.Show("Заявка отклонена.", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                MessageBox.Show("Пожалуйста, выберите заявку для отклонения.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
