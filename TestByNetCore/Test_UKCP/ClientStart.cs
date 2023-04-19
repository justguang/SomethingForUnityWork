/// <summary>
///********************************************
/// ClassName    ：  ClientStart
/// Author       ：  LCG
/// CreateTime   ：  2022/5/13 星期五 
/// Description  ：  测试UKCP网络库
///********************************************/
/// </summary>
using System;
using System.Threading.Tasks;
using RGuang.Net.UKcp;
using RGuang.Utils;

namespace Test.Test_UKCP
{
    public class ClientStart
    {
        static UKCPNet<ClientSession, NetMsg> client;
        static Task<bool> checkConnectServerTask = null;

        public static void Init(string ip, int port)
        {
            client = new UKCPNet<ClientSession, NetMsg>(new ULogConfig { loggerType = ULoggerType.Console });
            client.StartAsClient(ip, port);
            checkConnectServerTask = client.ConnectServer(200, 5000);

            Task.Run(ConnectCheck);

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
                    client.clientSession.SendMsg(new NetMsg { info = ipt });
                }

            }

        }

        static int counter = 0;
        static async void ConnectCheck()
        {
            while (true)
            {
                await Task.Delay(3000);
                if (checkConnectServerTask != null && checkConnectServerTask.IsCompleted)
                {
                    if (checkConnectServerTask.Result)
                    {
                        ULog.ColorLog(ULogColor.Green, "Conncet server success.");
                        checkConnectServerTask = null;
                        //await Task.Run(SendPingMsg);
                    }
                }
                else
                {
                    ++counter;
                    if (counter > 4)
                    {
                        ULog.Error(string.Format("Connect failed {0} Times, Check your network connection.", counter));
                        checkConnectServerTask = null;
                        break;
                    }
                    else
                    {
                        ULog.Warn(string.Format("Connect failed {0} Times, Retry...", counter));
                        checkConnectServerTask = client.ConnectServer(200, 5000);
                    }
                }
            }
        }

        static async void SendPingMsg()
        {
            while (true)
            {
                await Task.Delay(5000);
                if (client != null && client.clientSession != null)
                {
                    client.clientSession.SendMsg(new NetMsg
                    {
                        cmd = CMD.NetPing,
                        netPing = new NetPing
                        {
                            isOver = false
                        }
                    });
                    ULog.ColorLog(ULogColor.Green, "Client send Ping message.");
                }
                else
                {
                    ULog.ColorLog(ULogColor.Green, "Client send Ping cancel.");
                    break;
                }
            }
        }
    }


    public class ServerStart
    {
        static UKCPNet<ServerSession, NetMsg> server;

        public static void Init(string ip, int port)
        {
            server = new UKCPNet<ServerSession, NetMsg>(new ULogConfig { loggerType = ULoggerType.Console });
            server.StartAsServer(ip, port);

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
                    server.BroadCastMsg(new NetMsg
                    {
                        info = ipt,
                    });

                }
            }

        }


    }
}
