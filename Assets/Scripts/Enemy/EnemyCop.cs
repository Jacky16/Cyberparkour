using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCop : Enemy
{
    [Header("Laser Settings")]
    [SerializeField] LineRenderer laserRenderer;
    [SerializeField] Transform laserPos;
    [SerializeField] LayerMask layerMaskLaser;
    [SerializeField] Vector3 offset;

    [SerializeField]EnemySound enemySound;

    protected override void ChasePlayer()
    {

        transform.LookAt(player);
        //LineRendererAimToPlayer();

    }
    protected override void AttackPlayer()
    {
        base.AttackPlayer();
        //LineRendererAimToPlayer();

        if(playerInAttackRange)
        enemySound.PlaySniperShoot();
    }

    void LineRendererAimToPlayer()
    {
        RaycastHit hit;
        Vector3 dir = (player.position - laserPos.position).normalized;

        if (Physics.Raycast(laserPos.position,dir, out hit, Mathf.Infinity,layerMaskLaser))
        {
            Debug.DrawRay(laserPos.position, dir, Color.blue, 1);
            Debug.Log(hit.collider.transform.parent.name);
            laserRenderer.SetPosition(0, laserPos.position);
            laserRenderer.SetPosition(1, hit.point + offset);

        }
    }
}
