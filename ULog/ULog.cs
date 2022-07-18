/// <summary>
///********************************************
/// ClassName    ：  ULog
/// Author       ：  LCG
/// CreateTime   ：  2022/5/10 星期二 
/// Description  ：  日志工具核心类
///********************************************/
/// </summary>
using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading;


/// <summary>
/// ULog扩展
/// </summary>
public static class ExtensionMethods
{
    /// <summary>
    /// 打印普通日志
    /// </summary>
    /// <param name="log">要打印的内容</param>
    /// <param name="args">格式化的参数</param>
    public static void Log(this object obj, string log, params object[] args)
    {
        ULogs.ULog.Log(string.Format(log, args));
    }
    /// <summary>
    /// 打印普通日志
    /// </summary>
    /// <param name="log">要打印的内容</param>
    public static void Log(this object obj, object log)
    {
        ULogs.ULog.Log(log);
    }

    /// <summary>
    /// 打印带颜色的日志
    /// </summary>
    /// <param name="color">设置内容显示的颜色</param>
    /// <param name="log">要打印的内容</param>
    /// <param name="args">格式化的参数</param>
    public static void ColorLog(this object obj, ULogs.ULogColor color, string log, params object[] args)
    {
        ULogs.ULog.ColorLog(color, string.Format(log, args));
    }
    /// <summary>
    /// 打印带颜色的日志
    /// </summary>
    /// <param name="color">设置内容显示的颜色</param>
    /// <param name="log">要打印的内容</param>
    public static void ColorLog(this object obj, ULogs.ULogColor color, object log)
    {
        ULogs.ULog.ColorLog(color, log);
    }

    /// <summary>
    /// 打印警告日志
    /// </summary>
    /// <param name="log">要打印的内容</param>
    /// <param name="args">格式化的参数</param>
    public static void Warn(this object obj, string log, params object[] args)
    {
        ULogs.ULog.Warn(string.Format(log, args));
    }
    /// <summary>
    /// 打印警告日志
    /// </summary>
    /// <param name="log">要打印的内容</param>
    public static void Warn(this object obj, object log)
    {
        ULogs.ULog.Warn(log);
    }

    /// <summary>
    /// 打印错误日志
    /// </summary>
    /// <param name="log">要打印的内容</param>
    /// <param name="args">格式化的参数</param>
    public static void Error(this object obj, string log, params object[] args)
    {
        ULogs.ULog.Error(string.Format(log, args));
    }
    /// <summary>
    /// 打印错误日志
    /// </summary>
    /// <param name="log">要打印的内容</param>
    public static void Error(this object obj, object log)
    {
        ULogs.ULog.Error(log);
    }

    /// <summary>
    /// 打印堆栈
    /// </summary>
    /// <param name="log">要打印的内容</param>
    /// <param name="args">格式化的参数</param>
    public static void Trace(this object obj, string log, params object[] args)
    {
        ULogs.ULog.Trace(string.Format(log, args));
    }
    /// <summary>
    /// 打印堆栈
    /// </summary>
    /// <param name="log">要打印的内容</param>
    public static void Trace(this object obj, object log)
    {
        ULogs.ULog.Trace(log);
    }
}


namespace ULogs
{
    /// <summary>
    /// 日志工具核心类
    /// </summary>
    public class ULog
    {
        /// <summary>
        /// unity类型的输出日志
        /// </summary>
        class UnityLogger : ILogger
        {
            Type type = Type.GetType("UnityEngine.Debug, UnityEngine");
            public void Log(string msg, ULogColor logColor)
            {
                if (logColor != ULogColor.None)
                {
                    msg = UnityLogColor(msg, logColor);
                }
                type.GetMethod("Log", new Type[] { typeof(object) }).Invoke(null, new object[] { msg });
            }

            public void Warn(string msg)
            {
                msg = UnityLogColor(msg, ULogColor.Yellow);
                type.GetMethod("LogWarning", new Type[] { typeof(object) }).Invoke(null, new object[] { msg });
            }

