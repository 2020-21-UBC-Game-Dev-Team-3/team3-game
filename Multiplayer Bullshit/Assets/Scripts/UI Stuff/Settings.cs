using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class Settings : MonoBehaviour
{
    public Volume volume;
//    public Slider BloomIntensitySlider;
    // public Slider LGGSlider;
    // public float LGGValue;
    // public Dropdown shadows;

    private void Update(){
        ChangeBloomIntensitySettings();
    }

    public void ChangeBloomIntensitySettings()
    {
        GameObject gameObject = GameObject.Find("Post Processing");
        volume = gameObject.GetComponent<Volume>();
        Bloom bloom;
        volume.profile.TryGet(out bloom);
        Debug.Log( bloom.intensity.value);
        bloom.intensity.value = OptionsPP.bloomValue;

   }


    // public void ChangeLggSettings(){
    //     GameObject gameObject = GameObject.Find("Volume");
    //     volume = gameObject.GetComponent<Volume>();
    //     ColorAdjustments ca;
    //     volume.profile.TryGet(out ca);
    //     ca.postExposure.value = LGGValue;

    // }

} 
