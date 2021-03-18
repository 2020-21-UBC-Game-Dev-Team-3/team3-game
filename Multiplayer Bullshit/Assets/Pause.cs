using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pause : MonoBehaviour
{
   public GameObject Canvas;
    public Camera Camera;
    bool Paused = false;
 
    void Start(){
        Canvas.gameObject.SetActive (false);
     }
 
    void Update () {
        if (Input.GetKeyDown ("escape")) {
            if(Paused == true){
                Time.timeScale = 1.0f;
                Canvas.gameObject.SetActive (false);
                Cursor.visible = false;
                Cursor.lockState =  CursorLockMode.Confined;
                //Camera.audio.Play ();
                Paused = false;
             } else {
                Time.timeScale = 0.0f;
                Canvas.gameObject.SetActive (true);
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.Locked;
                //Camera.audio.Pause ();
                Paused = true;
             }
         }
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
