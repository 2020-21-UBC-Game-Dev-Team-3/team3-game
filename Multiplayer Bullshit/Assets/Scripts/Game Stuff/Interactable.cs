using UnityEngine;
using Photon.Pun;

public class Interactable : MonoBehaviourPunCallbacks
{
    public string interactableName;

    public string GetInteractableName()
    {
        return interactableName;
    }
}