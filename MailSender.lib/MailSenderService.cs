using System.Diagnostics;
using System.Net;
using System.Net.Mail;

namespace MailSender.lib
{
    public class MailSenderService
    {
        public string ServerAddress { get; set; }

        public int ServerPort { get; set; }

        public bool UseSSL { get; set; }

        public string Login { get; set; }

        public string Password { get; set; }

        public void SendMessage(string SenderAddress, string RecipientAddress, string Subject, string Body)
        {
            var from = new MailAddress(SenderAddress);      //от кого отправляем
            var to = new MailAddress(RecipientAddress);     //кому отправляем 

            //создаем почтовое отправление
            using (var message = new MailMessage(from, to))
            {
                message.Subject = Subject;      //тема письма
                message.Body = Body;            //текст письма

                //создаем клиента SMTP почты, через который будет отправляться почта
                using (var client = new SmtpClient(ServerAddress, ServerPort))
                {
                    client.EnableSsl = UseSSL;

                    //указываем учетные данные почты клиента
                    client.Credentials = new NetworkCredential
                    {
                        UserName = Login,
                        Password = Password
                    };

                    //отправляем сообщение
                    try
                    {
                        client.Send(message);
                    }
                    catch (SmtpException e)
                    {
                        Trace.TraceError(e.ToString());
                        throw;
                    }
                }
            }
        }
    }
}
