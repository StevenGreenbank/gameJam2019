using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : Singleton<DialogueManager>, IDialogueManager
{
    public Text nameText;
    public Text dialogueText;
    protected Queue<string> sentences;
    public Image dialogueWindow;

    public event OnDialogueEnd dialogueEnd
    {
        add
        {
            dialogueEnd -= value;
            dialogueEnd += value;
        }
        remove
        {
            dialogueEnd -= value;
        }
    }

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
        dialogueEnd?.Invoke();
       
       // dialogueEnd = null;
    }

    public void SetDialogueBoxState(bool active)
    {
        dialogueWindow.enabled = active;
        dialogueWindow.transform.gameObject.SetActive(active);
    }
}
