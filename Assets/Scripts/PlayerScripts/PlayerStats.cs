using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public int hp = 100;
    public int mp = 0;
    public RectTransform playerMPBar;
    public float playerMPMax = 495.9f;
    // Start is called before the first frame update
    void Start()
    {
        //PlayerPrefs.SetInt("PlayerMP", 0);

        mp = PlayerPrefs.GetInt("PlayerMP");
        playerMPBar = GameObject.FindWithTag("MP").GetComponent<RectTransform>();

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        mp = PlayerPrefs.GetInt("PlayerMP");
        if (PlayerPrefs.GetInt("PlayerMP") <= 100)
        {
            float currentMP = PlayerPrefs.GetInt("PlayerMP");
            playerMPBar.sizeDelta = new Vector2((currentMP * (playerMPMax / 100)), playerMPBar.sizeDelta.y);
        }
    }
}
