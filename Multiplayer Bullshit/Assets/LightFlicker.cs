using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightFlicker : MonoBehaviour
{
    public GameObject background;
    public GameObject light;
    private bool lightIsOn;
    private int timer;
    //private int[] timerInitialOn = { 55, 55, 55, 70};
    //private int[] timerInitialOff = { 80, 80, 100 };
    private int[] timerInitialOn = { 110, 110, 110, 140};
    private int[] timerInitialOff = { 160, 160, 200 };
    private int onIndex;
    private int offIndex;
    private float onChance = 0.7f;
    private float rand;


    // Start is called before the first frame update
    void Start()
    {
        background = GameObject.FindGameObjectWithTag("Background");
        light = GameObject.FindGameObjectWithTag("DirLight");
        lightIsOn = true;
        onIndex = 0;
        offIndex = 0;
        timer = 30;

    }

    // Update is called once per frame
    void Update()
    {
        if (timer == 0)
        {
            if (lightIsOn)
            {
                timer = timerInitialOn[onIndex];
                onIndex++;
            }
            else
            {
                timer = timerInitialOff[offIndex];
                offIndex++;
            }
        }

        if (timer <= 30)
        {
            if (Random.Range(0.0f, 1.0f) <= onChance)
            {
                lightOn();
            } 
            else
            {
                lightOff();
            }
        }

        if (onIndex == timerInitialOn.Length)
        {
            onIndex = 0;
        }
        if (offIndex == timerInitialOff.Length)
        {
            offIndex = 0;
        }

        timer--;
    }

    void lightOn()
    {
        light.SetActive(true);
        background.SetActive(true);
        lightIsOn = true;
    }

    void lightOff()
    {
        light.SetActive(false);
        background.SetActive(false);
        lightIsOn = false;
    }
}
