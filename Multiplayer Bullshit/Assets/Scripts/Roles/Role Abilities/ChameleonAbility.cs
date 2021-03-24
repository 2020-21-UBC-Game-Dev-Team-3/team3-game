using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;

public class ChameleonAbility : RoleAbility {
  float chamRange = 4;


  Camera cam;
  GameObject reticle;
  Vector3 reticlePosition;

  public PhotonView photonView;

  private void Awake() {
    cooldownTimer = 0;
    StartCoroutine(InitiateCooldown());
    photonView = GetComponent<PhotonView>();
  }

  private void Start() {
    cam = Camera.main;
    reticle = GameObject.Find("Assassin Reticle");
    reticlePosition = reticle.GetComponent<RectTransform>().transform.position;
  }

  public override void SetAbilityText() {
    abilityText = GameObject.Find("Main Camera/Ability Text Canvas/AbilityText").GetComponent<TextMeshProUGUI>();
    abilityText.text = "Chameleon";
  }

  public override void UseAbility() => StartCoroutine(ChamKill());

  IEnumerator ChamKill() {
    Ray ray = cam.ScreenPointToRay(reticlePosition);
    ray.origin = cam.transform.position;

    if (Physics.Raycast(ray, out RaycastHit hit, chamRange)) {
      Debug.Log(hit.collider.gameObject.name);

      if (hit.collider.CompareTag("Player") && !hit.collider.gameObject.GetComponent<PhotonView>().IsMine) {
        Debug.Log(hit.collider.gameObject.GetComponent<PhotonView>().ViewID);
        photonView.RPC("KillTarget", RpcTarget.All, hit.collider.gameObject.GetComponent<PhotonView>().ViewID);
        yield return StartCoroutine(StartChameleon());
        yield return StartCoroutine(InitiateCooldown());
      }
    }
  }

  [PunRPC]
  public void KillTarget(int targetID) {
    Transform target = PhotonView.Find(targetID).transform;
    target.GetComponent<IDamageable>()?.TakeHit();
  }

  IEnumerator StartChameleon() {

    cam.cullingMask |= 1 << LayerMask.NameToLayer("Chameleon");
    photonView.RPC("OnChameleon", RpcTarget.All);
    yield return new WaitForSeconds(8);
    photonView.RPC("OffChameleon", RpcTarget.All);

  }

  [PunRPC]
  public void OnChameleon() {
    gameObject.layer = 15;
    foreach (Transform child in transform) {
      if (child.gameObject.name != "Minimap Indicator")
        child.gameObject.layer = 15;
    }
  }
  [PunRPC]
  public void OffChameleon() {
    gameObject.layer = 9;
    foreach (Transform child in transform) {
      if (child.gameObject.name != "Minimap Indicator")
        child.gameObject.layer = 9;
    }
  }



}

