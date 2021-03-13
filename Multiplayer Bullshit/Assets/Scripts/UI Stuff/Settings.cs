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
        ChangeBrightnessSettings();
    }

    public void ChangeBloomIntensitySettings()
    {
        GameObject gameObject = GameObject.Find("Post Processing");
        volume = gameObject.GetComponent<Volume>();
        Bloom bloom;
        volume.profile.TryGet(out bloom);
        bloom.intensity.value = OptionsPP.bloomValue;

   }


    public void ChangeBrightnessSettings(){
        GameObject gameObject = GameObject.Find("Post Processing");
        volume = gameObject.GetComponent<Volume>();
        ColorAdjustments ca;
        volume.profile.TryGet(out ca);
        ca.postExposure.value = OptionsPP.brightnessValue;

    }

    public void ChangeShadowSettings(){
        GameObject gameObject = GameObject.Find("Post Processing");
        volume = gameObject.GetComponent<Volume>();
        ShadowsMidtonesHighlights smh;
        volume.profile.TryGet(out smh);
        float shadows = OptionsPP.shadowsValue;
        Debug.Log(shadows);

        smh.shadows.value = new Vector3(shadows, shadows, shadows);

    }

} 
