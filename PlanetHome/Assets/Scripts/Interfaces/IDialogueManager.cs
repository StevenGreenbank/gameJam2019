using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//public delegate void OnDialogueEnd();

public delegate void OnDialogueEnd();
public interface IDialogueManager
{
    event OnDialogueEnd dialogueEnd;

    void HideDialogueBox();
    void ShowDialogueBox();
    void StartDialogue(Dialogue dialogue);
    void DisplayNextSentence();
    void EndDialogue();
    void SetDialogueBoxState(bool active);
}
