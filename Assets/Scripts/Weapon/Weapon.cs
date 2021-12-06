using System.Collections;
using TMPro;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [Header("Data Weapon")]
    [SerializeField] WeaponData WeaponData;

    int bulletsLeft, bulletsShots;

    bool isShooting, isReadyToShoot, isReloading;
    

    [Header("SpawnPoint")]
    [SerializeField] Transform spawnPoint;

    [SerializeField] bool allowShoot = true;

    [Header("Graphics")]
    [SerializeField] ParticleSystem muzzleFlash;
    TextMeshProUGUI ammoText;

    AudioSource audioSource;
    Animator anim;
    Rigidbody rbPlayer;

    private void Awake()
    {
        //LLenar el cargador
        bulletsLeft = WeaponData.magazineSize;
        
        ammoText = GameObject.FindGameObjectWithTag("AmmoText").GetComponent<TextMeshProUGUI>();
        audioSource = GetComponent<AudioSource>();
        anim = GetComponent<Animator>();
        rbPlayer = GameObject.FindGameObjectWithTag("Player").GetComponent<Rigidbody>();
        isReadyToShoot = true;
    }
    private void Update()
    {
        anim.SetBool("IsRunning", rbPlayer.velocity.magnitude > 4);
    }


    #region Funciones
    public void Reload()
    {
        if (!CheckDataWeapon())
        {
            Debug.LogError("Falta la información del arma");
            return;
        }
        if (bulletsLeft < WeaponData.magazineSize && !isReloading)
            StartCoroutine(ReloadCoroutine());
    }
    public void Shoot(LayerMask _layerMaskWeapon)
    {
        if (!CheckDataWeapon())
        {
            Debug.LogError("Falta la información del arma");
            return;
        }
        if (spawnPoint)
        {
            if (isReadyToShoot && !isReloading && bulletsLeft > 0)
            {
                bulletsShots = 0;
                StartCoroutine(ShootCoroutine(_layerMaskWeapon));
            }     
        }
        else
        {
            Debug.LogError("Falta el punto de Spawn de la arma");
        }

    }
    
    void ReloadFinished()
    {
        bulletsLeft = WeaponData.magazineSize;
        isReloading = false;
        UpdateText();
    }
    public void UpdateText()
    {
        if (!CheckDataWeapon())
        {
            Debug.LogError("Falta la información del arma");
            return;
        }

        if (ammoText)
        {
            ammoText.SetText(bulletsLeft + " / " + WeaponData.magazineSize);
        }
        else
        {
            Debug.LogError("Falta setear el texto de la municion");
        }
    }
    bool CheckDataWeapon()
    {
        return WeaponData;
    }

    void PlayAudioShoot()
    {
        if (!CheckDataWeapon())
        {
            Debug.LogError("Falta la información del arma");
            return;
        }

        if (WeaponData.shootAudio)
            audioSource.PlayOneShot(WeaponData.shootAudio);
       
    }
    #endregion


    #region Coroutinas
    IEnumerator ShootCoroutine(LayerMask _layerMaskWeapon)
    {
        RaycastHit hit;

        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, Mathf.Infinity, _layerMaskWeapon))
        {
            //Play Sound
            PlayAudioShoot();

            //Do Anim Shoot

            anim.SetTrigger("Shoot");
           

            //Calcular la dirección de l punto A y B
            Vector3 dirWithoutSpread = hit.point - spawnPoint.position;

            float spread = WeaponData.spread;

            float x = Random.Range(-spread, spread);
            float y = Random.Range(-spread, spread);

            //Calcular la nueva direccion con el spread

            Vector3 dirWithSpread = dirWithoutSpread + new Vector3(x, y, 0);
            if (WeaponData.bulletPrefab)
            {
                //Instanciar el bullet
                GameObject currentBullet = Instantiate(WeaponData.bulletPrefab, spawnPoint.position, Quaternion.identity, null);
            
                currentBullet.transform.forward = dirWithSpread.normalized;
                
                Debug.DrawLine(Camera.main.transform.position, hit.point, Color.green, 2);

                //Añadir fuerza al bullet
                Rigidbody rbBullet = currentBullet.GetComponent<Rigidbody>();

                //rbBullet.velocity = rbPlayer.velocity;
                rbBullet.AddForce(dirWithoutSpread.normalized * WeaponData.shootForce, ForceMode.Impulse);
                rbBullet.AddForce(Camera.main.transform.up * WeaponData.upwardForce, ForceMode.Impulse);

            }

            if (muzzleFlash)
            {
                muzzleFlash.Play();
            }

            bulletsLeft--;
            bulletsShots++;

            UpdateText();

            //Cadencia
            if (allowShoot)
            {
                isReadyToShoot = false;
                yield return new WaitForSeconds(WeaponData.timeBetweeShooting);
                isReadyToShoot = true;
            }
        }
    }
    IEnumerator ReloadCoroutine()
    {
        anim.SetTrigger("Reload");
        isReloading = true;
        yield return new WaitForSeconds(WeaponData.reloadTime);
        ReloadFinished();
    }
    #endregion

    private void OnEnable()
    {
        if (ammoText)
            ammoText.enabled = true;
    }
    private void OnDisable()
    {
        if(ammoText)
            ammoText.enabled = false;
    }

}
