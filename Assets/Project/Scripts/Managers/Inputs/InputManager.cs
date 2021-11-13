using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    //Referencias
    PlayerMovement playerMovement;
    PlayerLook playerLook;
    WallRun wallRun;
    private void Awake()
    {
        GameObject playerGO = GameObject.FindGameObjectWithTag("Player");

        playerMovement = playerGO.GetComponent<PlayerMovement>();
        playerLook = playerGO.GetComponentInChildren<PlayerLook>();
        wallRun = playerGO.GetComponentInChildren<WallRun>();

    }
    public void OnMovement(InputAction.CallbackContext ctx)
    {
        Vector2 axis = ctx.ReadValue<Vector2>();
        playerMovement.SetAxis(axis);
    }
    public void OnJump(InputAction.CallbackContext ctx)
    {
        if (ctx.started)
        {
            playerMovement.Jump();
            wallRun.WallJump();
        }
    }
    public void OnSprint(InputAction.CallbackContext ctx)
    {

        bool holding = ctx.ReadValueAsButton();
        playerMovement.SetInputSprint(holding);
    }
    public void OnMouseMovement(InputAction.CallbackContext ctx)
    {
        Vector2 axis = ctx.ReadValue<Vector2>();
        playerLook.MouseMovement(axis);
    }
    
}
