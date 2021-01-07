using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightFlicker : MonoBehaviour {

  public Light light;
  private bool flickerOn;
  private float timer;
  private bool increasing;


  // Start is called before the first frame update
  void Start() {
    light = GetComponent<Light>();
    timer = 40;
    flickerOn = true;
    increasing = true;
  }

  // Update is called once per frame
  void Update() {
    if (flickerOn) {
            gradualFlicker();
            
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
            timer++;
        } else
        {
            timer--;
        }
    }


  void lightOn() {
    light.intensity = 0.8f;
  }

  

  public void FreezeTime() {
    flickerOn = false;
    lightOn();
    Time.timeScale = 0;
  }

}
