using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundManager : MonoBehaviour
{
    // Start is called before the first frame update
    public AudioSource[] sources;
    public Slider MainVolumeSlider;
    public Slider MusicSlider;
    public Slider SFXslider;
    void Start()
    {
        if(!PlayerPrefs.HasKey("main volume"))
        {
            PlayerPrefs.SetFloat("main volume", 1);
        }
        if (!PlayerPrefs.HasKey("music volume"))
        {
            PlayerPrefs.SetFloat("music volume", 1);
        }
        if (!PlayerPrefs.HasKey("sfx volume"))
        {
            PlayerPrefs.SetFloat("sfx volume", 1);
        }
        PlayerPrefs.Save();
        sources = Object.FindObjectsOfType(typeof(AudioSource)) as AudioSource[];
        MainVolumeSlider.onValueChanged.AddListener(delegate { MainVolumeControl(); });
        StartCoroutine(LateStart(0.1f));


    }
    public void MainVolumeControl()
    {
        PlayerPrefs.SetFloat("main volume", MainVolumeSlider.value);
        foreach (AudioSource a in sources)
        {
            a.volume = PlayerPrefs.GetFloat("main volume");
        }
        PlayerPrefs.Save();
    }
    public void Update()
    {
        sources = Object.FindObjectsOfType(typeof(AudioSource)) as AudioSource[];

    }
    IEnumerator LateStart(float waitTime)
    {
        MainVolumeSlider.value = PlayerPrefs.GetFloat("main volume");
        PlayerPrefs.SetFloat("main volume", MainVolumeSlider.value);
        yield return new WaitForSeconds(waitTime);
        foreach (AudioSource a in sources)
        {
            a.volume = PlayerPrefs.GetFloat("main volume");
        }
        PlayerPrefs.Save();
    }
}
