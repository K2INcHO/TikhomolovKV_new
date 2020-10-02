using Microsoft.Extensions.DependencyInjection;

namespace MailSender.ViewModels
{
    //создаем специальный объект, который предназначен для того чтобы из любой точки разметки
    //   получить любую ViewModel
    class ViewModelLocator
    {
        //внутри будет набор свойств, каждое из которых представляет свою ViewModel
        public MainWindowViewModel MainWindowModel => App.Services.GetRequiredService<MainWindowViewModel>();

    }
}
