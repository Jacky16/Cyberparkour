using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySound : MonoBehaviour
{
    [SerializeField] AudioClip[] audioSteps;
    [SerializeField] AudioClip[] audioShoots;

    AudioSource audioSource;
    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }
    void PlayRandomStep()
    {
        int rand = Random.Range(0, audioSteps.Length);
        audioSource.PlayOneShot(audioSteps[rand]);
    }
    void PlayRandomShoot()
    {
        int rand = Random.Range(0, audioShoots.Length);
        audioSource.PlayOneShot(audioShoots[rand]);
    }
}
