using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using UchetGIC.DataFiles;

namespace UchetGIC.MiniWindows
{
    /// <summary>
    /// Логика взаимодействия для AddLocalWindow.xaml
    /// </summary>
    public partial class AddLocalWindow : Window
    {
        public static event EventHandler LocalAdded;

        public ObservableCollection<Company> Companies { get; set; }
        public AddLocalWindow()
        {
            InitializeComponent();

            Companies = new ObservableCollection<Company>(OdbConnectHelper.DbEntities.Company.ToList());

            LocalAdded?.Invoke(this, EventArgs.Empty);
        }

        private void BtnAddLocal_OnClick(object sender, RoutedEventArgs e)
        {
            var localServ= new LocalServ
            {
                Number = Convert.ToInt16(TxtNumber.Text),
                Mail = Convert.ToString(TxtMail.Text)
                
            };

            OdbConnectHelper.DbEntities.LocalServ.Add(localServ);
            OdbConnectHelper.DbEntities.SaveChanges();

            LocalAdded?.Invoke(this, EventArgs.Empty);


            MessageBox.Show("Позиция успешно добавлена!",
                "Уведомление",
                MessageBoxButton.OK,
                MessageBoxImage.Information);

            Close();
        }
    }
}
