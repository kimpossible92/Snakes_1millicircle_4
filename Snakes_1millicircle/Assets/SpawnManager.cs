using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using System.Collections.Generic;
public class SpawnManager : NetworkBehaviour
{
    public GameObject Ruby;
    public GameObject Corms;
    GameObject rubiGo;
    GameObject rubiGo1;
    GameObject rubiGo2;
    GameObject rubiGo3;
    GameObject cormGo;
    [Space]
    protected bool spawningRuby;
    void Start()
    {
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
        Vector3 rp = new Vector3(randX[Random.Range(0, randX.Length)], randY[Random.Range(0, randY.Length)], 0);
        CmdStart(rp);
    }
    void RespawnLevel()
    {
        
    }
    void Update()
    {
        if(!GameObject.FindGameObjectWithTag("corm"))
        {
            
        }
    }
    [Command]
    void CmdStart(Vector3 rp)
    {
        rubiGo = Instantiate(Ruby, rp, Quaternion.identity) as GameObject;
        NetworkServer.Spawn(rubiGo);
        LocalPos rubipos = rubiGo.GetComponent<LocalPos>();
        rubipos.InstantPosition(rp);
        RpcStart();
    }
    [ClientRpc]
    void RpcStart()
    {

    }
    public override void OnStartClient()
    {
        base.OnStartClient();
        ClientScene.RegisterPrefab(Ruby);
    }
}
