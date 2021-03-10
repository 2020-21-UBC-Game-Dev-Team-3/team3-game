using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;

public class TrapAbility : RoleAbility {

  [SerializeField] GameObject trapPrefab; // the trap - what gets instantiated
  TrapperUI trapperUI; // the UI which displays how many traps are active

  private float timeBeforeTrapDeploy = 2f;
  private int maxNumberTraps = 3;
  private int currNumberTraps = 0;

  private void Start() {
    trapperUI = FindObjectOfType<TrapperUI>();
    trapperUI.SetText("Traps active: " + currNumberTraps.ToString() + "/" + maxNumberTraps.ToString());
  }

  public override void UseAbility() {
    if (currNumberTraps < maxNumberTraps) {
      pv.RPC("SpawnTrap", RpcTarget.All);
      currNumberTraps++;
      trapperUI.SetText("Traps active: " + currNumberTraps.ToString() + "/" + maxNumberTraps.ToString());
    }
  }

  [PunRPC]
  public void SpawnTrap() {
    StartCoroutine(SpawnTrapCoroutine());
  }

  IEnumerator SpawnTrapCoroutine() {
    Vector3 tempPos = transform.position;
    yield return new WaitForSeconds(0);
    Instantiate(trapPrefab, tempPos, Quaternion.identity);
  }

  public void DecrementCurrTraps() {
    currNumberTraps--;
    trapperUI.SetText("Traps active: " + currNumberTraps.ToString() + "/" + maxNumberTraps.ToString());
  }
}