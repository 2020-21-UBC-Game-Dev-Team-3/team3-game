using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Pun;
using UnityEngine.UI;

public class TaskBar : MonoBehaviour {

  [SerializeField] TextMeshProUGUI taskProgress;
  [SerializeField] Slider slider;
  private int count;
  public int totalNumOfTasks;
  PhotonView pv;
  PlayerActionController playerPac;
  PlayerActionController ghostPac;
  public bool ghostFound = false;
  public bool playerFound = false;

  private void Awake() {
    pv = GetComponent<PhotonView>();
  }

  // Start is called before the first frame update
  void Start() {
    Debug.Log(taskProgress);
    taskProgress.text = "Tasks Completed: " + count.ToString() + "/" + "10";
  }

  private void Update() {

    /*    // OLD
        if (GameObject.Find("GhostPlayer(Clone)") != null) {
          ghostPac = GameObject.Find("GhostPlayer(Clone)").GetComponent<PlayerActionController>();
        } else {
          playerPac = GameObject.Find("player2(Clone)").GetComponent<PlayerActionController>();
        }
    */
    /*    if (!playerFound) {
          Debug.Log("finding player...");
          playerPac = GameObject.Find("player2(Clone)").GetComponent<PlayerActionController>();
          playerFound = true;
        }

        if (!ghostFound && playerPac == null) {
          if (GameObject.Find("GhostPlayer(Clone)").GetComponent<PhotonView>().IsMine) {
            Debug.Log("finding ghost...");
            ghostPac = GameObject.Find("GhostPlayer(Clone)").GetComponent<PlayerActionController>();
            ghostFound = true;
          }
        }*/

    slider.maxValue = totalNumOfTasks;
  }

  public void IncrementTaskBar() {
    StartCoroutine(UpdateText());
/*    playerPac.tbIHolder = false;
    if (ghostPac != null) ghostPac.tbIHolder = false;*/
  }

  IEnumerator UpdateText() {
    pv.RPC("UpdateTextBox", RpcTarget.All);
    yield return new WaitForSeconds(2);
  }

  [PunRPC]
  void UpdateTextBox() {
    count++;
    if (count < totalNumOfTasks) {
      slider.value = count;
      taskProgress.text = "Tasks Completed: " + count.ToString() + "/" + totalNumOfTasks.ToString();
    } else {
      slider.value = count;
      taskProgress.text = "Tasks Completed: " + count.ToString() + "/" + totalNumOfTasks.ToString();
      FindObjectOfType<GameManager>().CrewmateWin();
    }
  }
}
