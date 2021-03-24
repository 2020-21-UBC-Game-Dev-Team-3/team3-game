using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Pun;

public class MinigameManager : MonoBehaviour
{
    bool taskListOpen = true;
    GameObject taskList;
    int minigameMax = 3;

    PhotonView pv;
    PlayerManager localPM;

    string dartsText = "Take a break and play some darts (Arcade 3F)";
    string drinksText = "Mix yourself a cold and refreshing drink (Bar 2F)";
    string icebergsText = "Steer the ship away from icebergs (Navigation 3F)";
    string lifeboatText = "Help survivors onto the lifeboat (Lifeboats 1F)";
    string scavengerText = "Search for clues on the ship (Magnifying Glass 2F)";

    public List<string> assignedMinigames = new List<string>();
    public List<string> availableMinigames = new List<string>();
    public List<string> completedMinigames = new List<string>();
    List<TextMeshProUGUI> tasksText = new List<TextMeshProUGUI>();

    void Awake()
    {
        availableMinigames.AddRange(new string[] { "Darts minigame", "Drink mixing minigame", "Iceberg minigame", "Lifeboat minigame", "Scavenger hunt minigame" });
        pv = GetComponent<PhotonView>();
        localPM = PhotonView.Find((int)pv.InstantiationData[0]).GetComponent<PlayerManager>();
    }

    // Start is called before the first frame update
    void Start()
    {
        if (gameObject.tag == "Ghost")
        {
            RetrieveTasks();
            assignedMinigames.AddRange(completedMinigames);
            SetUpTaskList();
            foreach (string minigame in completedMinigames) UpdateTaskComplete(minigame);
        }
        else RandomizeMinigames();
    }

    void RetrieveTasks()
    {
        assignedMinigames = localPM.playerTasksRemaining;
        completedMinigames = localPM.playerTasksCompleted;
        if (completedMinigames.Contains("")) completedMinigames.Remove("");
    }

    void RandomizeMinigames()
    {
        for (int i = 0; i < minigameMax; i++)
        {
            int rng = Random.Range(0, availableMinigames.Count);
            assignedMinigames.Add(availableMinigames[rng]);
            availableMinigames.RemoveAt(rng);
        }

        localPM.playerTasksRemaining = assignedMinigames;
    }

    public void SetUpMinigameAssignment()
    {
        taskList = GameObject.Find("Task List");
        tasksText.AddRange(taskList.GetComponentsInChildren<TextMeshProUGUI>());
        SetUpTaskList();
    }

    void SetUpTaskList()
    {
        for (int i = 0; i < tasksText.Count; i++)
        {
            switch (assignedMinigames[i])
            {
                case "Darts minigame":
                    tasksText[i].text = dartsText;
                    break;

                case "Drink mixing minigame":
                    tasksText[i].text = drinksText;
                    break;

                case "Iceberg minigame":
                    tasksText[i].text = icebergsText;
                    break;

                case "Lifeboat minigame":
                    tasksText[i].text = lifeboatText;
                    break;

                case "Scavenger hunt minigame":
                    tasksText[i].text = scavengerText;
                    break;

                default:
                    break;
            }
        }
    }

    public bool IsAssignedMinigame(string minigame) => assignedMinigames.Contains(minigame);

    public void OnMinigameComplete(string minigame)
    {
        if (minigame != "none")
        {
            assignedMinigames.Remove(minigame);
            localPM.UpdateTasksOnPlayerManager(minigame);

            UpdateTaskComplete(minigame);
        }
    }


    void UpdateTaskComplete(string minigame)
    {
        string updatedTaskText;
        switch (minigame)
        {
            case "Darts minigame":
                updatedTaskText = "<s>" + dartsText + "<s>";
                for (int i = 0; i < tasksText.Count; i++)
                {
                    if (tasksText[i].text == dartsText) tasksText[i].text = updatedTaskText;
                }
                break;

            case "Drink mixing minigame":
                updatedTaskText = "<s>" + drinksText + "<s>";
                for (int i = 0; i < tasksText.Count; i++)
                {
                    if (tasksText[i].text == drinksText) tasksText[i].text = updatedTaskText;
                }
                break;

            case "Iceberg minigame":
                updatedTaskText = "<s>" + icebergsText + "<s>";
                for (int i = 0; i < tasksText.Count; i++)
                {
                    if (tasksText[i].text == icebergsText) tasksText[i].text = updatedTaskText;
                }
                break;

            case "Lifeboat minigame":
                updatedTaskText = "<s>" + lifeboatText + "<s>";
                for (int i = 0; i < tasksText.Count; i++)
                {
                    if (tasksText[i].text == lifeboatText) tasksText[i].text = updatedTaskText;
                }
                break;

            case "Scavenger hunt minigame":
                updatedTaskText = "<s>" + scavengerText + "<s>";
                for (int i = 0; i < tasksText.Count; i++)
                {
                    if (tasksText[i].text == scavengerText) tasksText[i].text = updatedTaskText;
                }
                break;

            default:
                break;
        }
    }

    void Update()
    {
        if (Input.GetKeyDown("t") && !taskListOpen) OpenTaskList();
        else if (Input.GetKeyDown("t") && taskListOpen) CloseTaskList();
    }

    public void OpenTaskList()
    {
        taskList.SetActive(true);
        taskListOpen = true;
    }

    public void CloseTaskList()
    {
        taskList.SetActive(false);
        taskListOpen = false;
    }
}
