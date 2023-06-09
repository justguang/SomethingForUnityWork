#if UNITY_2021_1_OR_NEWER
using UnityEngine;
using UnityEngine.Pool;

namespace RGuang.Utils.Pool
{

    /// <summary>
    /// 普通Class类型对象池
    /// </summary>
    /// <typeparam name="T">要求对象可实例化的引用类型</typeparam>
    public class ExampleClassPool<T> : BaseClassPool<T> where T : class, new()
    {
        /// <summary>
        /// 新池默认大小
        /// </summary>
        public int DefaultSize = 50;
        /// <summary>
        /// 池最大大小【防爆内存】
        /// </summary>
        public int MaxSize = 500;
        /// <summary>
        /// true 每次归还，对对象重复检测，消耗些性能，打包发布时建议false
        /// </summary>
        public bool CheckPool = true;

        public ExampleClassIPool<T> InitPool()
        {
            mDefaultSize = DefaultSize;
            mMaxSize = MaxSize;
            mCheckPool = CheckPool;
            InitPool(DefaultSize, MaxSize, CheckPool);
            return this;
        }
        public ExampleClassIPool<T> InitPool(int size, int maxSize)
        {
            mDefaultSize = size;
            mMaxSize = maxSize;
            mCheckPool = CheckPool;
            base.InitPool(size, maxSize);
            return this;
        }

        //当池内没有元素时，回调此方法创建新对象
        protected override T OnCreatePoolItem()
        {
            return base.OnCreatePoolItem();
        }

        //当池内元素数量超过最大数量时，回调此方法销毁对象
        protected override void OnDestroyPoolItem(T obj)
        {
            //System.GC.ReRegisterForFinalize(obj);
            base.OnDestroyPoolItem(obj);
        }

        //当从池内获取元素时，回调此方法进行附加操作
        protected override void OnGetPoolItem(T obj)
        {
            base.OnGetPoolItem(obj);
        }

        //当将对象归还池内时，回调此方法进行附加操作
        protected override void OnReleasePoolItem(T obj)
        {
            base.OnReleasePoolItem(obj);
        }

    }
}

#endif
