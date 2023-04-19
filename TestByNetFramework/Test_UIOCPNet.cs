/// <summary>
///********************************************
/// ClassName    ：  Test_UIOCPNet
/// Author       ：  LCG
/// CreateTime   ：  2022/6/20 星期一
/// Description  ：  test UIOCPNet
///********************************************/
/// </summary>
using System;
using System.Collections.Generic;
using RGuang.Net.IOCPNet;

namespace TestByNetFramework
{
    public class Test_UIOCPNet
    {

        
        public void Init()
        {
            IOCPTool.EnableLogSaveToFile("test_UIOCP");
            IOCPNet<UIOCPNet_Example, NetMsg> net = new IOCPNet<UIOCPNet_Example, NetMsg>();
            net.StartAsClient("127.0.0.1", 19022);
            //net.StartAsServer("127.0.0.1", 19022, 1000);

            while (true)
            {
                string ipt = Console.ReadLine();
                if (ipt == "quit")
                {
                    net.CloseClient();
                    //net.CloseServer();
                    break;
                }
                else
                {
                    ////Server Send
                    //List<UIOCPNet_Example> tokenList = net.GetTokenList();
                    //int len = tokenList.Count;
                    //for (int i = 0; i < len; i++)
                    //{
                    //    tokenList[i].SendMsg(new NetMsg
                    //    {
                    //        msg = string.Format("Broadcast:{0}", ipt),
                    //    });
                    //}

                    //Client Send
                    net.token.SendMsg(new NetMsg
                    {
                        msg = ipt,
                    });
                }
            }
        }

    }
}
