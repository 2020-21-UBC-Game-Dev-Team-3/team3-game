using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickManager : MonoBehaviour {

  [SerializeField] private string selectedTag;
  [SerializeField] Switch[] switchList;

  // Start is called before the first frame update
  void Start() {
    selectedTag = "Switch";
    switchList = FindObjectsOfType<Switch>();
  }

  // Update is called once per frame
  void Update() {
    CheckRay();
    CheckWin();
  }

  private void CheckRay() {
    if (Input.GetMouseButtonDown(0)) {
      var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
      RaycastHit hit;
      if (Physics.Raycast(ray, out hit)) {
        if (hit.collider.tag == selectedTag) {
          Debug.Log("Switch!");
          hit.collider.gameObject.GetComponent<Switch>().TurnOn();
        }
      }
    }
  }

  private void CheckWin() {
    if (AllOn()) {
      Debug.Log("You win!");
    }
  }

  private bool AllOn() {
    for (int i = 0; i < switchList.Length; i++) {
      if (!switchList[i].isOn) {
        return false;
      } 
    }
    return true;
  }

}
