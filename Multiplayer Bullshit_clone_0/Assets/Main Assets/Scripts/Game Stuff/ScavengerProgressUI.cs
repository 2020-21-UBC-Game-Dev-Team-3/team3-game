﻿using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScavengerProgressUI : MonoBehaviour {

  [SerializeField] TextMeshProUGUI scavengerTextProgress;

  public int itemsFoundText;

  public void StartCounter() {
    itemsFoundText = 0;
    scavengerTextProgress.text = "Items Found: " + itemsFoundText.ToString() + "/3";
  }

  public void Increment() {
    itemsFoundText++;
    scavengerTextProgress.text = "Items Found: " + itemsFoundText.ToString() + "/3";
  }

  public void DisplayComplete() {
    scavengerTextProgress.text = "TASK COMPLETE";
  }
}
