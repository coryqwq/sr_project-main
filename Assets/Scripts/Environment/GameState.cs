using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
public class GameState : MonoBehaviour
{
    public GameObject whiteScreen;
    public GameObject blackScreen;
    public Animator playerAnim;
    public GameObject player;
    public Transform cameraCompound;
    public Transform cameraFollowTrigger;

    public bool flag0 = false;
    public bool flag1 = false;

    public float[] xCamPos = new float[] { 0.02f, 14.9f, 2.43f, 5.5f, 0f, 12.2f, 3.88f, 10.31f};
    public string[] levelTitles = new string[] { "Heart of the Forest", "Forest Meadow", "Azure Lake",
                                                "Twilit Forest", "Near the Floral Flute", "Grove of the Spirit Tree",
                                                "Deep Fairy Forest", "Fungos Forest", "Reaper's Heart", "Temple of Rebirth"};
    public TextMeshProUGUI levelTitleText;
    private void Start()
    {
        int i = SceneManager.GetActiveScene().buildIndex;

        //set title level
        levelTitleText.text = levelTitles[i - 1];

        //set player to spawn on right side of last stage when backtracking portal
        if (PlayerPrefs.GetInt("SpawnPosition", 0) == -1)
        {
            if (i == 5)
            {
                player.transform.localPosition = new Vector3(player.transform.localPosition.x, player.transform.localPosition.y + 2.3676666f, player.transform.localPosition.z);
                cameraCompound.position = new Vector3(15.48f, 0.3510833f, cameraCompound.position.z);
            }
            else
            {
                cameraCompound.position = new Vector3(xCamPos[i - 1], cameraCompound.position.y, cameraCompound.position.z);
            }
            SetRightSide();
        }
    }
    void SetRightSide()
    {
        player.transform.localScale = new Vector3(player.transform.localScale.x * -1, player.transform.localScale.y, player.transform.localScale.z);
        player.GetComponent<PlayerController>().direction = -1;
        player.transform.localPosition = new Vector3(-player.transform.localPosition.x, player.transform.localPosition.y, player.transform.localPosition.z);
        cameraFollowTrigger.localPosition = new Vector3(-cameraFollowTrigger.localPosition.x, cameraFollowTrigger.localPosition.y, cameraFollowTrigger.localPosition.z);
    }

    void Update()
    {
        //white opening transition screen, player in dead animation
        if (PlayerPrefs.GetInt("LoadTransition", 0) == 0 && !flag0)
        {
            playerAnim.SetTrigger("IsNotAwake");
            whiteScreen.SetActive(true);
            flag0 = true;
        }

        //black opening transition screen, player in standby/move animation
        if (PlayerPrefs.GetInt("LoadTransition", 0) == 1 && !flag1)
        {
            playerAnim.SetTrigger("IsAwake");
            blackScreen.SetActive(true);
            flag1 = true;
        }
    }
}
