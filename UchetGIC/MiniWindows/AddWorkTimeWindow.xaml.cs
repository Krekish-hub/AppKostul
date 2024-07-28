using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using UchetGIC.DataFiles;

namespace UchetGIC.MiniWindows
{
    public partial class AddWorkTimeWindow : Window
    {
        public WorkTime NewWorkTime { get; private set; }
        private int _employeeId;
        private bool _isUpdating = false;

        public AddWorkTimeWindow(int employeeId)
        {
            InitializeComponent();
            _employeeId = employeeId;

            TxtStartTime.Text = "00:00:00";
            TxtEndTime.Text = "00:00:00";
            TxtBreakTime.Text = "00:00:00";
        }

        private void TextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            TextBox textBox = sender as TextBox;
            if (textBox != null && textBox.Text == "00:00:00")
            {
                textBox.Text = string.Empty;
            }
        }

        private void TextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            TextBox textBox = sender as TextBox;
            if (textBox != null && string.IsNullOrEmpty(textBox.Text))
            {
                textBox.Text = "00:00:00";
            }
        }

        private void TextBox_PreviewTextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            TextBox textBox = sender as TextBox;
            if (textBox == null || !Regex.IsMatch(e.Text, @"^[0-9]$")) return;

            e.Handled = true;

            int caretIndex = textBox.CaretIndex;
            string text = textBox.Text.Remove(caretIndex, textBox.SelectionLength).Insert(caretIndex, e.Text);
            text = text.Replace(":", "");

            if (text.Length > 6)
            {
                text = text.Substring(0, 6);
            }

            _isUpdating = true;
            textBox.Text = FormatTime(text);
            textBox.CaretIndex = Math.Min(caretIndex + 1, textBox.Text.Length);
            _isUpdating = false;
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (_isUpdating) return;

            TextBox textBox = sender as TextBox;
            if (textBox == null) return;

            int caretIndex = textBox.CaretIndex;
            string text = textBox.Text.Replace(":", "");

            if (text.Length > 6)
            {
                text = text.Substring(0, 6);
            }

            _isUpdating = true;
            textBox.Text = FormatTime(text);
            textBox.CaretIndex = caretIndex;
            _isUpdating = false;
        }

        private string FormatTime(string input)
        {
            input = input.PadRight(6, '0');
            return input.Insert(2, ":").Insert(5, ":");
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            if (DpDate.SelectedDate == null || 
                string.IsNullOrEmpty(TxtStartTime.Text) || 
                string.IsNullOrEmpty(TxtEndTime.Text) || 
                string.IsNullOrEmpty(TxtBreakTime.Text))
            {
                MessageBox.Show("Пожалуйста, заполните все поля.", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            try
            {
                TimeSpan startTime = TimeSpan.Parse(TxtStartTime.Text);
                TimeSpan endTime = TimeSpan.Parse(TxtEndTime.Text);
                TimeSpan breakTime = TimeSpan.Parse(TxtBreakTime.Text);

                if (endTime < startTime)
                {
                    MessageBox.Show("Время окончания не может быть раньше времени начала.",
                        "Ошибка", 
                        MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                TimeSpan totalTime = endTime - startTime - breakTime;

                if (totalTime < TimeSpan.Zero)
                {
                    MessageBox.Show("Общее время не может быть отрицательным.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                NewWorkTime = new WorkTime
                {
                    EmployeeID = _employeeId,
                    Date = DpDate.SelectedDate.Value,
                    StartTime = startTime,
                    EndTime = endTime,
                    BreakTime = breakTime,
                    TotalTime = totalTime
                };

                TxtTotalTime.Text = totalTime.ToString(@"hh\:mm\:ss");

                DialogResult = true;
            }
            catch (FormatException)
            {
                MessageBox.Show("Неверный формат времени. Пожалуйста, используйте формат чч:мм:сс.", 
                    "Ошибка", 
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Произошла ошибка: {ex.Message}", "Ошибка", MessageBoxButton.OK, 
                    MessageBoxImage.Error);
            }
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }
    }
}
