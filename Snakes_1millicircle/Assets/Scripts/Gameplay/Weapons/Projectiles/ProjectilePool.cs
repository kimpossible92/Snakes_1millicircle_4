using System;
using Gameplay.Helpers;
using UnityEngine;
using PlayerSystem;
namespace Gameplay.Weapons.Projectiles
{
    public abstract class ProjectilePool : MonoBehaviour, IDamageDealer
    {
        #region Interface
        public void ReturnToPool()
        {
            gameObject.SetActive(false);
        }
        #endregion
        [SerializeField]
        private float _speed;

        [SerializeField] 
        private float _damage;
        private string Nicknamed = "";
        public void setNamed(string nick) { Nicknamed = nick; }
        public string NickName => Nicknamed;

        private UnitBattleIdentity _battleIdentity;


        public UnitBattleIdentity BattleIdentity => _battleIdentity;
        public float Damage => _damage;

        

        public void Init(UnitBattleIdentity battleIdentity)
        {
            _battleIdentity = battleIdentity;
        }
        

        private void Update()
        {
            Move(_speed);
        }

        
        private void OnCollisionEnter2D(Collision2D other)
        {
            var damagableObject = other.gameObject.GetComponent<IDamagable>();
            
            if (damagableObject != null 
                && damagableObject.BattleIdentity != BattleIdentity && other.gameObject.tag !="bonus")
            {
                damagableObject.ApplyDamage(this);
                if (other.gameObject.GetComponent<EnemyShipController>()!=null) { FindObjectOfType<OverviewPanel>().SetAddScore(Nicknamed); }
                if (other.gameObject.GetComponent<PlayerView>() != null && other.gameObject.GetComponent<PlayerView>().PlName != Nicknamed) { FindObjectOfType<OverviewPanel>().SetAddSc(Nicknamed); }
            }
        }
        


        protected abstract void Move(float speed);
    }
}
