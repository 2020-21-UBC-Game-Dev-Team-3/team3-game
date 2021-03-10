using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapDeath : MonoBehaviour {

  private float timeBeforeActive = 2f;
  [SerializeField] MeshRenderer needle;
  [SerializeField] MeshRenderer needleBase;

  [SerializeField] Material normal;
  [SerializeField] Material transparent;

  private void Start() {
    needle.material = transparent;
    needleBase.material = transparent;
    GetComponent<Collider>().enabled = false; // disables the collider
    StartCoroutine(CountdownBeforeActive());
  }

  IEnumerator CountdownBeforeActive() {
    yield return new WaitForSeconds(timeBeforeActive);
    GetComponent<Collider>().enabled = true; // enables the collider
    needle.material = normal;
    needleBase.material = normal;
  }

  private void OnCollisionEnter(Collision collision) {
    collision.rigidbody.GetComponent<PlayerActionController>().TakeHit();
  }

}
