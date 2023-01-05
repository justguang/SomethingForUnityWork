/// <summary>
///********************************************
/// ClassName    ：  Test_UIOCPNet
/// Author       ：  LCG
/// CreateTime   ：  2022/6/20 星期一
/// Description  ：  test UIOCPNet
///********************************************/
/// </summary>
using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using UIOCPNet;

namespace TestByNetFramework
{
    public class Test_UIOCPNet
    {

        
        public void Init()
        {

            IOCPNet<UIOCPNet_Example, NetMsg> net = new IOCPNet<UIOCPNet_Example, NetMsg>();
            //net.StartAsClient("103.46.128.44", 40707);
            net.StartAsServer("127.0.0.1", 19022,1000);

            while (true)
            {
                string ipt = Console.ReadLine();
                if (ipt == "quit")
                {
                    net.CloseClient();
                    break;
                }
                else
                {
                    net.token.SendMsg(new NetMsg
                    {
                        msg = ipt,
                    });
                }
            }
        }

    }
}
