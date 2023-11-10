using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlateCompleteVisual : MonoBehaviour
{
    [Serializable]
    public struct KitchenObjectSo_GameObject {
        public KitchenObjectSO kitchenObjectSO;
        public GameObject gameObject;
    }

    [SerializeField] private PlateKitchenObject plateKitchenObject;
    [SerializeField] private List<KitchenObjectSo_GameObject> kitchenObjectSoGameObjectsList;

    private void Start()
    {
        plateKitchenObject.OnIngredientAdded += PlateKitchenObject_OnIngredientAdded;

        foreach (KitchenObjectSo_GameObject kitchenObjectSoGameObject in kitchenObjectSoGameObjectsList)
        {
            kitchenObjectSoGameObject.gameObject.SetActive(false);
        }
    }

    private void PlateKitchenObject_OnIngredientAdded(object sender, PlateKitchenObject.OnIngredientAddedEventArgs e)
    {
        foreach (KitchenObjectSo_GameObject kitchenObjectSoGameObject in kitchenObjectSoGameObjectsList)
        {
            if (kitchenObjectSoGameObject.kitchenObjectSO == e.kitchenObjectSO)
            {
                kitchenObjectSoGameObject.gameObject.SetActive(true);
            }
        }
    }
}
