using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pause : MonoBehaviour
{
    public GameObject Canvas;
    public GameObject Camera;
    bool Paused = false;
 
    void Start(){
        Canvas.gameObject.SetActive (false);
     }
 
    void Update () {
        if (Input.GetKeyDown (KeyCode.Escape)) {
            Debug.Log("escape pressed");
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
}
