/// <summary>
///********************************************
/// ClassName    ：  UColliderBase
/// Author       ：  LCG
/// CreateTime   ：  2022/5/12 星期四 
/// Description  ：  碰撞体抽象基类
///********************************************/
/// </summary>
using UMaths;

namespace UPhysxs {
    public abstract class UColliderBase {
        public string name;
        public UVector3 mPos;

        public virtual bool DetectContact(UColliderBase collider, ref UVector3 normal, ref UVector3 borderAdjust) {
            if(collider is UBoxCollider) {
                return DetectBoxContact((UBoxCollider)collider, ref normal, ref borderAdjust);
            }
            else if(collider is UCylinderCollider) {
                return DetectSphereContact((UCylinderCollider)collider, ref normal, ref borderAdjust);
            }
            else {
                //TODO:
                return false;
            }
        }

        public abstract bool DetectSphereContact(UCylinderCollider col, ref UVector3 normal, ref UVector3 borderAdjust);

        public abstract bool DetectBoxContact(UBoxCollider col, ref UVector3 normal, ref UVector3 borderAdjust);
    }

    /// <summary>
    /// 碰撞信息
    /// </summary>
    public class CollisionInfo {
        public UColliderBase collider;
        public UVector3 normal;
        public UVector3 borderAdjust;
    }
}
