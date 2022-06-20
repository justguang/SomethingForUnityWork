/// <summary>
///********************************************
/// ClassName    ：  UIOCPNet_Example
/// Author       ：  LCG
/// CreateTime   ：  2022/6/20 星期一
/// Description  ：  Example
///********************************************/
/// </summary>
using System;

namespace UIOCPNet
{

    #region 网络数据协议消息体
    [Serializable]
    public class NetMsg : IOCPMsg
    {
        public string msg;

    }
    #endregion


    public class UIOCPNet_Example: IOCPToken<NetMsg>
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
            IOCPTool.Log("收到数据：" + msg.msg);
        }
    }
}
