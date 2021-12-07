using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    public Weapon currentWeapon { get; private set; }
    [SerializeField] LayerMask layerMaskWeapon;

    int selectedWeapon = 0;

    private void Start()
    {
        InitStartWeapon();
    }

    private void InitStartWeapon()
    {
        if (transform.childCount > 0)
        {
            currentWeapon = transform.GetChild(selectedWeapon).GetComponent<Weapon>();

            foreach (Transform weapon in transform)
            {
                weapon.GetComponent<PickeableWeapon>().SetEquiped(true);
            }
        }
    }

    public void ShootWeapon()
    {
        if(currentWeapon)
        currentWeapon.Shoot(layerMaskWeapon);
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
                currentWeapon = transform.GetChild(i).GetComponent<Weapon>();
                transform.GetChild(i).GetComponent<Weapon>().UpdateText();
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
        if(transform.childCount <= 2)
        {
            currentWeapon = _wp;
            selectedWeapon = currentWeapon.transform.GetSiblingIndex();
            currentWeapon.UpdateText();
            SelectWeapon();
        }
        
    }

    public void SelectFirst()
    {
        selectedWeapon = 0; 
        SelectWeapon();
    }
    public void SetMouseAxis(Vector2 _axisMouseWheel)
    {
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

    public void SetNullCurrentWeapon()
    {
        currentWeapon = null;
    }
    public bool CanPickUp()
    {
        return transform.childCount < 2;
    }
}
