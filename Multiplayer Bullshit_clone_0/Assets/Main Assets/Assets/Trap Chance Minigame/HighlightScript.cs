using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HighlightScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnMouseOver()
    {
        gameObject.transform.GetChild(0).gameObject.SetActive(true);

    }

    void OnMouseExit()
    {

        gameObject.transform.GetChild(0).gameObject.SetActive(false);

    }
}
