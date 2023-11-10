using Gameplay.ShipSystems;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
public class SphereDetectionArea : DetectionArea, IDetectable
{
    private SphereCollider _collider;
    public string targetTag = "Player";
    public int rays = 6;
    public int distance = 15;
    public float angle = 20;
    public Vector3 offset;
    private Transform target;
    bool GetRaycast(Vector3 dir)
    {
        bool result = false;
        RaycastHit hit = new RaycastHit();
        Vector3 pos = transform.position + offset;
        if (Physics.Raycast(pos, dir, out hit, distance))
        {
            if (hit.transform == target)
            {
                result = true;
                Debug.DrawLine(pos, hit.point, Color.green);
            }
            else
            {
                Debug.DrawLine(pos, hit.point, Color.blue);
            }
        }
        else
        {
            Debug.DrawRay(pos, dir * distance, Color.red);
        }
        return result;
    }
    bool RayToScan()
    {
        bool result = false;
        bool a = false;
        bool b = false;
        float j = 0;
        for (int i = 0; i < rays; i++)
        {
            var x = Mathf.Sin(j);
            var y = Mathf.Cos(j);

            j += angle * Mathf.Deg2Rad / rays;

            Vector3 dir = transform.TransformDirection(new Vector3(x, 0, y));
            if (GetRaycast(dir)) a = true;

            if (x != 0)
            {
                dir = transform.TransformDirection(new Vector3(-x, 0, y));
                if (GetRaycast(dir)) b = true;
            }
        }

        if (a || b) result = true;
        return result;
    }
    private void Start()
    {
        target = GameObject.FindGameObjectWithTag(targetTag).transform;
        _collider = gameObject.GetComponent<SphereCollider>();
    }
    void Update()
    {
        //if (Vector3.Distance(transform.position, target.position) < distance)
        //{
        //	if (RayToScan())
        //	{
        //		transform.LookAt(target);
        //		GetComponent<WeaponSystem>().TriggerFire();
        //	//	 target
        //		// Контакт с целью
        //	}
        //	else
        //	{
        //		// Поиск цели...
        //		//_detectables.Remove(detectable);
        //	}
        //}
    }
    public override void Create(float distance, Vector3 center)
    {
        _collider = gameObject.GetComponent<SphereCollider>();
        //_collider.isTrigger = true;
        _collider.radius = 1.3f;
        _collider.center = center;
    }
    public void Detect()
    {
        if (GetComponent<WeaponSystem>() != null) { GetComponent<WeaponSystem>().TriggerFire(); print("fire"); }
    }

    public void UnDetect()
    {
        //throw new NotImplementedException();
        //print("UnDetect");
    }

    public Vector3 GetPosition()
    {
        return transform.position;
    }

    public GameObject GetGameObject()
    {
        return gameObject;
    }
}