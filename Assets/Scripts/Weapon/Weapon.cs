using System.Collections;
using TMPro;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [Header("Data Weapon")]
    [SerializeField] WeaponData weaponData;

    int bulletsShots;

    bool isReadyToShoot, isReloading;

    int totalAmmo, ammoInCargador;

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
        
        ammoText = GameObject.FindGameObjectWithTag("AmmoText").GetComponent<TextMeshProUGUI>();
        audioSource = GetComponent<AudioSource>();
        anim = GetComponent<Animator>();
        rbPlayer = GameObject.FindGameObjectWithTag("Player").GetComponent<Rigidbody>();
        isReadyToShoot = true;
    }

    private void Start()
    {
        totalAmmo = weaponData.totalAmmo;
        ammoInCargador = weaponData.magazineSize;
        UpdateText();
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
        if (totalAmmo > 0 && !isReloading)
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
            print(totalAmmo + ammoInCargador);
            if (isReadyToShoot && !isReloading && ammoInCargador > 0)
            {
                //bulletsShots = 0;
                StartCoroutine(ShootCoroutine(_layerMaskWeapon));
            }     
        }
        else
            Debug.LogError("Falta el punto de Spawn de la arma");
    }
    
    void ReloadFinished()
    {
        totalAmmo -= bulletsShots;

        if(totalAmmo <= 0)
            totalAmmo = 0;


        ammoInCargador += bulletsShots;
        bulletsShots = 0;
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
            ammoText.SetText(ammoInCargador + " / " + totalAmmo);
        }
        else
        {
            Debug.LogError("Falta setear el texto de la municion");
        }
    }
    bool CheckDataWeapon()
    {
        return weaponData;
    }

    void PlayAudioShoot()
    {
        if (!CheckDataWeapon())
        {
            Debug.LogError("Falta la información del arma");
            return;
        }

        int rand = Random.Range(0, weaponData.shootsAudio.Length);
        audioSource.PlayOneShot(weaponData.shootsAudio[rand]);

    }
    void PlayAudioReload()
    {
        if (!CheckDataWeapon())
        {
            Debug.LogError("Falta la información del arma");
            return;
        }

        int rand = Random.Range(0, weaponData.reloadAudio.Length);
        audioSource.PlayOneShot(weaponData.reloadAudio[rand]);
    }
    #endregion


    #region Coroutinas
    IEnumerator ShootCoroutine(LayerMask _layerMaskWeapon)
    {
        RaycastHit hit;

        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, Mathf.Infinity, _layerMaskWeapon))
        {
            //print(hit.collider.name);
            //Play Sound
            PlayAudioShoot();

            //Do Anim Shoot

            anim.SetTrigger("Shoot");
           

            //Calcular la dirección de l punto A y sasas
            Vector3 dirWithoutSpread = hit.point - spawnPoint.position;

            float spread = weaponData.spread;

            float x = Random.Range(-spread, spread);
            float y = Random.Range(-spread, spread);

            //Calcular la nueva direccion con el spread
            Vector3 dirWithSpread = dirWithoutSpread + new Vector3(x, y, 0);
            if (weaponData.bulletPrefab)
            {
                //Instanciar el bullet
                GameObject currentBullet = Instantiate(weaponData.bulletPrefab, spawnPoint.position, Quaternion.identity, null);
            
                currentBullet.transform.forward = dirWithSpread.normalized;
                
                Debug.DrawLine(Camera.main.transform.position, hit.point, Color.green, 2);

                //Añadir fuerza al bullet
                Rigidbody rbBullet = currentBullet.GetComponent<Rigidbody>();

                //rbBullet.velocity = rbPlayer.velocity;
                rbBullet.AddForce(dirWithoutSpread.normalized * weaponData.shootForce, ForceMode.Impulse);
                rbBullet.AddForce(Camera.main.transform.up * weaponData.upwardForce, ForceMode.Impulse);

                //Asignar el tiempo para destruirlo
                currentBullet.GetComponent<Bullet>().InitBullet(weaponData.timeTodestroy, weaponData.damage, weaponData.killInOneShoot, weaponData.explosionPrefab);

                //Debug.Break();
            }

            if (muzzleFlash)
            {
                muzzleFlash.Play();
            }

            ammoInCargador--;
            bulletsShots++;

            UpdateText();

            //Cadencia
            if (allowShoot)
            {
                isReadyToShoot = false;
                yield return new WaitForSeconds(weaponData.timeBetweeShooting);
                isReadyToShoot = true;
            }
        }
    }
    IEnumerator ReloadCoroutine()
    {
        PlayAudioReload();
        anim.SetTrigger("Reload");
        isReloading = true;
        yield return new WaitForSeconds(weaponData.reloadTime);
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
