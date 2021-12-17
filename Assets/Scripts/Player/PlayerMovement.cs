using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using DG.Tweening;

public class PlayerMovement : MonoBehaviour
{

    [SerializeField] Transform orientation;
    float playerHeight = 2f;

    [Space]
    [Header("Movement")]
    [SerializeField] float walkSpeed = 4f;
    [SerializeField] float airMultiplier = 0.4f;
    [SerializeField] float acceleration = 10f;
    float currentSpeed = 6f;
    float movementMultiplier = 10f;


    [Space]
    [Header("Jumping")]
    public float jumpForce = 5f;


    [Space]
    [Header("Dashing")]
    [SerializeField] float dashVelocity;
    [SerializeField] float timeBtwDashes;
    [SerializeField] int dashesCount = 3;
    int currentDashes = 0;
    bool isDashing;
    bool canDash = true;


    [Space]
    [Header("Slide")]
    [SerializeField] float slideVelocity = 20;
    [SerializeField] float minVelToSlide = 2;
    [SerializeField] float timeBtwSliding = 1;
    [SerializeField] float heightInSliding = 1;
    bool isCrouching;
    bool isSliding;
    float normalHeight;


    [Space]
    [Header("Drag")]
    [SerializeField] float groundDrag = 6f;
    [SerializeField] float airDrag = 2f;
    [SerializeField] float dashDrag = 10;
    [SerializeField] float slideDrag = 10;


    [Space]
    [Header("Ground Detection")]
    [SerializeField] Transform groundCheck;
    [SerializeField] LayerMask groundMask;
    [SerializeField] float groundDistance = 0.2f;
    public bool isGrounded { get; private set; }

    [Space]
    [Header("Camera Effects")]
    [SerializeField] CinemachineVirtualCamera fpsCam;
    [SerializeField] float amplitudeGain;
    [SerializeField] float FrequencyGain;
    CinemachineBasicMultiChannelPerlin noiseFPSCam;

    //Vectors
    Vector2 axis;
    Vector3 moveDirection;
    Vector3 slopeMoveDirection;


    //Components
    Rigidbody rb;
    CapsuleCollider capsuleCollider;

    //Raycast
    RaycastHit slopeHit;

    //References
    PlayerGlobalVolume playerGlobalVolume;
    WallRun wallRun;
    AudioPlayer audioPlayer;



    private void Awake()
    {
        Init();
    }

    private void Init()
    {
        rb = GetComponent<Rigidbody>();
        playerGlobalVolume = GetComponentInChildren<PlayerGlobalVolume>();
        capsuleCollider = GetComponentInChildren<CapsuleCollider>();
        wallRun = GetComponent<WallRun>();
        noiseFPSCam = fpsCam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        audioPlayer = GetComponent<AudioPlayer>();
        rb.freezeRotation = true;
        normalHeight = capsuleCollider.height;
    }

    private void Update()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        MovementControl();
       

        slopeMoveDirection = Vector3.ProjectOnPlane(moveDirection, slopeHit.normal);
        NoiseCamera();
    }


    private void FixedUpdate()
    {
        MovementPlayer();
    }
   

    #region Normal Movement

    private void MovementControl()
    {
        moveDirection = orientation.forward * axis.y + orientation.right * axis.x;

        ControlDrag();

        //Acceleration
        currentSpeed = Mathf.Lerp(currentSpeed, walkSpeed, acceleration * Time.deltaTime);

    }

    public void Jump()
    {
        if (isGrounded || isSliding)
        {
            audioPlayer.PlayAudioJump();
            rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
            rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
        }
    }

    void ControlDrag()
    {
        if (isDashing)
            rb.drag = dashDrag;

        if (isGrounded && (!isDashing && !isSliding))
            rb.drag = groundDrag;
        
        if ((isGrounded && isSliding && isCrouching) && !isDashing)
            rb.drag = slideDrag;

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
    #endregion

    #region DashLogic
    public void Dash()
    {
        if(currentDashes < dashesCount && canDash)
        {
            StartCoroutine(DashCoroutine());
            StartCoroutine(ResetDash());
        }
    }
    IEnumerator ResetDash()
    {
        yield return new WaitForSeconds(timeBtwDashes);
        currentDashes--;
    }
    IEnumerator DashCoroutine()
    {
        isDashing = true;
        canDash = false;

        audioPlayer.PlayAudioDash();
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

        isDashing = false;
        yield return new WaitForSeconds(timeBtwDashes);
        canDash = true;
        playerGlobalVolume.SetVolumeDash(isDashing);
    }
    #endregion

    #region Slide

    public void Slide()
    {
        if (!capsuleCollider) return;
        if (isGrounded && !wallRun.isWallRuning && rb.velocity.magnitude >= minVelToSlide)
        {
            if (!isSliding)
            {
                StartCoroutine(SlideCoroutine());
            }
        }
    }
    IEnumerator SlideCoroutine()
    {
     
        isSliding = true;

        audioPlayer.PlayAudioSlide();

        capsuleCollider.height = heightInSliding;

        rb.AddForce(Camera.main.transform.forward * slideVelocity, ForceMode.Impulse);


        yield return new WaitForSeconds(timeBtwSliding);

        isSliding = false;
 
    }


    public void Crouch(bool _isHoldingKey)
    {
        playerGlobalVolume.SetVolumeSliding(isSliding);
        isCrouching = _isHoldingKey;
        if (capsuleCollider)
        {
            if (_isHoldingKey)
            {
                capsuleCollider.height = heightInSliding;
                
            }
            else
            {
                capsuleCollider.height = normalHeight;             
            }
        }
    }
    #endregion
    private void NoiseCamera()
    {
        if (rb.velocity.magnitude == 0)
        {
            DOTween.To(() => noiseFPSCam.m_AmplitudeGain, x => noiseFPSCam.m_AmplitudeGain = x, amplitudeGain, 1);
            DOTween.To(() => noiseFPSCam.m_FrequencyGain, x => noiseFPSCam.m_FrequencyGain = x, FrequencyGain, 1);
        }
        else
        {
            DOTween.To(() => noiseFPSCam.m_AmplitudeGain, x => noiseFPSCam.m_AmplitudeGain = x, 0, .5f);
            DOTween.To(() => noiseFPSCam.m_FrequencyGain, x => noiseFPSCam.m_FrequencyGain = x, 0, .5f);
        }
    }

    #region Getters and Setters
    public void SetAxis(Vector2 _axis)
    {
        axis = _axis;
    }

    public float GetGroundDrag()
    {
        return groundDrag;
    }

    public bool IsMoving()
    {
        return axis != Vector2.zero && isGrounded && !wallRun.isWallRuning;
    }
    public bool IsDashing()
    {
        return isDashing;
    }
    public bool IsSliding()
    {
        return isSliding;
    }

    #endregion

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(groundCheck.position, groundDistance);
    }
}