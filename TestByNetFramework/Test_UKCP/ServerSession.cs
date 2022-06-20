/// <summary>
///********************************************
/// ClassName    ：  ServerSession
/// Author       ：  LCG
/// CreateTime   ：  2022/5/13 星期五
/// Description  ：  测试UKCP网络库
///********************************************/
/// </summary>
using KCPExampleProtocol;
using System;
using UKcps;
using ULogs;

namespace TestByNetFramework
{
    public class ServerSession : KCPSession<NetMsg>
    {
        protected override void OnConnected()
        {
            ULog.ColorLog(ULogColor.Green, "Client Online, Sid:{0}", m_sid);
        }

        protected override void OnDisConnected()
        {
            ULog.Warn("Client Offline, Sid:{0}", m_sid);
        }

        protected override void OnReciveMsg(NetMsg msg)
        {
            ULog.ColorLog(ULogColor.Magenta, "Sid:{0} recive data, CMD:{1}, info:{2} ", m_sid, msg.cmd, msg.info);
            if (msg.cmd == CMD.NetPing)
            {
                if (msg.netPing.isOver)
                {
                    CloseSession();
                }
                else
                {
                    checkCounter = 0;
                    NetMsg pingMsg = new NetMsg
                    {
                        cmd = CMD.NetPing,
                        netPing = new NetPing
                        {
                            isOver = false
                        }
                    };
                    SendMsg(pingMsg);
                }
            }
        }

        private int checkCounter = 0;
        DateTime checkTime = DateTime.UtcNow.AddSeconds(5);
        protected override void OnUpdate(DateTime now)
        {
            if (now > checkTime)
            {
                checkTime = now.AddSeconds(5);
                checkCounter++;
                if (checkCounter > 3)
                {
                    NetMsg pingMsg = new NetMsg
                    {
                        cmd = CMD.NetPing,
                        netPing = new NetPing
                        {
                            isOver = true
                        }
                    };
                    OnReciveMsg(pingMsg);
                }
            }
        }
    }
}
