using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponPicker : MonoBehaviour
{
    [SerializeField] Transform weaponHolder;
    [SerializeField] float dropForce = 10;
    Rigidbody rbPlayer;

    private void Awake()
    {
        rbPlayer = GetComponentInParent<Rigidbody>();
    }

    public void PickUp(Transform _weapon)
    {
        //Emparentar el arma
        _weapon.transform.SetParent(weaponHolder);
        //Poner las posiciones y rotaciones a 0
        _weapon.transform.localPosition = Vector3.zero;
        _weapon.transform.localRotation = Quaternion.Euler(0, 0, 0);

        _weapon.GetComponent<Rigidbody>().isKinematic = true;
       
      
    }
    public void Drop(float _dropForce = 10)
    {
        //Obtener el Rigidbody de la arma dropeada
        Rigidbody rbDroppedWeapon = weaponHolder.GetComponentInChildren<Rigidbody>();
        rbDroppedWeapon.isKinematic = false;
        rbDroppedWeapon.useGravity = true;

        //Desenparentarla
        weaponHolder.GetChild(0).SetParent(null);

        //Añadir una fuerza hacia delante (Lanzarla)
        rbPlayer.velocity = rbPlayer.velocity; 
        rbDroppedWeapon.AddForce(Camera.main.transform.forward * _dropForce, ForceMode.Impulse);
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Weapon"))
        {
            print("hola");
            if (Input.GetKeyDown(KeyCode.E))
            {
                //Lanzar el arma actual
                Drop(dropForce);
 
                //Recoger el arma del suelo
                PickUp(other.transform);

            }
        }
    }
   
}
