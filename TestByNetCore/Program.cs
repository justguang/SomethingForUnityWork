using System;
using Test.Test_UKCP;
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
            Test_UTimer test_UTimer = new Test_UTimer();
            test_UTimer.Init();
            //end

            //## test UMath
            //Test_UMath test_UMath = new Test_UMath();
            //test_UMath.Init();
            //end

            //ULog.InitSetting(new ULogConfig
            //{
            //    loggerType = ULoggerType.Console
            //});

            //## test UKCP
            //ClientStart.Init("127.0.0.1", 6080);

            Console.WriteLine("## Test End ##");
            Console.ReadKey();
        }
    }
}
