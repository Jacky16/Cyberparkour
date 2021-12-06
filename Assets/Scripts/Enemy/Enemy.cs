using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI; 

public class Enemy : MonoBehaviour
{
    NavMeshAgent agent;
    Transform player;

    [Header("Settings Velocity")]
    [SerializeField] float velocityPatrolling = 2.5f;
    [SerializeField] float velocityChasing = 5f;
    [SerializeField]
    LayerMask whatIsPLayer;
    float currentSpeed = 0;

    [Header("Patrolling Settings")]
    [SerializeField] bool isIdle;
    [SerializeField] Transform[] walkPoints;
    [SerializeField] float timeBetweenPoint = 1;
    bool canPatrolling = true;
    int nextPoint = 0;
   
    [Header("Attack Settings")]
    [SerializeField] float timeBetweenAttacks;
    bool alredyAttack;

    [Header("Ranges Settings")]
    [SerializeField] float sightRange;
    [SerializeField] float attackRange;
    bool playerInSightRange, playerInAttackRange;

    //Components
    Animator anim;
    EnemyShoot enemyShoot;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        enemyShoot = GetComponent<EnemyShoot>();
    }
    private void Update()
    {
        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPLayer);
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPLayer);
        agent.speed = currentSpeed;
        anim.SetFloat("Speed", agent.velocity.magnitude);
        //Patrol
        if (!playerInSightRange && !playerInAttackRange && !isIdle) Patroling();

        //Chase
        if (playerInSightRange && !playerInAttackRange) ChasePlayer();

        //Attack
        if (playerInAttackRange && playerInSightRange) AttackPlayer();
    }
    void Patroling()
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
    void ChasePlayer()
    {
        currentSpeed = velocityChasing;
        agent.SetDestination(player.position);
    }
    void AttackPlayer()
    {
        currentSpeed = 0;
        agent.SetDestination(transform.position);
        Vector3 dir = player.position - transform.position;

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
