using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameState : MonoBehaviour
{
    public GameObject whiteScreen;
    public Animator[] transition;
    public bool flag = false;
    // Update is called once per frame
    void Update()
    {
        if (PlayerPrefs.GetInt("LoadTransition", 0) == 0 && !flag)
        {
            whiteScreen.SetActive(true);
            flag = true;
        }

        if (PlayerPrefs.GetInt("LoadTransition", 0) == 1 && !flag)
        {
            transition[1].SetBool("Fade", false);
            flag = true;
        }
    }
}
