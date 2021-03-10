using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Role : MonoBehaviour, IPunInstantiateMagicCallback
{
    public enum Roles
    {
        Imposter, Crewmate
    }

    [HideInInspector] public Roles currRole;

    public void OnPhotonInstantiate(PhotonMessageInfo info)
    {
        info.Sender.TagObject = this.gameObject;
    }
}
