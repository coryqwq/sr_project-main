using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public RectTransform playerMPBar;
    public RectTransform playerHPBar;

    public float playerMPMax = 495.9f;
    public float playerHPMax = 1544f;

    // Start is called before the first frame update
    void Start()
    {
        PlayerPrefs.SetInt("PlayerHP", 100);
        //PlayerPrefs.SetInt("PlayerMP", 100);

        playerMPBar = GameObject.FindWithTag("MP").GetComponent<RectTransform>();
        playerHPBar = GameObject.FindWithTag("HP").GetComponent<RectTransform>();


    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (PlayerPrefs.GetInt("PlayerMP") <= 100)
        {
            float currentMP = PlayerPrefs.GetInt("PlayerMP");
            playerMPBar.sizeDelta = new Vector2((currentMP * (playerMPMax / 100)), playerMPBar.sizeDelta.y);
        }
        if (PlayerPrefs.GetInt("PlayerHP") <= 100)
        {
            float currentHP = PlayerPrefs.GetInt("PlayerHP");
            playerHPBar.sizeDelta = new Vector2((currentHP * (playerHPMax / 100)), playerHPBar.sizeDelta.y);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("EnemyAttack") && !GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("playerLimit"))
        {
            PlayerPrefs.SetInt("PlayerHP", PlayerPrefs.GetInt("PlayerHP") - 1);
        }
    }
}
