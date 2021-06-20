using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.EventSystems;
public class GameState : MonoBehaviour
{
    public GameObject whiteScreen;
    public GameObject blackScreen;
    public GameObject levelTitle;
    public Animator playerAnim;
    public GameObject player;
    public GameObject enemy;
    public Transform cameraCompound;
    public Transform cameraFollowTrigger;
    public GameObject dialogueCompound;
    public float[] xCamPos = new float[] { 0.02f, 14.9f, 2.43f, 5.5f, 0f, 12.2f, 3.88f, 10.31f };
    public string[] levelTitles = new string[] { "Heart of the Forest", "Forest Meadow", "Azure Lake",
                                                "Twilit Forest", "Near the Floral Flute", "Grove of the Spirit Tree",
                                                "Deep Fairy Forest", "Fungos Forest", "Temple of Rebirth", "Reaper's Throne"," "};
    public TextMeshProUGUI levelTitleText;
    public GameObject cutscene1;
    public GameObject cutscene2;
    public GameObject cutscene3;

    public GameObject hud;

    public GameObject deadDialogueCompound;

    public Scene sceneScript;

    public GameObject dialogueCompound2;

    public GameObject prompt;
    public bool flag = false;
    public bool flag1 = false;
    public bool flag2 = false;

    public GameObject InterSceneDialogue0;
    public GameObject InterSceneDialogue1;
    public GameObject InterSceneDialogue2;
    public GameObject InterSceneDialogue3;

    public GameObject transitionImage;
    public GameObject transitionImage2;

    public GameObject finalMessage;


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
        if (SceneManager.GetActiveScene().name == "LevelScene" && PlayerPrefs.GetInt("LoadTransition", 0) == 0)
        {
            playerAnim.SetTrigger("IsNotAwake");
            hud.SetActive(false);
            whiteScreen.SetActive(true);
            player.GetComponent<PlayerController>().enableInput = false;
            levelTitle.SetActive(false);
        }
        
        if(SceneManager.GetActiveScene().name == "LevelScene 11" && PlayerPrefs.GetInt("LoadTransition", 0) == 0)
        {
            playerAnim.SetTrigger("IsNotAwake");
            whiteScreen.SetActive(true);
        }

        //black opening transition screen, player in standby/move animation
        if (PlayerPrefs.GetInt("LoadTransition", 0) == 1)
        {
            playerAnim.SetTrigger("IsAwake");
            blackScreen.SetActive(true);
        }

        if (SceneManager.GetActiveScene().name == "LevelScene 9" && PlayerPrefs.GetInt("cutscene1") == 0)
        {
            player.GetComponent<PlayerController>().enableInput = false;
            blackScreen.GetComponent<Animator>().SetTrigger("FadeIn");
            levelTitle.SetActive(false);
            hud.SetActive(false);
            cutscene1.SetActive(true);
        }
        else if (SceneManager.GetActiveScene().name == "LevelScene 9" && PlayerPrefs.GetInt("cutscene1") == 1)
        {
            player.GetComponent<PlayerController>().enableInput = false;
            levelTitle.SetActive(false);
            hud.SetActive(false);
            StartCoroutine(DelayEnablePlayer(5));
        }

        if (SceneManager.GetActiveScene().name == "LevelScene 8" && (PlayerPrefs.GetInt("LoadTransition") == 1))
        {
            dialogueCompound2.SetActive(true);
        }

        if (SceneManager.GetActiveScene().name == "LevelScene 8" && (PlayerPrefs.GetInt("LoadTransition") == 0))
        {
            playerAnim.SetTrigger("IsNotAwake");
            hud.SetActive(false);
            whiteScreen.SetActive(true);
            player.GetComponent<PlayerController>().enableInput = false;
            levelTitle.SetActive(false);
        }

        if (SceneManager.GetActiveScene().name == "LevelScene 10")
        {
            player.GetComponent<PlayerController>().enableInput = false;
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
        if (whiteScreen.activeInHierarchy)
        {
            if (SceneManager.GetActiveScene().name == "LevelScene"
                       && whiteScreen.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).normalizedTime > 1
                       && !whiteScreen.GetComponent<Animator>().IsInTransition(0))
            {
                whiteScreen.SetActive(false);
                hud.SetActive(true);
                dialogueCompound.SetActive(true);

            }

            if (SceneManager.GetActiveScene().name == "LevelScene 8"
            && whiteScreen.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).normalizedTime > 1
            && !whiteScreen.GetComponent<Animator>().IsInTransition(0))
            {
                whiteScreen.SetActive(false);
                hud.SetActive(true);
                dialogueCompound.SetActive(true);

            }
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

        if (enemy != null)
        {
            if (!player.GetComponent<PlayerController>().alive && enemy.GetComponent<EnemyBossController>().alive && !flag)
            {
                StartCoroutine(GameOverSequence(2));
                flag = true;
            }

            if (SceneManager.GetActiveScene().name == "LevelScene 9" && !enemy.GetComponent<EnemyBossController>().alive && !flag)
            {
                StartCoroutine(EndSequence(4));
                flag = true;
            }
        }


