using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviourPunCallbacks {

  [SerializeField] Transform[] teleportLocations = new Transform[10]; // assuming this is the max players in a game

  public void TeleportPlayers() {
    GameObject[] alivePlayers = GameObject.FindGameObjectsWithTag("Player");
    for (int i = 0; i < alivePlayers.Length; i++) {
      alivePlayers[i].gameObject.transform.position = teleportLocations[i].transform.position;
    }
  }

  /*  [HideInInspector] public bool isConnectedToMaster;
    // Start is called before the first frame update

    public IEnumerator EndGame() {
      yield return new WaitForSeconds(20f);
      *//* PhotonNetwork.LeaveRoom();*//* //FOR TESTING
    }

    private void OnLevelWasLoaded(int level) {
      if (SceneManager.GetActiveScene().name == "Gaming")
        StartCoroutine("EndGame");
    }

    public override void OnLeftRoom() {
      if (SceneManager.GetActiveScene().name == "Gaming") {
        PhotonNetwork.Disconnect();
        PhotonNetwork.LoadLevel(0);
      }
    }*/
}
