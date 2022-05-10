/// <summary>
///********************************************
/// ClassName    ：  TickTimer
/// Author       ：  LCG
/// CreateTime   ：  2022/5/10 星期二 
/// Description  ：  毫秒级的精确定时器
///********************************************/
/// </summary>
using System;
using System.Collections.Concurrent;
using System.Threading;

namespace UTimers
{
    public class TickTimer : UTimer
    {
        class TickTaskPack
        {
            public int tid;
            public Action<int> cb;
            public TickTaskPack(int tid, Action<int> cb)
            {
                this.tid = tid;
                this.cb = cb;
            }
        }

        //1970-01-01 00：00：00.00
        private readonly DateTime metaStartDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0);
        private readonly bool setHandle;
        private readonly ConcurrentDictionary<int, TickTask> taskDic;
        private readonly ConcurrentQueue<TickTaskPack> packQueue;
        private const string tidLock = "TickTimer_tidLock";


        private readonly Thread timerThread;
        public TickTimer(int interval = 0, bool setHandle = true)
        {
            this.setHandle = setHandle;
            taskDic = new ConcurrentDictionary<int, TickTask>();

            if (setHandle)
            {
                packQueue = new ConcurrentQueue<TickTaskPack>();
            }

            if (interval != 0)
            {
                void StartTick()
                {
                    try
                    {
                        while (true)
                        {
                            UpdateTask();
                            Thread.Sleep(interval);
                        }
                    }
                    catch (ThreadAbortException e)
                    {
                        WarnFunc?.Invoke($"Tick Thread Abort:{e}.");
                    }
                }

                timerThread = new Thread(new ThreadStart(StartTick));
                timerThread.Start();
            }
        }


        public override int AddTask(uint delay, Action<int> taskCallBack, Action<int> cancelCallBack, int count = 1)
        {
            int tid = GenerateId();
            double startTime = GetUTCMilliseconds();
            double destTime = startTime + delay;
            TickTask task = new TickTask(tid, delay, count, destTime, taskCallBack, cancelCallBack, startTime);
            if (taskDic.TryAdd(tid, task))
            {
                return tid;
            }
            else
            {
                WarnFunc?.Invoke($"KEY:{tid} already exist~!");
                return -1;
            }

        }
        public override bool DelTask(int tid)
        {
            if (taskDic.TryRemove(tid, out TickTask task))
            {
                if (setHandle && task.cancelCallBack != null)
                {
                    packQueue.Enqueue(new TickTaskPack(tid, task.cancelCallBack));
                }
                else
                {
                    task.cancelCallBack?.Invoke(tid);
                }
                return true;
            }
            else
            {
                WarnFunc?.Invoke($"tid:{tid} remove failed.");
                return false;
            }
        }
        public override void Reset()
        {
            if (!packQueue.IsEmpty)
            {
                WarnFunc?.Invoke($"callback queue is not empty.");
            }

            taskDic.Clear();
            if (timerThread != null)
            {
                timerThread.Abort();
            }
        }
        public void UpdateTask()
        {
            double nowTime = GetUTCMilliseconds();
            foreach (var item in taskDic)
            {
                TickTask task = item.Value;
                if (nowTime < task.destTime) continue;

                ++task.loopIndex;
                if (task.count > 0)
                {
                    --task.count;
                    if (task.count == 0)
                    {
                        //taskDic属于线程安全字典， 遍历中删除元素无影响
                        FinishTask(task.tid);
                    }
                    else
                    {
                        task.destTime = task.startTime + task.delay * (task.loopIndex + 1);
                        CallTaskCB(task.tid, task.taskCallBack);
                    }
                }
                else
                {
                    task.destTime = task.startTime + task.delay * (task.loopIndex + 1);
                    CallTaskCB(task.tid, task.taskCallBack);
                }
            }
        }
        public void HandleTask()
        {
            while (packQueue!=null&&packQueue.Count>0)
            {
                if(packQueue.TryDequeue(out TickTaskPack pack))
                {
                    pack.cb.Invoke(pack.tid);
                }
                else
                {
                    ErrorFunc?.Invoke($"packQueue dequeue data Error!!!");
                }
            }
        }
        
        void FinishTask(int tid)
        {
            if (taskDic.TryRemove(tid, out TickTask task))
            {
                CallTaskCB(task.tid, task.taskCallBack);
                task.taskCallBack = null;
            }
            else
            {
                WarnFunc?.Invoke($"Remove tid:{tid} task in Dic failed.");
            }
        }
        void CallTaskCB(int tid, Action<int> taskCB)
        {
            if (setHandle)
            {
                packQueue.Enqueue(new TickTaskPack(tid, taskCB));
            }
            else
            {
                taskCB.Invoke(tid);
            }
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
        private double GetUTCMilliseconds()
        {
            TimeSpan ts = DateTime.UtcNow - metaStartDateTime;
            return ts.TotalMilliseconds;
        }


        class TickTask
        {
            public int tid;
            public uint delay;
            public int count;
            public double destTime;
            public Action<int> taskCallBack;
            public Action<int> cancelCallBack;

            public double startTime;
            public ulong loopIndex;

            public TickTask(
                int tid,
                uint delya,
                int count,
                double destTime,
                Action<int> taskCallBack,
                Action<int> cancelCallBack,
                double startTime)
            {
                this.tid = tid;
                this.delay = delya;
                this.count = count;
                this.destTime = destTime;
                this.taskCallBack = taskCallBack;
                this.cancelCallBack = cancelCallBack;
                this.startTime = startTime;

                this.loopIndex = 0;
            }
        }

    }
}
