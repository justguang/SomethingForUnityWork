/// <summary>
///********************************************
/// ClassName    ：  KCPSession
/// Author       ：  LCG
/// CreateTime   ：  2022/5/13 星期五
/// Description  ：  网络会话数据接收与发送
///********************************************/
/// </summary>
using System;
using System.Net;
using System.Net.Sockets.Kcp;
using System.Threading;
using System.Threading.Tasks;
using ULogs;

namespace UKcps
{
    public enum SessionState
    {
        None,
        Connected,
        DisConnected,
    }

    public abstract class KCPSession<T> where T : KCPMsg, new()
    {
        protected uint m_sid;
        Action<byte[], IPEndPoint> m_udpSender;
        private IPEndPoint m_remotePoint;
        protected SessionState m_sessionState = SessionState.None;

        public Action<uint> OnSessionClose;
        public KCPHandle m_handle;
        public Kcp m_kcp;
        private CancellationTokenSource cts;
        private CancellationToken ct;


        public void InitSession(uint sid, Action<byte[], IPEndPoint> udpSender, IPEndPoint remotePoint)
        {
            this.m_sid = sid;
            this.m_udpSender = udpSender;
            this.m_remotePoint = remotePoint;
            this.m_sessionState = SessionState.Connected;

            m_handle = new KCPHandle();
            m_kcp = new Kcp(sid, m_handle);
            m_kcp.NoDelay(1, 10, 2, 1);
            m_kcp.WndSize(64, 64);
            m_kcp.SetMtu(512);

            m_handle.Out = (Memory<byte> buffer) =>
              {
                  byte[] bytes = buffer.ToArray();
                  m_udpSender(bytes, m_remotePoint);
              };

            m_handle.Recv = (byte[] buffer) =>
            {
                buffer = UKCPTool.DeCompress(buffer);
                T msg = UKCPTool.DeSerializ<T>(buffer);
                if (msg != null)
                {
                    OnReciveMsg(msg);
                }
            };

            OnConnected();

            cts = new CancellationTokenSource();
            ct = cts.Token;
            Task.Run(Update, ct);
        }
        public void ReciveData(byte[] buffer)
        {
            m_kcp.Input(buffer.AsSpan());
        }
        async void Update()
        {
            try
            {
                while (true)
                {
                    DateTime now = DateTime.UtcNow;
                    OnUpdate(now);
                    if (ct.IsCancellationRequested)
                    {
                        this.ColorLog(ULogColor.Cyan, "Session update Task is Cancelled.");
                        break;
                    }
                    else
                    {
                        m_kcp.Update(now);
                        int len;
                        while ((len = m_kcp.PeekSize()) > 0)
                        {
                            var buffer = new byte[len];
                            if (m_kcp.Recv(buffer) >= 0)
                            {
                                m_handle.Recive(buffer);
                            }
                        }

                        await Task.Delay(10);
                    }
                }
            }
            catch (Exception e)
            {
                this.Warn("Session update Exception:{0}", e.ToString());
            }
        }
        public void SendMsg(T msg)
        {
            if (!IsConnected())
            {
                this.Warn("Session DisConnected. Can not send msg.");
                return;
            }

            byte[] serialize_bytes = UKCPTool.Serialize(msg);
            if (serialize_bytes != null)
            {
                byte[] compress_bytes = UKCPTool.Compress(serialize_bytes);
                SendMsg(compress_bytes);
            }

        }
        public void SendMsg(byte[] msg_bytes)
        {
            if (!IsConnected())
            {
                this.Warn("Session DisConnected. Can not send msg.");
                return;
            }
            //msg_bytes = UKCPTool.Compress(msg_bytes);
            m_kcp.Send(msg_bytes.AsSpan());
        }
        public void CloseSession()
        {
            cts.Cancel();
            OnDisConnected();
            OnSessionClose?.Invoke(m_sid);
            OnSessionClose = null;

            m_sessionState = SessionState.DisConnected;
            m_remotePoint = null;
            m_udpSender = null;
            m_sid = 0;

            m_handle = null;
            m_kcp = null;
            cts = null;
        }


        protected abstract void OnConnected();
        protected abstract void OnUpdate(DateTime now);
        protected abstract void OnReciveMsg(T msg);
        protected abstract void OnDisConnected();


        public override bool Equals(object obj)
        {
            if (obj is KCPSession<T>)
            {
                KCPSession<T> s = obj as KCPSession<T>;
                return m_sid == s.m_sid;
            }
            return false;
        }
        public override int GetHashCode()
        {
            return m_sid.GetHashCode();
        }
        public uint GetSessionID()
        {
            return m_sid;
        }
        public bool IsConnected()
        {
            return m_sessionState == SessionState.Connected;
        }

    }
}
