using MailSender.ViewModels.Base;

namespace MailSender.ViewModels
{
    class MainWindowViewModel : ViewModel
    {
        public string _Title = "Тестовое окно";

        public string Title
        {
            get => _Title;
            set => Set(ref _Title, value);
        }
    }
}
