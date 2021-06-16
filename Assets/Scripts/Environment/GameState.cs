using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
public class GameState : MonoBehaviour
{
    public GameObject whiteScreen;
    public GameObject blackScreen;
    public GameObject levelTitle;
    public Animator playerAnim;
    public GameObject player;
    public Transform cameraCompound;
    public Transform cameraFollowTrigger;
    public GameObject dialogueCompound;
    public float[] xCamPos = new float[] { 0.02f, 14.9f, 2.43f, 5.5f, 0f, 12.2f, 3.88f, 10.31f};
    public string[] levelTitles = new string[] { "Heart of the Forest", "Forest Meadow", "Azure Lake",
                                                "Twilit Forest", "Near the Floral Flute", "Grove of the Spirit Tree",
                                                "Deep Fairy Forest", "Fungos Forest", "Reaper's Heart", "Temple of Rebirth"};
    public TextMeshProUGUI levelTitleText;
    public GameObject cutscene1;
    public GameObject cutscene2;
    public GameObject hud;
    private void Start()
    {
        int i = SceneManager.GetActiveScene().buildIndex;

        //set title level
        levelTitleText.text = levelTitles[i - 2];

        //set player to spawn on right side of last stage when backtracking portal
        if (PlayerPrefs.GetInt("SpawnPosition", 0) == -1)
        {
            if (i == 6)
            {
                player.transform.localPosition = new Vector3(player.transform.localPosition.x, player.transform.localPosition.y + 2.3676666f, player.transform.localPosition.z);
                cameraCompound.position = new Vector3(15.48f, 0.3510833f, cameraCompound.position.z);
            }
            else
            {
                cameraCompound.position = new Vector3(xCamPos[i - 2], cameraCompound.position.y, cameraCompound.position.z);
            }
            SetRightSide();
        }
        //white opening transition screen, player in dead animation
        if (PlayerPrefs.GetInt("LoadTransition", 0) == 0)
        {
            playerAnim.SetTrigger("IsNotAwake");
            hud.SetActive(false);
            whiteScreen.SetActive(true);
            player.GetComponent<PlayerController>().enableInput = false;
            levelTitle.SetActive(false);
        }

        //black opening transition screen, player in standby/move animation
        if (PlayerPrefs.GetInt("LoadTransition", 0) == 1)
        {
            playerAnim.SetTrigger("IsAwake");
            blackScreen.SetActive(true);
        }

        if (SceneManager.GetActiveScene().name == "LevelScene 9" && PlayerPrefs.GetInt("cutscene1") == 0)
        {
            blackScreen.GetComponent<Animator>().SetTrigger("FadeIn");
            levelTitle.SetActive(false);
            hud.SetActive(false);
            cutscene1.SetActive(true);
        }
        else if (SceneManager.GetActiveScene().name == "LevelScene 9" && PlayerPrefs.GetInt("cutscene1") == 1)
        {
            levelTitle.SetActive(false);
            hud.SetActive(false);
            StartCoroutine(DelayEnablePlayer(5));
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
        if (SceneManager.GetActiveScene().name == "LevelScene"
            && whiteScreen.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).normalizedTime > 1 
            && !whiteScreen.GetComponent<Animator>().IsInTransition(0))
        {
            whiteScreen.SetActive(false);
            hud.SetActive(true);
            dialogueCompound.SetActive(true);

        }

        if (SceneManager.GetActiveScene().name == "LevelScene 9" && PlayerPrefs.GetInt("cutscene1") == 0)
        {
            if (cutscene1.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).normalizedTime > 1
            && !cutscene1.GetComponent<Animator>().IsInTransition(0))
            {
                cutscene1.SetActive(false);
                hud.SetActive(true);
                dialogueCompound.SetActive(true);
            }
        }
    }

    public IEnumerator DelayEnablePlayer(int delay)
    {
        blackScreen.GetComponent<Animator>().SetTrigger("FadeOut");
        cutscene2.SetActive(true);
        yield return new WaitForSeconds(delay);
        blackScreen.GetComponent<Animator>().SetTrigger("FadeIn");
        yield return new WaitForSeconds(1);
        cutscene2.SetActive(false);
        blackScreen.GetComponent<Animator>().SetTrigger("FadeOut");
        player.GetComponent<PlayerController>().enableInput = true;
        hud.SetActive(true);
        hud.GetComponent<Animator>().SetBool("enable", true);
        levelTitle.SetActive(true);
        PlayerPrefs.SetInt("cutscene1", 1);
    }
}
