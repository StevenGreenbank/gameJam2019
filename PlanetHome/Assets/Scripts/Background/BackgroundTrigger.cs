using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundTrigger : MonoBehaviour
{
    public Sprite Background;
    public void TriggerBackground()
    {
        BackgroundManager.Instance.SetBackground(Background);
    }
}
