using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CharacterMovementScript : MonoBehaviour
{
    public NavMeshAgent agent;
    private HeroCombat heroCombatScript;

    [SerializeField] private KeyCode stopKeycode;

    public float rotateSpeedMovement = 0.075f;
    public float rotateVelocity;

    public bool abilityCasting = false;

    float prevX;
    float prevZ;
    private bool SetHero = false;
    public void setSetHero()
    {
        SetHero = !SetHero;
    }
    public bool isSettedMyHero()
    {
        return SetHero;
    }
    // Start is called before the first frame update
    void Start()
    {
        // GetComponents Initialization
        SetHero = false;
        agent = gameObject.GetComponent<NavMeshAgent>();
        agent.updateRotation = false;

        heroCombatScript = GetComponent<HeroCombat>();
    }

    // Update is called once per frame
    void Update()
    {
        if (GetComponent<PhotonView>().IsMine)
        {
            CheckStopMovement();

            // Check if the player has a target
            if (heroCombatScript.targetedEnemy != null)
            {
                if (heroCombatScript.targetedEnemy.GetComponent<HeroCombat>() != null)
                {
                    if (!heroCombatScript.targetedEnemy.GetComponent<HeroCombat>().isHeroAlive)
                    {
                        heroCombatScript.targetedEnemy = null;
                    }
                }
            }

            if (Input.GetMouseButtonDown(0))
            {
                RaycastHit hit;

                if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, Mathf.Infinity))
                {
                    if (hit.collider.tag == "Floor"&&SetHero==true)
                    {
                        agent.SetDestination(hit.point); 
                        if (GetComponent<Player>() != null) { GetComponent<Player>().SetmovedDir(hit.point); }
                        heroCombatScript.targetedEnemy = null;
                        agent.stoppingDistance = 0;
                    }
                }
            }
        }
        else if (GameObject.FindGameObjectWithTag("offline") != null)
        {
            CheckStopMovement();
            //print("GetComponent<PhotonView>().IsMine");
            // Check if the player has a target
            if (heroCombatScript.targetedEnemy != null)
            {
                if (heroCombatScript.targetedEnemy.GetComponent<HeroCombat>() != null)
                {
                    if (!heroCombatScript.targetedEnemy.GetComponent<HeroCombat>().isHeroAlive)
                    {
                        heroCombatScript.targetedEnemy = null;
                    }
                }
            }

            if (Input.GetMouseButtonDown(0))
            {
                RaycastHit hit;

                if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, Mathf.Infinity))
                {
                    if (hit.collider.tag == "Floor" && SetHero == true)
                    {
                        // Move player NavAgent to raycast
                        agent.SetDestination(hit.point); //print(hit.point);
                        if(GetComponent<Player>()!=null) { GetComponent<Player>().SetmovedDir(hit.point); }
                        heroCombatScript.targetedEnemy = null;
                        agent.stoppingDistance = 0;
                    }
                }
            }
        }
    }

    // Adjusting player NavAgent rotation
    private void LateUpdate()
    {
        if (agent.velocity.sqrMagnitude > Mathf.Epsilon && heroCombatScript.notCasting)
        {
            transform.localRotation = Quaternion.LookRotation(new Vector3(agent.velocity.normalized.x, 0, agent.velocity.normalized.z));
            prevX = agent.velocity.normalized.x;
            prevZ = agent.velocity.normalized.z;
        }
    }

    IEnumerator ResetStopMovement()
    {
        yield return new WaitForSeconds(0.2f);
        agent.isStopped = false;
        abilityCasting = false;
    }

    void CheckStopMovement()
    {
        if (Input.GetKeyDown(stopKeycode))
        {
            JustStopMovement(false);
        }
    }

    public void JustStopMovement(bool _abilityCasting)
    {
        //heroCombatScript.targetedEnemy = null;
        agent.isStopped = true;
        agent.SetDestination(transform.position);
        abilityCasting = _abilityCasting;

        StartCoroutine(ResetStopMovement());
    }
}
