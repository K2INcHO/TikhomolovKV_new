using System.Windows;
using MailSender.lib;
using System.Net;
using System.Net.Mail;
using MailSender.Data;
using MailSender.Models;

namespace MailSender
{
    public partial class MainWindow
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void OnSendButtonClick(object Sender, RoutedEventArgs E)
        {
            var sender = SendersList.SelectedItem as Sender;
            if (sender is null) return;

            //проверка на пустоту заполнения темы и текста письма
            if (letterTitle.Text == "" || letterBody.Text == "")
            {
                tabControl.SelectedItem = tabLetters;
                MessageBox.Show("Письмо не заполнено!", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (!(RecipientsList.SelectedItem is Recipient recipient))
            {
                tabControl.SelectedItem = tabLists;
                MessageBox.Show("Не выбран адресат!", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (!(ServersList.SelectedItem is Server server)) return;
            if (!(MessagesList.SelectedItem is Message message)) return;

            var send_service = new MailSenderService
            {
                ServerAddress = server.Address,
                ServerPort = server.Port,
                UseSSL = server.UseSSL,
                Login = server.Login,
                Password = server.Password,
            };

            try
            {
                send_service.SendMessage(sender.Address, recipient.Address, message.Subject, message.Body);
            }
            catch (SmtpException error)
            {
                MessageBox.Show("Ошибка при отправке почты " + error.Message, "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnClock_Click(object sender, RoutedEventArgs e)
        {
            tabControl.SelectedItem = tabPlanner;
        }
    }
}
