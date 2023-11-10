using PathologicalGames;
using UnityEngine;

public class StructureModel : MonoBehaviour
{
    float yHeight = 0;

    public void SwapModel(GameObject model, Quaternion rotation)
    {
        foreach (Transform child in transform)
        {
            var childObj = child.gameObject;
            if (model.name.Equals(childObj.name))
            {
                childObj.SetActive(true);
                var correctedRotation = rotation.eulerAngles;
                correctedRotation= new Vector3(-90,correctedRotation.y,correctedRotation.z);
                child.localRotation = Quaternion.Euler(correctedRotation);
            }
            else
            {
                childObj.SetActive(false);
            }
        }
        
        /*
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
           // PoolManager2.Pools["Buildings"].Despawn(child.transform);
        }

        print("pravi se " + model.name);
        var structure = Instantiate(model, transform);
        structure.transform.localPosition = new Vector3(0, yHeight, 0);
        structure.transform.localRotation = rotation;
        */
    }
}
