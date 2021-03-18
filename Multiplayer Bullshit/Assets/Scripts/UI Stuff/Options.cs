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
  [SerializeField] Toggle xToggle;
  [SerializeField] Toggle yToggle;
  [SerializeField] Toggle vsyncToggle;
  Resolution[] resolutions;
  bool saved = false;
  
  public Volume volume;
  public Camera mainCam;
  int currentResolutionIndex = 0;

  private void Start() {
    resolutions = Screen.resolutions;
    resolutionDropdown.ClearOptions();
    List<string> options = new List<string>();
        
    
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
    
   
    
    setOriginalValues();
    

    


  }

  private void Update() {
      
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

    public void SaveChanges(){
        OptionsPP.bloomValue = bloomSlider.value;
        OptionsPP.brightnessValue = brightnessSlider.value;
        OptionsPP.shadowsValue = shadowsSlider.value;
        OptionsPP.fovValue = fovSlider.value;
        OptionsPP.qualityValue = qualityDropdown.value;
        OptionsPP.fullScreen = fullscreenToggle.isOn;
        OptionsPP.textureValue = textureDropdown.value;
        OptionsPP.aaValue = aaDropdown.value;
        OptionsPP.xValue = xToggle.isOn;
        OptionsPP.yValue = yToggle.isOn;
        OptionsPP.vsyncVal = vsyncToggle.isOn;
        
        saved = true; 
    }

    public void CheckSaved(){
        if(saved == false){
          setOriginalValues();
      }
      saved = false;

    }

    private void setOriginalValues(){
        bloomSlider.value = OptionsPP.bloomValue;
        brightnessSlider.value = OptionsPP.brightnessValue;
        shadowsSlider.value = OptionsPP.shadowsValue;
        fovSlider.value = OptionsPP.fovValue;
        resolutionDropdown.value = currentResolutionIndex;
        qualityDropdown.value = OptionsPP.qualityValue;
        textureDropdown.value = OptionsPP.textureValue;
        aaDropdown.value = OptionsPP.aaValue;
        fullscreenToggle.isOn = OptionsPP.fullScreen;
        xToggle.isOn = OptionsPP.xValue;
        yToggle.isOn = OptionsPP.yValue;
        vsyncToggle.isOn = OptionsPP.vsyncVal;
    }
   
}
