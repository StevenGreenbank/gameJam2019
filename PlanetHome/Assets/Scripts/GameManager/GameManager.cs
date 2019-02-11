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
    public VideoManager videoManager;
    public AudioManager soundManager;

    // current script to load
    public TextAsset instructionsScript;
    // list of all scripts, for searching for script via name
    public TextAsset[] scripts;
    // list of instructions to run
    private Queue<Instruction> instructions;

    private Dictionary<string, int> gameValues;

    // Start is called before the first frame update
    void Start()
    {
        gameValues = new Dictionary<string, int>();
    }

    /// <summary>
    /// Take a script name file, load lines into scriptLines
    /// </summary>
    public void LoadScript(TextAsset script)
    {
        instructions = new Queue<Instruction>();
        instructionsScript = script;
        // code to load a script from file
        string fs = instructionsScript.text;
        string[] scriptLines = Regex.Split(fs, "\n|\r|\r\n");

        ParseScript(scriptLines);
        RunScript();
    }

    /// <summary>
    /// take raw script lines, convert them to commands.
    /// </summary>
    void ParseScript(string[] scriptLines)
    {
        foreach (string line in scriptLines)
        {
            if (line.Trim().Length > 0)
            {
                string[] split = line.Split('~');
                string command = split[0].Trim();
                var splitList = split.ToList().Select(x => x.Trim()).ToList();
                splitList.RemoveAt(0);
                split = splitList.ToArray();
                instructions.Enqueue(new Instruction(command, split));
            }
        }
    }

    /// <summary>
    /// Run all commands in the instructions list
    /// </summary>
    void RunScript()
    {
        if (instructions.Count > 0)
        {
            Instruction instruction = instructions.Dequeue();
            RunInstruction(instruction);
        }
    }

    void RunInstruction(Instruction instruction)
    {
        bool runNextInstruction = true;
        switch (instruction.command.ToLower())
        {
            case "comment":
                break;
            case "else":
                RemoveUnusedBranchCommands();
                break;
            case "loadscript":
                TextAsset asset = FindScriptFromName(instruction.variables[0]);
                LoadScript(asset);
                break;
            case "background":
                SetBackground(instruction.variables[0]);
                break;
            case "addsprite":
                AddSprite(instruction.variables[0], Single.Parse(instruction.variables[1]), Single.Parse(instruction.variables[2]));
                break;
            case "replacesprite":
                ReplaceSprite(instruction.variables[0], instruction.variables[1]);
                break;
            case "removeallsprites":
                RemoveAllSprites();
                break;
            case "wait":
                float timer = float.Parse(instruction.variables[0]);
                runNextInstruction = false;
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
                runNextInstruction = false;
                break;
            case "closedialogue":
                CloseDialogue();
                break;
            case "hotspot":
                Hotspot(instruction.variables[0], instruction.variables[1], Single.Parse(instruction.variables[2]), Single.Parse(instruction.variables[3]));
                break;
            case "set":
                SetValue(instruction.variables[0], Int32.Parse(instruction.variables[1]));
                break;
            case "add":
                AddValue(instruction.variables[0], instruction.variables[1]);
                break;
            case "subtract":
                SubtractValue(instruction.variables[0], instruction.variables[1]);
                break;
            case "if":
                BranchStatement(instruction.variables[0], instruction.variables[1], instruction.variables[2]);
                break;
            case "playvideo":
                PlayVideo(instruction.variables[0]);
                break;
            case "stopvideo":
                StopVideo(instruction.variables[0]);
                break;
            default:
                break;
        }
        if (runNextInstruction)
            RunScript();
    }

    private void PlayVideo(string videoName)
    {
        videoManager.PlayMovie(videoName);
    }
    private void StopVideo(string videoName)
    {
        videoManager.StopMovie(videoName);
    }
    private void RemoveUnusedBranchCommands()
    {
        Instruction trash;
        do
        {
            if (instructions.Count > 0)
            {
                trash = instructions.Dequeue();
            }
            else
            {
                throw new Exception("Encountered an if statement without an end!");
            }

        } while (trash.command != "else" && trash.command != "end");
    }
    private void BranchStatement(string value1, string operation, string value2)
    {
        bool result = false;
        int intValue1;
        int intValue2;
        if (!Int32.TryParse(value1, out intValue1))
        {
            intValue1 = gameValues[value1];
        }
        if (!Int32.TryParse(value2, out intValue2))
        {
            intValue2 = gameValues[value2];
        }

        switch (operation)
        {
            case "=":
                if (intValue1 == intValue2)
                    result = true;
                break;
            case ">":
                if (intValue1 > intValue2)
                    result = true;
                break;
            case "<":
                if (intValue1 < intValue2)
                    result = false;
                break;
        }

        if (!result)
        {
            RemoveUnusedBranchCommands();
        }
    }

    private void SubtractValue(string key, string strValue)
    {
        int value;
        if (!Int32.TryParse(strValue, out value))
        {
            value = gameValues[strValue];
        }
        if (gameValues.ContainsKey(key))
        {
            gameValues[key] = gameValues[key] - value;
        }
        else
            throw new Exception("Key " + key + " not found");
    }

    private void AddValue(string key, string strValue)
    {
        int value;
        if (!Int32.TryParse(strValue, out value))
        {
            value = gameValues[strValue];
        }
        if (gameValues.ContainsKey(key))
        {
            gameValues[key] = gameValues[key] + value;
        }
        else
        {
            gameValues[key] = value;
        }
    }

    private void SetValue(string key, int value)
    {
        if (gameValues.ContainsKey(key))
        {
            gameValues[key] = value;
        }
        else
        {
            gameValues[key] = value;
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

    private void Hotspot(string spriteTag, string scriptName, float x, float y)
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
        // horrible hack
        if (dialogueManager.dialogueEnd == null)
            dialogueManager.dialogueEnd += RunScript;
        dialogueManager.StartDialogue(new Dialogue(name, trimmed.ToArray()));
    }



    private void StopMusic(string musicTag)
    {
        audioManager.Stop(musicTag);
    }

    private void PlaySound(string soundTag)
    {
        soundManager.Play(soundTag);
    }

    private void PlayMusic(string songName, bool loop)
    {
        audioManager.Play(songName);
    }

    IEnumerator Wait(float seconds)
    {
        //System.Threading.Thread.Sleep(seconds * 1000);
        yield return new WaitForSeconds(seconds);
        RunScript();
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

    void AddSprite(string spriteTag, float x, float y)
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
