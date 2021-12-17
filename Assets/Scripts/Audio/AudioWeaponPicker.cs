using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class AudioWeaponPicker : MonoBehaviour
{
     AudioSource audioSource;
    [SerializeField] AudioClip[] audiosPickup;
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void PlayAudioPickUpWeapon()
    {
        if (!audioSource.isPlaying)
        {
            int rand = Random.Range(0, audiosPickup.Length);
            audioSource.DOFade(1, .25f).OnStart(() => audioSource.PlayOneShot(audiosPickup[rand]));
        }
    }
}
