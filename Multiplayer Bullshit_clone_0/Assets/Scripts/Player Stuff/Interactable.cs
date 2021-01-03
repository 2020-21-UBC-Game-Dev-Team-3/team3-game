using UnityEngine;
using Photon.Pun;

public class Interactable : MonoBehaviourPunCallbacks {
  //private PlayerManager playerManager;
  //public GameObject interactionEvent;
  //public PhotonView interactionEventPV;

  public string interactableName;

  public Transform interactableTransform;

  public GameObject indicator;
  // PhotonView indicatorPV;

  public float radius;

  //[SerializeField] Transform player;

  public override void OnEnable() {
    /*interactionEvent = GameObject.Find("Emergency Pop-Up");*/
    indicator = interactableTransform.GetChild(0).gameObject;
    //indicatorPV = indicator.GetComponent<PhotonView>();
    //interactionEventPV = interactionEvent.GetComponent<PhotonView>();
  }

  private void OnTriggerEnter(Collider other) {
    if (other.CompareTag("Player")) {
      indicator.SetActive(true);
    }
  }

  private void OnTriggerExit(Collider other) {
    if (other.CompareTag("Player")) {
      indicator.SetActive(false);
    }
  }


  //void Update()
  //{
  //    if (!indicatorPV.IsMine) return;
  //}



  public string GetInteractableName() {
    return interactableName;
  }

  private void OnDrawGizmosSelected() {
    Gizmos.color = Color.cyan;
    Gizmos.DrawWireSphere(transform.position, radius);
  }
}
