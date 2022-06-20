/// <summary>
///********************************************
/// ClassName    ：  IOCPToken
/// Author       ：  LCG
/// CreateTime   ：  2022/6/20 星期一
/// Description  ：  IOCP连接会话Token(session)
///********************************************/
/// </summary>
using System;
using System.Collections.Generic;
using System.Net.Sockets;

namespace UIOCPNet
{
    /// <summary>
    /// 连接状态
    /// </summary>
    public enum TokenState
    {
        None,
        /// <summary>
        /// 连接
        /// </summary>
        Connected,
        /// <summary>
        /// 断开连接
        /// </summary>
        DisConnected,
    }

    /// <summary>
    /// IOCP连接会话Token(session)；
    /// 接收方式：Socket.ReceiveAsync
    /// </summary>
    public class IOCPToken
    {
        public int tokenID;
        /// <summary>
        /// token的连接状态
        /// </summary>
        public TokenState tokenState = TokenState.None;

        Socket skt;
        SocketAsyncEventArgs rcvSAEA;
        List<byte> readList = new List<byte>();

        public IOCPToken()
        {
            rcvSAEA = new SocketAsyncEventArgs();
            rcvSAEA.Completed += new EventHandler<SocketAsyncEventArgs>(IO_Completed);
            rcvSAEA.SetBuffer(new byte[2048], 0, 2048);
        }

        public void InitToken(Socket skt)
        {
            this.skt = skt;
            this.tokenState = TokenState.Connected;
            OnConnected();
            StartAsyncRcv();
        }

        void StartAsyncRcv()
        {
            bool suspend = skt.ReceiveAsync(rcvSAEA);
            if (suspend == false)
            {
                ProcessRcv();
            }
        }
        void ProcessRcv()
        {
            if (rcvSAEA.BytesTransferred > 0 && rcvSAEA.SocketError == SocketError.Success)
            {
                byte[] bytes = new byte[rcvSAEA.BytesTransferred];
                Buffer.BlockCopy(rcvSAEA.Buffer,0,bytes,0,rcvSAEA.BytesTransferred);
                readList.AddRange(bytes);
                ProcessByteList();
                StartAsyncRcv();
            }
            else
            {
                IOCPTool.Warn("Token:{0} Close:{1}",tokenID,rcvSAEA.SocketError.ToString());
                CloseToken();
            }
        }

        void ProcessByteList()
        {

        }

        void IO_Completed(object sender, SocketAsyncEventArgs saea)
        {
            //
        }

        public void CloseToken()
        {

        }

        void OnConnected()
        {
            IOCPTool.Log("Connect Success.");
        }

    }
}
