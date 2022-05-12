/// <summary>
///********************************************
/// ClassName    ：  UCylinderCollider
/// Author       ：  LCG
/// CreateTime   ：  2022/5/12 星期四 
/// Description  ：  确定性圆柱体碰撞体
///********************************************/
/// </summary>
using System.Collections.Generic;
using UMaths;

namespace PEPhysxs {
    public class UCylinderCollider : UColliderBase {
        public UInt mRadius;

        public UCylinderCollider(ColliderConfig cfg) {
            mPos = cfg.mPos;
            mRadius = cfg.mRadius;
            name = cfg.mName;
        }

        public void CalcCollidersInteraction(List<UColliderBase> colliders, ref UVector3 velocity, ref UVector3 borderAdjust) {
            if(velocity == UVector3.zero) {
                return;
            }
            List<CollisionInfo> collisionInfoList = new List<CollisionInfo>();
            UVector3 normal = UVector3.zero;
            UVector3 adj = UVector3.zero;
            for(int i = 0; i < colliders.Count; i++) {
                if(DetectContact(colliders[i], ref normal, ref adj)) {
                    CollisionInfo info = new CollisionInfo {
                        collider = colliders[i],
                        normal = normal,
                        borderAdjust = adj
                    };
                    collisionInfoList.Add(info);
                }
            }

            if(collisionInfoList.Count == 1) {
                //单个碰撞体，修正速度
                CollisionInfo info = collisionInfoList[0];
                velocity = CorrectVelocity(velocity, info.normal);
                borderAdjust = info.borderAdjust;
                //this.Log("单个碰撞体，校正速度：" + velocity.ConvertViewVector3().ToString());
            }
            else if(collisionInfoList.Count > 1) {
                UVector3 centerNormal = UVector3.zero;
                CollisionInfo info = null;
                UArgs borderNormalAngle = CalcMaxNormalAngle(collisionInfoList, velocity, ref centerNormal, ref info);
                UArgs angle = UVector3.Angle(-velocity, centerNormal);
                if(angle > borderNormalAngle) {
                    velocity = CorrectVelocity(velocity, info.normal);
                    //this.Log("多个碰撞体，校正速度：" + velocity.ConvertViewVector3());
                    UVector3 adjSum = UVector3.zero;
                    for(int i = 0; i < collisionInfoList.Count; i++) {
                        adjSum += collisionInfoList[i].borderAdjust;
                    }
                    borderAdjust = adjSum;
                }
                else {
                    velocity = UVector3.zero;
                    //this.Log("速度方向反向量在校正法线夹角内，无法移动：" + angle.ConvertViewAngle());
                }
            }
            else {
                //this.Log("no contact objs.");
            }
        }

        private UArgs CalcMaxNormalAngle(List<CollisionInfo> infoList, UVector3 velocity, ref UVector3 centerNormal, ref CollisionInfo info) {
            for(int i = 0; i < infoList.Count; i++) {
                centerNormal += infoList[i].normal;
            }
            centerNormal /= infoList.Count;

            UArgs normalAngle = UArgs.Zero;
            UArgs velocityAngle = UArgs.Zero;
            for(int i = 0; i < infoList.Count; i++) {
                UArgs tmpNorAngle = UVector3.Angle(centerNormal, infoList[i].normal);
                if(normalAngle < tmpNorAngle) {
                    normalAngle = tmpNorAngle;
                }

                //找出速度方向与法线方向夹角最大的碰撞法线，速度校正由这个法线来决定
                UArgs tmpVelAngle = UVector3.Angle(velocity, infoList[i].normal);
                if(velocityAngle < tmpVelAngle) {
                    velocityAngle = tmpVelAngle;
                    info = infoList[i];
                }
            }

            return normalAngle;
        }

        private UVector3 CorrectVelocity(UVector3 velocity, UVector3 normal) {
            if(normal == UVector3.zero) {
                return velocity;
            }

            //确保是靠近，不是远离
            if(UVector3.Angle(normal, velocity) > UArgs.HALFPI) {
                UInt prjLen = UVector3.Dot(velocity, normal);
                if(prjLen != 0) {
                    velocity -= prjLen * normal;
                }
            }
            return velocity;
        }

        public override bool DetectBoxContact(UBoxCollider col, ref UVector3 normal, ref UVector3 borderAdjust) {
            UVector3 disOffset = mPos - col.mPos;
            //计算向量在长方体轴向上的投影长度
            UInt dot_disX = UVector3.Dot(disOffset, col.mDir[0]);
            UInt dot_disZ = UVector3.Dot(disOffset, col.mDir[2]);
            //投影钳制在轴向里
            UInt clamp_x = UCalc.Clamp(dot_disX, -col.mSize.x, col.mSize.x);
            UInt clamp_z = UCalc.Clamp(dot_disZ, -col.mSize.z, col.mSize.z);
            //计算轴向上的投影向量
            UVector3 s_x = clamp_x * col.mDir[0];
            UVector3 s_z = clamp_z * col.mDir[2];
            //计算表面最近的接触点：碰撞体中心位置+轴向偏移
            UVector3 point = col.mPos;
            point += s_x;
            point += s_z;

            UVector3 po = mPos - point;
            po.y = 0;

            if(UVector3.SqrMagnitude(po) > mRadius * mRadius) {
                return false;
            }
            else {
                normal = po.normalized;
                UInt len = po.magnitude;
                borderAdjust = normal * (mRadius - len);
                return true;
            }
        }

        public override bool DetectSphereContact(UCylinderCollider col, ref UVector3 normal, ref UVector3 borderAdjust) {
            UVector3 disOffset = mPos - col.mPos;
            if(UVector3.SqrMagnitude(disOffset) > (mRadius + col.mRadius) * (mRadius + col.mRadius)) {
                return false;
            }
            else {
                normal = disOffset.normalized;
                borderAdjust = normal * (mRadius + col.mRadius - disOffset.magnitude);
                return true;
            }
        }
    }
}
