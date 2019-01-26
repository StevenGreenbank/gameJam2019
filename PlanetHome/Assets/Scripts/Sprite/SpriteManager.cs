using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class SpriteManager : Singleton<SpriteManager>
{
    // holds all the sprites, whether it be game objects or characters
    public GameObject[] sprites;
    private Transform spriteParent;
    // Start is called before the first frame update
    void Start()
    {
        spriteParent = new GameObject("Sprites").transform;
    }


    public void AddSprite(Vector3 coordinates, string spriteTag)
    {
        GameObject sprite = sprites.FirstOrDefault(x => x.tag.Equals(spriteTag, StringComparison.CurrentCultureIgnoreCase));
        if (sprite == null)
        {
            throw new Exception("Sprite " + spriteTag + " not found");
        }
        GameObject instance = Instantiate(sprite, coordinates, Quaternion.identity) as GameObject;
        instance.transform.SetParent(spriteParent);
    }

    public void ChangeSprite(string oldSpriteTag, string newSpriteTag)
    {
        foreach (Transform child in spriteParent.transform)
        {
            if (child.tag.Equals(oldSpriteTag, StringComparison.CurrentCultureIgnoreCase))
            {
                GameObject go = child.gameObject;
                Vector3 location = child.position;
                Destroy(go);
                AddSprite(location, newSpriteTag);
                break;
            }
        }
    }

    public void RemoveSprite(string spriteTag)
    {
        foreach (Transform transform in spriteParent.transform)
        {
            if (transform.tag.Equals(spriteTag, StringComparison.CurrentCultureIgnoreCase))
            {
                transform.SetParent(null);
                Destroy(transform.gameObject);
                break;
            }
        }
    }

    public void RemoveAllSprites()
    {
        foreach (Transform transform in spriteParent.transform)
        {
            transform.SetParent(null);
            Destroy(transform.gameObject);
        }

    }

    // Update is called once per frame
}
