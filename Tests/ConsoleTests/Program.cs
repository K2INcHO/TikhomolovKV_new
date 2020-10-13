using System;
using System.Threading.Tasks;

namespace ConsoleTests
{
    class Program
    {
        static void Main(string[] args)
        {
            //TPLOverview.Start();
            var task = AsyncAwayTest.StartAsync();
            var proccess_messages_task = AsyncAwayTest.ProcessDataTestAsync();

            Console.WriteLine("Тестовая задача запущена и мы ее ждем!");

            Task.WaitAll(task, proccess_messages_task);

            Console.WriteLine("Главный поток работу закончил!");
            Console.ReadLine();
        }
    }
}
