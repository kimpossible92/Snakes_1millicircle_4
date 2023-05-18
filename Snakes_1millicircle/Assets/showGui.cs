using UnityEngine;
using System.Collections;

public class showGui : MonoBehaviour {
    public float countTime = 0;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
    void OnGUI()
    {
        string strcount = GUI.TextField(new Rect(20, 40, 40, 20), string.Format("{0}", countTime));
    }
}
