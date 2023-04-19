using System;

namespace TestByNetFramework
{
    class Program
    {
        static void Main(string[] args)
        {
            //## Test UKCP
            //ServerStart.Init("127.0.0.1",6080);
            //### End

            //## Test UIOCPNet
            Test_UIOCPNet test_UIOCPNet = new Test_UIOCPNet();
            test_UIOCPNet.Init();
            //## End

            Console.WriteLine("## Test End on .Net Framework ##");
            Console.ReadKey();
        }
    }
}
