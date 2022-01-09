using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseManager : MonoBehaviour
{
    bool isPaused;
    [SerializeField] Animator anim;
    [SerializeField] GameObject canvasPause;
    MusicManager musicManager;
    private void Awake()
    {
        if(GameObject.FindObjectOfType<MusicManager>().TryGetComponent(out MusicManager _Mm)){
            musicManager = _Mm;
        }

    }
    public void Pause()
    {
        InputManager.canMove = false;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        canvasPause.SetActive(true);
        isPaused = !isPaused;
        Time.timeScale = 0;
        musicManager.DoLowPassFilter();
    }
    public void ToMainMenu()
    {
        StartCoroutine(LoadMainMenu());
    }
    public void Resume()
    {
        InputManager.canMove = true;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        Time.timeScale = 1;
        isPaused = !isPaused;
        musicManager.QuitLowPassFilter();
    }
    IEnumerator LoadMainMenu()
    {
        anim.SetTrigger("fadeOut");
        yield return new WaitForSecondsRealtime(1);
        Time.timeScale = 1;
        SceneManager.LoadScene("MainMenuScene");
    }
}
