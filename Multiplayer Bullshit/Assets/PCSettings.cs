using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;
public class PCSettings : MonoBehaviour
{
    public Camera mainCam;
    // Start is called before the first frame update

    public void ChangeFOV(float slideVal){
        GameObject gameObject = GameObject.Find("Main Camera");
        Camera proj = gameObject.GetComponent<Camera>();
        proj.fieldOfView = slideVal;

    }

    public void ChangeVSync(bool isOn){
        int valueOfVSync;
        if (isOn == true){
            valueOfVSync = 2;
        }else{
            valueOfVSync = 0; 
        }
        QualitySettings.vSyncCount = valueOfVSync;
    }

    public void InvertYAxis(bool invert){
        if(invert == true){
            float invertedAxis = -Input.GetAxis("Vertical");
        }
    }

    public void InvertXAxis(bool xInvert){
        if(xInvert == true){
            float invertedAxis = -Input.GetAxis("Horizontal");
        }
    }
  

    // Update is called once per frame
    
}
