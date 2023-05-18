using System.Collections;
using System.Collections.Generic;
using Gameplay.Weapons.Projectiles;
using UnityEngine;

public class LaserBeam : ProjectilePool
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
    protected override void Move(float speed)
    {
        transform.Translate(speed * Time.deltaTime * Vector3.up);
    }
}
