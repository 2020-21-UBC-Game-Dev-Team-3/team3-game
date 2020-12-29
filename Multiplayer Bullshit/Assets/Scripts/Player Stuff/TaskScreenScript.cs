using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaskScreenScript : MonoBehaviour
{
    public GameObject minigameCanvas;
    //public GameObject taskCanvas;

    // Start is called before the first frame update
    void Start()
    {
        minigameCanvas = GameObject.FindGameObjectWithTag("MinigameCanvas");
        minigameCanvas.SetActive(false);
        //taskCanvas = GameObject.FindGameObjectWithTag("TaskCanvas");
        //taskCanvas.SetActive(true);

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /*public void ActivateMinigameScreen()
    {
        minigameCanvas.SetActive(true);
    }*/

    public void CloseMinigameScreen()
    {
        minigameCanvas.SetActive(false);
    }

    /*public void ActivateTaskCanvas() {
        taskCanvas.SetActive(true);
    }
    public void CloseTaskCanvas()
    {
        taskCanvas.SetActive(false);
    }*/
}
