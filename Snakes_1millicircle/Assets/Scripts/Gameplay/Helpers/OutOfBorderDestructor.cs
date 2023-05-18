using System.Collections;
using System.Collections.Generic;
using Gameplay.Helpers;
using UnityEngine;

public class OutOfBorderDestructor : MonoBehaviour
{

    [SerializeField]
    private SpriteRenderer _representation;
    
    void Update()
    {
        CheckBorders();
    }
    
    private void CheckBorders()
    {
        if(!GameAreaHelper.IsInGameplayArea(transform, _representation.bounds))
        {
            Destroy(gameObject);
        }
    }
}
