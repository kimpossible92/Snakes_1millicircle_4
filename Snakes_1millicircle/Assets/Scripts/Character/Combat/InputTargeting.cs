using Photon.Pun;
using Photon.Realtime;
using PlayerSystem;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class InputTargeting : MonoBehaviourPun, IPunObservable
{
    [SerializeField] private GameObject selectedHero;
    [SerializeField] private bool heroPlayer;
    [SerializeField] private Player GetPlayer;
    RaycastHit hit;

    GameObject targetedEnemyRef;
    GameObject prevEnemyRef;
    GameObject prevObjInteractRef;
    GameObject targetedObjInteractRef;
    TrackEnemyInfo_HUDWindow HUDWindow;

    [SerializeField] private Image HudTargetWindow;

    [SerializeField] private GameObject mouseClick_Prefab;

    [DisplayWithoutEdit] public float mousePosX;
    [DisplayWithoutEdit] public float mousePosY;
    [DisplayWithoutEdit] public float mousePosZ;
    private bool SetSkill = false;
    public string isLocalUserId;
    #region HeroMovement
    //private NavMeshAgent agent;
    //private Vector3 targetDestination;
    //private PhotonView photon;
    //private Animator animator;
    //private AttackManager attackManager;
    #endregion
    public void setUpdate()
    {

    }
    void Start()
    {
        if (!SetSkill) { return; }
        selectedHero = GameObject.FindGameObjectWithTag("MyPlayer");
        HUDWindow = HudTargetWindow.GetComponent<TrackEnemyInfo_HUDWindow>();
    }
    public void SetHero(Image image1)
    {
        selectedHero = this.GetComponentInChildren<HeroClass>().gameObject;
        HudTargetWindow = image1;
        foreach (Player2 p in PhotonNetwork.PlayerList)
        {
            if (p.IsLocal)
            {
                isLocalUserId = p.UserId;
            }
        }
        HUDWindow = HudTargetWindow.GetComponent<TrackEnemyInfo_HUDWindow>();
        SetSkill = true;
        //agent = this.GetComponentInChildren<NavMeshAgent>();
        //photon = this.GetComponentInChildren<PhotonView>();
        //animator = this.GetComponentInChildren<Animator>();
        //targetDestination = transform.position;
        //attackManager = this.GetComponent<AttackManager>();
    }
    // Update is called once per frame
    void Update()
    {
        GetComponent<PhotonView>().RPC("RpcMoveAttack", RpcTarget.AllBuffered);
        if (GameObject.FindGameObjectWithTag("offline") != null)
        {
            RpcMoveAttack();
        }
    }
    public void Follow(GameObject target)
    {
        //agent.Resume();
        //agent.SetDestination(target.transform.position);
        //animator.SetBool("isMoving", true);
    }
    public void Stop()
    {
        //agent.Stop();
        //animator.SetBool("isMoving", false);

    }
    [PunRPC]
    void RpcMoveAttack()
    {
        if (this.GetComponentInChildren<PhotonView>().IsMine)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            //print("ismine");
            // Minion Targeting
            if (Input.GetMouseButtonDown(0))
            {
                if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, Mathf.Infinity))
                {
                    GameObject _temp = Instantiate(mouseClick_Prefab, new Vector3(hit.point.x, hit.point.y, hit.point.z), Quaternion.Euler(-90, -0, 0));

                    // if minion is targetable
                    if (hit.collider.GetComponent<TargetableScript>() != null)
                    {
                        if (hit.collider.gameObject.GetComponent<TargetableScript>().enemyType == TargetableScript.EnemyType.Minion)
                        {
                            if (hit.collider.gameObject != selectedHero.GetComponent<HeroCombat>().targetedEnemy && selectedHero.GetComponent<HeroCombat>().targetedEnemy != null)
                            {
                                selectedHero.GetComponent<HeroCombat>().ResetAutoAttack(false); // fix auto infinite range  ?
                            }

                            selectedHero.GetComponent<HeroCombat>().targetedEnemy = hit.collider.gameObject;
                            selectedHero.GetComponent<HeroCombat>().moveToEnemy = true;

                            if (prevEnemyRef == null)
                            {
                                targetedEnemyRef = selectedHero.GetComponent<HeroCombat>().targetedEnemy;
                                prevEnemyRef = targetedEnemyRef;
                                selectedHero.GetComponent<HeroCombat>().targetedEnemy.GetComponent<Outline>().enabled = true;

                                HUDWindow.GetTargetedEnemy(targetedEnemyRef);
                            }
                            else
                            {
                                prevEnemyRef.GetComponent<Outline>().enabled = false;
                                targetedEnemyRef = selectedHero.GetComponent<HeroCombat>().targetedEnemy;
                                prevEnemyRef = targetedEnemyRef;
                                selectedHero.GetComponent<HeroCombat>().targetedEnemy.GetComponent<Outline>().enabled = true;

                                HUDWindow.GetTargetedEnemy(targetedEnemyRef);
                            }
                        }
                    }

                    else if (hit.collider.gameObject.GetComponent<TargetableScript>() == null)
                    {
                        if (targetedEnemyRef != null)
                            targetedEnemyRef.GetComponent<Outline>().enabled = false;

                        prevEnemyRef = null;
                        targetedEnemyRef = null;
                    }
                }
            }
            // Minion Targeting left-click no move
            if (Input.GetMouseButtonDown(0))
            {
                if (Physics.Raycast(ray, out hit, Mathf.Infinity))
                {

                    GameObject _temp = Instantiate(mouseClick_Prefab, new Vector3(hit.point.x, hit.point.y, hit.point.z), Quaternion.Euler(-90, -0, 0));

                    // if minion is targetable
                    if (hit.collider.GetComponent<TargetableScript>() != null)
                    {
                        if (hit.collider.gameObject.GetComponent<TargetableScript>().enemyType == TargetableScript.EnemyType.Minion)
                        {
                            if (hit.collider.gameObject != selectedHero.GetComponent<HeroCombat>().targetedEnemy)
                            {
                                selectedHero.GetComponent<HeroCombat>().ResetAutoAttack(false); // fix auto infinite range  ?
                            }

                            selectedHero.GetComponent<HeroCombat>().targetedEnemy = hit.collider.gameObject;
                            selectedHero.GetComponent<HeroCombat>().targetedObjectInteract = hit.collider.gameObject;
                            selectedHero.GetComponent<HeroCombat>().moveToEnemy = false;

                            if (prevEnemyRef == null)
                            {
                                targetedEnemyRef = selectedHero.GetComponent<HeroCombat>().targetedEnemy;
                                prevEnemyRef = targetedEnemyRef;
                                selectedHero.GetComponent<HeroCombat>().targetedEnemy.GetComponent<Outline>().enabled = true;

                                HUDWindow.GetTargetedEnemy(targetedEnemyRef);
                            }
                            else
                            {
                                prevEnemyRef.GetComponent<Outline>().enabled = false;
                                targetedEnemyRef = selectedHero.GetComponent<HeroCombat>().targetedEnemy;
                                prevEnemyRef = targetedEnemyRef;
                                selectedHero.GetComponent<HeroCombat>().targetedEnemy.GetComponent<Outline>().enabled = true;

                                HUDWindow.GetTargetedEnemy(targetedEnemyRef);
                            }
                        }
                    }

                    else if (hit.collider.gameObject.GetComponent<TargetableScript>() == null)
                    {
                        if (targetedEnemyRef != null)
                            targetedEnemyRef.GetComponent<Outline>().enabled = false;

                        prevEnemyRef = null;
                        targetedEnemyRef = null;
                        HUDWindow.RemoveTargetedEnemy();
                    }
                }
            }
        }
        else if (GameObject.FindGameObjectWithTag("offline") != null)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            // Minion Targeting
            if (Input.GetMouseButtonDown(0))
            {
                if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, Mathf.Infinity))
                {
                    GameObject _temp = Instantiate(mouseClick_Prefab, new Vector3(hit.point.x, hit.point.y, hit.point.z), Quaternion.Euler(-90, -0, 0));
                    //print("isminenot1");
                    // if minion is targetable
                    if (hit.collider.GetComponent<TargetableScript>() != null)
                    {
                        if (hit.collider.gameObject.GetComponent<TargetableScript>().enemyType == TargetableScript.EnemyType.Minion)
                        {
                            if (hit.collider.gameObject != selectedHero.GetComponent<HeroCombat>().targetedEnemy && selectedHero.GetComponent<HeroCombat>().targetedEnemy != null)
                            {
                                selectedHero.GetComponent<HeroCombat>().ResetAutoAttack(false); // fix auto infinite range  ?
                            }
                            //print("isminenot");
                            selectedHero.GetComponent<HeroCombat>().targetedEnemy = hit.collider.gameObject;
                            selectedHero.GetComponent<HeroCombat>().moveToEnemy = true;

                            if (prevEnemyRef == null)
                            {
                                targetedEnemyRef = selectedHero.GetComponent<HeroCombat>().targetedEnemy;
                                prevEnemyRef = targetedEnemyRef;
                                selectedHero.GetComponent<HeroCombat>().targetedEnemy.GetComponent<Outline>().enabled = true;

                                HUDWindow.GetTargetedEnemy(targetedEnemyRef);
                            }
                            else
                            {
                                //print("outline");
                                prevEnemyRef.GetComponent<Outline>().enabled = false;
                                targetedEnemyRef = selectedHero.GetComponent<HeroCombat>().targetedEnemy;
                                prevEnemyRef = targetedEnemyRef;
                                selectedHero.GetComponent<HeroCombat>().targetedEnemy.GetComponent<Outline>().enabled = true;

                                HUDWindow.GetTargetedEnemy(targetedEnemyRef);
                            }
                        }
                    }

                    else if (hit.collider.gameObject.GetComponent<TargetableScript>() == null)
                    {
                        if (targetedEnemyRef != null)
                            targetedEnemyRef.GetComponent<Outline>().enabled = false;

                        prevEnemyRef = null;
                        targetedEnemyRef = null;
                    }
                }
            }
            // Minion Targeting left-click no move
            if (Input.GetMouseButtonDown(0))
            {
                if (Physics.Raycast(ray, out hit, Mathf.Infinity))
                {

                    GameObject _temp = Instantiate(mouseClick_Prefab, new Vector3(hit.point.x, hit.point.y, hit.point.z), Quaternion.Euler(-90, -0, 0));

                    // if minion is targetable
                    if (hit.collider.GetComponent<TargetableScript>() != null)
                    {
                        if (hit.collider.gameObject.GetComponent<TargetableScript>().enemyType == TargetableScript.EnemyType.Minion)
                        {
                            if (hit.collider.gameObject != selectedHero.GetComponent<HeroCombat>().targetedEnemy)
                            {
                                selectedHero.GetComponent<HeroCombat>().ResetAutoAttack(false); // fix auto infinite range  ?
                            }

                            selectedHero.GetComponent<HeroCombat>().targetedEnemy = hit.collider.gameObject;

                            selectedHero.GetComponent<HeroCombat>().moveToEnemy = false;

                            if (prevEnemyRef == null)
                            {
                                targetedEnemyRef = selectedHero.GetComponent<HeroCombat>().targetedEnemy;
                                prevEnemyRef = targetedEnemyRef;
                                //selectedHero.GetComponent<HeroCombat>().targetedEnemy.GetComponent<Outline>().enabled = true;

                                HUDWindow.GetTargetedEnemy(targetedEnemyRef);
                            }
                            else
                            {
                                prevEnemyRef.GetComponent<Outline>().enabled = false;
                                targetedEnemyRef = selectedHero.GetComponent<HeroCombat>().targetedEnemy;
                                prevEnemyRef = targetedEnemyRef;
                                selectedHero.GetComponent<HeroCombat>().targetedEnemy.GetComponent<Outline>().enabled = true;

                                HUDWindow.GetTargetedEnemy(targetedEnemyRef);
                            }
                        }
                        else if (hit.collider.gameObject.GetComponent<TargetableScript>().enemyType == TargetableScript.EnemyType.any)
                        {
                            if (prevObjInteractRef == null)
                            {
                                targetedObjInteractRef = hit.collider.gameObject.GetComponent<TargetableScript>().gameObject;
                                prevObjInteractRef = targetedObjInteractRef;
                                OnSetBaseCounter(targetedObjInteractRef.GetComponent<BaseCounter>());
                                //selectedHero.GetComponent<HeroCombat>().targetedObjectInteract.GetComponent<Outline>().enabled = true;
                                //print("prevObjInteractRef==null");
                            }
                            else
                            {
                                //prevObjInteractRef.GetComponent<Outline>().enabled = false;
                                targetedObjInteractRef = selectedHero.GetComponent<HeroCombat>().targetedObjectInteract;
                                prevObjInteractRef = targetedObjInteractRef;
                                //print("prevObjInteractRef");
                                //selectedHero.GetComponent<HeroCombat>().targetedObjectInteract.GetComponent<Outline>().enabled = true;
                            }
                        }
                    }
                    
                    else if (hit.collider.gameObject.GetComponent<TargetableScript>() == null)
                    {
                        if (targetedEnemyRef != null)
                            targetedEnemyRef.GetComponent<Outline>().enabled = false;

                        prevEnemyRef = null;
                        targetedEnemyRef = null;
                        HUDWindow.RemoveTargetedEnemy();
                    }
                }
            }
        }
    }
    public void OnSetBaseCounter(BaseCounter baseCounter1)
    {
        baseCounter1.Interact(GetPlayer);
    }
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        //throw new System.NotImplementedException();

        if (stream.IsWriting)
        {
            stream.SendNext(transform.position);
        }
        else if (stream.IsReading && GetComponent<PlayerView>() != null)
        {
            //selectedHero.GetComponent<HeroCombat>().targetedEnemy = (GameObject)stream.ReceiveNext();
        }
    }
}
