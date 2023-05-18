using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using System.Collections;

public class SyncTime2 : NetworkBehaviour
{
    public float serverTimeDif;
    public float clientTimeDif;
    GetNetTime GetNTime;
    GameObject time;
    Text TimeLog;
    IEnumerator DisplayTime(float TimeDiff)
    {
        for(;;)
        {
            TimeLog.text = "" + ((float)System.DateTime.UtcNow.TimeOfDay.TotalSeconds + TimeDiff);
            yield return new WaitForSeconds(0.01f);
        }
    }
	// Use this for initialization
	void Start () {
        GetNTime = GetComponent<GetNetTime>();
        time = GameObject.Find("time");
        TimeLog = time.GetComponent<Text>();
        if (isServer)
        {
            serverTimeDif = (float)System.DateTime.UtcNow.TimeOfDay.TotalSeconds - (float)GetNetTime.NetworkTime().TimeOfDay.TotalSeconds;
            StartCoroutine("DisplayTime", serverTimeDif);
        }
        else
        {
            clientTimeDif = (float)System.DateTime.UtcNow.TimeOfDay.TotalSeconds - (float)GetNetTime.NetworkTime().TimeOfDay.TotalSeconds;
            StartCoroutine("DisplayTime", clientTimeDif);
        }
	}
	// Update is called once per frame
	void Update () {
	
	}
}
