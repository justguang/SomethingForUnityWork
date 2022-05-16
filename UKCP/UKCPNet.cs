/// <summary>
///********************************************
/// ClassName    ：  UNet
/// Author       ：  LCG
/// CreateTime   ：  2022/5/13 星期五
/// Description  ：  基于kcp 封装，实现可靠udp  
///********************************************/
/// </summary>
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using ULogs;

namespace UKcps
{
    [Serializable]
    public abstract class KCPMsg { }

    public class UKCPNet<T, K>
        where T : KCPSession<K>, new()
        where K : KCPMsg, new()
    {
        UdpClient udp;
        IPEndPoint remotePoint;

        private CancellationTokenSource cts;
        private CancellationToken ct;

        public UKCPNet(ULogConfig cfg)
        {
            ULog.InitSetting(cfg);//初始化日志
            cts = new CancellationTokenSource();
            ct = cts.Token;
        }
        public UKCPNet()
        {
            cts = new CancellationTokenSource();
            ct = cts.Token;
        }


        #region Server
        private Dictionary<uint, T> sessionDic = null;
        public void StartAsServer(string ip, int port)
        {
            sessionDic = new Dictionary<uint, T>();
            udp = new UdpClient(new IPEndPoint(IPAddress.Parse(ip), port));
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                udp.Client.IOControl((IOControlCode)(-1744830452), new byte[] { 0, 0, 0, 0 }, null);
            }

            //remotePoint = new IPEndPoint(IPAddress.Parse(ip), port);
            ULog.ColorLog(ULogColor.Green, "Server start...");

            //在线程池中进行接收数据
            Task.Run(ServerRecive, ct);
        }
        async void ServerRecive()
        {

            UdpReceiveResult result;
            while (true)
            {
                try
                {
                    if (ct.IsCancellationRequested)
                    {
                        this.ColorLog(ULogColor.Cyan, "ServerRecive Task is Canceled.");
                        break;
                    }

                    result = await udp.ReceiveAsync();

                    uint sid = BitConverter.ToUInt32(result.Buffer, 0);
                    //判断sid数据
                    if (sid == 0)
                    {
                        sid = GenerateUniqueSessionID();
                        byte[] sid_bytes = BitConverter.GetBytes(sid);
                        byte[] conv_bytes = new byte[8];
                        Array.Copy(sid_bytes, 0, conv_bytes, 4, 4);
                        SendUdpMsg(conv_bytes, result.RemoteEndPoint);
                    }
                    else
                    {
                        //业务逻辑处理
                        if (!sessionDic.TryGetValue(sid, out T session))
                        {
                            session = new T();
                            session.InitSession(sid, SendUdpMsg, result.RemoteEndPoint);
                            session.OnSessionClose = OnServerSessionClose;
                            lock (sessionDic)
                            {
                                sessionDic.Add(sid, session);
                            }
                        }
                        else
                        {
                            session = sessionDic[sid];
                        }
                        session.ReciveData(result.Buffer);
                    }

                }
                catch (Exception e)
                {
                    this.Warn("Server udp recive data exception:{0}", e.ToString());
                }
            }
        }
        void OnServerSessionClose(uint sid)
        {
            if (sessionDic.ContainsKey(sid))
            {
                lock (sessionDic)
                {
                    sessionDic.Remove(sid);
                }
                this.Warn("SessionID:{0} remove from sessionDic.", sid);
            }
            else
            {
                this.Error("SessionID:{0} cannot find in sessionDic.", sid);
            }
        }
        public void CloseServer()
        {
            foreach (var item in sessionDic)
            {
                item.Value.CloseSession();
            }
            sessionDic = null;
            if (udp != null)
            {
                udp.Close();
                udp = null;
                cts.Cancel();
            }
        }
        #endregion

