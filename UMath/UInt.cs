/// <summary>
///********************************************
/// ClassName    ：  UInt
/// Author       ：  LCG
/// CreateTime   ：  2022/5/10 星期二 
/// Description  ：  定点数运算
///********************************************/
/// </summary>
using System;

namespace UMaths
{
    public struct UInt
    {
        private long scaledValue;
        public long ScaledValue
        {
            get { return scaledValue; }
            set { scaledValue = value; }
        }

        //位移计数
        const int BIT_MOVE_COUNT = 10;
        const long MULTIPLIER_FACTOR = 1 << BIT_MOVE_COUNT;

        public static readonly UInt zero = new UInt(0);
        public static readonly UInt one = new UInt(1);

        #region 构造函数
        //供给内部使用，scaledValue已经缩放后的数据
        private UInt(long scaledValue)
        {
            this.scaledValue = scaledValue;
        }

        public UInt(float val)
        {
            scaledValue = (long)Math.Round(val * MULTIPLIER_FACTOR);
        }

        public UInt(int val)
        {
            scaledValue = val * MULTIPLIER_FACTOR;
        }

        //float损失精度，必须显示转换
        public static explicit operator UInt(float f)
        {
            return new UInt((long)Math.Round(f * MULTIPLIER_FACTOR));
        }

        //int不损失精度，可以隐式转换
        public static implicit operator UInt(int i)
        {
            return new UInt(i);
        }

        #endregion

        #region 运算符
        public static UInt operator +(UInt a, UInt b)
        {
            return new UInt(a.scaledValue + b.scaledValue);
        }

        public static UInt operator -(UInt a, UInt b)
        {
            return new UInt(a.scaledValue - b.scaledValue);
        }

        public static UInt operator *(UInt a, UInt b)
        {
            long value = a.scaledValue * b.scaledValue;
            if (value >= 0)
            {
                value >>= BIT_MOVE_COUNT;
            }
            else
            {
                value = -(-value >> BIT_MOVE_COUNT);
            }
            return new UInt(value);
        }

        public static UInt operator /(UInt a, UInt b)
        {
            if (b.scaledValue == 0)
            {
                throw new Exception("除数不能为零.");
            }

            return new UInt((a.scaledValue << BIT_MOVE_COUNT) / b.scaledValue);
        }

        public static UInt operator -(UInt value)
        {
            return new UInt(-value.scaledValue);
        }


        public static bool operator ==(UInt a, UInt b)
        {
            return a.scaledValue == b.scaledValue;
        }

        public static bool operator !=(UInt a, UInt b)
        {
            return a.scaledValue != b.scaledValue;
        }

        public static bool operator >(UInt a, UInt b)
        {
            return a.scaledValue > b.scaledValue;
        }

        public static bool operator <(UInt a, UInt b)
        {
            return a.scaledValue < b.scaledValue;
        }

        public static bool operator >=(UInt a, UInt b)
        {
            return a.scaledValue >= b.scaledValue;
        }

        public static bool operator <=(UInt a, UInt b)
        {
            return a.scaledValue <= b.scaledValue;
        }

        public static UInt operator >>(UInt value, int moveCount)
        {
            if (value.scaledValue >= 0)
            {
                return new UInt(value.scaledValue >> moveCount);
            }
            else
            {
                return new UInt(-(-value.scaledValue >> moveCount));
            }
        }

        public static UInt operator <<(UInt value, int moveCount)
        {
            return new UInt(value.scaledValue << moveCount);
        }
        #endregion


        /// <summary>
        /// 转换完成后，不可再参与逻辑运算
        /// </summary>
        public float RawFloat
        {
            get { return scaledValue * 1.0f / MULTIPLIER_FACTOR; }
        }

        public int RawInt
        {
            get
            {
                if (scaledValue >= 0)
                {
                    return (int)(scaledValue >> BIT_MOVE_COUNT);
                }
                else
                {
                    return -(int)(-scaledValue >> BIT_MOVE_COUNT);
                }
            }
        }

        public override bool Equals(object obj)
        {
            if (obj == null) return false;
            UInt vInt = (UInt)obj;
            return scaledValue == vInt.scaledValue;
        }

        public override int GetHashCode()
        {
            return scaledValue.GetHashCode();
        }

        public override string ToString()
        {
            return RawFloat.ToString();
        }
    }
}
