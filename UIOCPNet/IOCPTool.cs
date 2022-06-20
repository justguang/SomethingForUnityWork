/// <summary>
///********************************************
/// ClassName    ：  IOCPTool
/// Author       ：  LCG
/// CreateTime   ：  2022/6/20 星期一
/// Description  ：  工具函数
///********************************************/
/// </summary>
using System;
using System.Threading;

namespace UIOCPNet
{
    /// <summary>
    /// 工具函数
    /// </summary>
    public static class IOCPTool
    {
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
