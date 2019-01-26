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
        Debug.Log("Starting convo with " + dialogue.name);
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
            EndDialogue();
            return;
        }
        string sentence = sentences.Dequeue();
        dialogueText.text = sentence;
        Debug.Log(sentence);
    }

    public void EndDialogue()
    {
        Debug.Log("End of conversation");
        SetDialogueBoxState(false);
    }

    private void SetDialogueBoxState(bool active)
    {
        image.enabled = active;
        image.transform.gameObject.SetActive(active);
    }
}
