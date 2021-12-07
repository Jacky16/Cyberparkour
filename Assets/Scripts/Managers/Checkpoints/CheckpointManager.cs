using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class CheckpointManager : MonoBehaviour
{
    Vector3 checkpointPos;
    Transform b;
    Quaternion checkpointRot;
    Rigidbody rb;
    Image imageCanvasFade;
    [SerializeField] float durationFade = .5f;


    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        imageCanvasFade = GameObject.FindGameObjectWithTag("CanvasCheckpoint").GetComponentInChildren<Image>();
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

    IEnumerator GoToCheckpoint()
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
            StartCoroutine(GoToCheckpoint());
        }
    }
}
