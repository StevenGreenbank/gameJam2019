using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;
using System.IO;
using System.Text.RegularExpressions;

public class GameManager : Singleton<GameManager>
{
    public DialogueManager dialogueManager;
    public BackgroundManager backgroundManager;
    public SpriteManager spriteManager;
    public AudioManager audioManager;

    public TextAsset instructionsScript;
    public TextAsset[] scripts;
    private string[] scriptLines;
    private List<Instruction> instructions;

    // Start is called before the first frame update
    void Start()
    {
    }

    /// <summary>
    /// Take a script name file, load lines into scriptLines
    /// </summary>
    public void LoadScript(TextAsset script)
    {
        instructions = new List<Instruction>();
        instructionsScript = script;
        // code to load a script from file
        string fs = instructionsScript.text;
        scriptLines = Regex.Split(fs, "\n|\r|\r\n");

        ParseScript();
        RunScript();
    }

    /// <summary>
    /// take raw script lines, convert them to commands.
    /// </summary>
    void ParseScript()
    {
        foreach (string line in scriptLines)
        {
            if (line.Trim().Length > 0)
            {
                string[] split = line.Split(',');
                string command = split[0].Trim();
                var splitList = split.ToList().Select(x => x.Trim()).ToList();
                splitList.RemoveAt(0);
                split = splitList.ToArray();
                instructions.Add(new Instruction(command, split));
            }
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
                int timer = Int32.Parse(instruction.variables[0]);
                StartCoroutine(Wait(timer));
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
            case "closedialogue":
                CloseDialogue();
                break;
            case "hotspot":
                Hotspot(instruction.variables[0], instruction.variables[1], Int32.Parse(instruction.variables[2]), Int32.Parse(instruction.variables[3]));
                break;
            default:
                break;
        }
    }


    public TextAsset FindScriptFromName(string scriptName)
    {
        TextAsset result = scripts.FirstOrDefault(x => x.name == scriptName);
        return result;
    }
    private void RunHotSpotDelegate(TextAsset script)
    {
        LoadScript(script);
    }

    private void Hotspot(string spriteTag, string scriptName, int x, int y)
    {
        Vector3 coordinates = new Vector3(x, y, 0f);

        TextAsset script = FindScriptFromName(scriptName);

        spriteManager.CreateHotspotSprite(spriteTag, script, coordinates);

    }

    private void CloseDialogue()
    {
        dialogueManager.HideDialogueBox();
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

    IEnumerator Wait(int seconds)
    {
        //System.Threading.Thread.Sleep(seconds * 1000);
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
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
