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
using System.Threading;
using System.Threading.Tasks;
using RGuang.Net.IOCPNet;

namespace Test
{
    public class Test_UIOCPNet
    {
        public IOCPNet<UIOCPNet_Example1, NetMsg> net;
        static bool client_TaskWork = false;
        public void Init()
        {
            IOCPTool.EnableLogSaveToFile("testguang_client");

            net = new IOCPNet<UIOCPNet_Example1, NetMsg>();
            //net.StartAsServer("127.0.0.1", 19021, 1000);
            net.StartAsClient("127.0.0.1", 19021);
            client_TaskWork = false;
            Task t = Task.Run(async () =>
            {
                while (client_TaskWork)
                {
                    Thread.Sleep(16);
                    net?.token?.SendMsg(new NetMsg
                    {
                        msg = "壹贰叁肆伍陆柒捌玖拾" +
                        "壹贰叁肆伍陆柒捌玖拾" +
                        "壹贰叁肆伍陆柒捌玖拾" +
                        "壹贰叁肆伍陆柒捌玖拾" +
                        "壹贰叁肆伍陆柒捌玖拾" +
                        "壹贰叁肆伍陆柒捌玖拾" +
                        "壹贰叁肆伍陆柒捌玖拾" +
                        "壹贰叁肆伍陆柒捌玖拾" +
                        "壹贰叁肆伍陆柒捌玖拾" +
                        "壹贰叁肆伍陆柒捌玖拾" //10 =>2*10*10=200byte
                    });
                }
            });

            while (true)
            {
                string ipt = Console.ReadLine();
                if (string.IsNullOrEmpty(ipt)) continue;
                if (ipt == "quit")
                {
                    IOCPTool.Warn("Net Exit~!");
                    //net.CloseServer();
                    net.CloseClient();
                    break;
                }
                else
                {
                    //List<UIOCPNet_Example1> tokenList = net.GetTokenList();
                    //int len = tokenList.Count;
                    //for (int i = 0; i < len; i++)
                    //{
                    //    tokenList[i].SendMsg(new NetMsg
                    //    {
                    //        msg = string.Format("Broadcast:{0}", ipt),
                    //    });
                    //}

                    net.token.SendMsg(new NetMsg
                    {
                        msg = string.Format("client:" + ipt)
                    });
                }
            }
        }




        /// <summary>
        /// 示例
        /// </summary>
        public class UIOCPNet_Example1 : IOCPToken<NetMsg>
        {
            protected override void OnConnected()
            {
                IOCPTool.ColorLog(IOCPLogColor.Green, "Connected~");
            }

            protected override void OnDisConnected()
            {
                IOCPTool.Warn("DisConnected！");
            }

            protected override void OnReceiveMsg(NetMsg msg)
            {
                IOCPTool.Log("ReceiveMsg：" + msg.msg);

                //List<UIOCPNet_Example1> tokenList = net.GetTokenList();
                //int len = tokenList.Count;
                //for (int i = 0; i < len; i++)
                //{
                //    tokenList[i].SendMsg(new NetMsg
                //    {
                //        msg = string.Format("server已收到:{0}", msg.msg),
                //    });
                //}
            }


        }

    }


}
