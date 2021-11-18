using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponPicker : MonoBehaviour
{
    [SerializeField] Transform weaponHolder;
    [SerializeField] float dropForce = 10;
    [SerializeField] WeaponManager weaponManager;

    GameObject weaponToPickUp;
    Rigidbody rbPlayer;

    private void Awake()
    {
        rbPlayer = GetComponentInParent<Rigidbody>();
    }
    private void Start()
    {
        weaponManager.SetCurrentWeapon(GetCurrentWeaponGameObject().GetComponent<Weapon>());
    }

    public void PickUp()
    {
        if (weaponToPickUp)
        {
            Drop();

            Pick();       
        }    
    }
    void Pick()
    {
        //Emparentar el arma
        weaponToPickUp.transform.SetParent(weaponHolder);
        //Poner las posiciones y rotaciones a 0
        weaponToPickUp.transform.localPosition = Vector3.zero;
        weaponToPickUp.transform.localRotation = Quaternion.Euler(0, 0, 0);

        weaponToPickUp.GetComponent<Rigidbody>().isKinematic = true;

        Weapon wp = weaponToPickUp.GetComponent<Weapon>();
        weaponManager.SetCurrentWeapon(wp);
    }
    void Drop()
    {
        //Obtener el Rigidbody de la arma dropeada
        Rigidbody rbDroppedWeapon = GetCurrentWeaponGameObject().GetComponent< Rigidbody>();
        rbDroppedWeapon.isKinematic = false;
        rbDroppedWeapon.useGravity = true;

        //Desenparentarla
        GetCurrentWeaponGameObject().transform.SetParent(null);

        //Añadir una fuerza hacia delante (Lanzarla)
        rbPlayer.velocity = rbPlayer.velocity; 
        rbDroppedWeapon.AddForce(Camera.main.transform.forward * dropForce, ForceMode.Impulse);
    }
    GameObject GetCurrentWeaponGameObject()
    {
        return weaponHolder.GetChild(0).gameObject;
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Weapon"))
        {
            weaponToPickUp = other.gameObject;
        }
    }
   
}
