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
    if (pac == null) { 
        pac = GameObject.Find("player2(Clone)").GetComponent<PlayerActionController>();
    }
    //if (Input.GetKeyDown(KeyCode.T)) {
    //  Increment();
    //}
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
    if (count < 10) {
      count++;
      slider.value = count;
      taskProgress.text = "Tasks Completed: " + count.ToString() + "/" + "10";
    }
  }
}
