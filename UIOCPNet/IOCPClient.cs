/// <summary>
///********************************************
/// ClassName    ：  IOCPClient
/// Author       ：  LCG
/// CreateTime   ：  2022/6/20 星期一
/// Description  ：  基于IOCP封装的异步套接字通信，客户端
///********************************************/
/// </summary>
using System;
using System.Net;
using System.Net.Sockets;

namespace UIOCPNet
{

    /// <summary>
    /// 基于IOCP封装的异步套接字通信【客户端】；
    /// 套接字类型 => Stream；
    /// 协议 => Tcp；
    /// 连接方式 => Socket.ConnectAsync。
    /// </summary>
    public class IOCPClient
    {
        Socket skt;
        SocketAsyncEventArgs saea;

        public IOCPClient()
        {
            saea = new SocketAsyncEventArgs();
            saea.Completed += new EventHandler<SocketAsyncEventArgs>(IO_Completed);
        }

        /// <summary>
        /// 启动client
        /// </summary>
        /// <param name="ip">连接的ip</param>
        /// <param name="port">端口</param>
        public void StartAsClient(string ip, int port)
        {
            IPEndPoint pt = new IPEndPoint(IPAddress.Parse(ip), port);
            skt = new Socket(pt.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            saea.RemoteEndPoint = pt;

            IOCPTool.ColorLog(IOCPLogColor.Green, "Client Start...");
            StartConnect();
        }

        //开始异步连接
        void StartConnect()
        {
            bool suspend = skt.ConnectAsync(saea);
            if (suspend == false)
            {
                ProcessConnect();
            }
            else
            {
                IOCPTool.Log("连接挂起");
            }
        }

        void ProcessConnect()
        {
            IOCPTool.Log("连接成功");
        }

        void IO_Completed(object sender, SocketAsyncEventArgs saea)
        {
            ProcessConnect();
        }
    }
}
