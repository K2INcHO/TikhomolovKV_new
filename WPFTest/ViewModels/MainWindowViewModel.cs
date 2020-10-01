using System;
using System.Timers;
using System.Windows;
using System.Windows.Input;
//using WPFTests.Infrasrtructure.Commands;
using WPFTest.ViewModels.Base;

namespace WPFTest.ViewModels
{
    class MainWindowViewModel : ViewModel
    {
        public string _Title = "Тестовое окно";

        public string Title
        {
            get => _Title;
            set => Set(ref _Title, value); //пишем более коротко
            //set
            //{
            //    if (_Title == value) return;
            //    _Title = value;
            //    //установка имени свойства
            //    //OnPropertyChanged("Title");
            //    //OnPropertyChanged(nameof(Title)); //данный вариант предпочтительнее
            //    OnPropertyChanged(); //а так проще, так как добавили атрибут [CallerMemberName] во ViewModel
            //    //атрибуты - специальные пометки внутри кода, которые интегрируются в результаты компиляции
            //}
        }

        public DateTime CurrentTime => DateTime.Now;

        private bool _TimerEnabled = true;

        public bool TimerEnabled
        {
            get => _TimerEnabled;
            set
            {
                if (!Set(ref _TimerEnabled, value)) return;
                _Timer.Enabled = value;
            }
        }
        
        //создаем таймер
        private readonly Timer _Timer;

        public MainWindowViewModel()
        {
            _Timer = new Timer(100);
            _Timer.Elapsed += OnTimerElapsed;
            _Timer.AutoReset = true;
            _Timer.Enabled = true;
        }

        private void OnTimerElapsed(object Sender, ElapsedEventArgs E)
        {
            OnPropertyChanged(nameof(CurrentTime));
        }
    }
}
