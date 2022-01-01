using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    bool canCatch = true;
    private void OnTriggerEnter(Collider other)
    {
        if(other.transform.parent.TryGetComponent(out CheckpointManager _chpM))
        {
            if (canCatch)
            {
                _chpM.SetCheckpoint(transform);
                GetComponentInChildren<ParticleSystem>().startColor = Color.green;
                canCatch = false;
            }
        }
    }
}
