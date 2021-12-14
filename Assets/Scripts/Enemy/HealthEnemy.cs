using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthEnemy : Health
{
    [SerializeField] protected GameObject explosionDeathPrefab;

    protected override void OnDeath()
    {

        InstantiateExplosionVFX();
        Destroy(gameObject);
    }
    public override void InstantDeath()
    {
        base.InstantDeath();
        InstantiateExplosionVFX();
    }

    void InstantiateExplosionVFX()
    {
        if (explosionDeathPrefab)
            Instantiate(explosionDeathPrefab, null);
    }
}
