using System;
using System.Linq;
using System.Windows;
using UchetGIC.DataFiles;

namespace UchetGIC.MiniWindows
{
    public partial class AddPositionWindow : Window
    {
        public static event EventHandler PositionAdded;

        public AddPositionWindow()
        {
            InitializeComponent();
            LoadComboBoxes();
        }

        private void LoadComboBoxes()
        {
            // Получаем уникальные значения Level из базы данных
            var levels = OdbConnectHelper.DbEntities.Role
                .Select(role => role.Level)
                .Distinct()
                .Where(level => level.HasValue) // Убираем значения, которые могут быть null
                .Select(level => level.Value)   // Преобразуем Nullable<int> в int
                .ToList();

            // Привязываем данные к ComboBox
            CmbLvl.ItemsSource = levels;
        }

        private void BtnAddPosition_Click(object sender, RoutedEventArgs e)
        {
            // Извлекаем выбранный уровень как числовое значение
            if (CmbLvl.SelectedItem is int selectedLevel)
            {
                var role = new Role
                {
                    Name = TxtRole.Text,
                    Level = selectedLevel
                };

                OdbConnectHelper.DbEntities.Role.Add(role);
                OdbConnectHelper.DbEntities.SaveChanges();

                PositionAdded?.Invoke(this, EventArgs.Empty);

                MessageBox.Show("Позиция успешно добавлена!",
                    "Уведомление",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information);

                Close();
            }
            else
            {
                MessageBox.Show("Пожалуйста, выберите уровень.",
                    "Ошибка",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }
    }
}