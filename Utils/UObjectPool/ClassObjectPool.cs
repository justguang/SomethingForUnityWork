using System;
using System.Collections.Generic;

namespace RGuang.Utils
{
    /// <summary>
    /// 自定义类型对象池
    /// </summary>
    public class ClassObjectPool : IDisposable
    {
        readonly object mLock = "mLock";

        #region 构造
        public ClassObjectPool()
        {
            mMaxSize = 70;
            mTypeNum = 70;
            mPool = new Dictionary<int, Queue<object>>(mTypeNum);
            UnUsedCount = new Dictionary<Type, int>(mTypeNum);
        }
        /// <param name="typeNum">对象种类数量</param>
        /// <param name="poolMaxSize">每个对象池最大容量</param>
        public ClassObjectPool(uint typeNum, uint poolMaxSize)
        {
            mMaxSize = (int)poolMaxSize;
            mTypeNum = (int)typeNum;
            mPool = new Dictionary<int, Queue<object>>(mTypeNum);
            UnUsedCount = new Dictionary<Type, int>(mTypeNum);
        }
        #endregion

        #region 数据
        int mTypeNum = 100;//对象种类数量
        /// <summary>
        /// 获取池内对象种类数量
        /// </summary>
        public int TypeNum => mTypeNum;

        int mMaxSize = 100;//对象池最大容量
        /// <summary>
        /// 获取池最大容量
        /// </summary>
        public int PoolMaxSize => mMaxSize;

        /// <summary>
        /// key => 类型哈希码
        /// </summary>
        private readonly Dictionary<int, Queue<object>> mPool;
        /// <summary>
        /// 显示所有对象池限制对象
        /// </summary>
        public readonly Dictionary<Type, int> UnUsedCount;//闲置对象
        /// <summary>
        /// 获取指定类型的对象池内对象数量，如果对象池不存在返回-1
        /// </summary>
        /// <typeparam name="T">指定类型</typeparam>
        /// <returns></returns>
        public int GetUnUsedCount<T>()
        {
            var tp = typeof(T);
            if (UnUsedCount.ContainsKey(tp))
            {
                return UnUsedCount[tp];
            }
            return -1;
        }
        #endregion



        /// <summary>
        /// 创建新对象池
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="key">对象类型的HashCode作为key</param>
        /// <returns>返回true则创建成功</returns>
        bool CreateObjectPool<T>(int key) where T : class, new()
        {
            bool result = false;
            lock (mLock)
            {
                if (!mPool.ContainsKey(key))
                {
                    if (mPool.Count < mTypeNum)
                    {
                        mPool.Add(key, new Queue<object>(mMaxSize));
                        UnUsedCount.Add(typeof(T), 0);
                    }
                    else
                    {
                        RGuang.Utils.ULog.Warn("获取大对象池数量达最大，{0}", mTypeNum);
                    }
                }
            }

            if (mPool.ContainsKey(key)) result = true;
            return result;
        }


        /// <summary>
        /// 取对象
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <returns></returns>
        public T Get<T>() where T : class, new()
        {
            T result = null;
            var key = typeof(T).GetHashCode();
            var exist = mPool.ContainsKey(key);
            if (!exist) exist = CreateObjectPool<T>(key);
            lock (mLock)
            {
                if (exist && mPool[key].TryDequeue(out object obj))
                {
                    result = obj as T;
                    UnUsedCount[typeof(T)] -= 1;
                }
            }

            if (result == null)
            {
                if (exist)
                {
                    //对象池存在，但池内没有对象，创建新对象
                    result = new T();
                }
                else
                {
                    //对象池不存在...
                    RGuang.Utils.ULog.Warn("获取对象池不存在，对象类型:{0}", typeof(T));
                }
            }

            return result;
        }

        /// <summary>
        /// 还对象
        /// </summary>
        /// <param name="obj">要归还的对象</param>
        public void Release<T>(object obj) where T : class, new()
        {
            var key = typeof(T).GetHashCode();
            if (mPool.ContainsKey(key))
            {
                lock (mLock)
                {
                    if (mPool.ContainsKey(key))
                    {
                        var pool = mPool[key];
                        if (pool.Count < mMaxSize)
                        {
                            pool.Enqueue(obj);
                            UnUsedCount[typeof(T)] += 1;
                        }
                        else
                        {
                            RGuang.Utils.ULog.Warn("获取对象池{0},数量达最大，{1}", typeof(T), mMaxSize);
                        }
                    }

                }
            }
            else
            {
                RGuang.Utils.ULog.Warn("归还对象池不存在，对象类型:{0}", typeof(T));
            }
        }

        /// <summary>
        /// 释放对象池资源
        /// </summary>
        public void Dispose()
        {
            mPool.Clear();
            UnUsedCount.Clear();
        }
    }

}

