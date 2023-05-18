using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class LobbyPlayer : NetworkLobbyPlayer {
    public void isLocalLobbyPlayer()
    {
        if (!isLocalPlayer)
            return;
        int playercount = 0;
        foreach (PlayerController player in ClientScene.localPlayers)
            playercount += (player == null || player.playerControllerId == -1) ? 0 : 1;
    }
    [ClientRpc]
    public void RpcPlayerCount(float Count)
    {
        MyLobby.lobbythis.showUI.GetComponent<showGui>().countTime = Count;
        //MyLobby.lobbythis.showUI.SetActive(Count != 0);
    }
    public void Counts(float counts)
    {

    }
    public void isLocalRemove()
    {
        if (!isLocalPlayer)
            return;
        int playercounts = 0;
        foreach(PlayerController p in ClientScene.localPlayers)
        {
            playercounts += (p == null || p.playerControllerId == -1) ? 0 : 1;
        }
        //RemovePlayer();
    }
    [ClientRpc]
    public void RpcRemove()
    {
        isLocalRemove();
    }
}
