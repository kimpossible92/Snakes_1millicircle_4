using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIPlay : MonoBehaviour
{
    public static bool isInvincible = false; public static float timeSpentInvincible;
    [SerializeField] UnityEngine.UI.Button play;
    [SerializeField] UnityEngine.UI.Button pause;
    [SerializeField] UnityEngine.UI.Button newgame;
    private void Awake()
    {
        foreach(var pi in FindObjectsOfType<Gameplay.Spawners.Spawner>())
        {
            pi.NoSpawnStart();
        }
    }
    int score = 0;
    public void NewGame()
    {
        score = 0;
        foreach (var pi in FindObjectsOfType<Gameplay.Spawners.Spawner>())
        {
            pi.StartSpawn();
        }
        foreach (var pi in FindObjectsOfType<Road>())
        {
            pi.StSpawn();
        }
        FindObjectOfType<CollShip>().NewStart(); isInvincible = true;
        GameObject.Find("Canvas").transform.Find("New Game").gameObject.SetActive(false); pse = false;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //if (isInvincible)
        //{
        //    timeSpentInvincible += Time.deltaTime;

        //    if (timeSpentInvincible < 3f)
        //    {
        //        float remainder = timeSpentInvincible % .3f;
        //        GameObject.Find("Canvas").transform.Find("New Game").gameObject.SetActive(remainder > .8f);
        //    }

        //    else
        //    {
        //        GameObject.Find("Canvas").transform.Find("New Game").gameObject.SetActive(false);
        //        isInvincible = false;
        //    }
        //}
    }
    public void Pause1()
    {
        Time.timeScale = 0;
        pse = true;
    }
    public void Resume()
    {

        Time.timeScale = 1;
        play.gameObject.SetActive(false);
        newgame.gameObject.SetActive(false);
        pse = false;
    }
    public void Reset()
    {
        foreach (var pi in FindObjectsOfType<EnemyShipController>())
        {
            Destroy(pi.gameObject);
        }
        foreach (var pi in FindObjectsOfType<Gameplay.Spawners.Spawner>())
        {
            pi.StopSpawn();
        }
        foreach (var pi in FindObjectsOfType<Road>())
        {
            pi.StopSpawn();
        }
        Time.timeScale = 1;
        score = 0;
        foreach (var pi in FindObjectsOfType<Gameplay.Spawners.Spawner>())
        {
            pi.StartSpawn();
        }
        foreach (var pi in FindObjectsOfType<Road>())
        {
            pi.StSpawn();
        }
        FindObjectOfType<CollShip>().NewStart(); isInvincible = true;
        play.gameObject.SetActive(false);
        newgame.gameObject.SetActive(false);
        GameObject.Find("Canvas").transform.Find("New Game").gameObject.SetActive(false);
        pse = false;
    }
    public void addScore(int sc)
    {
        score += sc;
    }
    bool pse = false;
    private void OnGUI()
    {
        ScoreUI();
        if (!pse)
        {
            pause.gameObject.SetActive(true);
        }
        else
        {
            pause.gameObject.SetActive(false);
            play.gameObject.SetActive(true);
            newgame.gameObject.SetActive(true);
        }
    }
    void ScoreUI()
    {
        Rect lifeIconRect = new Rect(10, 10, 32, 32);

        GUIStyle style = new GUIStyle();
        style.fontSize = 30;
        style.fontStyle = FontStyle.Bold;
        style.normal.textColor = Color.yellow;

        Rect labelRect = new Rect(lifeIconRect.xMax + 10, lifeIconRect.y, 60, 32);
        GUI.Label(labelRect, score.ToString(), style);
    }
}
