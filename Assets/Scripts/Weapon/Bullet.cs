using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    float timeToDestroy;
    GameObject explosionBullet;
    private void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent(out Health _health))
        {
            print(other.name + " tiene: " + _health.GetHealth());
        }
        if (explosionBullet)
        {
            Instantiate(explosionBullet, transform.position, Quaternion.identity, null);
          
        }

        if(TryGetComponent(out MeshRenderer _mr))
        {
            _mr.enabled = false;
        }
        Destroy(gameObject,1);
    }
    

    public void InitBullet(float _time = 5,GameObject _prefabExplosion = null)
    {
        explosionBullet = _prefabExplosion;
        timeToDestroy = _time;
        Destroy(gameObject, timeToDestroy);
    }
}
