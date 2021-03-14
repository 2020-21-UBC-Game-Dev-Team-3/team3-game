using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class Options : MonoBehaviour {


  [SerializeField] Slider bloomSlider;
  [SerializeField] Slider brightnessSlider;
  [SerializeField] Slider shadowsSlider;
  [SerializeField] Dropdown textureDropdown; 
  [SerializeField] Dropdown qualityDropdown;
  [SerializeField] Dropdown aaDropdown;
  [SerializeField] Dropdown resolutionDropdown;
  [SerializeField] Toggle fullscreenToggle;
  [SerializeField] Slider fovSlider;
  Resolution[] resolutions;

  
  public Volume volume;
  public Camera mainCam;

  private void Start() {
    resolutions = Screen.resolutions;
    resolutionDropdown.ClearOptions();
    List<string> options = new List<string>();
        
    int currentResolutionIndex = 0;
    for(int i = 0; i < resolutions.Length; i++){
        string option = resolutions[i].width + "x" + resolutions[i].height;
        options.Add(option);
        if(resolutions[i].width == Screen.currentResolution.width && resolutions[i].height == Screen.currentResolution.height){
            currentResolutionIndex = i;
        }
    }
    resolutionDropdown.AddOptions(options);
    resolutionDropdown.value = currentResolutionIndex;
    resolutionDropdown.RefreshShownValue();
    
   
    
    bloomSlider.value = OptionsPP.bloomValue;
    brightnessSlider.value = OptionsPP.brightnessValue;
    shadowsSlider.value = OptionsPP.shadowsValue;
    fovSlider.value = OptionsPP.fovValue;
    qualityDropdown.value = QualitySettings.GetQualityLevel();
    textureDropdown.value = 1;
    aaDropdown.value = 0;


  }

  private void Update() {
    OptionsPP.bloomValue = bloomSlider.value;
    OptionsPP.brightnessValue = brightnessSlider.value;
    OptionsPP.shadowsValue = shadowsSlider.value;
    OptionsPP.fovValue = fovSlider.value;
    AdjustBloom(bloomSlider.value);
    AdjustBrightness(brightnessSlider.value);
    AdjustShadows(shadowsSlider.value);
    ChangeFOV(fovSlider.value);

  }

  public void AdjustBloom(float blomValue) {
        GameObject gameObject = GameObject.Find("Post Processing");
        volume = gameObject.GetComponent<Volume>();
        Bloom bloom;
        volume.profile.TryGet(out bloom);
        bloom.intensity.value = blomValue;
  }
    public void AdjustBrightness(float LGGValue){

        GameObject gameObject = GameObject.Find("Post Processing");
        volume = gameObject.GetComponent<Volume>();
        ColorAdjustments ca;
        volume.profile.TryGet(out ca);
        ca.postExposure.value = LGGValue;


    }
    public void AdjustShadows(float shadowVal){
        GameObject gameObject = GameObject.Find("Post Processing");
        volume = gameObject.GetComponent<Volume>();
        ShadowsMidtonesHighlights smh;
        volume.profile.TryGet(out smh);
        smh.shadows.value = new Vector3(shadowVal, shadowVal, shadowVal);
        
    }
     public void SetFullscreen(bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
    }

    public void SetResolution(int resolutionIndex)
    {
        Resolution resolution = resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }

    public void SetTextureQuality(int textureIndex)
    {
        QualitySettings.masterTextureLimit = textureIndex;
        qualityDropdown.value = 6;
    }

    public void SetAntiAliasing(int aaIndex)
    {
        QualitySettings.antiAliasing = aaIndex;
        qualityDropdown.value = 6;
    }

    public void SetQuality(int qualityIndex)
    {
        if (qualityIndex != 6) // if the user is not using any of the presets
            QualitySettings.SetQualityLevel(qualityIndex);

        switch (qualityIndex)
        {
            case 0: // quality level - very low
                textureDropdown.value = 3;
                aaDropdown.value = 0;
                break;
            case 1: // quality level - low
                textureDropdown.value = 2;
                aaDropdown.value = 0;
                break;
            case 2: // quality level - medium
                textureDropdown.value = 1;
                aaDropdown.value = 0;
                break;
            case 3: // quality level - high
                textureDropdown.value = 0;
                aaDropdown.value = 0;
                break;
            case 4: // quality level - very high
                textureDropdown.value = 0;
                aaDropdown.value = 1;
                break;
            case 5: // quality level - ultra
                textureDropdown.value = 0;
                aaDropdown.value = 2;
                break;
        }
        
        qualityDropdown.value = qualityIndex;
    }

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
   
}
   //public Dropdown resolutionDropdown;
//     public Dropdown qualityDropdown;
//     public Dropdown textureDropdown;
//     public Dropdown aaDropdown;
//    // public Slider volumeSlider;

