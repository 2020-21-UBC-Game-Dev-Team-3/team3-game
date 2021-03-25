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
  PlayerActionController pac;

  private void Awake() {
    pv = GetComponent<PhotonView>();
  }

  // Start is called before the first frame update
  void Start() {
    Debug.Log(taskProgress);
    taskProgress.text = "Tasks Completed: " + count.ToString() + "/" + "10";
  }

  private void Update() {

        if (GameObject.Find("GhostPlayer(Clone)") != null)
        {
            pac = GameObject.Find("GhostPlayer(Clone)").GetComponent<PlayerActionController>();
        } else
        {
            pac = GameObject.Find("player2(Clone)").GetComponent<PlayerActionController>();
        }

        slider.maxValue = totalNumOfTasks;
  }

  public void IncrementTaskBar() {
    StartCoroutine(UpdateText());
    pac.tbIHolder = false;
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
