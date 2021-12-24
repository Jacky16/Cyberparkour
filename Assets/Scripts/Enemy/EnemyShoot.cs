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
    [SerializeField] float damage;

    bool isShooting, isReadyToShoot, isReloading;
 
    public void Shoot(Transform toShoot)
    {
        if (spawnPoint)
        {
            RaycastHit hit;
            Vector3 dir = toShoot.position - spawnPoint.position;

            if (Physics.Raycast(spawnPoint.position,dir, out hit, Mathf.Infinity))
            {
                //Play Sound

                Debug.DrawRay(spawnPoint.position, dir, Color.green,5);
                //Do Anim Shoot



                //Calcular la dirección de l punto A y B


                if (bulletPrefab)
                {
                    //Instanciar el bullet
                    GameObject currentBullet = Instantiate(bulletPrefab, spawnPoint.position, Quaternion.identity, null);
                    currentBullet.transform.forward = dir.normalized;
                    currentBullet.GetComponent<Bullet>().InitBullet(5, damage);
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
