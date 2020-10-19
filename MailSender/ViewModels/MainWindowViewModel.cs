using System.Collections.ObjectModel;
using System;
using System.Windows;
using System.Windows.Input;
using MailSender.Data;
using MailSender.lib.Models;
using MailSender.ViewModels.Base;
using MailSender.Infrastructure.Commands; //нужно чтобы подцепить LambdaCommand
using System.Linq;
using MailSender.lib.Interfaces;
using Microsoft.Extensions.Configuration;

namespace MailSender.ViewModels
{
    class MainWindowViewModel : ViewModel
    {
        private readonly IMailService _MailService;
        private readonly IStore<Recipient> _RecipientsStore;
        private readonly IStore<Sender> _SendersStore;
        private readonly IStore<Message> _MessagesStore;
        private readonly IStore<Server> _ServersStore;
        private readonly IStore<SchedulerTask> _SchedulerTasksStore;
        private readonly IMailSchedulerService _MailSchedulerService;

        public StatisticViewModel Statistic { get; } = new StatisticViewModel();


        public string _Title = "Тестовое окно";

        public string Title
        {
            get => _Title;
            set => Set(ref _Title, value);
        }

        //модель представления будет хранить свои данные в следующих полях
        private ObservableCollection<Server> _Servers;
        private ObservableCollection<Sender> _Senders;
        private ObservableCollection<Recipient> _Recipients;
        private ObservableCollection<Message> _Messages;

        //создаем свойства, которые будут управлять этими полями
        #region Свойства
        public ObservableCollection<Server> Servers
        {
            get => _Servers;
            set => Set(ref _Servers, value);
        }

        public ObservableCollection<Sender> Senders
        {
            get => _Senders;
            set => Set(ref _Senders, value);
        }

        public ObservableCollection<Recipient> Recipients
        {
            get => _Recipients;
            set => Set(ref _Recipients, value);
        }

        public ObservableCollection<Message> Messages
        {
            get => _Messages;
            set => Set(ref _Messages, value);
        }

        private Server _SelectedServer;

        public Server SelectedServer
        {
            get => _SelectedServer;
            set => Set(ref _SelectedServer, value);
        }

        private Sender _SelectedSender;

        public Sender SelectedSender
        {
            get => _SelectedSender;
            set => Set(ref _SelectedSender, value);
        }

        private Recipient _SelectedRecipient;

        public Recipient SelectedRecipient
        {
            get => _SelectedRecipient;
            set => Set(ref _SelectedRecipient, value);
        }

        private Message _SelectedMessage;

        public Message SelectedMessage
        {
            get => _SelectedMessage;
            set => Set(ref _SelectedMessage, value);
        }

        #endregion

        private ICommand _LoadDataCommand;

        public ICommand LoadDataCommand => _LoadDataCommand
            ??= new LambdaCommand(OnLoadDataCommandExecuted, CanLoadCommandExecute);

        private bool CanLoadCommandExecute(object p) => true;

        private void OnLoadDataCommandExecuted(object p)
        {
            Servers = new ObservableCollection<Server>(_ServersStore.GetAll());
            Senders = new ObservableCollection<Sender>(_SendersStore.GetAll());
            Recipients = new ObservableCollection<Recipient>(_RecipientsStore.GetAll());
            Messages = new ObservableCollection<Message>(_MessagesStore.GetAll());
        }
        #region Команды

        //    #region Команда_1 - CreateNewServerCommand
        //        private ICommand _CreateNewServerCommand;

        //        public ICommand CreateNewServerCommand => _CreateNewServerCommand
        //            ??= new LambdaCommand(OnCreateNewServerCommandExecuted, CanCreateNewServerCommandExecute);

        //        private bool CanCreateNewServerCommandExecute(object p) => true;
            
        //        //основное действие, выполняемое командой
        //        private void OnCreateNewServerCommandExecuted(object p)
        //        {
        //            MessageBox.Show("Создание нового сервера!", "Управление серверами");
        //        }
        //    #endregion

        //    #region Команда_2 - EditServerCommand
        //        private ICommand _EditServerCommand;

        //        public ICommand EditServerCommand => _EditServerCommand
        //            ??= new LambdaCommand(OnEditServerCommandExecuted, CanEditServerCommandExecute);

        //        private bool CanEditServerCommandExecute(object p) => p is Server ||  SelectedServer != null;

        //        //основное действие, выполняемое командой
        //        private void OnEditServerCommandExecuted(object p)
        //        {
        //            //var server = SelectedServer;  //первый вариант привязки

        //            var server = p as Server ?? SelectedServer;    //второй вариант привязки
        //            if (server is null) return;

        //            MessageBox.Show($"Редактирование сервера! {server.Address}", "Управление серверами");
        //        }
        //    #endregion

        //    #region Команда_3 - DeleteServerCommand
        //        private ICommand _DeleteServerCommand;

        //        public ICommand DeleteServerCommand => _DeleteServerCommand
        //            ??= new LambdaCommand(OnDeleteServerCommandExecuted, CanDeleteServerCommandExecute);

        //        private bool CanDeleteServerCommandExecute(object p) => p is Server || SelectedServer != null;

        //        //основное действие, выполняемое командой
        //        private void OnDeleteServerCommandExecuted(object p)
        //        {
        //            var server = p as Server ?? SelectedServer;    //второй вариант привязки
        //            if (server is null) return;

        //            Servers.Remove(server);
        //            SelectedServer = Servers.FirstOrDefault();

        //            //MessageBox.Show("Удаление сервера!", "Управление серверами");
        //        }
        //#endregion

        //    #region Команда_4 - SendMailCommand
        //        private ICommand _SendMailCommand;

        //        public ICommand SendMailCommand => _SendMailCommand
        //            ??= new LambdaCommand(OnSendMailCommandExecuted, CanSendMailCommandExecute);

        //        private bool CanSendMailCommandExecute(object p) 
        //        {
        //            if (SelectedServer is null) return false;
        //            if (SelectedSender is null) return false;
        //            if (SelectedRecipient is null) return false;
        //            if (SelectedMessage is null) return false;
        //            return true;
        //        }

        //        //основное действие, выполняемое командой
        //        private void OnSendMailCommandExecuted(object p)
        //        {
        //            var server = SelectedServer;
        //            var sender = SelectedSender;
        //            var recipient = SelectedRecipient;
        //            var message = SelectedMessage;
        //            var mail_sender = _MailService.GetSender(server.Address, server.Port, server.UseSSL, server.Login, server.Password);

        //            mail_sender.Send(sender.Address, recipient.Address, message.Subject, message.Body);

        //            Statistic.MessageSended();
        //        }
        //#endregion

        #endregion

        //инициализируем эти свойства в конструкторе, чтобы туда попали эти данные
        //public MainWindowViewModel(IMailService MailService, IConfiguration config)

        public MainWindowViewModel(IMailService MailService, 
            IStore<Recipient> RecipientsStore,
            IStore<Sender> SendersStore,
            IStore<Message> MessagesStore,
            IStore<Server> ServersStore,
            IStore<SchedulerTask> SchedulerTasksStore,
            IMailSchedulerService MailSchedulerService
            )
        {

            //Unit of Work
            _RecipientsStore = RecipientsStore;
            _SendersStore = SendersStore;
            _MessagesStore = MessagesStore;
            _ServersStore = ServersStore;
            _SchedulerTasksStore = SchedulerTasksStore;
            _MailService = MailService;
            _MailSchedulerService = MailSchedulerService;
        }
    }
}
