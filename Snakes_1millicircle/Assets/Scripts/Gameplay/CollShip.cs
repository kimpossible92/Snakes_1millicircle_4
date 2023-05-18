using System.Collections;
using UnityEngine;
using Gameplay.Spaceships;
using Gameplay.ShipSystems;

public class CollShip : MonoBehaviour
{
    public enum slojnost
    {
        easy = 1, normal = 3, hard = 5, nevosmojno = 10
    }
    public slojnost OnSlojnost;
    public static bool isInvincible = false;
    public static float timeSpentInvincible;
    public Texture2D lifeIconTexture;
    public static bool dead = false;
    public static int life = 100;
    [SerializeField] LayerMask layer; public float speed2 = 0.04f;
    public float speed = 0.1f;[SerializeField]
    bool showGUI = true;
    public float limitx1 = -2, limitx = 16f, limity1 = -1, limity = 7;
    // NEED TO ADD
    public static Vector2 bombermanPosition, bombermanPositionRounded;
    Vector2 dir2; int dr = 5;
    Animator anim; Vector3 startpos1;
    public void NewStart()
    {
        dead = false;
        life = 100;
        startpos1 = transform.position;
    }
    // Use this for initialization
    private void Start()
    {
        NewStart();
    }
    private void Update()
    {
        if (dead)
        {
            //Application.LoadLevel("SampleScene"); 
            foreach (var pi in FindObjectsOfType<Gameplay.Spawners.Spawner>())
            {
                //pi.StopSpawn();
            }
            //GameObject.Find("Canvas").transform.Find("New Game").gameObject.SetActive(true);
        }
        if (isInvincible)
        {
            timeSpentInvincible += Time.deltaTime;

            if (timeSpentInvincible < 3f)
            {
                float remainder = timeSpentInvincible % .3f;
                GetComponent<Renderer>().enabled = remainder > .15f;
            }

            else
            {
                GetComponent<Renderer>().enabled = true;
                isInvincible = false;
            }
        }
    }
    void DisplayLifeCount()
    {
        Rect lifeIconRect = new Rect(10, 150, 32, 32);
        GUI.DrawTexture(lifeIconRect, lifeIconTexture);

        GUIStyle style = new GUIStyle();
        style.fontSize = 30;
        style.fontStyle = FontStyle.Bold;
        style.normal.textColor = Color.yellow;

        Rect labelRect = new Rect(lifeIconRect.xMax + 10, lifeIconRect.y, 60, 32);
        GUI.Label(labelRect, life.ToString(), style);
    }
    void OnGUI()
    {
        if (!showGUI)
            return;
        DisplayLifeCount();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "enemy")
        {
            life -= (int)OnSlojnost;
            if (life <= 0)
            {
                dead = true;
            }
            isInvincible = true;
            timeSpentInvincible = 0;
        }
        if (collision.gameObject.tag == "bonus")
        {
            if (collision.GetComponent<Spaceship>().bonusRead == 0) { life += 20; Destroy(collision.gameObject); }
            if (collision.GetComponent<Spaceship>().bonusRead == 1) { GetComponent<MovementSystem>().loadMove(); Destroy(collision.gameObject); }
            if (collision.GetComponent<Spaceship>().bonusRead == 2) { GetComponent<WeaponSystem>().SetNewSpeed(); Destroy(collision.gameObject); }
        }
    }
}