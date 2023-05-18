using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rubin : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }
    void OnCollisionExit(Collision col)
    {
        if (col.gameObject.tag == "w")
        {
            CmdCollision();
        }
    }
    void CmdCollision()
    {
        int[] randX;
        randX = new int[15];
        for (int i = 0; i < 15; i++)
        {
            randX[i] = i - 7;
        }
        int[] randY;
        randY = new int[15];
        for (int j = 0; j < 15; j++)
        {
            randY[j] = j - 7;
        }
        Vector3 rp = new Vector3(randX[Random.Range(0, randX.Length)], randY[Random.Range(0, randY.Length)], 0);
        RpcCollision(rp);
    }
    void RpcCollision(Vector3 rp)
    {
        transform.position = rp;
    }
}
