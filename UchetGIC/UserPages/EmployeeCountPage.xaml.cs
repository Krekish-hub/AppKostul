using System.Linq;
using System.Windows.Controls;
using UchetGIC.DataFiles;

namespace UchetGIC.UserPages
{
    public partial class EmployeeCountPage : Page
    {
        public EmployeeCountPage()
        {
            InitializeComponent();
            LoadEmployeeCounts();
        }

        private void LoadEmployeeCounts()
        {
            var employeeCounts = OdbConnectHelper.DbEntities.Objects
                .Select(o => new
                {
                    ObjectName = o.Name,
                    EmployeeCount = o.EmployeeObject.Count
                }).ToList();
            DataGridEmployeeCount.ItemsSource = employeeCounts;
        }
    }
}