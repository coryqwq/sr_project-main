using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scene : MonoBehaviour
{
    int log = 0;
    public void Log()
    {
        Debug.Log("clicked" + log);
        log++;
    }
    public void QuitGame()
    {
        Application.Quit();
    }
}