            public void Error(string msg)
            {
                msg = UnityLogColor(msg, ULogColor.Red);
                type.GetMethod("LogError", new Type[] { typeof(object) }).Invoke(null, new object[] { msg });
            }

            private string UnityLogColor(string msg, ULogColor color)
            {
                switch (color)
                {
                    case ULogColor.Red:
                        msg = string.Format("<color=#FF0000>{0}</color>", msg);
                        break;
                    case ULogColor.Green:
                        msg = string.Format("<color=#00FF00>{0}</color>", msg);
                        break;
                    case ULogColor.Blue:
                        msg = string.Format("<color=#0000FF>{0}</color>", msg);
                        break;
                    case ULogColor.Cyan:
                        msg = string.Format("<color=#00FFFF>{0}</color>", msg);
                        break;
                    case ULogColor.Magenta:
                        msg = string.Format("<color=#FF00FF>{0}</color>", msg);
                        break;
                    case ULogColor.Yellow:
                        msg = string.Format("<color=#FFFF00>{0}</color>", msg);
                        break;
                    case ULogColor.None:
                    default:
                        msg = string.Format("<color=#FF0000>{0}</color>", msg);
                        break;
                }
                return msg;
            }
        }

        /// <summary>
        /// 控制台类型的输出日志
        /// </summary>
        class ConsoleLogger : ILogger
        {
            public void Log(string msg, ULogColor logColor)
            {
                WriteConsoleLog(msg, logColor);
            }
            public void Warn(string msg)
            {
                WriteConsoleLog(msg, ULogColor.Yellow);
            }
            public void Error(string msg)
            {
                WriteConsoleLog(msg, ULogColor.Red);
            }
            private void WriteConsoleLog(string msg, ULogColor color)
            {
                switch (color)
                {
                    case ULogColor.Red:
                        Console.ForegroundColor = ConsoleColor.DarkRed;
                        Console.WriteLine(msg);
                        Console.ForegroundColor = ConsoleColor.Gray;
                        break;
                    case ULogColor.Green:
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine(msg);
                        Console.ForegroundColor = ConsoleColor.Gray;
                        break;
                    case ULogColor.Blue:
                        Console.ForegroundColor = ConsoleColor.Blue;
                        Console.WriteLine(msg);
                        Console.ForegroundColor = ConsoleColor.Gray;
                        break;
                    case ULogColor.Cyan:
                        Console.ForegroundColor = ConsoleColor.Cyan;
                        Console.WriteLine(msg);
                        Console.ForegroundColor = ConsoleColor.Gray;
                        break;
                    case ULogColor.Magenta:
                        Console.ForegroundColor = ConsoleColor.Magenta;
                        Console.WriteLine(msg);
                        Console.ForegroundColor = ConsoleColor.Gray;
                        break;
                    case ULogColor.Yellow:
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.WriteLine(msg);
                        Console.ForegroundColor = ConsoleColor.Gray;
                        break;
                    case ULogColor.None:
                    default:
                        Console.WriteLine(msg);
                        break;
                }
            }
        }



