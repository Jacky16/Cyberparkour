using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using TMPro;

public class WeaponManager : MonoBehaviour
{
    [SerializeField] GameObject bulletPrefab;

    [Header("Settings Bullet")]
    [SerializeField] float shootForce;
    [SerializeField] float upwardForce;

    [Header("Settings Weapon")]
    [SerializeField] float timeBetweeShooting;
    [SerializeField] float spread;
    [SerializeField] float reloadTime;
    [SerializeField] float timeBetweeShots;
    [SerializeField] int magazineSize;
    [SerializeField] bool allowButtonHold;

    int bulletsLeft, bulletsShots;

    bool isShooting, isReadyToShoot, isReloading;

    [Header("References")]
    [SerializeField] CinemachineVirtualCamera fpsCam;
    [SerializeField] Transform attackPoint;

    [SerializeField] bool allowInvoke = true;

    [Header("Graphics")]
    [SerializeField] ParticleSystem muzzleFlash;
    [SerializeField] TextMeshProUGUI ammoText;

    private void Awake()
    {
        //LLenar el cargador
        bulletsLeft = magazineSize;
        isReadyToShoot = true;
    }
    private void Update()
    {
        MyInput();
        if (ammoText)
        {
            ammoText.SetText(bulletsLeft + " / " + magazineSize);
        }
    }
    void MyInput()
    {
        
        if (allowButtonHold)
            isShooting = Input.GetKey(KeyCode.Mouse0);
        else
            isShooting = Input.GetKeyDown(KeyCode.Mouse0);

        //Recargar
        if (Input.GetKeyDown(KeyCode.R))
        {
            if (bulletsLeft < magazineSize && !isReloading)
                Reload();
        }
        //Shooting
        if(isReadyToShoot && isShooting && !isReloading && bulletsLeft > 0)
        {
            bulletsShots = 0;

            if (attackPoint)
                Shoot();
            else
                Debug.LogError("Falta el punto de spawn");
        }
    }

    void Shoot()
    {
        isReadyToShoot = false;

        Vector3 centerScreen = new Vector3(0.5f, 0.5f, 0);
        Ray ray = Camera.main.ViewportPointToRay(centerScreen, 0);
        RaycastHit hit;
       
        //Comprobar si choca el raycast
        Vector3 targetPoint;

        if(Physics.Raycast(fpsCam.transform.position,fpsCam.transform.forward,out hit,Mathf.Infinity))
        {
            targetPoint = hit.point;
        }
        else
        {
            Debug.LogError("Mal");

        }


        //Calcular la dirección de l punto A y B
        Vector3 dirWithoutSpread = hit.point - attackPoint.position;

        float x = Random.Range(-spread, spread);
        float y = Random.Range(-spread, spread);

        //Calcular la nueva direccion con el spread

        Vector3 dirWithSpread = dirWithoutSpread + new Vector3(x, y, 0);

        if (bulletPrefab)
        {
            //Instanciar el bullet
            GameObject currentBullet = Instantiate(bulletPrefab, attackPoint.position, Quaternion.Euler(0,90,0),null);
            //Debug.Break();
            currentBullet.transform.forward = dirWithSpread.normalized;

            Debug.DrawLine(fpsCam.transform.position, hit.point, Color.red, 2);

            //Añadir fuerza al bullet
            currentBullet.GetComponent<Rigidbody>().AddForce(dirWithoutSpread.normalized * shootForce, ForceMode.Impulse);
            currentBullet.GetComponent<Rigidbody>().AddForce(fpsCam.transform.up * upwardForce, ForceMode.Impulse);
            
        }

        
        if (muzzleFlash)
        {
            muzzleFlash.Play();
        }

        bulletsLeft--;
        bulletsShots++;

        if (allowInvoke)
        {
            Invoke("ResetShoot", timeBetweeShooting);
            allowInvoke = false;
        }
    }

    void ResetShoot()
    {
        isReadyToShoot = true;
        allowInvoke = true;
    }

    void Reload()
    {
        isReloading = true;
        Invoke("ReloadFinished", reloadTime);
    }

    void ReloadFinished()
    {
        bulletsLeft = magazineSize;
        isReloading = false;
    }







}
