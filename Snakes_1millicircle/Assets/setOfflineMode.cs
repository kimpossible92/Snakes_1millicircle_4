using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class setOfflineMode : MonoBehaviour
{
    [SerializeField] private new_offline_mode new_Offline_Mode1;
    // Start is called before the first frame update
    void Awake()
    {
        Instantiate(new_Offline_Mode1.gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
