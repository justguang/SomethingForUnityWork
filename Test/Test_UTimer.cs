/// <summary>
///********************************************
/// ClassName    ：  Test_UTimer
/// Author       ：  LCG
/// CreateTime   ：  2022/5/10 星期二 
/// Description  ：  测试计时器
///********************************************/
/// </summary>
using System;
using System.Threading.Tasks;
using ULogs;
using UTimers;

namespace Test
{
    public class Test_UTimer
    {
        public void Init()
        {
            ULog.InitSetting();

            ULog.Log("test UTimer Run...");
            TickTimer timer = new TickTimer(0,false)
            {
                LogFunc = ULog.Log,
                WarnFunc = ULog.Warn,
                ErrorFunc = ULog.Error
            };

            uint interval = 66;
            int count = 100;
            int sum = 0;
            int taskId = 0;

            Task.Run(async () =>
            {
                await Task.Delay(2000);
                DateTime historyTime = DateTime.UtcNow;
                taskId = timer.AddTask(
                    interval,
                    (int tid) =>
                    {
                        DateTime nowTime = DateTime.UtcNow;
                        TimeSpan ts = nowTime - historyTime;
                        historyTime = DateTime.UtcNow;
                        int delta = (int)(ts.TotalMilliseconds - interval);
                        ULog.ColorLog(ULogColor.Yellow, $"时间差:{delta}");

                        sum += Math.Abs(delta);
                        ULog.ColorLog(ULogColor.Magenta, "tid:{0} work.", tid);
                    },
                    (int tid) =>
                    {
                        ULog.ColorLog(ULogColor.Magenta, "tid:{0} cancel.", tid);
                    },
                    count);
            });

            Task.Run(async ()=> {
                while (true)
                {
                    timer.UpdateTask();
                    //timer.HandleTask();
                    await Task.Delay(2);
                }
            });

            while (true)
            {
                string opt = Console.ReadLine();
                if (opt == "calc")
                {
                    ULog.ColorLog(ULogColor.Red, "平均间隔：." + sum * 1.0f / count);
                }
                else if (opt == "d")
                {
                    timer.DelTask(taskId);
                }
            }
        }

    }
}
