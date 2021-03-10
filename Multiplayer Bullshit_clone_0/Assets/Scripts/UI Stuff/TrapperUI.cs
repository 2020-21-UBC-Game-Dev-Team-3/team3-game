using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TrapperUI : MonoBehaviour {

  [SerializeField] TextMeshProUGUI trapperText;

  public void SetText(string s) {
    trapperText.text = s;
  }
}
