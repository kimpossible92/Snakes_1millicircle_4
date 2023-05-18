using Gameplay.ShipSystems;
using Gameplay.Spaceships;
using Photon.Pun;
using PlayerSystem;
using UnityEngine;

namespace Gameplay.ShipControllers
{
    public abstract class ShipController :  MonoBehaviourPun, IPunObservable
    {
        
        private ISpaceship _spaceship;
        public ISpaceship spaceship1 => _spaceship;
        public void Init(ISpaceship spaceship)
        {
            _spaceship = spaceship;
        }

        private void Start(){

        }
        private void Update()
        {
            ProcessHandling(_spaceship.MovementSystem);
            ProcessFire(_spaceship.WeaponSystem);
            
        }

        protected abstract void ProcessHandling(MovementSystem movementSystem);
        protected abstract void ProcessFire(WeaponSystem fireSystem);

        public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
        {
            if (stream.IsWriting)
            {
                stream.SendNext(transform.position);
            }
            else if (stream.IsReading&& GetComponent<PlayerView>()!=null)
            {
               GetComponent<PlayerView>().smoothMovement = (Vector3)stream.ReceiveNext();
            }
        }
    }
}
