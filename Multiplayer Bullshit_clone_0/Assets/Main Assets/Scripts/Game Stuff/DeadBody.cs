using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadBody : Interactable
{
    [SerializeField] GameObject indicator;

    void Update()
    {
        if (outline.enabled)
        {
            indicator.SetActive(true);
        }
        else
        {
            indicator.SetActive(false);
        }
    }


    // Start is called before the first frame update
    //void Start()
    //{
    //    interactableName = "Dead body";
    //}

}
