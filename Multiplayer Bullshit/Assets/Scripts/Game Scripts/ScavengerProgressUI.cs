using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScavengerProgressUI : MonoBehaviour {

  [SerializeField] TextMeshProUGUI scavengerTextProgress;
    [SerializeField] TextMeshProUGUI scavengerRewardText;
    [SerializeField] GameObject scavengerRewardImage;

  public int itemsFoundText;

  public void StartCounter() {
    itemsFoundText = 0;
    scavengerTextProgress.text = "Items Found: " + itemsFoundText.ToString() + "/3";
  }

  public void IncrementItemsFoundText() {
    itemsFoundText++;
    scavengerTextProgress.text = "Items Found: " + itemsFoundText.ToString() + "/3";
  }

  public void DisplayComplete() {
    scavengerTextProgress.text = "TASK COMPLETE";
  }

    public void DisplayImposterFloorNumber(int x)
    {
        scavengerRewardImage.SetActive(true);
        scavengerRewardText.text = x.ToString();
    }

    public void TurnOffScavengerRewardImage() => scavengerRewardImage.SetActive(false);
}
