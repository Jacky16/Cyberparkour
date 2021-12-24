using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpHealth : PowerUp
{
    [SerializeField] float health = 50;

    protected override void OnPickUp(Collider col)
    {
        base.OnPickUp(col);
        col.GetComponent<Health>().AddLife(health);
        Destroy(gameObject,1);
    }
} 
    
