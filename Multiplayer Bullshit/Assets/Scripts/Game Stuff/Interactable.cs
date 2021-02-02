using UnityEngine;
using Photon.Pun;
using cakeslice;

public abstract class Interactable : MonoBehaviourPunCallbacks
{    
    public string interactableName;

    public Transform interactableTransform;

    [SerializeField] GameObject indicator;

    public override void OnEnable() => indicator = interactableTransform.GetChild(0).gameObject;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            indicator.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            indicator.SetActive(false);
        }
    }

}
