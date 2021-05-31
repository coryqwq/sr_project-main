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
    public Transform cameraCompound;
    public Transform cameraFollowTrigger;

    public bool flag0 = false;
    public bool flag1 = false;

    private void Start()
    {
        //set player to spawn on right side of last stage when backtracking portal
        if (PlayerPrefs.GetInt("SpawnPosition", 0) == -1)
        {
            SetRightSide();
            if (SceneManager.GetActiveScene().buildIndex == 1)
            {
                cameraCompound.position = new Vector3(0.02f, cameraCompound.position.y, cameraCompound.position.z);
            }
            if (SceneManager.GetActiveScene().buildIndex == 2)
            {
                cameraCompound.position = new Vector3(14.9f, cameraCompound.position.y, cameraCompound.position.z);
            }
            if (SceneManager.GetActiveScene().buildIndex == 3)
            {
                cameraCompound.position = new Vector3(2.43f, cameraCompound.position.y, cameraCompound.position.z);
            }
            if (SceneManager.GetActiveScene().buildIndex == 4)
            {
                cameraCompound.position = new Vector3(5.5f, cameraCompound.position.y, cameraCompound.position.z);
            }
            if (SceneManager.GetActiveScene().buildIndex == 5)
            {
                player.transform.localPosition = new Vector3(player.transform.localPosition.x, player.transform.localPosition.y + 2.3676666f, player.transform.localPosition.z);
                cameraCompound.position = new Vector3(15.48f, 0.3510833f, cameraCompound.position.z);
                Debug.Log("asdf");
            }
            if (SceneManager.GetActiveScene().buildIndex == 6)
            {
                cameraCompound.position = new Vector3(12.2f, cameraCompound.position.y, cameraCompound.position.z);
            }
        }
    }
    void SetRightSide()
    {
        player.GetComponent<SpriteRenderer>().flipX = false;
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
