using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mass : MonoBehaviour
{
    public int MaxX;
    public int MaxY;
    [SerializeField]
    GameObject[] blockpref;
    public Vector2 vector2position;
    public Block[] blocksp;
    [SerializeField] GameObject LevelParent;
    float height;
    public void Massiv2()
    {
        foreach(var bl in blocksp)
        {
            Destroy(bl.gameObject);
        }
    }
    public void MassivField()
    {
        blocksp = new Block[MaxX * MaxY];
        for (int row = 0; row < MaxY; row++)
        {
            for (int col = 0; col < MaxX; col++)
            {
                Createblock(col, row);
            }
        }
    }
    public void Createblock(int i, int j)
    {
        var block = blockpref[Random.Range(0, blockpref.Length)];
        GameObject vblck = ((GameObject)Instantiate(block, vector2position + new Vector2(i * 1f, j * 1f), block.transform.rotation));
        vblck.transform.SetParent(LevelParent.transform);
        blocksp[j * MaxX + i] = vblck.GetComponent<Block>();
        vblck.GetComponent<Block>().row = j;
        vblck.GetComponent<Block>().col = i;
    }
    // Start is called before the first frame update
    void Start()
    {
        MassivField();
    }
    // Update is called once per frame
    void Update()
    {

    }
}
