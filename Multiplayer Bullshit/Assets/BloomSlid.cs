using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class BloomSlid : MonoBehaviour
{
    public Volume volume;
   public Slider BloomIntensitySlider;
    // public Slider LGGSlider;
    // public float LGGValue;
    // public Dropdown shadows;


    public void ChangeBloomIntensitySettings(float blomValue)
    {
        GameObject gameObject = GameObject.Find("Post Processing");
        volume = gameObject.GetComponent<Volume>();
        Bloom bloom;
        volume.profile.TryGet(out bloom);
        Debug.Log( bloom.intensity.value);
        bloom.intensity.value = blomValue;

    }
    // public void ChangeLggSettings(){
    //     GameObject gameObject = GameObject.Find("Volume");
    //     volume = gameObject.GetComponent<Volume>();
    //     ColorAdjustments ca;
    //     volume.profile.TryGet(out ca);
    //     ca.postExposure.value = LGGValue;

    // }
    // public void ChangeShadows(){
    //     GameObject gameObject = GameObject.Find("Volume");
    //     volume = gameObject.GetComponent<Volume>();
    //     ShadowsMidtonesHighlights smh;
    //     volume.profile.TryGet(out smh);
    //     if(shadows.value == 0){
    //         smh.shadows.value = new Vector3(1.0f,1.0f, 1.0f);
    //     }
    //     if(shadows.value == 1){
    //         smh.shadows.value = new Vector3(.70f,.70f,.70f);
    //     }else if(shadows.value == 2){
    //         smh.shadows.value = new Vector3(1.5f,1.5f, 1.5f);

    //     }else if(shadows.value == 3){
    //         smh.shadows.value = new Vector3(2.3f, 2.3f, 2.3f);
    //     }
        
    // }
} 
