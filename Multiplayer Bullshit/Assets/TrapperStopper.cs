using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapperStopper : MonoBehaviour {

  private void OnTriggerEnter(Collider other) {
    if (!(other.GetComponent<Role>().subRole == Role.Roles.Trapper)) return;
    other.GetComponent<TrapAbility>().isInVotingRoom = true;
  }

  private void OnTriggerExit(Collider other) {
    if (!(other.GetComponent<Role>().subRole == Role.Roles.Trapper)) return;
    other.GetComponent<TrapAbility>().isInVotingRoom = false;
  }
}
