using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using YamlDotNet;
using UnityEngine.UI;

public class BackgroundManager : Singleton<BackgroundManager> 
{
    // Start is called before the first frame update
    public Image Background;
    public SpriteRenderer spriteRenderer;

    void Start()
    {
        
    }

    /// <summary>
    /// Change the background to the picture we want and make sure it's visible.
    /// </summary>
    /// <param name="backgroundObject"></param>
    public void SetBackground(Sprite backgroundObject)
    {
        Background.sprite = backgroundObject;
        spriteRenderer.sprite = backgroundObject;
       // Background.SetActive(true);
    }

    // Update is called once per frame
}
