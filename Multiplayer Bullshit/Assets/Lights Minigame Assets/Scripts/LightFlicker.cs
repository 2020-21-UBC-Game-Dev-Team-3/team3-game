using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightFlicker : MonoBehaviour {

  //public GameObject background;
  //public GameObject light;
  public Light light;
  //public GameObject black;
  //private bool lightIsOn;
  private bool flickerOn;
  //private int timer;
  private float timer;
  //private int[] timerInitialOn = { 60, 60, 70 };
  //private int[] timerInitialOff = { 50, 50, 60 };
  //private int onIndex;
  //private int offIndex;
  //private float onChance = 0.7f;
  //private float rand;
  private bool increasing;


  // Start is called before the first frame update
  void Start() {
    //background = GameObject.FindGameObjectWithTag("Background");
    //light = GameObject.FindGameObjectWithTag("DirLight");
    //black = GameObject.FindGameObjectWithTag("Black");
    light = GetComponent<Light>();
    //lightIsOn = true;
    //onIndex = 0;
    //offIndex = 0;
    timer = 40;
    flickerOn = true;
    increasing = true;
  }

  // Update is called once per frame
  void Update() {
    if (flickerOn) {
            //Flicker();
            gradualFlicker();

            //light.intensity = Mathf.PingPong(Time.time * 2, 0.7f);
            
    }
  }

void gradualFlicker()
    {
        if (timer <= 0)
        {
            increasing = true;
        }
        if (timer >= 115)
        {
            increasing = false;
        }

        if (timer >= 0 && timer <= 80)
        {
            light.intensity = timer / 200;
        }

        if (increasing)
        {
            timer += 2;
        } else
        {
            timer -= 2;
        }
    }

  /*void Flicker() {
    if (timer == 0) {
      if (lightIsOn) {
        timer = timerInitialOn[onIndex];
        onIndex++;
      } else {
        timer = timerInitialOff[offIndex];
        offIndex++;
      }
    }

    if (timer <= 30) {
      if (Random.Range(0.0f, 1.0f) <= onChance) {
        lightOn();
        timer = timerInitialOn[onIndex];
            } else {
        lightOff();
        timer = timerInitialOff[offIndex];
        }
    }

    if (onIndex == timerInitialOn.Length) {
      onIndex = 0;
    }
    if (offIndex == timerInitialOff.Length) {
      offIndex = 0;
    }

    timer--;
  }*/

  void lightOn() {
    //light.SetActive(true);
    //background.SetActive(true);
    //black.SetActive(false);
    light.intensity = 0.8f;
    //lightIsOn = true;
  }

  /*void lightOff() {
    //light.SetActive(false);
    //background.SetActive(false);
    //black.SetActive(true);
    lightIsOn = false;
  }*/

  public void FreezeTime() {
    flickerOn = false;
    lightOn();
    Time.timeScale = 0;
  }

}
