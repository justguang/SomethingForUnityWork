/// <summary>
///********************************************
/// ClassName    ：  UKCPTool
/// Author       ：  LCG
/// CreateTime   ：  2022/5/13 星期五 
/// Description  ：  UKcp 网络工具
///********************************************/
/// </summary>
using System;
using System.IO;
using System.IO.Compression;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using ULogs;

namespace UKcps
{
    public class UKCPTool
    {

        public static byte[] Serialize<T>(T msg) where T : KCPMsg
        {
            using (MemoryStream ms = new MemoryStream())
            {
                try
                {
                    BinaryFormatter bf = new BinaryFormatter();
                    bf.Serialize(ms, msg);
                    ms.Seek(0, SeekOrigin.Begin);
                    return ms.ToArray();
                }
                catch (SerializationException e)
                {
                    ULog.Error("Failed to Serialize, Resaon:{0}", e.Message);
                    throw;
                }
            }
        }

        public static T DeSerializ<T>(byte[] bytes) where T : KCPMsg
        {
            using (MemoryStream ms = new MemoryStream(bytes))
            {
                try
                {
                    BinaryFormatter bf = new BinaryFormatter();
                    T msg = (T)bf.Deserialize(ms);
                    return msg;
                }
                catch (SerializationException e)
                {
                    ULog.Error("Failed to DeSerialize, Resaon:{0}, bytesLen:{1}", e.Message, bytes.Length);
                    throw;
                }
            }
        }

        public static byte[] Compress(byte[] input)
        {
            using (MemoryStream outMS = new MemoryStream())
            {
                using (GZipStream gzs = new GZipStream(outMS, CompressionMode.Compress, true))
                {
                    gzs.Write(input, 0, input.Length);
                    gzs.Close();
                    return outMS.ToArray();
                }
            }
        }

        public static byte[] DeCompress(byte[] input)
        {
            using (MemoryStream inputMS = new MemoryStream(input))
            {
                using (MemoryStream outMS = new MemoryStream())
                {
                    using (GZipStream gzs = new GZipStream(inputMS, CompressionMode.Decompress))
                    {
                        byte[] bytes = new byte[1024];
                        int len = 0;
                        while ((len = gzs.Read(bytes, 0, bytes.Length)) > 0)
                        {
                            outMS.Write(bytes, 0, len);
                        }
                        gzs.Close();
                        return outMS.ToArray();
                    }
                }
            }
        }

        static readonly DateTime utcStart = new DateTime(1970, 1, 1);
        public static ulong GetUTCStartMilliSeconds()
        {
            TimeSpan ts = DateTime.UtcNow - utcStart;
            return (ulong)ts.TotalMilliseconds;
        }


    }
}
