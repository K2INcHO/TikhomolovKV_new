using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace WPFTest.Infrastructure.Commands.Base
{
    abstract class Command : ICommand
    {
        //метод который выполняет непосредственно действия, порученные команде (например, закрыть окно)
        public event EventHandler CanExecuteChanged 
        {
            add => CommandManager.RequerySuggested += value;
            remove => CommandManager.RequerySuggested -= value;
        }

        //функция, которая возвращает либо истину, либо ложь. 
        //Если команда может выполниться, она должна вернуть истину, если ее выполнить нельзя, то она вернет ложь
        bool ICommand.CanExecute(object parameter) => CanExecute(parameter);

        void ICommand.Execute(object parameter) => Execute(parameter);

        protected virtual bool CanExecute(object p) => true;

        protected abstract void Execute(object p);
    }
}
