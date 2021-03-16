using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;

public class Trap : Interactable {

  private float timeBeforeActive = 2f;
  [SerializeField] MeshRenderer needle;
  [SerializeField] MeshRenderer needleBase;

  [SerializeField] Material normal;
  [SerializeField] Material transparent;

  [SerializeField] PhotonView pv;
  [SerializeField] PlayerActionController pac;
  
  private void Start() {
    pv = GetComponent<PhotonView>();
    needle.material = transparent;
    needleBase.material = transparent;
    GetComponent<Collider>().enabled = false; // disables the collider
    StartCoroutine(CountdownBeforeActive());
    pac = GameObject.Find("player2(Clone)").GetComponent<PlayerActionController>();
  }

  IEnumerator CountdownBeforeActive() {
    yield return new WaitForSeconds(timeBeforeActive);
    GetComponent<Collider>().enabled = true; // enables the collider
    needle.material = normal;
    needleBase.material = normal;
  }

  private void OnTriggerEnter(Collider other) {

    // copy-paste from Interactable OnTriggerEnter()
    if (other.CompareTag("Player") && other.gameObject.GetComponent<PlayerActionController>().pv.IsMine) {
      //indicator.SetActive(true);
      outline.enabled = true;
    }
    
    if (other.GetComponent<Role>().currRole == Role.Roles.Crewmate) {
      //other.GetComponent<PlayerActionController>().TakeHit();
      //Destroy();
      if (SceneManager.sceneCount == 1) { 
        pac.currMinigameSceneName = "Trap Chance Minigame"; 
        pac.exitMinigame(false);
        SceneManager.LoadScene("Trap Chance Minigame", LoadSceneMode.Additive);
      }
            
            Destroy();
    }
  }

  public void Destroy() {
        pv.RPC("DestroyObject", RpcTarget.All);
  }

  [PunRPC]
  private void DestroyObject() {
    Debug.Log("DESTROYING OBJECT");
        Destroy(this.gameObject);
  }
}
