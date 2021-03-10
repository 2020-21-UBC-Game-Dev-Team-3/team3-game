using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.Rendering;
using HutongGames.PlayMaker;

public class CrewmateLightsOn : MonoBehaviour
{
    public Volume volume;
    public double exposureValue = -1.5;
    public double vignettevalue = 0.55;
    ColorAdjustments coloradjustments;
    Vignette vignette;

    // Start is called before the first frame update
    void Start()
    {
        exposureValue = -1.5;
        vignettevalue = 0.55;
        volume.profile.TryGet<ColorAdjustments>(out coloradjustments);
        volume.profile.TryGet<Vignette>(out vignette);

    }

    void ResetLights2()
    {
        exposureValue = -1.5;
        vignettevalue = 0.55;
    }

    void Update()
    {

        coloradjustments.postExposure.value = (float)(exposureValue += 2.00 * Time.deltaTime);

        if (exposureValue >= .05)
        {
            exposureValue = .05;
        }

        vignette.intensity.value = (float)(vignettevalue -= .20 * Time.deltaTime);

        if (vignettevalue <= .2)
        {
            vignettevalue = .2;
        }




    }
}
