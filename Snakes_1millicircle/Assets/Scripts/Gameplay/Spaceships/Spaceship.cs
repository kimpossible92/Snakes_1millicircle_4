using System;
using Gameplay.ShipControllers;
using Gameplay.ShipSystems;
using Gameplay.Weapons;
using UnityEngine;
using Gameplay.Spawners;
namespace Gameplay.Spaceships
{
    public class Spaceship : MonoBehaviour, ISpaceship, IDamagable
    {
        [SerializeField]
        private ShipController _shipController;
        [SerializeField]
        private Sprite[] bonusSprites;
        [SerializeField]
        private MovementSystem _movementSystem;
        
        private Spawner GetSpawner;
        [SerializeField]
        private WeaponSystem _weaponSystem;

        [SerializeField]
        private UnitBattleIdentity _battleIdentity;

        [SerializeField]
        private float lvllive=1;
        protected int bonustype = 0;
        public  int bonusRead => bonustype;
        public MovementSystem MovementSystem => _movementSystem;
        public WeaponSystem WeaponSystem => _weaponSystem;
        public UnitBattleIdentity BattleIdentity => _battleIdentity;
        public void setlive(float l)
        {
            lvllive =l;
        }
        private void Start()
        {
            _shipController.Init(this);
            _weaponSystem.Init(_battleIdentity);
        }
        void Update()
        {

        }
        [SerializeField] int scoreenemy = 200;
        public void ApplyDamage(IDamageDealer damageDealer)
        {
            //print(damageDealer.Damage);
            lvllive -= damageDealer.Damage;
            if (lvllive <= 0) {
                int randBonus = UnityEngine.Random.Range(0, 5);
                if (randBonus == 0) {
                    tag = "bonus"; 
                    bonustype = UnityEngine.Random.Range(0, bonusSprites.Length); 
                    gameObject.transform.Find("Hull").GetComponent<SpriteRenderer>().sprite = bonusSprites[bonustype]; 
                }
                else {
                    //FindObjectOfType<UIPlay>().addScore(scoreenemy); 
                    GetComponent<EnemySp>().GetSpawner.lvlplus();
                    Destroy(gameObject);
                }
            }
        }
    }
}
