using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class PoolSpawn : NetworkBehaviour
{
    public int objPoolSize = 5;
    public GameObject unitPrefab;
    public GameObject wormPrefab;
    public List<Transform> waypoints;
    public Vector3 newdirect;
    public GameObject[] unitPool;
    public List<GameObject> wormGroup;
    public NetworkConnection Author;
    public NetworkHash128 assetId { get; set; }
    public delegate GameObject SpawnDelegate(Vector3 position);
    public delegate void UnSpawnDelegate(GameObject spawned);
    public override void OnStartLocalPlayer()
    {
        base.OnStartLocalPlayer();
    }

    public void TranslatePools(Vector3 direction)
    {
        newdirect = direction;
        transform.Translate(newdirect);
        print(newdirect);
    }
    public GameObject AddPools(Vector3 v)
    {
        assetId = unitPrefab.GetComponent<NetworkIdentity>().assetId;
        GameObject worm = (GameObject)MonoBehaviour.Instantiate(unitPrefab, v, Quaternion.identity);
        waypoints.Insert(0, worm.transform);
        ClientScene.RegisterSpawnHandler(assetId, SpawnObject, UnSpawnObject);
        return worm;
    }
    public void Addpoints(Vector3 v)
    {
        if (waypoints.Count > 0)
        {
            waypoints.Last().position = v;
            waypoints.Insert(0, waypoints.Last());
            waypoints.RemoveAt(waypoints.Count - 1);
        }
    }
    void Start()
    {
        /*assetId = unitPrefab.GetComponent<NetworkIdentity>().assetId;
        unitPool = new GameObject[objPoolSize];
        for (int i = 0; i < objPoolSize; ++i)
        {
            unitPool[i] = (GameObject)Instantiate(unitPrefab, Vector3.zero, Quaternion.identity);
            unitPool[i].name = "Corm" + i;
            unitPool[i].SetActive(false);
        }
        ClientScene.RegisterSpawnHandler(assetId, SpawnObject, UnSpawnObject);*/
    }
    void Update()
    {
        
    }
    public GameObject GetFromPool(Vector3 position)
    {
        foreach (var obj in unitPool)
        {
            if (obj.activeInHierarchy)
            {
                Debug.Log("Activating object" + obj.name + "at" + position);
                obj.transform.position = position;
                obj.SetActive(true);
                return obj;
            }
        }
        return null;
    }
    public GameObject SpawnObject(Vector3 position, NetworkHash128 assetId)
    {
        return GetFromPool(position);
    }
    public void UnSpawnObject(GameObject spawned)
    {
        spawned.SetActive(false);
    }
}
