using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Networking.NetworkSystem;
using UnityEngine.SceneManagement;

public class NetSide : NetworkManager {
    static public NetSide networkthis;
    [SerializeField]
    protected MySide myside;
    public bool StartLan = false;
    public string PlayScene;
    public delegate void NetworkServerDisconnectHandler();
    public delegate void NetworkClientDisconnectHandler();
    public delegate void NetworkClientConnectHandler();
    public delegate void NetworkServerConnectHandler();
    public delegate void NetworkServerAddPlayerHandler(NetworkConnection conn, short playerControllerId, NetworkReader extraMessageReader);
    private NetworkServerDisconnectHandler ServerDisconnect = new NetworkServerDisconnectHandler(OnNetworkServerDisconnectInternal);
    private NetworkClientDisconnectHandler ClientDisconnect = new NetworkClientDisconnectHandler(OnNetworkClientDisconnectInternal);
    private NetworkServerConnectHandler ServerConnect = new NetworkServerConnectHandler(NetworkServerConnectInternal);
    private NetworkClientConnectHandler ClientConnected = new NetworkClientConnectHandler(NetworkClientConnectInternal);
    private NetworkServerAddPlayerHandler ServerAddPlayer = new NetworkServerAddPlayerHandler(ServerAddPlayerInternal);
    private static void OnNetworkServerDisconnectInternal()
    {

    }
    private static void OnNetworkClientDisconnectInternal()
    {

    }
    private static void NetworkServerConnectInternal()
    {

    }
    private static void NetworkClientConnectInternal()
    {

    }
    public void AddClientDisconnect(NetworkClientDisconnectHandler handler)
    {
        ClientDisconnect += handler;
    }
    public void RemoveClientHandler(NetworkClientDisconnectHandler handler)
    {
        ClientDisconnect -= handler;
    }
    public void AddServerDisconnect(NetworkServerDisconnectHandler handler)
    {
        ServerDisconnect += handler;
    }
    public void RemoveServerDisconnectHandler(NetworkServerDisconnectHandler handler)
    {
        ServerDisconnect -= handler;
    }
    public void AddClientConnect(NetworkClientConnectHandler handler)
    {
        ClientConnected += handler;
    }
    public void RemoveClientConnect(NetworkClientConnectHandler handler)
    {
        ClientConnected -= handler;
    }
    public void AddServerConnect(NetworkServerConnectHandler handler)
    {
        ServerConnect += handler;
    }
    public void RemoveServerConnect(NetworkServerConnectHandler handler)
    {
        ServerConnect -= handler;
    }
    public static void ServerAddPlayerInternal(NetworkConnection conn,short playerControllerId, NetworkReader extraMessageReader)
    {

    }
    void Awake()
    {
        
        if(networkthis!=null)
        {
        }
        else
        {
            networkthis = this;
            connections = new List<MySide>();
            GetComponent<Host>().connections = connections;
            PlayScene = onlineScene;
        }
    }
    public List<MySide> connections
    {
        get;
        set;
    }
    public override void OnServerAddPlayer(NetworkConnection conn, short playerControllerId)
    {
        StartLan = true;
        MySide newplayer = Instantiate<MySide>(myside);
        DontDestroyOnLoad(newplayer);
        NetworkServer.AddPlayerForConnection(conn, newplayer.gameObject, playerControllerId);
        if (conn.playerControllers[0].unetView.isServer)
        {
            inhost = true;
        }
        else
        {
            inhost = false;
        }
    }
    public override void OnStartHost()
    {
        base.OnStartHost();
    }
    public override void OnStopHost()
    {
        base.OnStopHost();
        //GetComponent<Host>().LostHostOnHost();
        //ClientScene.SetReconnectId(client.connection.connectionId, client.peers);
    }
    //OnLobbyClientSceneChanged
    public override void OnClientDisconnect(NetworkConnection conn)
    {
        ClientDisconnect();
        //migrationManager.SendPeerInfo();
        GetComponent<Host>().LostHostOnClient(conn);
        //base.OnClientDisconnect(conn);
    }
    public bool inhost = false;
    public override void OnClientSceneChanged(NetworkConnection conn)
    {
        base.OnClientSceneChanged(conn);
    }
    public override void OnStartClient(NetworkClient client)
    {
        base.OnStartClient(client);
        //migrationManager.Initialize(client, matchInfo);
        GetComponent<Host>().Initialize(client, matchInfo);
    }
    public override void OnClientConnect(NetworkConnection conn)
    {
        base.OnClientConnect(conn);
        ClientConnected();
    }
    public override void OnServerConnect(NetworkConnection conn)
    {
        base.OnServerConnect(conn);
        GetComponent<Host>().SendPeerInfo();
        ServerConnect();
    }
    public override void OnServerDisconnect(NetworkConnection conn)
    {
        Destroy(conn.playerControllers[0].gameObject);
        ServerDisconnect();
        GetComponent<Host>().SendPeerInfo();
        NetworkServer.Destroy(conn.playerControllers[0].gameObject);
    }
    public override void OnServerReady(NetworkConnection conn)
    {
        NetworkServer.SetClientReady(conn);
        //base.OnServerReady(conn);
    }
    public override void OnServerRemovePlayer(NetworkConnection conn, PlayerController player)
    {
        base.OnServerRemovePlayer(conn, player);
    }
    public override void OnServerAddPlayer(NetworkConnection conn, short playerControllerId, NetworkReader extraMessageReader)
    {
        base.OnServerAddPlayer(conn, playerControllerId, extraMessageReader);
        ServerAddPlayer(conn, playerControllerId, extraMessageReader);
    }
    public int hostId;
    void Start () {
        networkthis = this;
    }
    float countTimer = 0;
    void Update()
    {
    }
}
