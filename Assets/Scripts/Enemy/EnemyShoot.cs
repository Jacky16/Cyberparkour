using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShoot : MonoBehaviour
{
    [Header("SpawnPoint")]
    [SerializeField] Transform spawnPoint;

    [Header("Graphics")]
    [SerializeField] ParticleSystem muzzleFlash;
    [SerializeField] GameObject bulletPrefab;

    [Header("Bullet Settings")]
    [SerializeField] float shootForce;


    bool isShooting, isReadyToShoot, isReloading;

    public void Shoot(Transform toShoot)
    {
        if (spawnPoint)
        {
            RaycastHit hit;

            if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, Mathf.Infinity))
            {
                //Play Sound


                //Do Anim Shoot



                //Calcular la dirección de l punto A y B
                Vector3 dir = toShoot.position - spawnPoint.position;


                if (bulletPrefab)
                {
                    //Instanciar el bullet
                    GameObject currentBullet = Instantiate(bulletPrefab, spawnPoint.position, Quaternion.identity, null);

                    currentBullet.transform.forward = dir.normalized;

                    //Añadir fuerza al bullet
                    Rigidbody rbBullet = currentBullet.GetComponent<Rigidbody>();

                    rbBullet.AddForce(dir.normalized * shootForce, ForceMode.Impulse);

                }

                if (muzzleFlash)
                {
                    muzzleFlash.Play();
                }
            }
        }
    }
    
}
