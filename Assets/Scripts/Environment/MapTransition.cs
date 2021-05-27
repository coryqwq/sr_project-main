using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapTransition : MonoBehaviour
{
    public string sceneName;
    public Scene sceneScript;

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.name == "Portal0")
        {
            sceneScript.LoadScene(2);
        }
        if (other.gameObject.name == "Portal1")
        {
            PlayerPrefs.SetInt("SpawnPosition", -1);
            sceneScript.LoadScene(1);
        }
        if (other.gameObject.name == "Portal2")
        {
            sceneScript.LoadScene(3);
        }
        if (other.gameObject.name == "Portal3")
        {
            PlayerPrefs.SetInt("SpawnPosition", -1);
            sceneScript.LoadScene(2);
        }
        if (other.gameObject.name == "Portal4")
        {
            sceneScript.LoadScene(4);
        }
        if (other.gameObject.name == "Portal5")
        {
            PlayerPrefs.SetInt("SpawnPosition", -1);
            sceneScript.LoadScene(3);
        }
        if (other.gameObject.name == "Portal6")
        {
            sceneScript.LoadScene(5);
        }
        if (other.gameObject.name == "Portal7")
        {
            PlayerPrefs.SetInt("SpawnPosition", -1);
            sceneScript.LoadScene(4);
        }
        if (other.gameObject.name == "Portal8")
        {
            sceneScript.LoadScene(6);
        }
        if (other.gameObject.name == "Portal9")
        {
            PlayerPrefs.SetInt("SpawnPosition", -1);
            sceneScript.LoadScene(5);
        }
        if (other.gameObject.name == "Portal10")
        {
            sceneScript.LoadScene(6);
        }
    }
}
