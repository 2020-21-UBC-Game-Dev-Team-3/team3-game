using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System.IO;

public class Assassin : MonoBehaviour {

  private string ability1 = "e";
  private string nameOfWeapon = "Knife";

  private float headLevelHeight = 1;

  [SerializeField] GameObject myTarget;

  private void Update() {
    if (Input.GetKeyDown(ability1) && IsMyController()) { // w/o IsMyController() pressing e would instantiate something for ALL players in the game
      ThrowKnife();
    }
  }

  public bool IsMyController() {
    PhotonView pv = GetComponent<PhotonView>();
    return pv.IsMine;
  }

  private void ThrowKnife() {


    if (myTarget == null) {
      Debug.Log("No valid target selected!");
    } else if (myTarget.tag == "Player") {
      Vector3 offset = transform.forward;
      Vector3 headLevel = new Vector3(transform.position.x, transform.position.y + headLevelHeight, transform.position.z);
      PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", nameOfWeapon), headLevel + offset, Quaternion.identity);
      Debug.Log("knife released");
    }
  }

  public Vector3 GetTargetPosition() {
    if (myTarget != null) {
      Debug.Log(myTarget.transform.position);
      return myTarget.transform.position;
    } else {
      Debug.Log("ERROR: assassin has no target");
      return new Vector3(0, 0, 0);
    }
  }
}
