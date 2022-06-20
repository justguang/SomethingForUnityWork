/// <summary>
///********************************************
/// ClassName    ：  IOCPClient
/// Author       ：  LCG
/// CreateTime   ：  2022/6/20 星期一
/// Description  ：  基于IOCP封装的异步套接字通信
///********************************************/
/// </summary>
using System;
using System.Net;
using System.Net.Sockets;

namespace UIOCPNet
{
    /// <summary>
    /// SocketType => Stream
    /// ProtocolType => Tcp
    /// Connect => Socket.ConnectAsync
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

        public void StartAsClient(string ip, int port)
        {
            IPEndPoint pt = new IPEndPoint(IPAddress.Parse(ip), port);
            skt = new Socket(pt.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            saea.RemoteEndPoint = pt;

            IOCPTool.ColorLog(IOCPLogColor.Green, "Client Start...");
            StartConnect();
        }

        void StartConnect()
        {
            bool suspend = skt.ConnectAsync(saea);
            if (suspend == false)
            {
                IOCPTool.Log("连接成功");
                ProcessConnect();
            }
            else
            {
                IOCPTool.Log("连接挂起");
            }
        }

        void ProcessConnect()
        {

        }

        void IO_Completed(object sender,SocketAsyncEventArgs saea)
        {

        }
    }
}
