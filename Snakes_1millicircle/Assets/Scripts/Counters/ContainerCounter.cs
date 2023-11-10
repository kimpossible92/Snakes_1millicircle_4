using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContainerCounter : BaseCounter
{
    public event EventHandler OnPlayerGrabbedObject;

    [SerializeField] private KitchenObjectSO kitchenObjectSO;
    
    public override void Interact(Player player)
    {
        Debug.Log("Interacted with container");
        if (!player.HasKitchenObject())
        {
            //Player2 not carrying an object
            print("kitchenOjb!=null"+kitchenObjectSO != null);
            KitchenObject.SpawnKitchenObject(kitchenObjectSO, player);
            OnPlayerGrabbedObject?.Invoke(this,EventArgs.Empty);
        }
    }

    
}
