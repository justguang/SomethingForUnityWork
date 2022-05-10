using System;
using ULogs;

namespace Test
{
    class Program
    {
        static void Main(string[] args)
        {
            //## test ULog
            Test_ULog test_ULog = new Test_ULog();
            test_ULog.Init();
            //end


            Console.WriteLine("Hello World!");
            Console.ReadKey();
        }
    }
}
