using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using TMPro;

public class WeaponManager : MonoBehaviour
{
    Weapon currentWeapon;

    [SerializeField]int selectedWeapon = 0;


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

    

    void SelectWeapon()
    {
        int i = 0;
        foreach(Transform weapon in transform)
        {
            if(selectedWeapon == i)
            {
                weapon.gameObject.SetActive(true);
            }
            else
            {
                weapon.gameObject.SetActive(false);
            }
            i++;
        }
    }
    public void SetCurrentWeapon(Weapon _wp)
    {
        currentWeapon = _wp;
    }
    public void SetMouseAxis(Vector2 _axisMouseWheel)
    {
        print(_axisMouseWheel.y);
        int previousSelected = selectedWeapon;
        if(_axisMouseWheel.y > 0f)
        {
            if(selectedWeapon >= transform.childCount - 1)
            {
                selectedWeapon = transform.childCount - 1;
            }
            else
            {
                selectedWeapon++;
            }
        }

        if (_axisMouseWheel.y < 0f)
        {
            if (selectedWeapon <= transform.childCount - 1)
            {
                selectedWeapon = 0;
            }
            else
            {
                selectedWeapon--;
            }
        }

        if(previousSelected != selectedWeapon)
        {
            SelectWeapon();
        }
    }
}
