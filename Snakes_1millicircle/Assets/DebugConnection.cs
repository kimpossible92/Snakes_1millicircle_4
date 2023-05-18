using UnityEngine.Networking;
using System.Text;
class DebugConnection : NetworkConnection
{
    public override void TransportRecieve(byte[] bytes, int numBytes, int channelId)
    {
        StringBuilder msg = new StringBuilder();
        for (int i = 0; i < numBytes; i++)
        {
            var s = string.Format("{0:X2}", bytes[i]);
            msg.Append(s);
            if (i > 50) break;
        }
        HandleBytes(bytes, numBytes, channelId);
    }
    public override bool TransportSend(byte[] bytes, int numBytes, int channelId, out byte error)
    {
        StringBuilder msg = new StringBuilder();
        for (int i = 0; i < numBytes; i++)
        {
            var s = string.Format("{0:X2}", bytes[i]);
            msg.Append(s);
            if (i > 50) break;
        }
        return NetworkTransport.Send(hostId, connectionId, channelId, bytes, numBytes, out error);
    }
}
