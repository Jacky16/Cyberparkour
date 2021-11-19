using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using DG.Tweening;

public class PlayerGlobalVolume : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] float wallRunVel = 2;
    [SerializeField] float dashVel = .5f;
    [SerializeField] float slideVel = .5f;
    [SerializeField] float rewindVel = .5f;


    [Header("Volumes profiles")]
    [SerializeField] VolumeProfile playerWallRunVP;
    [SerializeField] VolumeProfile playerDashRunVP;
    [SerializeField] VolumeProfile playerSlideRunVP;
    [SerializeField] VolumeProfile playerRewindVP;
    [SerializeField]Volume playerVolume;
    bool isRewinding,isDashing,isWallRunning,isSiliding;


    public void SetVolumeWallRun(bool _isWallRunning)
    {
        isWallRunning = _isWallRunning;
        if (isRewinding) return;
        if (_isWallRunning)
        {
            playerVolume.profile = playerWallRunVP;
            playerVolume.weight = Mathf.Lerp(playerVolume.weight, 1, wallRunVel * Time.deltaTime);
        }
        else
        {   
            playerVolume.weight = Mathf.Lerp(playerVolume.weight, 0, wallRunVel * Time.deltaTime);      
        }
    }

    public void SetVolumeDash(bool _isDashing)
    {
        isDashing = _isDashing;
        if (isRewinding) return;
        if (_isDashing)
        {
            playerVolume.profile = playerDashRunVP;
            DOTween.To(() => playerVolume.weight, x => playerVolume.weight = x, 1, dashVel);
        }
        else
        {
            DOTween.To(() => playerVolume.weight, x => playerVolume.weight = x, 0, dashVel);
        }
    }
    public void SetVolumeSliding(bool _isSliding)
    {
        isSiliding = _isSliding;
        if (isRewinding) return;
        if (_isSliding)
        {
            playerVolume.profile = playerSlideRunVP;
            DOTween.To(() => playerVolume.weight, x => playerVolume.weight = x, 1, slideVel);
        }
        else
        {
            DOTween.To(() => playerVolume.weight, x => playerVolume.weight = x, 0, slideVel);
        }
    }
    public void SetVolumeRewind(bool _isRewinding)
    {
        isRewinding = _isRewinding;

        if (_isRewinding)
        {
            playerVolume.profile = playerRewindVP;
            DOTween.To(() => playerVolume.weight, x => playerVolume.weight = x, 1, rewindVel);
        }
        else
        {
            DOTween.To(() => playerVolume.weight, x => playerVolume.weight = x, 0, rewindVel);
        }
    }
}
