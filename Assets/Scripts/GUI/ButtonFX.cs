using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonFX : MonoBehaviour
{
    public Animator[] button;
    public AudioClip[] sfx;
    public void ContinueButtonHighlightOn()
    {
        button[0].SetBool("Highlight", true);
        GetComponent<AudioSource>().PlayOneShot(sfx[0]);
    }
    public void ContinueButtonHighlightOff()
    {
        button[0].SetBool("Highlight", false);

    }
    public void QuitButtonHighlightOn()
    {
        button[1].SetBool("Highlight", true);
        GetComponent<AudioSource>().PlayOneShot(sfx[0]);
    }
    public void QuitButtonHighlightOff()
    {
        button[1].SetBool("Highlight", false);
    }

    public void AcceptButtonHighlightOn()
    {
        button[0].SetBool("Highlight", true);
        GetComponent<AudioSource>().PlayOneShot(sfx[0]);
    }
    public void AcceptButtonHighlightOff()
    {
        button[0].SetBool("Highlight", false);
    }
    public void DeclineButtonHighlightOn()
    {
        button[1].SetBool("Highlight", true);
        GetComponent<AudioSource>().PlayOneShot(sfx[0]);
    }
    public void DeclineButtonHighlightOff()
    {
        button[1].SetBool("Highlight", false);
    }

    public void ConfirmSFX()
    {
        GetComponent<AudioSource>().PlayOneShot(sfx[1]);
    }
}
