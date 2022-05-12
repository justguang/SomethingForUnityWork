/// <summary>
///********************************************
/// ClassName    ：  ColliderConfig
/// Author       ：  LCG
/// CreateTime   ：  2022/5/12 星期四 
/// Description  ：  碰撞体配置
///********************************************/
/// </summary>
using UMaths;

namespace PEPhysxs {
    public class ColliderConfig {
        public string mName;
        public ColliderType mType;
        public UVector3 mPos;

        //box
        public UVector3 mSize;
        public UVector3[] mAxis;//轴向

        //cylinder
        public UInt mRadius;//半径
    }

    public enum ColliderType {
        Box,
        Cylinder,
    }
}
