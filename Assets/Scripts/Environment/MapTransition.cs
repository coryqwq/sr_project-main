using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MapTransition : MonoBehaviour
{
    public Scene sceneScript; 

    private void OnTriggerEnter(Collider other)
    {
        int sceneIndex = SceneManager.GetActiveScene().buildIndex;
        if(other.gameObject.name == "LeftPortal")
        {
            PlayerPrefs.SetInt("SpawnPosition", -1);
            sceneScript.LoadScene(sceneIndex - 1);
        }
        else if(other.gameObject.name == "RightPortal")
        {
            PlayerPrefs.SetInt("SpawnPosition", 1);
            sceneScript.LoadScene(sceneIndex + 1);
        }
    }
}
