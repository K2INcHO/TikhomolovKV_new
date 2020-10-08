using System;
using System.Threading;

namespace ConsoleTests
{
    class Program
    {
        static void Main(string[] args)
        {
            var main_thread = Thread.CurrentThread;
            var main_thread_id = main_thread.ManagedThreadId;

            main_thread.Name = "Главный поток!"; //присваиваем потоку его имя
 
            //TimerMethod();
            var timer_thread = new Thread(TimerMethod); //создаем новый поток класса Thread
            timer_thread.Name = "Поток часов";
            timer_thread.IsBackground = true;           //устанавливаем отметку для потока что он фоновый
            timer_thread.Start();       //командуем объекту Thread чтобы он запустил поток (стал горячим)

            for(var i = 0;i<1000; i++)
            {
                Console.WriteLine("Главный поток {0}", i);
                Thread.Sleep(10);
            }

            Console.WriteLine("Главный поток работу закончил!");
            Console.ReadLine();
        }

        private static void TimerMethod()
        {
            PrintThreadInfo();
            while (true)
            {
                Console.Title = DateTime.Now.ToString("HH:mm:ss:ffff");
                Thread.Sleep(100); // усыпляем поток на 100мс
                //Thread.SpinWait(10); // остановка потока, но без его усыпления
            }
        }

        private static void PrintThreadInfo()
        {
            var thread = Thread.CurrentThread;
            Console.WriteLine("id:{0}; name:{1}; priority:{2}",
                thread.ManagedThreadId,thread.Name, thread.Priority);
        }
    }
}
