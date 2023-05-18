using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
using UnityEngine.Networking.NetworkSystem;
using UnityEngine.Networking.Types;
public class MySide : NetworkBehaviour {
    public class Message : MessageBase
    {

    }
    [SerializeField]
    protected GameObject instancePlayer;

    public float serverTimeDif;
    public float clientTimeDif;
    GetNetTime GetNTime;
    GameObject time;
    Text TimeLog;
    [SyncVar]
    public int playersId;
    static List<MySide> playerSide = new List<MySide>();
    public NetSide mynetworkmanager;
    public MyLobby mylobbymanager;
    public Material[] colorset;
    private Host Migration;
    [SerializeField]
    public List<PeerInfoMessage> PeerList;
    [SerializeField]
    private PeerInfoMessage peermessage;
    [SerializeField]
    private PeerInfoMessage peerClient;
    bool isHost = false;
    public int hostId;

    bool AddMoveIfNew(Prog inProg)
    {
        float timestamp = Prog.GetTimeStamp;
        if(timestamp>12.0f)
        {
            return true;
        }
        return false;
    }
    void Start()
    {
    }
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Space) && NetworkManager.singleton.client != null)
        {
            try
            {
                System.Reflection.FieldInfo info = (typeof(NetworkClient)).GetField("m_AsyncConnect", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            }
            catch (System.Exception e)
            {
                Debug.LogException(e);
            }
        }
    }
    void OnDestroy()
    {
        ((NetSide)NetworkManager.singleton).RemoveClientHandler(MyReconn);
        ((NetSide)NetworkManager.singleton).RemoveClientConnect(Connec);
        GetComponent<Prog>().serveractive = false;
    }
    public void Client()
    {
        ((NetSide)NetworkManager.singleton).AddClientDisconnect(MyReconn);
        ((NetSide)NetworkManager.singleton).AddClientConnect(Connec);
    }
    private void Connec()
    {
        StringMessage message = new StringMessage(NetSide.singleton.matchInfo.address);
        ClientScene.AddPlayer(NetSide.singleton.client.connection, 0, message);
    }
    private void MyReconn()
    {
        StartCoroutine(Reconn());
    }
    private IEnumerator Reconn()
    {
        byte error;
        NetworkID network = NetworkID.Invalid;
        SourceID source = SourceID.Invalid;
        NodeID node = NodeID.Invalid;
        NetworkTransport.RemoveHost(NetSide.singleton.client.connection.hostId);
        //Закрыть открытый сокет, закрыть все соединение, принадлежащее этому сокету
        NetSide.singleton.client.Shutdown();
        NetSide.singleton.client = null;
        NetworkTransport.ConnectAsNetworkHost(NetworkManager.singleton.client.connection.hostId, "", 7777, network, source, node, out error);
        yield return new WaitForSeconds(1.0f);
        NetworkTransport.Shutdown();
        NetSide.singleton.StartClient();
    }
    private string playerNameValue()
    {
        return string.Format("{0}", playersId);
    }

    public int playerId
    {
        get { return playersId; }
    }
    [Server]
    public void ViewId(int playerId)
    {
        this.playersId = playerId;
    }
    public MySide clients
    {
        get;
        private set;
    }
    public LobbyPlayer lobby
    {
        get;
        set;
    }
    [Client]
    public override void OnStartLocalPlayer()
    {
        base.OnStartLocalPlayer();
        clients = this;
        SyncPlayer();
    }
    public override void OnNetworkDestroy()
    {
        base.OnNetworkDestroy();
    }
    [Client]
    public void SyncPlayer()
    {

    }
    void OnCollisionEnter(Collision coll)
    {

    }
}
