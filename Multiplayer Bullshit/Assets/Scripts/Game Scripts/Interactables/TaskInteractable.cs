using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public abstract class TaskInteractable : Interactable
{
    protected override void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<PhotonView>().IsMine && (other.gameObject.GetComponent<Role>().currRole == Role.Roles.Crewmate)
            && (other.CompareTag("Player") || other.CompareTag("Ghost")))
        {
            outline.enabled = true;
        }
    }

    protected override void OnTriggerExit(Collider other)
    {
        if (other.gameObject.GetComponent<PhotonView>().IsMine && (other.gameObject.GetComponent<Role>().currRole == Role.Roles.Crewmate)
            && (other.CompareTag("Player") || other.CompareTag("Ghost")))
        {
            outline.enabled = false;
        }
    }

}