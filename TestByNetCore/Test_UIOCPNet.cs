/// <summary>
///********************************************
/// ClassName    ：  Test_UIOCPNet
/// Author       ：  LCG
/// CreateTime   ：  2022/6/20 星期一
/// Description  ：  test UIOCPNet
///********************************************/
/// </summary>
using System;
using UIOCPNet;

namespace Test
{
    public class Test_UIOCPNet
    {

        public void Init()
        {
            IOCPServer server = new IOCPServer();
            server.StartAsServer("192.168.0.122", 19020, 1000);
        }

    }


}
