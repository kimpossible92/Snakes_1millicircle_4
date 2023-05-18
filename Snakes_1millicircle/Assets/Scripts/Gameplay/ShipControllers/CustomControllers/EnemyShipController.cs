using System.Collections;
using System.Collections.Generic;
using Gameplay.ShipControllers;
using Gameplay.ShipSystems;

using UnityEngine;

public class EnemyShipController : ShipController
{
    [SerializeField]
    private Vector2 _fireDelay;

    private bool _fire = true;
    protected int anotherMovement = 0;
    public void setAnotherMovement(int any)
    {
        anotherMovement = any;
    }
    
    protected override void ProcessHandling(MovementSystem movementSystem)
    {
        if (anotherMovement == 1) { movementSystem.LateralMovement(Time.deltaTime); }
        else if (anotherMovement == -1) { movementSystem.HorizontalMovement(Time.deltaTime); }
        else if (anotherMovement == -2) { }
        else { movementSystem.LongitudinalMovement(Time.deltaTime); }
    }
    protected override void ProcessFire(WeaponSystem fireSystem)
    {
        if (!_fire)
            return;

        fireSystem.TriggerFire();
        StartCoroutine(FireDelay(Random.Range(_fireDelay.x, _fireDelay.y)));
    }


    private IEnumerator FireDelay(float delay)
    {
        _fire = false;
        yield return new WaitForSeconds(delay);
        _fire = true;
    }
}
