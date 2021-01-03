using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knife : MonoBehaviour {

  Vector3 targetPosition;
  Assassin myAssassin;
  float knifeSpeed = 3f;

  // Start is called before the first frame update
  void Start() {
    OrientKnife();
    FindMyAssassin();
  }

  void OrientKnife() {
  }

  void FindMyAssassin() {
    Assassin[] assassinList = FindObjectsOfType<Assassin>();
    foreach (Assassin a in assassinList) {
      Debug.Log(a.IsMyController());
      if (a.IsMyController()) {
        myAssassin = a;
      }
    }
    targetPosition = myAssassin.GetTargetPosition();
  }

  private void Update() {
    transform.position = Vector3.MoveTowards(transform.position, targetPosition, knifeSpeed * Time.deltaTime);
    transform.LookAt(targetPosition);
  }

  private void OnCollisionEnter(Collision collision) { // TEMPORARY KILL METHOD, DELETE THIS METHOD AFTER KILL MECHANIC IMPLEMENTED
    if (collision.gameObject.tag == "Player") {
      Destroy(collision.gameObject);
    }
  }

}
