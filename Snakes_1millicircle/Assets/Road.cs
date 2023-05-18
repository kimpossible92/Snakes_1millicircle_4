using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum roadScale
{
    Easy=1,
    Medium = 2,
    Hard = 3,
    BIG =7
}
public class Road : MonoBehaviour
{
    [SerializeField] Material[] mts;
    public int delcount=0;
    //[SerializeField] bool ui=false;
    //[SerializeField] UnityEngine.UI.Button continueButton;
    //[SerializeField] UnityEngine.UI.Button newgame;
    public Material GetMaterial() { return mts[pcolor]; }
    private int pcolor;
    [SerializeField]
    private float posX;
    [SerializeField]
    private float posY;
    [SerializeField] private offcorm[] Offcorms;
    [SerializeField] offcorm newcorm;
    [SerializeField]
    private Vector2 _spawnPeriodRange;
    [SerializeField]
    private Vector2 _spawnDelayRange;
    //[SerializeField] GameObject leftpart;
    //[SerializeField] GameObject rightpart;
    [SerializeField]
    LayerMask MoveOnSightLayer;
    public bool getpause()
    {
        return gamePaused;
    }
    bool gamePaused;
    int score2 = 0;
    List<offcorm> GetOffcorms = new List<offcorm>();
    List<offcorm> GetEnems = new List<offcorm>();
    public int countplus = 2;
    int lives=8;int scores2 = 0;
    //[SerializeField]UnityEngine.UI.Text GetTextdead;
    //[SerializeField]UnityEngine.UI.Text getTextscores;
    int[] vscales = { (int)roadScale.Easy, (int)roadScale.Medium, (int)roadScale.Hard, (int)roadScale.BIG};
    bool ngame = false;
    public int mycolor() { return pcolor; }
    public void removeCorm(offcorm offcorm)
    {
        GetOffcorms.Remove(offcorm);
    }
    public void addScore(int sc)
    {
        FindObjectOfType<UIPlay>().addScore(sc);
        //scores2 += sc;
    }
    public void minuslive()
    {
        lives--;
        print(lives);
    }
    public void removeEnem(offcorm offcorm)
    {
        GetEnems.Remove(offcorm);
    }
    public void Paused()
    {
        gamePaused = true;
        //continueButton.gameObject.SetActive(ngame);
        //newgame.gameObject.SetActive(true);
        Time.timeScale = 0f;
    }
    public void Play()
    {
        Time.timeScale = 1f;
        //continueButton.gameObject.SetActive(false); newgame.gameObject.SetActive(false);
        gamePaused = false;
    }
    public void newgamestart()
    {
        Time.timeScale = 1f;
        lives = 8;
        scores2 = 0;
        ngame = true;
        //continueButton.gameObject.SetActive(false); newgame.gameObject.SetActive(false);
        gamePaused = false;
    }
    public void StopSpawn()
    {
        StopCoroutine(GetEnumeratorIK());
    }
    public void StSpawn()
    {
        StartCoroutine(cormSpawn());
        //InvokeRepeating("cormSpawn", 2f, 2f);
    }
    // Start is called before the first frame update
    void Start()
    {
        //if (ui) { Paused(); }
        limitsec = Random.Range(20, 40);
        //StartCoroutine(GetEnumeratorIK());
       
    }

    public void newspawns()
    {
        pcolor = Random.Range(0, Offcorms.Length);
        GameObject worm = InstatceOffCorm(posX, -2.5f, Random.Range(0, vscales.Length));
        GameObject worm2 = InstatceOffCorm(-posX, -posY, Random.Range(0, vscales.Length));
        GameObject worm3 = InstatceOffCorm(posX - 1.5f, -2.5f, Random.Range(0, vscales.Length));
        GameObject worm4 = InstatceOffCorm(posX + 1.5f, -2.5f, Random.Range(0, vscales.Length));
        GameObject worm5 = InstatceOffCorm(posX + 1.5f, posY, Random.Range(0, vscales.Length));
        GameObject worm6 = InstatceOffCorm(posX - 1.5f, posY, Random.Range(0, vscales.Length));
        GameObject worm7 = (GameObject)MonoBehaviour.Instantiate(newcorm.gameObject, transform.position + new Vector3(-posX - 1.5f, -2.5f, 0), Quaternion.identity);
        GameObject worm8 = (GameObject)MonoBehaviour.Instantiate(newcorm.gameObject, transform.position - new Vector3(-posX, posY, 0), Quaternion.identity);
        GameObject worm9 = (GameObject)MonoBehaviour.Instantiate(newcorm.gameObject, transform.position + new Vector3(-posX + 1.5f, -2.5f, 0), Quaternion.identity);
        //leftpart.GetComponent<Renderer>().material = mts[pcolor];
        //rightpart.GetComponent<Renderer>().material = mts[pcolor];
    }
    
