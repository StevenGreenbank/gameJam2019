using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class GameManager : Singleton<GameManager>
{
    DialogueTrigger dialogueTrigger = new DialogueTrigger();

    DialogueManager dialogueManager = new DialogueManager();
    BackgroundManager backgroundManager = new BackgroundManager();
    SpriteManager spriteManager = new SpriteManager();
    AudioManager audioManager = new AudioManager();

    public string scriptName;
    private string[] scriptLines;
    private List<Instruction> instructions;

    // Start is called before the first frame update
    void Start()
    {
  
    }

    /// <summary>
    /// Take a script name file, load lines into scriptLines
    /// </summary>
    void LoadScript()
    {
        // code to load a script from file
    }

    /// <summary>
    /// take raw script lines, convert them to commands.
    /// </summary>
    void ParseScript()
    {
        foreach (string line in scriptLines)
        {
            string[] split = line.Split(',');
            string command = split[0].Trim();
            var splitList = split.ToList().Select(x => x.Trim()).ToList();
            splitList.RemoveAt(0);
            split = splitList.ToArray();
            instructions.Add(new Instruction(command, split));
        }
    }

    /// <summary>
    /// Run all commands in the instructions list
    /// </summary>
    void RunScript()
    {
        foreach(Instruction instruction in instructions)
        {
            RunInstruction(instruction);
        }
    }

    void RunInstruction(Instruction instruction)
    {
        switch (instruction.command.ToLower())
        {
            case "background":
                SetBackground(instruction.variables[0]);
                break;
            case "addsprite":
                AddSprite(instruction.variables[0], Int32.Parse(instruction.variables[1]), Int32.Parse(instruction.variables[2]));
                break;
            case "replacesprite":
                ReplaceSprite(instruction.variables[0], instruction.variables[1]);
                break;
            case "removeallsprites":
                RemoveAllSprites();
                break;
            case "wait":
                Wait(Int32.Parse(instruction.variables[0]));
                break;
            case "playmusic":
                bool loop = false;
                if (instruction.variables.Length == 2)
                    loop = Boolean.Parse(instruction.variables[1]);
                PlayMusic(instruction.variables[0], loop);
                break;
            case "playsound":
                PlaySound(instruction.variables[0]);
                break;
            case "stopmusic":
                StopMusic(instruction.variables[0]);
                break;
            case "dialogue":
                ShowDialogue(instruction.variables);
                break;
            default:
                break;
        }
    }

    private void ShowDialogue(string[] variables)
    {
        string name = variables[0];
        var trimmed = variables.ToList();
        trimmed.RemoveAt(0);
        dialogueManager.StartDialogue(new Dialogue(name, trimmed.ToArray()));
    }

    private void StopMusic(string musicTag)
    {
        audioManager.Stop(musicTag);
    }

    private void PlaySound(string soundTag)
    {
        audioManager.Play(soundTag);
    }

    private void PlayMusic(string songName, bool loop)
    {
        audioManager.Play(songName);
    }

    IEnumerable Wait(int seconds)
    {
        yield return new WaitForSeconds(seconds);
    }

    private void RemoveAllSprites()
    {
        spriteManager.RemoveAllSprites();
    }

    private void ReplaceSprite(string oldSpriteTag, string newSpriteTag)
    {
        spriteManager.ChangeSprite(oldSpriteTag, newSpriteTag);
    }

    void SetBackground(string backgroundTag)
    {
        backgroundManager.SetBackground(backgroundTag);
    }

    void AddSprite(string spriteTag, int x, int y)
    {
        Vector3 coordinates = new Vector3(x, y, 0f);
        spriteManager.AddSprite(coordinates, spriteTag);
    }


    /// <summary>
    /// Event to continue running script when we hit a dialogue or something
    /// </summary>
    void ContinueRunningScript()
    {

    }

    private void SetDialogue(string name, string[] sentences)
    {
        Dialogue dialogue = new Dialogue(name, sentences);
        dialogueTrigger.dialogue = dialogue;
        dialogueTrigger.TriggerDialogue();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
