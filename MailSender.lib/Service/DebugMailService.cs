﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using MailSender.lib.Interfaces;

namespace MailSender.lib.Service
{
    public class DebugMailService : IMailService
    {
        private readonly IEncryptorService _EncryptorService;

        public DebugMailService(IEncryptorService EncryptorService)
        {
            _EncryptorService = EncryptorService;
        }

        public IMailSender GetSender(string Server, int Port, bool SSL, string Login, string Password)
        {
            return new DebugMailSender(Server, Port, SSL, Login, Password);
        }

        private class DebugMailSender : IMailSender
        {
            private readonly string _Address;
            private readonly int _Port;
            private readonly bool _SSL;
            private readonly string _Login;
            private readonly string _Password;

            public DebugMailSender(string Address, int Port, bool SSL, string Login, string Password)
            {
                _Address = Address;
                _Port = Port;
                _SSL = SSL;
                _Login = Login;
                _Password = Password;
            }

            public void Send(string SenderAddress, string RecipientAddress, string Subject, string Body)
            {
                Debug.WriteLine("Отправка почты через сервер {0}:{1} SSL:{2} (Login:{3}; Pass:{4})",
                    _Address, _Port, _SSL, _Login, _Password);
                Debug.WriteLine("Сообщение от {0} к {1}:\r\n{2}\r\n{3}",
                    SenderAddress, RecipientAddress, Subject, Body);
            }

            public void Send(string SenderAddress, IEnumerable<string> RecipientsAddress, string Subject, string Body)
            {
                foreach (var recipient_address in RecipientsAddress)
                    Send(SenderAddress, recipient_address, Subject, Body);
            }

            public void SendParallel(string SenderAddress, IEnumerable<string> RecipientsAddress, string Subject, string Body)
            {
                Send(SenderAddress, RecipientsAddress, Subject, Body);
            }

            public async Task SendAsync(string SenderAddress, string RecipientAddress, string Subject, string Body, CancellationToken Cancel = default)
            {
                
            }

            public async Task SendAsync(string SenderAddress, IEnumerable<string> RecipientsAddress, string Subject, string Body, System.IProgress<(string Recipient, double Percent)> Progress = null, CancellationToken Cancel = default)
            {
                
            }

            public async Task SendParallelAsync(string SenderAddress, IEnumerable<string> RecipientsAddress, string Subject, string Body, CancellationToken Cancel = default)
            {
                
            }
        }
    }
}
