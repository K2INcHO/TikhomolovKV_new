using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;

namespace ConsoleTests
{
    static class CriticalSelectionTests
    {
        public static void Start()
        {
            //LockSynchronizationTest();

            var manual_reset_event = new ManualResetEvent(false);
            var auto_reset_event = new AutoResetEvent(false);

            EventWaitHandle starter = manual_reset_event;

            for (var i = 0; i < 10; i++)
            {
                var local_i = i;
                new Thread(() =>
                {
                    Console.WriteLine("Поток {0} запущен", local_i);
                    starter.WaitOne();
                    starter.Reset();
                    Console.WriteLine("Поток {0} завершил свою работу", local_i);
                }).Start();
            }

            Console.WriteLine("Все потоки созданы и готовы к работе.");

            Console.ReadLine();
            starter.Set();
            Console.ReadLine();

            //Mutex - именованный объект ядра системы
            //Mutex mutex1 = new Mutex(true, "Тестовый мютекс", out var created1);
            //Mutex mutex2 = new Mutex(true, "Тестовый мютекс", out var created2);

            //mutex1.WaitOne();       //застрянет до тех пор пока кто-то не освободит мютекс
            //mutex1.ReleaseMutex();  

            //var semaphore = new Semaphore(0, 10);   //мютекс в который можно войти много раз

            //semaphore.WaitOne();
            //semaphore.Release();
        }

        private static void LockSynchronizationTest()
        {
            var threads = new Thread[10];

            for (var i = 0; i < threads.Length; i++)
            {
                var local_i = i;
                threads[i] = new Thread(() => PrintData($"Message from thread {local_i}", 10));
            }

            for (var i = 0; i < threads.Length; i++)
            {
                threads[i].Start();
            }
        }

        private static readonly object __SyncRoot = new object();

        private static void PrintData(string Message, int Count)
        {
            for(var i = 0; i < Count; i++)
            {
                lock (__SyncRoot)
                {
                    Console.Write("Thread id: {0};", Thread.CurrentThread.ManagedThreadId);
                    Console.Write("\t");
                    Console.Write(Message);
                    Console.WriteLine();
                }

                //эквивалент работы lock (на низком уровне)
                //Monitor.Enter(__SyncRoot);
                //try
                //{
                //    Console.Write("Thread id: {0};", Thread.CurrentThread.ManagedThreadId);
                //    Console.Write("\t");
                //    Console.Write(Message);
                //    Console.WriteLine();
                //}
                //finally
                //{
                //    Monitor.Exit(__SyncRoot);
                //}

            }
        }
    }

    // выполняем синхронизацию потоков...
    //[Synchronization] //для .NET Framework
    public class FileLogger: ContextBoundObject
    {
        private readonly string _LogFileName;
        public FileLogger(string LogFileName)
        {
            _LogFileName = LogFileName;
        }

        public void  Log(string Message)
        {
            File.WriteAllText(_LogFileName, Message);
        }
    }
}