//     //float currentVolume;

//     // Start is called before the first frame update
//     void Start()
//     {
//         // resolutionDropdown.ClearOptions();
//         // List<string> options = new List<string>();
//         // resolutions = Screen.resolutions;
//         // int currentResolutionIndex = 0;

//         // for (int i = 0; i < resolutions.Length; i++)
//         // {
//         //     string option = resolutions[i].width + " x " + resolutions[i].height;
//         //     options.Add(option);

//         //     if (resolutions[i].width == Screen.currentResolution.width && resolutions[i].height == Screen.currentResolution.height)
//         //         currentResolutionIndex = i;
//         // }

//         // resolutionDropdown.AddOptions(options);
//         // resolutionDropdown.RefreshShownValue();
//         // LoadSettings(currentResolutionIndex);
//     }



//     // public void SetFullscreen(bool isFullscreen)
//     // {
//     //     Screen.fullScreen = isFullscreen;
//     // }

//     // public void SetResolution(int resolutionIndex)
//     // {
//     //     Resolution resolution = resolutions[resolutionIndex];
//     //     Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
//     // }

//     public void SetTextureQuality(int textureIndex)
//     {
//         QualitySettings.masterTextureLimit = textureIndex;
//         qualityDropdown.value = 6;
//     }

//     public void SetAntiAliasing(int aaIndex)
//     {
//         QualitySettings.antiAliasing = aaIndex;
//         qualityDropdown.value = 6;
//     }

//     public void SetQuality(int qualityIndex)
//     {
//         if (qualityIndex != 6) // if the user is not using any of the presets
//             QualitySettings.SetQualityLevel(qualityIndex);

//         switch (qualityIndex)
//         {
//             case 0: // quality level - very low
//                 textureDropdown.value = 3;
//                 aaDropdown.value = 0;
//                 break;
//             case 1: // quality level - low
//                 textureDropdown.value = 2;
//                 aaDropdown.value = 0;
//                 break;
//             case 2: // quality level - medium
//                 textureDropdown.value = 1;
//                 aaDropdown.value = 0;
//                 break;
//             case 3: // quality level - high
//                 textureDropdown.value = 0;
//                 aaDropdown.value = 0;
//                 break;
//             case 4: // quality level - very high
//                 textureDropdown.value = 0;
//                 aaDropdown.value = 1;
//                 break;
//             case 5: // quality level - ultra
//                 textureDropdown.value = 0;
//                 aaDropdown.value = 2;
//                 break;
//         }
        
//         qualityDropdown.value = qualityIndex;
//     }

//     public void ExitGame()
//     {
//         Application.Quit();
//     }

//     public void SaveSettings()
//     {
//         PlayerPrefs.SetInt("QualitySettingPreference", qualityDropdown.value);
//     //     PlayerPrefs.SetInt("ResolutionPreference", resolutionDropdown.value);
//         PlayerPrefs.SetInt("TextureQualityPreference", textureDropdown.value);
//         PlayerPrefs.SetInt("AntiAliasingPreference", aaDropdown.value);
//     //     PlayerPrefs.SetInt("FullscreenPreference", Convert.ToInt32(Screen.fullScreen));
//     }

//     public void LoadSettings(int currentResolutionIndex)
//     {
//         if (PlayerPrefs.HasKey("QualitySettingPreference"))
//             qualityDropdown.value = PlayerPrefs.GetInt("QualitySettingPreference");
//         else
//             qualityDropdown.value = 3;

//     //     if (PlayerPrefs.HasKey("ResolutionPreference"))
//     //         resolutionDropdown.value = PlayerPrefs.GetInt("ResolutionPreference");
//     //     else
//     //         resolutionDropdown.value = currentResolutionIndex;

//     //     if (PlayerPrefs.HasKey("TextureQualityPreference"))
//     //         textureDropdown.value = PlayerPrefs.GetInt("TextureQualityPreference");
//     //     else
//     //         textureDropdown.value = 0;

//     //     if (PlayerPrefs.HasKey("AntiAliasingPreference"))
//     //         aaDropdown.value = PlayerPrefs.GetInt("AntiAliasingPreference");
//     //     else
//     //         aaDropdown.value = 0;

//     //     if (PlayerPrefs.HasKey("FullscreenPreference"))
//     //         Screen.fullScreen = Convert.ToBoolean(PlayerPrefs.GetInt("FullscreenPreference"));
//     //     else
//     //         Screen.fullScreen = true;

//     //     if (PlayerPrefs.HasKey("VolumePreference"))
//     //         volumeSlider.value = PlayerPrefs.GetFloat("VolumePreference");
//     //     else
//     //         volumeSlider.value = PlayerPrefs.GetFloat("VolumePreference");
//     }

// }
