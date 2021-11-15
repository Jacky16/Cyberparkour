using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLook : MonoBehaviour
{
    [Header("References")]
    [SerializeField] WallRun wallRun;

    [SerializeField] private float sensX = 100f;
    [SerializeField] private float sensY = 100f;

    [SerializeField] Transform cam = null;
    [SerializeField] Transform orientation = null;

    Vector2 mouseAxis;
    Vector2 rotation;

    float multiplier = 0.01f;
    Color color;
    


    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        mouseAxis.x = Input.GetAxis("Mouse X");
        mouseAxis.y = Input.GetAxis("Mouse Y");

        rotation.y += mouseAxis.x * sensX * multiplier;
        rotation.x -= mouseAxis.y * sensY * multiplier;

        rotation.x = Mathf.Clamp(rotation.x, -90f, 90f);

        cam.transform.localRotation = Quaternion.Euler(rotation.x, rotation.y, wallRun.tilt);
        Debug.Log(rotation.y);
        orientation.transform.localRotation = Quaternion.Euler(0, rotation.y, 0);
    }

    internal void MouseMovement(Vector2 axis)
    {
        //mouseAxis = axis;
    }
}
