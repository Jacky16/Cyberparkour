using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class MusicManager : MonoBehaviour
{
    [SerializeField] bool dontDestroyOnLoad;
    AudioSource audioSource;
    [SerializeField] AudioLowPassFilter audioLowPassFilter;
    public static MusicManager musicManager;
    private void OnEnable()
    {
        if (dontDestroyOnLoad)
        {
            DontDestroyOnLoad(this);
        }
    }
    private void Awake()
    {
        if (musicManager != null)
            musicManager = this;
        
        audioSource = GetComponent<AudioSource>();
    }

    public void EffectByScenes()
    {
        StartCoroutine(EffectByScenesCoroutine());
    }
    IEnumerator EffectByScenesCoroutine()
    {
        DoLowPassFilter();
        yield return new WaitForSeconds(2);
        QuitLowPassFilter();
    }
    public void StopMusic()
    {
        audioSource.DOFade(0, 1).OnComplete(() => audioSource.Stop());
    }
    public void DoLowPassFilter()
    {
        audioLowPassFilter.enabled = true;
    }
    public void QuitLowPassFilter()
    {
        audioLowPassFilter.enabled = false;
    }
    public void PlayMusic()
    {
        audioSource.Play();
        audioSource.DOFade(1, 1);
    }


}
