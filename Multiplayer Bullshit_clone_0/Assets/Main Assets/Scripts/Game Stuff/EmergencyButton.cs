using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class EmergencyButton : Interactable
{
    [SerializeField] GameObject indicator;

    void Update()
    {
        if (outline.enabled)
        {
            indicator.SetActive(true);
        }
        else
        {
            indicator.SetActive(false);
        }
    }


    //void Start() => interactableName = "Emergency button";


    //bool isActivated = false;

    //private void Update()
    //{
    //    if(isActivated)
    //    {
    //        interactionEventPV.RPC("TurnOnEmergencyPopUp", RpcTarget.All);
    //    }
    //}


    //public override void Interact(PhotonView pv)
    //{
    //    interactionEvent.GetComponent<InteractionEvent>().pv = pv;
    //    interactionEvent.SetActive(true);

    //}

    //[PunRPC]
    //public void TurnOnEmergencyPopUp()
    //{
    //    StartCoroutine(ShowEmergencyPopUp());
    //}


    //IEnumerator ShowEmergencyPopUp()
    //{
    //    interactionEvent.SetActive(true);
    //    yield return new WaitForSeconds(2);
    //    interactionEvent.SetActive(false);
    //}
}
