using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using ExitGames.Client.Photon;
using Photon.Realtime;
using Photon.Pun.UtilityScripts;
using Photon.Pun;
using Photon.Pun.Demo.Asteroids;

public class PlrListEntry : MonoBehaviour
{
    [Header("UI References")]
    public Text PlayerNameText;

    public Image PlayerColorImage;
    public Button PlayerReadyButton;
    public Image PlayerReadyImage;

    private int ownerId;
    private bool isPlayerReady;

    #region UNITY

    public void OnEnable()
    {
        PlayerNumbering.OnPlayerNumberingChanged += OnPlayerNumberingChanged;
    }
    public void Start()
    {
        if (PhotonNetwork.LocalPlayer.ActorNumber != ownerId)
        {
            PlayerReadyButton.gameObject.SetActive(false);
        }
        else
        {
            ExitGames.Client.Photon.Hashtable initialProps = new ExitGames.Client.Photon.Hashtable() { { AsteroidsGame.PLAYER_READY, isPlayerReady }, { AsteroidsGame.PLAYER_LIVES, AsteroidsGame.PLAYER_MAX_LIVES } };
            PhotonNetwork.LocalPlayer.SetCustomProperties(initialProps);
            PhotonNetwork.LocalPlayer.SetScore(0);

            PlayerReadyButton.onClick.AddListener(() =>
            {
                isPlayerReady = !isPlayerReady;
                SetPlayerReady(isPlayerReady);

                ExitGames.Client.Photon.Hashtable props = new ExitGames.Client.Photon.Hashtable() { { AsteroidsGame.PLAYER_READY, isPlayerReady } };
                PhotonNetwork.LocalPlayer.SetCustomProperties(props);

                if (PhotonNetwork.IsMasterClient)
                {
                    if (FindObjectOfType<LobbyMainPanel>() != null) FindObjectOfType<LobbyMainPanel>().LocalPlayerPropertiesUpdated();
                    else { }
                }

            });
        }
    }

    public void OnDisable()
    {
        PlayerNumbering.OnPlayerNumberingChanged -= OnPlayerNumberingChanged;
    }

    #endregion

    public void Initialize(int playerId, string playerName)
    {
        ownerId = playerId;
        PlayerNameText.text = playerName;
    }

    private void OnPlayerNumberingChanged()
    {
        foreach (Player p in PhotonNetwork.PlayerList)
        {
            if (p.ActorNumber == ownerId)
            {
                PlayerColorImage.color = AsteroidsGame.GetColor(p.GetPlayerNumber());
            }
        }
    }

    public void SetPlayerReady(bool playerReady)
    {
        PlayerReadyButton.GetComponentInChildren<Text>().text = playerReady ? "Ready!" : "Ready?";
        PlayerReadyImage.enabled = playerReady;
    }
}
