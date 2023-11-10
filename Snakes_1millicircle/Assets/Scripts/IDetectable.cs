using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDetectable
{
    void Detect();
    void UnDetect();

    Vector3 GetPosition();
    GameObject GetGameObject();
}