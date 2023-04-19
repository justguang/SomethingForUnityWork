
/// <summary>
///********************************************
/// ClassName    ：  ColliderConfig
/// Author       ：  LCG
/// CreateTime   ：  2022/5/12 星期四 
/// Description  ：  碰撞体配置
///********************************************/
/// </summary>
using RGuang.Utils.UMath;
namespace RGuang.Utils.UPhysx
{

    /// <summary>
    /// 碰撞体配置
    /// </summary>
    public class ColliderConfig {

        /// <summary>
        /// 碰撞体名字
        /// </summary>
        public string mName;
        /// <summary>
        /// 碰撞体类型
        /// </summary>
        public ColliderType mType;
        /// <summary>
        /// 碰撞体位置
        /// </summary>
        public UVector3 mPos;

        #region box 类型的属性
        /// <summary>
        /// 轴向x、y、z三个方向
        /// </summary>
        public UVector3[] mAxis;
        /// <summary>
        /// 大小
        /// </summary>
        public UVector3 mSize;
        #endregion

        #region cylinder 类型的属性
        /// <summary>
        /// 半径
        /// </summary>
        public UInt mRadius;
        #endregion
    }


    /// <summary>
    /// 碰撞体类型
    /// </summary>
    public enum ColliderType {

        /// <summary>
        /// 方形【矩形】
        /// </summary>
        Box,
        /// <summary>
        /// 圆柱形
        /// </summary>
        Cylinder,
    }
}
