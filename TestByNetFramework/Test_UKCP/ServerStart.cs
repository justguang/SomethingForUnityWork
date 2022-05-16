/// <summary>
///********************************************
/// ClassName    ：  ServerStart
/// Author       ：  LCG
/// CreateTime   ：  2022/5/13 星期五 
/// Description  ：  测试UKCP网络库
///********************************************/
/// </summary>
using KCPExampleProtocol;
using System;
using UKcps;
using ULogs;

namespace Test.Test_UKCP
{
    public class ServerStart
    {
        static UKCPNet<ServerSession, NetMsg> server = null;
        public static void Init(string ip, int port)
        {
            ULog.InitSetting(new ULogConfig { loggerType = ULoggerType.Console });
            server = new UKCPNet<ServerSession, NetMsg>();
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
                    server.BroadCastMsg(new NetMsg { info = ipt });
                }

            }
        }

    }
}
