using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSliding : MonoBehaviour
{
    [Header("Slide Settings")]
    [SerializeField] float heightInSliding = 1;
    [SerializeField] float timeBtwSliding = 1;
    [SerializeField] float slideVelocity = 20;
    [SerializeField] float slideDrag = 8;
    [SerializeField] float minVelToSlide = 2;
    float normalDrag;
    bool isSliding;
    float normalHeight;

    Rigidbody rb;
    WallRun wallRun;
    PlayerMovement playerMovement;
    [SerializeField]
    CapsuleCollider capsuleCollider;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        wallRun = GetComponent<WallRun>();
        playerMovement = GetComponent<PlayerMovement>();
        capsuleCollider = GetComponentInChildren<CapsuleCollider>();
    }
    private void Start()
    {
        normalHeight = capsuleCollider.height;
        normalDrag = playerMovement.GetGroundDrag();
    }
    
    public void Slide()
    {
        if (!capsuleCollider) return;
        if (playerMovement.isGrounded && !wallRun.isWallRuning && rb.velocity.magnitude >= minVelToSlide)
        {
            if (!isSliding)
            {
                StartCoroutine(SlideCoroutine());
            }
        }
    }

    public void Crouch(bool _isHoldingKey)
    {
        if (!capsuleCollider) return;

        if (_isHoldingKey)
        {
            capsuleCollider.height = heightInSliding;
        }
        else
        {
            capsuleCollider.height = normalHeight;
        }
    }


    IEnumerator SlideCoroutine()
    {
        isSliding = true;

        rb.drag = slideDrag;

        capsuleCollider.height = heightInSliding;

        rb.AddForce(Camera.main.transform.forward * slideVelocity, ForceMode.VelocityChange);

        yield return new WaitForSeconds(timeBtwSliding);

        rb.drag = normalDrag;
        isSliding = false;

        capsuleCollider.height = normalHeight;

    }



}
