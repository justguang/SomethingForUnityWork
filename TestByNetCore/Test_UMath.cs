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
            //Test_UInt();
            //Test_UCalc();
            Test_UVector3();
        }


        void Test_UCalc()
        {
            UInt val = 3;
            Console.WriteLine(UCalc.Sqrt(val));
            Console.WriteLine();
        }

        void Test_UVector3()
        {
            UVector3 v = new UVector3(2, 2, 2);
            Console.WriteLine(v.magnitude);
            Console.WriteLine();

            Console.WriteLine("a:" + v);
            Console.WriteLine("b:" + UVector3.Normalize(v));
            Console.WriteLine("c:" + v.normalized);
            v.Normalize();
            Console.WriteLine("d:" + v);
            Console.WriteLine("--------------");

            UVector3 v1 = new UVector3(1, 0, 0);
            UVector3 v2 = new UVector3(1, 1, 0);
            UArgs uArgs = UVector3.Angle(v1, v2);
            Console.WriteLine(string.Format("angle value:{0}\n float:{1}\n info:{2}\n ",
                                            uArgs.ConvertViewAngle(),uArgs.ConvertToFloat(),uArgs));

            Console.WriteLine("--------------");


            UVector3 v3 = new UVector3(1, 0, 0);
            UVector3 v4 = new UVector3(1, (UInt)1.732f, 0);
            UArgs uArgs2 = UVector3.Angle(v3, v4);
            Console.WriteLine(string.Format("angle2 value:{0}\n float:{1}\n info:{2}\n ",
                                            uArgs2.ConvertViewAngle(), uArgs2.ConvertToFloat(), uArgs2));

        }

        void Test_UInt()
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
