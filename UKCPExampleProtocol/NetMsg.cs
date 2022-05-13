/// <summary>
///********************************************
/// ClassName    ：  NetMsg
/// Author       ：  LCG
/// CreateTime   ：  2022/5/13 星期五 
/// Description  ：  UKCP 网络通信数据协议
///********************************************/
/// </summary>
using System;
using UKcps;

namespace KCPExampleProtocol
{
    [Serializable]
    public class NetMsg:KCPMsg
    {
        public CMD cmd;
        public NetPing netPing;
        public string info;
    }

    [Serializable]
    public class NetPing
    {
        //心跳，true => 连接结束
        public bool isOver;
    }

    public enum CMD
    {
        None,
        NetPing,
    }
}