        if (SceneManager.GetActiveScene().name == "LevelScene 10" && !flag)
        {
            StartCoroutine(DelayPrompt(2));
            flag = true;
        }

        if (PlayerPrefs.GetInt("cutscene2") == 1 && !flag1)
        {
            StartCoroutine(BadEndingSequence());
            flag1 = true;
        }

        if (FindObjectOfType<CameraController>().displayMessage)
        {
            finalMessage.SetActive(true);
        }

        if (SceneManager.GetActiveScene().name == "LevelScene 12" && !flag)
        {
            StartCoroutine(StartCredits());
        }
    }

    public IEnumerator DelayEnablePlayer(int delay)
    {
        blackScreen.GetComponent<Animator>().SetTrigger("FadeOut");
        cutscene2.SetActive(true);
        yield return new WaitForSeconds(5);
        blackScreen.GetComponent<Animator>().SetTrigger("FadeIn");
        yield return new WaitForSeconds(1);
        cutscene2.SetActive(false);
        blackScreen.GetComponent<Animator>().SetTrigger("FadeOut");
        yield return new WaitForSeconds(1);
        player.GetComponent<PlayerController>().enableInput = true;
        hud.SetActive(true);
        hud.GetComponent<Animator>().SetBool("enable", true);
        levelTitle.SetActive(true);

        PlayerPrefs.SetInt("cutscene1", 1);
    }

    public IEnumerator GameOverSequence(int delay)
    {
        yield return new WaitForSeconds(delay);
        blackScreen.GetComponent<Animator>().SetTrigger("FadeIn");
        yield return new WaitForSeconds(4);
        deadDialogueCompound.SetActive(true);
        PlayerPrefs.SetInt("PlayerHP", 100);
        sceneScript.StartRespawnTransition();
    }

    public IEnumerator EndSequence(int delay)
    {
        yield return new WaitForSeconds(delay);
        blackScreen.GetComponent<Animator>().SetTrigger("FadeIn");
        yield return new WaitForSeconds(2);
        dialogueCompound2.SetActive(true);
        PlayerPrefs.SetInt("End", 1);
    }

    public IEnumerator DelayPrompt(int delay)
    {
        yield return new WaitForSeconds(delay);
        prompt.SetActive(true);

    }

    public void BadEndingDialogue()
    {
        ChangeSortOrder();
        PlayerPrefs.SetInt("End", 2);
        dialogueCompound.SetActive(true);
        
    }
    public void GoodEndingDialogue()
    {
        ChangeSortOrder();
        StartCoroutine(GoodEndingSequence());

    }

    public void ChangeSortOrder()
    {
        prompt.GetComponent<Canvas>().sortingOrder = 1;
        blackScreen.GetComponent<Animator>().SetTrigger("FadeIn");

    }
    IEnumerator BadEndingSequence()
    {
        yield return new WaitForSeconds(2);
        GetComponent<AudioSource>().Play();
        cutscene1.SetActive(true);
        dialogueCompound.SetActive(false);
        yield return new WaitForSeconds(5);
        cutscene1.SetActive(false);
        InterSceneDialogue0.SetActive(true);
        yield return new WaitForSeconds(8);
        InterSceneDialogue0.SetActive(false);
        InterSceneDialogue1.SetActive(true);
        yield return new WaitForSeconds(8);
        transitionImage.SetActive(true);
        PlayerPrefs.SetInt("SpawnPosition", 0);
        PlayerPrefs.SetInt("LoadTransition", 0);
        PlayerPrefs.SetInt("cutscene1", 0);
        PlayerPrefs.SetInt("cutscene2", 0);
        PlayerPrefs.SetInt("PlayerMP", 0);
        PlayerPrefs.SetInt("PlayerHP", 100);
        PlayerPrefs.SetInt("End", 0);
        yield return new WaitForSeconds(3);
        SceneManager.LoadScene(2);
    }
    IEnumerator GoodEndingSequence()
    {
        yield return new WaitForSeconds(1);
        InterSceneDialogue2.SetActive(true);
        yield return new WaitForSeconds(10);
        InterSceneDialogue2.SetActive(false);
        cutscene2.SetActive(true);
        yield return new WaitForSeconds(4);
        cutscene2.SetActive(false);
        InterSceneDialogue3.SetActive(true);
        yield return new WaitForSeconds(12);
        transitionImage2.SetActive(true);
        yield return new WaitForSeconds(6);
        PlayerPrefs.SetInt("SpawnPosition", 0);
        PlayerPrefs.SetInt("LoadTransition", 0);
        PlayerPrefs.SetInt("cutscene1", 0);
        PlayerPrefs.SetInt("cutscene2", 0);
        PlayerPrefs.SetInt("PlayerMP", 0);
        PlayerPrefs.SetInt("PlayerHP", 100);
        PlayerPrefs.SetInt("End", 0);
        SceneManager.LoadScene(13);
    }
    IEnumerator StartCredits()
    {
        yield return new WaitForSeconds(7);
        SceneManager.LoadScene(0);
    }
}
