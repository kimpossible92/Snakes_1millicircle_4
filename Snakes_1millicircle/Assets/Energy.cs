using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Energy : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
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
        transform.position = new Vector3(randX[Random.Range(0, randX.Length)], randY[Random.Range(0, randY.Length)], 0);
    }
    void OnCollisionEnter(Collision coll)
    {
        if (coll.gameObject.tag == "w")
        {
            Destroy(gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
