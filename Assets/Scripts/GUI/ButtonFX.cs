using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonFX : MonoBehaviour
{
    public Animator[] button;

    public void ContinueButtonHighlightOn()
    {
        button[0].SetBool("Highlight", true);
    }
    public void ContinueButtonHighlightOff()
    {
        button[0].SetBool("Highlight", false);
    }
    public void QuitButtonHighlightOn()
    {
        button[1].SetBool("Highlight", true);
    }
    public void QuitButtonHighlightOff()
    {
        button[1].SetBool("Highlight", false);
    }
}
