using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.Rendering;
using HutongGames.PlayMaker;


public class crewmateLightsOut : MonoBehaviour
{
    public Volume volume;
    public double vignettevalue = 0.2;
    public double chromaticaberrationvalue = 0;
    public float gainvalue = .2f;
    public Vignette vignette;
    public ChromaticAberration chromaticaberration;
    public PlayMakerFSM myFSM;
    public LiftGammaGain liftgammagain;
    
   
    // Start is called before the first frame update
    void Start()
    {
        volume.profile.TryGet<Vignette>(out vignette);
        volume.profile.TryGet<ChromaticAberration>(out chromaticaberration);
        volume.profile.TryGet<LiftGammaGain>(out liftgammagain);
    }

    void ResetLights()
    {
        vignettevalue = 0.2;
        chromaticaberrationvalue = 0;
        gainvalue = .2f;
    }


    void Update()
    {
        liftgammagain.gain.value = new Vector3(gainvalue, gainvalue, gainvalue);

     
        vignette.intensity.value = (float)(vignettevalue += .20 * Time.deltaTime);

        if (vignettevalue >= .8)
        {
            vignettevalue = .8;
        }

       chromaticaberration.intensity.value = (float)(chromaticaberrationvalue += .20 * Time.deltaTime);

        if (chromaticaberrationvalue >= .99)
        {
            chromaticaberrationvalue = .99;
        }



    }
}
