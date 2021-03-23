using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.Rendering;
using HutongGames.PlayMaker;

public class CrewmateLightsOn : MonoBehaviour
{
    public Volume volume;
    public double vignettevalue = 0.8;
    public double chromaticaberrationvalue = .99;
    public Vignette vignette;
    public ChromaticAberration chromaticaberration;


    // Start is called before the first frame update
    void Start()
    {
        vignettevalue = 0.8;
        chromaticaberrationvalue = .99;
        volume.profile.TryGet<Vignette>(out vignette);
        volume.profile.TryGet<ChromaticAberration>(out chromaticaberration);
    }

    void ResetLights2()
    {
        vignettevalue = 0.8;
        chromaticaberrationvalue = .99;
    }

    void Update()
    {


        chromaticaberration.intensity.value = (float)(chromaticaberrationvalue -= .20 * Time.deltaTime);

        if (chromaticaberrationvalue <= .1)
        {
            chromaticaberrationvalue = .0;
        }

        vignette.intensity.value = (float)(vignettevalue -= .20 * Time.deltaTime);

        if (vignettevalue <= .2)
        {
            vignettevalue = .2;
        }
    }



    }

