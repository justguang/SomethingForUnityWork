/// <summary>
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
            IOCPNet<UIOCPNet_Example,NetMsg> client = new IOCPNet<UIOCPNet_Example, NetMsg>();
            //client.StartAsClient("192.168.1.122", 19021);
            client.StartAsClient("43.138.26.253", 7799);//43.138.26.253

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
                    client.token.SendMsg(new NetMsg
                    {
                        msg = ipt,
                    });
                }
            }
        }

    }
}
