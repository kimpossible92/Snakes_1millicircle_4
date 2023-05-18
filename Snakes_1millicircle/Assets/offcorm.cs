using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Gameplay.Helpers;
using Gameplay.Weapons;
using Gameplay.Weapons.Projectiles;

public class offcorm : ProjectilePool
{
    #region Data
    List<ProjectilePool> objects;
    Transform objectsParent;
    #endregion
    void AddObject(ProjectilePool sample, Transform objects_parent)
    {
        GameObject temp = GameObject.Instantiate(sample.gameObject);
        temp.name = sample.name;
        temp.transform.SetParent(objects_parent);
        objects.Add(temp.GetComponent<ProjectilePool>());
        if (temp.GetComponent<Animator>())
            temp.GetComponent<Animator>().StartPlayback();
        temp.SetActive(false);
    }
    public void Initialize(int count, ProjectilePool sample, Transform objects_parent)
    {
        objects = new List<ProjectilePool>(); //инициализируем List
        objectsParent = objects_parent; //инициализируем локальную переменную для последующего использования
        for (int i = 0; i < count; i++)
        {
            AddObject(sample, objects_parent); //создаем объекты до указанного количества
        }
    }
    public ProjectilePool GetObject()
    {
        for (int i = 0; i < objects.Count; i++)
        {
            if (objects[i].gameObject.activeInHierarchy == false)
            {
                return objects[i];
            }
        }
        AddObject(objects[0], objectsParent);
        return objects[objects.Count - 1];
    }
    [SerializeField] int color;
    public Vector3 strt;
    public int Color { get => color; set => color = value; }
    int pos1, pos2;
    // Start is called before the first frame update
    void Start()
    {
        //strt = transform.position;
        pos1 = Random.Range(-1, 2);
        pos2 = Random.Range(-1, 2);
        if (pos2 == 0)
        {
            pos2 = -1;
        }
    }

    // Update is called once per frame
    void Update()
    {
        //       if (FindObjectOfType<Road>().getpause()) return;
        //if (tag == "corm")
        //{
        transform.Translate(new Vector3(0.08f * pos2, -0.08f, 0));
        //}
        if (transform.position.x > 50)
        {
            transform.position = new Vector3(-50, transform.position.y, transform.position.z);
        }
        if (transform.position.x < -50)
        {
            transform.position = new Vector3(50, transform.position.y, transform.position.z);
        }
        if (transform.position.y > 35f)
        {
            transform.position = new Vector3(transform.position.x, -5f, transform.position.z);
        }
        if (transform.position.y < -10)
        {
            transform.position = new Vector3(transform.position.x, 30, transform.position.z);
        }
    }

    protected override void Move(float speed)
    {
        throw new System.NotImplementedException();
    }
    [SerializeField] int easy = 20;
    [SerializeField] int medium = 50;
    [SerializeField] int hard = 100;
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (gameObject.tag == "enemy" && collision.gameObject.tag == "Player")
        {
            if (collision.gameObject.tag == "Player")
            {
                if (transform.localScale.x == 1) { FindObjectOfType<Road>().addScore(hard); }
                if (transform.localScale.x == 2) { FindObjectOfType<Road>().addScore(medium); }
                if (transform.localScale.x == 3) { FindObjectOfType<Road>().addScore(easy); }
                if (transform.localScale.x == 7) { FindObjectOfType<Road>().addScore(easy); }
            }
            if (transform.localScale.x == 2)
            {
                transform.localScale = new Vector3(1, 1, 1);
                if (pos2 == -1)
                {
                    GameObject second = Instantiate(gameObject, transform.position + new Vector3(-2, -2, 0), Quaternion.identity);
                    transform.position = new Vector3(transform.position.x + 2, transform.position.y + 2, transform.position.z);
                    pos2 = 1;
                }
                if (pos2 == 0 || pos2 == 1)
                {
                    GameObject second = Instantiate(gameObject, transform.position + new Vector3(2, 2, 0), Quaternion.identity);
                    transform.position = new Vector3(transform.position.x - 2, transform.position.y - 2, transform.position.z);
                    pos2 = -1;
                }


            }
            else if (transform.localScale.x == 7)
            {
                transform.localScale = new Vector3(3, 3, 3);
                if (pos2 == -1)
                {
                    GameObject second = Instantiate(gameObject, transform.position + new Vector3(-3, -3, 0), Quaternion.identity);
                    transform.position = new Vector3(transform.position.x + 3, transform.position.y + 3, transform.position.z);
                    pos2 = 1;
                }
                if (pos2 == 0 || pos2 == 1)
                {
                    GameObject second = Instantiate(gameObject, transform.position + new Vector3(3, 3, 0), Quaternion.identity);
                    transform.position = new Vector3(transform.position.x - 3, transform.position.y - 3, transform.position.z);
                    pos2 = -1;
                }
            }
            else if (transform.localScale.x == 3)
            {
                transform.localScale = new Vector3(2, 2, 2);
                if (pos2 == -1)
                {
                    GameObject second = Instantiate(gameObject, transform.position + new Vector3(-2, -2, 0), Quaternion.identity);
                    transform.position = new Vector3(transform.position.x + 2, transform.position.y + 2, transform.position.z);
                    pos2 = 1;
                }
                if (pos2 == 0 || pos2 == 1)
                {
                    GameObject second = Instantiate(gameObject, transform.position + new Vector3(2, 2, 0), Quaternion.identity);
                    transform.position = new Vector3(transform.position.x - 2, transform.position.y - 2, transform.position.z);
                    pos2 = -1;
                }
            }
            else if (transform.localScale.x == 1 && collision.gameObject.tag == "Player")
            {
                FindObjectOfType<Road>().removeCorm(this);
                Destroy(gameObject);
            }
            else
            {
                transform.localScale = new Vector3(1, 1, 1);
                if (pos2 == 0 || pos2 == -1)
                {
                    transform.position = new Vector3(transform.position.x + 2, transform.position.y + 3, transform.position.z);
                    pos2 = 1;
                }
                if (pos2 == 1)
                {
                    transform.position = new Vector3(transform.position.x - 2, transform.position.y - 3, transform.position.z);
                    pos2 = -1;
                }

            }
        }

    }
}
