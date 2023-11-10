using System.Collections.Generic;
using System.Linq;
using MEC;
using PathologicalGames;
using UnityEngine;
using WalkerScripts;

public class SpawnWalkers : MonoBehaviour
{
    private const float waitTime = 1f;
    [SerializeField] private PlacementManager placementManager;
    [SerializeField] private Transform personPrefab;
    [SerializeField] private float spawnCoolDown=30f;
    
    private void Start()
    {
        Timing.RunCoroutine(SpawnWalkersCoRoutine());
    }


    private IEnumerator<float> SpawnWalkersCoRoutine()
    {
        while (true)
        {
//            print("u korutini sam ");
            yield return Timing.WaitForSeconds(waitTime);
            var spawnStructureList=MakeListOfStructuresThatShouldSpawnPeople();
            bool spawnedSomething=false;
            for (int i = 0; i < spawnStructureList.Count; i++)
            {
                var structure = spawnStructureList[i];
                var spawnedPerson=PoolManager2.Pools["Buildings"].Spawn(personPrefab);
                var spawnPos=structure.transform.position;
                spawnedPerson.position = spawnPos;
                var script=spawnedPerson.GetComponent<WalkerMovement>();
                print("spawn pozicija je " + spawnPos);
                var a=new Vector3Int((int) spawnPos.x,0,(int) spawnPos.z);
                print("konvertovana pozicija je " + a);
                script.targetPosition = a;
                var chosenPoint=script.FindClosestRoad();
                script.targetPosition = chosenPoint;
                print(" zadata meta " + chosenPoint);
                spawnedSomething = true;
            }

            if (spawnedSomething)
            {
             //   print("u cekanju sam ");
                yield return Timing.WaitForSeconds(spawnCoolDown);
            }
        }
    }
    
    private List<StructureModel> MakeListOfStructuresThatShouldSpawnPeople()
    {
        List<StructureModel> structureList= new List<StructureModel>();
        foreach (var kvp in placementManager.structureDictionary)
        {
            //print("spisak zgrada " + a.Value);
            var cellType = placementManager.placementGrid[kvp.Key.x, kvp.Key.z];
            if (cellType == CellType.Building)
            {
                var listOfNeighours = placementManager.placementGrid.GetAllAdjacentCellTypes(kvp.Key.x, kvp.Key.z);
                
                if (listOfNeighours.Contains(CellType.Road))
                {
                    structureList.Add(kvp.Value);
                    //ToDo maybe add walker details per building? Link StructureModel from building to its preset for easier information gathering?
                    //ToDo cooldown for spawn per each building?
                }
            }
        }
        return structureList;
    }
}
