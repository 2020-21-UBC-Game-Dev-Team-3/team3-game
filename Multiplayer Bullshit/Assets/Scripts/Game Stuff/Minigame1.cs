using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Minigame1 : Interactable
{

    void Start()
    {
        interactableName = "Minigame1";
    }


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