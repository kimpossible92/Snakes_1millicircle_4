#if ENABLE_UNET
using UnityEngine.UI;
using UnityEngine.Networking.Match;

namespace UnityEngine.Networking
{
    [AddComponentMenu("Network/NetworkManagerHUD")]
    [RequireComponent(typeof(NetworkManager))]
    [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
    public class NetworkManagerHUD_Simple : MonoBehaviour
    {
        [SerializeField]
        protected InputField GameName;
        [SerializeField]
        protected Text Status;
        public MyLobby manager;
        [SerializeField]
        public bool showGUI = true;
        [SerializeField]
        public int offsetX;
        [SerializeField]
        public int offsetY;
        private int switchpanel = 0;
        private int switchClient = 0;
        bool showServer = false;
        // Use this for initialization
        void Awake()
        {
            manager = GetComponent<MyLobby>();
        }
        void OnGUI()
        {
            if (!showGUI)
                return;
            int xpos = 10 + offsetX;
            int ypos = 40 + offsetY;
            int spacing = 24;
            if(manager.back==true)
            {
                if (GUI.Button(new Rect(5, 20, 80, 20), "<<back"))
                {
                    if (manager.panel == 2)
                    {
                        manager.StartLobbyHostforBack();
                        switchpanel = 1;
                    }
                    if (manager.panel == 3)
                    {
                        manager.StartLobbyClientforBack();
                        switchpanel = 1;
                    }
                    if (manager.panel == 4)
                    {
                        manager.StopHost();
                        switchpanel = 1;
                    }
                }
            }

            if (!NetworkClient.active && !NetworkServer.active && manager.matchMaker == null)
            {

            }
            else //if(NetworkClient.active && NetworkServer.active && manager.matchMaker != null)
            {
                if (NetworkServer.active)
                {
                    GUI.Label(new Rect(xpos, ypos, 300, 20), "Server: port=" + manager.networkPort);
                    ypos += spacing;
                }
                if (NetworkClient.active)
                {
                    GUI.Label(new Rect(xpos, ypos, 300, 20), "Client Address:=" + manager.networkAddress + " port=" + manager.networkPort);
                    ypos += spacing;
                }
            }
            if (NetworkClient.active && !ClientScene.ready)
            {
                if (switchpanel == 4)
                {

                    if (GUI.Button(new Rect(xpos, ypos, 200, 20), "Client Ready"))
                    {
                        ClientScene.Ready(manager.client.connection);
                        if (ClientScene.localPlayers.Count == 0)
                        {
                            ClientScene.AddPlayer(0);
                        }
                    }
                    ypos += spacing;
                }
            }
            if (!NetworkServer.active && !NetworkClient.active)
            {
                ypos += 10;
                if (manager.matchMaker == null)
                {
                    if (GUI.Button(new Rect(xpos, ypos, 200, 20), "Enable Match Maker"))
                    {
                        manager.StartMatchMaker();
                    }
                    ypos += spacing;
                }
                else
                {
                    if (manager.matchInfo == null)
                    {
                        if (manager.matches == null)
                        {
                            if (GUI.Button(new Rect(xpos, ypos, 200, 20), "Create Internet Match"))
                            {
                                if (string.IsNullOrEmpty(manager.matchName))
                                {
                                    Debug.Log("return");
                                    return;
                                }
                                if(manager.matchName=="default")
                                {
                                    Debug.Log("return");
                                    return;
                                }
                                CreateInternetMatch();
                                manager.matchName = string.Empty;
                            }
                            ypos += spacing;
                            GUI.Label(new Rect(xpos, ypos, 100, 20), "Room:");
                            manager.matchName = GUI.TextField(new Rect(xpos + 100, ypos, 100, 20), manager.matchName);
                            ypos += spacing;
                            ypos += 10;
                            if (GUI.Button(new Rect(xpos, ypos, 200, 20), "Find Internet Match"))
                            {
                                manager.matchMaker.ListMatches(0, 20, "", true, 0, 0, manager.OnMatchList);
                                switchpanel = 0;
                            }
                            ypos += spacing;
                        }
                        else
                        {
                            foreach (var match in manager.matches)
                            {
                                if (GUI.Button(new Rect(xpos, ypos, 200, 20), "Join match:" + match.name))
                                {
                                    manager.matchName = match.name;
                                    Debug.Log(match.name);
                                    manager.matchSize = (uint)match.currentSize;
                                    manager.matchMaker.JoinMatch(match.networkId, "", "", "", 0, 0, manager.OnMatchJoined);
                                    manager.back = true;
                                }
                                ypos += spacing;
                            }
                        }
                    }
                    if (GUI.Button(new Rect(xpos, ypos, 200, 20), "Change MM server"))
                    {
                        showServer = !showServer;
                    }
                    if (showServer)
                    {
                        ypos += spacing;
                        if (GUI.Button(new Rect(xpos, ypos, 100, 20), "Local"))
                        {
                            manager.SetMatchHost("localhost", 1337, false);
                            showServer = false;
                        }
                        ypos += spacing;
                        if (GUI.Button(new Rect(xpos, ypos, 100, 20), "Internet"))
                        {
                            manager.SetMatchHost("mm.unet.unity3d.com", 443, true);
                            showServer = false;
                        }
                        ypos += spacing;
                        if (GUI.Button(new Rect(xpos, ypos, 100, 20), "Staging"))
                        {
                            manager.SetMatchHost("staging-mm.unet.unity3d.com", 443, true);
                            showServer = false;
                        }

                    }
                    ypos += spacing;
                    GUI.Label(new Rect(xpos, ypos, 300, 20), "MM Uri:" + manager.matchMaker.baseUri);
                    if (GUI.Button(new Rect(xpos, ypos, 200, 20), "Disable Match Maker"))
                    {
                        manager.StopMatchMaker();
                    }
                    ypos += spacing;

                }
            }
            if (switchpanel == 1)
            {
                manager.back = false;
                manager.SvatAndDirect = 0;
                ypos += 10;
                if (manager.matchMaker == null)
                {
                    if (GUI.Button(new Rect(xpos, ypos, 200, 20), "Enable Match Maker"))
                    {
                        manager.StartMatchMaker();
                        switchpanel = 2;
                    }
                    ypos += spacing;
                }
            }
            else if (switchpanel == 2)
            {
                manager.back = false;
                manager.SvatAndDirect = 0;
                if (GUI.Button(new Rect(xpos, ypos, 200, 20), "CreateInternetMatch"))
                {
                    if (string.IsNullOrEmpty(manager.matchName))
                    {
                        Debug.Log("return");
                        return;
                    }
                    if (manager.matchName == "default")
                    {
                        Debug.Log("return");
                        return;
                    }
                    CreateInternetMatch();
                    manager.matchName = string.Empty;
                    switchpanel = 0;
                    manager.back = true;
                }
                ypos += spacing;
                GUI.Label(new Rect(xpos, ypos, 100, 20), "Room:");
                manager.matchName = GUI.TextField(new Rect(xpos + 100, ypos, 100, 20), manager.matchName);
                ypos += spacing;
                ypos += 10;
                if (GUI.Button(new Rect(xpos, ypos, 200, 20), "Find Internet Match"))
                {
                    manager.matchMaker.ListMatches(0, 20, "", true, 0, 0, manager.OnMatchList);
                    switchpanel = 3;
                }
                ypos += spacing;
            }
            else if(switchpanel==3)
            {
                if (manager.matchInfo == null)
                {
                    if (manager.matches == null)
                    {
                    }
                    else
                    {
                        foreach (var match in manager.matches)
                        {
                            if (GUI.Button(new Rect(xpos, ypos, 200, 20), "Join match:" + match.name))
                            {
                                manager.matchName = match.name;
                                Debug.Log(match.name);
                                manager.matchSize = (uint)match.currentSize;
                                manager.matchMaker.JoinMatch(match.networkId, "", "", "", 0, 0, manager.OnMatchJoined);
                                manager.back = true;
                            }
                            ypos += spacing;
                        }
                    }
                }
                
            }
        }
        private void CreateInternetMatch()
        {
            manager.CreateInternetMatch(manager.matchName, (success, matchInfo) =>
            {
                if (!success)
                {
                    print("no internet");
                }
                else
                {
                }
            });
            manager.SvatAndDirect = 1;
        }
    }
};
# endif