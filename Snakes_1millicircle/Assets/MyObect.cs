using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MyObect : MonoBehaviour
{
    [SerializeField]private List<GameObject> GetBlocks = new List<GameObject>();
    public List<Transform> waypoints = new List<Transform>();
    public int sec = 0;
    [SerializeField]
    private GameObject unitPrefab;
    private int newHead = 0;
    public Vector3 mDirection;
    int addmove = 0; 
    float dirspeed = 0.3f;
    public void SetSpeed(float dspeed)
    {
        dirspeed = dspeed;
    }
    private void Start()
    {
        addmove = 1;
        sec = 1;
        StartCoroutine(FDay());
    }
    public void setHead(int _head)
    {
        newHead = _head;
    }
    private void Update()
    {
        if (transform.position.x < -17)
        {
            transform.position = new Vector3(9, transform.position.y, transform.position.z);
        }
        if (transform.position.x > 10)
        {
            transform.position = new Vector3(-16, transform.position.y, transform.position.z);
        }
        if (transform.position.y < -6)
        {
            transform.position = new Vector3(transform.position.x, 10, transform.position.z);
        }
        if (transform.position.y > 10)
        {
            transform.position = new Vector3(transform.position.x, -6, transform.position.z);
        }

        if (newHead == 1)
        {
            mDirection = Vector2.right * dirspeed;
        }
        else if (newHead == 2)
        {
            mDirection = -Vector2.right * dirspeed;
        }
        else if (newHead == 3)
        {
            mDirection = Vector2.up * dirspeed;
        }
        else if (newHead == 4)
        {
            mDirection = -Vector2.up * dirspeed;
        }
        else if (newHead == 0)
        {
            mDirection = Vector2.zero;
        }
    }
    int wCounter = 0;
    void CmdMoveWorm()
    {
        GameObject worm = (GameObject)MonoBehaviour.Instantiate(unitPrefab, transform.position, Quaternion.identity);
        //worm.GetComponent<MeshRenderer>().enabled = false;
        if (wCounter == 0)
        {
            worm.GetComponent<_Objects>().SetLast(true);
        }
        wCounter += 1;
        wList.Add(worm);
        
        RpcMove(worm);
    }
    void RpcMove(GameObject wormpos)
    {
        waypoints.Insert(0, wormpos.transform);
    }
    #region cmdspawn1
    void CmdSpawn()
    {

        GameObject worm = MonoBehaviour.Instantiate(this.unitPrefab) as GameObject;
        //NetworkIdentity wormId = this.GetComponent<NetworkIdentity>();
        //NetworkServer.SpawnWithClientAuthority(worm, this.connectionToClient);
        wList.Add(worm);
        new WaitForSeconds(0.001f);
        RpcSpawn(worm);

    }
    void RpcSpawn(GameObject w)
    {
        waypoints.Insert(0, w.transform);
    }
    #endregion
    public List<GameObject> wList = new List<GameObject>();
    public IEnumerator FDay()
    {
        while (sec != 0)
        {

           
            Vector3 v = transform.position;
            if (addmove == 1)
            {
                CmdMoveWorm();
                addmove = 2;
                //   }
            }
            transform.Translate(mDirection);
            sec = sec + 1;
            sec++;
            yield return new WaitForSeconds(0.1f);
            CmdTranslate(v);
        }
    }
    void CmdTranslate(Vector3 v)//, Vector3 Direction)
    {
        //transform.Translate(mDirection);
        RpcTranslate(v);
        new WaitForSeconds(0.005f);
    }
    void RpcTranslate(Vector3 v)//, Vector3 Direction)
    {
        //Direction = mDirection;
        //newdir = Direction;
        v = transform.position;
        //ves = v;
        //transform.Translate(Direction);
        if (waypoints.Count > 0)
        {
            waypoints.Last().position = v;
            waypoints.Insert(0, waypoints.Last());
            waypoints.RemoveAt(waypoints.Count - 1);
        }
        new WaitForSeconds(0.005f);
    }
    public void Rotations(Vector3 v)
    {
        this.transform.rotation = Quaternion.Euler(new Vector3(v.x, v.y, 0));
    }
    public void TransformPostion(Vector3 point)
    {
        transform.position = point;
    }
    int score = 0;[SerializeField] float ypos; 
    public Texture2D lifeIconTexture;
    private void OnGUI()
    {
        Rect lifeIconRect = new Rect(10, 150, 32, 32);
        GUI.DrawTexture(lifeIconRect, lifeIconTexture);

        GUIStyle style = new GUIStyle();
        style.fontSize = 30;
        style.fontStyle = FontStyle.Bold;
        style.normal.textColor = Color.yellow;
        Rect labelRect = new Rect(lifeIconRect.xMax + 10, lifeIconRect.y, 60, 32);
        GUI.Label(labelRect, score.ToString(), style);
    }
    public bool GetFull()
    {
        if (GetBlocks.Count() > 240)
        {
            score++;
            return true;
        }
        return false;
    }
    private void OnDestroy()
    {
        foreach(var wl in wList)
        {
            Destroy(wl);
        }
        wList.Clear();
        wList = new List<GameObject>();
    }
    void OnCollisionEnter(Collision coll)
    {
        if (coll.gameObject.GetComponent<Block>()!=null)
        {
            if (!GetBlocks.Contains(coll.gameObject)) { GetBlocks.Add(coll.gameObject); }
            if (wCounter < 8) { addmove = 1; }
            if (coll.gameObject.GetComponent<Block>() != null)
            {
                coll.gameObject.GetComponent<Block>().SetCube2();
            }
        }
    }
}
