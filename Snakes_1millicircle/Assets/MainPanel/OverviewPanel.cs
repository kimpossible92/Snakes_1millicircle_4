using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

using ExitGames.Client.Photon;
using Photon.Realtime;
using Photon.Pun.UtilityScripts;
using Photon.Pun;
using Photon.Pun.Demo.Asteroids;
using System.Linq;

public class OverviewPanel : MonoBehaviourPunCallbacks
{
    public GameObject PlayerOverviewEntryPrefab;

    private Dictionary<int, GameObject> playerListEntries;

    #region UNITY

    public void Awake()
    {
        playerListEntries = new Dictionary<int, GameObject>();
        Nickname = Nickname = new List<PlayerSpaceship>();
        foreach (Player2 p in PhotonNetwork.PlayerList)
        {
            GameObject entry = Instantiate(PlayerOverviewEntryPrefab);
            entry.transform.SetParent(gameObject.transform);
            entry.transform.localScale = Vector3.one;
            entry.GetComponent<Text>().color = AsteroidsGame.GetColor(p.GetPlayerNumber());
            entry.GetComponent<Text>().text = string.Format("{0}\nScore: {1}\nLives: {2}", p.NickName, p.GetScore(), AsteroidsGame.PLAYER_MAX_LIVES);

            playerListEntries.Add(p.ActorNumber, entry);
            PlayerSpaceship playerSpace = new PlayerSpaceship();
            playerSpace.NickName = p.NickName;
            playerSpace.score = 0;
            Nickname.Add(playerSpace);
        }
    }

    #endregion

    #region PUN CALLBACKS
    public List<PlayerSpaceship> Nickname = new List<PlayerSpaceship>();
    public List<string> nickScore = new List<string>();
    public void SetAddScore(string named)
    {
        foreach (var Nscore in Nickname)
        {
            if (Nscore.NickName == named) { Nscore.score += 1; }
        }
    }
    public void SetAddSc(string named)
    {
        foreach (var Nscore in Nickname)
        {
            if (Nscore.NickName == named) { Nscore.score += 10; }
        }
    }
    public void setHealth(int keyd,string named, int health)
    {
        foreach (var Nscore in Nickname)
        {
            if (Nscore.NickName == named) { Nscore.score = (int)health; }
        }
        foreach(var plListEnt in playerListEntries)
        {
            if (plListEnt.Key == keyd)
            {
                plListEnt.Value.GetComponent<Text>().text = string.Format("{0}\nScore: {1}\nLives: {2}", named, 0, health);
            }   
        }
    }
    public int CurrentScore(string named)
    {
        int n = 0;
        foreach (var Nscore in Nickname)
        {
            if (Nscore.NickName == named) { n = Nscore.score; }
        }
        return n;
    }
    public override void OnPlayerLeftRoom(Player2 otherPlayer)
    {
        GameObject go = null;
        if (this.playerListEntries.TryGetValue(otherPlayer.ActorNumber, out go))
        {
            Destroy(playerListEntries[otherPlayer.ActorNumber]);
            playerListEntries.Remove(otherPlayer.ActorNumber);
        }
    }

    public override void OnPlayerPropertiesUpdate(Player2 targetPlayer, Hashtable changedProps)
    {
        GameObject entry;
        if (playerListEntries.TryGetValue(targetPlayer.ActorNumber, out entry))
        {
            entry.GetComponent<Text>().text = string.Format("{0}\nScore: {1}\nLives: {2}", targetPlayer.NickName, CurrentScore(targetPlayer.NickName), targetPlayer.CustomProperties[AsteroidsGame.PLAYER_LIVES]);
        }
    }
    private void OnGUI()
    {
        int cp = 0;
        foreach (var Nscore in Nickname)
        {
            GUI.Label(new Rect(10, 75 + (cp * 10), 120, 32), Nscore.NickName + ":" + Nscore.score);
            cp += 1;
        }
    }
    #endregion
}