using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponPicker : MonoBehaviour
{
    [SerializeField] WeaponManager weaponManager;

    [SerializeField] PickeableWeapon weaponToPickUp;

    public void PickUp()
    {
        if (weaponToPickUp && weaponManager.CanPickUp())
        {
            if (!weaponToPickUp.isEqquiped)
            {
                Weapon weaponToPickup = weaponToPickUp.GetComponent<Weapon>();
                weaponToPickUp.Pick();
                weaponManager.SetCurrentWeapon(weaponToPickup);
                weaponToPickUp = null;
            }
        }
    }

    public void Drop()
    {
        if(weaponManager.currentWeapon.TryGetComponent<PickeableWeapon>(out PickeableWeapon  _currentPickWeapon))
        {
            if (_currentPickWeapon.isEqquiped)
            {
                _currentPickWeapon.Drop();
                weaponManager.SelectFirst();
            } 

        }
    }
  
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Weapon"))
        {
            //weaponToPickUp = other.gameObject;
        }
        if (other.TryGetComponent<PickeableWeapon>(out PickeableWeapon _wpicker))
        {
            if(!_wpicker.isEqquiped)
            weaponToPickUp = _wpicker;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent<PickeableWeapon>(out PickeableWeapon _wpicker))
        {
            if (!_wpicker.isEqquiped)
                weaponToPickUp = null;
        }
    }
  

}
