using UnityEngine;
using System;
using System.Net;
using System.Collections;
using System.Net.Sockets;
using UnityEngine.UI;

public class GetNetTime : MonoBehaviour {
    public static System.DateTime NetworkTime()
    {
        //default windows server
        const string ntpServer = "time.windows.com";
        //NTP message size -16 bytes of the digest
        var ntpData = new byte[48];
        ntpData[0] = 0x1B;
        var addresses = Dns.GetHostEntry(ntpServer).AddressList;
        var ipEndPoint = new IPEndPoint(addresses[0], 123);
        var socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
        socket.Connect(ipEndPoint);
        socket.ReceiveTimeout = 3000;
        socket.Send(ntpData);
        socket.Receive(ntpData);
        socket.Close();
        const byte serverReplyTime = 40;
        ulong intPart = System.BitConverter.ToUInt32(ntpData, serverReplyTime);
        ulong fractPart = System.BitConverter.ToUInt32(ntpData, serverReplyTime + 4);
        intPart = SwapEndianness(intPart);
        fractPart = SwapEndianness(fractPart);
        var milliseconds = (intPart * 1000) + ((fractPart * 1000) / 0x100000000L);
        var networkDateTime = (new System.DateTime(1900, 1, 1, 0, 0, 0, System.DateTimeKind.Utc)).AddMilliseconds((long)milliseconds);
        return networkDateTime;
    }

    private static uint SwapEndianness(ulong x)
    {
        return (uint)(((x & 0x000000ff) << 24) +
            ((x & 0x0000ff00) << 8) +
            ((x & 0x00ff0000) >> 8) +
            ((x & 0xff000000) >> 24));
    }

    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
