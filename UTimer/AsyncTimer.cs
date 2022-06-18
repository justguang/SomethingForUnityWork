/// <summary>
///********************************************
/// ClassName    ：  AsyncTimer
/// Author       ：  LCG
/// CreateTime   ：  2022/6/18 星期六
/// Description  ：  使用Async await语法驱动，运行在线程池中，可设置多线程或逻辑主线回调任务
///********************************************/
/// </summary>
using System;
using System.Collections.Concurrent;

namespace UTimers
{
    public class AsyncTimer : UTimer
    {
        private readonly ConcurrentDictionary<int, AsyncTask> taskDic;
        private const string tidLock = "AsyncTimer_tidLock";

        public override int AddTask(uint delay, Action<int> taskCallBack, Action<int> cancelCallBack, int count = 1)
        {
            return 0;
        }

        public override bool DelTask(int tid)
        {
            return false;
        }

        public override void Reset()
        {
        }

        protected override int GenerateId()
        {
            lock (tidLock)
            {
                while (true)
                {
                    ++tid;
                    if (tid == int.MaxValue)
                    {
                        tid = 0;
                    }
                    if (!taskDic.ContainsKey(tid))
                    {
                        return tid;
                    }
                }
            }
        }


        class AsyncTask
        {
            public int tid;
            public uint delay;
            public int count;

            public Action<int> taskCallBack;
            public Action<int> cancelCallBack;

            public DateTime startTime;
            public ulong loopIndex;

            public AsyncTask(
                int tid,
                uint delya,
                int count,
                Action<int> taskCallBack,
                Action<int> cancelCallBack)
            {
                this.tid = tid;
                this.delay = delya;
                this.count = count;
                this.taskCallBack = taskCallBack;
                this.cancelCallBack = cancelCallBack;
                this.startTime = DateTime.UtcNow; ;

                this.loopIndex = 0;
            }
        }
    }
}
