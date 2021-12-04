using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI; 

public class Enemy : MonoBehaviour
{
    NavMeshAgent agent;
    Transform player;
    [SerializeField]
    LayerMask whatIsGround;
    [SerializeField]
    LayerMask whatIsPLayer;

    //Patroling
    [SerializeField] bool isIdle;
    [SerializeField] Transform[] walkPoints;
    [SerializeField] float timeBetweenPoint = 1;
    bool canPatrolling = true;
    int nextPoint = 0;
    float counter = 0;
   

    //Attack
    [SerializeField] float timeBetweenAttacks;
    bool alredyAttack;

    //States
    [SerializeField] float sightRange;
    [SerializeField] float attackRange;
    bool playerInSightRange, playerInAttackRange;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        agent = GetComponent<NavMeshAgent>();
    }
    private void Update()
    {
        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPLayer);
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPLayer);

        //Patrol
        if (!playerInSightRange && !playerInAttackRange && !isIdle) Patroling();

        //Chase
        if (playerInSightRange && !playerInAttackRange) ChasePlayer();

        //Attack
        if (playerInAttackRange && playerInSightRange) AttackPlayer();
    }
    void Patroling()
    {
        Vector3 toGo = walkPoints[nextPoint].position;
        if (canPatrolling)
        {
            agent.SetDestination(toGo);
        }

        float distanceToNextPoint = Vector3.Distance(transform.position, toGo);
        if (distanceToNextPoint < agent.stoppingDistance)
        {
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
        agent.SetDestination(player.position);
    }
    void AttackPlayer()
    {
        agent.SetDestination(transform.position);

        transform.LookAt(player);

        Debug.Log("Atacando al player");
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, sightRange);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
