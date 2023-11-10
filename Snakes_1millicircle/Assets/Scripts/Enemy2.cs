using Gameplay.ShipSystems;
using System.Collections;
using UnityEngine;
public enum TypeEnemyAttack
{
    tolchock,
    fire
}
public class Enemy2 : MonoBehaviour
{
    private Coroutine _watching;

    [SerializeField] private DetectionArea DetectionArea;

    [SerializeField] private float Distance = 1f;
    [SerializeField] private float FieldOfView = 30;
    [SerializeField] private Vector3 Center = Vector3.zero;
    public string targetTag = "Player";
    public int rays = 6;
    public int distance = 15;
    public float angle = 20;
    public Vector3 offset;
    private Transform target;
    [SerializeField]
    private Transform _transform;
    #region fps1
    [SerializeField] LayerMask layerFps1;
    [SerializeField] LayerMask layerMaskFps2;
    protected TypeEnemyAttack CurrentAttack;
    //public AttackDefinition attack;
    private float timeOfLastAttack;
    private bool playerIsAlive;
    public float patrolTime = 10;
    public float aggroRange = 6.6f;
    public Transform[] waypoints;
    int index;
    public UnityEngine.AI.NavMeshAgent agent;
    float speed, agentSpeed;
    Transform player;
    #endregion
    private void Start()
    {
        DetectionArea = GetComponent<DetectionArea>();
        timeOfLastAttack = float.MinValue;
        playerIsAlive = true;
        index = Random.Range(0, waypoints.Length);
        if (_transform == null)
            _transform = gameObject.GetComponent<Transform>();
        if (DetectionArea != null)
            DetectionArea.Create(Distance, Center);
        target = GameObject.FindGameObjectWithTag(targetTag).transform;
        player = GameObject.FindGameObjectWithTag(targetTag).transform;
        //agent.destination = waypoints[index].position;
        //agent.speed = agentSpeed / 2; print(agent.destination);
        if (waypoints.Length > 0)
        {
            InvokeRepeating("Patrol", Random.Range(0, 0.8f), 10f);
        }
        InvokeRepeating("tick2", 0, 0.5f);
    }
    public void Tick()
    {


    }
    public void WatchDetect()
    {

        //yield return new WaitForFixedUpdate();
    }
    void Patrol()
    {
        index = index == waypoints.Length - 1 ? 0 : index + 1;
    }
    void tick2()
    {
        var currentPosition = _transform.position;
        foreach (var detectableObjects in DetectionArea.GetDetectableInArea())
        {

            var direction = (detectableObjects.GetPosition() - currentPosition);
            var direction2 = (target.position - currentPosition);
            var distance = Vector3.Distance(detectableObjects.GetPosition(), currentPosition);

            if (!Physics.Raycast(currentPosition, direction, out var hit, distance))
            {
                detectableObjects.UnDetect();

                continue;
            }

            if (hit.transform.gameObject != detectableObjects.GetGameObject())
            {
                detectableObjects.UnDetect();
                continue;
            }

            if (!(Vector3.Angle(_transform.right, direction.normalized) < FieldOfView / 2f))
            {
                detectableObjects.UnDetect();
                continue;
            }
            if (MMDebug.Raycast3DBoolean(currentPosition, direction2, GetComponent<SphereDetectionArea>().distance, layerMaskFps2, Color.cyan))
            {
            }
            else if (MMDebug.Raycast3DBoolean(currentPosition, direction2, GetComponent<SphereDetectionArea>().distance, layerFps1, Color.cyan))
            {
                _transform.LookAt(target);
                if (GetComponent<WeaponSystem>() != null)
                {
                    GetComponent<WeaponSystem>().TriggerFire(); print("tick");
                }

                detectableObjects.Detect();

            }
            else
            {
            }
        }
    }
}