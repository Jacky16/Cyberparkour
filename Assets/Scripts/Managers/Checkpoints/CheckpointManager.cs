using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class CheckpointManager : MonoBehaviour
{
    private Vector3 checkpointPos;
    private Transform b;
    private Quaternion checkpointRot;
    private Rigidbody rb;
    private Image imageCanvasFade;
    [SerializeField] private float durationFade = .5f;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        imageCanvasFade = GameObject.FindGameObjectWithTag("CanvasCheckpoint").GetComponentInChildren<Image>();
    }

    private void Start()
    {
        checkpointPos = transform.position;
    }

    public void GoToCheckPoint()
    {
        StartCoroutine(GoToCheckpointCoroutine());
    }

    private IEnumerator GoToCheckpointCoroutine()

    {
        InputManager.canMove = false;
        imageCanvasFade.DOFade(1, durationFade);

        yield return new WaitForSeconds(durationFade + 0.1f);

        transform.position = checkpointPos;
        imageCanvasFade.DOFade(0, durationFade);

        rb.velocity = Vector3.zero;
        InputManager.canMove = true;
    }

    public void SetCheckpoint(Transform _pos)
    {
        b = _pos;
        checkpointPos = _pos.position;
        checkpointRot = _pos.localRotation;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("ToCheckpoint"))
        {
            GoToCheckPoint();
        }
    }
}