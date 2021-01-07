using UnityEngine;
using Photon.Pun;

public class Interactable : MonoBehaviourPunCallbacks
{
    public string interactableName;

    public Transform interactableTransform;

    public GameObject indicator;

    public float radius;

    public override void OnEnable()
    {
        indicator = interactableTransform.GetChild(0).gameObject;
    }

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

    public string GetInteractableName()
    {
        return interactableName;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}