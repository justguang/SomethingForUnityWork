# UKCP

using RGuang.Net.UKcp;

namespace RGuang.Net
使用 Kcp 2.0【源码：https://github.com/KumoKyaku/KCP】

基于udp封装可靠通信网络库

序列化【Serialize、DeSerializ】：MemoryStream、BinaryFormatter
压缩【Compress、DeCompress】：MemoryStream、GZipStream

【Client】
using UKcps;
UKCPNet<ClientSession, NetMsg> client = new UKCPNet<ClientSession, NetMsg>();
client.StartAsClient("192.168.1.100", 6688);
client.ConnectServer(200, 5000);//5000连接多久超时，200每次尝试连接间隔，时间单位毫秒。连接成功返回true

【Server】
using UKcps;
UKCPNet<ServerSession, NetMsg> server = new UKCPNet<ServerSession, NetMsg>();
server.StartAsServer("192.168.1.100", 6688);


【ServerSession、ClientSession】使用者自己定义的类，需继承KCPSession，处理连接和接收消息；
【NetMsg】使用者自己定义的网络消息协议类，需继承KCPMsg。


【发送网络消息】
client.clientSession.SendMsg(xxx);
server.BroadCastMsg(xxx);