        /// <summary>
        /// 日志配置
        /// </summary>
        public static ULogConfig cfg;
        /// <summary>
        /// 日志输出
        /// </summary>
        private static ILogger logger;
        /// <summary>
        /// 日志写入文件
        /// </summary>
        private static StreamWriter logFileWriter = null;
        /// <summary>
        /// 日志初始化
        /// </summary>
        /// <param name="logConfig">日志配置【默认null，自动配置】</param>
        public static void InitSetting(ULogConfig logConfig = null)
        {
            if (logConfig == null)
            {
                logConfig = new ULogConfig();
            }
            ULog.cfg = logConfig;


            //unity或控制台类型的日志
            if (cfg.loggerType == ULoggerType.Console)
            {
                logger = new ConsoleLogger();
            }
            else
            {
                logger = new UnityLogger();
            }

            //是否保存日志
            if (!cfg.enableSave)
            {
                return;
            }

            //是否覆盖日志
            if (cfg.enableCover)
            {
                string path = cfg.savePath + cfg.saveName;
                try
                {
                    if (Directory.Exists(cfg.savePath))
                    {
                        if (File.Exists(path))
                        {
                            File.Delete(path);
                        }
                    }
                    else
                    {
                        Directory.CreateDirectory(cfg.savePath);
                    }
                    logFileWriter = File.AppendText(path);
                    logFileWriter.AutoFlush = true;
                }
                catch
                {
                    logFileWriter = null;
                }
            }
            else
            {
                string prefix = DateTime.Now.ToString("yyyyMMdd@HH-mm-ss.ffff");
                string path = cfg.savePath + prefix + cfg.saveName;
                try
                {
                    if (!Directory.Exists(cfg.savePath))
                    {
                        Directory.CreateDirectory(cfg.savePath);
                    }
                    logFileWriter = File.AppendText(path);
                    logFileWriter.AutoFlush = true;
                }
                catch
                {
                    logFileWriter = null;
                }
            }


        }




        #region public static【 Log、Warn、Error、StackTrace】

        /// <summary>
        /// 打印普通日志
        /// </summary>
        /// <param name="msg">要打印的内容</param>
        /// <param name="args">格式化的参数</param>
        public static void Log(string msg, params object[] args)
        {
            if (!cfg.enableLog) return;

            msg = DecorateLog(string.Format(msg, args));
            logger.Log(msg);
            if (cfg.enableSave)
            {
                WriteToFile(string.Format("[Log] {0}", msg));
            }
        }
        /// <summary>
        /// 打印普通日志
        /// </summary>
        /// <param name="obj">要打印的内容</param>
        public static void Log(object obj)
        {
            if (!cfg.enableLog) return;

            string msg = DecorateLog(obj.ToString());
            logger.Log(msg);
            if (cfg.enableSave)
            {
                WriteToFile(string.Format("[Log] {0}", msg));
            }
        }

        /// <summary>
        /// 打印带颜色的日志
        /// </summary>
        /// <param name="color">设置内容颜色</param>
        /// <param name="msg">要打印的内容</param>
        /// <param name="args">格式化的参数</param>
        public static void ColorLog(ULogColor color, string msg, params object[] args)
        {
            if (!cfg.enableLog) return;

            msg = DecorateLog(string.Format(msg, args));
            logger.Log(msg, color);
            if (cfg.enableSave)
            {
                WriteToFile(string.Format("[Log] {0}", msg));
            }
        }
        /// <summary>
        /// 打印带颜色的日志
        /// </summary>
        /// <param name="color">设置内容颜色</param>
        /// <param name="obj">要打印的内容</param>
        public static void ColorLog(ULogColor color, object obj)
        {
            if (!cfg.enableLog) return;

            string msg = DecorateLog(obj.ToString());
            logger.Log(msg, color);
            if (cfg.enableSave)
            {
                WriteToFile(string.Format("[Log] {0}", msg));
            }
        }

        /// <summary>
        /// 打印警告日志
        /// </summary>
        /// <param name="msg">要打印的内容</param>
        /// <param name="args">格式化的参数</param>
        public static void Warn(string msg, params object[] args)
        {
            if (!cfg.enableLog) return;

            msg = DecorateLog(string.Format(msg, args));
            logger.Warn(msg);
            if (cfg.enableSave)
            {
                WriteToFile(string.Format("[Warning] {0}", msg));
            }
        }
        /// <summary>
        /// 打印警告日志
        /// </summary>
        /// <param name="obj">要打印的内容</param>
        public static void Warn(object obj)
        {
            if (!cfg.enableLog) return;

            string msg = DecorateLog(obj.ToString());
            logger.Warn(msg);
            if (cfg.enableSave)
            {
                WriteToFile(string.Format("[Warning] {0}", msg));
            }
        }

