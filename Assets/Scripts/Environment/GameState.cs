using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class GameState : MonoBehaviour
{
    public GameObject whiteScreen;
    public GameObject blackScreen;
    public Animator playerAnim;
    public GameObject player;
    public Transform mainCamera;
    public Transform cameraFollowTrigger;

    public bool flag0 = false;
    public bool flag1 = false;

    private void Start()
    {
        //set player to spawn on left side of last stage when backtracking portal
        if(PlayerPrefs.GetInt("SpawnPosition", 0) == -1 && SceneManager.GetActiveScene().buildIndex == 1)
        {
            player.transform.position = new Vector3(5.3f, player.transform.position.y, player.transform.position.z);
            player.GetComponent<SpriteRenderer>().flipX = false;
            player.GetComponent<PlayerController>().direction = -1;

            mainCamera.position = new Vector3(3.323896f, mainCamera.position.y, mainCamera.position.z);
            cameraFollowTrigger.position = new Vector3(1.95f, cameraFollowTrigger.position.y, cameraFollowTrigger.position.z);

        }
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
