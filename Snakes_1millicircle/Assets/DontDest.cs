using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontDest : MonoBehaviour
{
    Camera my_cam;
    // Start is called before the first frame update
    void Start()
    {
        //DontDestroyOnLoad(this.gameObject);
        my_cam = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        my_cam.gameObject.SetActive(true);
    }
}
