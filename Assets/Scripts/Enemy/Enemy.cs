using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using SensorToolkit;

public class Enemy : MonoBehaviour
{
    protected Transform player;
    protected NavMeshAgent agent;

    [Header("Settings Velocity")]
    [SerializeField] protected float velocityPatrolling = 2.5f;
    [SerializeField] protected float velocityChasing = 5f;
    [SerializeField]
    protected LayerMask whatIsPLayer;
    protected float currentSpeed = 0;

    [Header("Patrolling Settings")]
    [SerializeField] protected bool isIdle;
    [SerializeField] protected Transform[] walkPoints;
    [SerializeField] protected float timeBetweenPoint = 1;
    protected struct StartTransforms
    {
        public Vector3 position;
        public Quaternion rotation;
    }
    protected StartTransforms startTransforms;
    Vector3 startPosition;
    bool canPatrolling = true;
    int nextPoint = 0;
   
    [Header("Attack Settings")]
    [SerializeField] protected float timeBetweenAttacks;
    protected bool alredyAttack;

    [Header("Ranges Settings")]
    [SerializeField] protected float sightRange;
    [SerializeField] protected float attackRange;
    protected bool playerInSightRange, playerInAttackRange;

    [Header("FOV Settings")]
    [SerializeField] protected bool useFOV;
    protected TriggerSensor triggerSensor;
    protected FOVCollider fOVCollider;
    protected bool isInFOV;

    //Components
    protected Animator anim;
    protected EnemyShoot enemyShoot;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        enemyShoot = GetComponent<EnemyShoot>();
        triggerSensor = GetComponent<TriggerSensor>();
        fOVCollider = GetComponent<FOVCollider>();
    }
    private void Start()
    {
        startTransforms.position = transform.position;
        startTransforms.rotation = transform.rotation;
    }
    private void Update()
    {
        Checkers();
        AnimationsSetters();
        StateMachine();
    }

    private void AnimationsSetters()
    {
        anim.SetFloat("Speed", agent.velocity.magnitude);
        if(useFOV)
            anim.SetBool("PlayerInRange", playerInAttackRange && isInFOV);
        else
            anim.SetBool("PlayerInRange", playerInAttackRange);
    }
   
    private void Checkers()
    {
        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPLayer);
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPLayer);
        isInFOV = triggerSensor.GetDetectedByName(player.GetChild(0).name).Contains(player.GetChild(0).gameObject);
        agent.speed = currentSpeed;
    }

    private void StateMachine()
    {
        //Patrol
        if (!playerInSightRange && !playerInAttackRange) Patroling();
      
        if (useFOV)
        {
            //Chase
            if (playerInSightRange && !playerInAttackRange && isInFOV) ChasePlayer();
            //Attack
            if (playerInAttackRange && playerInSightRange && isInFOV) AttackPlayer();
        }
        else
        {
            //Chase
            if (playerInSightRange && !playerInAttackRange) ChasePlayer();

            //Attack
            if (playerInAttackRange && playerInSightRange) AttackPlayer();
        }
    }

    protected virtual void Patroling()
    {
        if (!isIdle)
        {
            currentSpeed = velocityPatrolling;
            Vector3 toGo = walkPoints[nextPoint].position;
            if (canPatrolling)
            {
                agent.SetDestination(toGo);
            }

            float distanceToNextPoint = Vector3.Distance(transform.position, toGo);
            if (distanceToNextPoint < agent.stoppingDistance)
            {
                currentSpeed = 0;
                StartCoroutine(SearchWalkPoint());
            }
        }
        else
        {
            agent.SetDestination(startTransforms.position);

            //Idle
            if(agent.velocity.magnitude <= 0)
            transform.rotation = Quaternion.Slerp(transform.rotation, startTransforms.rotation, 5 * Time.deltaTime);
        }
    }
    IEnumerator SearchWalkPoint()
    {
        canPatrolling = false;
        int maxWalkPoints = walkPoints.Length;
        nextPoint++;
        if(nextPoint >= maxWalkPoints)
            nextPoint = 0;
        yield return new WaitForSeconds(timeBetweenPoint);
        canPatrolling = true;
             
    }
    protected virtual void ChasePlayer()
    {
        currentSpeed = velocityChasing;
        agent.SetDestination(player.position);
    }
    protected virtual void AttackPlayer()
    {
        currentSpeed = 0;
        agent.SetDestination(transform.position);

        if (Vector3.Distance(transform.position, player.position) > 3)
            transform.LookAt(player);
     

        if (!alredyAttack)
        {
            alredyAttack = true;
            StartCoroutine(Attack());
        }
    }
    IEnumerator Attack()
    {
        //Attack
        anim.SetTrigger("Shoot");
        yield return new WaitForSeconds(timeBetweenAttacks);
        alredyAttack = false;
    }

    public void ShootPlayer()
    {
        //Se ejecuta en el evento de la animacion
        enemyShoot.Shoot(player);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, sightRange);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);

        if (isIdle) return;
        for(int i = 0; i < walkPoints.Length; i++)
        {
            Gizmos.color = Color.blue;
            if(i < walkPoints.Length - 1)
            Gizmos.DrawLine(walkPoints[i].position, walkPoints[i + 1].position);
            else
            Gizmos.DrawLine(walkPoints[walkPoints.Length - 1].position, walkPoints[0].position);

            Gizmos.DrawSphere(walkPoints[i].position, .5f);
        }
    }


}
