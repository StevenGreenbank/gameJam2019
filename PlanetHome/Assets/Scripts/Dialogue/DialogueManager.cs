using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : Singleton<DialogueManager>
{
    public Text nameText;
    public Text dialogueText;
    private Queue<string> sentences;
    public Image image;
    public bool isInDialogue;
    public Dictionary<string, string> textStuff;

    public  delegate void OnDialogueEnd();
    public OnDialogueEnd dialogueEnd;

    void Start()
    {
        sentences = new Queue<string>();
        
    }

    public void HideDialogueBox()
    {
        SetDialogueBoxState(false);
    }

    public void ShowDialogueBox()
    {
        SetDialogueBoxState(true);
    }

    public void StartDialogue(Dialogue dialogue)
    {
        nameText.text = dialogue.name;

        SetDialogueBoxState(true);
        isInDialogue = true;

        sentences.Clear();

        foreach (string sentence in dialogue.sentences)
        {
            sentences.Enqueue(sentence);

        }
        DisplayNextSentence();

    }

    public void DisplayNextSentence()
    {
        if (sentences.Count == 0)
        {
            Debug.Log("end of dialogue reached");
            EndDialogue();
            return;
        }
        string sentence = sentences.Dequeue();
        dialogueText.text = sentence;
    }

    public void EndDialogue()
    {
        Debug.Log("End of conversation");
        SetDialogueBoxState(false);
        isInDialogue = false;
        dialogueEnd?.Invoke();
       // dialogueEnd = null;
    }

    private void SetDialogueBoxState(bool active)
    {
        image.enabled = active;
        image.transform.gameObject.SetActive(active);
    }
}
