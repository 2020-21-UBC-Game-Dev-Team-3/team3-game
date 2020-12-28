using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Switch : MonoBehaviour {

  [SerializeField] private string selectedTag;

  // Start is called before the first frame update
  void Start() {

  }

  // Update is called once per frame
  void Update() {
    CheckRay();
  }

  private void CheckRay() {
    if (Input.GetMouseButtonDown(0)) {
      var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
      RaycastHit hit;
      if (Physics.Raycast(ray, out hit)) {
        if (hit.collider.tag == selectedTag) {
          Debug.Log("Switch!");
        }
      }
    }
  }

}
