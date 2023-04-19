/// <summary>
///********************************************
/// ClassName    ：  Test_ServerSkt
/// Author       ：  LCG
/// CreateTime   ：  2022/6/24 星期五 
/// Description  ：  
///********************************************/
/// </summary>
using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;

namespace Test
{

    public class Test_ServerSkt
    {
        Socket skt;
        Socket client;
        byte[] bytes = new byte[1024];


        [Serializable]
        class A
        {
            public int i;
            public string str;
        }

        public void Init(string ip, int port)
        {
            skt = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            skt.Bind(new IPEndPoint(IPAddress.Parse(ip), port));
            skt.Listen(10);
            StartAsAccept();
        }

        void StartAsAccept()
        {
            skt.BeginAccept(new AsyncCallback(AcceptCB), null);
        }

        void AcceptCB(IAsyncResult ar)
        {
            Console.WriteLine("client Online...");
            client = skt.EndAccept(ar);

            A a = new A
            {
                i = 666,
                str = "hello",
            };

            byte[] msg = null;
            using (MemoryStream ms = new MemoryStream())
            {
                try
                {

                    BinaryFormatter bf = new BinaryFormatter();
                    bf.Serialize(ms, a);
                    ms.Seek(0, SeekOrigin.Begin);
                    msg = ms.ToArray();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
            };

            if (msg != null)
            {
                client.Send(msg);
            }
            client.BeginReceive(bytes, 0, bytes.Length, SocketFlags.None, new AsyncCallback(ReceiveCB), null);
        }

        void ReceiveCB(IAsyncResult ar)
        {
            int len = client.EndReceive(ar);
            string str = Encoding.ASCII.GetString(bytes, 0, len);

            Console.WriteLine("收到：" + str);
        }
    }
}
