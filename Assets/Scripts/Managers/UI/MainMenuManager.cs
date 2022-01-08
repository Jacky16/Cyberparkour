using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField] Animator anim;
    [SerializeField] MusicManager musicManager;
    public void Play()
    {
        musicManager.StopMusic();
        StartCoroutine(LoadGameplayScene());
    }
    IEnumerator LoadGameplayScene()
    {
        anim.SetTrigger("fadeOut");
        yield return new WaitForSeconds(1);
        SceneManager.LoadScene("GS_Tutorial");
    }

    public void Exit()
    {
        Application.Quit();
    }
}
