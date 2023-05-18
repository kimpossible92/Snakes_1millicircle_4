using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Collections;
using System;
using UnityEngine.Networking.Match;
using UnityEngine.Networking.Types;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.Networking.NetworkSystem;

public class MyLobby : NetworkLobbyManager
{
    public bool disconnectedHost = false;
    static public  MyLobby lobbythis;
    private const int mc = 1;//maxConnections
    private const int mp = 4;
    private const int mppc = 2;
    public bool back = false;
    public int panel;
    public NetworkConnection clientConn;
    private int Netstate;
    public int SvatAndDirect = 0;//0=None, 1=matchMaking, 2=Direct.
    public int scenemode;
    private int connectServer = 0;//0=None, 1=connected, 2=Disconnected
    DebugConnection debuconnection;
    protected ulong matchId;
    public Action<NetworkPlayer> playerJoin;//joined
    public Action<NetworkPlayer> playerLeft;
    public event Action<NetworkConnection> ClientDisconnected;
    public event Action<bool, string> ChangeScene;
    public event Action<bool, MatchInfo> MatchCreate;
    private event Action<bool, MatchInfo> MatchCreated;
    public event Action<bool, MatchInfo> matchJoined;
    private Action<bool, MatchInfo> boolMatchJoined;
    private int localPlayersCount;
    public delegate void LobbyServerDisconnectHandler();
    public delegate void LobbyClientDisconnectHandler();
    public delegate void LobbyClientConnectHandler();
    public delegate void LobbyServerConnectHandler();
    public delegate void LobbyServerAddPlayerHandler(NetworkConnection conn,short playerControllerId, NetworkReader extraMessageReader);
    private LobbyServerDisconnectHandler LobbyServerDisconnect = new LobbyServerDisconnectHandler(OnLobbyServerDisconnectInternal);
    private LobbyClientDisconnectHandler LobbyClientDisconnect = new LobbyClientDisconnectHandler(OnLobbyClientDisconnectInternal);
    private LobbyServerConnectHandler LobbyServerConnect = new LobbyServerConnectHandler(LobbyServerConnectInternal);
    private LobbyClientConnectHandler LobbyClientConnect = new LobbyClientConnectHandler(LobbyClientConnectInternal);
    private LobbyServerAddPlayerHandler lobbyServerAddPlayer = new LobbyServerAddPlayerHandler(LobbyServerAddPlayerInternal);
    private List<PlayerController> playercontrollers;
    //protected LobbyHook lbHook;
    public GameObject showUI;
    [SerializeField]
    Text Infos;
    private static void OnLobbyServerDisconnectInternal()
    {

    }
    private static void OnLobbyClientDisconnectInternal()
    {

    }
    private static void LobbyServerConnectInternal()
    {

    }
    private static void LobbyClientConnectInternal()
    {

    }
    private static void LobbyServerAddPlayerInternal(NetworkConnection conn, short playerControllerId, NetworkReader extraMessageReader)
    {

    }
    public void AddLobbyServerPlayer(LobbyServerAddPlayerHandler handler)
    {
        lobbyServerAddPlayer += handler;
    }
    public void RemoveLobbyServerPlayer(LobbyServerAddPlayerHandler handler)
    {
        lobbyServerAddPlayer -= handler;
    }
    public void AddClientDisconnect(LobbyClientDisconnectHandler handler)
    {
        LobbyClientDisconnect += handler;
    }
    public void RemoveClientHandler(LobbyClientDisconnectHandler handler)
    {
        LobbyClientDisconnect -= handler;
    }
    public void AddServerDisconnect(LobbyServerDisconnectHandler handler)
    {
        LobbyServerDisconnect += handler;
    }
    public void RemoveServerDisconnectHandler(LobbyServerDisconnectHandler handler)
    {
        LobbyServerDisconnect -= handler;
    }
    public void AddClientConnect(LobbyClientConnectHandler handler)
    {
        LobbyClientConnect += handler;
    }
    public void RemoveClientConnect(LobbyClientConnectHandler handler)
    {
        LobbyClientConnect -= handler;
    }
    public void AddServerConnect(LobbyServerConnectHandler handler)
    {
        LobbyServerConnect += handler;
    }
    public void RemoveServerConnect(LobbyServerConnectHandler handler)
    {
        LobbyServerConnect -= handler;
    }
    void Start()
    {
        NetworkServer.SetNetworkConnectionClass<DebugConnection>();
        client = new NetworkClient();
        client.RegisterHandler(MsgType.Error, OnError);
        //lbHook = GetComponent<LobbyHook>();
        lobbythis = this;
        back = false;
    }
    void OnError(NetworkMessage netMsg)
    {
        var errorMsg = netMsg.ReadMessage<ErrorMessage>();
        Debug.Log("Error:" + errorMsg.errorCode);
    }
    public void CreateInternetMatch(string matchName, Action<bool, MatchInfo> onCreate)
    {
        //0
        SvatAndDirect = 1;
        StartMatchMaker();
        MatchCreated = onCreate;
        matchMaker.CreateMatch(matchName, 4, true, string.Empty, string.Empty, string.Empty, 0, 0, OnMatchCreate);
    }
    public override void OnMatchCreate(bool success, string extendedInfo, MatchInfo matchInfo)
    {
        base.OnMatchCreate(success, extendedInfo, matchInfo);
        matchId = (System.UInt64)matchInfo.networkId;
        if (success)
        {
            Netstate = 3;
        }
        else
        {
            Netstate = 0;
        }
        if (MatchCreated != null)
        {
            MatchCreated(success, matchInfo);
            MatchCreated = null;
        }
        if (MatchCreate != null)
        {
            MatchCreate(success, matchInfo);
        }
        SvatAndDirect = 1;
    }
    public override void OnDestroyMatch(bool success, string extendedInfo)
    {
        //3
        //DropConnection
        base.OnDestroyMatch(success, extendedInfo);
        if (connectServer==2)
        {
            StopMatchMaker();
            StopHost();
        }
    }
    private void OnInternetMatchCreate(bool success, string extendedInfo, MatchInfo matchInfo)
    {
        if (success)
        {
            Debug.Log("Create match succeeded");
            MatchInfo hostInfo = matchInfo;
            NetworkServer.Listen(hostInfo, 9000);
            singleton.StartHost(hostInfo);
        }
        else
        {
            Debug.LogError("Create match failed");
        }
    }
    public override void OnMatchJoined(bool success, string extendedInfo, MatchInfo matchInfo)
    {
        base.OnMatchJoined(success, extendedInfo, matchInfo);
        if (success)
        {
            Netstate = 3;
        }
        else
        {
            Netstate = 1;
        }
        if (boolMatchJoined != null)
        {
            boolMatchJoined(success, matchInfo);
            boolMatchJoined = null;
        }
        if (matchJoined != null)
        {
            matchJoined(success, matchInfo);
        }
    }
    public override void OnMatchList(bool success, string extendedInfo, List<MatchInfoSnapshot> matchList)
    {
        base.OnMatchList(success, extendedInfo, matchList);
        if (!success)
        {
            print("failed" + extendedInfo);
        }
        else
        {
            if (matchList.Count > 0)
            {

            }
        }
    }
    public void StartLobbyHostforBack()
    {
        if (SvatAndDirect==1)
        {
            matchMaker.DestroyMatch((NetworkID)matchId, 0, OnDestroyMatch);
            connectServer = 2;
        }
        else
        {
            StopHost();
        }
    }
    public void StartLobbyClientforBack()
    {
        StopClient();
        if(SvatAndDirect==1)
        {
            StopMatchMaker();
        }
    }
    public override void OnStartHost()
    {
        //2
        base.OnStartHost();
        panel = 2;
        back = true;

    }
    public override void OnLobbyStartServer()
    {
        if (lobbySlots.Length == 0)
            lobbySlots = new NetworkLobbyPlayer[maxPlayers];

        base.OnLobbyStartServer();
    }
    public override void OnLobbyClientEnter()
    {
        base.OnLobbyClientEnter();
    }
    public override void OnLobbyClientExit()
    {
        base.OnLobbyClientExit();
    }
    public override void OnLobbyClientSceneChanged(NetworkConnection conn)
    {
        string scene = SceneManager.GetActiveScene().name;
        if(scene==playScene)
        {
            Netstate = 4;
        }
        if (Netstate == 4)//inGame
        {
            if (SvatAndDirect == 1)
            {
                if (conn.playerControllers[0].unetView.isServer)
                {
                    print("inGame");
                    back = false;
                    disconnectedHost = true;
                    panel = 2;
                }
                else
                {
                    back = true;
                    panel = 3;
                }
            }
            else
            {
                if (conn.playerControllers[0].unetView.isClient)
                {
                    back = true;
                    panel = 2;

                }
                else
                {
                    back = true;
                    panel = 3;
                }
            }
        }
        base.OnLobbyClientSceneChanged(conn);
    }
    public override void OnClientDisconnect(NetworkConnection conn)
    {
        //base.OnClientDisconnect(conn);
        back = true;
        panel = 3;
        LobbyClientDisconnect();
        GetComponent<Host>().LostHostOnClient(conn);
    }
    public override void OnStartClient(NetworkClient client)
    {
        base.OnStartClient(client);
        GetComponent<Host>().Initialize(client, matchInfo);
    }
    public override void OnStopHost()
    {
        base.OnStopHost();
    }
    public override void OnLobbyServerDisconnect(NetworkConnection conn)
    {
        //base.OnLobbyServerDisconnect(conn);
        for(int i=0;i<lobbySlots.Length;++i)
        {
            LobbyPlayer p = (LobbyPlayer)lobbySlots[i];
            if(p != null)
            {
                p.RpcRemove();
            }
        }
        GetComponent<Host>().SendPeerInfo();
        LobbyServerDisconnect();
    }
    public override void OnServerDisconnect(NetworkConnection conn)
    {
        //base.OnServerDisconnect(conn);
        LobbyServerDisconnect();
        GetComponent<Host>().SendPeerInfo();
    }
    public override GameObject OnLobbyServerCreateLobbyPlayer(NetworkConnection conn, short playerControllerId)
    {
        GameObject obj = Instantiate(lobbyPlayerPrefab.gameObject) as GameObject;
        LobbyPlayer newPlayer = obj.GetComponent<LobbyPlayer>();
        for(int i = 0; i<lobbySlots.Length;++i)
        {
            LobbyPlayer p = (LobbyPlayer)lobbySlots[i];
            if(p!=null)
            {
                p.isLocalLobbyPlayer();
            }
        }
        return obj;
    }
    public override void OnServerAddPlayer(NetworkConnection conn, short playerControllerId, NetworkReader extraMessageReader)
    {
        lobbyServerAddPlayer(conn, playerControllerId, extraMessageReader);
        //base.OnServerAddPlayer(conn, playerControllerId, extraMessageReader);
    }
    float countTimer = 0;
    int sec;
    bool doneAddingPlayer = false;
    short controllerId;
    void Update()
    {
        if (countTimer == 0)
        {
            return;
        }
        if (Time.time > countTimer)
        {
            countTimer = 0;
            for (int i = 0; i < lobbySlots.Length; ++i)
            {
                if (lobbySlots[i] != null)
                {
                    (lobbySlots[i] as LobbyPlayer).RpcPlayerCount(0);
                }
            }
            //ServerChangeScene(playScene);
        }
        else
        {
            for(int i = 0;i<lobbySlots.Length;++i)
            {
                if(lobbySlots[i]!=null)
                {
                    (lobbySlots[i] as LobbyPlayer).RpcPlayerCount(countTimer);
                }
            }
        }
    }
    void Awake()
    {
        if (lobbythis != null)
        {
            Destroy(gameObject);
        }
        else
        {
            lobbythis = this;
        }
    }
    public override void OnLobbyServerPlayersReady()
    {
        countTimer = Time.time + 5;
        bool ready = true;
        for(int i = 0; i<lobbySlots.Length;++i)
        {
            if (lobbySlots[i] != null)
                ready &= lobbySlots[i].readyToBegin;
        }
        base.OnLobbyServerPlayersReady();
    }
    public float prematchCountdown = 5.0f;
    public void MatchInfos()
    {
        var i = this;
    }
    public override void OnLobbyServerPlayerRemoved(NetworkConnection conn, short playerControllerId)
    {
        base.OnLobbyServerPlayerRemoved(conn, playerControllerId);
    }
    public override void OnLobbyClientAddPlayerFailed()
    {
        base.OnLobbyClientAddPlayerFailed();
    }
    public override bool OnLobbyServerSceneLoadedForPlayer(GameObject lobbyPlayer, GameObject gamePlayer)
    {
        //if (lbHook)lbHook.OnLobbyServerSceneLoadedForPlayer(this, lobbyPlayer, gamePlayer);
        base.OnLobbyServerSceneLoadedForPlayer(lobbyPlayer, gamePlayer);
        return true;
    }
    public override void OnServerConnect(NetworkConnection conn)
    {
        //1
        if (numPlayers >= maxPlayers)// || Netstate != 3)
        {
            conn.Disconnect();
        }
        else
        {
            if (Netstate == 3)
            {

            }
        }
        base.OnServerConnect(conn);
        GetComponent<Host>().SendPeerInfo();
        LobbyServerConnect();
    }
    public override void OnClientConnect(NetworkConnection conn)
    {
        base.OnClientConnect(conn);
        if (!NetworkServer.active)
        {
            panel = 3;
            back = true;
        }
        if(clientConn==null)
        {
            clientConn = conn;
        }
        LobbyClientConnect();
    }
}
