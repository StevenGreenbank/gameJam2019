using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using YamlDotNet;

public class BackgroundManager : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject Background;

    void Start()
    {
        
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
}
