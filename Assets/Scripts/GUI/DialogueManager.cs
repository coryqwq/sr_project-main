using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
public class DialogueManager : MonoBehaviour
{
    public GameObject player;
    public GameObject dialogueObject;

    DialogueTrigger dialogueTriggerScript;

    public TextMeshProUGUI nameText;
    public TextMeshProUGUI dialogueText;
    public bool typing = false;
    public string currentSentence;

    public Animator[] anim;
    public ArrayList names = new ArrayList();
    public int animIndex = 0;

    public AudioSource voice;
    public AudioClip[] voiceLine;
    public int voiceIndex = 0;

    public Queue<string> sentences;
    public int index = 0;

    public bool dialogueStart = true;
    public bool flag = false;
    public bool flag1 = false;

    public Button continueButton;

    public GameObject sceneTransition;

    private void Update()
    {
        if (dialogueStart == true && flag1 == false)
        {
            continueButton.gameObject.SetActive(true);
            EventSystem.current.GetComponent<EventSystem>().SetSelectedGameObject(continueButton.gameObject);

            if (player != null)
            {
                player.GetComponent<PlayerController>().enabled = false;
            }

            flag1 = true;
        }
    }
    public void StartDialogue(Dialogue[] dialogue)
    {
        dialogueStart = true;
        dialogueTriggerScript = dialogueObject.GetComponent<DialogueTrigger>();

        //initialize array of character names
        NameArray();

        //initialize sentences queue
        sentences = new Queue<string>();
        sentences.Clear();

        if (flag == false)
        {
            //start opening anim for dialogue box
            anim[0].SetBool("IsOpen", true);
            anim[1].SetBool("IsOpen", true);
            flag = true;
        }


        //enqueque the sentence from, and for as times as there are sentences in the first dialogue element
        foreach (string sentence in dialogue[0].sentences)
        {
            sentences.Enqueue(sentence);
        }

        //display the sentence
        DisplayNextSentence();
    }

    public void DisplayNextSentence()
    {
        //start closing animation for dialogue box and last character to speak
        if (sentences.Count == 0 && typing == false && index == dialogueTriggerScript.dialogue.Length - 1)
        {
            EndDialogue();
            index = 0;
            return;

        }

        //start closing animation for current character after finishing speaking
        //and set the sentences queue with dialogue of next character
        if (sentences.Count == 0 && typing == false)
        {
            index++;
            //enqueque the sentence from, and for as times as there are sentences in
            //the specified dialogue element
            foreach (string sentence in dialogueTriggerScript.dialogue[index].sentences)
            {
                sentences.Enqueue(sentence);
            }
            anim[animIndex].SetBool("IsSpeaking", false);
        }

        //compare all names in names arraylist to that of current character speaking
        //to assign animIndex based on nameindex 
        //to get an index for starting/stoping anim that corresponds to the current character speaking
        for (int i = 0; i < names.Count; i++)
        {
            if ((string)names[i] == dialogueTriggerScript.dialogue[index].name)
            {
                //note: the names arraylist starts counting from 0
                //0 and 1 for animIndex is reserved for the dialogue box and background anim
                //offset animIndex by +2 to access animation array starting from element 2
                animIndex = i + 2;
            }
        }

        //if dialogue box is not typing letters, start opening anim for current character
        //and set the sentences queue for next sentence of the current character
        if (typing == false)
        {
            //display name of current character speaking
            nameText.text = dialogueTriggerScript.dialogue[index].name;

            anim[animIndex].SetBool("IsSpeaking", true);

            //assign and play voice line
            //voice.Stop();
            //voice.clip = voiceLine[voiceIndex];
            //voice.Play();
            //voiceIndex++;

            string sentence = sentences.Dequeue();
            StartCoroutine(TypeSentence(sentence));
            return;
        }

        //if dialogue box is typing letters, stop typing, and instantly display full dialogue text
        if (typing == true)
        {
            StopAllCoroutines();
            dialogueText.text = currentSentence;
            typing = false;
        }
    }

    public void NameArray()
    {
        //flag for when name already exists in names arraylist
        bool check = false;

        //iterate for loop for as many times as the length of dialogue array
        for (int i = 0; i < dialogueTriggerScript.dialogue.Length; i++)
        {
            //set flag to false
            check = false;

            //iterate for loop for as many times as the length of names arraylist
            for (int j = 0; j < names.Count; j++)
            {
                //check to see if the name already exists in the names arraylist 
                if ((string)names[j] == dialogueTriggerScript.dialogue[i].name)
                {
                    //flag the comparison if name already exists in names arraylist
                    check = true;
                }
            }

            //add name to names arraylist for when comparison is not flagged
            if (check == false)
            {
                names.Add(dialogueTriggerScript.dialogue[i].name);
            }
        }
    }

    //creates tpying effect in dialogue box
    IEnumerator TypeSentence(string sentence)
    {
        typing = true;

        //save the current sentence to reference later when the user skips typing effect
        currentSentence = sentence;

        //reset dialogue box text
        dialogueText.text = "";

        //add one letter at the given interval for as many times as there are letters in the dialogue 
        foreach (char letter in sentence.ToCharArray())
        {
            //gameObject.GetComponent<AudioSource>().Play();
            dialogueText.text += letter;

            yield return new WaitForSeconds(0.06f);
        }

        typing = false;
    }
    void EndDialogue()
    {

        continueButton.gameObject.SetActive(false);

        dialogueStart = false;
        flag = false;
        flag1 = false;

        anim[0].SetBool("IsOpen", false);
        anim[1].SetBool("IsOpen", false);
        anim[animIndex].SetBool("IsSpeaking", false);

        if (player != null)
        {
            player.GetComponent<PlayerController>().enabled = true;
        }
    }
}
