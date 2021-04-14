using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class Scene : MonoBehaviour
{
    public GameObject titleMenuTransition;
    public Animator transition;
    public void StartLevel()
    {
        PlayerPrefs.SetInt("LoadTransition", 0);
        StartCoroutine(DelayStartLevel());
    }
    IEnumerator DelayStartLevel()
    {
        titleMenuTransition.SetActive(true);
        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene("LevelScene");
    }
    public void QuitGame()
    {
        Application.Quit();
    }
}
