using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PianoInteract : MonoBehaviour
{
    public List<AudioClip> loa;
    public AudioSource audioSource;
    private int frames;
    public void PlaySound()
    {
        RandomClip();
        audioSource.Play();
    }
    public void RandomClip()
    {
        audioSource.clip = loa[Random.Range(0, loa.Count)];
    }
    public void Update()
    {
        frames++;
        if (frames % 300 == 0)
        {
            PlaySound();
            if(frames > 3000)
            {
                frames = 0;
            }
        }
    }
}


