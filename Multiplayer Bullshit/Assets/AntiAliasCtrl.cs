using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

public class AntiAliasCtrl : MonoBehaviour
{
    public Camera mainCam;
    public Dropdown dd;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        GameObject gameObject = GameObject.Find("Main Camera");
       // var cameraData = camera.GetUniversalAdditionalCameraData();

        //Camera.main.GetComponent<UniversalAdditionalCameraData>().antialiasing = AntialiasingMode.SubpixelMorphologicalAntiAliasing;
        //Debug.Log(cameraData);
        UnityEngine.Rendering.Universal.UniversalAdditionalCameraData uac = gameObject.GetComponent<Camera>().GetComponent<UnityEngine.Rendering.Universal.UniversalAdditionalCameraData>();

        
        if(dd.value == 0){
            uac.antialiasing = AntialiasingMode.None;   
        }else if(dd.value == 1){
            uac.antialiasing = AntialiasingMode.FastApproximateAntialiasing; 
        } else{
            uac.antialiasing = AntialiasingMode.SubpixelMorphologicalAntiAliasing;   
        }
         

        
    }
}
