using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPlayer : Health
{
    [SerializeField] private CheckpointManager checkpointManager;
    private bool inmortal;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            inmortal = !inmortal;
        }
    }

    protected override void OnDeath()
    {
        if (!inmortal)
            checkpointManager.GoToCheckPoint();
    }
}