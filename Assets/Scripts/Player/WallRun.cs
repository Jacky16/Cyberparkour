using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class WallRun : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private Transform orientation;

    [Header("Detection")]
    [SerializeField] private float wallDistance = .5f;
    [SerializeField] private float minimumJumpHeight = 1.5f;

    [Header("Wall Running")]
    [SerializeField] private float wallRunGravity;
    [SerializeField] private float wallRunJumpForce;

    [Header("Camera")]
    [SerializeField] private CinemachineVirtualCamera cam;
    [SerializeField] private float fov;
    [SerializeField] private float wallRunfov;
    [SerializeField] private float wallRunfovTimeTransition;
    [SerializeField] private float camTilt;
    [SerializeField] private float camTiltTimeTransition;

    public float tilt { get; private set; }

    private bool wallLeft = false;
    private bool wallRight = false;
    public bool isWallRuning;

    RaycastHit leftWallHit;
    RaycastHit rightWallHit;

    private Rigidbody rb;

    PlayerGlobalVolume playerGlobalVolume;


    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        playerGlobalVolume = GetComponent<PlayerGlobalVolume>();
        fov = cam.m_Lens.FieldOfView;
    }

    private void Update()
    {
        CheckWall();

        WallRunManager();
    }

    private void WallRunManager()
    {
        isWallRuning = CanWallRun() && (wallLeft || wallRight);
        if (playerGlobalVolume)
        {
            playerGlobalVolume.SetVolumeWallRun(isWallRuning);
        }
        if (CanWallRun())
        {
            if (wallLeft)
            {
                StartWallRun();
            }
            else if (wallRight)
            {
                StartWallRun();
            }
            else
            {
                StopWallRun();
            }
        }
        else
        {
            StopWallRun();
        }
    }

    bool CanWallRun()
    {
        return !Physics.Raycast(transform.position, Vector3.down, minimumJumpHeight);
    }
    void CheckWall()
    {
        wallLeft = Physics.Raycast(transform.position, -orientation.right, out leftWallHit, wallDistance);
        wallRight = Physics.Raycast(transform.position, orientation.right, out rightWallHit, wallDistance);
    }

    public void WallJump()
    {
        if (CanWallRun())
        {

            if (wallLeft)
            {
                Vector3 wallRunJumpDirection = transform.up + (leftWallHit.normal / 1.5f);
                rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
                rb.AddForce(wallRunJumpDirection * wallRunJumpForce * 100, ForceMode.Force);
            }
            else if (wallRight)
            {
                Vector3 wallRunJumpDirection = transform.up + (leftWallHit.normal / 1.5f);
                rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
                rb.AddForce(wallRunJumpDirection * wallRunJumpForce * 100, ForceMode.Force);
            }
        }
       
    }

    void StartWallRun()
    {
        rb.useGravity = false;

        //Gravedad propia cuando estas haciendo Wall Running
        rb.AddForce(Vector3.down * wallRunGravity, ForceMode.Force);

        cam.m_Lens.FieldOfView = Mathf.Lerp(cam.m_Lens.FieldOfView, wallRunfov, wallRunfovTimeTransition * Time.deltaTime);

        //Rotacion de camara
        if (wallLeft)
        {
            tilt = Mathf.Lerp(tilt, -camTilt, camTiltTimeTransition * Time.deltaTime);
        }
        else if (wallRight)
        {
            tilt = Mathf.Lerp(tilt, camTilt, camTiltTimeTransition * Time.deltaTime);
        }

       
    }
    void StopWallRun()
    {
        rb.useGravity = true;

        cam.m_Lens.FieldOfView = Mathf.Lerp(cam.m_Lens.FieldOfView, fov, wallRunfovTimeTransition * Time.deltaTime);
        tilt = Mathf.Lerp(tilt, 0, camTiltTimeTransition * Time.deltaTime);
    }
}