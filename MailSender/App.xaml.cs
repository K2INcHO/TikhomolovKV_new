using System;
using MailSender.lib.Interfaces;
using MailSender.lib.Service;
using MailSender.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace MailSender
{
    //создаем контейнер сервисов для их последующего использования в приложении
    public partial class App
    {
        //создаем хостинг, в котором будет "жить" контейнер
        private static IHost _Hosting;

        //создаем приложение при помощи "построителя хоста"
        public static IHost Hosting => _Hosting
            ??= Host.CreateDefaultBuilder(Environment.GetCommandLineArgs())
            .ConfigureServices(ConfigureServices)
            .Build();

        public static IServiceProvider Services => Hosting.Services;

        private static void ConfigureServices(HostBuilderContext host, IServiceCollection services)
        {
            services.AddSingleton<MainWindowViewModel>();
            services.AddTransient<IMailService, SmtpMailService>();
            //services.AddScoped<>();
        }
    }
}
