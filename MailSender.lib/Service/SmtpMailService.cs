using System.Diagnostics;
using System.Net;
using System.Net.Mail;
using MailSender.lib.Interfaces;

namespace MailSender.lib.Service
{
    public class SmtpMailService : IMailService
    {
        public IMailSender GetSender(string Server, int Port, bool SSL, string Login, string Password)
        {
            return new SmtpMailSender(Server, Port, SSL, Login, Password);
        }
    }

    internal class SmtpMailSender : IMailSender
    {
        private readonly string _Address;
        private readonly int _Port;
        private readonly bool _SSL;
        private readonly string _Login;
        private readonly string _Password;

        public SmtpMailSender(string Address, int Port, bool SSL, string Login, string Password)
        {
            _Address = Address;
            _Port = Port;
            _SSL = SSL;
            _Login = Login;
            _Password = Password;
        }

        public void Send (string SenderAddress, string RecipientAddress, string Subject, string Body)
        {
            var from = new MailAddress(SenderAddress);      //от кого отправляем
            var to = new MailAddress(RecipientAddress);     //кому отправляем 

            //создаем почтовое отправление
            using (var message = new MailMessage(from, to))
            {
                message.Subject = Subject;      //тема письма
                message.Body = Body;            //текст письма

                //создаем клиента SMTP почты, через который будет отправляться почта
                using (var client = new SmtpClient(_Address, _Port))
                {
                    client.EnableSsl = _SSL;

                    //указываем учетные данные почты клиента
                    client.Credentials = new NetworkCredential
                    {
                        UserName = _Login,
                        Password = _Password
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
