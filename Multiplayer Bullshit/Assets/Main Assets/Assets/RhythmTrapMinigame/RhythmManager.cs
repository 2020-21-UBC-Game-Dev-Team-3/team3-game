using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RhythmManager : MonoBehaviour
{

    public GameObject x, c, v, b, n, m;
    public KeyActivateScript xScript, cScript, vScript, bScript, nScript, mScript;
    bool keyDown = false;

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
        if (Input.GetKeyDown("x") && !keyDown)
        {
            StartCoroutine(pressX());

        } else if (Input.GetKeyDown("c") && !keyDown)
        {
            StartCoroutine(pressC());
        }
        else if (Input.GetKeyDown("v") && !keyDown)
        {
            StartCoroutine(pressV());
        }
        else if (Input.GetKeyDown("b") && !keyDown)
        {
            StartCoroutine(pressB());
        }
        else if (Input.GetKeyDown("n") && !keyDown)
        {
            StartCoroutine(pressN());
        }
        else if (Input.GetKeyDown("m") && !keyDown)
        {
            StartCoroutine(pressM());
        }
    }

    IEnumerator pressX()
    {
        cScript.keyDown = true;
        vScript.keyDown = true;
        bScript.keyDown = true;
        nScript.keyDown = true;
        mScript.keyDown = true;
        keyDown = true;

        yield return new WaitForSeconds(0.2f);

        cScript.keyDown = false;
        vScript.keyDown = false;
        bScript.keyDown = false;
        nScript.keyDown = false;
        mScript.keyDown = false;
        keyDown = false;
    }

    IEnumerator pressC()
    {
        xScript.keyDown = true;
        vScript.keyDown = true;
        bScript.keyDown = true;
        nScript.keyDown = true;
        mScript.keyDown = true;
        keyDown = true;

        yield return new WaitForSeconds(0.2f);

        xScript.keyDown = false;
        vScript.keyDown = false;
        bScript.keyDown = false;
        nScript.keyDown = false;
        mScript.keyDown = false;
        keyDown = false;
    }

    IEnumerator pressV()
    {
        cScript.keyDown = true;
        xScript.keyDown = true;
        bScript.keyDown = true;
        nScript.keyDown = true;
        mScript.keyDown = true;
        keyDown = true;

        yield return new WaitForSeconds(0.2f);

        cScript.keyDown = false;
        xScript.keyDown = false;
        bScript.keyDown = false;
        nScript.keyDown = false;
        mScript.keyDown = false;
        keyDown = false;
    }

    IEnumerator pressB()
    {
        cScript.keyDown = true;
        vScript.keyDown = true;
        xScript.keyDown = true;
        nScript.keyDown = true;
        mScript.keyDown = true;
        keyDown = true;

        yield return new WaitForSeconds(0.2f);

        cScript.keyDown = false;
        vScript.keyDown = false;
        xScript.keyDown = false;
        nScript.keyDown = false;
        mScript.keyDown = false;
        keyDown = false;
    }

    IEnumerator pressN()
    {
        cScript.keyDown = true;
        vScript.keyDown = true;
        bScript.keyDown = true;
        xScript.keyDown = true;
        mScript.keyDown = true;
        keyDown = true;

        yield return new WaitForSeconds(0.2f);

        cScript.keyDown = false;
        vScript.keyDown = false;
        bScript.keyDown = false;
        xScript.keyDown = false;
        mScript.keyDown = false;
        keyDown = false;
    }

    IEnumerator pressM()
    {
        cScript.keyDown = true;
        vScript.keyDown = true;
        bScript.keyDown = true;
        nScript.keyDown = true;
        xScript.keyDown = true;
        keyDown = true;

        yield return new WaitForSeconds(0.2f);

        cScript.keyDown = false;
        vScript.keyDown = false;
        bScript.keyDown = false;
        nScript.keyDown = false;
        xScript.keyDown = false;
        keyDown = false;
    }
}
