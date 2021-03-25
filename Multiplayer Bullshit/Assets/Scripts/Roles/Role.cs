﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Role : MonoBehaviour, IPunInstantiateMagicCallback
{
    public string updatedRoleString;
    public enum Roles
    {
        Imposter, Crewmate, Assassin, Chameleon, Trapper, Disarmer, None
    }

    public Roles currRole;
    public Roles subRole;

    public void OnPhotonInstantiate(PhotonMessageInfo info)
    {
        info.Sender.TagObject = this.gameObject;
    }
}
