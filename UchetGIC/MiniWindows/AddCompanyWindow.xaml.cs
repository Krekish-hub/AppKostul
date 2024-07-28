using System;
using System.Windows;
using UchetGIC.DataFiles;

namespace UchetGIC.MiniWindows
{
    /// <summary>
    /// Логика взаимодействия для AddCompanyWindow.xaml
    /// </summary>
    public partial class AddCompanyWindow : Window
    {
        public static event EventHandler CompanyAdded;

        public AddCompanyWindow()
        {
            InitializeComponent();
        }

        private void BtnAddCompany_OnClick(object sender, RoutedEventArgs e)
        {
            var CompanyObj = new Company()
            {
                NameCompany = TxtNameCompany.Text,
                City = TxtCity.Text,
                Address = TxtAdress.Text
            };

            OdbConnectHelper.DbEntities.Company.Add(CompanyObj);
            OdbConnectHelper.DbEntities.SaveChanges();
            
            CompanyAdded?.Invoke(this, EventArgs.Empty);

            MessageBox.Show("Компания добавлена.");
            this.Close();
        }
    }
}
