using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Gameplay.Spawners
{
    public class Spawner : MonoBehaviour
    {

        [SerializeField]
        private GameObject _object;
        int level=0;
        [SerializeField]
        private Transform _parent;
        
        [SerializeField]
        private Vector2 _spawnPeriodRange;
        
        [SerializeField]
        private Vector2 _spawnDelayRange;

        [SerializeField]
        private bool _autoStart = true;
        [SerializeField]
        private Sprite[] Objects;
        [SerializeField] private int anothermovement = 0;
        public void lvlplus()
        {
            level++;
        }
        private void Start()
        {
            if (_autoStart)
                StartSpawn();
        }
        public void NoSpawnStart() { _autoStart = false; }

        public void StartSpawn()
        {
            StartCoroutine(Spawn());
        }

        public void StopSpawn()
        {
            StopAllCoroutines();
        }


        private IEnumerator Spawn()
        {
            yield return new WaitForSeconds(Random.Range(_spawnDelayRange.x, _spawnDelayRange.y));
            
            while (true)
            {
                
                var enem = Instantiate(_object, transform.position, transform.rotation, _parent);
                if (23 >= level) { 
                    enem.transform.Find("Hull").GetComponent<SpriteRenderer>().sprite = Objects[level];
                    if (level >= 8) { enem.GetComponent<Spaceships.Spaceship>().setlive(200f); enem.GetComponent<Gameplay.ShipSystems.WeaponSystem>().setRocketOrBeam(); }
                    else if (level >= 16) { enem.GetComponent<Spaceships.Spaceship>().setlive(300f); enem.GetComponent<Gameplay.ShipSystems.WeaponSystem>().setBeam(); }
                    else if (level >= 21) { enem.GetComponent<Spaceships.Spaceship>().setlive(400f); enem.GetComponent<Gameplay.ShipSystems.WeaponSystem>().setBeam(); }
                    else { enem.GetComponent<Spaceships.Spaceship>().setlive(1f); }
                }
                else { level = 0; }
                enem.GetComponent<EnemySp>().GetSpawner = this;
                if (anothermovement == 1) { enem.GetComponent<EnemyShipController>().setAnotherMovement(1); }
                if (anothermovement == -1) { enem.GetComponent<EnemyShipController>().setAnotherMovement(-1); }
                else {  }
                yield return new WaitForSeconds(Random.Range(_spawnPeriodRange.x, _spawnPeriodRange.y));
            }
        }
    }
}
