using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField] Animator anim;
    [SerializeField] MusicManager musicManager;
    private void Awake()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }
    public void Play()
    {
        
        StartCoroutine(LoadGameplayScene());
    }
    IEnumerator LoadGameplayScene()
    {
        anim.SetTrigger("fadeOut");
        yield return new WaitForSeconds(1);
        SceneManager.LoadScene("GS_Tutorial");
        musicManager.QuitLowPassFilter();
    }

    public void Exit()
    {
        Application.Quit();
    }
}
