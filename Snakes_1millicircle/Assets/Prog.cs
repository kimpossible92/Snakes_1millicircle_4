using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
[System.Serializable]
public struct SpawnGroup
{
    public int indexway;
    public List<Transform> waypoints;
    public float elap;
    public SpawnGroup(int index,List<Transform> ways)
    {
        this.waypoints = ways;
        this.indexway = index;
        this.elap = 0f;
    }
    public override string ToString()
    {
        return base.ToString();
    }
}
public class Prog : NetworkBehaviour
{
    public GameObject bullet;
    public NetworkConnection Author;
    public const float GetTimeStamp = 6.0f;//() { return (float)6.0f; }
    [SerializeField]
    public List<SpawnGroup> spawnGroup;
    [SerializeField]
    public List<SpawnGroup> removeSpawnGroup;
    public List<GameObject> wirms;
    public List<GameObject> wList = new List<GameObject>();
    [SyncVar]
    protected int m_PlayerId;
    [SerializeField]
    public SelectSpawn selectSpawn;
    NetworkClient myClient;
    [SerializeField]
    public GameObject unitPrefab;
    bool unspawn = false;
    [SyncVar]
    public int sec = 0;
    [SyncVar]
    private Vector3 mDirection;
    [SyncVar]
    uint moveCount = 0;
    public Vector3 newdir;
    [SerializeField]
    public List<Transform> waypoints = new List<Transform>();
    [SerializeField]
    public List<Transform> rubipos;
    [SyncVar]
    public int addmove = 0;
    [SyncVar]
    public int newHead = 0;
    float dirspeed = 0.3f;
    [SerializeField]
    NetworkConnection wirmConnection;
    public GameObject MySpawnManager;
    private NetworkWriter m_LocalTransformWrite;
    public GameObject SelectSpawnprefab;
    private Vector3 m_PreviosPosition;
    private float m_LastCSendTime;
    [SerializeField]
    public GameObject Ruby;
    Vector3 myrubipos;
    [Client]
    private void SendWorm()
    {
        if (newHead == 0)
        { return; }
        this.m_LocalTransformWrite.StartMessage((short)6);
        this.m_LocalTransformWrite.Write(this.netId);
        if (GetComponent<NetworkTransform>().transformSyncMode == NetworkTransform.TransformSyncMode.SyncTransform)
        {
            this.SerializeModeWorm(this.m_LocalTransformWrite);
        }
        this.m_PreviosPosition = this.transform.position;//this.mDirection;
        this.m_LocalTransformWrite.FinishMessage();
        ClientScene.readyConnection.SendWriter(this.m_LocalTransformWrite, this.GetNetworkChannel());
    }
    private void SerializeModeWorm(NetworkWriter writer)
    {
        writer.Write(this.transform.position);
        this.m_PreviosPosition = this.transform.position;//this.mDirection;
    }
    [Server]
    public void SetPlayerId(int id)
    {
        m_PlayerId = id;
    }
    public override void OnStartLocalPlayer()
    {
        base.OnStartLocalPlayer();
        wirmConnection = this.connectionToClient;
        NetworkIdentity netId = this.GetComponent<NetworkIdentity>();
        if (this.isServer)
        {
            this.wirmConnection = netId.connectionToClient;
        }
        else
        {
            this.wirmConnection = netId.connectionToServer;
        }
        int[] randX;
        randX = new int[29];
        for (int i = 0; i < 29; i++)
        {
            randX[i] = i - 14;
        }
        int[] randY;
        randY = new int[15];
        for (int j = 0; j < 15; j++)
        {
            randY[j] = j - 7;
        }
        int randomx = randX[Random.Range(0, randX.Length)];
        int randomy = randY[Random.Range(0, randY.Length)];
        transform.position = new Vector3(randomx, randomy, 0);
        //CmdMoveWorm();
    }
    public override void OnStartServer()
    {
        base.OnStartServer();
    }
    public int severcount = 0;
    string firstmessage;
    string allmessage;
    public GameObject[] findall;
    public GameObject[] findone;
    NetworkTransform Networktransform;
    // Use this for initialization
    public void Start()
    {
        newHead = 0;
        disconnectplayer = false;
        findone = GameObject.FindGameObjectsWithTag("w");
        serveractive = true;
        sec = 1;
        StartCoroutine(FDay());
        spawnGroup = new List<SpawnGroup>();
        Networktransform = GetComponent<NetworkTransform>();
    }

