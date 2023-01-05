﻿/// <summary>
///********************************************
/// ClassName    ：  UIOCPNet_Example
/// Author       ：  LCG
/// CreateTime   ：  2022/6/20 星期一
/// Description  ：  Example
///********************************************/
/// </summary>
using System;
using System.Collections.Generic;

namespace UIOCPNet
{

    #region 网络数据协议消息体
    /// <summary>
    /// 网络数据协议消息体
    /// </summary>
    [Serializable]
    public class NetMsg : IOCPMsg
    {
        public string msg;

    }
    #endregion

    /// <summary>
    /// 示例
    /// </summary>
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
            IOCPTool.Log("ReceiveMsg：" + msg.msg);

        }


    }
}
