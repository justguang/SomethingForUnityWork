/// <summary>
///********************************************
/// ClassName    ：  IOCPTool
/// Author       ：  LCG
/// CreateTime   ：  2022/6/20 星期一
/// Description  ：  工具函数
///********************************************/
/// </summary>
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading;

namespace UIOCPNet
{
    /// <summary>
    /// 工具函数
    /// </summary>
    public static class IOCPTool
    {
        /// <summary>
        /// 处理数据粘包分包
        /// </summary>
        /// <param name="byteList">所有接收的数据</param>
        /// <returns>返回一个完整的数据包，如果不完整返回null</returns>
        public static byte[] SplitLogicBytes(ref List<byte> byteList)
        {
            byte[] buff = null;
            if (byteList.Count > 4)
            {
                byte[] data = byteList.ToArray();
                int len = BitConverter.ToInt32(data, 0);
                if (byteList.Count >= len + 4)
                {
                    buff = new byte[len];
                    Buffer.BlockCopy(data, 4, buff, 0, len);
                    byteList.RemoveRange(0, len + 4);
                }
            }
            return buff;
        }

        /// <summary>
        /// 序列化
        /// </summary>
        /// <param name="msg">要序列化的IOCPMsg</param>
        public static byte[] Serialize(IOCPMsg msg)
        {
            byte[] data = null;
            MemoryStream ms = new MemoryStream();
            BinaryFormatter bf = new BinaryFormatter();
            try
            {
                bf.Serialize(ms, msg);
                ms.Seek(0, SeekOrigin.Begin);
                data = ms.ToArray();
            }
            catch (SerializationException e)
            {
                Error("Failed to serialie. Reson:{0}", e.Message);
            }
            finally
            {
                ms.Close();
            }
            return data;
        }

        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="bytes">要反序列化的字节数组</param>
        public static IOCPMsg DesSerialize(byte[] bytes)
        {
            IOCPMsg msg = null;
            MemoryStream ms = new MemoryStream(bytes);
            BinaryFormatter bf = new BinaryFormatter();
            try
            {
                msg = (IOCPMsg)bf.Deserialize(ms);
            }
            catch (SerializationException e)
            {
                Error("Failed to desSerialie. Reson:{0} , bytesLen:{1}", e.Message, bytes.Length);
            }
            finally
            {
                ms.Close();
            }
            return msg;
        }

        #region LOG
        public static Action<string> LogFunc;
        public static Action<IOCPLogColor, string> ColorLogFunc;
        public static Action<string> WarnFunc;
        public static Action<string> ErrorFunc;

        public static void Log(string msg, params object[] args)
        {
            msg = string.Format(msg, args);
            if (LogFunc != null)
            {
                LogFunc(msg);
            }
            else
            {
                ConsoleLog(msg, IOCPLogColor.None);
            }
        }
        public static void ColorLog(IOCPLogColor color, string msg, params object[] args)
        {
            msg = string.Format(msg, args);
            if (ColorLogFunc != null)
            {
                ColorLogFunc(color, msg);
            }
            else
            {
                ConsoleLog(msg, color);
            }
        }
        public static void Warn(string msg, params object[] args)
        {
            msg = string.Format(msg, args);
            if (WarnFunc != null)
            {
                WarnFunc(msg);
            }
            else
            {
                ConsoleLog(msg, IOCPLogColor.Yellow);
            }
        }
        public static void Error(string msg, params object[] args)
        {
            msg = string.Format(msg, args);
            if (ErrorFunc != null)
            {
                ErrorFunc(msg);
            }
            else
            {
                ConsoleLog(msg, IOCPLogColor.Red);
            }
        }
        static void ConsoleLog(string msg, IOCPLogColor color)
        {
            int threadID = Thread.CurrentThread.ManagedThreadId;
            msg = string.Format("ThreadID:{0} >> {1}", threadID, msg);
            switch (color)
            {
                case IOCPLogColor.Red:
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    Console.WriteLine(msg);
                    Console.ForegroundColor = ConsoleColor.Gray;
                    break;
                case IOCPLogColor.Green:
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine(msg);
                    Console.ForegroundColor = ConsoleColor.Gray;
                    break;
                case IOCPLogColor.Blue:
                    Console.ForegroundColor = ConsoleColor.Blue;
                    Console.WriteLine(msg);
                    Console.ForegroundColor = ConsoleColor.Gray;
                    break;
                case IOCPLogColor.Cyan:
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Console.WriteLine(msg);
                    Console.ForegroundColor = ConsoleColor.Gray;
                    break;
                case IOCPLogColor.Magenta:
                    Console.ForegroundColor = ConsoleColor.Magenta;
                    Console.WriteLine(msg);
                    Console.ForegroundColor = ConsoleColor.Gray;
                    break;
                case IOCPLogColor.Yellow:
                    Console.ForegroundColor = ConsoleColor.DarkYellow;
                    Console.WriteLine(msg);
                    Console.ForegroundColor = ConsoleColor.Gray;
                    break;
                case IOCPLogColor.None:
                default:
                    Console.WriteLine(msg);
                    break;
            }
        }
        #endregion


    }

    /// <summary>
    /// 日志颜色
    /// </summary>
    public enum IOCPLogColor
    {
        None,
        Red,
        Green,
        Blue,
        Cyan,
        Magenta,
        Yellow,
    }
}
