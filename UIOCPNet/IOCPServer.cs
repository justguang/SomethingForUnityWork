/// <summary>
///********************************************
/// ClassName    ：  IOCPServer
/// Author       ：  LCG
/// CreateTime   ：  2022/6/20 星期一 
/// Description  ：  基于IOCP封装的异步套接字通信，服务端
///********************************************/
/// </summary>
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading;

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
        IOCPTokenPool tokenPool;//会话token缓存池
        List<IOCPToken> tokenList;//管理已连接的token
        int curConnCount = 0;//当前连接数量
        Semaphore acceptSemaphore;//连接信号量限制
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
            curConnCount = 0;
            acceptSemaphore = new Semaphore(maxConnectCount, maxConnectCount);
            tokenPool = new IOCPTokenPool(maxConnectCount);
            tokenList = new List<IOCPToken>();
            for (int i = 0; i < maxConnectCount; i++)
            {
                IOCPToken token = new IOCPToken { tokenID = i + 1 };
                tokenPool.Push(token);
            }


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
            acceptSemaphore.WaitOne();
            bool suspend = skt.AcceptAsync(saea);
            if (suspend == false)
            {
                ProcessAccept();
            }
        }

        //有个连接进来
        void ProcessAccept()
        {
            Interlocked.Increment(ref curConnCount);
            IOCPToken token = tokenPool.Pop();
            lock (tokenList)
            {
                tokenList.Add(token);
            }
            token.InitToken(saea.AcceptSocket);
            token.onTokenClose = OnTokenClose;
            IOCPTool.ColorLog(IOCPLogColor.Green, "Client Online, Allocate TokenID:{0}", token.tokenID);
        }

        //有个连接断开
        void OnTokenClose(int tokenID)
        {
            //根据tokenID找出对应索引
            int index = -1;
            int len = tokenList.Count;
            for (int i = 0; i < len; i++)
            {
                if (tokenList[i].tokenID == tokenID)
                {
                    index = i;
                    break;
                }
            }

            //回收token
            if (index != -1)
            {
                tokenPool.Push(tokenList[index]);
                lock (tokenList)
                {
                    tokenList.RemoveAt(index);
                }
                Interlocked.Decrement(ref curConnCount);
                acceptSemaphore.Release();
            }
            else
            {
                IOCPTool.Error("TokenID:{0} cannot find in server tokenList.", tokenID);
            }
        }

        /// <summary>
        /// 获取当前连接数量
        /// </summary>
        public List<IOCPToken> GetTokenList()
        {
            return tokenList;
        }

        /// <summary>
        /// 关闭server
        /// </summary>
        public void CloseServer()
        {
            for (int i = 0; i < tokenList.Count; i++)
            {
                tokenList[i].CloseToken();
            }
            tokenList = null;

            if (skt != null)
            {
                skt.Close();
                skt = null;
            }
        }

        void IO_Completed(object sender, SocketAsyncEventArgs saea)
        {
            ProcessAccept();
        }
    }
}
