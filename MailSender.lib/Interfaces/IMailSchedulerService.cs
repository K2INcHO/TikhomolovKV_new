using MailSender.lib.Models;
using System;
using System.Collections.Generic;

namespace MailSender.lib.Interfaces
{
    public interface IMailSchedulerService
    {
        void Start();

        void Stop();

        void AddTask(DateTime Time, Sender Sender, IEnumerable<Recipient> Recipients, Server Server, Message Message);
    }
}
