using System.Collections.Generic;
using Gameplay.Weapons;
using UnityEngine;
using System.Linq;

namespace Gameplay.ShipSystems
{
    public class WeaponSystem : MonoBehaviour
    {

        [SerializeField]
        private List<Weapon> _weapons;
        [SerializeField] private GameObject goParent;
        bool ll1 = false;

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                ll1 = true;
            }
            if (Input.GetKeyDown(KeyCode.T))
            {
                ll1 = false;
            }
        }
        public void setRocketOrBeam() { _weapons.ForEach(w => w.setwp()); }
        public void setBeam() { _weapons.ForEach(w => w.setwp2()); }
        public void Init(UnitBattleIdentity battleIdentity)
        {
            if (tag == "Player")
            {
                var lg = Instantiate(_weapons[0], goParent.transform);
                var lg2 = Instantiate(_weapons[0], goParent.transform);
                lg.transform.localPosition = new Vector3(-4.5f, 2.5f);
                lg2.transform.localPosition = new Vector3(4.5f, 2.5f);
                _weapons.Add(lg);
                _weapons.Add(lg2);
                lg.Init(battleIdentity);
                lg2.Init(battleIdentity);
                ll1 = false;
            }
            _weapons.ForEach(w => w.Init(battleIdentity));
        }


        public void TriggerFire()
        {
            if (tag == "Player")
            {
                _weapons.ForEach(w => w.setpl1());
                if (ll1 == true) { _weapons.ForEach(w => w.TriggerFire()); }
                if (ll1 == false)
                {
                    _weapons[0].TriggerFire();
                    _weapons[1].TriggerFire();
                }
            }
            else if (tag == "bonus") { }
            else { _weapons.ForEach(w => w.TriggerFire()); }
        }
        public void SetNewSpeed()
        {
            _weapons.ForEach(w => w.newCooldown());
        }
    }
}
