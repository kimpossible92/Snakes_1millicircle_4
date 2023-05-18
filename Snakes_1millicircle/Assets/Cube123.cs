using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cube123 : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag=="corm")
        {
            Destroy(gameObject);
        }
        if (collision.gameObject.tag == "wirm"||
        collision.gameObject.tag == "w"
            )
        {
            FindObjectOfType<MyProjectile>().DestroyObject();
        }
    }
}
