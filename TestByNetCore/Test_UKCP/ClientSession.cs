/// <summary>
///********************************************
/// ClassName    ：  ClientSession
/// Author       ：  LCG
/// CreateTime   ：  2022/5/13 星期五 
/// Description  ：  客户端session
///********************************************/
/// </summary>
using System;
using RGuang.Net.UKcp;
using RGuang.Utils;

namespace Test.Test_UKCP
{
    class ClientSession : KCPSession<NetMsg>
    {
        protected override void OnConnected()
        {
        }

        protected override void OnDisConnected()
        {
        }

        protected override void OnReciveMsg(NetMsg msg)
        {
            ULog.ColorLog(ULogColor.Magenta,"sid:{0}, Recive server data info:{1}",m_sid,msg.info);
        }

        protected override void OnUpdate(DateTime now)
        {
        }
    }

    class ServerSession : KCPSession<NetMsg>
    {
        protected override void OnConnected()
        {
            ULog.ColorLog(ULogColor.Green, " One Client connected...");
        }

        protected override void OnDisConnected()
        {
            ULog.ColorLog(ULogColor.Red, " One Client Disconnected...");
        }

        protected override void OnReciveMsg(NetMsg msg)
        {
            ULog.ColorLog(ULogColor.Magenta, "sid:{0}, Recive Client data info:{1}", m_sid, msg.info);
        }

        protected override void OnUpdate(DateTime now)
        {
        }
    }
}
