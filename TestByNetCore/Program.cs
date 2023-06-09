using System;
using System.Collections.Generic;
using RGuang.Utils;
using Test.Test_UKCP;


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
            //ClientStart.Init("127.0.0.1", 19021);
            //ServerStart.Init("127.0.0.1", 19021);
            //end


            //## test UIOCPNet
            //Test_UIOCPNet test_UIOCP = new Test_UIOCPNet();
            //test_UIOCP.Init();
            //end

            //## test Socket
            //Test_ServerSkt client = new Test_ServerSkt();
            //client.Init("127.0.0.1", 19022);
            //end

            //## test PriorityQueue
            //int count = 1000000;
            //while (count > 0)
            //{
            //    count--;
            //    LoopTestForUpriorityQueue();
            //}

            Console.WriteLine("## Test End on .Net Core##");
            Console.ReadKey();
        }

        static void LoopTestForUpriorityQueue()
        {
            //random add data
            Random r = new Random();
            int queCount = r.Next(100, 9999);
            List<ItemForUPriorityQueue> itemLst = new List<ItemForUPriorityQueue>(queCount);
            for (int i = 0; i < queCount; i++)
            {
                itemLst.Add(new ItemForUPriorityQueue
                {
                    itemName = $"item_{i}",
                    priority = r.Next(0, 10000)
                });
            }

            //enqueue
            UPriorityQueue<ItemForUPriorityQueue> uQueue = new UPriorityQueue<ItemForUPriorityQueue>();
            uQueue.Enqueue(itemLst.ToArray());

            //random remove item
            int rmvCount = r.Next(1, 9999);
            for (int i = 0; i < rmvCount; i++)
            {
                int index = r.Next(0, uQueue.Count);
                int uIndex = uQueue.IndexOf(itemLst[index]);
                if (uIndex >= 0)
                {
                    uQueue.RemoveAt(uIndex);
                }
            }

            //dequeue
            List<ItemForUPriorityQueue> outList = new List<ItemForUPriorityQueue>();
            while (uQueue.Count > 0)
            {
                ItemForUPriorityQueue item = uQueue.Dequeue();
                outList.Add(item);
                item.PrintInfo();
            }

            //order check
            for (int i = 0; i < outList.Count; i++)
            {
                if (i < outList.Count - 1)
                {
                    if (outList[i].priority > outList[i + 1].priority)
                    {
                        Exception e = new Exception("优先级异常.");
                        throw e;
                    }
                }
            }
        }
    }
    class ItemForUPriorityQueue : IComparable<ItemForUPriorityQueue>
    {
        public string itemName;
        public float priority;
        public int CompareTo(ItemForUPriorityQueue other)
        {
            if (priority < other.priority)
            {
                return -1;
            }
            else if (priority > other.priority)
            {
                return 1;
            }
            else
            {
                return 0;
            }
        }

        public void PrintInfo()
        {
            Console.WriteLine($"itemName;{itemName}, priority:{priority}");
        }
    }
}
