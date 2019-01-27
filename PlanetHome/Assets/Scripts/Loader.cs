using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Loader : MonoBehaviour
{
    public TextAsset startScript;
    public GameManager gm;
    // Start is called before the first frame update
    public void Start()
    {
        gm.LoadScript(startScript);
    }
}
