using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteTrigger : MonoBehaviour
{
    // Start is called before the first frame update
    public string spriteTag;
    public string oldSpriteTag;
    public Vector3 coordinates;
    public void AddSprite()
    {
        SpriteManager.Instance.AddSprite(coordinates, spriteTag);
    }

    public void RemoveSprite()
    {
        SpriteManager.Instance.RemoveSprite(spriteTag);
    }

    public void ChangeSprite()
    {
        SpriteManager.Instance.ChangeSprite(oldSpriteTag, spriteTag);
    }
}
