using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class Wirm : NetworkBehaviour
{
    public NetworkConnection Auth;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
    public static void Copyng(Wirm origin, Wirm copy)
    {
        copy.transform.position = origin.transform.position;
    }
}
