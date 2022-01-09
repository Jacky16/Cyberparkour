using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponPicker : MonoBehaviour
{
    [SerializeField] WeaponManager weaponManager;
    [SerializeField] PickeableWeapon weaponToPickUp;
    [SerializeField] LayerMask layerMaskWeapon;
    AudioWeaponPicker audioWeaponPicker;

    private void Awake()
    {
        audioWeaponPicker = GetComponent<AudioWeaponPicker>();
    }
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
                audioWeaponPicker.PlayAudioPickUpWeapon();
            }
        }
    }

    public void Drop()
    {
        if(weaponManager.currentWeapon.TryGetComponent<PickeableWeapon>(out PickeableWeapon  _currentPickWeapon) && weaponManager.currentWeapon != null)
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
        if (other.TryGetComponent<PickeableWeapon>(out PickeableWeapon _wpicker))
        {
            if (!_wpicker.isEqquiped)
            {
                weaponToPickUp = _wpicker;
                PickUp();

            }
        }
        else
        {
            weaponToPickUp = null;
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
