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
    }
}
