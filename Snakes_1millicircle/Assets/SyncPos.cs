using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class SyncPos : NetworkBehaviour
{
    [SyncVar]
    private Vector3 syncPos;
    Transform myTransform;
    float lerpRate = 15.0f;
	// Use this for initialization
    void Start () {
        InvokeRepeating("TestTimeOut", 2, 0.2f);
    }
	void TestTimeOut()
    {
        TransmitPos();
        LerpPosition();
    }
    void LerpPosition()
    {
        if(isLocalPlayer)
        {
            myTransform.position = Vector3.Lerp(myTransform.position, syncPos, lerpRate * Time.deltaTime);
        }
    }
    [Command]
    void CmdPosToServer(Vector3 pos)
    {
        syncPos = pos;
    }
    [ClientCallback]
    void TransmitPos()
    {
        CmdPosToServer(myTransform.position);
    }
	// Update is called once per frame
	void Update () {
	
	}
}
