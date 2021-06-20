using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    public Dialogue[] dialogue;
    public GameObject dialogueManager;

    //public void TriggerDialogue()
    void Start()
    {
        DialogueManager dialogueManagerScript = dialogueManager.GetComponent<DialogueManager>();
        dialogueManagerScript.dialogueObject = gameObject;
        
        dialogueManagerScript.StartDialogue(dialogue);
        gameObject.SetActive(false);
    }
}
