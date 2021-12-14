using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SensorToolkit;

public class Bullet : MonoBehaviour
{
    float timeToDestroy;
    bool killOneShoot;
    float damage;
    GameObject explosionBullet;
    private void OnTriggerEnter(Collider other)
    {

        if (other.TryGetComponent(out Health _health))
        {
            Damage(_health);

        }
        DoExplosionVFX();

        if (!other.TryGetComponent(out FOVCollider _fc))
        {

            Destroy(gameObject);
        }
    }


    private void OnCollisionEnter(Collision collision)
    {

        if (collision.gameObject.TryGetComponent(out Health _health))
        {
            Damage(_health);
        }
        DoExplosionVFX();

        Destroy(gameObject);
    }
    private void DoExplosionVFX()
    {
        if (explosionBullet)
        {
            Instantiate(explosionBullet, transform.position, Quaternion.identity, null);
        }
    }

    private void Damage(Health _health)
    {
        if (killOneShoot)
            _health.InstantDeath();
        else
            _health.DoDamage(damage);

        print(_health.gameObject.name + " tiene: " + _health.GetHealth());
    }

    public void InitBullet(float _time = 5,float _damage = 0,bool _killOneShoot = false, GameObject _prefabExplosion = null)
    {
        explosionBullet = _prefabExplosion;
        timeToDestroy = _time;
        killOneShoot = _killOneShoot;
        damage = _damage;
        Destroy(gameObject, timeToDestroy);
    }
}
