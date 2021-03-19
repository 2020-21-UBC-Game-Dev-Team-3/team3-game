using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightsMinigame : Interactable
{
    public bool lightsOff;
    
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (lightsOff) outline.enabled = true;
        else outline.enabled = false;
    }
}
