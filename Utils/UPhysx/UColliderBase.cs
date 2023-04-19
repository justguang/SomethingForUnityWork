
/// <summary>
///********************************************
/// ClassName    ：  UColliderBase
/// Author       ：  LCG
/// CreateTime   ：  2022/5/12 星期四 
/// Description  ：  碰撞体抽象基类
///********************************************/
/// </summary>
/// 
using RGuang.Utils.UMath;
namespace RGuang.Utils.UPhysx
{
    /// <summary>
    /// 碰撞体基类
    /// </summary>
    public abstract class UColliderBase {

        /// <summary>
        /// 碰撞体名
        /// </summary>
        public string name;
        /// <summary>
        /// 碰撞体位置
        /// </summary>
        public UVector3 mPos;

        /// <summary>
        /// 与目标碰撞体交互
        /// </summary>
        /// <param name="collider">目标碰撞体基类</param>
        /// <param name="normal">碰撞法线，矫正后的移动方向</param>
        /// <param name="borderAdjust">碰撞后的边界矫正</param>
        /// <returns>true 发送碰撞，false无碰撞</returns>
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


        /// <summary>
        /// 与Cylinder类型的碰撞体交互
        /// </summary>
        /// <param name="col">Cylinder类型的碰撞体</param>
        /// <param name="normal">碰撞法线，矫正后的移动方向</param>
        /// <param name="borderAdjust">碰撞后的边界矫正</param>
        /// <returns>true 发送碰撞，false无碰撞</returns>
        public abstract bool DetectSphereContact(UCylinderCollider col, ref UVector3 normal, ref UVector3 borderAdjust);


        /// <summary>
        /// 与box类型的碰撞体交互
        /// </summary>
        /// <param name="col">box类型的碰撞体</param>
        /// <param name="normal">碰撞法线，矫正后的移动方向</param>
        /// <param name="borderAdjust">碰撞后的边界矫正</param>
        /// <returns>true 发送碰撞，false无碰撞</returns>
        public abstract bool DetectBoxContact(UBoxCollider col, ref UVector3 normal, ref UVector3 borderAdjust);
    }

    /// <summary>
    /// 碰撞信息
    /// </summary>
    public class CollisionInfo {
        /// <summary>
        /// 目标碰撞体
        /// </summary>
        public UColliderBase collider;
        /// <summary>
        /// 碰撞后的法线向量【移动方向】
        /// </summary>
        public UVector3 normal;
        /// <summary>
        /// 碰撞后的位置矫正
        /// </summary>
        public UVector3 borderAdjust;
    }
}