        /// <summary>
        /// 打印错误日志
        /// </summary>
        /// <param name="msg">要打印的内容</param>
        /// <param name="args">格式化的参数</param>
        public static void Error(string msg, params object[] args)
        {
            if (!cfg.enableLog) return;

            msg = DecorateLog(string.Format(msg, args), true);
            logger.Error(msg);
            if (cfg.enableSave)
            {
                WriteToFile(string.Format("[Error] {0}", msg));
            }
        }
        /// <summary>
        /// 打印错误日志
        /// </summary>
        /// <param name="obj">要打印的内容</param>
        public static void Error(object obj)
        {
            if (!cfg.enableLog) return;

            string msg = DecorateLog(obj.ToString(), true);
            logger.Error(msg);
            if (cfg.enableSave)
            {
                WriteToFile(string.Format("[Error] {0}", msg));
            }
        }

        /// <summary>
        /// 打印堆栈
        /// </summary>
        /// <param name="msg">要打印的内容</param>
        /// <param name="args">格式化的参数</param>
        public static void Trace(string msg, params object[] args)
        {
            if (!cfg.enableLog) return;

            msg = DecorateLog(string.Format(msg, args), true);
            logger.Log(msg, ULogColor.Magenta);
            if (cfg.enableSave)
            {
                WriteToFile(string.Format("[Stack Trace] {0}", msg));
            }
        }
        /// <summary>
        /// 打印堆栈
        /// </summary>
        /// <param name="obj">要打印的内容</param>
        public static void Trace(object obj)
        {
            if (!cfg.enableLog) return;

            string msg = DecorateLog(obj.ToString(), true);
            logger.Log(msg, ULogColor.Magenta);
            if (cfg.enableSave)
            {
                WriteToFile(string.Format("[Stack Trace] {0}", msg));
            }
        }
        #endregion


        #region private static Tool

        /// <summary>
        /// 修饰日志
        /// </summary>
        /// <param name="msg">要修饰的内容</param>
        /// <param name="isTrace">是否显示堆栈【默认false不显示】</param>
        private static string DecorateLog(string msg, bool isTrace = false)
        {
            StringBuilder sb = new StringBuilder(cfg.logPrefix, 100);
            if (cfg.enableTime)
            {
                //时间格式   时：分：秒.毫秒
                sb.AppendFormat(" {0}", DateTime.Now.ToString("HH:mm:ss.ffff"));
            }
            if (cfg.enableThreadID)
            {
                sb.AppendFormat(" {0}", GetThreadID());
            }

            sb.AppendFormat(" {0} {1}", cfg.logSeparate, msg);
            if (isTrace)
            {
                sb.AppendFormat(" \nStackTrace:{0}", GetLogTrace());
            }

            return sb.ToString();
        }

        /// <summary>
        /// 获取线程ID
        /// </summary>
        /// <returns>返回当前线程ID</returns>
        private static string GetThreadID()
        {
            return string.Format(" ThreadID:{0}", Thread.CurrentThread.ManagedThreadId);
        }

        /// <summary>
        /// 获取堆栈信息
        /// </summary>
        /// <returns>返回堆栈信息</returns>
        private static string GetLogTrace()
        {
            StackTrace st = new StackTrace(3, true);
            string traceInfo = "";
            for (int i = 0; i < st.FrameCount; i++)
            {
                StackFrame tempSF = st.GetFrame(i);
                traceInfo += string.Format("\n {0}::{1} line:{2}", tempSF.GetFileName(), tempSF.GetMethod(), tempSF.GetFileLineNumber());
            }
            return traceInfo;
        }

        /// <summary>
        /// 写日志内容到文件
        /// </summary>
        /// <param name="msg">要写的内容</param>
        private static void WriteToFile(string msg)
        {
            try
            {
                logFileWriter.WriteLine(msg);
            }
            catch
            {
                logFileWriter = null;
            }

        }
        #endregion
    }
}

