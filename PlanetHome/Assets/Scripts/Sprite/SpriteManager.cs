using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class SpriteManager : Singleton<SpriteManager>
{
    public GameManager gm;
    // holds all the sprites, whether it be game objects or characters
    public GameObject[] sprites;
    private Transform spriteParent;
    // Start is called before the first frame update
    void Start()
    {
        spriteParent = new GameObject("Sprites").transform;
    }


    public GameObject AddSprite(Vector3 coordinates, string spriteTag)
    {
        GameObject sprite = sprites.FirstOrDefault(x => x.tag.Equals(spriteTag, StringComparison.CurrentCultureIgnoreCase));
        if (sprite == null)
        {
            throw new Exception("Sprite " + spriteTag + " not found");
        }
        GameObject instance = Instantiate(sprite, coordinates, Quaternion.identity) as GameObject;
        instance.transform.SetParent(spriteParent);
        return instance;
    }

    public void TestDelegateFunction()
    {
        Debug.Log("delegate function worked");
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

        /*while(spriteParent.transform.childCount > 0)
        {
            Transform child = transform.GetChild(0);
            child.parent = null;
            Destroy(child.gameObject);
        }*/

        var children = GetComponentsInChildren<Transform>(spriteParent);

      //  var children = new List<Transform>(spriteParent.transform);

        foreach (Transform transform in spriteParent.transform)
        {
            Destroy(transform.gameObject);
        }

    }

    internal void CreateHotspotSprite(string spriteTag, TextAsset script, Vector3 coordinates)
    {
        GameObject instance = AddSprite(coordinates, spriteTag);

        instance.AddComponent<CollisionScript>();
        CollisionScript collisionScript = instance.GetComponent<CollisionScript>();
        collisionScript.script = script;
        collisionScript.SetDelegate(RunHotSpotDelegate);
    }

    private void RunHotSpotDelegate(TextAsset script)
    {
        Debug.Log("Before loading script");
        gm.LoadScript(script);
        Debug.Log("After Loading script");
    }
}
