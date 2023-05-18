using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class Fire : NetworkBehaviour
{
    public NetworkIdentity localPlayer;
    public int newHeads;
    private Vector3 firedirection;
    // Use this for initialization
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (newHeads == 1)
        {
            firedirection = Vector2.right * 5f;
        }
        if (newHeads == 2)
        {
            firedirection = -Vector2.right * 5f;
        }
        if (newHeads == 3)
        {
            firedirection = Vector2.up * 5f;
        }
        if (newHeads == 4)
        {
            firedirection = -Vector2.up * 5f;
        }
        transform.Translate(firedirection);
    }
    void OnCollisionEnter(Collision coll)
    {
        if (coll.gameObject.tag == "w" && coll.gameObject.GetComponent<NetworkIdentity>().isLocalPlayer != localPlayer.isLocalPlayer)
        {
            Destroy(coll.gameObject);
            if (gameObject.tag == "bullet")
            {
                Destroy(gameObject);
            }
        }

    }
}
