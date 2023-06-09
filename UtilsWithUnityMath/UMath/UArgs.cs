
using System;
/// <summary>
///********************************************
/// ClassName    ：  UArgs
/// Author       ：  LCG
/// CreateTime   ：  2022/5/11 星期三
/// Description  ：  运算参数
///********************************************/
/// </summary>
namespace RGuang.Utils.UMath
{
    public struct UArgs
    {
        public int value;
        public uint multipler;

        public UArgs(int value, uint multipler)
        {
            this.value = value;
            this.multipler = multipler;
        }

        public static UArgs Zero = new UArgs(0, 10000);
        public static UArgs HALFPI = new UArgs(15708, 10000);
        public static UArgs PI = new UArgs(31416, 10000);
        public static UArgs TWOPI = new UArgs(62832, 10000);

        public static bool operator >(UArgs a, UArgs b)
        {
            if (a.multipler == b.multipler)
            {
                return a.value > b.value;
            }
            else
            {
                throw new System.Exception("multipler is unequal.");
            }
        }
        public static bool operator <(UArgs a, UArgs b)
        {
            if (a.multipler == b.multipler)
            {
                return a.value < b.value;
            }
            else
            {
                throw new System.Exception("multipler is unequal.");
            }
        }
        public static bool operator >=(UArgs a, UArgs b)
        {
            if (a.multipler == b.multipler)
            {
                return a.value >= b.value;
            }
            else
            {
                throw new System.Exception("multipler is unequal.");
            }
        }
        public static bool operator <=(UArgs a, UArgs b)
        {
            if (a.multipler == b.multipler)
            {
                return a.value <= b.value;
            }
            else
            {
                throw new System.Exception("multipler is unequal.");
            }
        }
        public static bool operator ==(UArgs a, UArgs b)
        {
            if (a.multipler == b.multipler)
            {
                return a.value == b.value;
            }
            else
            {
                throw new System.Exception("multipler is unequal.");
            }
        }
        public static bool operator !=(UArgs a, UArgs b)
        {
            if (a.multipler == b.multipler)
            {
                return a.value != b.value;
            }
            else
            {
                throw new System.Exception("multipler is unequal.");
            }
        }

        /// <summary>
        /// 转化为视图角度，不可再用于逻辑运算
        /// </summary>
        public int ConvertViewAngle()
        {
            float radians = ConvertToFloat();
            return (int)Math.Round(radians / Math.PI * 180);
        }

        /// <summary>
        /// 转化为视图弧度，不可再用于逻辑运算
        /// </summary>
        public float ConvertToFloat()
        {
            return value * 1.0f / multipler;
        }


        public override bool Equals(object obj)
        {
            return obj is UArgs args &&
                value == args.value &&
                multipler == args.multipler;
        }

        public override int GetHashCode()
        {
            return value.GetHashCode();
        }

        public override string ToString()
        {
            return $"value:{value} multipler:{multipler}";
        }
    }
}
