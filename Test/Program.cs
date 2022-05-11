using System;
using ULogs;

namespace Test
{
    class Program
    {
        static void Main(string[] args)
        {
            //## test ULog
            //Test_ULog test_ULog = new Test_ULog();
            //test_ULog.Init();
            //end

            //## test UTimer
            //Test_UTimer test_UTimer = new Test_UTimer();
            //test_UTimer.Init();
            //end

            //## test UMath
            Test_UMath test_UMath = new Test_UMath();
            test_UMath.Init();
            //end

            Console.WriteLine("## Test End ##");
            Console.ReadKey();
        }
    }
}
