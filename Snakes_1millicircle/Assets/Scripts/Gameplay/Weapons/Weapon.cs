using System.Collections;
using Gameplay.Weapons.Projectiles;
using Photon.Pun;
using UnityEngine;

namespace Gameplay.Weapons
{
    public class Weapon : MonoBehaviourPun
    {

        [SerializeField]
        private ProjectilePool _projectile;
        [SerializeField]
        private ProjectilePool[] projectiles;
        int wpnum;
        [SerializeField]
        private Transform _barrel;

        [SerializeField]
        private float _cooldown;


        private bool _readyToFire = true;
        private UnitBattleIdentity _battleIdentity;

        public void Update()
        {
            if (Input.GetKeyDown(KeyCode.Q)&&projectiles.Length > wpnum)
            {
                wpnum++;
            }

            if (Input.GetKeyDown(KeyCode.E) && wpnum > 0)
            {
                wpnum--;
            }
            if(projectiles.Length < wpnum || wpnum < 0)
            {
                wpnum = 0;
            }
        }
        public void newCooldown()
        {
            //_cooldown = 0.33f;
            Invoke("old", 8.0f);
        }
        private void old()
        {
            _cooldown = 0.1f;
        }
        public void setwp2()
        {
            wpnum = 2;
        }
        public void setwp()
        {
            wpnum = 1;
        }
        public void Init(UnitBattleIdentity battleIdentity)
        {
            _battleIdentity = battleIdentity;
        }

        bool isplayer = false;
        public void setpl1()
        {
            isplayer = true;
        }
        public void TriggerFire()
        {
            if (!_readyToFire)
                return;
            _projectile = projectiles[wpnum];
            var proj = Instantiate(_projectile, _barrel.position, _barrel.rotation);
            proj.Init(_battleIdentity);
            if (isplayer) { proj.tag = "Player"; proj.setNamed(Photon.Pun.PhotonNetwork.NickName); PoolManager.GetObject(proj.gameObject.name, proj.gameObject.transform.position, proj.gameObject.transform.rotation); proj.GetComponent<SpriteRenderer>().color = Color.green; }
            StartCoroutine(Reload(_cooldown));
        }


        private IEnumerator Reload(float cooldown)
        {
            _readyToFire = false;
            yield return new WaitForSeconds(cooldown);
            _readyToFire = true;
        }

    }
}
