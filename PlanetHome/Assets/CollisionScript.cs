using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionScript : MonoBehaviour
{
    public TextAsset script;
    public delegate void FunctionToRun(TextAsset script);
    FunctionToRun funct;
    // Start is called before the first frame update
    void Start()
    {
    }

    public void SetDelegate(FunctionToRun function)
    {
        funct += function;
    }

    private void OnMouseDown()
    {
        Debug.Log("you clicked on this");
        if (funct != null)
            funct(script);
        // remove delegate once it's been clicked on; can reset it with script if necessary
        funct = null;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
