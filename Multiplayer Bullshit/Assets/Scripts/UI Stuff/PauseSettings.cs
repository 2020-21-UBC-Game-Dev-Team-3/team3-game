using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;


public class PauseSettings : MonoBehaviour
{
    public Slider brightnessSlider;
    public Slider shadowsSlider;
    public Slider bloomSlider;
    public Dropdown QualityDD;
    public Toggle fullscreenToggle;
    public Volume volume;
    // Start is called before the first frame update
    void Start()
    {
        brightnessSlider.value = OptionsPP.brightnessValue;
        shadowsSlider.value = OptionsPP.shadowsValue;
        bloomSlider.value = OptionsPP.bloomValue;
        QualityDD.value = OptionsPP.qualityValue;
        fullscreenToggle.isOn  = OptionsPP.fullScreen;
    }


    public void ChangeBrightnessSettings(float brightness){


        GameObject gameObject = GameObject.Find("Post Processing");
        volume = gameObject.GetComponent<Volume>();
        ColorAdjustments ca;
        volume.profile.TryGet(out ca);
        ca.postExposure.value = brightness;


    
       OptionsPP.brightnessValue = brightness;
    }

    public void AdjustBloom(float blomValue) {
        GameObject gameObject = GameObject.Find("Post Processing");
        volume = gameObject.GetComponent<Volume>();
        Bloom bloom;
        volume.profile.TryGet(out bloom);
        bloom.intensity.value = blomValue;

        OptionsPP.bloomValue = blomValue;
  }

    public void AdjustShadows(float shadowVal){
        GameObject gameObject = GameObject.Find("Post Processing");
        volume = gameObject.GetComponent<Volume>();
        ShadowsMidtonesHighlights smh;
        volume.profile.TryGet(out smh);
        smh.shadows.value = new Vector3(shadowVal, shadowVal, shadowVal);
        OptionsPP.shadowsValue = shadowVal;
        
    }
     public void SetFullscreen(bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
        
    }
    public void SetQuality(int qualityIndex)
    {
        QualitySettings.SetQualityLevel(qualityIndex);

    }
}
