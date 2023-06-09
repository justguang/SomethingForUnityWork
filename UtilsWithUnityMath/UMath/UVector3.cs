/// <summary>
///********************************************
/// ClassName    ：  UVector3
/// Author       ：  LCG
/// CreateTime   ：  2022/5/11 星期三
/// Description  ：  确定性向量运算
///********************************************/
/// </summary>
using System;
#if UNITY_ENV
using UnityEngine;
#endif
namespace RGuang.Utils.UMath
{
    public struct UVector3
    {
        public UInt x;
        public UInt y;
        public UInt z;

        public UVector3(UInt x, UInt y, UInt z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }

#if UNITY_ENV
        public UVector3(Vector3 v)
        {
            this.x = (UInt)v.x;
            this.y = (UInt)v.y;
            this.z = (UInt)v.z;
        }
#endif

        public UInt this[int index]
        {
            get
            {
                switch (index)
                {
                    case 0:
                        return x;
                    case 1:
                        return y;
                    case 2:
                        return z;
                    default:
                        return 0;
                }
            }
            set
            {
                switch (index)
                {
                    case 0:
                        x = value;
                        break;
                    case 1:
                        y = value;
                        break;
                    case 2:
                        z = value;
                        break;
                }
            }
        }

        #region 常用的向量
        public static UVector3 zero
        {
            get
            {
                return new UVector3(0, 0, 0);
            }
        }
        public static UVector3 one
        {
            get
            {
                return new UVector3(1, 1, 1);
            }
        }
        public static UVector3 forward
        {
            get
            {
                return new UVector3(0, 0, 1);
            }
        }
        public static UVector3 back
        {
            get
            {
                return new UVector3(0, 0, -1);
            }
        }
        public static UVector3 left
        {
            get
            {
                return new UVector3(-1, 0, 0);
            }
        }
        public static UVector3 right
        {
            get
            {
                return new UVector3(1, 0, 0);
            }
        }
        public static UVector3 up
        {
            get
            {
                return new UVector3(0, 1, 0);
            }
        }
        public static UVector3 down
        {
            get
            {
                return new UVector3(0, -1, 0);
            }
        }
        #endregion

        #region 运算符
        public static UVector3 operator +(UVector3 v1, UVector3 v2)
        {
            UInt x = v1.x + v2.x;
            UInt y = v1.y + v2.y;
            UInt z = v1.z + v2.z;
            return new UVector3(x, y, z);
        }
        public static UVector3 operator -(UVector3 v1, UVector3 v2)
        {
            UInt x = v1.x - v2.x;
            UInt y = v1.y - v2.y;
            UInt z = v1.z - v2.z;
            return new UVector3(x, y, z);
        }
        public static UVector3 operator *(UVector3 v, UInt value)
        {
            UInt x = v.x * value;
            UInt y = v.y * value;
            UInt z = v.z * value;
            return new UVector3(x, y, z);
        }
        public static UVector3 operator *(UInt value, UVector3 v)
        {
            UInt x = v.x * value;
            UInt y = v.y * value;
            UInt z = v.z * value;
            return new UVector3(x, y, z);
        }
        public static UVector3 operator /(UVector3 v, UInt value)
        {
            UInt x = v.x / value;
            UInt y = v.y / value;
            UInt z = v.z / value;
            return new UVector3(x, y, z);
        }
        public static UVector3 operator -(UVector3 v)
        {
            UInt x = -v.x;
            UInt y = -v.y;
            UInt z = -v.z;
            return new UVector3(x, y, z);
        }
        public static bool operator ==(UVector3 v1, UVector3 v2)
        {
            return v1.x == v2.x && v1.y == v2.y && v1.z == v2.z;
        }
        public static bool operator !=(UVector3 v1, UVector3 v2)
        {
            return v1.x != v2.x || v1.y != v2.y || v1.z != v2.z;
        }
        #endregion

        /// <summary>
        /// 当前向量长度的平方
        /// </summary>
        public UInt sqrMagnitude
        {
            get
            {
                return x * x + y * y + z * z;
            }
        }

        public static UInt SqrMagnitude(UVector3 v)
        {
            return v.x * v.x + v.y * v.y + v.z * v.z;
        }

        public UInt magnitude
        {
            get
            {
                return UCalc.Sqrt(this.sqrMagnitude);
            }
        }

        /// <summary>
        /// 返回当前定点向量的单位向量
        /// </summary>
        public UVector3 normalized
        {
            get
            {
                if (magnitude > 0)
                {
                    UInt rate = UInt.one / magnitude;
                    return new UVector3(x * rate, y * rate, z * rate);
                }
                else
                {
                    return zero;
                }
            }
        }

        /// <summary>
        /// 返回传入的定点向量的单位向量
        /// </summary>
        /// <param name="v">传入的定点向量</param>
        public static UVector3 Normalize(UVector3 v)
        {
            if (v.magnitude > 0)
            {
                UInt rate = UInt.one / v.magnitude;
                return new UVector3(v.x * rate, v.y * rate, v.z * rate);
            }
            else
            {
                return zero;
            }
        }

        /// <summary>
        /// 规格化当前定点向量为单位向量
        /// </summary>
        public void Normalize()
        {
            UInt rate = UInt.one / magnitude;
            x = x * rate;
            y = y * rate;
            z = z * rate;
        }

        /// <summary>
        /// 点乘
        /// </summary>
        public static UInt Dot(UVector3 a, UVector3 b)
        {
            return a.x * b.x + a.y * b.y + a.z * b.z;
        }

        /// <summary>
        /// 叉乘
        /// </summary>
        public static UVector3 Crose(UVector3 a, UVector3 b)
        {
            return new UVector3(a.y * b.z - a.z * b.y,
                                a.z * b.x - a.x * b.z,
                                a.x * b.y - a.y * b.x);
        }

        /// <summary>
        /// 获取两个向量的夹角
        /// </summary>
        public static UArgs Angle(UVector3 from, UVector3 to)
        {
            UInt dot = Dot(from, to);
            UInt mod = from.magnitude * to.magnitude;
            if (mod == 0)
            {
                return UArgs.Zero;
            }
            UInt value = dot / mod;

            //返余弦计算
            return UCalc.Acos(value);
        }

#if UNITY_ENV
        /// <summary>
        /// 获取浮点数向量，【注意：不可再进行逻辑运算】
        /// </summary>
        public Vector3 ConvertViewVector3()
        {
            return new Vector3(x.RawFloat, y.RawFloat, z.RawFloat);
        }
#endif

        public long[] ConvertLongArray()
        {
            return new long[] { x.ScaledValue, y.ScaledValue, z.ScaledValue };
        }

        public override bool Equals(object obj)
        {
            if (obj == null) return false;

            UVector3 v = (UVector3)obj;

            return v.x == x && v.y == y && v.z == z;
        }

        public override int GetHashCode()
        {
            return x.GetHashCode();
        }

        /// <summary>
        /// function  ToString
        /// </summary>
        /// <returns> x;{0} y:{1} z:{2} </returns>
        public override string ToString()
        {
            return string.Format("x;{0} y:{1} z:{2}", x, y, z);
        }

    }
}
