using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "Weapon", order = 1)]
public class WeaponData : ScriptableObject
{

    [Header("Settings Bullet")]
    public float shootForce;
    public float upwardForce;
    public float timeTodestroy;

    [Header("Settings Shoot")]
    public float timeBetweeShooting;
    public float spread;
    public float reloadTime;
    public float timeBetweeShots;
    public int magazineSize;
    public bool allowButtonHold;

    [Header("Prefabs")]
    public GameObject explosionPrefab;
    public GameObject bulletPrefab;

    [Header("Audios")]
    public AudioClip shootAudio;

}



