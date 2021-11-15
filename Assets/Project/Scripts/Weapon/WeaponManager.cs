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
        currentWeapon.Shoot();
    }

    public void ReloadWeapon()
    {
        currentWeapon.Reload();
    }


    public void SetCurrentWeapon(Weapon _wp)
    {
        currentWeapon = _wp;
    }
}
