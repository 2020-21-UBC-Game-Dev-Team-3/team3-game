using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.Rendering;
using HutongGames.PlayMaker;


public class crewmateLightsOut : MonoBehaviour
{
    public Volume volume;
    public double exposureValue = .05;
    public double vignettevalue = 0.15;
    ColorAdjustments coloradjustments;
    Vignette vignette;
    public PlayMakerFSM myFSM;
    
   
    // Start is called before the first frame update
    void Start()
    {

        volume.profile.TryGet<ColorAdjustments>(out coloradjustments);
        volume.profile.TryGet<Vignette>(out vignette);

    }

    void ResetLights()
    {
        exposureValue = .05;
        vignettevalue = 0.15;

    }


    void Update()
    {

        coloradjustments.postExposure.value = (float)(exposureValue -= 2.00 * Time.deltaTime);

        if (exposureValue <= -1.5)
        {
            exposureValue = -1.5;
        }

       vignette.intensity.value = (float)(vignettevalue += .20 * Time.deltaTime);

        if (vignettevalue >= .55)
        {
            vignettevalue = .55;
        }




    }
}
