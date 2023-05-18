using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour
{
    //public HitCandy candy;
    public int row;
    public int col;
    public int types;
    public List<GameObject> block = new List<GameObject>();
    private int ccc;
    public bool emptyes = false;
    [SerializeField] Sprite GetSprite1;
    [HideInInspector] public int modelvlsquare;
    public int addScore;
    public GameObject prefab;
    [SerializeField] Vector2 position;
    public GameObject Cube;
    [SerializeField] GameObject Cube2;
    // Use this for initialization
    void Start()
    {
       
    }
    public void SetCube2()
    {
        Cube2.gameObject.SetActive(true);
    }
    private void Awake()
    {
        Cube.gameObject.SetActive(false);
        Cube2.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void DestroyBlocks()
    {
        prefab.SetActive(false);
        Invoke("Spawn", 10.0f);
    }
    public void Spawn()
    {
        //Instantiate(prefab, position, prefab.transform.rotation);
        prefab.SetActive(true);
    }
    public void Visible()
    {
        Cube.gameObject.SetActive(true);
        Cube2.gameObject.SetActive(false);
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "w")
        {
            //Cube.gameObject.SetActive(true);
        }
    }
}
public class SquareBlocks
{
    public int blck;
    public void Changeblck(int bl) { blck = bl; }
    public int block() { return blck; }
    public int obstacle;
}