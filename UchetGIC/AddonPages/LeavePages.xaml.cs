using System.Windows.Controls;
using UchetGIC.DataFiles;

namespace UchetGIC.AddonPages
{
    /// <summary>
    /// Логика взаимодействия для LeavePages.xaml
    /// </summary>
    public partial class LeavePages : Page
    {
        private Employees _currentEmployee;

        public LeavePages(Employees employee)
        {
            InitializeComponent();

            _currentEmployee = employee;
            
        }
    }
}
