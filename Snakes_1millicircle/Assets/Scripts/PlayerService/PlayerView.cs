using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;
using WeaponSystems;
using Photon.Pun;

namespace PlayerSystem
{
    public class PlayerView : MonoBehaviourPun, IPunObservable
    {
        public PhotonView pv;
        private float speed = 50f;
        [SerializeField]
        private Text playerName;
        public string PlName => playerName.text;
        [SerializeField]
        private GameObject playerCamera, shotPos;
        [SerializeField] protected Joystick joystick;
        public float joyHorizontal;
        public float joyVertical;
        protected bool jump;
        //private Rigidbody2D myBody;
        [HideInInspector]public Vector3 smoothMovement;
        //public Vector3 smMove => smoothMovement;
        float rotationHorizontal;
        [SerializeField] protected JoyButton joyButton;
        [SerializeField] private bool setCamera = false;
        public Slider slider1,slider2;
        //public GameObject Ekcard;
        public bool isSetCam => setCamera;
        // Start is called before the first frame update
        void Start()
        {
           
            joyButton = FindObjectOfType<JoyButton>();
            //myBody = GetComponent<Rigidbody2D>();
            if (setCamera) { playerCamera = Camera.main.gameObject; shotPos = this.gameObject;return; }
            if (pv.IsMine)
            {
                joystick = FindObjectOfType<Joystick>();
                playerName.text = PhotonNetwork.NickName;
                var currentpl = new PlayerSpaceship();
                currentpl.NickName = PhotonNetwork.NickName;
                currentpl.score = 0;
                //FindObjectOfType<OverviewPanel>().Nickname.Add(currentpl);
                //playerCamera.SetActive(true);
                //Camera.main.gameObject.SetActive(false);
                
            }
        }

        // Update is called once per frame
        void Update()
        {
            if (setCamera)
            {
                return;
            }
            if (pv.IsMine)
            {
                Movement();
                
                //if(Input.GetMouseButtonDown(0))
                //{
                //    SpawnBullet();
                //}
                CheckExitScreen();
            }
            GetComponent<Gameplay.ShipControllers.CustomControllers.PlayerShipController>().OnFired();
            
            //else
            //{
            //    ReadMovement();
            //}



        }

        void Movement()
        {
            joyHorizontal = joystick.Horizontal; //* 100;
            //joyVertical = joystick.Vertical;// * 100;
            rotationHorizontal += joyHorizontal * Time.deltaTime * 100;
            
            transform.rotation = Quaternion.Euler(0, 0, rotationHorizontal);
            if (joyButton.Pressed)
            {
                joyVertical = 1;
                transform.Translate(Vector2.up*0.1f);
            }
        }

        void ReadMovement()
        {
            transform.position = Vector3.Lerp(transform.position, smoothMovement, Time.deltaTime * 5f);
        }
        private void CheckExitScreen()
        {
            //if (Camera.main == null)
            //{
            //    return;
            //}

            if (Mathf.Abs(transform.position.x) > (Camera.main.orthographicSize * Camera.main.aspect))
            {
                transform.position = new Vector3(-Mathf.Sign(transform.position.x) * Camera.main.orthographicSize * Camera.main.aspect, 0, transform.position.z);
                transform.position -= transform.position.normalized * 0.1f; // offset a little bit to avoid looping back & forth between the 2 edges 
            }

            if (Mathf.Abs(transform.position.z) > Camera.main.orthographicSize)
            {
                transform.position = new Vector3(transform.position.x, transform.position.y, -Mathf.Sign(transform.position.z) * Camera.main.orthographicSize);
                transform.position -= transform.position.normalized * 0.1f; // offset a little bit to avoid looping back & forth between the 2 edges 
            }
        }
        public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
        {
            //if (stream.IsWriting)
            //{
            //    stream.SendNext(transform.position);
            //}
            //else if (stream.IsReading)
            //{
            //    smoothMovement = (Vector3)stream.ReceiveNext();
            //}
        }
    }
}