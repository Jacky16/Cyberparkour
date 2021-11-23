using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickeableWeapon : PickeableObject
{
    Rigidbody rbPlayer;
    [SerializeField] float dropDownForce = 15;
    [SerializeField] float dropUpForce = 7;

    public bool isEqquiped { get; private set; }
    [SerializeField] WeaponManager weaponManager;
    Weapon weapon;
    Animator animWeapon;

    Collider coll;
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        coll = GetComponent<Collider>();
        rbPlayer = GameObject.FindGameObjectWithTag("Player").GetComponent<Rigidbody>();
        rb.collisionDetectionMode = CollisionDetectionMode.ContinuousSpeculative;
        weapon = GetComponent<Weapon>();
        animWeapon = GetComponent<Animator>();

        if (!isEqquiped)
        {
            rb.useGravity = true;
            rb.isKinematic = false;
            coll.isTrigger = false;
            weapon.enabled = false;
            animWeapon.enabled = false;
        }
        else
        {
            coll.isTrigger = true;
            rb.isKinematic = true;
            weapon.enabled = true;
            animWeapon.enabled = true;

            rb.collisionDetectionMode = CollisionDetectionMode.Discrete;

        }


    }
    public override void Pick()
    {
        weapon.enabled = true;
        animWeapon.enabled = true;

        rb.collisionDetectionMode = CollisionDetectionMode.Discrete;

        isEqquiped = true;
        transform.SetParent(weaponManager.transform);

        transform.localPosition = Vector3.zero;
        transform.localEulerAngles = Vector3.zero;

        rb.isKinematic = true;
        coll.isTrigger = true;
    }

    public override void Drop()
    {
        weapon.enabled = false;
        animWeapon.SetTrigger("Reset");
        animWeapon.enabled = false;


        isEqquiped = false;

        transform.SetParent(null);

        rb.isKinematic = false;
        coll.isTrigger = false;

        rb.velocity = rbPlayer.velocity;

        rb.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
        Transform camTransform = Camera.main.transform;
        rb.AddForce(camTransform.forward * dropDownForce, ForceMode.Impulse);
        rb.AddForce(camTransform.up * dropUpForce, ForceMode.Impulse);

        float random = Random.Range(-1f, 1f);

        rb.AddTorque(new Vector3(random, random, random) * 10);

        weaponManager.SetNullCurrentWeapon();
    }

    public void SetEquiped(bool _b)
    {
        isEqquiped = _b;
    }

   
}
