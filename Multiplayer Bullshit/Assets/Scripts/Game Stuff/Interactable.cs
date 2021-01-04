using UnityEngine;
using Photon.Pun;

public class BadInteractable : MonoBehaviourPunCallbacks
{
   public string badInteractableName;

    public string GetInteractableName()
    {
        return badInteractableName;
    }
}