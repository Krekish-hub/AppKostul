using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using UchetGIC.AddonPages;
using UchetGIC.DataFiles;

namespace UchetGIC.ControllPages
{
    public partial class UserPage : Page
    {
        private ObservableCollection<Employees> _allEmployees;
        private CollectionViewSource _employeesViewSource;
        public UserPage()
        {
            InitializeComponent();
            LoadData();

            _employeesViewSource = new CollectionViewSource { Source = _allEmployees };
            _employeesViewSource.Filter += ApplyFilter;
            GridTabel.ItemsSource = _employeesViewSource.View;

            CmbFilterCompany.ItemsSource = OdbConnectHelper.DbEntities.Company.ToList();
            CmbFilterCompany.DisplayMemberPath = "NameCompany";
            CmbFilterCompany.SelectedValuePath = "CompanyID";

            CmbFilterPosition.ItemsSource = OdbConnectHelper.DbEntities.Role.ToList();
            CmbFilterPosition.DisplayMemberPath = "Name";
            CmbFilterPosition.SelectedValuePath = "ID";

            TxtFilterLastName.TextChanged += FilterChanged;
            CmbFilterCompany.SelectionChanged += FilterChanged;
            CmbFilterPosition.SelectionChanged += FilterChanged;
            DpFilterHireDateFrom.SelectedDateChanged += FilterChanged;
            DpFilterHireDateTo.SelectedDateChanged += FilterChanged;

            AddEmployeePage.EmployeeAdded += OnEmployeeAdded;
            EditEmployeePage.EmployeeUpdated += OnEmployeeUpdated; // Подписка на событие обновления
        }

        private void OnEmployeeUpdated(object sender, EventArgs e)
        {
            LoadData();
            _employeesViewSource.Source = _allEmployees;
        }


        private void LoadData()
        {
            _allEmployees = new ObservableCollection<Employees>(OdbConnectHelper.DbEntities.Employees.ToList());
        }

        private void OnEmployeeAdded(object sender, EventArgs e)
        {
            LoadData();
            _employeesViewSource.Source = _allEmployees;
        }

        private void ApplyFilter(object sender, FilterEventArgs e)
        {
            var emp = e.Item as Employees;
            if (emp == null) return;

            bool isMatch = true;

            if (!string.IsNullOrWhiteSpace(TxtFilterLastName.Text) && TxtFilterLastName.Text != "Фамилия")
            {
                isMatch &= emp.LastName.IndexOf(TxtFilterLastName.Text, StringComparison.OrdinalIgnoreCase) >= 0;
            }

            if (CmbFilterCompany.SelectedValue is int companyId)
            {
                isMatch &= emp.Company == companyId;
            }

            if (CmbFilterPosition.SelectedValue is int positionId)
            {
                isMatch &= emp.IDUser == positionId;
            }

            if (DpFilterHireDateFrom.SelectedDate is DateTime hireDateFrom)
            {
                isMatch &= emp.HireDate >= hireDateFrom;
            }

            if (DpFilterHireDateTo.SelectedDate is DateTime hireDateTo)
            {
                isMatch &= emp.HireDate <= hireDateTo;
            }

            e.Accepted = isMatch;
        }

        private void FilterChanged(object sender, EventArgs e)
        {
            _employeesViewSource.View.Refresh();
        }

        private void BtnResetFilters_Click(object sender, RoutedEventArgs e)
        {
            TxtFilterLastName.Text = "Фамилия";
            CmbFilterCompany.SelectedIndex = -1;
            CmbFilterPosition.SelectedIndex = -1;
            DpFilterHireDateFrom.SelectedDate = null;
            DpFilterHireDateTo.SelectedDate = null;

            _employeesViewSource.View.Refresh();
        }

        private void TxtFilter_GotFocus(object sender, RoutedEventArgs e)
        {
            var textBox = sender as TextBox;
            if (textBox.Text == "Фамилия")
            {
                textBox.Text = string.Empty;
                textBox.Foreground = new SolidColorBrush(Colors.Black);
            }
        }

        private void TxtFilter_LostFocus(object sender, RoutedEventArgs e)
        {
            var textBox = sender as TextBox;
            if (string.IsNullOrWhiteSpace(textBox.Text))
            {
                textBox.Text = "Фамилия";
                textBox.Foreground = new SolidColorBrush(Colors.Gray);
            }
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            FrameApp.FrameObj.Navigate(new AddEmployeePage());
        }

        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            if (GridTabel.SelectedItem is Employees selectedEmployee)
            {
                FrameApp.FrameObj.Navigate(new EditEmployeePage(selectedEmployee));
            }
            else
            {
                MessageBox.Show("Пожалуйста, выберите сотрудника для редактирования.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (GridTabel.SelectedItems.Count > 0)
                {
                    for (int i = 0; i < GridTabel.SelectedItems.Count; ++i)
                    {
                        Employees employeesObj = GridTabel.SelectedItems[i] as Employees;
                        OdbConnectHelper.DbEntities.Employees.Remove(employeesObj);
                    }
                    OdbConnectHelper.DbEntities.SaveChanges();
                    MessageBox.Show("Данные о сотруднике успешно удалены!", "Уведомление", MessageBoxButton.OK, MessageBoxImage.Information);
                    LoadData();
                    _employeesViewSource.Source = _allEmployees;
                }
                else
                {
                    MessageBox.Show("Данных в таблице нет!", "Уведомление", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Критическая ошибка в работе приложения:" + ex.Message, "Уведомление", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
    }
}