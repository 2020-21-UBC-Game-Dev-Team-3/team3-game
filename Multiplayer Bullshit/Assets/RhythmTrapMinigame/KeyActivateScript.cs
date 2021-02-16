﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyActivateScript : MonoBehaviour
{
    public KeyCode key;
    bool active = false;
    GameObject note;
    SpriteRenderer sr;
    Color old;

    public bool createMode;
    public GameObject n;

    public bool keyDown;

    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    // Start is called before the first frame update
    void Start()
    {
        old = sr.color;
    }

    // Update is called once per frame
    void Update()
    {
        if (keyDown)
        {
            return;
        }

        if (createMode) {
            if(Input.GetKeyDown(key)){
                Instantiate(n,transform.position,Quaternion.identity);

            }
        }else{

        
        if (Input.GetKeyDown(key))
        {
            StartCoroutine(Pressed());
        }

        if (Input.GetKeyDown(key) && active)
        {
            Destroy(note);
        }

        }
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        active = true;

        if (col.gameObject.tag == "Note")
        {
            note = col.gameObject;
        }
    }

    void OnTriggerExit2D(Collider2D col)
    {
        active = false;
    }

    IEnumerator Pressed()
    {
        Color old = GetComponent<SpriteRenderer>().color;
        sr.color = new Color(0, 0, 0);

        yield return new WaitForSeconds(0.2f);

        sr.color = old;
    }
}
