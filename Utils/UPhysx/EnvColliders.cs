/// <summary>
///********************************************
/// ClassName    ：  EnvColliders
/// Author       ：  LCG
/// CreateTime   ：  2022/5/12 星期四 
/// Description  ：  确定性碰撞环境
///********************************************/
/// </summary>
using System.Collections.Generic;

namespace RGuang.Utils.UPhysx
{
    public class EnvColliders
    {

        /// <summary>
        /// 所有碰撞配置
        /// </summary>
        public List<ColliderConfig> envColliCfgLst;

        /// <summary>
        /// 根据碰撞配置生成的逻辑层碰撞体
        /// </summary>
        List<UColliderBase> envColliLst;

        public void Init()
        {
            envColliLst = new List<UColliderBase>();

            //遍历所有碰撞配置，生成逻辑层碰撞体
            int cfgCount = envColliCfgLst.Count;
            for (int i = 0; i < cfgCount; i++)
            {
                ColliderConfig cfg = envColliCfgLst[i];
                if (cfg.mType == ColliderType.Box)
                {
                    envColliLst.Add(new UBoxCollider(cfg));
                }
                else if (cfg.mType == ColliderType.Cylinder)
                {
                    envColliLst.Add(new UCylinderCollider(cfg));
                }
                else
                {
                    //TODO...其他形状的碰撞器
                }
            }
        }


        /// <summary>
        /// 获取逻辑层所有环境碰撞
        /// </summary>
        /// <returns> 返回 UColliderBase类型的List集合 </returns>
        public List<UColliderBase> GetEnvColliders()
        {
            return envColliLst;
        }
    }
}
