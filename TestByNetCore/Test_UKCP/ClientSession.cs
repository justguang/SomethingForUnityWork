/// <summary>
///********************************************
/// ClassName    ：  ClientSession
/// Author       ：  LCG
/// CreateTime   ：  2022/5/13 星期五 
/// Description  ：  客户端session
///********************************************/
/// </summary>
using System;
using UKcps;
using KCPExampleProtocol;
using ULogs;

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
}
