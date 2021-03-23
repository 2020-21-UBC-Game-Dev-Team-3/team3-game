using UnityEngine;
using Photon.Pun;

public abstract class Interactable : MonoBehaviourPunCallbacks {
  public string interactableName;

  [HideInInspector] public Outline outline;

  void Awake() {
    outline = GetComponent<Outline>();
    outline.enabled = false;
  }

  protected virtual void OnTriggerEnter(Collider other) {
    if (other.CompareTag("Player") && other.gameObject.GetComponent<PlayerActionController>().pv.IsMine) {
      outline.enabled = true;
    }
  }

  protected virtual void OnTriggerExit(Collider other) {
    if (other.CompareTag("Player") && other.gameObject.GetComponent<PlayerActionController>().pv.IsMine) {
      outline.enabled = false;
    }
  }

}