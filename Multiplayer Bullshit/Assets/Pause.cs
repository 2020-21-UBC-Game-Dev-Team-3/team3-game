using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pause : MonoBehaviour
{
   public GameObject Canvas;
    public Camera Camera;
    bool Paused = false;
 
    void Start(){
        Canvas = GameObject.Find("Pause");
        Canvas.gameObject.SetActive (false);
        //Camera = Camera.main;
     }
 
    void Update () {
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
        Canvas.gameObject.SetActive(true);
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Locked;
        //Camera.audio.Pause ();
        Paused = true;
    }    
    
    void ClosePauseMenu()
    {
        Time.timeScale = 1.0f;
        Canvas.gameObject.SetActive(false);
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Confined;
        //Camera.audio.Play ();
        Paused = false;
    }

     public void Resume(){
        Time.timeScale = 1.0f;
        Canvas.gameObject.SetActive (false);
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        // Camera.audio.Play ();
     }
     public void QuitGame(){
         Application.Quit();
     } 
}
