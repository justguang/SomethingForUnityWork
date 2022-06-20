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
            IOCPMsg msg = new IOCPMsg
            {
                msg = "justguang",
            };


            byte[] data = IOCPTool.Serialize(msg);
            IOCPMsg msg_data = IOCPTool.DesSerialize(data);


            IOCPServer server = new IOCPServer();
            server.StartAsServer("192.168.1.122", 19021, 1000);
        }

    }


}
