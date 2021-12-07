using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointManager : MonoBehaviour
{
    Vector3 checkpointPos;
    Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }
    private void Start()
    {
        checkpointPos = transform.position;
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            GoToCheckpoint();
        }
    }

    public void GoToCheckpoint()
    {
        transform.position = checkpointPos;
        rb.velocity = Vector3.zero;
    }
    public void SetCheckpoint(Transform _pos)
    {
        checkpointPos = _pos.position;
    }
}
