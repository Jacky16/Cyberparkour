using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Audio;
using UnityEngine.Events;

public class OptionsManager : MonoBehaviour
{
    [Header("Canvas GameObjects")]
    [SerializeField] GameObject canvasBeforeOptions;
    [SerializeField] GameObject canvasOptions;
    [SerializeField] GameObject firstButtonSelected;
    [Header("Audio")]
    [SerializeField] AudioMixer audiomixer;
    [SerializeField] Slider sliderMaster;
    [SerializeField] Slider sliderMusic;
    [SerializeField] Slider sliderSounds;

    [Header("Default Volume")]
    float volumeMusic = .5f;
    float volumeSounds = .5f;
    float volumeMaster = .5f;

    [SerializeField] Toggle fullScreenToogle;

    [SerializeField] UnityEvent OnReturn;
    bool isFullScreen;
    private void OnEnable()
    {
        LoadValuesAudio();
        LoadSliders();
    }

    public void Return()
    {
        canvasBeforeOptions.SetActive(true);

        //Se desactiva el canvas cuando acaba la animacion

        SaveValuesAudio();

        //Se Llama a las animaciones Init del canvasBefore
        OnReturn.Invoke();

        gameObject.SetActive(false);
    }
    public void SaveValuesAudio()
    {
        PlayerPrefs.SetFloat("audioMusicValue", volumeMusic);
        PlayerPrefs.SetFloat("audioSoundValue", volumeSounds);
        PlayerPrefs.SetFloat("audioMasterValue", volumeMaster);
    }
    public void LoadValuesAudio()
    {
        volumeMusic = PlayerPrefs.GetFloat("audioMusicValue", 0.75f);
        volumeSounds = PlayerPrefs.GetFloat("audioSoundValue", 0.75f);
        volumeMaster = PlayerPrefs.GetFloat("audioMasterValue", 0.75f);
        print(volumeMusic);

        audiomixer.SetFloat("musicVolume", volumeMusic);
        audiomixer.SetFloat("soundVolume", volumeSounds);
        audiomixer.SetFloat("masterVolume", volumeMaster);

    }

    public void LoadSliders()
    {
        sliderMusic.value = volumeMusic;
        sliderSounds.value = volumeSounds;
        sliderMaster.value = volumeMaster;
    }

    public void OnChangeMusicVolume(float _sliderValue)
    {
        audiomixer.SetFloat("musicVolume", Mathf.Log10(_sliderValue) * 20);
        volumeMusic = _sliderValue;
        SaveValuesAudio();
    }
    public void OnChangeSFXVolume(float _sliderValue)
    {
        audiomixer.SetFloat("soundVolume", Mathf.Log10(_sliderValue) * 20);
        volumeSounds = _sliderValue;
    }
    public void OnChangeMasterVolume(float _sliderValue)
    {
        audiomixer.SetFloat("masterVolume", Mathf.Log10(_sliderValue) * 20);
        volumeMaster = _sliderValue;
    }

    public void FullScreen(bool _b)
    {
        Screen.fullScreen = _b;
    }

}