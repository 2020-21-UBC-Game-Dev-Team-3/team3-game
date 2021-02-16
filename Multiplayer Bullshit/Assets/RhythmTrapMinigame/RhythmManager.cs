using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RhythmManager : MonoBehaviour
{

    public GameObject x, c, v, b, n, m;
    public KeyActivateScript xScript, cScript, vScript, bScript, nScript, mScript;

    // Start is called before the first frame update
    void Start()
    {
        xScript = x.GetComponent<KeyActivateScript>();
        cScript = c.GetComponent<KeyActivateScript>();
        vScript = v.GetComponent<KeyActivateScript>();
        bScript = b.GetComponent<KeyActivateScript>();
        nScript = n.GetComponent<KeyActivateScript>();
        mScript = m.GetComponent<KeyActivateScript>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("x") || Input.GetKeyDown("c") || Input.GetKeyDown("v") || Input.GetKeyDown("b") || Input.GetKeyDown("n") || Input.GetKeyDown("m"))
        {
            xScript.keyDown = true;
            cScript.keyDown = true;
            vScript.keyDown = true;
            bScript.keyDown = true;
            nScript.keyDown = true;
            mScript.keyDown = true;
        } else
        {
            xScript.keyDown = false;
            cScript.keyDown = false;
            vScript.keyDown = false;
            bScript.keyDown = false;
            nScript.keyDown = false;
            mScript.keyDown = false;
        }
    }
}
