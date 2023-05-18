using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Networking.NetworkSystem;
using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class Host : NetworkMigrationManager {
    [SerializeField]
    public MyLobby manager;
    [SerializeField]
    public NetSide netside;
    Action Disconn;
    GameObject[] objmiration;
    bool lobbyreconnect = false;
    SceneChangeOption SChange;
    public PeerInfoMessage HostInfo;
    public PeerInfoMessage newhostInfo = new PeerInfoMessage();
    public NetworkConnection reconnectID;
    private bool shutdown = false;
    bool lobbyrepick = false;
    /*public int oldServerConnectionId
    {
        get
        {
            return this.olderServerConnId;
        }
    }
    public bool disconnectedFromHost
    {
        get
        {
            return this._DisconnectedFromHost;
        }
    }
    public bool hostWasShutdown
    {
        get
        {
            return this.shutdown;
        }
    }
    public bool waitingReconnectToNewHost
    {
        get
        {
            return this.waitReconnectToNewHost;
        }
    }
    public bool waitingToBecomeNewHost
    {
        get
        {
            return this.waitToBecomeNewHost;
        }
    }*/
    public List<MySide> connections
    {
        get;
        set;
    }
    void Start()
    {
        manager = GetComponent<MyLobby>();
        if (manager != null)
        {
            lobbyreconnect = true;
        }
        else //if (manager == null)
        {
            lobbyreconnect = false;
            netside = GetComponent<NetSide>();
        }
        stopFDay = false;
    }
    bool stopFDay = false;
    void Update()
    {
        objmiration = GameObject.FindGameObjectsWithTag("w");
        if (stopFDay == true)
        {
        }
    }
    public void ConnectPlayerServer(MySide myplayer)
    {
        connections.Add(myplayer);
        if (NetworkServer.active)
        {
            for (int i = 0; i < connections.Count; i++)
            {
                connections[i].ViewId(i);
            }
        }
        string sceneName = SceneManager.GetActiveScene().name;
        if (sceneName == netside.onlineScene)
        {
        }
    }
    private bool waitReconnectToNewHost = false;
    private bool _DisconnectedFromHost = false;
    private bool waitToBecomeNewHost = false;
    private int olderServerConnId = -1;
    private bool leavegame = false;
    private string newHostAddresss;
    private PeerInfoMessage[] _Peers;
    /*public void Reset(int reconnectId)
    {
        this.olderServerConnId = -1;
        this.waitToBecomeNewHost = false;
        this.waitReconnectToNewHost = false;
        this._DisconnectedFromHost = false;
        this.shutdown = false;
        ClientScene.SetReconnectId(reconnectId, _Peers);
        if (!((System.Object)NetworkManager.singleton != (System.Object)null))
            return;
        NetworkManager.singleton.SetupMigrationManager(this);
    }*/

    void Awake()
    {
        manager = GetComponent<MyLobby>();
    }
    void OnGUI()
    {
        if (!showGUI)
            return;
        SChange = SceneChangeOption.StayInOnlineScene;
        if (lobbyreconnect == true)
        {
            if (manager.disconnectedHost == true)
            {
                if (lobbyrepick == false)
                {
                    if (GUI.Button(new Rect(5, 20, 80, 20), "<<-back"))
                    {
                        lobbyrepick = true;
                    }
                }
                if (lobbyrepick == true)
                {
                    if (GUI.Button(new Rect(5, 20, 80, 20), "<<-back"))
                    {
                        lobbyrepick = false;
                    }
                }
                if(NetworkClient.active)
                {
                    if (lobbyrepick == true)
                    {
                        LobbyrepicknewHost();
                    }
                    if (this.waitToBecomeNewHost == true)
                    {
                        LobbyYouArenewHost();
                    }
                }
                if (NetworkServer.active)
                {
                    if(lobbyrepick)
                    {
                        repicknewHost();
                    }
                    if (this.waitReconnectToNewHost == true)
                    {
                        LobbyReconnectAsClient();
                    }
                }
            }
        }
        if (NetworkServer.active || NetworkClient.active)
        {
            if (lobbyreconnect == false)
            {
                if (repick==false)
                {
                    if (GUI.Button(new Rect(5, 20, 80, 20), "<<-back"))
                    {
                        repick = true;
                    }
                }
                else
                {
                    if (GUI.Button(new Rect(5, 20, 80, 20), "<<-back"))
                    {
                        repick = false;
                    }

                }
                if (NetworkServer.active)
                {
                    if (repick == true)
                    {
                        repicknewHost();
                    }
                    if (this.waitReconnectToNewHost==true)
                    {
                        print("waitReconnectToNewHost=true");
                        ReconnectAsClient();
                    }
                }
                if (NetworkClient.active)
                {
                    if (repick == true)
                    {
                        picknewHost();
                    }
                    if (this.waitToBecomeNewHost == true)
                    {
                        GUI.Label(new Rect(720, 20, 140, 20), "You are new host");
                        YouArenewHost();
                    }
                }
            }
        }
    }
    bool repick = false;
    private void LobbyrepicknewHost()
    {
        bool youAreNewhost;
        if (GUI.Button(new Rect(120, 20, 140, 20), "Pick new Host") && this.FindNewHost(out this.newhostInfo, out youAreNewhost))
        {
            this.newHostAddress = this.newhostInfo.address;
            if(youAreNewhost)
            {
                this.waitToBecomeNewHost = true;
            }
            else
            {
                this.waitReconnectToNewHost = true;
            }
        }
    }
    private void repicknewHost()
    {
        bool youAreNewhost;
        if (GUI.Button(new Rect(120, 20, 140, 20), "Pick new Host") && this.FindNewHost(out this.newhostInfo, out youAreNewhost))
        {
            this.newHostAddress = this.newhostInfo.address;
            if (youAreNewhost)
            {

            }
            else
            {
                this.waitReconnectToNewHost = true;
            }
        }
    }
    private void picknewHost()
    {
        bool youAreNewhost;
        if (GUI.Button(new Rect(120, 20, 140, 20), "Pick new Host") && this.FindNewHost(out this.newhostInfo, out youAreNewhost))
        {
            this.newHostAddress = this.newhostInfo.address;
            if (youAreNewhost)
            {
                this.waitToBecomeNewHost = true;
            }
            else
            {
                this.waitReconnectToNewHost = true;
            }
        }
    }
    private void ReconnectAsClient()
    {
        if (GUI.Button(new Rect(280, 20, 120, 20), "Reconnect As Client"))
        {
            this.Reset(0);
            if ((System.Object)NetSide.singleton != (System.Object)null)
            {
                NetSide.singleton.networkAddress = GUI.TextField(new Rect(280, 50, 120, 20), NetSide.singleton.networkAddress);
                NetSide.singleton.StartClient();
            }
        }
    }
    private void LobbyReconnectAsClient()
    {
        if (GUI.Button(new Rect(280, 20, 120, 20), "Reconnect As Client"))
        {
            this.Reset(0);
            if ((System.Object)MyLobby.singleton != (System.Object)null)
            {
                MyLobby.singleton.networkAddress = GUI.TextField(new Rect(280, 50, 120, 20), MyLobby.singleton.networkAddress);
                MyLobby.singleton.StartClient();
            }
        }
    }
    private void newAddress()
    {
        this.Reset(0);
        if ((System.Object)NetSide.singleton != (System.Object)null)
        {
            NetSide.singleton.networkAddress = GUI.TextField(new Rect(280, 50, 120, 20), NetSide.singleton.networkAddress);
            NetSide.singleton.StartClient();
        }
    }
    private void YouArenewHost()
    {
        if (GUI.Button(new Rect(280, 20, 120, 20), "Start as host"))
        {
            this.BecomeNewHost(NetSide.singleton.networkPort);
            stopFDay = true;
            foreach (GameObject players in objmiration)
            {
                players.GetComponent<Prog>().sec = 0;
                players.GetComponent<Prog>().disconnectplayer = true;
                StartCoroutine(players.GetComponent<Prog>().FDay());
                StartCoroutine(players.GetComponent<Prog>().afterplayerdisconect());
            }
            this.SendPeerInfo();
        }
    }
    private void LobbyYouArenewHost()
    {
        if (GUI.Button(new Rect(280, 20, 120, 20), "Start as host"))
        {
            if((System.Object)MyLobby.singleton !=(System.Object)null)
            {
                this.BecomeNewHost(MyLobby.singleton.networkPort);
            }
            foreach (GameObject players in objmiration)
            {
                players.GetComponent<Prog>().sec = 0;
                players.GetComponent<Prog>().disconnectplayer = true;
                StartCoroutine(players.GetComponent<Prog>().FDay());
                StartCoroutine(players.GetComponent<Prog>().afterplayerdisconect());
            }
        }
    }
    public void LeaveGame()
    {

        if (GUI.Button(new Rect(750, 20, 80, 20), "leave game"))
        {
            if ((System.Object)MyLobby.singleton != (System.Object)null)
            {
                manager.SetupMigrationManager((Host)null);
                manager.StopHost();
            }
            this.Reset(-1);
        }
    }
    protected override void OnPeersUpdated(PeerListMessage peers)
    {
        base.OnPeersUpdated(peers);
    }
    protected override void OnClientDisconnectedFromHost(NetworkConnection conn, out NetworkMigrationManager.SceneChangeOption sceneChange)
    {
        base.OnClientDisconnectedFromHost(conn, out sceneChange);

        /*PeerInfoMessage hostInfo = HostInfo;
        bool reconnectNewHost = false;
        FindNewHost(out hostInfo, out reconnectNewHost);//error NetworkMigrationManager FindLowestHost нет сверстниковUnityEngine.Networking.NetworkMigrationManager: FindNewHost(PeerInfoMessage &, Boolean &)Хост: ClientDisconnectedFromHost()(по Assets / Host.cs: 50) Ведущий: OnGUI()(по Assets / Host.cs: 33)
        if (reconnectNewHost == true)
        {
            BecomeNewHost(7777);
        }
        else
        {
            newHostAddress = hostInfo.address;//error NullReferenceException: ссылка на объект не установлена в экземпляр объекта Host.ClientDisconnectedFromHost()(по Assets / Host.cs: 58) Host.OnGUI()(по Assets / Host.cs: 33)
            Reset(oldServerConnectionId);
            UnityEngine.Networking.NetworkManager.singleton.networkAddress = newHostAddress;
            UnityEngine.Networking.NetworkManager.singleton.client.ReconnectToNewHost(newHostAddress, UnityEngine.Networking.NetworkManager.singleton.networkPort);
        }*/
    }
    public void ClientHost()
    {
        SceneChangeOption option = SceneChangeOption.StayInOnlineScene;
        OnClientDisconnectedFromHost(client.connection, out option);
    }
    protected override void OnServerReconnectPlayer(NetworkConnection newConnection, GameObject oldPlayer, int oldConnectionId, short playerControllerId)
    {
        base.OnServerReconnectPlayer(newConnection, oldPlayer, oldConnectionId, playerControllerId);
    }
    protected override void OnAuthorityUpdated(GameObject go, int connectionId, bool authorityState)
    {
        base.OnAuthorityUpdated(go, connectionId, authorityState);
        if(authorityState)
        {
        }
    }
    protected override void OnServerReconnectPlayer(NetworkConnection newConnection, GameObject oldPlayer, int oldConnectionId, short playerControllerId, NetworkReader extraMessageReader)
    {
        base.OnServerReconnectPlayer(newConnection, oldPlayer, oldConnectionId, playerControllerId, extraMessageReader);
    }
    protected override void OnServerHostShutdown()
    {
        base.OnServerHostShutdown();
    }
    protected override void OnServerReconnectObject(NetworkConnection newConnection, GameObject oldObject, int oldConnectionId)
    {
        base.OnServerReconnectObject(newConnection, oldObject, oldConnectionId);
    }
}
