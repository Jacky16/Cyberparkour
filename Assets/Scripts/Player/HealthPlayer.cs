using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPlayer : Health
{
    [SerializeField] private CheckpointManager checkpointManager;

    protected override void OnDeath()
    {
        checkpointManager.GoToCheckPoint();
    }
}