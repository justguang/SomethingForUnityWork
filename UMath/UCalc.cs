/// <summary>
///********************************************
/// ClassName    ：  UCalc
/// Author       ：  LCG
/// CreateTime   ：  2022/5/11 星期三
/// Description  ：  常用定点数数学运算
///********************************************/
/// </summary>
using System;

namespace UMaths
{
    public class UCalc
    {
        public static UInt Sqrt(UInt value, int interatorCount = 8)
        {
            if (value == UInt.zero) return 0;

            if (value < UInt.zero)
            {
                throw new Exception();
            }

            UInt result = value;
            UInt history;
            int count = 0;
            do
            {
                history = result;
                result = (result + value / result) >> 1;
                ++count;
            } while (result != history && count < interatorCount);

            return result;
        }

        public static UArgs Acos(UInt value)
        {
            UInt rate = (value * AcosTable.HalfIndexCount) + AcosTable.HalfIndexCount;
            rate = Clamp(rate, UInt.zero, AcosTable.IndexCount);
            return new UArgs(AcosTable.table[rate.RawInt], AcosTable.Multipler);
        }

        public static UInt Clamp(UInt value, UInt min, UInt max)
        {
            if (value < min)
            {
                return min;
            }
            if (value > max)
            {
                return max;
            }
            return value;
        }

    }
}
