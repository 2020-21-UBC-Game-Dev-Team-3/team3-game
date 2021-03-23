using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.Rendering;
using HutongGames.PlayMaker;


public class crewmateLightsOut : MonoBehaviour
{
    public Volume volume;
    //public double exposureValue = .05;
    public double vignettevalue = 0.15;
    public Vector4 gainvalue = new Vector4(1,1,1,1);
    public double chromaticaberrationvalue = 0;
    //ColorAdjustments coloradjustments;
    public Vignette vignette;
    public ChromaticAberration chromaticaberration;
    public LiftGammaGain liftgammagain;
    public PlayMakerFSM myFSM;
    
   
    // Start is called before the first frame update
    void Start()
    {

        //volume.profile.TryGet<ColorAdjustments>(out coloradjustments);
        volume.profile.TryGet<Vignette>(out vignette);
        volume.profile.TryGet<ChromaticAberration>(out chromaticaberration);
        volume.profile.TryGet<LiftGammaGain>(out liftgammagain);

    }

    void ResetLights()
    {

        liftgammagain.gain.value = gainvalue; 
        vignettevalue = 0.15;
        chromaticaberrationvalue = 0;

    }


    void Update()
    {

        liftgammagain.gain.value -= new Vector4(0.3f, 0.3f, 0.3f, 0.3f) * Time.deltaTime;

        if (liftgammagain.gain.value.magnitude <= new Vector4(0.2f, 0.2f, 0.2f, 0.2f).magnitude)
        {
            liftgammagain.gain.value = new Vector4(0.2f, 0.2f, 0.2f, 0.2f);
        }

       vignette.intensity.value = (float)(vignettevalue += .20 * Time.deltaTime);

        if (vignettevalue >= .55)
        {
            vignettevalue = .55;
        }

       chromaticaberration.intensity.value = (float)(chromaticaberrationvalue += .20 * Time.deltaTime);

        if (chromaticaberrationvalue >= .99)
        {
            vignettevalue = .99;
        }



    }
}
