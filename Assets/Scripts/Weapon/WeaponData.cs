using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "Weapon", order = 1)]
public class WeaponData : ScriptableObject
{

    [Header("Settings Bullet")]
    public float shootForce;
    public float upwardForce;
    public float timeTodestroy;
    public bool killInOneShoot;
    public float damage;

    [Header("Raycast Shoot")]
    public bool shootRaycast;
    public float distanceShoot;


    [Header("Settings Shoot")]
    public float timeBetweeShooting;
    public float spread;
    public float reloadTime;
    public float timeBetweeShots;
    public int magazineSize;
    public int totalAmmo;

    [Header("Prefabs")]
    public GameObject explosionPrefab;
    public GameObject bulletPrefab;

    [Header("Audios")]
    public AudioClip[] reloadAudio;
    public AudioClip[] shootsAudio;

}



