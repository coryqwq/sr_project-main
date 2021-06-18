using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class BGM : MonoBehaviour
{
    public GameObject bgmSource;
    public AudioClip[] bgm;

    public int currentSceneIndex;
    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(gameObject);
        currentSceneIndex = SceneManager.GetActiveScene().buildIndex;

        if (currentSceneIndex == 2 || currentSceneIndex == 3)
        {
            if (!bgmSource.GetComponent<AudioSource>().clip == bgm[0])
            {
                bgmSource.GetComponent<AudioSource>().Stop();
                bgmSource.GetComponent<AudioSource>().clip = bgm[0];
                bgmSource.GetComponent<AudioSource>().Play();
            }
        }
        if (currentSceneIndex == 4 || currentSceneIndex == 5)
        {
            if (!bgmSource.GetComponent<AudioSource>().clip == bgm[1])
            {
                bgmSource.GetComponent<AudioSource>().Stop();
                bgmSource.GetComponent<AudioSource>().clip = bgm[1];
                bgmSource.GetComponent<AudioSource>().Play();
            }
        }
        if (currentSceneIndex == 6)
        {
            if (!bgmSource.GetComponent<AudioSource>().clip == bgm[2])
            {
                bgmSource.GetComponent<AudioSource>().Stop();
                bgmSource.GetComponent<AudioSource>().clip = bgm[2];
                bgmSource.GetComponent<AudioSource>().Play();
            }
        }
        if (currentSceneIndex == 7 || currentSceneIndex == 8)
        {
            if (!bgmSource.GetComponent<AudioSource>().clip == bgm[3])
            {
                bgmSource.GetComponent<AudioSource>().Stop();
                bgmSource.GetComponent<AudioSource>().clip = bgm[3];
                bgmSource.GetComponent<AudioSource>().Play();
            }
        }
        if (currentSceneIndex == 9)
        {
            if (!bgmSource.GetComponent<AudioSource>().clip == bgm[4])
            {
                bgmSource.GetComponent<AudioSource>().Stop();
                bgmSource.GetComponent<AudioSource>().clip = bgm[4];
                bgmSource.GetComponent<AudioSource>().Play();
            }
        }

        if (currentSceneIndex == 11)
        {
            if (!bgmSource.GetComponent<AudioSource>().clip == bgm[5])
            {
                bgmSource.GetComponent<AudioSource>().Stop();
                bgmSource.GetComponent<AudioSource>().clip = bgm[5];
                bgmSource.GetComponent<AudioSource>().Play();
            }
        }

        if (currentSceneIndex == 13)
        {
            if (!bgmSource.GetComponent<AudioSource>().clip == bgm[6])
            {
                bgmSource.GetComponent<AudioSource>().Stop();
                bgmSource.GetComponent<AudioSource>().clip = bgm[6];
                bgmSource.GetComponent<AudioSource>().Play();
            }
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
