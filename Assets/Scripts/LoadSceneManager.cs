using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadSceneManager : MonoBehaviour
{
    [SerializeField] Animator anim;
    [SerializeField] string sceneName;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GameObject.FindGameObjectWithTag("MusicManager").GetComponent<MusicManager>().EffectByScenes();
            StartCoroutine(LoadScene());
        }
    }

    IEnumerator LoadScene()
    {
        anim.SetTrigger("fadeOut");
        yield return new WaitForSeconds(1);
        SceneManager.LoadScene(sceneName);
    }
}
