﻿/// <summary>
///********************************************
/// ClassName    ：  Test_UIOCPNet
/// Author       ：  LCG
/// CreateTime   ：  2022/6/20 星期一
/// Description  ：  test UIOCPNet
///********************************************/
/// </summary>
using System;
using UIOCPNet;

namespace TestByNetFramework
{
    public class Test_UIOCPNet
    {
        public void Init()
        {
            IOCPClient client = new IOCPClient();
            client.StartAsClient("192.168.1.122", 19021);

            while (true)
            {
                string ipt = Console.ReadLine();
                if (ipt == "quit")
                {
                    client.CloseClient();
                    break;
                }
                else
                {
                    client.token.SendMsg(new IOCPMsg
                    {
                        msg = ipt,
                    });
                }
            }
        }

    }
}
