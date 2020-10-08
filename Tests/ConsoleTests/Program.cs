﻿using System;
using System.Collections.Concurrent;
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

            //var printer_thread = new Thread(PrintMessage)
            //{
            //    IsBackground = true,
            //    Name = "Parameter printer"
            //};
            //printer_thread.Start("Hello World!");

            var message = "Hello World!~";
            var count = 10;

            //new Thread(() => PrintMessage(message, count)) { IsBackground = true }.Start();

            //var print_task = new PrintMessageTask(message, count);
            //print_task.Start();

            //for(var i = 0;i<1000; i++)
            //{
            //    Console.WriteLine("Главный поток {0}", i);
            //    Thread.Sleep(10);
            //}

            Console.WriteLine("Главный поток работу закончил!");
            Console.ReadLine();
        }

        //передача данных в поток
        private static void PrintMessage(object parameter)
        {
            PrintThreadInfo();
            var msg = (string)parameter;

            var thread_id = Thread.CurrentThread.ManagedThreadId;
            for(var i = 0; i < 10; i++)
            {
                Console.WriteLine("id:{0}\t{1}",thread_id, msg);
                Thread.Sleep(10);
            }
        }

        //передача данных в поток (с аргументами)
        private static void PrintMessage(string Message, int Count)
        {
            PrintThreadInfo();

            var thread_id = Thread.CurrentThread.ManagedThreadId;
            for (var i = 0; i < Count; i++)
            {
                Console.WriteLine("id:{0}\t{1}", thread_id, Message);
                Thread.Sleep(10);
            }
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



    class PrintMessageTask
    {
        private readonly string _Message;
        private readonly int _Count;
        private Thread _Thread;

        public PrintMessageTask(string Message, int Count)
        {
            _Message = Message;
            _Count = Count;
            _Thread = new Thread(ThreadMethod) { IsBackground = true};
        }

        public void Start()
        {
            if (_Thread?.IsAlive == false)
                _Thread?.Start();
        }

        private void ThreadMethod()
        {
            var thread_id = Thread.CurrentThread.ManagedThreadId;
            for (var i = 0; i < _Count; i++)
            {
                Console.WriteLine("id:{0}\t{1}", thread_id, _Message);
                Thread.Sleep(10);
            }

            _Thread = null;
        }
    }
}
