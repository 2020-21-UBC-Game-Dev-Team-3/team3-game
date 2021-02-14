using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectHighlightScript : MonoBehaviour
{
    public GameObject playWires;
    public RhythmTrapScript rhythmTrapScript;
    bool waiting;

    // Start is called before the first frame update
    void Start()
    {
        rhythmTrapScript = playWires.GetComponent<RhythmTrapScript>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnMouseOver()
    {
        print(rhythmTrapScript.waiting);
        if (!rhythmTrapScript.waiting)
        {
            gameObject.transform.GetChild(0).gameObject.SetActive(true);
        }
    }

    void OnMouseExit()
    {
        
        gameObject.transform.GetChild(0).gameObject.SetActive(false);
        
    }


}
