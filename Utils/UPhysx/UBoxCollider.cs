
/// <summary>
///********************************************
/// ClassName    ：  UBoxCollider
/// Author       ：  LCG
/// CreateTime   ：  2022/5/12 星期四 
/// Description  ：  确定性矩形碰撞体
///********************************************/
/// </summary>
/// 
using RGuang.Utils.UMath;
namespace RGuang.Utils.UPhysx
{
    /// <summary>
    /// 方形碰撞体
    /// </summary>
    public class UBoxCollider : UColliderBase {
        /// <summary>
        /// 碰撞体大小
        /// </summary>
        public UVector3 mSize;
        /// <summary>
        /// 轴向x、y、z【right、up、forward】
        /// </summary>
        public UVector3[] mDir;//轴向

        public UBoxCollider(ColliderConfig cfg) {
            mPos = cfg.mPos;
            mSize = cfg.mSize;
            mDir = new UVector3[3];
            mDir[0] = cfg.mAxis[0];
            mDir[1] = cfg.mAxis[1];
            mDir[2] = cfg.mAxis[2];
            name = cfg.mName;
        }

        public override bool DetectBoxContact(UBoxCollider col, ref UVector3 normal, ref UVector3 borderAdjust) {
            //分离轴算法TODO...
            return false;
        }

        public override bool DetectSphereContact(UCylinderCollider col, ref UVector3 normal, ref UVector3 borderAdjust) {
            UVector3 tmpNormal = UVector3.zero;
            UVector3 tmpAdjust = UVector3.zero;
            bool result = col.DetectBoxContact(this, ref tmpNormal, ref tmpAdjust);
            normal = -tmpNormal;
            borderAdjust = -tmpAdjust;
            return result;
        }
    }
}
