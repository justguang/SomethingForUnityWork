/// <summary>
///********************************************
/// ClassName    ：  EnvColliders
/// Author       ：  LCG
/// CreateTime   ：  2022/5/12 星期四 
/// Description  ：  确定性碰撞环境
///********************************************/
/// </summary>
using System.Collections.Generic;

namespace PEPhysxs {
    public class EnvColliders {
        public List<ColliderConfig> envColliCfgLst;

        List<UColliderBase> envColliLst;

        public void Init() {
            envColliLst = new List<UColliderBase>();
            for(int i = 0; i < envColliCfgLst.Count; i++) {
                ColliderConfig cfg = envColliCfgLst[i];
                if(cfg.mType == ColliderType.Box) {
                    envColliLst.Add(new UBoxCollider(cfg));
                }
                else if(cfg.mType == ColliderType.Cylinder) {
                    envColliLst.Add(new UCylinderCollider(cfg));
                }
                else {
                    //TODO...其他形状的碰撞器
                }
            }
        }

        public List<UColliderBase> GetEnvColliders() {
            return envColliLst;
        }
    }
}
