using UnityEngine;
using UnityEngine.Networking;
[RequireComponent(typeof(Transform))]
[RequireComponent(typeof(NetworkTransform))]
public class LocalPos : NetworkBehaviour {
    private NetworkTransform Transforms;
    [SyncVar]
    protected Vector3 instanceposition;
    [Server]
    public void InstantPosition(Vector3 position)
    {
        instanceposition = position;
    }
    // Use this for initialization
    void Start()
    {
        Transforms = GetComponent<NetworkTransform>();
        transform.position = instanceposition;
    }
	
	// Update is called once per frame
	void Update ()
    {

	}
    void OnCollisionExit(Collision col)
    {
        Transforms.SetDirtyBit(1);
        if (col.gameObject.tag == "w")
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
            transform.position = rp;
        }
    }
}
