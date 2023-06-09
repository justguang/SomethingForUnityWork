#if UNITY_2021_1_OR_NEWER
using UnityEngine;
using UnityEngine.Pool;

namespace RGuang.Utils.Pool
{
    public class ExampleGameObjectPool : BaseGamePool
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

        private void Awake()
        {
            mDefaultSize = DefaultSize;
            mMaxSize = MaxSize;
            mCheckPool = CheckPool;
            InitPool(DefaultSize, MaxSize, CheckPool);
        }

        protected override GameObject OnCreatePoolItem()
        {
            return base.OnCreatePoolItem();
        }

        protected override void OnGetPoolItem(GameObject obj)
        {
            base.OnGetPoolItem(obj);
        }

        protected override void OnReleasePoolItem(GameObject obj)
        {
            base.OnReleasePoolItem(obj);
        }

        protected override void OnDestroyPoolItem(GameObject obj)
        {
            base.OnDestroyPoolItem(obj);
        }

        public void SetPrefab(GameObject prefab)
        {
            this.mPrefab = prefab;
        }

    }

}

#endif