        #region Client
        public T clientSession;
        public void StartAsClient(string ip, int port)
        {
            udp = new UdpClient(0);
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                udp.Client.IOControl((IOControlCode)(-1744830452), new byte[] { 0, 0, 0, 0 }, null);
            }
            remotePoint = new IPEndPoint(IPAddress.Parse(ip), port);
            ULog.ColorLog(ULogColor.Green, "Client start...");

            //在线程池中进行接收数据
            Task.Run(ClientRecive, ct);
        }
        public Task<bool> ConnectServer(int interval, int maxIntervalSum = 5000)
        {
            SendUdpMsg(new byte[4], remotePoint);
            int checkTimeCount = 0;

            Task<bool> task = Task.Run(async () =>
            {
                while (true)
                {
                    await Task.Delay(interval);
                    checkTimeCount += interval;
                    if (clientSession != null && clientSession.IsConnected())
                    {
                        return true;
                    }
                    else
                    {
                        if (checkTimeCount > maxIntervalSum)
                        {
                            return false;
                        }
                    }
                }
            });
            return task;
        }
        async void ClientRecive()
        {
            UdpReceiveResult result;
            while (true)
            {
                try
                {
                    if (ct.IsCancellationRequested)
                    {
                        ULog.ColorLog(ULogColor.Cyan, "ClientRecive Task is Canceled.");
                        break;
                    }

                    result = await udp.ReceiveAsync();
                    if (Equals(remotePoint, result.RemoteEndPoint))
                    {
                        uint sid = BitConverter.ToUInt32(result.Buffer, 0);
                        //收到sid数据
                        if (sid == 0)
                        {
                            if (clientSession != null && clientSession.IsConnected())
                            {
                                //已经建立连接，初始化完成了，收到了多余的sid，直接放弃
                                ULog.Warn("client is Init Done, Sid surplus.");
                            }
                            else
                            {
                                //未初始化，收到服务器分配的sid数据，初始化一个客户都session
                                sid = BitConverter.ToUInt32(result.Buffer, 4);
                                ULog.ColorLog(ULogColor.Green, "Udp request conv dis:{0}", sid);

                                //session初始化
                                clientSession = new T();
                                clientSession.InitSession(sid, SendUdpMsg, remotePoint);
                                clientSession.OnSessionClose = OnClientSessionClose;
                            }
                        }
                        else
                        {
                            //业务逻辑处理
                            if (clientSession != null && clientSession.IsConnected())
                            {
                                clientSession.ReciveData(result.Buffer);
                            }
                            else
                            {
                                //消息在 clientSession 初始化前提前到达，直接丢弃,
                                //直到初始化完成，kcp重传再开始处理
                                ULog.Warn("Client is Initing...");
                            }
                        }
                    }
                    else
                    {
                        ULog.Warn("Client udp recive illegal target data.");
                    }

                }
                catch (Exception e)
                {
                    ULog.Warn("Client udp recive data exception:{0}", e.ToString());
                }
            }
        }
        void OnClientSessionClose(uint sid)
        {
            cts.Cancel();
            if (udp != null)
            {
                udp.Close();
                udp = null;
            }
            ULog.Warn("client session close, sid:{0}", sid);
        }
        public void CloseClient()
        {
            if (clientSession != null)
            {
                clientSession.CloseSession();
            }
        }
        #endregion

        void SendUdpMsg(byte[] bytes, IPEndPoint remotePoint)
        {
            if (udp != null)
            {
                udp.SendAsync(bytes, bytes.Length, remotePoint);
            }
        }

        public void BroadCastMsg(K msg)
        {
            byte[] bytes = UKCPTool.Serialize<K>(msg);
            foreach (var item in sessionDic)
            {
                item.Value.SendMsg(bytes);
            }
        }

        private uint sid = 0;
        public uint GenerateUniqueSessionID()
        {
            lock (sessionDic)
            {
                while (true)
                {
                    ++sid;
                    if (sid > uint.MaxValue)
                    {
                        sid = 1;
                    }
                    if (!sessionDic.ContainsKey(sid))
                    {
                        break;
                    }
                }
            }
            return sid;
        }
    }
}
