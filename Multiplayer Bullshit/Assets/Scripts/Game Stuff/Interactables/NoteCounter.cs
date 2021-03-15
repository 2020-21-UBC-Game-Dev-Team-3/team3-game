using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NoteCounter : MonoBehaviour {

  public int count = 0;

  public void Update() {
    if (count == 9) {
      Debug.Log("you won");
      SceneManager.UnloadSceneAsync("Rhythm Trap Minigame");
    }
  }
}
