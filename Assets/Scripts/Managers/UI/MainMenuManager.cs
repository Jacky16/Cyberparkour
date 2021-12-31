using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField] Animator anim;

    public void Play()
    {
        StartCoroutine(LoadGameplayScene());
    }
    IEnumerator LoadGameplayScene()
    {
        anim.SetTrigger("fadeOut");
        yield return new WaitForSeconds(1);
        SceneManager.LoadScene("GameplayScene");
    }

    public void Exit()
    {
        Application.Quit();
    }
}
