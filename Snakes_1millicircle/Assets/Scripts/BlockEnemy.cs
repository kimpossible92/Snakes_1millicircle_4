using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockEnemy : MonoBehaviour
{
    [HideInInspector]public Vector3 myPoint;
    // Start is called before the first frame update
    void Start()
    {
        myPoint = transform.position;  
    }
    [SerializeField]protected float speed1 = 1;
    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.MoveTowards(
           transform.position,
            myPoint, Time.deltaTime * speed1);

        if (transform.position == myPoint)
        {
            myPoint = new Vector3(myPoint.x, -myPoint.y, myPoint.z);
        }
    }
}
