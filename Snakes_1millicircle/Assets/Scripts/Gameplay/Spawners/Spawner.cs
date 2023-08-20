using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
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
        [SerializeField] private bool _autoSpawn22 = false;
        [SerializeField]
        private Sprite[] Objects;
        [SerializeField] private int anothermovement = 0;
        public Slider PlayerHealthHUD;
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
            //if(!_autoSpawn22)StartCoroutine(Spawn());
            //else { 
            StartCoroutine(Spawn22());
            //}
        }

        public void StopSpawn()
        {
            StopAllCoroutines();
        }
        public bool setSpawnBool()
        {
            bool isFind = false;
            foreach(var obj123 in FindObjectsOfType<EnemySp>())
            {
                if (obj123.GetSpawner == this) isFind = true;
            }
            return isFind;
        }
        private IEnumerator Spawn22()
        {
            yield return new WaitForSeconds(Random.Range(_spawnDelayRange.x, _spawnDelayRange.y));

            while (true)
            {
                if (!setSpawnBool())
                {
                    var enem = Instantiate(_object, transform.position, transform.rotation, _parent);
                    enem.GetComponent<Enemy_Combat_Script>().SetSliders(PlayerHealthHUD);
                    enem.GetComponent<EnemySp>().GetSpawner = this;
                }
                yield return new WaitForSeconds(Random.Range(_spawnPeriodRange.x, _spawnPeriodRange.y));
            }
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
