﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScavengerHuntStarter : Interactable {

  [SerializeField] Transform[] transforms = new Transform[7];
  [SerializeField] ScavengerProgressUI scavengerProgressUI;
  List<int> intList = new List<int>();
  private int itemsFound;
  private int imposterFloorNumber;

  public void CollectItem() {
    IncrementItemsFound();
    scavengerProgressUI.IncrementItemsFoundText();
    if (itemsFound == 3) {
      CompleteTask();
    }
  }

  private void CompleteTask() {
    // There are 4 things that need to happen:
    // 1) itemsFound needs to be reset to 0 and
    // 2) Taskbar needs to be incremented and
    // 3) scavengerProgessUI needs to display "TASK COMPLETE" before
    // 4) scavengerProgressUI needs to be deactivated

    intList.Clear();

    StartCoroutine(CompleteTaskCoroutine());
  }

  IEnumerator CompleteTaskCoroutine() {
    itemsFound = 0;
    FindObjectOfType<TaskBar>().IncrementTaskBar();
    scavengerProgressUI.DisplayComplete();
    yield return new WaitForSeconds(2);
    FindImposterFloorNumber();
    scavengerProgressUI.DisplayImposterFloorNumber(imposterFloorNumber);
    yield return new WaitForSeconds(2);
    scavengerProgressUI.gameObject.SetActive(false);
  }

  void IncrementItemsFound() {
    itemsFound++;
  }

  public void ActivateScavengerHunt() {

    scavengerProgressUI.StartCounter();
    scavengerProgressUI.gameObject.SetActive(true);

    int tempInt;
    for (int i = 0; i < 3; i++) {
      do {
        tempInt = Random.Range(1, 7);
      } while (intList.Contains(tempInt));
      intList.Add(tempInt);
    }

    Debug.Log(intList[0].ToString() + " " + intList[1].ToString() + " " + intList[2].ToString());

    for (int i = 0; i < intList.Count; i++) {
      transforms[intList[i]].gameObject.SetActive(true);
    }
  }

    void FindImposterFloorNumber()
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");

        for (int i = 0; i < players.Length; i++)
        {
            if (players[i].GetComponent<Role>().currRole == Role.Roles.Imposter)
            {
                switch (Mathf.Round(players[i].transform.position.y))
                {
                    case 7f:
                        imposterFloorNumber = 3;
                        break;

                    case 4f:
                        imposterFloorNumber = 2;
                        break;

                    case 0f:
                        imposterFloorNumber = 1;
                        break;

                    default:
                        Debug.Log("false");
                        break;
                }

            }
        }
    }


}
