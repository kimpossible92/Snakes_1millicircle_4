using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MyProjectile : MonoBehaviour
{
    [SerializeField] TextMesh textmesh;
    List<GameObject> wList2 = new List<GameObject>();
    void Start()
    {
        _Spawn();
    }
    [SerializeField] int level;
    [SerializeField]List<Vector3> positions = new List<Vector3>();
    [SerializeField] BlockEnemy[] blockEnemies;
    [SerializeField] private GameObject SetObject;
    [SerializeField] private List<GameObject> GetBlocks = new List<GameObject>();
    [SerializeField]
    private GameObject[] unitPrefabs;
    Vector3[] _positionsText = { new Vector3(12, -4, 51), new Vector3(15, 9, 51), new Vector3(25, 0, 51) };
    Vector3 rotateStart, rotateKoef;
    Vector3 positionStart, positionKoef;
    Vector3 myPoint = Vector3.right;
    [SerializeField] Button buttonPlay;
    public void DestroyObject()
    {
        Destroy(SetObject.gameObject);
    }
    public void RemoveBlocks(GameObject go)
    {
        GetBlocks.Remove(go);
    }
    public void SetBlocks(GameObject go)
    {
        GetBlocks.Add(go);
    }
    public bool FindBlocks(GameObject go)
    {
        return GetBlocks.Contains(go);
    }
    public void SetSpawn()
    {
        _Spawn();

    }
    List<GameObject> enemies=new List<GameObject>();
    void _Spawn()
    {
        if (SetObject != null) { Destroy(SetObject);SetObject = null; }
        List<Vector3> vector3s = new List<Vector3>();
        GameObject worm = MonoBehaviour.Instantiate(this.unitPrefabs[Random.Range(0, unitPrefabs.Length)], new Vector3(1, 0, 0), Quaternion.identity) as GameObject;
        SetObject = worm;

        foreach(var enem in enemies)
        {
            if (enem != null) { Destroy(enem); }
        }
        enemies.Clear();
        for (int i = 0; i < 3; i++)
        {
            var p1 = positions[Random.Range(0, positions.Count)];
            while (!vector3s.Contains(p1))
            {
                p1 = positions[Random.Range(0, positions.Count)]; vector3s.Add(p1);
                GameObject enemy1 = Instantiate(blockEnemies[Random.Range(0, blockEnemies.Length)].gameObject, p1, Quaternion.identity);
                enemies.Add(enemy1);
            }
            _TextMesh(_positionsText[i]);
        }
    }
    protected float speed1 = 0;
    float Xmove;
    float Ymove;
    [SerializeField] private Image[] GetImage = new Image[4];
    public void setX(float x1) { Xmove = x1; }
    public void setY(float y1) { Ymove = y1; }
    // Update is called once per frame
    void Update()
    {
        if (SetObject != null) { if (SetObject.GetComponent<MyObect>().GetFull()) { buttonPlay.gameObject.SetActive(true); } else { buttonPlay.gameObject.SetActive(false); myPoint = SetObject.transform.position; } }
        else { buttonPlay.gameObject.SetActive(true); }
        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            positionStart = Input.mousePosition;
        }
        else { /*positionStart = null;*/ }
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            rotateStart = Input.mousePosition;
        }
        if (Input.GetKey(KeyCode.Mouse0))
        {
            rotateKoef = rotateStart - Input.mousePosition;
            //SetObject.GetComponent<MyObect>().Rotations(new Vector3(rotateKoef.x, rotateKoef.y, 0));
        }

        Xmove = Input.GetAxis("Horizontal");
        Ymove = Input.GetAxis("Vertical");
        if (SetObject != null)
        {
           
            if (GetImage[0].GetComponent<JoyButton>().Pressed) { SetObject.GetComponent<MyObect>().setHead(3); }
            if (GetImage[1].GetComponent<JoyButton>().Pressed) { SetObject.GetComponent<MyObect>().setHead(4); }
            if (GetImage[2].GetComponent<JoyButton>().Pressed) { SetObject.GetComponent<MyObect>().setHead(1); }
            if (GetImage[3].GetComponent<JoyButton>().Pressed) { SetObject.GetComponent<MyObect>().setHead(2); }
            if (Input.GetKey(KeyCode.Space))
            {
                SetObject.GetComponent<MyObect>().setHead(0);
            }
            if (Xmove > 0.0001f)
            {
                SetObject.GetComponent<MyObect>().setHead(1);
                SetObject.GetComponent<MyObect>().SetSpeed(1.0f);
            }
            else if (Xmove < -0.0001f)
            {
                SetObject.GetComponent<MyObect>().setHead(2);
                SetObject.GetComponent<MyObect>().SetSpeed(1.0f);
            }
            else { SetObject.GetComponent<MyObect>().SetSpeed(0.3f); }
            if (Ymove > 0.0001f)
            {
                SetObject.GetComponent<MyObect>().setHead(3);
                SetObject.GetComponent<MyObect>().SetSpeed(1.2f);
            }
            else if (Ymove < -0.0001f)
            {
                SetObject.GetComponent<MyObect>().setHead(4);
                SetObject.GetComponent<MyObect>().SetSpeed(1.2f);
            }
            else { SetObject.GetComponent<MyObect>().SetSpeed(0.3f); }
        }
        if (Input.GetKey(KeyCode.Mouse1))
        {
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit raycastHit;
            speed1 += 0.2f;
            if (Physics.Raycast(ray, out raycastHit))
            {
                //myPoint = new Vector3(raycastHit.point.x, raycastHit.point.y, 0);
            }
            if (speed1 > 10) { speed1 = 0.3f; }
        }
        else { 
            speed1 = 0.3f;
            
        }
        if (SetObject != null)
        {
            SetObject.transform.position = (Vector3.MoveTowards(
            SetObject.transform.position,
            myPoint, Time.deltaTime*speed1));

            float[] _points = { myPoint.x, myPoint.y, myPoint.z };
            for (int i = 0; i < 3; i++)
            {
                if (wList2[i] != null) wList2[i].GetComponent<TextMesh>().text = _points[i].ToString();
            }
        }
    }
    void _TextMesh(Vector3 _position)
    {
        GameObject worm = MonoBehaviour.Instantiate(this.textmesh.gameObject, _position, Quaternion.identity) as GameObject;
        wList2.Add(worm);
        new WaitForSeconds(0.001f);
    }
}
