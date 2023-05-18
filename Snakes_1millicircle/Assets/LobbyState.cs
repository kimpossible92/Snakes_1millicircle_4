using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
using UnityEngine.Networking.NetworkSystem;
using UnityEngine.Networking.Types;

public class LobbyState : NetworkBehaviour {
    void OnDestroy()
    {
        ((MyLobby)MyLobby.singleton).AddClientDisconnect(lobbyReconn);
        ((MyLobby)MyLobby.singleton).AddClientConnect(Connec);
    }
    void Connec()
    {
        StringMessage m = new StringMessage(MyLobby.singleton.matchInfo.address);
        ClientScene.AddPlayer(MyLobby.singleton.client.connection, 0, m);
    }
    void lobbyReconn()
    {
        StartCoroutine(Reconn());
    }
    private IEnumerator Reconn()
    {
        NetworkTransport.RemoveHost(MyLobby.singleton.client.connection.hostId);
        MyLobby.singleton.client.Shutdown();
        MyLobby.singleton.client = null;
        yield return new WaitForSeconds(1.0f);
        NetworkTransport.Shutdown();
        MyLobby.singleton.StartClient();
    }
}
