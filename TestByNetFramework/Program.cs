using System;
using Test.Test_UKCP;

namespace Test
{
    class Program
    {
        static void Main(string[] args)
        {

            ServerStart.Init("127.0.0.1",6080);
            

            Console.WriteLine("## Test End ##");
            Console.ReadKey();
        }
    }
}
