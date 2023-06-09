#if UNITY_2021_1_OR_NEWER
using UnityEngine;
using UnityEngine.Pool;

namespace RGuang.Utils
{

    /// <summary>
    /// 普通class对象池基类
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class BaseClassPool<T> where T : class, new()
    {
    
        /// <summary>
        /// 默认池大小
        /// </summary>
        protected int mDefaultSize = 50;
        /// <summary>
        /// 池最大大小，超出最大数量，对象池则销毁超出的元素，防止爆内存【产生GC】
        /// </summary>
        protected int mMaxSize = 500;
        /// <summary>
        /// true 对池内重复对象检测，占用些性能开销，建议打包发布时设置false
        /// </summary>
        protected bool mCheckPool = true;
        /// <summary>
        /// object pool
        /// </summary>
        ObjectPool<T> mPool;

        /// <summary>
        /// 从池内已调用的元素数量
        /// </summary>
        public int mActiveCount => mPool.CountActive;
        /// <summary>
        /// 池内未调用的元素数量
        /// </summary>
        public int mInactiveCount => mPool.CountInactive;
        /// <summary>
        /// 池内总的元素数量
        /// </summary>
        public int mTotalCount => mPool.CountAll;
        
        /// <summary>
        /// 对象池初始化
        /// </summary>
        /// <param name="collectionCheck">true，开启检测重复item，会占用些性能【建议：开发调式阶段true，打包发布阶段false】</param>
        protected void InitPool() => mPool = new ObjectPool<GameObject>(OnCreatePoolItem,
            OnGetPoolItem, OnReleasePoolItem, OnDestroyPoolItem, mCheckPool, mDefaultSize, mMaxSize);

        /// <summary>
        /// 对象池初始化
        /// </summary>
        /// <param name="collectionCheck">true，开启检测重复item，会占用些性能【建议：开发调式阶段true，打包发布阶段false】</param>
        protected void InitPool(bool collectionCheck = true) => mPool = new ObjectPool<T>(OnCreatePoolItem,
            OnGetPoolItem, OnReleasePoolItem, OnDestroyPoolItem, collectionCheck, mDefaultSize, mMaxSize);

        /// <summary>
        /// 对象池初始化
        /// </summary>
        /// <param name="size">默认池内元素数量</param>
        /// <param name="maxSize">池内可容纳元素最大数量</param>
        /// <param name="collectionCheck">true，开启检测重复item，会占用些性能【建议：开发调式阶段true，打包发布阶段false】</param>
        protected void InitPool(int size, int maxSize, bool collectionCheck = true) => mPool = new ObjectPool<T>(OnCreatePoolItem,
            OnGetPoolItem, OnReleasePoolItem, OnDestroyPoolItem, collectionCheck, size, maxSize);

        /// <summary>
        /// 池内创建元素方式
        /// </summary>
        /// <returns>返回创建好的元素</returns>
        protected virtual T OnCreatePoolItem() => new T();
        /// <summary>
        /// 池内获取元素操作
        /// </summary>
        /// <param name="obj">要操作的元素</param>
        protected virtual void OnGetPoolItem(T obj) { }
        /// <summary>
        /// 池内归还元素操作
        /// </summary>
        /// <param name="obj">要操作的元素</param>
        protected virtual void OnReleasePoolItem(T obj) { }
        /// <summary>
        /// 池内销毁元素的操作
        /// </summary>
        /// <param name="obj">要操作的元素</param>
        protected virtual void OnDestroyPoolItem(T obj) { }//System.GC.ReRegisterForFinalize(obj);


        /// <summary>
        /// 从池内获取一个元素
        /// </summary>
        /// <returns></returns>
        public T Get() => mPool.Get();
        /// <summary>
        /// 将元素归还池内
        /// </summary>
        /// <param name="obj">归还的元素</param>
        public void Release(T obj) => mPool.Release(obj);
        /// <summary>
        /// 清空对象池
        /// </summary>
        public void Clear() => mPool.Clear();

    }
}
#endif
