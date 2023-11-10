using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class RecipeSO : ScriptableObject
{
    // Start is called before the first frame update
    public List<KitchenObjectSO> kitchenObjectSOList;
    public string recipeName;
}
