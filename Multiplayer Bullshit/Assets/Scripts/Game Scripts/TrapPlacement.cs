using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapPlacement : MonoBehaviour {

  private void OnTriggerEnter(Collider collision) {
    if (collision.gameObject.GetComponent<TrapAbility>() != null) {
      collision.gameObject.GetComponent<TrapAbility>().isTouchingTrap = true;
    }
  }
  private void OnTriggerExit(Collider collision) {
    if (collision.gameObject.GetComponent<TrapAbility>() != null) {
      collision.gameObject.GetComponent<TrapAbility>().isTouchingTrap = false;
    }
  }
}
