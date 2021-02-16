using UnityEngine;
using Photon.Pun;

public abstract class Interactable : MonoBehaviourPunCallbacks
{
    public string interactableName;

    public Transform interactableTransform;

    [SerializeField] GameObject indicator;

    Outline outline;

    void Awake() => outline = GetComponent<Outline>();

    public override void OnEnable() => indicator = interactableTransform.GetChild(0).gameObject;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && other.gameObject.GetComponent<PlayerActionController>().pv.IsMine)
        {
            indicator.SetActive(true);
            outline.enabled = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && other.gameObject.GetComponent<PlayerActionController>().pv.IsMine)
        {
            indicator.SetActive(false);
            outline.enabled = false;
        }
    }

}
