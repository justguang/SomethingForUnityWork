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
using UIOCPNet;

namespace Test
{
    public class Test_UIOCPNet
    {
        public void Init()
        {

            IOCPNet<UIOCPNet_Example, NetMsg> server = new IOCPNet<UIOCPNet_Example, NetMsg>();
            server.StartAsServer("192.168.1.122", 19021, 1000);

            while (true)
            {
                string ipt = Console.ReadLine();
                if (ipt == "quit")
                {
                    server.CloseServer();
                    break;
                }
                else
                {
                    List<UIOCPNet_Example> tokenList = server.GetTokenList();
                    int len = tokenList.Count;
                    for (int i = 0; i < len; i++)
                    {
                        tokenList[i].SendMsg(new NetMsg
                        {
                            msg = string.Format("Broadcast:{0}", ipt),
                        });
                    }
                }
            }
        }

    }


}
