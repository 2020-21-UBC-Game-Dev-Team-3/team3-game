using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Cinemachine.Utility;
using UnityEngine.Serialization;
using Cinemachine;

public class cameraOffset : MonoBehaviour
{

    GameObject camObj; //This is your camera with the free look component on it

    CinemachineFreeLook freeLook; // this reference the free look component in your camera

    //I named this variable comp for "Composer", you can name it however you like. This is the cinemachine component with all the aiming stuff on it
    CinemachineComposer comp;

    // Start is called before the first frame update
    void Start()
    {
        camObj = GameObject.FindWithTag("MainCamera");

        freeLook = camObj.GetComponent<CinemachineFreeLook>();

        comp = freeLook.GetRig(1).GetCinemachineComponent<CinemachineComposer>();

        // This is how you get to "Tracked Object Offset" of the rig as well as any other property in the rigs.
        comp.m_TrackedObjectOffset.x = 30;
    }
}