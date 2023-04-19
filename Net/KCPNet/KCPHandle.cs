/// <summary>
///********************************************
/// ClassName    ：  KCPHandle
/// Author       ：  LCG
/// CreateTime   ：  2022/5/13 星期五
/// Description  ：  kcp 数据处理器
///********************************************/
/// </summary>
using System;
using System.Buffers;
using System.Net.Sockets.Kcp;

namespace RGuang.Net.UKcp
{
    public class KCPHandle:IKcpCallback
    {
        public Action<Memory<byte>> Out;

        public void Output(IMemoryOwner<byte> buffer,int avalidLenght)
        {
            using (buffer)
            {
                Out(buffer.Memory.Slice(0, avalidLenght));
            }
        }

        public Action<byte[]> Recv;
        public void Recive(byte[] buffer)
        {
            Recv(buffer);
        }
    }
}
