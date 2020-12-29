using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Switch : MonoBehaviour {

  public bool isOn;

  // Start is called before the first frame update
  void Start() {
    isOn = false;
  }

  public void TurnOn() {
    isOn = true;
  }

  // Update is called once per frame
  void Update() {

  }
}
