using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    //Referencias
    [Header("Referencias")]
    [SerializeField] WeaponManager weaponManager;
    [SerializeField] WeaponPicker weaponPicker;

    PlayerMovement playerMovement;
    PlayerLook playerLook;
    WallRun wallRun;
    PlayerRewind playerRewind;
    public static bool canMove = true;
    private void Awake()
    {
        InitReferences();
    }

    private void InitReferences()
    {
        GameObject playerGO = GameObject.FindGameObjectWithTag("Player");

        playerMovement = playerGO.GetComponent<PlayerMovement>();
        playerLook = playerGO.GetComponent<PlayerLook>();
        wallRun = playerGO.GetComponent<WallRun>();
        playerRewind = playerGO.GetComponent<PlayerRewind>();
    }

    public void OnMovement(InputAction.CallbackContext ctx)
    {
        Vector2 axis;
        if (canMove)
           axis = ctx.ReadValue<Vector2>();
        else
            axis = Vector2.zero;
        playerMovement.SetAxis(axis);
    }
    public void OnJump(InputAction.CallbackContext ctx)
    {
        if (!canMove) return;
        if (ctx.started)
        {
            playerMovement.Jump();
            wallRun.WallJump();
        }
    }
    public void OnMouseWheel(InputAction.CallbackContext ctx)
    {
        if (!canMove) return;
        if (ctx.performed)
        weaponManager.SetMouseAxis(ctx.ReadValue<Vector2>());
    }
    public void OnShoot(InputAction.CallbackContext ctx)
    {
        if (!canMove) return;
        if (ctx.started)
        {
            if (weaponManager)
                weaponManager.ShootWeapon();
            else
                Debug.LogError("Falta setear el WeaponManager en el InputManager");

        }
    }
    public void OnPickUp(InputAction.CallbackContext ctx)
    {
        if (!canMove) return;
        if (ctx.started)
        {
            if (weaponPicker)
            {
                weaponPicker.PickUp();         
            }
            else
                Debug.LogError("Falta setear el WeaponPicker en el InputManager");
        }    
    }
    public void OnDrop(InputAction.CallbackContext ctx)
    {
        if (!canMove) return;
        if (ctx.started)
        {
            if (weaponPicker)
            {
                weaponPicker.Drop();
            }
            else
                Debug.LogError("Falta setear el WeaponPicker en el InputManager");

        }
    }


    public void OnReload(InputAction.CallbackContext ctx)
    {
        if (!canMove) return;
        if (ctx.started)
        {
            if (weaponManager)
                weaponManager.ReloadWeapon();
            else
                Debug.LogError("Falta setear el WeaponManager en el InputManager");
        }   
    }
    public void OnSlide(InputAction.CallbackContext ctx)
    {
        if (!canMove) return;
        playerMovement.Crouch(ctx.ReadValueAsButton());

        if (ctx.started)
        {
            playerMovement.Slide();
        }
    }

    public void OnDash(InputAction.CallbackContext ctx)
    {
        if (!canMove) return;
        if (ctx.started)
        {
            playerMovement.Dash();
        }
    }
    public void OnRewind(InputAction.CallbackContext ctx)
    {
        if (!canMove) return;
        if (ctx.started)
        {
            playerRewind.StartRewind();
        }
    }



    public void OnMouseMovement(InputAction.CallbackContext ctx)
    {
        Vector2 axis = ctx.ReadValue<Vector2>();
        playerLook.MouseMovement(axis);
    }
    
}
