using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using UnityEngine.Networking.NetworkSystem;

public class Begin : NetworkBehaviour {
    const short MyBeginMsg = 1002;
    NetworkClient _client;
    public void SendReadyToBeginMessage(int myId)
    {
        var msg = new IntegerMessage();
        _client.Send(MyBeginMsg, msg);
    }
    public void Init(NetworkClient client)
    {
        _client = client;
        NetworkServer.RegisterHandler(MyBeginMsg, OnServerReadyToBeginMessage);
    }
    void OnServerReadyToBeginMessage(NetworkMessage netMsg)
    {
        var beginMessage = netMsg.ReadMessage<IntegerMessage>();
        Debug.Log("receive" + beginMessage.value);
    }
}
