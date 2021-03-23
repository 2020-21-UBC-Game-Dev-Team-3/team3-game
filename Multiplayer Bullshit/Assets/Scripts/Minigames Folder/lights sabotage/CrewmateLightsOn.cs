using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.Rendering;
using HutongGames.PlayMaker;

public class CrewmateLightsOn : MonoBehaviour
{
    public Volume volume;
    //public double exposureValue = -1.5;
    public double vignettevalue = 0.8;
    public Vector4 gainvalue = new Vector4(.3f, .3f, .3f, .3f);
    public double chromaticaberrationvalue = .99;
    //ColorAdjustments coloradjustments;
    public Vignette vignette;
    public ChromaticAberration chromaticaberration;
    public LiftGammaGain liftgammagain;

    // Start is called before the first frame update
    void Start()
    {
        //exposureValue = -1.5;
        vignettevalue = 0.8;
        chromaticaberrationvalue = .99;
        liftgammagain.gain.value = gainvalue;
        //volume.profile.TryGet<ColorAdjustments>(out coloradjustments);
        volume.profile.TryGet<Vignette>(out vignette);
        volume.profile.TryGet<ChromaticAberration>(out chromaticaberration);
        volume.profile.TryGet<LiftGammaGain>(out liftgammagain);

    }

    void ResetLights2()
    {
        //exposureValue = -1.5;

        vignettevalue = 0.8;
        chromaticaberrationvalue = .99;
        liftgammagain.gain.value = gainvalue;
    }

    void Update()
    {

        liftgammagain.gain.value += new Vector4(0.3f, 0.3f, 0.3f, 0.3f) * Time.deltaTime;

        if (liftgammagain.gain.value.magnitude >= new Vector4(1f, 1f, 1f, 1f).magnitude)
        {
            liftgammagain.gain.value = new Vector4(1f, 1f, 1f, 1f);
        }

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

