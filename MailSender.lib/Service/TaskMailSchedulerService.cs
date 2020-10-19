using MailSender.lib.Interfaces;
using MailSender.lib.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MailSender.lib.Service
{
    public class TaskMailSchedulerService : IMailSchedulerService
    {
        private readonly IStore<SchedulerTask> _TaskStore;
        private readonly IMailService _MailService;
        private Task _SchedulerTask;
        private readonly CancellationTokenSource _TaskCancellation = new CancellationTokenSource();

        public TaskMailSchedulerService(IStore<SchedulerTask> TaskStore, IMailService MailService)
        {
            _TaskStore = TaskStore;
            _MailService = MailService;
        }

        public void Start()
        {
            //var scheduler_task = Task.Run(SchedulerTaskMethod);
            _SchedulerTask = Task.Factory.StartNew(SchedulerTaskMethodAsync, TaskCreationOptions.LongRunning);
        }

        public void Stop() => _TaskCancellation.Cancel();

        private async void SchedulerTaskMethodAsync()
        {
            var cancel = _TaskCancellation.Token;
            while (true)
            {
                cancel.ThrowIfCancellationRequested();

                var next_task = _TaskStore.GetAll()
                    .OrderBy(t => t.Time)
                    .FirstOrDefault(t => t.Time > DateTime.Now);

                if (next_task is null) break;

                TimeSpan sleep_time = next_task.Time - DateTime.Now;

                if (sleep_time.TotalMilliseconds > 0)
                    await Task.Delay(sleep_time, cancel);

                await Execute(next_task);
            }
        }

        private async Task Execute (SchedulerTask task)
        {
            var server = task.Server;
            var sender = _MailService.GetSender(server.Address, server.Port, server.UseSSL, server.Login, server.Password);
            await sender.SendAsync(
                task.Sender.Address, task.Recipients.Select(r => r.Address), 
                task.Message.Subject, task.Message.Body
                );
        }

        public void AddTask(DateTime Time, Sender Sender, IEnumerable<Recipient> Recipients, Server Server, Message Message)
        {
            Stop();

            _TaskStore.Add(new SchedulerTask
            {
                Message = Message,
                Recipients = Recipients.ToArray(),
                Server = Server,
                Sender = Sender,
                Time = Time
            });

            Start();
        }
    }
}
