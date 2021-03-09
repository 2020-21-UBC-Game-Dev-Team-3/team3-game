using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public abstract class RoleAbility : MonoBehaviour
{
    bool onCooldown;

    public float cooldownTimer;

    [HideInInspector] public PhotonView pv;

    void Awake()
    {
        pv = GetComponent<PhotonView>();
    }

    public void InitiateAbility()
    {
        if (!onCooldown) UseAbility(); 
    }

    public abstract void UseAbility();

    public virtual IEnumerator InitiateCooldown()
    {
        onCooldown = true;
        yield return new WaitForSeconds(cooldownTimer);
        onCooldown = false;
    }
}
