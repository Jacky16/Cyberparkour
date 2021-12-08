using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    float timeToDestroy;
    private void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent(out Health _health))
        {
            print(other.name + " tiene: " + _health.GetHealth());
        }
       
        Destroy(gameObject);
    }
   
    public void SetTimeToDestroy(float _time = 5)
    {
        timeToDestroy = _time;
        Destroy(gameObject, timeToDestroy);
    }
}