    public Vector3 ves;
    [Command]
    void CmdGoWorm(Vector3 v, Vector3 Direction)
    {
        RpcGoWorm(Direction, v);
    }
    [ClientRpc]
    void RpcGoWorm(Vector3 Direction, Vector3 v)
    {
        Direction = mDirection;
        newdir = Direction;
        transform.Translate(Direction);
        v = transform.position;
        ves = v;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            CmdShoting();
        }
        if (transform.position.x < -25)
        {
            transform.position = new Vector3(25, transform.position.y, transform.position.z);
        }
        if (transform.position.x > 25)
        {
            transform.position = new Vector3(-25, transform.position.y, transform.position.z);
        }
        if (transform.position.y < -25)
        {
            transform.position = new Vector3(transform.position.x, 25, transform.position.z);
        }
        if (transform.position.y > 25)
        {
            transform.position = new Vector3(transform.position.x, -25, transform.position.z);
        }
        if (!isLocalPlayer)
            return;
        var Xmove = Input.GetAxis("Horizontal");
        var Ymove = Input.GetAxis("Vertical");
        if (Input.GetKey(KeyCode.Space))
        {
            newHead = 0;
        }
        if (Xmove > 0.0001f)
        {
            newHead = 1;
        }
        else if (Xmove < -0.0001f)
        {
            newHead = 2;
        }
        if (Ymove > 0.0001f)
        {
            newHead = 3;
        }
        else if (Ymove < -0.0001f)
        {
            newHead = 4;
        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            newHead = 1;
        }
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            newHead = 2;
        }
        if (Input.GetKey(KeyCode.UpArrow))
        {
            newHead = 3;
        }
        if (Input.GetKey(KeyCode.DownArrow))
        {
            newHead = 4;
        }
        if (newHead == 1)
        {
            mDirection = Vector2.right * dirspeed;
        }
        else if (newHead == 2)
        {
            mDirection = -Vector2.right * dirspeed;
        }
        else if (newHead == 3)
        {
            mDirection = Vector2.up * dirspeed;
        }
        else if (newHead == 4)
        {
            mDirection = -Vector2.up * dirspeed;
        }
        else if (newHead == 0)
        {
            mDirection = Vector2.zero;
        }
        if (!this.localPlayerAuthority || this.hasAuthority || (NetworkServer.active || (double)Time.time - (double)this.m_LastCSendTime <= (double)this.GetNetworkSendInterval())) return;
        this.SendWorm();
        this.m_LastCSendTime = Time.time;

    }
    GameObject serverGame;
    public bool disconnectplayer = false;
    bool findserver = false;
    [Command]
    void CmdTranslate(Vector3 v)//, Vector3 Direction)
    {
        //transform.Translate(mDirection);
        new WaitForSeconds(0.005f);
        RpcTranslate(v);
    }
    [ClientRpc]
    void RpcTranslate(Vector3 v)//, Vector3 Direction)
    {
        //Direction = mDirection;
        //newdir = Direction;
        if (newHead == 0)
            return;
        v = transform.position;
        ves = v;
        //transform.Translate(Direction);
        if (waypoints.Count > 0)
        {
            waypoints.Last().position = v;
            waypoints.Insert(0, waypoints.Last());
            waypoints.RemoveAt(waypoints.Count - 1);
        }
        new WaitForSeconds(0.005f);
    }
    [Command]
    void CmdShoting()
    {
        if (!isClient)
        {
            isShoot();
        }
        RpcShoting();
    }
    [ClientRpc]
    void RpcShoting()
    {
        isShoot();
    }
    void isShoot()
    {
        if (newHead != 0)
        {
            GameObject b = (GameObject)MonoBehaviour.Instantiate(this.bullet, transform.position, Quaternion.identity);
            b.GetComponent<Fire>().localPlayer = GetComponent<NetworkIdentity>();
            NetworkServer.SpawnWithClientAuthority(b, this.connectionToClient);
            b.GetComponent<Fire>().newHeads = newHead;
            new WaitForSeconds(0.01f);
        }
    }
    [Command]
    void CmdSpawn()
    {
        GameObject worm = MonoBehaviour.Instantiate(this.unitPrefab) as GameObject;
        NetworkIdentity wormId = this.GetComponent<NetworkIdentity>();
        NetworkServer.SpawnWithClientAuthority(worm, this.connectionToClient);
        wList.Add(worm);
        new WaitForSeconds(0.001f);
        RpcSpawn(worm);
    }
    [ClientRpc]
    void RpcSpawn(GameObject w)
    {
        waypoints.Insert(0, w.transform);
    }
    public void OnConnected(NetworkMessage netMsg)
    {
        NetworkMessage msg = netMsg;
    }
    public IEnumerator FDay()
    {
        if (disconnectplayer == false)
        {
            while (sec != 0)
            {

                yield return new WaitForSeconds(0.03f);
                Networktransform.SetDirtyBit(1);
                Vector3 v = transform.position;
                if (addmove == 1)
                {
                    if (disconnectplayer == false)
                    {
                        CmdMoveWorm();
                        addmove = 2;
                    }
                }
                transform.Translate(mDirection);
                
                CmdTranslate(v);
                
                sec = sec + 1;
                sec++;
            }
        }
        else
        {
            yield return new WaitForSeconds(0.03f);
        }
    }
    public IEnumerator afterplayerdisconect()
    {
        int newcount = 1;
        while (newcount != 0)
        {
            yield return new WaitForSeconds(0.03f);
            Vector3 v = transform.position;
            if (addmove == 1)
            {
                CmdSpawn();
                addmove = 2;
            }
            transform.Translate(mDirection);
            CmdTranslate(v);
            newcount = newcount + 1;
            newcount++;
        }
    }
    public bool serveractive = true;
    [Command]
    void CmdMoveWorm()
    {
        GameObject worm = (GameObject)MonoBehaviour.Instantiate(unitPrefab, transform.position, Quaternion.identity);
        NetworkIdentity wormId = worm.GetComponent<NetworkIdentity>();
        NetworkServer.SpawnWithClientAuthority(worm, this.connectionToClient);
        wList.Add(worm);
        new WaitForSeconds(0.01f);
        RpcMove(worm);
    }
    [ClientRpc]
    void RpcMove(GameObject wormpos)
    {
        waypoints.Insert(0, wormpos.transform);
    }
    int num = 0;
    Transform wormposition;
    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.tag == "corm")
        {
            addmove = 1;
        }
    }
    public PoolSpawn poolSpawn;
    public GameObject spawnPrefab;
    public override bool OnSerialize(NetworkWriter writer, bool initialState)
    {
        if(initialState)
        {
            writer.WritePackedUInt32((uint)this.sec);
            writer.WritePackedUInt32((uint)this.newHead);
            writer.WritePackedUInt32((uint)this.addmove);
            foreach(Transform ways in waypoints)
            {
                writer.WritePackedUInt32((uint)ways.position.x);
                writer.WritePackedUInt32((uint)ways.position.y);
                writer.WritePackedUInt32((uint)ways.position.z);
            }
            return true;
        }
        return base.OnSerialize(writer, initialState);
    }
    public override void OnDeserialize(NetworkReader reader, bool initialState)
    {
        if(initialState)
        {
            this.sec = (int)reader.ReadPackedUInt32();
            this.newHead = (int)reader.ReadPackedUInt32();
            this.addmove = (int)reader.ReadPackedUInt32();
            return;
        }
        int num = (int)reader.ReadPackedUInt32();
        if ((num & 1)!= 0)
        {
            this.sec = (int)reader.ReadPackedUInt32();
        }
        if ((num & 2) != 0)
        {
            this.newHead = (int)reader.ReadPackedUInt32();
        }
        if ((num & 3) != 0)
        {
           this.addmove = (int)reader.ReadPackedUInt32();
        }
    }
    public override void OnNetworkDestroy()
    {
        base.OnNetworkDestroy();
        foreach(var w in wList)
        {
            NetworkServer.UnSpawn(w);
            Destroy(w);
        }
        findall = GameObject.FindGameObjectsWithTag("w");
    }
    public override void OnStartAuthority()
    {
        base.OnStartAuthority();
        CmdMoveWorm();
    }
    public override void OnStartClient()
    {
        base.OnStartClient();
        foreach (var w in wList)
        {
            NetworkServer.UnSpawn(w);
            Destroy(w);
        }
        findone = GameObject.FindGameObjectsWithTag("w");
    }
    void addmyposition(GameObject myGo)
    {
        myGo.transform.position = gameObject.transform.position;
    }
    void Awake()
    {
        this.m_PreviosPosition = this.mDirection;
    }
}
