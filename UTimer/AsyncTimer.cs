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
using System.Threading;
using System.Threading.Tasks;

namespace UTimers
{
    public class AsyncTimer : UTimer
    {
        class AsyncTaskPack
        {
            public int tid;
            public Action<int> cb;

            public AsyncTaskPack(int tid, Action<int> cb)
            {
                this.tid = tid;
                this.cb = cb;
            }
        }

        private readonly ConcurrentDictionary<int, AsyncTask> taskDic;
        private ConcurrentQueue<AsyncTaskPack> packQue;
        private bool setHandle;
        private const string tidLock = "AsyncTimer_tidLock";
        
        private CancellationTokenSource cts;

        /// <summary>
        /// 实例化 AsyncTimer
        /// </summary>
        /// <param name="setHandle">如设置的setHandle=true，使用者需在外部update调用此函数驱动任务</param>
        public AsyncTimer(bool setHandle = false)
        {
            this.setHandle = setHandle;
            taskDic = new ConcurrentDictionary<int, AsyncTask>();
            if (setHandle)
            {
                packQue = new ConcurrentQueue<AsyncTaskPack>();
            }
            
            if (this.cts != null) this.cts.Cancel();
            this.cts = new CancellationTokenSource();
        }

        /// <summary>
        /// 添加定时任务
        /// </summary>
        /// <param name="delay">每次任务循环开始执行的延迟时间【单位毫秒】</param>
        /// <param name="taskCallBack">任务执行时回调</param>
        /// <param name="cancelCallBack">任务取消时回调</param>
        /// <param name="count">指定任务循环多少次【默认1次】, -1无限循环</param>
        /// <returns>返回该任务的id</returns>
        public override int AddTask(
            uint delay,
            Action<int> taskCallBack,
            Action<int> cancelCallBack,
            int count = 1)
        {
            int tid = GenerateId();
            AsyncTask task = new AsyncTask(tid, delay, count, taskCallBack, cancelCallBack);
            RunTaskInPool(task);

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

        /// <summary>
        /// 删除任务
        /// </summary>
        /// <param name="tid">要删除的任务的id</param>
        /// <returns>删除成功返回true</returns>
        public override bool DelTask(int tid)
        {
            if (taskDic.TryRemove(tid, out AsyncTask task))
            {
                LogFunc?.Invoke($"Remove tid:{tid} task in taskDic success.");
                task.cts.Cancel();

                if (setHandle && task.cancelCallBack != null)
                {
                    packQue.Enqueue(new AsyncTaskPack(task.tid, task.cancelCallBack));
                }
                else
                {
                    task.cancelCallBack?.Invoke(task.tid);
                }

                return true;
            }
            else
            {
                ErrorFunc?.Invoke($"Remove tid:{tid} task in taskDic failed.");
                return false;
            }

        }


        /// <summary>
        /// 重置，停止并清理所有任务
        /// </summary>
        public override void Reset()
        {
            if (packQue != null && !packQue.IsEmpty)
            {
                WarnFunc?.Invoke("Call Queue is not Empty.");
            }
            taskDic.Clear();
            tid = 0;

            if (this.cts != null)
            {
                this.cts.Cancel();
                this.cts.Dispose();
                this.cts = null;
            }

        }

        /// <summary>
        /// 如实例化AsyncTimer定时器，参数setHandle设置为true，使用者需在外部update调用此函数驱动定时任务执行回调
        /// </summary>
        public void UpdateHandleTask()
        {
            if (packQue != null && packQue.Count > 0)
            {
                if (packQue.TryDequeue(out AsyncTaskPack pack))
                {
                    pack.cb?.Invoke(pack.tid);
                }
                else
                {
                    WarnFunc?.Invoke($"packQue dequeue data failed.");
                }
            }
        }

        void RunTaskInPool(AsyncTask task)
        {
            Task.Run(async () =>
            {
                if (task.count > 0)
                {
                    //限定次数的循环任务
                    do
                    {
                        --task.count;//可循环次数-1
                        ++task.loopIndex;//已循环次数+1
                        int delay = (int)(task.delay + task.fixDelta);
                        if (delay > 0)
                        {
                            await Task.Delay(delay, task.ct);
                        }
                        TimeSpan ts = DateTime.UtcNow - task.startTime;
                        task.fixDelta = (int)(task.delay * task.loopIndex - ts.TotalMilliseconds);
                        CallBackTaskCB(task);
                    } while (task.count > 0);

                }
                else
                {
                    //无限循环任务
                    while (true)
                    {
                        ++task.loopIndex;//已循环次数+1
                        int delay = (int)(task.delay + task.fixDelta);
                        if (delay > 0)
                        {
                            await Task.Delay(delay, task.ct);
                        }
                        TimeSpan ts = DateTime.UtcNow - task.startTime;
                        task.fixDelta = (int)(task.delay * task.loopIndex - ts.TotalMilliseconds);
                        CallBackTaskCB(task);
                    }
                }
            },this.cts.Token);
        }

        void CallBackTaskCB(AsyncTask task)
        {
            if (setHandle)
            {
                packQue.Enqueue(new AsyncTaskPack(task.tid, task.taskCallBack));
            }
            else
            {
                task.taskCallBack.Invoke(task.tid);
            }

            if (task.count == 0)
            {
                if (taskDic.TryRemove(task.tid, out AsyncTask temp))
                {
                    LogFunc?.Invoke($"Task tid:{task.tid} tun to completion.");
                }
                else
                {
                    ErrorFunc?.Invoke($"Remove tid:{task.tid} task in taskDic failed.");
                }
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
                        tid = 1;
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
            public int tid;//唯一id
            public uint delay;//任务(循环)开始执行时的延迟【毫秒】
            public int count;//任务循环次数(默认1次)

            public Action<int> taskCallBack;//任务执行回调
            public Action<int> cancelCallBack;//任务取消回调

            public DateTime startTime;//开始时间
            public ulong loopIndex;//任务已循环次数
            public int fixDelta;//延迟累积产生的误差

            //取消任务的token
            public CancellationTokenSource cts;
            public CancellationToken ct;

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
                this.fixDelta = 0;

                cts = new CancellationTokenSource();
                ct = cts.Token;
            }

        }
    }
}