    public GameObject InstatCorm(float px1, float py1, float numScale)
    {
        pcolor = Random.Range(0, Offcorms.Length);
        GameObject worm = (GameObject)MonoBehaviour.Instantiate(newcorm.gameObject, new Vector3(px1, py1, 0), Quaternion.identity);
        GetEnems.Add(worm.GetComponent<offcorm>());
        worm.GetComponent<offcorm>().strt = worm.transform.position;
        return worm;
    }
    bool load5sec = false;
    float vector3pos;
    public bool Load5sec { get => load5sec; set => load5sec = value; }
    
    IEnumerator GetEnumeratorIK()
    {
        while (true)
        {
            yield return new WaitForSeconds(5.0f);
        }
    }
    public GameObject InstatceOffCorm(float px1, float py1, float numScale)
    {
        pcolor = Random.Range(0, Offcorms.Length);
        GameObject worm = (GameObject)MonoBehaviour.Instantiate(Offcorms[0].gameObject, new Vector3(px1, py1, 0), Quaternion.identity);
        worm.transform.localScale = new Vector3(numScale, numScale, numScale);
        GetOffcorms.Add(worm.GetComponent<offcorm>());
        worm.GetComponent<offcorm>().strt = worm.transform.position;
        PoolManager.GetObject(worm.name, worm.transform.position, worm.transform.rotation);
        return worm;
    }
    public IEnumerator cormSpawn()
    {
        yield return new WaitForSeconds(Random.Range(_spawnDelayRange.x, _spawnDelayRange.y));

        while (true)
        {
            //if (GetOffcorms.Count < 1)
            //{
            delcount = 0;
                countplus++;
                if (countplus > 8)
                {
                    countplus = 2;
                }
            GameObject worm = InstatceOffCorm(Random.Range(-10.5f, 10.5f), 8, vscales[Random.Range(0, vscales.Length)]);
            //}
            yield return new WaitForSeconds(Random.Range(_spawnPeriodRange.x, _spawnPeriodRange.y));
        }

    }
    int sec = 0;
    int limitsec = 0;
    public void newSpawn()
    {
        if (countplus > 5)
        {
            countplus = 2;
        }
        for (int ast = 0; ast < countplus + 2; ast++)
        {
            GameObject worm = InstatceOffCorm(Random.Range(-10.5f, 10.5f), 8, vscales[Random.Range(0, vscales.Length)]);
        }
        for (int ast = 0; ast < countplus + 2; ast++)
        {
            GameObject worm = InstatCorm(Random.Range(-10.5f, 10.5f), Random.Range(3.5f, 8.5f), vscales[Random.Range(0, vscales.Length)]);
        }
        
    }
    public void newStart()
    {
        GameObject worm = (GameObject)MonoBehaviour.Instantiate(Offcorms[pcolor].gameObject, transform.position + new Vector3(posX, -2.5f, 0), Quaternion.identity);
        GameObject worm2 = (GameObject)MonoBehaviour.Instantiate(Offcorms[pcolor].gameObject, transform.position - new Vector3(posX, posY, 0), Quaternion.identity);
        GameObject worm3 = (GameObject)MonoBehaviour.Instantiate(Offcorms[pcolor].gameObject, transform.position + new Vector3(posX - 1.5f, -2.5f, 0), Quaternion.identity);
        GameObject worm4 = (GameObject)MonoBehaviour.Instantiate(Offcorms[pcolor].gameObject, transform.position + new Vector3(posX + 1.5f, -2.5f, 0), Quaternion.identity);
        GameObject worm5 = (GameObject)MonoBehaviour.Instantiate(Offcorms[pcolor].gameObject, transform.position - new Vector3(posX + 1.5f, posY, 0), Quaternion.identity);
        GameObject worm6 = (GameObject)MonoBehaviour.Instantiate(Offcorms[pcolor].gameObject, transform.position - new Vector3(posX - 1.5f, posY, 0), Quaternion.identity);
        GameObject worm7 = (GameObject)MonoBehaviour.Instantiate(newcorm.gameObject, transform.position + new Vector3(-posX - 1.5f, -2.5f, 0), Quaternion.identity);
        GameObject worm8 = (GameObject)MonoBehaviour.Instantiate(newcorm.gameObject, transform.position - new Vector3(-posX, posY, 0), Quaternion.identity);
        GameObject worm9 = (GameObject)MonoBehaviour.Instantiate(newcorm.gameObject, transform.position + new Vector3(-posX + 1.5f, -2.5f, 0), Quaternion.identity);
    }

    // Update is called once per frame
    void Update()
    {
        //vector3pos = FindObjectOfType<WormDataBase>().transform.position.x + 15;
        if (MMDebug.Raycast3DBoolean(transform.position - new Vector3(16.5f, 0, 0.1f), Vector3.down, 9.35f, MoveOnSightLayer, Color.red, true))
        {
            //SamSapiel.instnce.getWorm().GetComponent<WormDataBase>().plusindex(pcolor);
        }
        
    }
}
