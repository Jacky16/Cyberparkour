using DG.Tweening;
using System.Collections;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    [SerializeField] private bool dontDestroyOnLoad;
    private AudioSource audioSource;
    [SerializeField] private AudioLowPassFilter audioLowPassFilter;
    public static MusicManager musicManager;

    private void Awake()
    {
        if (musicManager != null)
            musicManager = this;

        DontDestroyOnLoad(gameObject);

        audioSource = GetComponent<AudioSource>();
    }

    public void EffectByScenes()
    {
        StartCoroutine(EffectByScenesCoroutine());
    }

    private IEnumerator EffectByScenesCoroutine()
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