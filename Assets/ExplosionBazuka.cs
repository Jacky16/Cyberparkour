using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionBazuka : MonoBehaviour
{
    
    private void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent(out HealthEnemy _he)){
            _he.InstantDeath();
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.TryGetComponent(out HealthEnemy _he))
        {
            _he.InstantDeath();
        }
    }
}
