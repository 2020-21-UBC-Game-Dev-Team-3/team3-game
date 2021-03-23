using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;
using System.IO;

public class TrapAbility : RoleAbility {

  public bool isTouchingTrap = false; // checks if Trapper is too close to another trap
  [SerializeField] GameObject trapPrefab; // the trap - what gets instantiated
  TrapperUI trapperUI; // the UI which displays how many traps are active

  private int maxNumberTraps = 3;
  private int currNumberTraps = 0;

    private void Awake()
    {
        StartCoroutine(InitiateCooldown());
    }

    private void Start() {
    trapperUI = FindObjectOfType<TrapperUI>();
    trapperUI.SetText("Traps active: " + currNumberTraps.ToString() + "/" + maxNumberTraps.ToString());
  }

  public override void UseAbility() {
    if (currNumberTraps < maxNumberTraps && !isTouchingTrap) {
      Vector3 tempPos = transform.position;
      /*      BroadcastMessage("InstantiateTrap", tempPos);*/
      FindObjectOfType<TrapManager>().InstantiateTrap(tempPos);
      currNumberTraps++;
      trapperUI.SetText("Traps active: " + currNumberTraps.ToString() + "/" + maxNumberTraps.ToString());
    }
  }

  public void DecrementTraps() {
    Debug.Log("DecrementTraps");
    GetComponent<PhotonView>().RPC("RPC_DecrementTraps", RpcTarget.All);
  }

  [PunRPC]
  public void RPC_DecrementTraps() {
    if (!isActiveAndEnabled) return;
    currNumberTraps--;
    trapperUI.SetText("Traps active: " + currNumberTraps.ToString() + "/" + maxNumberTraps.ToString());
    isTouchingTrap = false;
  }
}