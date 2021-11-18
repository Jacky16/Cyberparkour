using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using TMPro;

public class WeaponManager : MonoBehaviour
{
    Weapon currentWeapon;


    public void ShootWeapon()
    {
        if(currentWeapon)
        currentWeapon.Shoot();
    }

    public void ReloadWeapon()
    {
        if(currentWeapon)
        currentWeapon.Reload();
    }


    public void SetCurrentWeapon(Weapon _wp)
    {
        currentWeapon = _wp;
    }
}
