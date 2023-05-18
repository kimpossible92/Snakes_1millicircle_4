using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using System.Collections.Generic;

public class SelectSpawn : NetworkBehaviour {
    public Prog isLocalpl
    {
        get;
        set;
    }
    public override void OnStartLocalPlayer()
    {
        base.OnStartLocalPlayer();
        if (isLocalPlayer)
        {
            this.GetComponent<Prog>().enabled = true;
            isLocalpl = this.GetComponent<Prog>();
        }
    }
}
