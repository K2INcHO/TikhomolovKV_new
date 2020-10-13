using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace ConsoleTests
{
    static class TPLOverview
    {
        public static void Start()
        {
            Console.WriteLine("Главный поток работу закончил!");
            Console.ReadLine();
        }

        private static void ParallelInvokeMethod()
        {
            Console.WriteLine("ThrID:{0} - started", Thread.CurrentThread.ManagedThreadId);
            Thread.Sleep(250);
            Console.WriteLine("ThrID:{0} - finished", Thread.CurrentThread.ManagedThreadId);
        }

        private static void ParallelInvokeMethod(string msg)
        {
            Console.WriteLine("ThrID:{0} - started: {1}", Thread.CurrentThread.ManagedThreadId, msg);
            Thread.Sleep(250);
            Console.WriteLine("ThrID:{0} - finished: {1}", Thread.CurrentThread.ManagedThreadId, msg);
        }

        private static int Factorial(int n)
        {
            var factorial = 1;
            for (var i = 1; i <= n; i++)
                factorial *= n;
            return factorial;
        }

        private static int Sum(int n)
        {
            var sum = 1;
            for (var i = 1; i <= n; i++)
                sum += n;
            return sum;
        }
    }
}
