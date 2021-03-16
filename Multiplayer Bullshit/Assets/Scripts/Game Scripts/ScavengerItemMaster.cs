using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using System.IO;
using UnityEngine;

public class ScavengerItemMaster : MonoBehaviour {

  private int itemsFound;

  // Start is called before the first frame update
  void Start() {
    List<int> intList = new List<int>();
    int tempInt;
    for (int i = 0; i < 3; i++) {
      do {
        tempInt = Random.Range(1, 7);
      } while (intList.Contains(tempInt));
      intList.Add(tempInt);
    }

    for (int i = 0; i < intList.Count; i++) {
    }
  }

  public void CollectItem() {
    if (itemsFound < 3) {
      itemsFound++;
    } else {
      FindObjectOfType<TaskBar>().IncrementTaskBar();
    }
  }
}
