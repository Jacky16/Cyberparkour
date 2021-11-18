using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    [SerializeField] Transform orientation;

    [Header("Movement")]
    [SerializeField] float walkSpeed = 4f;
    [SerializeField] float airMultiplier = 0.4f;
    [SerializeField] float acceleration = 10f;
    float currentSpeed = 6f;
    float movementMultiplier = 10f;

    [Header("Jumping")]
    public float jumpForce = 5f;

    [Header("Dashing")]
    [SerializeField] float dashVelocity;
    [SerializeField] float timeBtwDashes;
    [SerializeField] int dashesCount = 3;
    int currentDashes = 0;
    bool isDashing;

    [Header("Drag")]
    [SerializeField] float groundDrag = 6f;
    [SerializeField] float airDrag = 2f;
    [SerializeField] float dashDrag = 10;

    Vector2 axis;

    [Header("Ground Detection")]
    [SerializeField] Transform groundCheck;
    [SerializeField] LayerMask groundMask;
    [SerializeField] float groundDistance = 0.2f;
    public bool isGrounded { get; private set; }
    bool isInputSprint = false;

    Vector3 moveDirection;

    public float GetGroundDrag()
    {
        return groundDrag;
    }

    Vector3 slopeMoveDirection;
    float playerHeight = 2f;


    Rigidbody rb;

    RaycastHit slopeHit;

    PlayerGlobalVolume playerGlobalVolume;
    PlayerSliding playerSliding;
    

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        playerGlobalVolume = GetComponentInChildren<PlayerGlobalVolume>();
        playerSliding = GetComponent<PlayerSliding>();
        rb.freezeRotation = true;
    }

    private void Update()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        MovementControl();

        slopeMoveDirection = Vector3.ProjectOnPlane(moveDirection, slopeHit.normal);
    }
    private void FixedUpdate()
    {
        MovementPlayer();
    }
    private bool OnSlope()
    {
        if (Physics.Raycast(transform.position, Vector3.down, out slopeHit, playerHeight / 2 + 0.5f))
        {
            if (slopeHit.normal != Vector3.up)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        return false;
    }

    private void MovementControl()
    {
        moveDirection = orientation.forward * axis.y + orientation.right * axis.x;

        ControlDrag();

        //Acceleration
        currentSpeed = Mathf.Lerp(currentSpeed, walkSpeed, acceleration * Time.deltaTime);

    }

    public void Jump()
    {
        if (isGrounded || playerSliding.IsSliding())
        {
            rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
            rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
        }
    }

    void ControlDrag()
    {
        if (isDashing)
            rb.drag = dashDrag;

        if (isGrounded && !isDashing)
            rb.drag = groundDrag;
        
        if(!isGrounded && !isDashing)
            rb.drag = airDrag;

    }

    void MovementPlayer()
    {
        //En el suelo sin bajada
        if (isGrounded && !OnSlope())
        {
            rb.AddForce(moveDirection.normalized * currentSpeed * movementMultiplier, ForceMode.Acceleration);
        }
        //En el suelo y en bajada
        else if (isGrounded && OnSlope())
        {
            rb.AddForce(slopeMoveDirection.normalized * currentSpeed * movementMultiplier, ForceMode.Acceleration);
        }
        //En el aire
        else if (!isGrounded)
        {
            rb.AddForce(moveDirection.normalized * currentSpeed * movementMultiplier * airMultiplier, ForceMode.Acceleration);
        }
    }
    public void Dash()
    {
        if(currentDashes < dashesCount)
        {
            StartCoroutine(DashCoroutine());
            StartCoroutine(ResetDash());
        }
    }
    IEnumerator ResetDash()
    {
        yield return new WaitForSeconds(2);
        currentDashes--;
    }
    IEnumerator DashCoroutine()
    {
        isDashing = true;
        playerGlobalVolume.SetVolumeDash(isDashing);
        if(axis == Vector2.zero)
        {
            rb.velocity = Camera.main.transform.forward *  dashVelocity;
        }
        else
        {
            rb.velocity = (moveDirection + Camera.main.transform.forward).normalized *  dashVelocity;
        }
        currentDashes++;

        if (currentDashes >= dashesCount)
            currentDashes = dashesCount;

        yield return new WaitForSeconds(timeBtwDashes);
        isDashing = false;
        playerGlobalVolume.SetVolumeDash(isDashing);
    }
    public void SetAxis(Vector2 _axis)
    {
        axis = _axis;
    }

   
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(groundCheck.position, groundDistance);
    }
}