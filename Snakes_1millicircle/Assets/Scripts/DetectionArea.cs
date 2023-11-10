using Gameplay.ShipSystems;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public abstract class DetectionArea : MonoBehaviour, IDetectable
{
    public abstract void Create(float distance, Vector3 center);


    private List<IDetectable> _detectables = new List<IDetectable>();
    public IEnumerable<IDetectable> GetDetectableInArea()
    {
        return _detectables;
    }
    void Start() { _detectables = new List<IDetectable>(); }
    private void OnCollisionStay(Collision collision)
    {
        if (!collision.gameObject.TryGetComponent<IDetectable>(out var detectable))
            return;
        //print("detected");
        //if(!_detectables.Contains(detectable)) 
        _detectables.Add(detectable);
    }
    public void OnTriggerEnter(Collider other)
    {
        if (!other.gameObject.TryGetComponent<IDetectable>(out var detectable))
            return;
        _detectables.Add(detectable);
    }

    public void OnTriggerExit(Collider other)
    {
        if (!other.gameObject.TryGetComponent<IDetectable>(out var detectable))
            return;

        _detectables.Remove(detectable);
    }
    public void Detect()
    {
        if (GetComponent<WeaponSystem>() != null) { GetComponent<WeaponSystem>().TriggerFire(); print("fire"); }
    }

    public void UnDetect()
    {
        //throw new NotImplementedException();
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