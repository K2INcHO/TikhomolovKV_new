using WPFTest.ViewModels.Base;

namespace WPFTest.ViewModels
{
    class MainWindowViewModel : ViewModel
    {
        public string _Title = "Тестовое окно";

        public string Title
        {
            get => _Title;
            set
            {
                if (_Title == value) return;
                _Title = value;
                OnPropertyChanged("Title");
            }
        }
    }
}
