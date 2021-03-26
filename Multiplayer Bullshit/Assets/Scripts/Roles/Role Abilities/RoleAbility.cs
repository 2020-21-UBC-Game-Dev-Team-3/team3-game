using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;

public abstract class RoleAbility : MonoBehaviour {
  bool onCooldown;

  public float cooldownTimer;

  public TextMeshProUGUI abilityText;

  [HideInInspector] public PhotonView pv;

  void Awake() {
    pv = GetComponent<PhotonView>();
  }

  private void Start() {
    SetAbilityText();
  }

  public void InitiateAbility() {
    if (!onCooldown) UseAbility();
  }

  public abstract void UseAbility();

  public abstract void SetAbilityText();

  public virtual IEnumerator InitiateCooldown() {
    onCooldown = true;
    yield return new WaitForSeconds(cooldownTimer);
    onCooldown = false;
  }

    public void RestartCooldown() => StartCoroutine(InitiateCooldown());
}
