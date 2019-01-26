using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using YamlDotNet;

public class SceneManager : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject Background;
    public GameObject[] Sprites;
    private int[] Hotspots;
    public string ScriptFile;

    void Start()
    {
        
    }

    // How to load?  Either load a string file, or try to load json object.  probably load json object, run validation on json object to ensure all variables exist,
    // 
    private void LoadScript(string scriptFile)
    {
        /*
        using (var fs = new StreamReader(scriptFile))
        {
            do
            {
                string line = fs.ReadLine();
                // go to function to parse line?
            } while (!fs.EndOfStream);
        }
        */
    }

    /// <summary>
    /// Change the background to the picture we want and make sure it's visible.
    /// </summary>
    /// <param name="backgroundObject"></param>
    void SetBackground(GameObject backgroundObject)
    {
        Background = backgroundObject;
        Background.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
