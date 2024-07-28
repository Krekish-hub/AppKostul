using System;
using System.Net;
using System.Net.Mail;
using System.Windows;
using System.Windows.Controls;
using UchetGIC.DataFiles;

namespace UchetGIC.UserPages
{
    public partial class FeedbackPage : Page
    {
        private Employees _currentEmployee;

        public FeedbackPage(Employees employee)
        {
            InitializeComponent();
            _currentEmployee = employee;
        }

        private void BtnSendFeedback_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var feedback = new Feedback
                {
                    EmployeeID = _currentEmployee.EmployeeID,
                    FeedbackText = TxtFeedback.Text,
                    DateSubmitted = DateTime.Now
                };

                OdbConnectHelper.DbEntities.Feedback.Add(feedback);
                OdbConnectHelper.DbEntities.SaveChanges();

                SendEmail(feedback);

                MessageBox.Show("Ваше сообщение отправлено.", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                TxtFeedback.Clear();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при отправке сообщения: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void SendEmail(Feedback feedback) 
        {
            var fromAddress = new MailAddress("krekish@list.ru", "krekish");
            var toAddress = new MailAddress("shilingovskiy.nik@mail.ru", "error");
            const string fromPassword = "8wEhE0BFHJNzezdwPUt6";
            const string subject = "Новое сообщение обратной связи";
            string body = $"Сообщение от сотрудника {feedback.EmployeeID}:\n\n{feedback.FeedbackText}";

            var smtp = new SmtpClient
            {
                Host = "smtp.mail.ru",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(fromAddress.Address, fromPassword)
            };

            using (var message = new MailMessage(fromAddress, toAddress)
            {
                Subject = subject,
                Body = body
            })
            {
                smtp.Send(message);
            }
        }
    }
}
