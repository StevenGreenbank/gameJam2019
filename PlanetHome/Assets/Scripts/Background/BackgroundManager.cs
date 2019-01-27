using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using YamlDotNet;
using UnityEngine.UI;
using System.Linq;
using System;

public class BackgroundManager : Singleton<BackgroundManager> 
{
    // Start is called before the first frame update
    public Sprite Background;
    public SpriteRenderer spriteRenderer;
    public GameObject[] backgrounds;

    public void HideBackground()
    {
        // Background. = false;
        spriteRenderer.enabled = false;
    }

    public void ShowBackground()
    {
        //Background.enabled = true;
        spriteRenderer.enabled = true;
    }


    /// <summary>
    /// Change the background to the picture we want and make sure it's visible.
    /// </summary>
    /// <param name="backgroundObject"></param>
    public void SetBackground(Sprite backgroundObject)
    {
        //Background.sprite = backgroundObject;
        spriteRenderer.sprite = backgroundObject;
        ScaleBackground();
       // Background.SetActive(true);
    }

    public void SetBackground(string backgroundTag)
    {
        GameObject background = backgrounds.FirstOrDefault(x => x.tag.Equals(backgroundTag, StringComparison.CurrentCultureIgnoreCase));
        SpriteRenderer picture = background.GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = picture.sprite;
        ScaleBackground();
    }


    private void ScaleBackground()
    {
        float cameraHeight = Camera.main.orthographicSize * 2;
        Vector2 cameraSize = new Vector2(Camera.main.aspect * cameraHeight, cameraHeight);
        Vector2 spriteSize = spriteRenderer.sprite.bounds.size;

        Vector2 scale = transform.localScale;
        if (cameraSize.x >= cameraSize.y)
        { // Landscape (or equal)
            scale *= cameraSize.x / spriteSize.x;
        }
        else
        { // Portrait
            scale *= cameraSize.y / spriteSize.y;
        }

        transform.position = Vector2.zero; // Optional
        transform.localScale = scale;
    }
    // Update is called once per frame
}
