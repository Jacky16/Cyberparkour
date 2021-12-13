using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SensorToolkit;

public class Bullet : MonoBehaviour
{
    float timeToDestroy;
    GameObject explosionBullet;
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Health _health))
        {
            print(other.name + " tiene: " + _health.GetHealth());
        }
        if (explosionBullet)
        {
            Instantiate(explosionBullet, transform.position, Quaternion.identity, null);

        }

     
  
        if(!other.TryGetComponent(out FOVCollider _fc)){

            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {

        if (collision.gameObject.TryGetComponent(out Health _health))
        {
            print(collision.gameObject.name + " tiene: " + _health.GetHealth());

        }
        if (explosionBullet)
        {
            Instantiate(explosionBullet, transform.position, Quaternion.identity, null);

        }

        
        print(collision.gameObject.name);
        GetComponent<Rigidbody>().isKinematic = true;
        Destroy(gameObject);
    }

    public void InitBullet(float _time = 5,GameObject _prefabExplosion = null)
    {
        explosionBullet = _prefabExplosion;
        timeToDestroy = _time;
        Destroy(gameObject, timeToDestroy);
    }
}
