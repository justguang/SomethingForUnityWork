/// <summary>
///********************************************
/// ClassName    ：  IOCPServer
/// Author       ：  LCG
/// CreateTime   ：  2022/6/20 星期一 
/// Description  ：  基于IOCP封装的异步套接字通信，服务端
///********************************************/
/// </summary>
using System;
using System.Net;
using System.Net.Sockets;

namespace UIOCPNet
{
    /// <summary>
    /// 基于IOCP封装的异步套接字通信【服务端】；
    /// 套接字类型 => Stream；
    /// 协议 => Tcp；
    /// 接收连接方式 => Socket.AcceptAsync。
    /// </summary>
    public class IOCPServer
    {
        Socket skt;
        SocketAsyncEventArgs saea;

        public int backlog = 100;

        public IOCPServer()
        {
            saea = new SocketAsyncEventArgs();
            saea.Completed += new EventHandler<SocketAsyncEventArgs>(IO_Completed);
        }

        /// <summary>
        /// 启动server
        /// </summary>
        /// <param name="ip">绑定的ip</param>
        /// <param name="port">端口</param>
        /// <param name="maxConnectCount">根据物理设备不同设置最大连接数</param>
        public void StartAsServer(string ip, int port, int maxConnectCount)
        {
            IPEndPoint pt = new IPEndPoint(IPAddress.Parse(ip), port);
            skt = new Socket(pt.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            skt.Bind(pt);
            skt.Listen(backlog);

            IOCPTool.ColorLog(IOCPLogColor.Green, "Server Start...");
            StartAccept();
        }

        //开始异步接收连接
        void StartAccept()
        {
            bool suspend = skt.AcceptAsync(saea);
            if (suspend == false)
            {
                ProcessAccept();
            }
        }

        void ProcessAccept()
        {

        }

        void IO_Completed(object sender, SocketAsyncEventArgs saea)
        {
            ProcessAccept();
        }
    }
}
