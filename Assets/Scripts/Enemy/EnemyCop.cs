using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCop : Enemy
{

    protected override void ChasePlayer()
    {

        transform.LookAt(player);
    }
    protected override void AttackPlayer()
    {
    }
}
