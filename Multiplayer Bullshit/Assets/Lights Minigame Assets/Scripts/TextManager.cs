using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TextManager : MonoBehaviour {

  [SerializeField] SwitchCount switchCount;
  [SerializeField] WinText winText;

  // Start is called before the first frame update
  void Start() {
    switchCount = this.gameObject.transform.GetChild(0).GetComponent<SwitchCount>();
    winText = this.gameObject.transform.GetChild(1).GetComponent<WinText>();
  }

  public void Win() {
    winText.gameObject.SetActive(true);
    FindObjectOfType<LightFlicker>().FreezeTime();
        SceneManager.LoadScene(sceneName: "Gaming", LoadSceneMode.Single);
    }

  public void IncrementCount() {
    switchCount.Increment();
  }
}
