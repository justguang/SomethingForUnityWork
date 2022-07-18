/// <summary>
///********************************************
/// ClassName    ：  ULogConfig
/// Author       ：  LCG
/// CreateTime   ：  2022/5/10 星期二
/// Description  ：  日志配置
///********************************************/
using System;

namespace ULogs
{
    /// <summary>
    /// 日志类型
    /// </summary>
    public enum ULoggerType
    {
        /// <summary>
        /// unity类型的日志
        /// </summary>
        Unity,
        /// <summary>
        /// 控制台类型的日志
        /// </summary>
        Console
    }


    /// <summary>
    /// 日志输出颜色
    /// </summary>
    public enum ULogColor
    {
        None,
        Red,
        Green,
        Blue,
        Cyan,
        Magenta,
        Yellow
    }


    /// <summary>
    /// ULog 配置
    /// </summary>
    public class ULogConfig
    {
        /// <summary>
        /// 日志启用开关【默认true，启用】
        /// </summary>
        public bool enableLog = true;
        /// <summary>
        /// 前缀标记【默认 #】
        /// </summary>
        public string logPrefix = "#";
        /// <summary>
        /// 是否显示时间标记【默认true，显示】
        /// </summary>
        public bool enableTime = true;
        /// <summary>
        /// 标记与日志具体内容间隔符号【默认 >>】
        /// </summary>
        public string logSeparate = ">";
        /// <summary>
        /// 是否显示线程ID【默认true，显示】
        /// </summary>
        public bool enableThreadID = true;
        /// <summary>
        /// 是否显示具体堆栈的消息【默认true，显示】
        /// </summary>
        public bool enableTrace = true;
        /// <summary>
        /// 是否将日志保存下来【默认true，保存】
        /// </summary>
        public bool enableSave = true;
        /// <summary>
        /// 是否覆盖原有保存的日志【默认true，覆盖】
        /// </summary>
        public bool enableCover = true;
        /// <summary>
        /// 日志类型【默认LoggerType.Console】
        /// </summary>
        public ULoggerType loggerType = ULoggerType.Console;
        /// <summary>
        /// 日志保存的路径【默认当前运行程序的更目录下Logs文件夹下】
        /// </summary>
        private string _savePath;
        /// <summary>
        /// 日志文件保存路径
        /// </summary>
        public string savePath
        {
            get
            {
                if (_savePath == null)
                {
                    if (loggerType == ULoggerType.Unity)
                    {
                        Type type = Type.GetType("UnityEngine.Application, UnityEngine");
                        _savePath = type.GetProperty("persistentDataPath").GetValue(null).ToString() + "/ULogs/";
                    }
                    else
                    {
                        _savePath = string.Format("{0}ULogs\\", AppDomain.CurrentDomain.BaseDirectory);
                    }
                }
                return _savePath;
            }
            set
            {
                _savePath = value;
            }
        }

        /// <summary>
        /// 日志文件保存的名字
        /// </summary>
        public string saveName = "ConsoleULog.txt";

    }


    interface ILogger
    {
        void Log(string msg, ULogColor logColor = ULogColor.None);

        void Warn(string msg);
        void Error(string msg);
    }

}
