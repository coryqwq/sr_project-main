using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class Scene : MonoBehaviour
{
    public GameObject transitionObject;
    public Animator transition;
    public int transitionTime = 4;
    public void Start()
    {
        if (SceneManager.GetActiveScene().name == "CutScene")
        {
            PlayerPrefs.SetInt("SpawnPosition", 0);
            PlayerPrefs.SetInt("LoadTransition", 0);
            PlayerPrefs.SetInt("PlayerMP", 0);
            StartCoroutine(DelayStartLevel(2));
        }
    }

    public void StartTitleMenuTranstion()
    {
        StartCoroutine(DelayStartLevel(1));
    }

    public void StartRespawnTransition()
    {
        StartCoroutine(DelayStartLevel(10));
    }


    IEnumerator DelayStartLevel(int i)
    {
        yield return new WaitForSeconds(transitionTime);
        transitionObject.SetActive(true);
        yield return new WaitForSeconds(4);
        PlayerPrefs.SetInt("LoadTransition", 0);

        SceneManager.LoadScene(i);
    }
    public void QuitGame()
    {
        Application.Quit();
    }

    public void LoadScene(int index)
    {
        PlayerPrefs.SetInt("LoadTransition", 1);
        StartCoroutine(DelayLoadScene(index));
    }
    IEnumerator DelayLoadScene(int index)
    {
        transition.SetTrigger("FadeIn");
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene(index);
    }
}
