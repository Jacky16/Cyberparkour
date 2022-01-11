using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SensorToolkit;

public class Bullet : MonoBehaviour
{
    private float timeToDestroy;
    private bool killOneShoot;
    private float damage;
    [SerializeField] private GameObject explosionBullet;
    private Vector3 postInstantiateVFX;

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Health _health))
        {
            Damage(_health);
        }
        DoExplosionVFX();
        if (!other.TryGetComponent(out FOVCollider _fc))
            Destroy(gameObject);
    }

    private void OnCollisionEnter(Collision collision)
    {
        print(collision.gameObject.name);

        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<Health>().DoDamage(100);
        }
        DoExplosionVFX();
        Destroy(gameObject);
    }

    private void DoExplosionVFX()
    {
        if (explosionBullet)
            Instantiate(explosionBullet, postInstantiateVFX, transform.rotation, null);
    }

    public void InitVFX(GameObject go, Vector3 _pos)
    {
        explosionBullet = go;
        postInstantiateVFX = _pos;
    }

    private void Damage(Health _health)
    {
        _health.DoDamage(100);

        //print(_health.gameObject.transform.parent.name + " tiene: " + _health.GetHealth());
    }

    public void InitBullet(float _time = 5, float _damage = 0, bool _killOneShoot = false, GameObject _prefabExplosion = null)
    {
        explosionBullet = _prefabExplosion;
        timeToDestroy = _time;
        killOneShoot = _killOneShoot;
        damage = _damage;
        Destroy(gameObject, timeToDestroy);
    }
}