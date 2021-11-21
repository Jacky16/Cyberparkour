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

    Collider coll;
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        coll = GetComponent<Collider>();
        rbPlayer = GameObject.FindGameObjectWithTag("Player").GetComponent<Rigidbody>();
        rb.collisionDetectionMode = CollisionDetectionMode.ContinuousSpeculative;

        if (!isEqquiped)
        {
            rb.useGravity = true;
            rb.isKinematic = false;
            coll.isTrigger = false;


        }
        else
        {
            coll.isTrigger = true;
            rb.isKinematic = true;
            rb.collisionDetectionMode = CollisionDetectionMode.Discrete;

        }


    }
    public override void Pick()
    {
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
    }

    public void SetEquiped(bool _b)
    {
        isEqquiped = _b;
    }

   
}
