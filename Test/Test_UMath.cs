/// <summary>
///********************************************
/// ClassName    ：  Test_UMath
/// Author       ：  LCG
/// CreateTime   ：  2022/5/11 星期三
/// Description  ：  测试 UMath
///********************************************/
/// </summary>

using System;
using UMaths;

namespace Test
{
    public class Test_UMath
    {
        public void Init()
        {

            UInt val1 = new UInt(3);
            UInt val2 = new UInt(1.5f);

            //比较
            if (val1 > val2)
            {
                Console.WriteLine("val1 > val2 ");
            }
            else
            {
                Console.WriteLine("val1 <= val2 ");
            }

            //位移
            UInt val3 = val1 << 1;
            Console.WriteLine("val1 << 1 =" + val3);

            //显示转换和隐式转换
            UInt val5 = (UInt)0.5f;
            UInt val4 = 1;
            Console.WriteLine(string.Format("val4:{0},  val5:{1}", val4, val5));

            Console.WriteLine();

            //乘除
            Console.WriteLine((val1 * val2).ToString());
            Console.WriteLine((val1 / val2).ToString());

            //模拟
            int hp = 100;
            var valHP = hp * new UInt(0.3f);
            Console.WriteLine("before scale=" + valHP.ScaledValue);
            Console.WriteLine("before float=" + valHP.RawFloat);
            Console.WriteLine("before int=" + valHP.RawInt);

            valHP = hp * new UInt(-0.3f);
            Console.WriteLine("after scale=" + valHP.ScaledValue);
            Console.WriteLine("after float=" + valHP.RawFloat);
            Console.WriteLine("after int=" + valHP.RawInt);

        }
    }
}
