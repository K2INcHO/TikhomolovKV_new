using System;
using System.Collections.Generic;
using System.Text;
using MailSender.ViewModels.Base;

namespace MailSender.ViewModels
{
    class StatisticViewModel : ViewModel
    {
        #region SendMessagesCount : TYPE - DESCRIPTION

        private int _SendMessagesCount;

        public int SendMessagesCount { get => _SendMessagesCount; private set => Set(ref _SendMessagesCount, value); }

        #endregion

        public void MessageSended() => SendMessagesCount++;

    }
}
