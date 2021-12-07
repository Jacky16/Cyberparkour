using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent(out Health _health))
        {

        }
       
        Destroy(gameObject);
    }
   
    private void OnEnable()
    {
        Destroy(gameObject, 5);
    }

}
