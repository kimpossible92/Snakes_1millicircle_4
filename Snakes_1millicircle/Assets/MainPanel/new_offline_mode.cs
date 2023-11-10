using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class new_offline_mode : MonoBehaviour
{
    [HideInInspector] public bool offline = false;
    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(this.gameObject);
        offline = true;
    }

    // Update is called once per frame
    void Update()
    {

    }
}