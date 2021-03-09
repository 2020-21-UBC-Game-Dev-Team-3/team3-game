using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SwitchCount : MonoBehaviour {

  [SerializeField] int currentCount;
  [SerializeField] TextMeshProUGUI textMeshPro;


  private void Start() {
    textMeshPro = GetComponent<TextMeshProUGUI>();
    currentCount = 0;
  }

  private void Update() {
    textMeshPro.text = currentCount.ToString() + "/3";
  }

  public void Increment() {
    currentCount++;
  }
}
