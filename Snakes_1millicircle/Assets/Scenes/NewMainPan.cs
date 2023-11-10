using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NewMainPan : MonoBehaviour
{
    private Dictionary<string, RoomInfo> cachedRoomList;
    private Dictionary<string, GameObject> roomListEntries; 
    public InputField PlayerNameInput;
    // Start is called before the first frame update
    private void Awake()
    {
        PhotonNetwork.AutomaticallySyncScene = true;

        cachedRoomList = new Dictionary<string, RoomInfo>();
        roomListEntries = new Dictionary<string, GameObject>();

        //PlayerNameInput.text = "Player2 " + Random.Range(1000, 10000);
    }
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
