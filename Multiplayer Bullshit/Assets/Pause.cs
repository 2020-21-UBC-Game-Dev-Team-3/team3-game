using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pause : MonoBehaviour
{
    private GameObject Canvas;
    private Camera Camera;
    bool Paused = false;
 
    void Start(){

        foreach (GameObject go in Resources.FindObjectsOfTypeAll(typeof(GameObject)) as GameObject[])
        {
            if (go.name == "Pause")
            {
                Canvas = go;
            }
        }
        Canvas.SetActive (false);
        //Camera = Camera.main;
     }
 
    void Update () {
        //Debug.Log(Canvas == null);
        if (Input.GetKeyDown ("escape")) {
            if(Paused){
                ClosePauseMenu();
             } else if(!Paused) {
                OpenPauseMenu();
             }
         }
     }


    void OpenPauseMenu()
    {
        Time.timeScale = 0.0f;
        Canvas.SetActive(true);
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Locked;
        //Camera.audio.Pause ();
        Paused = true;
    }    
    
    void ClosePauseMenu()
    {
        Time.timeScale = 1.0f;
        Canvas.SetActive(false);
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Confined;
        //Camera.audio.Play ();
        Paused = false;
    }

     public void Resume(){
        Time.timeScale = 1.0f;
        Canvas.SetActive (false);
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        // Camera.audio.Play ();
     }
     public void QuitGame(){
         Application.Quit();
     } 
}
