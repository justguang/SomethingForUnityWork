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
            Test_ULog test_ULog = new Test_ULog();
            test_ULog.Init();
            //end


            //## test UTimer
            //Test_UTimer test_UTimer = new Test_UTimer();
            //test_UTimer.Init();
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
            //end


            //## test UIOCPNet
            //Test_UIOCPNet test_UIOCP = new Test_UIOCPNet();
            //test_UIOCP.Init();
            //end

            //## test Socket
            //Test_ServerSkt client = new Test_ServerSkt();
            //client.Init("192.168.1.122", 19021);
            //end

            Console.WriteLine("## Test End on .Net Core##");
            Console.ReadKey();
        }
    }
}
