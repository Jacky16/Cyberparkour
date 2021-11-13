using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    [SerializeField] Transform orientation;

    [Header("Movement")]
    [SerializeField] float walkSpeed = 4f;
    [SerializeField] float airMultiplier = 0.4f;
    float currentSpeed = 6f;
    float movementMultiplier = 10f;

    [Header("Sprinting")]
    [SerializeField] float sprintSpeed = 6f;
    [SerializeField] float acceleration = 10f;


    [Header("Jumping")]
    public float jumpForce = 5f;


    [Header("Drag")]
    [SerializeField] float groundDrag = 6f;
    [SerializeField] float airDrag = 2f;

    Vector2 axis;

    [Header("Ground Detection")]
    [SerializeField] Transform groundCheck;
    [SerializeField] LayerMask groundMask;
    [SerializeField] float groundDistance = 0.2f;
    public bool isGrounded { get; private set; }
    bool isInputSprint = false;

    Vector3 moveDirection;
    Vector3 slopeMoveDirection;
    float playerHeight = 2f;


    Rigidbody rb;

    RaycastHit slopeHit;

    

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
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

        Sprint();
    }

    public void Jump()
    {
        if (isGrounded)
        {
            rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
            rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
        }
    }

    void Sprint()
    {
        if (isInputSprint && isGrounded)
        {
            currentSpeed = Mathf.Lerp(currentSpeed, sprintSpeed, acceleration * Time.deltaTime);
        }
        else
        {
            currentSpeed = Mathf.Lerp(currentSpeed, walkSpeed, acceleration * Time.deltaTime);
        }
    }

    void ControlDrag()
    {
        if (isGrounded)
        {
            rb.drag = groundDrag;
        }
        else
        {
            rb.drag = airDrag;
        }
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
    public void SetAxis(Vector2 _axis)
    {
        axis = _axis;
    }

    public void SetInputSprint(bool _b)
    {
        isInputSprint = _b;
    }
}