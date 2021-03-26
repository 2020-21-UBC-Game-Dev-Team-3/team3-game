﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;

public class Trap : Interactable {

  private float timeBeforeActive = 2f;
    public AudioSource audioSource;
  [SerializeField] MeshRenderer needle;
  [SerializeField] MeshRenderer needleBase;

  [SerializeField] Material normal;
  [SerializeField] Material transparent;
  [SerializeField] GameObject indicator;

  [SerializeField] PhotonView pv;
  [SerializeField] PlayerActionController pac;

  public float timeBeforeDespawn = 1f;

/*    private void Awake()
    {
    audioSource.volume = PlayerPrefs.GetFloat("main volume");
    audioSource.Play();
  }*/

    private void Start() {
    audioSource.volume = PlayerPrefs.GetFloat("main volume");
    audioSource.Play();
    pv = GetComponent<PhotonView>();
    needle.material = transparent;
    needleBase.material = transparent;
    GetComponent<Collider>().enabled = false; // disables the collider
    StartCoroutine(CountdownBeforeActive());
    pac = GameObject.Find("player2(Clone)").GetComponent<PlayerActionController>();
    StartCoroutine(Despawn());
  }

  void Update() {
    if (outline.enabled) {
      indicator.SetActive(true);
    } else {
      indicator.SetActive(false);
    }
  }

  IEnumerator CountdownBeforeActive() {
    yield return new WaitForSeconds(timeBeforeActive);
    GetComponent<Collider>().enabled = true; // enables the collider
    needle.material = normal;
    needleBase.material = normal;
  }
    [PunRPC]
    void PlaytheAudio()
    {
        audioSource.Play();
    }

  private void OnTriggerEnter(Collider other) {

    // copy-paste from Interactable OnTriggerEnter()
    if (other.CompareTag("Player") && other.gameObject.GetComponent<PlayerActionController>().pv.IsMine) {
      //indicator.SetActive(true);
      outline.enabled = true;
    }

    // if other is a crewmate and not a Disarmer
    if (other.GetComponent<Role>().currRole == Role.Roles.Crewmate && !(other.GetComponent<Role>().subRole == Role.Roles.Disarmer)) {
            pv.RPC("PlaytheAudio", RpcTarget.All);
            Destroy(); // needs to be here so other people don't step on it
      if (SceneManager.sceneCount == 1) {
        pac.currMinigameSceneName = "Trap Chance Minigame";
        pac.exitMinigame(false);
        SceneManager.LoadScene("Trap Chance Minigame", LoadSceneMode.Additive);
      }
    }
  }

  IEnumerator Despawn() {
    yield return new WaitForSeconds(timeBeforeDespawn);
    Destroy();
  }
  // Destroys the trap
  public void Destroy() {
    pv.RPC("DestroyObject", RpcTarget.All);
  }

  [PunRPC]
  private void DestroyObject() {
    FindObjectOfType<TrapAbility>().DecrementTraps();
    Destroy(gameObject);
  }
}
