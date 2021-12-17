using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class AudioPlayer : MonoBehaviour
{
    [SerializeField] AudioClip[] audiosRun;
    [SerializeField] AudioClip[] audiosRewind;
    [SerializeField] AudioClip[] audiosDash;
    [SerializeField] AudioClip[] audiosSlide;
    [SerializeField] AudioClip[] audiosJump;

    [SerializeField]AudioSource audioSource;
    [SerializeField] AudioSource audioSourceRun;

    PlayerMovement playerMovement;
    PlayerRewind playerRewind;
    private void Awake()
    {
        playerMovement = GetComponent<PlayerMovement>();
        playerRewind = GetComponent<PlayerRewind>();
    }
    private void Update()
    {
        if(playerMovement.IsMoving() && !playerMovement.IsDashing() && 
            !playerMovement.IsSliding() && !playerRewind.IsRewinding())
        {
            PlayAudioRun();
        }
        else
        {
            StopAudioRun();
        }
    }

    void PlayAudioRun()
    {
        if (!audioSourceRun.isPlaying)
        {
            int rand = Random.Range(0, audiosRun.Length);
            audioSourceRun.DOFade(1, .25f).OnStart(() => audioSourceRun.PlayOneShot(audiosRun[rand]));
        }
    }
    void StopAudioRun()
    {
        if(audioSourceRun.isPlaying)
        audioSourceRun.DOFade(0, .25f).OnComplete(() => audioSourceRun.Stop());

    }


    public void PlayAudioRewind()
    {
        StopAudio();
        if (!audioSource.isPlaying)
        {
            int rand = Random.Range(0, audiosRewind.Length);
            PlayAudio(audiosRewind[rand],.1f);
            
        }
        
    }
    public void PlayAudioDash()
    {
        StopAudio();
        if (!audioSource.isPlaying)
        {
            int rand = Random.Range(0, audiosDash.Length);
            PlayAudio(audiosDash[rand],1f);
        }
    }

    public void PlayAudioSlide()
    {
        StopAudio();
        if (!audioSource.isPlaying)
        {
            int rand = Random.Range(0, audiosSlide.Length);
            PlayAudio(audiosSlide[rand], 1f);
        }
    }
    public void PlayAudioJump()
    {
        StopAudio();

        if (!audioSource.isPlaying)
        {
            int rand = Random.Range(0, audiosJump.Length);
            PlayAudio(audiosJump[rand], 1f);
        }
    }
    public void StopAudio( bool _dofade = false,float _fadeDuration = 0.25f)
    {
        if (_dofade)
        {
            audioSource.DOFade(0, _fadeDuration).OnComplete(() => audioSource.Stop());

        }
        else
        {
            audioSource.Stop();
        }

    }
    void PlayAudio(AudioClip _audioClip,float _fadeDuration = 0.25f)
    {
        audioSource.DOFade(1, _fadeDuration).OnStart(() => audioSource.PlayOneShot(_audioClip));

    }
}
