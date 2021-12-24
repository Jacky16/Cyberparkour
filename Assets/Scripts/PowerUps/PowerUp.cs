using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour
{
    [SerializeField] GameObject prefabVFX;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")){
            OnPickUp(other);
        }
    }
   

    protected virtual void OnPickUp(Collider col) {
        
        if(prefabVFX)
        Instantiate(prefabVFX);
    }


}
