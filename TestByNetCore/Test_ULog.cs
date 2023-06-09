/// <summary>
///********************************************
/// ClassName    ：  Test_ULog
/// Author       ：  LCG
/// CreateTime   ：  2022/5/10 星期二 
/// Description  ：  测试ULog
///********************************************/
/// </summary>
using System;
using RGuang.Utils;

namespace Test
{

    class Root
    {
        public void Init()
        {
            this.Log("Init Root Log.");
            Mgr mgr = new Mgr();
            mgr.Init();
        }
    }

    class Mgr
    {
        public void Init()
        {
            this.Warn("Init Mgr Warn");
            Item item = new Item();
            item.Init();
        }
    }

    class Item
    {
        public void Init()
        {
            this.Error("Init Item Error");
            this.Trace("Trace this Func");
        }
    }


    public class Test_ULog
    {
        public void Init()
        {
            ULog.InitSetting(new ULogConfig
            {
                logLevel =  ULoggerLevel.Trace|ULoggerLevel.Log,
                enableSave = true,
                enableTrace = true,
                enableTime = true,
                enableCover = false,
                saveName = "testULog.txt",
            });

            ULog.Log("【Log】  六六六!!! {0}", "ServerLCGLog");
            ULog.ColorLog(ULogColor.Blue, "【ColorLog】  六六六!!! color=> {0}={1}", "ColorLog.Blue", ULogColor.Blue);
            ULog.ColorLog(ULogColor.Green, "【ColorLog】 六六六!!! color=> {0}={1}", "ColorLog.Green", ULogColor.Green);
            ULog.ColorLog(ULogColor.Cyan, "【ColorLog】  六六六!!! color=> {0}={1}", "ColorLog.Cyan", ULogColor.Cyan);
            ULog.ColorLog(ULogColor.Magenta, "【ColorLog】 六六六!!! color=> {0}={1}", "ColorLog.Magenta", ULogColor.Magenta);
            ULog.ColorLog(ULogColor.Yellow, "【ColorLog】 六六六!!! color=> " + "ColorLog.Yellow=" + ULogColor.Yellow);
            ULog.ColorLog(ULogColor.Red, "【ColorLog】 六六六!!! color=> {0}", "ColorLog.Red=" + ULogColor.Red);



            Root r = new Root();
            r.Init();


        }
    }
}
