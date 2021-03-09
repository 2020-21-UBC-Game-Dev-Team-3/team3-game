using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Switch : MonoBehaviour {

  public bool isOn;

  Animator animator;

  [SerializeField] Material myMaterial;

  // Start is called before the first frame update
  void Start() {
    isOn = false;
    animator = GetComponent<Animator>();
    animator.SetBool("lightOn", false);
    myMaterial.color = Color.red;
    }

    public void TurnOn() {
    isOn = true;
    animator.SetBool("lightOn", true);
    myMaterial.color = Color.green;
    }


}
